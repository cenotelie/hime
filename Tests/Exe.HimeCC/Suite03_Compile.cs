/*
 * @author Charles Hymans
 * */

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Hime.HimeCC;
using Hime.Kernel.Reporting;
using Hime.Parsers;
using NUnit.Framework;

namespace Hime.Tests.HimeCC
{
    [TestFixture]
    public class Suite03_Compile: BaseTestSuite
    {
        private static string source = Path.Combine(directory, "MathExp.gram");
      	
        private void Generate(string[] command)
        {
            if (Directory.Exists(directory)) Directory.Delete(directory, true);
            Directory.CreateDirectory(directory);
            Export(Path.GetFileName(command[0]), command[0]);
            Program.Main(command);
        }
        
        private Assembly Compile()
        {
            string[] command = new String[] { source, "--lexer", lexerFile, "--parser", parserFile };
	       	Generate(command);
            string redist = Assembly.GetAssembly(typeof(Hime.Redist.Parsers.ILexer)).Location;
			string redistPath = Path.Combine(directory, "Hime.Redist.dll");
			if (File.Exists(redistPath)) File.Delete(redistPath);
			File.Copy(redist, redistPath);
            using (CodeDomProvider compiler = CodeDomProvider.CreateProvider("C#"))
			{
	            CompilerParameters compilerparams = new CompilerParameters();
    	        compilerparams.GenerateExecutable = false;
        	    compilerparams.GenerateInMemory = true;
            	compilerparams.ReferencedAssemblies.Add(redistPath);
            	CompilerResults results = compiler.CompileAssemblyFromFile(compilerparams, new string[] { lexerFile, parserFile });
            	Assembly assembly = results.CompiledAssembly;
            	Directory.Delete(directory, true);
            	return assembly;
			}
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
            Export(fileName, command);
        }
        
		[Test]
        public void Test001_DefaultNamespace_GeneratedLexer()
        {
            string[] command = new String[] { source, "--lexer", lexerFile, "--parser", parserFile };
        	Generate(command);
            File.ReadAllText(lexerFile);
        }

        [Test]
        public void Test002_DefaultNamespace_ShouldNotFail()
        {
        	Assembly assembly = Compile();
            assembly.GetType("MathExp.MathExpLexer");
        }

		[Test]
        public void Test004_DefaultNamespace_GeneratedLexer()
        {
        	Assembly assembly = Compile();
            System.Type lexer = assembly.GetType("MathExp.MathExpLexer");
            Assert.IsNotNull(lexer);
        }

        [Test]
        public void Test005_Compile_ShouldNotFail()
        {
        	this.Compile();
        }

		[Test]
        public void Test006_DefaultNamespace_GeneratedParser()
        {
        	Assembly assembly = Compile();
            Type parser = assembly.GetType("MathExp.MathExpParser");
            Assert.IsNotNull(parser);
        }

        [Test]
        public void Test007_DontCrashOnEmptyFile()
        {
        	Generate(new string[] { Path.Combine(directory, "Empty.gram") });
        }

	
		// TODO: should simplify this test by adding a return code to main!!!
		[Test]
        public void Test008_ShouldNotFail()
        {
			string[] command = new string[] { Path.Combine(directory, "Kernel.gram"),
					Path.Combine(directory, "CFGrammars.gram"),
					Path.Combine(directory, "CSGrammars.gram"),
					"-g", "Hime.Kernel.FileCentralDogma", "-m", "LALR"
				};
            if (Directory.Exists(directory)) Directory.Delete(directory, true);
            Directory.CreateDirectory(directory);
            Export(Path.GetFileName(command[0]), command[0]);
			Export(Path.GetFileName(command[1]), command[1]);
			Export(Path.GetFileName(command[2]), command[2]);
			
			int result = Program.Main(command);
			Assert.AreEqual(0, result);
        }
	}
}
