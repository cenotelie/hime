/*
 * @author Charles Hymans
 * */

using NUnit.Framework;
using Hime.HimeCC;

namespace Hime.Tests.HimeCC
{
    [TestFixture]
    public class Suite03_Program
    {
		[Test]
		public void Test000_Main_ShouldReturnStatus()
		{
			int result = Program.Main(new string[0]);
			Assert.AreEqual(0, result);
		}
    }
}
