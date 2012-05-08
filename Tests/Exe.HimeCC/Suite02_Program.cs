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
    public class Suite02_Program : BaseTestSuite
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
            string dir = GetTestDirectory();
            string file = Path.Combine(dir, "MathExp.gram");
            ExportResource("Exe.HimeCC.MathExp.gram", file);
            string[] command = new String[] { file };
            Assert.AreEqual(0, Program.Main(command));
        }
    }
}
