/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:26
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
using Hime.Parsers;
using CommandLine;

namespace Hime.HimeCC
{
    public class Program
    {
        public static void Main(string[] args)
        {
        	Program program = new Program();
        	Options options = program.ParseArguments(args);
            if (options.Inputs.Count == 0) 
            {
            	System.Console.WriteLine(options.GetUsage());
            	return;
            }
        	program.Execute(options);
        }

        public Options ParseArguments(string[] args)
        {
            Options options = new Options();
            ICommandLineParser parser = new CommandLineParser();
            if (!parser.ParseArguments(args, options))
            {
            	options.Inputs = new List<string>();
            }
            return options;
        }

        public void Execute(Options options)
        {
            CompilationTask task = new CompilationTask();
            foreach (string input in options.Inputs)
                task.InputFiles.Add(input);
            task.Method = options.Method;
            // TODO: this test is probably not necessary, as options.GrammarName is already equal to null
            // TODO: remove this test
            if (options.GrammarName != null)
                task.GrammarName = options.GrammarName;
            if (options.Namespace != null)
                task.Namespace = options.Namespace;
            if (options.LexerFile != null)
                task.LexerFile = options.LexerFile;
            if (options.ParserFile != null)
                task.ParserFile = options.ParserFile;
            task.ExportLog = options.ExportHTMLLog;
            task.ExportDoc = options.ExportDocumentation;
            task.Execute();
        }
    }
}
// TODO: apply strict coding standards
// - remove all static methods
