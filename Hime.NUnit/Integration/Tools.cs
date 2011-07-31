using System;
using System.Collections.Generic;
using System.Text;
using Hime.Parsers;
using Hime.Kernel.Reporting;

namespace Hime.NUnit.Integration
{
    public class Tools
    {
        public bool BuildRawText(string grammar, ParsingMethod method)
        {
            CompilationTask task = new CompilationTask();
            task.InputRawData.Add(grammar);
            return this.RunTask(task, method);
        }
        
        // TODO: try to factor all calls to new CompilationTask
        // TODO: remove all static methods
        public bool BuildResource(string file, ParsingMethod method)
        {
            CompilationTask task = new CompilationTask();
            task.InputFiles.Add(file);
            return this.RunTask(task, method);
        }
        
        private bool RunTask(CompilationTask task, ParsingMethod method)
        {
            task.GrammarName = "Test";
            task.Method = method;
            Report report = task.Execute();
            return !report.HasErrors;
        }

        public void Export(string resourceName, string fileName)
        {
            System.Reflection.Assembly p_Assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string p_DefaultPath = "Resources";
            Kernel.Resources.ResourceAccessor accessor = new Kernel.Resources.ResourceAccessor(p_Assembly, p_DefaultPath);
            accessor.Export(resourceName, fileName);
            accessor.Close();
        }
        public string GetAllTextFor(string Name)
        {
            System.Reflection.Assembly p_Assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string p_DefaultPath = "Resources";
            Kernel.Resources.ResourceAccessor accessor = new Kernel.Resources.ResourceAccessor(p_Assembly, p_DefaultPath);
            string data = accessor.GetAllTextFor(Name);
            accessor.Close();
            return data;
        }
    }
}
