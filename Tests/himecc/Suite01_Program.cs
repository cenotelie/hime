using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using Hime.HimeCC;
using Hime.Parsers;

namespace Hime.NUnit.himecc
{
    [TestFixture]
    public class Suite01_Program
    {
        private static string[] p_DefaultCommand = new string[] { "MyGram.gram" };
        private Program program = new Program();

        [Test]
        public void Test000_MinimalCommand()
        {
            HimeCC.Options options = this.program.ParseArguments(p_DefaultCommand);
            Assert.IsNotNull(options);
        }

        [Test]
        public void Test001_DefaultNamespace()
        {
            HimeCC.Options options = this.program.ParseArguments(p_DefaultCommand);
            Assert.IsNull(options.Namespace);
        }

        [Test]
        public void Test002_DefaultMethod()
        {
            HimeCC.Options options = this.program.ParseArguments(p_DefaultCommand);
            Assert.AreEqual(options.Method, ParsingMethod.RNGLALR1);
        }

        [Test]
        public void Test003_DefaultLexer()
        {
            HimeCC.Options options = this.program.ParseArguments(p_DefaultCommand);
            Assert.IsNull(options.LexerFile);
        }

        [Test]
        public void Test004_DefaultParser()
        {
            HimeCC.Options options = this.program.ParseArguments(p_DefaultCommand);
            Assert.IsNull(options.ParserFile);
        }

        [Test]
        public void Test005_DefaultLogExport()
        {
            HimeCC.Options options = this.program.ParseArguments(p_DefaultCommand);
            Assert.IsFalse(options.ExportHTMLLog);
        }

        [Test]
        public void Test006_DefaultDocExport()
        {
            HimeCC.Options options = this.program.ParseArguments(p_DefaultCommand);
            Assert.IsFalse(options.ExportDocumentation);
        }

        [Test]
        public void Test007_DefaultInput()
        {
            HimeCC.Options options = this.program.ParseArguments(p_DefaultCommand);
            Assert.AreEqual(options.Inputs.Count, 1);
            Assert.AreEqual(options.Inputs[0], p_DefaultCommand[0]);
        }

        [Test]
        public void Test008_EmptyCommand()
        {
            HimeCC.Options options = this.program.ParseArguments(new string[] { });
            Assert.AreEqual(0, options.Inputs.Count);
        }
    }
}
