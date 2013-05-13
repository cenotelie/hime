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
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result);
            result = Hime.HimeCC.Program.Main(new string[0]);
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result);
        }

        [Test]
        public void Test_GibberishCommandLine()
        {
            int result = Hime.HimeCC.Program.Main(new string[] { "'\"ç" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultErrorParsingArgs, result);
        }

        [Test]
        public void Test_UnknownArgument()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram -x aa" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultErrorBadArgs, result);
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
            Assert.AreEqual(Hime.HimeCC.Program.ResultErrorCompiling, result);
            Assert.IsTrue(System.IO.File.Exists("ErrorLexer.cs"));
            Assert.IsTrue(System.IO.File.Exists("ErrorLexer.bin"));
            Assert.IsTrue(System.IO.File.Exists("ErrorParser.cs"));
            Assert.IsTrue(System.IO.File.Exists("ErrorParser.bin"));
        }

        [Test]
        public void Test_NominalCompilation()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result);
            Assert.IsTrue(System.IO.File.Exists("MathExpLexer.cs"));
            Assert.IsTrue(System.IO.File.Exists("MathExpLexer.bin"));
            Assert.IsTrue(System.IO.File.Exists("MathExpParser.cs"));
            Assert.IsTrue(System.IO.File.Exists("MathExpParser.bin"));
        }
    }
}
