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
            Hime.Kernel.Reporting.Reporter Reporter = new Hime.Kernel.Reporting.Reporter(typeof(Program));
            Hime.Kernel.Namespace root = Hime.Kernel.Namespace.CreateRoot();
            Hime.Kernel.Resources.ResourceCompiler compiler = new Hime.Kernel.Resources.ResourceCompiler();
            for (int i = 0; i != options.Inputs.Length; i++)
                compiler.AddInputFile(options.Inputs[i]);
            compiler.Compile(root, Reporter);
            Hime.Parsers.Grammar grammar = (Hime.Parsers.Grammar)root.ResolveName(Hime.Kernel.QualifiedName.ParseName(options.GrammarName));

            Hime.Parsers.CF.CFParserGenerator generator = null;
            switch (options.Method)
            {
                case Method.LR0:
                    generator = new Hime.Parsers.CF.LR.MethodLR0();
                    break;
                case Method.LR1:
                    generator = new Hime.Parsers.CF.LR.MethodLR1();
                    break;
                case Method.LALR1:
                    generator = new Hime.Parsers.CF.LR.MethodLALR1();
                    break;
                case Method.RNGLR1:
                    generator = new Hime.Parsers.CF.LR.MethodRNGLR1();
                    break;
                case Method.RNGLALR1:
                    generator = new Hime.Parsers.CF.LR.MethodRNGLALR1();
                    break;
            }
            Hime.Parsers.GrammarBuildOptions Options = null;
            if (options.LexerFile != null)
                Options = new Hime.Parsers.GrammarBuildOptions(Reporter, options.Namespace, generator, options.LexerFile, options.ParserFile);
            else
                Options = new Hime.Parsers.GrammarBuildOptions(Reporter, options.Namespace, generator, options.ParserFile);
            
            grammar.Build(Options);
            Options.Close();
            if (options.ExportHTMLLog)
            {
                string file = options.ParserFile.Replace(".cs", ".html");
                Reporter.ExportHTML(file, "Grammar Log");
                System.Diagnostics.Process.Start(file);
            }
        }
    }
}
