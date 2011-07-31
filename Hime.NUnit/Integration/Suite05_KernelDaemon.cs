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
	
namespace Hime.NUnit.Integration
{
	[TestFixture]
	public class Suite05_KernelDaemon
	{
		// TODO: should do this test without really generating (should be able to decide the output Stream instead of the path as a string)
		[Test]
		public void Test000_GenerateNextStep_GeneratesParserForCentralDogma()
		{
			DirectoryInfo currentDirectory = new DirectoryInfo(Environment.CurrentDirectory);
			DirectoryInfo projectRoot = currentDirectory.Parent.Parent.Parent;			
			KernelDaemon.GenerateNextStep(projectRoot.FullName);
		}

		[Test]
		public void Test001_GenerateNextStep_ShouldNotHaveErrors()
		{
			DirectoryInfo currentDirectory = new DirectoryInfo(Environment.CurrentDirectory);
			DirectoryInfo projectRoot = currentDirectory.Parent.Parent.Parent;			
			bool success = KernelDaemon.GenerateNextStep(projectRoot.FullName);
			Assert.IsTrue(success);
		}
		
		// TODO: should check the lexer and parser generated are public
		// TODO: do a test that generation does not fail
		// TODO: make so that the generation can be directly done with one himecc command (instead of calling yet another method)
	}
}
