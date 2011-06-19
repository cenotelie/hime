using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hime.VSTests.Integration
{
    static class Tools
    {
        public static bool BuildRawText(string text, Hime.Parsers.ParsingMethod method)
        {
            Parsers.CompilationTask Task = Parsers.CompilationTask.Create(text, "Test", method, "Analyzer", null, "TestAnalyze.cs", false, false, false);
            Kernel.Reporting.Report Report = Task.Execute();
            foreach (Kernel.Reporting.Section section in Report.Sections)
                foreach (Kernel.Reporting.Entry entry in section.Entries)
                    if (entry.Level == Kernel.Reporting.Level.Error)
                        return false;
            return true;
        }
        public static bool BuildResource(string file, string name, Hime.Parsers.ParsingMethod method)
        {
            Parsers.CompilationTask Task = Parsers.CompilationTask.Create(GetAllTextFor(file), name, method, "Analyzer", null, "TestAnalyze.cs", false, false, false);
            Kernel.Reporting.Report Report = Task.Execute();
            foreach (Kernel.Reporting.Section section in Report.Sections)
                foreach (Kernel.Reporting.Entry entry in section.Entries)
                    if (entry.Level == Kernel.Reporting.Level.Error)
                        return false;
            return true;
        }


        public static void Export(string resourceName, string fileName)
        {
            System.Reflection.Assembly p_Assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string p_DefaultPath = "Resources";
            Kernel.Resources.ResourceAccessor accessor = new Kernel.Resources.ResourceAccessor(p_Assembly, p_DefaultPath);
            accessor.Export(resourceName, fileName);
            accessor.Close();
        }
        public static string GetAllTextFor(string Name)
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
