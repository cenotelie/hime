using System;
using System.Collections.Generic;
using System.Text;
using Hime.Parsers;
using Hime.Kernel.Reporting;
using System.Reflection;
using Hime.Kernel.Resources;

namespace Hime.Tests.Integration
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
            Compiler compiler = new Compiler();
            Report report = compiler.Execute(task);
            return !report.HasErrors;
        }

        public void Export(string resourceName, string fileName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string p_DefaultPath = "Resources";
            Kernel.Resources.ResourceAccessor accessor = new ResourceAccessor(assembly, p_DefaultPath);
            accessor.Export(resourceName, fileName);
            accessor.Close();
        }
        public string GetAllTextFor(string name)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            ResourceAccessor accessor = new ResourceAccessor(assembly, "Resources");
            string data = accessor.GetAllTextFor(name);
            accessor.Close();
            return data;
        }
    }
}
