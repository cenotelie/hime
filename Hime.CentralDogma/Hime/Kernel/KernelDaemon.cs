using System.Collections.Generic;

namespace Hime.Kernel
{
    public static class KernelDaemon
    {
        public static void GenerateNextStep(string Path)
        {
            // Initialise logs
            Hime.Kernel.Reporting.Reporter Reporter = new Reporting.Reporter(typeof(KernelDaemon));

            // Test path
            Path += "\\Daemon\\";
            if (System.IO.Directory.Exists(Path))
                System.IO.Directory.Delete(Path, true);
            System.IO.Directory.CreateDirectory(Path);

            Reporter.BeginSection("Daemon");
            Reporter.Info("Daemon", "Hime Systems Daemon");
            Reporter.Info("Daemon", "output at : " + Path);
            Reporter.EndSection();

            // Checkout resources
            Resources.ResourceAccessor Session = new Resources.ResourceAccessor();
            Session.CheckOut("Daemon.Kernel.gram", Path + "Kernel.gram");
            Session.CheckOut("Daemon.Generators.ContextFreeGrammars.gram", Path + "Generators.ContextFreeGrammars.gram");
            Session.CheckOut("Daemon.Generators.ContextSensitiveGrammars.gram", Path + "Generators.ContextSensitiveGrammars.gram");

            // Compile
            Resources.ResourceCompiler Compiler = new Hime.Kernel.Resources.ResourceCompiler();
            Compiler.AddInputFile(Path + "Kernel.gram");
            Compiler.AddInputFile(Path + "Generators.ContextFreeGrammars.gram");
            Compiler.AddInputFile(Path + "Generators.ContextSensitiveGrammars.gram");
            Namespace DaemonRoot = new Namespace(null, "global");
            Compiler.Compile(DaemonRoot, Reporter);

            // Close session
            Session.Close();

            // Generate parser for FileCentralDogma
            Hime.Parsers.Grammar FileCentralDogma = (Hime.Parsers.Grammar)DaemonRoot.ResolveName(QualifiedName.ParseName("Hime.Kernel.FileCentralDogma"));
            Hime.Parsers.GrammarBuildOptions Options = new Hime.Parsers.GrammarBuildOptions(Reporter, "Hime.Kernel.Resources.Parser", new Hime.Parsers.CF.LR.MethodLALR1(), Path + "KernelResources.Parser.cs", false, null, false);
            FileCentralDogma.Build(Options);
            Options.Close();

            // Export log
            Reporter.ExportMHTML(Path + "DaemonLog.html", "Daemon Log");
            System.Diagnostics.Process.Start(Path + "DaemonLog.mht");
        }

        public static void BuildUnicode(string File)
        {
            string[] classes = { "Lu", "Ll", "Lt", "Lm", "Lo", "L", "Mn", "Mc", "Me", "M", "Nd", "Nl", "No", "N", "Pc", "Pd", "Ps", "Pe", "Pi", "Pf", "Po", "P", "Sm", "Sc", "Sk", "So", "S", "Zs", "Zl", "Zp", "Z", "Cc", "Cf", "Cs", "Co", "Cn", "C" };
            List<string> code = new List<string>();
            for (int i = 0; i != classes.Length; i++)
                BuildUnicode_Class(classes[i], code);
            System.IO.File.WriteAllLines(File, code.ToArray());
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
