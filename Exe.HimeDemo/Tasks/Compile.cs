using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Hime.CentralDogma;

namespace Hime.Demo.Tasks
{
    class Compile : IExecutable
    {
        private string file;
        private string nmspce;
        private ParsingMethod method;

        public Compile(string file, string nmspce, ParsingMethod method)
        {
            this.file = file;
            this.nmspce = nmspce;
            this.method = method;
        }

        public void Execute()
        {
            CompilationTask task = new CompilationTask();
            task.Method = method;
            task.Namespace = nmspce;
            task.ExportLog = false;
            task.ExportDocumentation = false;
            task.InputFiles.Add(file);
            task.ExecuteDo();
        }
    }
}
