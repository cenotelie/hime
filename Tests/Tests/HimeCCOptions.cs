using NUnit.Framework;

namespace Hime.Tests
{
    [TestFixture]
    public class HimeCCOptions : BaseTestSuite
    {
        [Test]
        public void Test_DefaultOutput()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram" });
            Assert.AreEqual(result, Hime.HimeCC.Program.ResultOK);
            Assert.IsTrue(System.IO.File.Exists("MathExpLexer.cs"));
            Assert.IsTrue(System.IO.File.Exists("MathExpLexer.bin"));
            Assert.IsTrue(System.IO.File.Exists("MathExpParser.cs"));
            Assert.IsTrue(System.IO.File.Exists("MathExpParser.bin"));
            Assert.IsFalse(System.IO.File.Exists("MathExp.dll"));
        }

        [Test]
        public void Test_OutputModeAssembly()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram -o Assembly" });
            Assert.AreEqual(result, Hime.HimeCC.Program.ResultOK);
            Assert.IsTrue(System.IO.File.Exists("MathExpLexer.cs"));
            Assert.IsTrue(System.IO.File.Exists("MathExpLexer.bin"));
            Assert.IsTrue(System.IO.File.Exists("MathExpParser.cs"));
            Assert.IsTrue(System.IO.File.Exists("MathExpParser.bin"));
        }
    }
}
