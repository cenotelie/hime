using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hime.HimeV.Model
{
    class Project
    {
        private const string p_TempNamespace = "Hime.HimeV.Temp";
        private const string p_TempFile = "Parser.cs";

        private List<string> p_Files;
        private Hime.Kernel.Namespace p_Root;
        private Hime.Kernel.QualifiedName p_SelectedGram;
        private Hime.Parsers.ParsingMethod p_Method;
        private System.Reflection.Assembly p_CurrentAssembly;

        public Project(string file, string name, Hime.Parsers.ParsingMethod method)
        {
            p_Files = new List<string>();
            p_Files.Add(file);
            p_SelectedGram = Hime.Kernel.QualifiedName.ParseName(name);
            p_Method = method;
        }

        public bool RegenerateParser()
        {
            Cleanup();
            if (!Generate())
                return false;
            return true;
        }

        public Hime.Redist.Parsers.SyntaxTreeNode Parse(string text)
        {
            if (p_CurrentAssembly == null) return null;
            Type LexerType = p_CurrentAssembly.GetType(p_TempNamespace + "." + p_SelectedGram.NakedName + "_Lexer");
            Type ParserType = p_CurrentAssembly.GetType(p_TempNamespace + "." + p_SelectedGram.NakedName + "_Parser");
            Type ActionType = ParserType.GetNestedType("Actions");
            System.Reflection.ConstructorInfo LexerConstructor = LexerType.GetConstructor(new Type[] { typeof(string) });
            System.Reflection.ConstructorInfo ParserConstructor = ParserType.GetConstructor(new Type[] { ActionType, LexerType });

            object Lexer = LexerConstructor.Invoke(new object[] { text });
            Hime.Redist.Parsers.IParser Parser = (Hime.Redist.Parsers.IParser)ParserConstructor.Invoke(new object[] { null, Lexer });
            return Parser.Analyse();
        }

        private void Cleanup()
        {
            p_CurrentAssembly = null;
        }

        private bool Generate()
        {
            Parsers.CompilationTask Task = new Parsers.CompilationTask(p_Files.ToArray(), p_SelectedGram.ToString(), p_Method, p_TempNamespace, p_TempFile, false, false);
            Kernel.Reporting.Report Report = Task.Execute();
            foreach (Kernel.Reporting.Section section in Report.Sections)
                foreach (Kernel.Reporting.Entry entry in section.Entries)
                    if (entry.Level == Kernel.Reporting.Level.Error)
                        return false;
            string code = System.IO.File.ReadAllText(p_TempFile);
            System.CodeDom.Compiler.CodeDomProvider compiler = System.CodeDom.Compiler.CodeDomProvider.CreateProvider("C#");
            System.CodeDom.Compiler.CompilerParameters compilerparams = new System.CodeDom.Compiler.CompilerParameters();
            compilerparams.GenerateExecutable = false;
            compilerparams.GenerateInMemory = true;
            compilerparams.ReferencedAssemblies.Add("Hime.Redist.dll");
            System.CodeDom.Compiler.CompilerResults results = compiler.CompileAssemblyFromSource(compilerparams, code);
            p_CurrentAssembly = results.CompiledAssembly;
            System.IO.File.Delete(p_TempFile);
            return (!results.Errors.HasErrors);
        }
    }
}
