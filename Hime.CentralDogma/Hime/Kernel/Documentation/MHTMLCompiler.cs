﻿/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Kernel.Documentation
{
    public class MHTMLCompiler
    {
        private List<MHTMLSource> sources;
        private string from;
        private string title;
        private string boundary;
        private int linebreak;

        public string From { get { return from; } set { from = value; } }
        public string Title { get { return title; } set { title = value; } }

        public MHTMLCompiler()
        {
            sources = new List<MHTMLSource>();
            from = "Generated by " + (typeof(MHTMLCompiler)).Name;
            title = "Some documentation";
            boundary = "----=_NextPart_000_0000_01CC2201.CF84F290";
            linebreak = 76;
        }

        public void AddSource(MHTMLSource source) { sources.Add(source); }

        public void CompileTo(string file)
        {
            System.IO.StreamWriter writer = new System.IO.StreamWriter(file, false, System.Text.Encoding.UTF8);
            writer.Write("From: ");
            writer.WriteLine("\"" + from + "\"");
            writer.Write("Subject: ");
            writer.WriteLine(title);
            writer.Write("Date: ");
            writer.WriteLine(System.DateTime.Now.ToLongDateString());
            writer.WriteLine("MIME-Version: 1.0");
            writer.WriteLine("Content-Type: multipart/related;");
            writer.WriteLine("\ttype=\"text/html\";");
            writer.WriteLine("\tboundary=\"" + boundary + "\"");

            foreach (MHTMLSource source in sources)
            {
                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine("--" + boundary);
                writer.Write("Content-Type: ");
                writer.WriteLine(source.ContentType);
                writer.Write("Content-Transfer-Encoding: ");
                writer.WriteLine(source.ContentTransferEncoding);
                writer.Write("Content-Location: ");
                writer.WriteLine(source.ContentLocation);
                writer.WriteLine();

                int length = 0;
                byte[] buffer = new byte[1024];
                string text = source.Read();
                while (text != null)
                {
                    while (linebreak < (length + text.Length))
                    {
                        string part1 = text.Substring(0, linebreak - length);
                        text = text.Substring(linebreak - length);
                        writer.WriteLine(part1);
                        length = 0;
                    }
                    length += text.Length;
                    writer.Write(text);
                    text = source.Read();
                }
                source.Close();
            }

            writer.WriteLine();
            writer.WriteLine();
            writer.WriteLine("--" + boundary + "--");
            writer.Close();
        }
    }
}