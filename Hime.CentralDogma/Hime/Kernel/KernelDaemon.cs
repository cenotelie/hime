using System.Collections.Generic;
using Hime.Parsers;
using Hime.Kernel.Reporting;
using System.IO;

namespace Hime.Kernel
{
    public static class KernelDaemon
    {
        public static bool GenerateNextStep(string path)
        {
            // Test path
            path += "\\Daemon\\";
            if (Directory.Exists(path)) Directory.Delete(path, true);
            DirectoryInfo directory = Directory.CreateDirectory(path);
            System.Console.WriteLine(directory.FullName);

            string contextFreeGrammar = "Generators.ContextFreeGrammars.gram";
            // Checkout resources
            Resources.ResourceAccessor session = new Resources.ResourceAccessor();
            session.CheckOut("Daemon.Kernel.gram", path + "Kernel.gram");
            session.CheckOut("Daemon." + contextFreeGrammar, path + contextFreeGrammar);
            session.CheckOut("Daemon.Generators.ContextSensitiveGrammars.gram", path + "Generators.ContextSensitiveGrammars.gram");

            // Compile
            CompilationTask task = new CompilationTask();
            task.InputFiles.Add(path + "Kernel.gram");
            task.InputFiles.Add(path + contextFreeGrammar);
            task.InputFiles.Add(path + "Generators.ContextSensitiveGrammars.gram");
            task.GrammarName = "Hime.Kernel.FileCentralDogma";
            task.Namespace = "Hime.Kernel.Resources.Parser";
            task.Method = Parsers.ParsingMethod.LALR1;
            task.ParserFile = path + "KernelResources.Parser.cs";
            task.ExportLog = true;
            Report result = task.Execute();
            
            // Close session
            session.Close();
            return !result.HasErrors;
        }

        public static void BuildUnicode(string file)
        {
            string[] classes = { "Lu", "Ll", "Lt", "Lm", "Lo", "L", "Mn", "Mc", "Me", "M", "Nd", "Nl", "No", "N", "Pc", "Pd", "Ps", "Pe", "Pi", "Pf", "Po", "P", "Sm", "Sc", "Sk", "So", "S", "Zs", "Zl", "Zp", "Z", "Cc", "Cf", "Cs", "Co", "Cn", "C" };
            List<string> code = new List<string>();
            for (int i = 0; i != classes.Length; i++)
                BuildUnicode_Class(classes[i], code);
            System.IO.File.WriteAllLines(file, code.ToArray());
        }
        private static void BuildUnicode_Class(string name, List<string> code)
        {
            System.Net.WebClient client = new System.Net.WebClient();
            string content = null;
            try { content = client.DownloadString("http://www.fileformat.info/info/unicode/category/" + name + "/list.htm"); }
            catch (System.Exception)
            {
                System.Console.WriteLine("Cannot build class: " + name);
                return;
            }
            System.Text.RegularExpressions.Regex exp = new System.Text.RegularExpressions.Regex("U\\+[0-9A-Za-z]+");
            System.Text.RegularExpressions.MatchCollection matches = exp.Matches(content);
            List<System.UInt16> results = new List<System.UInt16>();
            foreach (System.Text.RegularExpressions.Match match in matches)
            {
                string temp = match.Value.Substring(2);
                if (temp.Length > 4)
                    continue;
                results.Add(System.Convert.ToUInt16(temp, 16));
            }
            results.Sort();

            if (results.Count == 0)
                return;

            code.Add("UnicodeClass c_" + name + " = new UnicodeClass(\"" + name + "\");");
            System.UInt16 begin = results[0];
            System.UInt16 end = begin;
            for (int i = 1; i != results.Count; i++)
            {
                System.UInt16 current = results[i];
                if (current == end + 1)
                {
                    end = current;
                    continue;
                }
                string range = "c_" + name + ".spans.Add(new UnicodeSpan(0x" + begin.ToString("X") + ", 0x" + end.ToString("X") + "));";
                code.Add(range);
                begin = current;
                end = current;
            }
            string r = "c_" + name + ".spans.Add(new UnicodeSpan(0x" + begin.ToString("X") + ", 0x" + end.ToString("X") + "));";
            code.Add(r);
            code.Add("BuildClasses_Class(c_" + name + ");");
        }
    }
}
