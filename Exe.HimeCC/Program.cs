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
using Hime.CentralDogma;
using Hime.CentralDogma.Reporting;

namespace Hime.Compiler
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            Options options = new Options();
			CompilationTask task = options.BuildCompilationTaskFromArguments(args);
			if (task == null) 
			{
            	System.Console.WriteLine(options.GetUsage());
            	return 0;
			}
        	Report result = task.Execute();
			// TODO: maybe would be nicer to return the number of errors
			if (result.HasErrors) return 1;
			return 0;
        }
    }
}
