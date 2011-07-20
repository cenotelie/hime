using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using NUnit.Framework;
using Hime.Parsers;
using Hime.Kernel.Reporting;
using Hime.Redist.Parsers;

namespace Hime.NUnit
{
    public class BaseTestSuite
    {
        protected string GetAllTextFor(string resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string defaultPath = "Resources";
            Kernel.Resources.ResourceAccessor accessor = new Kernel.Resources.ResourceAccessor(assembly, defaultPath);
            string data = accessor.GetAllTextFor(resourceName);
            accessor.Close();
            return data;
        }

        protected Report CompileResource(string resource, ParsingMethod method)
        {
            CompilationTask task = new Parsers.CompilationTask();
            task.InputRawData.Add(GetAllTextFor(resource));
            task.Method = method;
            task.ParserFile = "Test.cs";
            Report report = task.Execute();
            return report;
        }

        protected Report CompileRaw(string rawInput, ParsingMethod method)
        {
            CompilationTask task = new Parsers.CompilationTask();
            task.InputRawData.Add(rawInput);
            task.Method = method;
            task.ParserFile = "Test.cs";
            Report report = task.Execute();
            return report;
        }

        protected Assembly Build()
        {
            string redist = Assembly.GetAssembly(typeof(Redist.Parsers.ILexer)).Location;
            System.IO.File.Copy(redist, "Hime.Redist.dll", true);
            string code = System.IO.File.ReadAllText("Test.cs");
            System.CodeDom.Compiler.CodeDomProvider compiler = System.CodeDom.Compiler.CodeDomProvider.CreateProvider("C#");
            System.CodeDom.Compiler.CompilerParameters compilerparams = new System.CodeDom.Compiler.CompilerParameters();
            compilerparams.GenerateExecutable = false;
            compilerparams.GenerateInMemory = true;
            compilerparams.ReferencedAssemblies.Add("mscorlib.dll");
            compilerparams.ReferencedAssemblies.Add("System.dll");
            compilerparams.ReferencedAssemblies.Add("Hime.Redist.dll");
            System.CodeDom.Compiler.CompilerResults results = compiler.CompileAssemblyFromSource(compilerparams, code);
            if (results.Errors.Count != 0)
                Assert.Fail(results.Errors[0].ToString());
            return results.CompiledAssembly;
        }

        protected SyntaxTreeNode Parse(Assembly assembly, string input, out bool errors)
        {
            Type lexerType = null;
            Type parserType = null;
            Type[] types = assembly.GetTypes();
            for (int i = 0; i != types.Length; i++)
            {
                if (types[i].BaseType == typeof(LexerText))
                    lexerType = types[i];
                else if (types[i].BaseType == typeof(LR0TextParser))
                    parserType = types[i];
                else if (types[i].BaseType == typeof(LR1TextParser))
                    parserType = types[i];
                else if (types[i].BaseType == typeof(BaseRNGLR1Parser))
                    parserType = types[i];
                else if (types[i].BaseType == typeof(BaseLRStarParser))
                    parserType = types[i];
            }
            Type actionType = parserType.GetNestedType("Actions");
            ConstructorInfo lexerConstructor = lexerType.GetConstructor(new Type[] { typeof(string) });
            ConstructorInfo parserConstructor = null;
            if (actionType == null)
                parserConstructor = parserType.GetConstructor(new Type[] { lexerType });
            else
                parserConstructor = parserType.GetConstructor(new Type[] { lexerType, actionType });

            object lexer = lexerConstructor.Invoke(new object[] { input });
            IParser parser = null;
            if (actionType == null)
                parser = parserConstructor.Invoke(new object[] { lexer }) as IParser;
            else
                parser = parserConstructor.Invoke(new object[] { lexer, null }) as IParser;
            SyntaxTreeNode root = parser.Analyse();
            errors = (parser.Errors.Count != 0);
            return root;
        }
    }
}