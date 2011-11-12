using System;
using System.Collections.Generic;
using System.Text;
using Hime.Parsers;
using System.IO;

namespace Hime.Demo.Tasks
{
    class Compile : IExecutable
    {
        public void Execute()
        {
            CompilationTask task = new CompilationTask(ParsingMethod.LRStar);
            task.Namespace = "Analyser";
            task.ExportLog = true;
            task.ExportDocumentation = true;
            task.ExportVisuals = false;
            task.InputFiles.Add("Languages\\MathExp.gram");
			string path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
			path = Path.Combine(path, "Graphviz 2.28");
			path = Path.Combine(path, "bin");
			path = Path.Combine(path, "dot.exe");
            task.DOTBinary = path;
			Compiler compiler = new Compiler(task);
            compiler.Execute();
        }
    }
}
