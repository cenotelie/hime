/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Kernel.Resources
{
    class ResourceAccessor
	{
        private static List<ResourceAccessor> accessors = new List<ResourceAccessor>();

		private System.Reflection.Assembly assembly;
		private string rootNamespace;
		private string defaultPath;
        private List<string> files;
        private List<System.IO.Stream> streams;
        private bool isClosed;

        public bool IsOpen { get { return !isClosed; } }
        public bool IsClosed { get { return isClosed; } }
        public ICollection<string> Files { get { return files; } }

        internal ResourceAccessor()
            : this(System.Reflection.Assembly.GetExecutingAssembly(), "Hime.Resources")
        { }
        public ResourceAccessor(System.Reflection.Assembly assembly, string defaultPath)
        {
            accessors.Add(this);
            this.assembly = assembly;
            this.rootNamespace = assembly.GetName().Name;
            if (defaultPath == null || defaultPath == string.Empty)
                this.defaultPath = rootNamespace + ".";
            else
                this.defaultPath = rootNamespace + "." + defaultPath + ".";
            this.files = new List<string>();
            this.streams = new List<System.IO.Stream>();
            this.isClosed = false;
        }

        public void Close()
        {
            foreach (string file in files)
                System.IO.File.Delete(file);
            foreach (System.IO.Stream stream in streams)
                stream.Close();
            isClosed = true;
            accessors.Remove(this);
        }

        public void AddCheckoutFile(string fileName)
        {
            if (isClosed)
                throw new AccessorClosedException(this);
            files.Add(fileName);
        }

        public void CheckOut(string resourceName, string fileName)
        {
            if (isClosed)
                throw new AccessorClosedException(this);
            System.IO.Stream stream = assembly.GetManifestResourceStream(defaultPath + resourceName);
            if (stream == null)
                throw new ResourceNotFoundException(resourceName);
            byte[] buffer = new byte[stream.Length];
            int readcount = stream.Read(buffer, 0, buffer.Length);
            System.IO.File.WriteAllBytes(fileName, buffer);
            files.Add(fileName);
        }

        public void Export(string resourceName, string fileName)
        {
            if (isClosed)
                throw new AccessorClosedException(this);
            System.IO.Stream stream = assembly.GetManifestResourceStream(defaultPath + resourceName);
            if (stream == null)
                throw new ResourceNotFoundException(resourceName);
            byte[] buffer = new byte[stream.Length];
            int readCount = stream.Read(buffer, 0, buffer.Length);
            System.IO.File.WriteAllBytes(fileName, buffer);
        }

        public System.IO.Stream GetStreamFor(string resourceName)
        {
            if (isClosed)
                throw new AccessorClosedException(this);
            System.IO.Stream stream = assembly.GetManifestResourceStream(defaultPath + resourceName);
            if (stream == null)
                throw new ResourceNotFoundException(resourceName);
            streams.Add(stream);
            return stream;
        }

        public string GetAllTextFor(string resourceName)
		{
            if (isClosed)
                throw new AccessorClosedException(this);
            // Get a stream on the resource
            System.IO.Stream stream = assembly.GetManifestResourceStream(defaultPath + resourceName);
            if (stream == null)
                throw new ResourceNotFoundException(resourceName);
            // Extract content to a buffer
			byte[] buffer = new byte[stream.Length];
			int readCount = stream.Read(buffer, 0, buffer.Length);
            if (readCount != buffer.Length)
                return null;
            // Detect encoding and strip encoding preambule
            System.Text.Encoding encoding = DetectEncoding(buffer);
            buffer = StripPreambule(buffer, encoding);
            // Return decoded text
			return new string(System.Text.Encoding.UTF8.GetChars(buffer));
		}
        
        private static System.Text.Encoding DetectEncoding(byte[] buffer)
        {
            if (DetectEncoding_TryEncoding(buffer, System.Text.Encoding.UTF8))
                return System.Text.Encoding.UTF8;
            if (DetectEncoding_TryEncoding(buffer, System.Text.Encoding.Unicode))
                return System.Text.Encoding.Unicode;
            if (DetectEncoding_TryEncoding(buffer, System.Text.Encoding.BigEndianUnicode))
                return System.Text.Encoding.BigEndianUnicode;
            if (DetectEncoding_TryEncoding(buffer, System.Text.Encoding.UTF32))
                return System.Text.Encoding.UTF32;
            if (DetectEncoding_TryEncoding(buffer, System.Text.Encoding.ASCII))
                return System.Text.Encoding.ASCII;
            return System.Text.Encoding.Default;
        }
        private static bool DetectEncoding_TryEncoding(byte[] buffer, System.Text.Encoding encoding)
        {
            byte[] preambule = encoding.GetPreamble();
            if (buffer.Length < preambule.Length)
                return false;
            for (int i = 0; i != preambule.Length; i++)
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
