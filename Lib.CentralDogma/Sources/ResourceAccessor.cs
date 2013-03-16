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

namespace Hime.CentralDogma
{
	/// <summary>
	/// Provides an API for the access to the resources embedded within an assembly and keeps track of which resource has been exported
	/// </summary>
    public sealed class ResourceAccessor : IDisposable
	{
        private static List<ResourceAccessor> accessors = new List<ResourceAccessor>();

		private Assembly assembly;
		private string rootNamespace;
		private string defaultPath;
        private List<string> files;
        private List<Stream> streams;

        /// <summary>
        /// Gets a collection of the files that have been exported
        /// </summary>
        public ICollection<string> Files { get { return files; } }

        /// <summary>
        /// Initializes the accessor for the CentralDogma assembly
        /// </summary>
        internal ResourceAccessor() : this(Assembly.GetExecutingAssembly(), "Resources") { }

        /// <summary>
        /// Initializes an accessor for the given assembly
        /// </summary>
        /// <param name="assembly">The assembly to explore</param>
        /// <param name="defaultPath">The default path to resources within the assembly</param>
        public ResourceAccessor(Assembly assembly, string defaultPath)
        {
            accessors.Add(this);
            this.assembly = assembly;
            this.rootNamespace = assembly.GetName().Name;
            this.defaultPath = rootNamespace + "." + defaultPath + ".";
            this.files = new List<string>();
            this.streams = new List<Stream>();
        }

        /// <summary>
        /// Disposes of the exported files and closes the open streams to the resources
        /// </summary>
        public void Dispose()
        {
            foreach (string file in files) File.Delete(file);
            foreach (Stream stream in streams) stream.Close();
            accessors.Remove(this);
        }

        /// <summary>
        /// Register an external file as an exported resource so that it can be removed on disposal
        /// </summary>
        /// <param name="fileName">The path and file name</param>
        public void AddCheckoutFile(string fileName)
        {
            files.Add(fileName);
        }

        /// <summary>
        /// Checks out a resource and exports it as a file
        /// </summary>
        /// <param name="resourceName">The resource to export</param>
        /// <param name="fileName">The file that will contain the resource</param>
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
		
        /// <summary>
        /// Exports a resource to a file without keeping a record of it
        /// </summary>
        /// <param name="resourceName">The resource to export</param>
        /// <param name="fileName">The file that will contain the resource</param>
        public void Export(string resourceName, string fileName)
        {
            byte[] buffer = this.ReadResource(resourceName);
            File.WriteAllBytes(fileName, buffer);
        }

        /// <summary>
        /// Gets a stream on a resource
        /// </summary>
        /// <param name="resourceName">the resource</param>
        /// <returns>A stream to the required resource, or null if it does not exists</returns>
        public Stream GetStreamFor(string resourceName)
        {
            Stream stream = this.assembly.GetManifestResourceStream(defaultPath + resourceName);
            if (stream == null) return null;
            streams.Add(stream);
            return stream;
        }

        /// <summary>
        /// Exports a resource as a string
        /// </summary>
        /// <param name="resourceName">The resource</param>
        /// <returns>The string representation of the resource</returns>
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
