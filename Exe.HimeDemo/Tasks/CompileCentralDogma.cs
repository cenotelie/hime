using System;
using System.Collections.Generic;
using System.Text;
using Hime.Parsers;
using System.IO;

namespace Hime.Demo.Tasks
{
    class CompileCentralDogma : IExecutable
    {
        public void Execute()
        {
            CompilationTask task = new CompilationTask();
            task.Method = ParsingMethod.LALR1;
            task.Namespace = "Hime.Demo.Generated.CD";
            task.ExportLog = true;
            task.ExportDocumentation = true;
            task.ExportVisuals = false;
            task.InputFiles.Add("Languages\\FileCentralDogma.gram");
            task.GrammarName = "FileCentralDogma";
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            path = Path.Combine(path, "Graphviz 2.28");
            path = Path.Combine(path, "bin");
            path = Path.Combine(path, "dot.exe");
            task.DOTBinary = path;
            task.Execute();
        }
    }
}
