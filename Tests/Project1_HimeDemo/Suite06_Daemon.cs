/*
 * Author: Charles Hymans
 * Date: 30/07/2011
 * Time: 15:33
 * 
 */
using System;
using System.IO;
using NUnit.Framework;
using Hime.Demo.Tasks;
using Hime.Kernel.Reporting;
using Hime.Parsers;

namespace Hime.Tests.Project1_HimeDemo
{
	[TestFixture]
	public class Suite06_Daemon
	{
		private Daemon daemon;
		
		[SetUp]
		public void SetUp() 
		{
			string currentDirectory = Environment.CurrentDirectory;
			DirectoryInfo projectRoot = new DirectoryInfo(currentDirectory).Parent.Parent.Parent;
			string inputPath = Path.Combine(projectRoot.FullName, "Lib.CentralDogma");
			inputPath = Path.Combine(inputPath, "Kernel");
			inputPath = Path.Combine(inputPath, "Generated");
			inputPath = Path.Combine(inputPath, "SourceGrammar");
			string outputPath = Path.Combine(currentDirectory, "Daemon");
			this.daemon = new Daemon(inputPath, outputPath);
		}
		
		// TODO: should do this test without really generating (should be able to decide the output Stream instead of the path as a string)
		[Test]
		public void Test000_GenerateNextStep_GeneratesParserForCentralDogma()
		{
			this.daemon.GenerateNextStep();
		}
		
		[Test, Ignore]
		public void Test001_GenerateNextStep_ShouldNotHaveErrors()
		{
			bool success = this.daemon.GenerateNextStep();
			Assert.IsTrue(success);
		}
		
		// TODO: should check the lexer and parser generated are public
		// TODO: do a test that generation does not fail
		// TODO: make so that the generation can be directly done with one himecc command (instead of calling yet another method)
		// TODO: does strange thing if last line of rules is commented out with //
		// TODO: should replace all return codes with exceptions
	}
}
