using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using System.Reflection;
using Hime.NUnit.Integration;

namespace Hime.NUnit.himecc
{
    [TestFixture]
    public class Suite01_Compile
    {
        private const string p_Directory = "Test";

        private void Generate(string[] command)
        {
            if (System.IO.Directory.Exists(p_Directory))
                System.IO.Directory.Delete(p_Directory, true);
            System.IO.Directory.CreateDirectory(p_Directory);
            new Tools().Export(System.IO.Path.GetFileName(command[0]), command[0]);
            HimeCC.Options options = HimeCC.Program.ParseArguments(command);
            HimeCC.Program.Execute(options);
        }
        
        private Assembly Compile(string[] command)
        {
            Generate(command);
            string redist = Assembly.GetAssembly(typeof(Redist.Parsers.ILexer)).Location;
            System.IO.File.Copy(redist, p_Directory + "\\Hime.Redist.dll");
            string code = System.IO.File.ReadAllText(command[0].Replace(".gram", ".cs"));
            System.CodeDom.Compiler.CodeDomProvider compiler = System.CodeDom.Compiler.CodeDomProvider.CreateProvider("C#");
            System.CodeDom.Compiler.CompilerParameters compilerparams = new System.CodeDom.Compiler.CompilerParameters();
            compilerparams.GenerateExecutable = false;
            compilerparams.GenerateInMemory = true;
            compilerparams.ReferencedAssemblies.Add(p_Directory + "\\Hime.Redist.dll");
            System.CodeDom.Compiler.CompilerResults results = compiler.CompileAssemblyFromSource(compilerparams, code);
            System.Reflection.Assembly assembly = results.CompiledAssembly;
            System.IO.Directory.Delete(p_Directory, true);
            return assembly;
        }

        // TODO: factor all calls to new Tools (inherit from Tools and call this TestTemplate)
        // TODO: or move the code in Tools into real classes
        [Test]
        public void Test000_Generate_ShouldNotFailBecauseResourceIsNotEmbedded()
        {
        	if (System.IO.Directory.Exists(p_Directory))
                System.IO.Directory.Delete(p_Directory, true);
            System.IO.Directory.CreateDirectory(p_Directory);
            string fileName = "MathExp.gram";
            string command = p_Directory + "\\" + fileName;
            new Tools().Export(fileName, command);
        }
        
        [Test]
        public void Test001_DefaultNamespace_GeneratedLexer()
        {
            System.Reflection.Assembly assembly = Compile(new string[] { p_Directory + "\\MathExp.gram" });
            System.Type lexer = assembly.GetType("MathExp.MathExp_Lexer");
            Assert.IsNotNull(lexer);
        }

        [Test]
        public void Test002_DefaultNamespace_GeneratedParser()
        {
            System.Reflection.Assembly assembly = Compile(new string[] { p_Directory + "\\MathExp.gram" });
            System.Type lexer = assembly.GetType("MathExp.MathExp_Parser");
            Assert.IsNotNull(lexer);
        }

        [Test]
        public void Test003_DontCrashOnEmptyFile()
        {
            Generate(new string[] { p_Directory + "\\Empty.gram" });
        }
    }
}
