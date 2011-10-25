/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
using Hime.Kernel.Reporting;
using System.IO;

namespace Hime.Parsers
{
    public sealed class CompilationTask
    {
        public ICollection<string> InputRawData { get; private set; }
        public ICollection<string> InputFiles { get; private set; }
        public string GrammarName { get; set; }
        public string Namespace { get; set; }
        public ParsingMethod Method { get; set; }
        public string LexerFile { get; set; }
        public string ParserFile { get; set; }
        public bool ExportDebug { get; set; }
        public bool ExportLog { get; set; }
        public bool ExportDoc { get; set; }
        public bool ExportVisuals { get; set; }
        public string DOTBinary { get; set; }
        public AccessModifier GeneratedCodeModifier { get; set; }
        
        public CompilationTask()
        {
            InputRawData = new List<string>();
            InputFiles = new List<string>();
            Method = ParsingMethod.RNGLALR1;
            ExportDebug = false;
            ExportLog = false;
            ExportDoc = false;
            ExportVisuals = false;
            GeneratedCodeModifier = AccessModifier.Public;
        }
    }
}
