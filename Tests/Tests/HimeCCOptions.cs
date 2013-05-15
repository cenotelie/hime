using NUnit.Framework;

namespace Hime.Tests
{
    [TestFixture]
    public class HimeCCOptions : BaseTestSuite
    {
        [Test]
        public void Test_NoOptions()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result);
        }

        [Test]
        public void Test_HelpLong() { Test_Help("-h"); }
        [Test]
        public void Test_HelpShort() { Test_Help("-h"); }
        private void Test_Help(string param)
        {
            SetTestDirectory();
            int result = Hime.HimeCC.Program.Main(new string[] { param });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result);
        }

        [Test]
        public void Test_RegenerateShort() { Test_Regeneration("-r"); }
        [Test]
        public void Test_RegenerateLong() { Test_Regeneration("--regenerate"); }
        private void Test_Regeneration(string param)
        {
            SetTestDirectory();
            int result = Hime.HimeCC.Program.Main(new string[] { param });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result);
            Assert.IsTrue(CheckFile("CommandLineLexer.cs"));
            Assert.IsTrue(CheckFile("CommandLineLexer.bin"));
            Assert.IsTrue(CheckFile("CommandLineParser.cs"));
            Assert.IsTrue(CheckFile("CommandLineParser.bin"));
            Assert.IsTrue(CheckFile("FileCentralDogmaLexer.cs"));
            Assert.IsTrue(CheckFile("FileCentralDogmaLexer.bin"));
            Assert.IsTrue(CheckFile("FileCentralDogmaParser.cs"));
            Assert.IsTrue(CheckFile("FileCentralDogmaParser.bin"));
        }

        [Test]
        public void Test_OutputModeDefault()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result);
            Assert.IsTrue(CheckFile("MathExpLexer.cs"));
            Assert.IsTrue(CheckFile("MathExpLexer.bin"));
            Assert.IsTrue(CheckFile("MathExpParser.cs"));
            Assert.IsTrue(CheckFile("MathExpParser.bin"));
            Assert.IsFalse(CheckFile("MathExp.dll"));
        }

        [Test]
        public void Test_OutputModeAssembly()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram -o:assembly" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result);
            Assert.IsTrue(CheckFile("MathExpLexer.cs"));
            Assert.IsTrue(CheckFile("MathExpLexer.bin"));
            Assert.IsTrue(CheckFile("MathExpParser.cs"));
            Assert.IsTrue(CheckFile("MathExpParser.bin"));
            Assert.IsTrue(CheckFile("MathExp.dll"));
        }

        [Test]
        public void Test_OutputModeNoSources()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram -o:nosources" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result);
            Assert.IsFalse(CheckFile("MathExpLexer.cs"));
            Assert.IsFalse(CheckFile("MathExpLexer.bin"));
            Assert.IsFalse(CheckFile("MathExpParser.cs"));
            Assert.IsFalse(CheckFile("MathExpParser.bin"));
            Assert.IsTrue(CheckFile("MathExp.dll"));
        }

        [Test]
        public void Test_GrammarName1()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            ExportResource("TestScript.gram", "TestScript.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram TestScript.gram -g MathExp" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result);
            Assert.IsTrue(CheckFile("MathExpLexer.cs"));
            Assert.IsTrue(CheckFile("MathExpLexer.bin"));
            Assert.IsTrue(CheckFile("MathExpParser.cs"));
            Assert.IsTrue(CheckFile("MathExpParser.bin"));
        }

        [Test]
        public void Test_GrammarName2()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            ExportResource("TestScript.gram", "TestScript.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram TestScript.gram -g MathExp" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result);
            Assert.IsTrue(CheckFile("MathExpLexer.cs"));
            Assert.IsTrue(CheckFile("MathExpLexer.bin"));
            Assert.IsTrue(CheckFile("MathExpParser.cs"));
            Assert.IsTrue(CheckFile("MathExpParser.bin"));
        }

        [Test]
        public void Test_Prefix_FileName()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram -p XXX" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result);
            Assert.IsTrue(CheckFile("XXXLexer.cs"));
            Assert.IsTrue(CheckFile("XXXLexer.bin"));
            Assert.IsTrue(CheckFile("XXXParser.cs"));
            Assert.IsTrue(CheckFile("XXXParser.bin"));
        }

        [Test]
        public void Test_Prefix_Directory()
        {
            SetTestDirectory();
            System.IO.Directory.CreateDirectory("YYY");
            string prefix = System.IO.Path.Combine("YYY", "XXX");
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram -p " + prefix });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result);
            Assert.AreEqual(4, System.IO.Directory.GetFiles("YYY").Length);
            Assert.IsTrue(CheckFile(prefix + "Lexer.cs"));
            Assert.IsTrue(CheckFile(prefix + "Lexer.bin"));
            Assert.IsTrue(CheckFile(prefix + "Parser.cs"));
            Assert.IsTrue(CheckFile(prefix + "Parser.bin"));
        }

        [Test]
        public void Test_MethodDefault()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram -o:assembly" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result);
            Assert.IsTrue(CheckFile("MathExp.dll"));
            System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFile(System.IO.Path.Combine(System.Environment.CurrentDirectory, "MathExp.dll"));
            System.Type type = assembly.GetType("MathExp.MathExpParser");
            Assert.IsNotNull(type);
            Assert.AreEqual(type.BaseType, typeof(Hime.Redist.Parsers.LRkParser));
        }

        [Test]
        public void Test_MethodRNGLR()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram -o:assembly -m:rnglr" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result);
            Assert.IsTrue(CheckFile("MathExp.dll"));
            System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFile(System.IO.Path.Combine(System.Environment.CurrentDirectory, "MathExp.dll"));
            System.Type type = assembly.GetType("MathExp.MathExpParser");
            Assert.IsNotNull(type);
            Assert.AreEqual(type.BaseType, typeof(Hime.Redist.Parsers.RNGLRParser));
        }

        [Test]
        public void Test_NamespaceDefault()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram -o:assembly" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result);
            Assert.IsTrue(CheckFile("MathExp.dll"));
            System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFile(System.IO.Path.Combine(System.Environment.CurrentDirectory, "MathExp.dll"));
            System.Type type = assembly.GetType("MathExp.MathExpParser");
            Assert.IsNotNull(type);
        }

        [Test]
        public void Test_NamespaceUserDefined()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram -o:assembly -n Hime.Tests.Generated" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result);
            Assert.IsTrue(CheckFile("MathExp.dll"));
            System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFile(System.IO.Path.Combine(System.Environment.CurrentDirectory, "MathExp.dll"));
            System.Type type = assembly.GetType("Hime.Tests.Generated.MathExpParser");
            Assert.IsNotNull(type);
        }

        [Test]
        public void Test_AccessDefault()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram -o:assembly" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result);
            Assert.IsTrue(CheckFile("MathExp.dll"));
            System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFile(System.IO.Path.Combine(System.Environment.CurrentDirectory, "MathExp.dll"));
            System.Type type = assembly.GetType("MathExp.MathExpParser");
            Assert.IsNotNull(type);
            Assert.IsFalse(type.IsVisible);
        }

        [Test]
        public void Test_AccessPublic()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram -o:assembly -a:public" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result);
            Assert.IsTrue(CheckFile("MathExp.dll"));
            System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFile(System.IO.Path.Combine(System.Environment.CurrentDirectory, "MathExp.dll"));
            System.Type type = assembly.GetType("MathExp.MathExpParser");
            Assert.IsNotNull(type);
            Assert.IsTrue(type.IsPublic);
            Assert.IsTrue(type.IsVisible);
        }

        [Test]
        public void Test_LogDefault()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result);
            Assert.IsFalse(CheckFile("MathExpLog.mht"));
        }

        [Test]
        public void Test_LogEnabled()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram -l" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result);
            Assert.IsTrue(CheckFile("MathExpLog.mht"));
        }

        [Test]
        public void Test_DocDefault()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result);
            Assert.IsFalse(System.IO.Directory.Exists("MathExpDoc"));
        }

        [Test]
        public void Test_DocEnabled()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram -d" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result);
            Assert.IsTrue(System.IO.Directory.Exists("MathExpDoc"));
        }
    }
}
