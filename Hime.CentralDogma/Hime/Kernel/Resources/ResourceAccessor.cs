using System.Collections.Generic;

namespace Hime.Kernel.Resources
{
    public sealed class ResourceAccessor
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
            System.IO.Stream Stream = assembly.GetManifestResourceStream(defaultPath + resourceName);
            if (Stream == null)
                throw new ResourceNotFoundException(resourceName);
            byte[] Buffer = new byte[Stream.Length];
            int ReadCount = Stream.Read(Buffer, 0, Buffer.Length);
            System.IO.File.WriteAllBytes(fileName, Buffer);
            files.Add(fileName);
        }

        public void Export(string resourceName, string fileName)
        {
            if (isClosed)
                throw new AccessorClosedException(this);
            System.IO.Stream Stream = assembly.GetManifestResourceStream(defaultPath + resourceName);
            if (Stream == null)
                throw new ResourceNotFoundException(resourceName);
            byte[] Buffer = new byte[Stream.Length];
            int ReadCount = Stream.Read(Buffer, 0, Buffer.Length);
            System.IO.File.WriteAllBytes(fileName, Buffer);
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
            System.IO.Stream Stream = assembly.GetManifestResourceStream(defaultPath + resourceName);
            if (Stream == null)
                throw new ResourceNotFoundException(resourceName);
            // Extract content to a buffer
			byte[] Buffer = new byte[Stream.Length];
			int ReadCount = Stream.Read(Buffer, 0, Buffer.Length);
            if (ReadCount != Buffer.Length)
                return null;
            // Detect encoding and strip encoding preambule
            System.Text.Encoding Encoding = DetectEncoding(Buffer);
            Buffer = StripPreambule(Buffer, Encoding);
            // Return decoded text
			return new string(System.Text.Encoding.UTF8.GetChars(Buffer));
		}
        
        private static System.Text.Encoding DetectEncoding(byte[] Buffer)
        {
            if (DetectEncoding_TryEncoding(Buffer, System.Text.Encoding.UTF8))
                return System.Text.Encoding.UTF8;
            if (DetectEncoding_TryEncoding(Buffer, System.Text.Encoding.Unicode))
                return System.Text.Encoding.Unicode;
            if (DetectEncoding_TryEncoding(Buffer, System.Text.Encoding.BigEndianUnicode))
                return System.Text.Encoding.BigEndianUnicode;
            if (DetectEncoding_TryEncoding(Buffer, System.Text.Encoding.UTF32))
                return System.Text.Encoding.UTF32;
            if (DetectEncoding_TryEncoding(Buffer, System.Text.Encoding.ASCII))
                return System.Text.Encoding.ASCII;
            return System.Text.Encoding.Default;
        }
        private static bool DetectEncoding_TryEncoding(byte[] Buffer, System.Text.Encoding Encoding)
        {
            byte[] Preambule = Encoding.GetPreamble();
            if (Buffer.Length < Preambule.Length)
                return false;
            for (int i = 0; i != Preambule.Length; i++)
            {
                if (Buffer[i] != Preambule[i])
                    return false;
            }
            return true;
        }
        private static byte[] StripPreambule(byte[] Buffer, System.Text.Encoding Encoding)
        {
            byte[] Preambule = Encoding.GetPreamble();
            if (Preambule.Length == 0)
                return Buffer;
            byte[] NewBuffer = new byte[Buffer.Length - Preambule.Length];
            System.Array.Copy(Buffer, Preambule.Length, NewBuffer, 0, NewBuffer.Length);
            return NewBuffer;
        }
	}
}
