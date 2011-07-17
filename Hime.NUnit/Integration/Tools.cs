using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Hime.Parsers;

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
        public bool BuildResource(string file, string name, ParsingMethod method)
        {
            CompilationTask task = new CompilationTask();
            task.InputFiles.Add(file);
            return this.RunTask(task, method);
        }
        
        private bool RunTask(CompilationTask task, ParsingMethod method)
        {
            task.GrammarName = "Test";
            task.Method = method;
            task.Namespace = "Analyze";
            task.ParserFile = "TestAnalyze.cs"; 
            Kernel.Reporting.Report Report = task.Execute();
            foreach (Kernel.Reporting.Section section in Report.Sections)
            {
                foreach (Kernel.Reporting.Entry entry in section.Entries)
                {
                    if (entry.Level == Kernel.Reporting.Level.Error) return false;
                }
            }
            return true;        	
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
