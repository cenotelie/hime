using System;
using System.Text;
using System.Collections.Generic;
using NUnit.Framework;
using Hime.HimeCC;
using Hime.Parsers;

namespace Hime.Tests.HimeCC
{
    [TestFixture]
    public class Suite01_Program
    {
        private static string[] defaultCommand = new string[] { "MyGram.gram" };
        private Options options = new Options();
		// TODO: move the tests on a suite about Options
        [Test]
        public void Test000_ParseArguments_MinimalCommand()
        {
            this.options.FillFromArguments(defaultCommand);
            Assert.IsNotNull(this.options);
        }

        [Test]
        public void Test001_ParseArguments_DefaultNamespace()
        {
            this.options.FillFromArguments(defaultCommand);
            Assert.IsNull(this.options.Namespace);
        }

        [Test]
        public void Test002_ParseArguments_DefaultMethod()
        {
            this.options.FillFromArguments(defaultCommand);
            Assert.AreEqual(this.options.Method, ParsingMethod.RNGLALR1);
        }

        [Test]
        public void Test003_ParseArguments_DefaultLexer()
        {
            this.options.FillFromArguments(defaultCommand);
            Assert.IsNull(this.options.LexerFile);
        }

        [Test]
        public void Test004_ParseArguments_DefaultParser()
        {
            this.options.FillFromArguments(defaultCommand);
            Assert.IsNull(this.options.ParserFile);
        }

        [Test]
        public void Test005_ParseArguments_DefaultLogExport()
        {
            this.options.FillFromArguments(defaultCommand);
			CompilationTask task = this.options.BuildCompilationTask();
            Assert.IsFalse(task.ExportLog);
        }

        [Test]
        public void Test006_ParseArguments_DefaultDocExport()
        {
            this.options.FillFromArguments(defaultCommand);
			CompilationTask task = this.options.BuildCompilationTask();
            Assert.IsFalse(task.ExportDocumentation);
        }

        [Test]
        public void Test007_ParseArguments_DefaultInput()
        {
            this.options.FillFromArguments(defaultCommand);
            Assert.AreEqual(this.options.Inputs.Count, 1);
            Assert.AreEqual(this.options.Inputs[0], defaultCommand[0]);
        }

        [Test]
        public void Test008_ParseArguments_EmptyCommand()
        {
            this.options.FillFromArguments(new string[] { });
            Assert.AreEqual(0, this.options.Inputs.Count);
        }
		
		[Test]
		public void Test009_Main_ShouldReturnStatus()
		{
			int result = Program.Main(new string[0]);
			Assert.AreEqual(0, result);
		}
    }
}
