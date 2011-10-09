using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using System.Reflection;
using Hime.NUnit.Integration;
using Hime.HimeCC;
using System.IO;
	
namespace Hime.NUnit.himecc
{
    [TestFixture]
    public class Suite02_Compile
    {
        private static string directory = "Test";
        private static string source = Path.Combine(directory, "MathExp.gram");
        private static string lexerFile = Path.Combine(directory, "MathExpLexer.cs");
        private static string parserFile = Path.Combine(directory, "MathExpParser.cs");
      	
        private void Generate(string[] command)
        {
            if (System.IO.Directory.Exists(directory))
                System.IO.Directory.Delete(directory, true);
            System.IO.Directory.CreateDirectory(directory);
            new Tools().Export(Path.GetFileName(command[0]), command[0]);
            Program.Main(command);
        }
        
        private Assembly Compile()
        {
            string[] command = new String[] { source, "--lexer", lexerFile, "--parser", parserFile };
	       	Generate(command);
            string redist = Assembly.GetAssembly(typeof(Redist.Parsers.ILexer)).Location;
            File.Copy(redist, directory + "\\Hime.Redist.dll");
            System.CodeDom.Compiler.CodeDomProvider compiler = System.CodeDom.Compiler.CodeDomProvider.CreateProvider("C#");
            System.CodeDom.Compiler.CompilerParameters compilerparams = new System.CodeDom.Compiler.CompilerParameters();
            compilerparams.GenerateExecutable = false;
            compilerparams.GenerateInMemory = true;
            compilerparams.ReferencedAssemblies.Add(directory + "\\Hime.Redist.dll");
            System.CodeDom.Compiler.CompilerResults results = compiler.CompileAssemblyFromFile(compilerparams, new string[] { lexerFile, parserFile });
            System.Reflection.Assembly assembly = results.CompiledAssembly;
            System.IO.Directory.Delete(directory, true);
            return assembly;
        }

        // TODO: factor all calls to new Tools (inherit from Tools and call this TestTemplate)
        // TODO: or move the code in Tools into real classes
        [Test]
        public void Test000_Generate_ShouldNotFailBecauseResourceIsNotEmbedded()
        {
        	if (System.IO.Directory.Exists(directory))
                System.IO.Directory.Delete(directory, true);
            System.IO.Directory.CreateDirectory(directory);
            string fileName = "MathExp.gram";
            string command = directory + "\\" + fileName;
            new Tools().Export(fileName, command);
        }
        
        [Test]
        public void Test001_DefaultNamespace_GeneratedLexer()
        {
            string[] command = new String[] { source, "--lexer", lexerFile, "--parser", parserFile };
        	Generate(command);
            File.ReadAllText(lexerFile);
        }

        [Test]
        public void Test002_DefaultNamespace_GeneratedLexer()
        {
        	Assembly assembly = Compile();
            System.Type lexer = assembly.GetType("MathExp.MathExpLexer");
            Assert.IsNotNull(lexer);
        }

        [Test]
        public void Test003_Compile_ShouldNotFail()
        {
        	this.Compile();
        }

		[Test]
        public void Test004_DefaultNamespace_GeneratedParser()
        {
        	Assembly assembly = Compile();
            Type parser = assembly.GetType("MathExp.MathExpParser");
            Assert.IsNotNull(parser);
        }

        [Test]
        public void Test005_DontCrashOnEmptyFile()
        {
        	Generate(new string[] { Path.Combine(directory, "Empty.gram") });
        }
    }
}
