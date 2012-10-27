/*
 * @author Charles Hymans
 * */

using System;
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
        public void Test003_BuildCompilationTaskFromArguments_DefaultOutput()
        {
            CompilationTask task = this.options.BuildCompilationTaskFromArguments(defaultCommand);
            Assert.IsNull(task.Output);
        }

        [Test]
        public void Test004_BuildCompilationTaskFromArguments_DefaultLogExport()
        {
            CompilationTask task = this.options.BuildCompilationTaskFromArguments(defaultCommand);
            Assert.IsFalse(task.ExportLog);
        }

        [Test]
        public void Test005_BuildCompilationTaskFromArguments_DefaultDocExport()
        {
            CompilationTask task = this.options.BuildCompilationTaskFromArguments(defaultCommand);
            Assert.IsFalse(task.ExportDocumentation);
        }

        [Test]
        public void Test006_BuildCompilationTaskFromArguments_DefaultInput()
        {
            CompilationTask task = this.options.BuildCompilationTaskFromArguments(defaultCommand);
            Assert.AreEqual(task.InputFiles.Count, 1);
			foreach (string input in task.InputFiles)
			{
            	Assert.AreEqual(input, defaultCommand[0]);
			}
        }

        [Test]
        public void Test007_BuildCompilationTaskFromArguments_EmptyCommand()
        {
            CompilationTask task = this.options.BuildCompilationTaskFromArguments(new string[] { });
            Assert.IsNull(task);
        }

		[Test]
        public void Test008_BuildCompilationTaskFromArguments_ShouldNotReturnNullOnParserOption()
        {
			string[] command = new string[] { "Test/MathExp.gram", "-o", "Test/Test" };
            CompilationTask task = this.options.BuildCompilationTaskFromArguments(command);
            Assert.IsNotNull(task);
        }
		
		[Test]
		public void Test0009_GetUsage_HasGrammarOption()
		{
			string usage = this.options.GetUsage();
			foreach (string line in usage.Split(new string[]{ Environment.NewLine }, StringSplitOptions.None))
			{
				System.Console.WriteLine(line);
				if (line.Contains("g, grammar")) Assert.Pass();
			}
			Assert.Fail();
		}
		
		[Test]
		public void Test010_GetUsage_HasNamespaceOption()
		{
			string usage = this.options.GetUsage();
			foreach (string line in usage.Split(new string[]{ Environment.NewLine }, StringSplitOptions.None))
			{
				if (line.Contains("n, namespace")) Assert.Pass();
			}
			Assert.Fail();
		}
		
		[Test]
		public void Test011_GetUsage_HasMethodOption()
		{
			string usage = this.options.GetUsage();
			foreach (string line in usage.Split(new string[]{ Environment.NewLine }, StringSplitOptions.None))
			{
				if (line.Contains("m, method")) Assert.Pass();
			}
			Assert.Fail();
		}
		
		[Test]
		public void Test012_GetUsage_HasLogOption()
		{
			string usage = this.options.GetUsage();
			foreach (string line in usage.Split(new string[]{ Environment.NewLine }, StringSplitOptions.None))
			{
				if (line.Contains("l, log")) Assert.Pass();
			}
			Assert.Fail();
		}
		
		[Test]
		public void Test013_GetUsage_HasDocOption()
		{
			string usage = this.options.GetUsage();
			foreach (string line in usage.Split(new string[]{ Environment.NewLine }, StringSplitOptions.None))
			{
				if (line.Contains("d, doc")) Assert.Pass();
			}
			Assert.Fail();
		}

        [Test]
        public void Test014_GetUsage_HasOuputOption()
        {
            string usage = this.options.GetUsage();
            foreach (string line in usage.Split(new string[] { Environment.NewLine }, StringSplitOptions.None))
            {
                if (line.Contains("o, output")) Assert.Pass();
            }
            Assert.Fail();
        }
    }
}