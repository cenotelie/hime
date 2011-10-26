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
        private string path;

        public Daemon(string directory)
        {
            this.path = Path.Combine(directory, "Daemon");
        }

        public void Execute()
        {
			this.GenerateNextStep();
        }
		
		public bool GenerateNextStep()
		{
			// Test path
            if (Directory.Exists(path)) Directory.Delete(path, true);
            DirectoryInfo directory = Directory.CreateDirectory(path);
            System.Console.WriteLine(directory.FullName);

			// Checkout resources
            using (ResourceAccessor session = new ResourceAccessor())
			{
				string pathToKernel = Path.Combine(path, "Kernel.gram");
				string pathToContextFree = Path.Combine(path, "CFGrammars.gram");
				string pathToContextSensitive = Path.Combine(path, "CSGrammars.gram");
            	session.CheckOut("Daemon.Kernel.gram", pathToKernel);
            	session.CheckOut("Daemon.CFGrammars.gram", pathToContextFree);
            	session.CheckOut("Daemon.CSGrammars.gram", pathToContextSensitive);

            	CompilationTask task = new CompilationTask();
         	   	task.InputFiles.Add(pathToKernel);
          	  	task.InputFiles.Add(pathToContextFree);
         	   	task.InputFiles.Add(pathToContextSensitive);
         	   	task.GrammarName = "Hime.Kernel.FileCentralDogma";
         	   	task.Namespace = "Hime.Kernel.Resources.Parser";
         	   	task.Method = ParsingMethod.LALR1;
         	   	// TODO: this assignment is a bit strange, should not be done like that?
        	    // see how it is done with options in himecc
        	    task.ParserFile = Path.Combine(path, "KernelResources.Parser.cs");
         	   	task.ExportLog = true;
        	    Compiler compiler = new Compiler();
       	     	Report result = compiler.Execute(task);
            
            	return !result.HasErrors;
			}			
		}
    }
}
