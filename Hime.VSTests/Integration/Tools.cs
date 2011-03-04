using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hime.VSTests.Integration
{
    static class Tools
    {
        public static bool BuildRawText(string text, Hime.Parsers.ParsingMethod method)
        {
            Parsers.CompilationTask Task = new Parsers.CompilationTask(text, "Test", method, "Analyzer", "TestAnalyze.cs", false, false);
            Kernel.Reporting.Report Report = Task.Execute();
            foreach (Kernel.Reporting.Section section in Report.Sections)
                foreach (Kernel.Reporting.Entry entry in section.Entries)
                    if (entry.Level == Kernel.Reporting.Level.Error)
                        return false;
            return true;
        }
        public static bool BuildResource(string file, string name, Hime.Parsers.ParsingMethod method)
        {
            Parsers.CompilationTask Task = new Parsers.CompilationTask(GetAllTextFor(file), name, method, "Analyzer", "TestAnalyze.cs", false, false);
            Kernel.Reporting.Report Report = Task.Execute();
            foreach (Kernel.Reporting.Section section in Report.Sections)
                foreach (Kernel.Reporting.Entry entry in section.Entries)
                    if (entry.Level == Kernel.Reporting.Level.Error)
                        return false;
            return true;
        }


        public static string GetAllTextFor(string Name)
        {
            System.Reflection.Assembly p_Assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string p_RootNamespace = p_Assembly.GetName().Name;
            string p_DefaultPath = p_RootNamespace + ".Resources.";
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
