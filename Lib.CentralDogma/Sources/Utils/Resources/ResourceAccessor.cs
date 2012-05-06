/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Hime.Utils.Resources
{
	// TODO: why is this class needed? Think about it
    class ResourceAccessor : IDisposable
	{
        private static List<ResourceAccessor> accessors = new List<ResourceAccessor>();

		private Assembly assembly;
		private string rootNamespace;
		private string defaultPath;
        private List<string> files;
        private List<Stream> streams;

        public ICollection<string> Files { get { return files; } }

        public ResourceAccessor()
        {
            accessors.Add(this);
            this.assembly = Assembly.GetExecutingAssembly();
            this.rootNamespace = assembly.GetName().Name;
			this.defaultPath = rootNamespace + ".Resources.";
            this.files = new List<string>();
            this.streams = new List<Stream>();
        }

        public void Dispose()
        {
            foreach (string file in files) File.Delete(file);
            foreach (Stream stream in streams) stream.Close();
            accessors.Remove(this);
        }

        public void AddCheckoutFile(string fileName)
        {
            files.Add(fileName);
        }

        public void CheckOut(string resourceName, string fileName)
        {
			Export(resourceName, fileName);
            files.Add(fileName);
        }
		
		private byte[] ReadResource(string resourceName)
		{
			using (Stream stream = GetStreamFor(resourceName))
			{
            	byte[] buffer = new byte[stream.Length];
            	stream.Read(buffer, 0, buffer.Length);
				return buffer;
			}
		}
		
        public void Export(string resourceName, string fileName)
        {
            byte[] buffer = this.ReadResource(resourceName);
            File.WriteAllBytes(fileName, buffer);
        }

        public Stream GetStreamFor(string resourceName)
        {
            Stream stream = this.assembly.GetManifestResourceStream(defaultPath + resourceName);
            if (stream == null) return null;
            streams.Add(stream);
            return stream;
        }

        public string GetAllTextFor(string resourceName)
		{
			byte[] buffer = ReadResource(resourceName);
            // Detect encoding and strip encoding preambule
            Encoding encoding = DetectEncoding(buffer);
            buffer = StripPreambule(buffer, encoding);
            // Return decoded text
			return new string(Encoding.UTF8.GetChars(buffer));
		}
        
        private static Encoding DetectEncoding(byte[] buffer)
        {
            if (DetectEncoding_TryEncoding(buffer, Encoding.UTF8))
                return Encoding.UTF8;
            if (DetectEncoding_TryEncoding(buffer, Encoding.Unicode))
                return Encoding.Unicode;
            if (DetectEncoding_TryEncoding(buffer, Encoding.BigEndianUnicode))
                return Encoding.BigEndianUnicode;
            if (DetectEncoding_TryEncoding(buffer, Encoding.UTF32))
                return Encoding.UTF32;
            if (DetectEncoding_TryEncoding(buffer, Encoding.ASCII))
                return Encoding.ASCII;
            return Encoding.Default;
        }
        private static bool DetectEncoding_TryEncoding(byte[] buffer, Encoding encoding)
        {
            byte[] preambule = encoding.GetPreamble();
            if (buffer.Length < preambule.Length) return false;
            for (int i = 0; i < preambule.Length; i++)
            {
                if (buffer[i] != preambule[i])
                    return false;
            }
            return true;
        }
        private static byte[] StripPreambule(byte[] buffer, System.Text.Encoding encoding)
        {
            byte[] preambule = encoding.GetPreamble();
            if (preambule.Length == 0)
                return buffer;
            byte[] newbuffer = new byte[buffer.Length - preambule.Length];
            System.Array.Copy(buffer, preambule.Length, newbuffer, 0, newbuffer.Length);
            return newbuffer;
        }
	}
}
