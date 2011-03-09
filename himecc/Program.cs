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
            Hime.Parsers.CompilationTask Task = Parsers.CompilationTask.Create(options.Inputs.ToArray(), options.GrammarName, options.Method, options.Namespace, options.LexerFile, options.ParserFile, options.ExportHTMLLog, options.ExportDocumentation);
            Task.Execute();
        }
    }
}
