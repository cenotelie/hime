/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:26
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using CommandLine;
using Hime.CentralDogma;

namespace Hime.Compiler
{
	// I do not like CommandLine because it forces you to have public fields!!!
    internal class Options
    {
        public Options()
        {
            this.inputs = new List<string>();
            this.method = ParsingMethod.RNGLALR1;
            this.exportLog = false;
        }

        [ValueList(typeof(List<string>))]
        public List<string> inputs;

        [Option("g", "grammar", Required = false, HelpText="Name of the grammar for which a parser shall be generated")]
        public string grammarName;

        [Option("n", "namespace", Required = false, HelpText = "Namespace for the generated Lexer and Parser classes")]
        public string outputNamespace;

        [Option("m", "method", Required = false, HelpText = "Name of the parsing method to use: LR0|LR1|LALR1|RNGLR1|RNGLALR1|LRStar")]
        public ParsingMethod method;

        [Option("o", "output", Required = false, HelpText = "Prefix for the compiler's outputs")]
        public string output;

        [Option("l", "log", Required = false, HelpText = "True to export the generation log (HTML file)")]
        public bool exportLog;

        [Option("d", "doc", Required = false, HelpText = "True to export the parser documentation (HTML files)")]
        public bool exportDocumentation;

        [HelpOption("h", "help", HelpText = "Display this help screen.")]
        public string GetUsage()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            CommandLine.Text.HelpText help = new CommandLine.Text.HelpText(assembly.FullName);
            help.AdditionalNewLineAfterOption = true;
            help.AddPreOptionsLine("This is free software. You may redistribute copies of it under the terms of");
            help.AddPreOptionsLine("the LGPL License <http://www.gnu.org/licenses/lgpl.html>.");
            help.AddPreOptionsLine("");
            help.AddPreOptionsLine("+- Usage:  --------------------------------------------------------+");
            help.AddPreOptionsLine("| Default usage:                                                   |");
            help.AddPreOptionsLine("|   > himecc MyGram.gram                                           |");
            help.AddPreOptionsLine("| Export the compilation log and grammar's documentation:          |");
            help.AddPreOptionsLine("|   > himecc MyGram.gram -l -d                                     |");
            help.AddPreOptionsLine("| Select the outputs' prefix:                                      |");
            help.AddPreOptionsLine("|   > himecc MyGram.gram -o mydir/foo                              |");
            help.AddPreOptionsLine("|   will generate fooLexer.cs, fooParser.cs, fooLexer.bin and      |");
            help.AddPreOptionsLine("|   fooParser.bin in directory 'mydir'                             |");
            help.AddPreOptionsLine("+------------------------------------------------------------------+");
            help.AddOptions(this);
            return help;
        }

		public CompilationTask BuildCompilationTaskFromArguments(string[] arguments)
		{
            CommandLineParser parser = new CommandLineParser();
            if (!parser.ParseArguments(arguments, this)) return null;
            if (this.inputs.Count == 0) return null;

			CompilationTask task = new CompilationTask();
            task.Namespace = this.outputNamespace;
            foreach (string input in this.inputs) task.AddInputFile(input);
            task.GrammarName = this.grammarName;
            task.Method = this.method;
            task.OutputPrefix = this.output;
            task.OutputLog = this.exportLog;
            task.OutputDocumentation = this.exportDocumentation;
			return task;
		}
	}
}
