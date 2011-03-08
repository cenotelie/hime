using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace Hime.HimeCC
{
    public class Options
    {
        public Options()
        {
            Inputs = new List<string>();
            Method = Parsers.ParsingMethod.RNGLALR1;
            ExportHTMLLog = false;
        }

        [ValueList(typeof(List<string>))]
        public List<string> Inputs;

        [Option("g", "grammar", Required=false, HelpText="Name of the grammar for which a parser shall be generated")]
        public string GrammarName;

        [Option("n", "namespace", Required = false, HelpText = "Namespace for the generated Lexer and Parser classes")]
        public string Namespace;

        [Option("m", "method", Required = false, HelpText = "Name of the parsing method to use: LR0|LR1|LALR1|RNGLR1|RNGLALR1")]
        public Parsers.ParsingMethod Method;

        [Option(null, "lexer", Required = false, HelpText = "Path and name of the file for the generated lexer")]
        public string LexerFile;

        [Option(null, "parser", Required = false, HelpText = "Path and name of the file for the generated parser")]
        public string ParserFile;

        [Option("l", "log", Required = false, HelpText = "True to export the generation log (HTML file)")]
        public bool ExportHTMLLog;

        [HelpOption("h", "help", HelpText = "Display this help screen.")]
        public string GetUsage()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            CommandLine.Text.HelpText help = new CommandLine.Text.HelpText(assembly.FullName);
            help.AdditionalNewLineAfterOption = true;
            help.AddPreOptionsLine("This is free software. You may redistribute copies of it under the terms of");
            help.AddPreOptionsLine("the LGPL License <http://www.gnu.org/licenses/lgpl.html>.");
            help.AddPreOptionsLine("Usage: himecc MyGram.gram");
            help.AddPreOptionsLine("Usage: himecc MyGram.gram -g MyGrammar -n Analyser -m LALR1 --parser MyGram.cs");
            help.AddOptions(this);
            return help;
        }

        public bool Check()
        {
            if (Inputs.Count == 0)
                return false;
            if (Namespace == null)
                Namespace = System.IO.Path.GetFileNameWithoutExtension(Inputs[0]);
            if (ParserFile == null)
                ParserFile = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Inputs[0]), System.IO.Path.GetFileNameWithoutExtension(Inputs[0]) + ".cs");
            return true;
        }
    }
}
