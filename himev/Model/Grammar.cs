using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hime.HimeV.Model
{
    class Grammar
    {
        private const string p_TempNamespace = "Hime.HimeV.Temp";
        private const string p_TempFile = "Parser.cs";

        private string p_Data;
        private Hime.Parsers.ParsingMethod p_Method;
        private System.Reflection.Assembly p_CurrentAssembly;

        public Grammar(string data)
        {
            p_Data = data;
            p_Method = Parsers.ParsingMethod.LALR1;
        }

        public bool GenerateParser()
        {
            Cleanup();
            if (!Generate())
                return false;
            return true;
        }

        public Hime.Redist.Parsers.SyntaxTreeNode Parse(string text)
        {
            if (p_CurrentAssembly == null) return null;
            Type LexerType = null;
            Type ParserType = null;
            Type[] types = p_CurrentAssembly.GetTypes();
            for (int i = 0; i != types.Length; i++)
            {
                if (types[i].BaseType == typeof(Redist.Parsers.LexerText))
                    LexerType = types[i];
                else if (types[i].BaseType == typeof(Redist.Parsers.LR1TextParser))
                    ParserType = types[i];
                else if (types[i].BaseType == typeof(Redist.Parsers.BaseRNGLR1Parser))
                    ParserType = types[i];
            }
            Type ActionType = ParserType.GetNestedType("Actions");
            System.Reflection.ConstructorInfo LexerConstructor = LexerType.GetConstructor(new Type[] { typeof(string) });
            System.Reflection.ConstructorInfo ParserConstructor = null;
            if (ActionType == null)
                ParserConstructor = ParserType.GetConstructor(new Type[] { LexerType });
            else
                ParserConstructor = ParserType.GetConstructor(new Type[] { LexerType, ActionType });

            object Lexer = LexerConstructor.Invoke(new object[] { text });
            Hime.Redist.Parsers.IParser Parser = null;
            if (ActionType == null)
                Parser = (Hime.Redist.Parsers.IParser)ParserConstructor.Invoke(new object[] { Lexer });
            else
                Parser = (Hime.Redist.Parsers.IParser)ParserConstructor.Invoke(new object[] { Lexer, null });
            return Parser.Analyse();
        }

        private void Cleanup()
        {
            p_CurrentAssembly = null;
        }

        private bool Generate()
        {
            string location = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Parsers.CompilationTask Task = new Parsers.CompilationTask();
            //TODO: complete
            //Parsers.CompilationTask.Create(p_Data, null, p_Method, p_TempNamespace, null, p_TempFile, false, false, false);
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
            compilerparams.ReferencedAssemblies.Add(System.IO.Path.Combine(location, "Hime.Redist.dll"));
            System.CodeDom.Compiler.CompilerResults results = compiler.CompileAssemblyFromSource(compilerparams, code);
            System.IO.File.Delete(p_TempFile);
            if (results.Errors.HasErrors)
                return false;
            p_CurrentAssembly = results.CompiledAssembly;
            return true;
        }
    }
}
