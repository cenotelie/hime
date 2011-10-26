/*
 * @author Charles Hymans
 * */

using NUnit.Framework;
using Hime.HimeCC;
using Hime.Parsers;

namespace Hime.Tests.HimeCC
{
    [TestFixture]
    public class Suite01_Options
    {
        private static string[] defaultCommand = new string[] { "MyGram.gram" };
        private Options options = new Options();
		
        [Test]
        public void Test000_BuildCompilationTaskFromArguments_MinimalCommand()
        {
            this.options.BuildCompilationTaskFromArguments(defaultCommand);
            Assert.IsNotNull(this.options);
        }

        [Test]
        public void Test001_BuildCompilationTaskFromArguments_DefaultNamespace()
        {
            CompilationTask task = this.options.BuildCompilationTaskFromArguments(defaultCommand);
            Assert.IsNull(task.Namespace);
        }

        [Test]
        public void Test002_BuildCompilationTaskFromArguments_DefaultMethod()
        {
            CompilationTask task = this.options.BuildCompilationTaskFromArguments(defaultCommand);
            Assert.AreEqual(task.Method, ParsingMethod.RNGLALR1);
        }

        [Test]
        public void Test003_BuildCompilationTaskFromArguments_DefaultLexer()
        {
            CompilationTask task = this.options.BuildCompilationTaskFromArguments(defaultCommand);
            Assert.IsNull(task.LexerFile);
        }

        [Test]
        public void Test004_BuildCompilationTaskFromArguments_DefaultParser()
        {
            CompilationTask task = this.options.BuildCompilationTaskFromArguments(defaultCommand);
            Assert.IsNull(task.ParserFile);
        }

        [Test]
        public void Test005_BuildCompilationTaskFromArguments_DefaultLogExport()
        {
            CompilationTask task = this.options.BuildCompilationTaskFromArguments(defaultCommand);
            Assert.IsFalse(task.ExportLog);
        }

        [Test]
        public void Test006_BuildCompilationTaskFromArguments_DefaultDocExport()
        {
            CompilationTask task = this.options.BuildCompilationTaskFromArguments(defaultCommand);
            Assert.IsFalse(task.ExportDocumentation);
        }

        [Test]
        public void Test007_BuildCompilationTaskFromArguments_DefaultInput()
        {
            CompilationTask task = this.options.BuildCompilationTaskFromArguments(defaultCommand);
            Assert.AreEqual(task.InputFiles.Count, 1);
			foreach (string input in task.InputFiles)
			{
            	Assert.AreEqual(input, defaultCommand[0]);
			}
        }

        [Test]
        public void Test008_BuildCompilationTaskFromArguments_EmptyCommand()
        {
            CompilationTask task = this.options.BuildCompilationTaskFromArguments(new string[] { });
            Assert.IsNull(task);
        }

		[Test]
        public void Test009_BuildCompilationTaskFromArguments_ShouldNotReturnNullOnParserOption()
        {
			string[] command = new string[] { "Test/MathExp.gram", "--lexer", "Test/TestLexer.cs", "--parser", "Test/TestParser.cs" };
            CompilationTask task = this.options.BuildCompilationTaskFromArguments(command);
            Assert.IsNotNull(task);
        }
    }
}