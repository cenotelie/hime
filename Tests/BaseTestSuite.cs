using System;
using System.IO;
using System.Reflection;
using Hime.CentralDogma;

namespace Hime.Tests
{
    public abstract class BaseTestSuite
    {
        protected const string log = "Log.txt";
        protected const string output = "output";

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
                File.AppendAllText(log, ex.Message + Environment.NewLine);
            }
        }

        protected void SetTestDirectory() {
            System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace();
            System.Diagnostics.StackFrame caller = trace.GetFrame(1);
            string dir = Path.Combine(directory, caller.GetMethod().Name);
            if (Directory.Exists(dir))
                Directory.Delete(dir, true);
            Directory.CreateDirectory(dir);
            Environment.CurrentDirectory = dir;
        }

        protected void ExportResource(string name, string file) { accessor.Export(name, file); }

        //protected string GetResourceContent(string name) { return accessor.GetAllTextFor(name); }
        
        /*protected Report CompileResource(string resource, ParsingMethod method)
        {
			return CompileRaw(GetResourceContent(resource), method);
        }

        protected Report CompileRaw(string rawInput, ParsingMethod method)
        {
            CompilationTask task = new CompilationTask();
            task.Method = method;
            task.AddInputRaw(rawInput);
            task.OutputPrefix = output;
            return task.Execute();
        }
        protected Report CompileRaw(string rawInput, ParsingMethod method, bool log)
        {
            CompilationTask task = new CompilationTask();
            task.Method = method;
            task.AddInputRaw(rawInput);
            task.OutputPrefix = output;
            task.OutputLog = log;
            return task.Execute();
        }

        protected Assembly Build()
        {
            string redist = Assembly.GetAssembly(typeof(TextLexer)).Location;
            using (CodeDomProvider compiler = CodeDomProvider.CreateProvider("C#"))
			{
            	CompilerParameters compilerparams = new CompilerParameters();
            	compilerparams.GenerateExecutable = false;
            	compilerparams.GenerateInMemory = true;
            	compilerparams.ReferencedAssemblies.Add("mscorlib.dll");
            	compilerparams.ReferencedAssemblies.Add("System.dll");
                compilerparams.ReferencedAssemblies.Add(redist);
                compilerparams.EmbeddedResources.Add(output + CompilationTask.PostfixLexerData);
                compilerparams.EmbeddedResources.Add(output + CompilationTask.PostfixParserData);
                CompilerResults results = compiler.CompileAssemblyFromFile(compilerparams, new string[] { output + CompilationTask.PostfixLexerCode, output + CompilationTask.PostfixParserCode });
                foreach (CompilerError error in results.Errors)
                    Console.WriteLine(error.ToString());
                Assert.AreEqual(0, results.Errors.Count);
            	return results.CompiledAssembly;
			}
        }

        protected ASTNode Parse(Assembly assembly, string input, out bool errors)
        {
            Type lexerType = null;
            Type parserType = null;
            Type[] types = assembly.GetTypes();
            for (int i = 0; i != types.Length; i++)
            {
                if (types[i].BaseType == typeof(TextLexer))
                    lexerType = types[i];
                else if (types[i].BaseType == typeof(LRkParser))
                    parserType = types[i];
                else if (types[i].BaseType == typeof(RNGLRParser))
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
            ASTNode root = parser.Parse();
            errors = (parser.Errors.Count != 0);
            return root;
        }*/
    }
}