using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.HimeCC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Options options = ParseArguments(args);
            if (options != null)
                Execute(options);
            else
                System.Console.WriteLine((new Options()).GetUsage());
        }

        public static Options ParseArguments(string[] args)
        {
            if (args.Length == 0)
                return null;
            Options options = new Options();
            CommandLine.ICommandLineParser parser = new CommandLine.CommandLineParser();
            if (!parser.ParseArguments(args, options))
                return null;
            return options;
        }

        public static void Execute(Options options)
        {
            Hime.Parsers.CompilationTask task = new Parsers.CompilationTask();
            foreach (string input in options.Inputs)
                task.InputFiles.Add(input);
            task.Method = options.Method;
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
            task.ExportVisuals = options.ExportDocumentation;
            task.Execute();
        }
    }
}
