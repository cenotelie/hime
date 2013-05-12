using NUnit.Framework;

namespace Hime.Tests
{
    [TestFixture]
    public class HimeCCReturn : BaseTestSuite
    {
        [Test]
        public void Test_NoArgument()
        {
            int result = Hime.HimeCC.Program.Main(null);
            Assert.AreEqual(result, Hime.HimeCC.Program.ResultOK);
            result = Hime.HimeCC.Program.Main(new string[0]);
            Assert.AreEqual(result, Hime.HimeCC.Program.ResultOK);
        }

        [Test]
        public void Test_GibberishCommandLine()
        {
            int result = Hime.HimeCC.Program.Main(new string[] { "'\"ç" });
            Assert.AreEqual(result, Hime.HimeCC.Program.ResultErrorParsingArgs);
        }

        [Test]
        public void Test_UnknownArgument()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram -x aa" });
            Assert.AreEqual(result, Hime.HimeCC.Program.ResultErrorBadArgs);
            Assert.IsFalse(System.IO.File.Exists("MathExpLexer.cs"));
            Assert.IsFalse(System.IO.File.Exists("MathExpLexer.bin"));
            Assert.IsFalse(System.IO.File.Exists("MathExpParser.cs"));
            Assert.IsFalse(System.IO.File.Exists("MathExpParser.bin"));
        }

        [Test]
        public void Test_CompilationError()
        {
            SetTestDirectory();
            ExportResource("Error.gram", "Error.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "Error.gram" });
            Assert.AreEqual(result, Hime.HimeCC.Program.ResultErrorCompiling);
            Assert.IsFalse(System.IO.File.Exists("ErrorLexer.cs"));
            Assert.IsFalse(System.IO.File.Exists("ErrorLexer.bin"));
            Assert.IsFalse(System.IO.File.Exists("ErrorParser.cs"));
            Assert.IsFalse(System.IO.File.Exists("ErrorParser.bin"));
        }

        [Test]
        public void Test_NominalCompilation()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram" });
            Assert.AreEqual(result, Hime.HimeCC.Program.ResultOK);
            Assert.IsTrue(System.IO.File.Exists("MathExpLexer.cs"));
            Assert.IsTrue(System.IO.File.Exists("MathExpLexer.bin"));
            Assert.IsTrue(System.IO.File.Exists("MathExpParser.cs"));
            Assert.IsTrue(System.IO.File.Exists("MathExpParser.bin"));
        }

        [Test]
        public void Test_HelpSwitch()
        {
            SetTestDirectory();
            int result = Hime.HimeCC.Program.Main(new string[] { "--help" });
            Assert.AreEqual(result, Hime.HimeCC.Program.ResultOK);
        }

        [Test]
        public void Test_Regeneration()
        {
            SetTestDirectory();
            int result = Hime.HimeCC.Program.Main(new string[] { "--regenerate" });
            Assert.AreEqual(result, Hime.HimeCC.Program.ResultOK);
            Assert.IsTrue(System.IO.File.Exists("CommandLineLexer.cs"));
            Assert.IsTrue(System.IO.File.Exists("CommandLineLexer.bin"));
            Assert.IsTrue(System.IO.File.Exists("CommandLineParser.cs"));
            Assert.IsTrue(System.IO.File.Exists("CommandLineParser.bin"));
        }
    }
}
