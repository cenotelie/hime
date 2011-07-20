using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using System.Reflection;
using Hime.NUnit.Integration;

namespace Hime.NUnit
{
    public class BaseTestSuite
    {
        public string GetAllTextFor(string resourceName)
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            string defaultPath = "Resources";
            Kernel.Resources.ResourceAccessor accessor = new Kernel.Resources.ResourceAccessor(assembly, defaultPath);
            string data = accessor.GetAllTextFor(resourceName);
            accessor.Close();
            return data;
        }

        protected Hime.Kernel.Reporting.Report CompileResource(string resource, Hime.Parsers.ParsingMethod method)
        {
            Hime.Parsers.CompilationTask task = new Parsers.CompilationTask();
            task.InputRawData.Add(GetAllTextFor(resource));
            task.Method = method;
            task.ParserFile = "Test.cs";
            Hime.Kernel.Reporting.Report report = task.Execute();
            return report;
        }

        protected Hime.Kernel.Reporting.Report CompileRaw(string rawInput, Hime.Parsers.ParsingMethod method)
        {
            Hime.Parsers.CompilationTask task = new Parsers.CompilationTask();
            task.InputRawData.Add(rawInput);
            task.Method = method;
            task.ParserFile = "Test.cs";
            Hime.Kernel.Reporting.Report report = task.Execute();
            return report;
        }

        protected Assembly Build()
        {
            string redist = Assembly.GetAssembly(typeof(Redist.Parsers.ILexer)).Location;
            System.IO.File.Copy(redist, "Hime.Redist.dll");
            string code = System.IO.File.ReadAllText("Test.cs");
            System.CodeDom.Compiler.CodeDomProvider compiler = System.CodeDom.Compiler.CodeDomProvider.CreateProvider("C#");
            System.CodeDom.Compiler.CompilerParameters compilerparams = new System.CodeDom.Compiler.CompilerParameters();
            compilerparams.GenerateExecutable = false;
            compilerparams.GenerateInMemory = true;
            compilerparams.ReferencedAssemblies.Add("Hime.Redist.dll");
            System.CodeDom.Compiler.CompilerResults results = compiler.CompileAssemblyFromSource(compilerparams, code);
            return results.CompiledAssembly;
        }

        protected Hime.Redist.Parsers.SyntaxTreeNode Parse(Assembly assembly, string input)
        {
            Type lexerType = null;
            Type parserType = null;
            Type[] types = assembly.GetTypes();
            for (int i = 0; i != types.Length; i++)
            {
                if (types[i].BaseType == typeof(Hime.Redist.Parsers.LexerText))
                    lexerType = types[i];
                else if (types[i].BaseType == typeof(Hime.Redist.Parsers.LR0TextParser))
                    parserType = types[i];
                else if (types[i].BaseType == typeof(Hime.Redist.Parsers.LR1TextParser))
                    parserType = types[i];
                else if (types[i].BaseType == typeof(Hime.Redist.Parsers.BaseRNGLR1Parser))
                    parserType = types[i];
                else if (types[i].BaseType == typeof(Hime.Redist.Parsers.BaseLRStarParser))
                    parserType = types[i];
            }
            Type actionType = parserType.GetNestedType("Actions");
            System.Reflection.ConstructorInfo lexerConstructor = lexerType.GetConstructor(new Type[] { typeof(string) });
            System.Reflection.ConstructorInfo parserConstructor = null;
            if (actionType == null)
                parserConstructor = parserType.GetConstructor(new Type[] { lexerType });
            else
                parserConstructor = parserType.GetConstructor(new Type[] { lexerType, actionType });

            object lexer = lexerConstructor.Invoke(new object[] { input });
            Hime.Redist.Parsers.IParser parser = null;
            if (actionType == null)
                parser = parserConstructor.Invoke(new object[] { lexer }) as Hime.Redist.Parsers.IParser;
            else
                parser = parserConstructor.Invoke(new object[] { lexer, null }) as Hime.Redist.Parsers.IParser;
            return parser.Analyse();
        }
    }
}