using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.HimeCC
{
    public class Program
    {
        static void Main(string[] args)
        {
            Options options = ParseArguments(args);
            if (options != null)
                Execute(options);
            else
                System.Console.WriteLine((new Options()).GetUsage());
        }

        public static Options ParseArguments(string[] args)
        {
            Options options = new Options();
            CommandLine.ICommandLineParser parser = new CommandLine.CommandLineParser();
            if (parser.ParseArguments(args, options))
            {
                if (!options.Check())
                    return null;
                return options;
            }
            return null;
        }

        public static void Execute(Options options)
        {
            Hime.Parsers.CompilationTask Task = new Parsers.CompilationTask(options.Inputs.ToArray(), options.GrammarName, options.Method, options.Namespace, options.LexerFile, options.ParserFile, options.ExportHTMLLog, false);
            Task.Execute();
        }
    }
}
