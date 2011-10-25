/*
 * Author: Charles Hymans
 * Date: 30/07/2011
 * Time: 15:33
 * 
 */
using System;
using System.IO;
using NUnit.Framework;
using Hime.Kernel;
	
namespace Hime.Tests.Integration
{
	[TestFixture, Ignore]
	public class Suite06_KernelDaemon
	{
		private string path;
		
		[SetUp]
		public void SetUp() 
		{
			DirectoryInfo currentDirectory = new DirectoryInfo(Environment.CurrentDirectory);
			DirectoryInfo projectRoot = currentDirectory.Parent.Parent.Parent;
			path = Path.Combine(Path.Combine(projectRoot.FullName, "Hime.NUnit"), "bin");

		}
		
		// TODO: should do this test without really generating (should be able to decide the output Stream instead of the path as a string)
		[Test]
		public void Test000_GenerateNextStep_GeneratesParserForCentralDogma()
		{
			Daemon.GenerateNextStep(this.path);
		}

		[Test]
		public void Test001_GenerateNextStep_ShouldNotHaveErrors()
		{
			bool success = Daemon.GenerateNextStep(this.path);
			Assert.IsTrue(success);
		}
		
		// TODO: should check the lexer and parser generated are public
		// TODO: do a test that generation does not fail
		// TODO: make so that the generation can be directly done with one himecc command (instead of calling yet another method)
		// TODO: does strange thing if last line of rules is commented out with //
		// TODO: should replace all return codes with exceptions
	}
}
