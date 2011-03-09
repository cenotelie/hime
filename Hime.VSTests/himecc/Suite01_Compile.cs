using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hime.VSTests.himecc
{
    [TestClass]
    public class Suite01_Compile
    {
        private const string p_Directory = "Test";

        private void Generate(string[] command)
        {
            if (System.IO.Directory.Exists(p_Directory))
                System.IO.Directory.Delete(p_Directory, true);
            System.IO.Directory.CreateDirectory(p_Directory);
            Integration.Tools.Export(System.IO.Path.GetFileName(command[0]), command[0]);
            HimeCC.Options options = HimeCC.Program.ParseArguments(command);
            HimeCC.Program.Execute(options);
        }
        private System.Reflection.Assembly Compile(string[] command)
        {
            Generate(command);
            string redist = System.Reflection.Assembly.GetAssembly(typeof(Redist.Parsers.ILexer)).Location;
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

        [TestMethod]
        public void Test001_DefaultNamespace_GeneratedLexer()
        {
            System.Reflection.Assembly assembly = Compile(new string[] { p_Directory + "\\MathExp.gram" });
            System.Type lexer = assembly.GetType("MathExp.MathExp_Lexer");
            Assert.IsNotNull(lexer);
        }

        [TestMethod]
        public void Test002_DefaultNamespace_GeneratedParser()
        {
            System.Reflection.Assembly assembly = Compile(new string[] { p_Directory + "\\MathExp.gram" });
            System.Type lexer = assembly.GetType("MathExp.MathExp_Parser");
            Assert.IsNotNull(lexer);
        }

        [TestMethod]
        public void Test003_DontCrashOnEmptyFile()
        {
            Generate(new string[] { p_Directory + "\\Empty.gram" });
        }
    }
}
