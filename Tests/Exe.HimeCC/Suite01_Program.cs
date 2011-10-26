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
        private static string[] p_DefaultCommand = new string[] { "MyGram.gram" };
        private Program program = new Program();

        [Test]
        public void Test000_ParseArguments_MinimalCommand()
        {
            Options options = this.program.ParseArguments(p_DefaultCommand);
            Assert.IsNotNull(options);
        }

        [Test]
        public void Test001_ParseArguments_DefaultNamespace()
        {
            Options options = this.program.ParseArguments(p_DefaultCommand);
            Assert.IsNull(options.Namespace);
        }

        [Test]
        public void Test002_ParseArguments_DefaultMethod()
        {
            Options options = this.program.ParseArguments(p_DefaultCommand);
            Assert.AreEqual(options.Method, ParsingMethod.RNGLALR1);
        }

        [Test]
        public void Test003_ParseArguments_DefaultLexer()
        {
            Options options = this.program.ParseArguments(p_DefaultCommand);
            Assert.IsNull(options.LexerFile);
        }

        [Test]
        public void Test004_ParseArguments_DefaultParser()
        {
            Options options = this.program.ParseArguments(p_DefaultCommand);
            Assert.IsNull(options.ParserFile);
        }

        [Test]
        public void Test005_ParseArguments_DefaultLogExport()
        {
            Options options = this.program.ParseArguments(p_DefaultCommand);
			CompilationTask task = options.BuildCompilationTask();
            Assert.IsFalse(task.ExportLog);
        }

        [Test]
        public void Test006_ParseArguments_DefaultDocExport()
        {
            Options options = this.program.ParseArguments(p_DefaultCommand);
			CompilationTask task = options.BuildCompilationTask();
            Assert.IsFalse(task.ExportDocumentation);
        }

        [Test]
        public void Test007_ParseArguments_DefaultInput()
        {
            Options options = this.program.ParseArguments(p_DefaultCommand);
            Assert.AreEqual(options.Inputs.Count, 1);
            Assert.AreEqual(options.Inputs[0], p_DefaultCommand[0]);
        }

        [Test]
        public void Test008_ParseArguments_EmptyCommand()
        {
            Options options = this.program.ParseArguments(new string[] { });
            Assert.AreEqual(0, options.Inputs.Count);
        }
		
		[Test]
		public void Test009_Main_ShouldReturnStatus()
		{
			int result = Program.Main(new string[0]);
			Assert.AreEqual(0, result);
		}
    }
}
