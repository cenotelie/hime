using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hime.VSTests.himecc
{
    [TestClass]
    public class Suite00_DefaultOptions
    {
        private static string[] p_DefaultCommand = new string[] { "MyGram.gram" };

        [TestMethod]
        public void Test000_MinimalCommand()
        {
            HimeCC.Options options = HimeCC.Program.ParseArguments(p_DefaultCommand);
            Assert.IsNotNull(options);
        }

        [TestMethod]
        public void Test001_DefaultNamespace()
        {
            HimeCC.Options options = HimeCC.Program.ParseArguments(p_DefaultCommand);
            Assert.AreEqual(options.Namespace, "MyGram");
        }

        [TestMethod]
        public void Test002_DefaultMethod()
        {
            HimeCC.Options options = HimeCC.Program.ParseArguments(p_DefaultCommand);
            Assert.AreEqual(options.Method, Parsers.ParsingMethod.RNGLALR1);
        }

        [TestMethod]
        public void Test003_DefaultLexer()
        {
            HimeCC.Options options = HimeCC.Program.ParseArguments(p_DefaultCommand);
            Assert.IsNull(options.LexerFile);
        }

        [TestMethod]
        public void Test004_DefaultParser()
        {
            HimeCC.Options options = HimeCC.Program.ParseArguments(p_DefaultCommand);
            Assert.AreEqual(options.ParserFile, "MyGram.cs");
        }

        [TestMethod]
        public void Test005_DefaultLogExport()
        {
            HimeCC.Options options = HimeCC.Program.ParseArguments(p_DefaultCommand);
            Assert.IsFalse(options.ExportHTMLLog);
        }

        [TestMethod]
        public void Test006_DefaultDocExport()
        {
            HimeCC.Options options = HimeCC.Program.ParseArguments(p_DefaultCommand);
        }

        [TestMethod]
        public void Test007_DefaultInput()
        {
            HimeCC.Options options = HimeCC.Program.ParseArguments(p_DefaultCommand);
            Assert.AreEqual(options.Inputs.Count, 1);
            Assert.AreEqual(options.Inputs[0], p_DefaultCommand[0]);
        }

        [TestMethod]
        public void Test008_EmptyCommand()
        {
            HimeCC.Options options = HimeCC.Program.ParseArguments(new string[] { });
            Assert.IsNull(options);
        }
    }
}
