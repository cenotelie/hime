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
using System.CodeDom.Compiler;

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
            using (ResourceAccessor accessor = new ResourceAccessor(assembly, "Resources"))
			{
	            return accessor.GetAllTextFor(name);
			}
        }

        protected Report CompileResource(string resource, ParsingMethod method)
        {
			return CompileRaw(GetAllTextFor(resource), method);
        }

        protected Report CompileRaw(string rawInput, ParsingMethod method)
        {
            CompilationTask task = new CompilationTask(method);
            task.InputRawData.Add(rawInput);
            task.LexerFile = lexerFile;
            task.ParserFile = parserFile;
			Compiler compiler = new Compiler(task);
            return compiler.Execute();
        }

        protected Assembly Build()
        {
            string redist = Assembly.GetAssembly(typeof(Hime.Redist.Parsers.ILexer)).Location;
            File.Copy(redist, Path.Combine(directory, "Hime.Redist.dll"), true);
            using (CodeDomProvider compiler = CodeDomProvider.CreateProvider("C#"))
			{
            	CompilerParameters compilerparams = new CompilerParameters();
            	compilerparams.GenerateExecutable = false;
            	compilerparams.GenerateInMemory = true;
            	compilerparams.ReferencedAssemblies.Add("mscorlib.dll");
            	compilerparams.ReferencedAssemblies.Add("System.dll");
            	compilerparams.ReferencedAssemblies.Add(Path.Combine(directory, "Hime.Redist.dll"));
            	CompilerResults results = compiler.CompileAssemblyFromFile(compilerparams, new string[] { lexerFile, parserFile });
            	Assert.AreEqual(0, results.Errors.Count);
            	return results.CompiledAssembly;
			}
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
                else if (types[i].BaseType == typeof(LR0BaseParser))
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

		// TODO: try to factor all calls to new CompilationTask
        // TODO: remove all static methods
        internal protected void Export(string resourceName, string fileName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string p_DefaultPath = "Resources";
            using (ResourceAccessor accessor = new ResourceAccessor(assembly, p_DefaultPath))
			{
            	accessor.Export(resourceName, fileName);
			}
        }
    }
}