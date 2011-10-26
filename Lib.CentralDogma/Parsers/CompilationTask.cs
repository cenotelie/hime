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
    public class CompilationTask
    {
        public ICollection<string> InputRawData { get; private set; }
        public ICollection<string> InputFiles { get; private set; }
        public string GrammarName { get; set; }
        public string Namespace { get; set; }
        public ParsingMethod Method { get; private set; }
        public string LexerFile { get; set; }
        public string ParserFile { get; set; }
        public bool ExportDebug { get; set; }
        public bool ExportLog { get; set; }
        public bool ExportDocumentation { get; set; }
        public bool ExportVisuals { get; set; }
        public string DOTBinary { get; set; }
        public AccessModifier GeneratedCodeModifier { get; set; }
        
        public CompilationTask(ParsingMethod method)
        {
            InputRawData = new List<string>();
            InputFiles = new List<string>();
            this.Method = method;
            ExportDebug = false;
            ExportLog = false;
            ExportDocumentation = false;
            ExportVisuals = false;
            GeneratedCodeModifier = AccessModifier.Public;
        }
    }
}
