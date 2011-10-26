/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:26
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;
using Hime.Parsers;

namespace Hime.HimeCC
{
	// TODO: put as much fields as possible into private
    public class Options
    {
        public Options()
        {
            this.Inputs = new List<string>();
            this.Method = Parsers.ParsingMethod.RNGLALR1;
            this.exportLog = false;
        }

        [ValueList(typeof(List<string>))]
        public List<string> Inputs;

        [Option("g", "grammar", Required = false, HelpText="Name of the grammar for which a parser shall be generated")]
        public string GrammarName;

        [Option("n", "namespace", Required = false, HelpText = "Namespace for the generated Lexer and Parser classes")]
        public string Namespace;

        [Option("m", "method", Required = false, HelpText = "Name of the parsing method to use: LR0|LR1|LALR1|RNGLR1|RNGLALR1|LRStar")]
        public Parsers.ParsingMethod Method;

        [Option(null, "lexer", Required = false, HelpText = "Path and name of the file for the generated lexer")]
        public string LexerFile;

        [Option(null, "parser", Required = false, HelpText = "Path and name of the file for the generated parser")]
        public string ParserFile;

        [Option("l", "log", Required = false, HelpText = "True to export the generation log (HTML file)")]
        private bool exportLog;

        [Option("d", "doc", Required = false, HelpText = "True to export the parser documentation (HTML files)")]
        private bool exportDocumentation;

        [HelpOption("h", "help", HelpText = "Display this help screen.")]
        public string GetUsage()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            CommandLine.Text.HelpText help = new CommandLine.Text.HelpText(assembly.FullName);
            help.AdditionalNewLineAfterOption = true;
            help.AddPreOptionsLine("This is free software. You may redistribute copies of it under the terms of");
            help.AddPreOptionsLine("the LGPL License <http://www.gnu.org/licenses/lgpl.html>.");
            help.AddPreOptionsLine("");
            help.AddPreOptionsLine("+- Usage:  --------------------------------------------------------+");
            help.AddPreOptionsLine("| For a file MyGram.gram containing grammar Foo,                   |");
            help.AddPreOptionsLine("|   > himecc MyGram.gram -g Foo -n Bar -m LALR1 --parser MyGram.cs |");
            help.AddPreOptionsLine("| Default options:                                                 |");
            help.AddPreOptionsLine("|   > himecc MyGram.gram                                           |");
            help.AddPreOptionsLine("| is equivalent to                                                 |");
            help.AddPreOptionsLine("|   > himecc MyGram.gram -g Foo -n Foo -m RNGLALR1                 |");
            help.AddPreOptionsLine("|    --lexer FooLexer.cs --parser FooParser.cs                     |");
            help.AddPreOptionsLine("+------------------------------------------------------------------+");
            help.AddOptions(this);
            return help;
        }

		public CompilationTask BuildCompilationTask()
		{
			CompilationTask task = new CompilationTask(this.Method);
            task.Namespace = this.Namespace;
            foreach (string input in this.Inputs) task.InputFiles.Add(input);
            task.GrammarName = this.GrammarName;
            task.LexerFile = this.LexerFile;
            task.ParserFile = this.ParserFile;
            task.ExportLog = this.exportLog;
            task.ExportDocumentation = this.exportDocumentation;
			return task;
		}
	
		public void FillFromArguments(string[] arguments)
		{
            CommandLineParser parser = new CommandLineParser();
            if (!parser.ParseArguments(arguments, this))
            {
            	this.Inputs = new List<string>();
            }
		}
	}
}
