namespace Hime.Kernel.Resources
{
    public sealed class ResourceAccessor
	{
        private static System.Collections.Generic.List<ResourceAccessor> p_Accessors = new System.Collections.Generic.List<ResourceAccessor>();

		private System.Reflection.Assembly p_Assembly;
		private string p_RootNamespace;
		private string p_DefaultPath;
        private System.Collections.Generic.List<string> p_Files;
        private bool p_IsClosed;

        public bool IsOpen { get { return !p_IsClosed; } }
        public bool IsClosed { get { return p_IsClosed; } }
        public System.Collections.Generic.IEnumerable<string> Files { get { return p_Files; } }

        internal ResourceAccessor()
            : this(System.Reflection.Assembly.GetExecutingAssembly(), "Hime.Resources")
        { }
        public ResourceAccessor(System.Reflection.Assembly assembly, string defaultPath)
        {
            p_Accessors.Add(this);
            p_Assembly = assembly;
            p_RootNamespace = p_Assembly.GetName().Name;
            if (defaultPath == null || defaultPath == string.Empty)
                p_DefaultPath = p_RootNamespace + ".";
            else
                p_DefaultPath = p_RootNamespace + "." + defaultPath + ".";
            p_Files = new System.Collections.Generic.List<string>();
            p_IsClosed = false;
        }

        ~ResourceAccessor()
        {
            while (p_Accessors.Count != 0)
                p_Accessors[0].Close();
        }

        public void Close()
        {
            foreach (string file in p_Files)
                System.IO.File.Delete(file);
            p_IsClosed = true;
            p_Accessors.Remove(this);
        }

        public void AddCheckoutFile(string fileName)
        {
            if (p_IsClosed)
                throw new AccessorClosedException(this);
            p_Files.Add(fileName);
        }

        public void CheckOut(string resourceName, string fileName)
        {
            if (p_IsClosed)
                throw new AccessorClosedException(this);
            System.IO.Stream Stream = p_Assembly.GetManifestResourceStream(p_DefaultPath + resourceName);
            if (Stream == null)
                throw new ResourceNotFoundException(resourceName);
            byte[] Buffer = new byte[Stream.Length];
            int ReadCount = Stream.Read(Buffer, 0, Buffer.Length);
            System.IO.File.WriteAllBytes(fileName, Buffer);
            p_Files.Add(fileName);
        }

        public void Export(string resourceName, string fileName)
        {
            if (p_IsClosed)
                throw new AccessorClosedException(this);
            System.IO.Stream Stream = p_Assembly.GetManifestResourceStream(p_DefaultPath + resourceName);
            if (Stream == null)
                throw new ResourceNotFoundException(resourceName);
            byte[] Buffer = new byte[Stream.Length];
            int ReadCount = Stream.Read(Buffer, 0, Buffer.Length);
            System.IO.File.WriteAllBytes(fileName, Buffer);
        }

        public string GetAllTextFor(string resourceName)
		{
            if (p_IsClosed)
                throw new AccessorClosedException(this);
            // Get a stream on the resource
            System.IO.Stream Stream = p_Assembly.GetManifestResourceStream(p_DefaultPath + resourceName);
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
