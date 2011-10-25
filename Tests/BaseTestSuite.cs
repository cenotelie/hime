using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Hime.Parsers;
using Hime.Kernel.Reporting;
using Hime.Redist.Parsers;
using Hime.Kernel.Resources;

namespace Hime.Tests
{
    public class BaseTestSuite
    {
        protected static string directory = "Test";
        protected static string lexerFile = Path.Combine(directory, "TestLexer.cs");
        protected static string parserFile = Path.Combine(directory, "TestParser.cs");
		
		// TODO: could factor this method with other GetAllTextFor in Tools.cs
        protected string GetAllTextFor(string name)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            ResourceAccessor accessor = new ResourceAccessor(assembly, "Resources");
            string data = accessor.GetAllTextFor(name);
            accessor.Close();
            return data;
        }

        protected Report CompileResource(string resource, ParsingMethod method)
        {
			return CompileRaw(GetAllTextFor(resource), method);
        }

        protected Report CompileRaw(string rawInput, ParsingMethod method)
        {
            CompilationTask task = new Parsers.CompilationTask();
            task.InputRawData.Add(rawInput);
            task.Method = method;
            task.LexerFile = lexerFile;
            task.ParserFile = parserFile;
            return (new Compiler()).Execute(task);
        }

        protected Assembly Build()
        {
            string redist = Assembly.GetAssembly(typeof(Hime.Redist.Parsers.ILexer)).Location;
            File.Copy(redist, Path.Combine(directory, "Hime.Redist.dll"), true);
            System.CodeDom.Compiler.CodeDomProvider compiler = System.CodeDom.Compiler.CodeDomProvider.CreateProvider("C#");
            System.CodeDom.Compiler.CompilerParameters compilerparams = new System.CodeDom.Compiler.CompilerParameters();
            compilerparams.GenerateExecutable = false;
            compilerparams.GenerateInMemory = true;
            compilerparams.ReferencedAssemblies.Add("mscorlib.dll");
            compilerparams.ReferencedAssemblies.Add("System.dll");
            compilerparams.ReferencedAssemblies.Add(Path.Combine(directory, "Hime.Redist.dll"));
            System.CodeDom.Compiler.CompilerResults results = compiler.CompileAssemblyFromFile(compilerparams, new string[] { lexerFile, parserFile });
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
                else if (types[i].BaseType == typeof(LR1BaseParser))
                    parserType = types[i];
                else if (types[i].BaseType == typeof(BaseRNGLR1Parser))
                    parserType = types[i];
                else if (types[i].BaseType == typeof(LRStarBaseParser))
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