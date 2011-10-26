/*
 * @author Charles Hymans
 * */

using System;
using System.IO;
using NUnit.Framework;
using Hime.HimeCC;

namespace Hime.Tests.HimeCC
{
    [TestFixture]
    public class Suite02_Program
    {
		[Test]
		public void Test000_Main_ShouldReturnStatus()
		{
			int result = Program.Main(new string[0]);
			Assert.AreEqual(0, result);
		}
		
		[Test]
        public void Test001_Main_ShouldNotFail()
        {
			/*
			string outputDirectory = "Suite03_Program";
            if (Directory.Exists("Suite03_Program")) Directory.Delete("Suite03_Program", true);
            Directory.CreateDirectory("Suite03_Program");
            , "--lexer", lexerFile, "--parser", parserFile
            */
			
			DirectoryInfo sourceDirectory = new DirectoryInfo(Environment.CurrentDirectory);
			sourceDirectory = sourceDirectory.Parent.Parent;
			string source = Path.Combine(sourceDirectory.FullName, "Resources");
			source = Path.Combine(source, "MathExp.gram");
			System.Console.WriteLine(source);
			
			string[] command = new String[] { source };
            Assert.AreEqual(0, Program.Main(command));
        }
    }
}
