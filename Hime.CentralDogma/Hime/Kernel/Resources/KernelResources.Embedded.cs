namespace Hime.Kernel.Resources
{
    class AccessorSession
    {
        private bool p_IsClosed;
        private System.Collections.Generic.List<string> p_Files;

        public bool IsOpen { get { return !p_IsClosed; } }
        public bool IsClosed { get { return p_IsClosed; } }
        public System.Collections.Generic.IEnumerable<string> Files { get { return p_Files; } }

        public AccessorSession()
        {
            p_IsClosed = false;
            p_Files = new System.Collections.Generic.List<string>();
        }

        public void AddCheckoutFile(string file)
        {
            if (p_IsClosed) throw new AccessorSessionClosedException(this);
            p_Files.Add(file);
        }

        public void Close()
        {
            foreach (string file in p_Files)
                System.IO.File.Delete(file);
            p_IsClosed = true;
        }
    }


	class ResourceAccessor
	{
		private static System.Reflection.Assembly p_Assembly;
		private static string p_RootNamespace;
		private static string p_DefaultPath;
        private static System.Collections.Generic.List<AccessorSession> p_Sessions;

        static ResourceAccessor()
		{
			p_Assembly = System.Reflection.Assembly.GetExecutingAssembly();
			p_RootNamespace = p_Assembly.GetName().Name;
			p_DefaultPath = p_RootNamespace + ".Hime.Resources.";
            p_Sessions = new System.Collections.Generic.List<AccessorSession>();
		}
        ~ResourceAccessor()
        {
            foreach (AccessorSession Session in p_Sessions)
                if (Session.IsOpen)
                    Session.Close();
        }

        public static AccessorSession CreateCheckoutSession()
        {
            AccessorSession Session = new AccessorSession();
            p_Sessions.Add(Session);
            return Session;
        }

        public static void CheckOut(AccessorSession session, string resourceName, string fileName)
        {
            if (!p_Sessions.Contains(session))
                throw new UnregisteredAccessorSessionException(session);
            if (session.IsClosed)
                throw new AccessorSessionClosedException(session);
            System.IO.Stream Stream = p_Assembly.GetManifestResourceStream(p_DefaultPath + resourceName);
            if (Stream == null)
                throw new ResourceNotFoundException(resourceName);
            byte[] Buffer = new byte[Stream.Length];
            int ReadCount = Stream.Read(Buffer, 0, Buffer.Length);
            System.IO.File.WriteAllBytes(fileName, Buffer);
            session.AddCheckoutFile(fileName);
        }

        public static void Export(string resourceName, string fileName)
        {
            System.IO.Stream Stream = p_Assembly.GetManifestResourceStream(p_DefaultPath + resourceName);
            if (Stream == null)
                throw new ResourceNotFoundException(resourceName);
            byte[] Buffer = new byte[Stream.Length];
            int ReadCount = Stream.Read(Buffer, 0, Buffer.Length);
            System.IO.File.WriteAllBytes(fileName, Buffer);
        }

		private static string GetAllTextFor(string Name)
		{
            // Get a stream on the resource
            System.IO.Stream Stream = p_Assembly.GetManifestResourceStream(p_DefaultPath + Name);
			if (Stream == null)
				return null;
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
