using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using System.Reflection;
using Hime.NUnit.Integration;
using Hime.HimeCC;
	
namespace Hime.NUnit.himecc
{
    [TestFixture]
    public class Suite02_Compile
    {
        private const string directory = "Test";

        private void Generate(string command)
        {
            if (System.IO.Directory.Exists(directory))
                System.IO.Directory.Delete(directory, true);
            System.IO.Directory.CreateDirectory(directory);
            new Tools().Export(System.IO.Path.GetFileName(command), command);
            Program program = new Program();
            Options options = program.ParseArguments(new string[] { command });
            program.Execute(options);
        }
        
        private Assembly Compile(string command)
        {
        	Generate(command);
            string redist = Assembly.GetAssembly(typeof(Redist.Parsers.ILexer)).Location;
            System.IO.File.Copy(redist, directory + "\\Hime.Redist.dll");
            string code = System.IO.File.ReadAllText(command.Replace(".gram", ".cs"));
            System.CodeDom.Compiler.CodeDomProvider compiler = System.CodeDom.Compiler.CodeDomProvider.CreateProvider("C#");
            System.CodeDom.Compiler.CompilerParameters compilerparams = new System.CodeDom.Compiler.CompilerParameters();
            compilerparams.GenerateExecutable = false;
            compilerparams.GenerateInMemory = true;
            compilerparams.ReferencedAssemblies.Add(directory + "\\Hime.Redist.dll");
            System.CodeDom.Compiler.CompilerResults results = compiler.CompileAssemblyFromSource(compilerparams, code);
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
            System.Reflection.Assembly assembly = Compile(directory + "\\MathExp.gram");
            System.Type lexer = assembly.GetType("MathExp.MathExp_Lexer");
            Assert.IsNotNull(lexer);
        }

        [Test]
        public void Test002_DefaultNamespace_GeneratedParser()
        {
            System.Reflection.Assembly assembly = Compile(directory + "\\MathExp.gram");
            Type parser = assembly.GetType("MathExp.MathExp_Parser");
            Assert.IsNotNull(parser);
        }

        [Test]
        public void Test003_DontCrashOnEmptyFile()
        {
            Generate(directory + "\\Empty.gram");
        }
    }
}
