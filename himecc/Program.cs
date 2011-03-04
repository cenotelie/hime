using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.HimeCC
{
    class Program
    {
        static void Main(string[] args)
        {
            Options options = new Options();
            CommandLine.ICommandLineParser parser = new CommandLine.CommandLineParser();
            if (parser.ParseArguments(args, options))
                Execute(options);
            else
                System.Console.WriteLine(options.GetUsage());
        }

        private static void Execute(Options options)
        {
            Hime.Parsers.CompilationTask Task = new Parsers.CompilationTask(options.Inputs, options.GrammarName, options.Method, options.Namespace, options.LexerFile, options.ParserFile, options.ExportHTMLLog, false);
            Task.Execute();
        }
    }
}
