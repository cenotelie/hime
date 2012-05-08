using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Hime.Parsers;
using Hime.Utils.Reporting;
using Hime.Redist.Parsers;
using Hime.Utils.Resources;
using System.CodeDom.Compiler;

namespace Hime.Tests
{
    public abstract class BaseTestSuite
    {
        protected ResourceAccessor accessor;
        protected string directory;

        protected BaseTestSuite()
        {
            accessor = new ResourceAccessor(Assembly.GetExecutingAssembly(), "Resources");
            directory = "Data_" + this.GetType().Name;
            try
            {
                if (Directory.Exists(directory))
                    Directory.Delete(directory, true);
                Directory.CreateDirectory(directory);
            }
            catch (IOException ex)
            {
                File.AppendAllText("Log.txt", ex.Message + Environment.NewLine);
            }
        }

        protected string GetTestDirectory() {
            System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace();
            System.Diagnostics.StackFrame caller = trace.GetFrame(1);
            string dir = Path.Combine(directory, caller.GetMethod().Name);
            if (Directory.Exists(dir))
                Directory.Delete(dir, true);
            Directory.CreateDirectory(dir);
            return dir;
        }

        protected string GetResourceContent(string name) { return accessor.GetAllTextFor(name); }
        protected void ExportResource(string name, string file) { accessor.Export(name, file); }

        protected Report CompileResource(string resource, ParsingMethod method, string lexer, string parser)
        {
			return CompileRaw(GetResourceContent(resource), method, lexer, parser);
        }

        protected Report CompileRaw(string rawInput, ParsingMethod method, string lexer, string parser)
        {
            CompilationTask task = new CompilationTask();
            task.Method = method;
            task.InputRawData.Add(rawInput);
            task.LexerFile = lexer;
            task.ParserFile = parser;
            return task.Execute();
        }

        protected Assembly Build(string lexer, string parser)
        {
            string redist = Assembly.GetAssembly(typeof(Hime.Redist.Parsers.LexerText)).Location;
            using (CodeDomProvider compiler = CodeDomProvider.CreateProvider("C#"))
			{
            	CompilerParameters compilerparams = new CompilerParameters();
            	compilerparams.GenerateExecutable = false;
            	compilerparams.GenerateInMemory = true;
            	compilerparams.ReferencedAssemblies.Add("mscorlib.dll");
            	compilerparams.ReferencedAssemblies.Add("System.dll");
                compilerparams.ReferencedAssemblies.Add(redist);
            	CompilerResults results = compiler.CompileAssemblyFromFile(compilerparams, new string[] { lexer, parser });
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
    }
}