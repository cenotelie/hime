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
using Hime.Kernel.Reporting;

namespace Hime.HimeCC
{
    public class Program
    {
        public static int Main(string[] args)
        {
        	Program program = new Program();
        	Options options = program.ParseArguments(args);
            if (options.Inputs.Count == 0) 
            {
            	System.Console.WriteLine(options.GetUsage());
            	return 0;
            }
        	Report result = program.Execute(options);
			// TODO: maybe would be nicer to return the number of errors
			if (result.HasErrors) return 1;
			return 0;
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

        public Report Execute(Options options)
        {
			CompilationTask task = options.BuildCompilationTask();
            Compiler compiler = new Compiler();
            return compiler.Execute(task);
        }
    }
}
