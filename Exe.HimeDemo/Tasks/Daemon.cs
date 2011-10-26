/*
 * @author Charles Hymans
 * */

using System;
using System.Collections.Generic;
using System.Text;
using Hime.Kernel;
using System.IO;
using Hime.Kernel.Resources;
using Hime.Parsers;
using Hime.Kernel.Reporting;

namespace Hime.Demo.Tasks
{
    public class Daemon : IExecutable
    {
		// input path: where to find the source grammars
        private string path;
		// output path: where to generate the result 
		// TODO: this should not be necessary
		// should be able to generate on 
		// standard output instead of files!!!
		private string outputPath;

        public Daemon(string inputPath, string outputPath)
        {
            this.path = inputPath;
			this.outputPath = outputPath;
        }

        public void Execute()
        {
			this.GenerateNextStep();
        }
		
		public bool GenerateNextStep()
		{
			System.Console.WriteLine(path);
            System.Console.WriteLine(this.outputPath);

			// Test path
            if (Directory.Exists(this.outputPath)) Directory.Delete(outputPath, true);
            Directory.CreateDirectory(outputPath);

			string pathToKernel = Path.Combine(path, "Kernel.gram");
			string pathToContextFree = Path.Combine(path, "CFGrammars.gram");
			string pathToContextSensitive = Path.Combine(path, "CSGrammars.gram");
			
            CompilationTask task = new CompilationTask(ParsingMethod.LALR1);
         	task.InputFiles.Add(pathToKernel);
          	task.InputFiles.Add(pathToContextFree);
         	task.InputFiles.Add(pathToContextSensitive);
         	task.GrammarName = "Hime.Kernel.FileCentralDogma";
         	task.Namespace = "Hime.Kernel.Resources.Parser";
         	// TODO: this assignment is a bit strange, should not be done like that?
        	// see how it is done with options in himecc
        	task.ParserFile = Path.Combine(this.outputPath, "KernelResources.Parser.cs");
         	task.ExportLog = true;
        	Compiler compiler = new Compiler();
       	    Report result = compiler.Execute(task);
            return !result.HasErrors;
		}
    }
}
