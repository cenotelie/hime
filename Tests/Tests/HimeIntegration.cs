using NUnit.Framework;

namespace Hime.Tests
{
    [TestFixture]
    public class HimeIntegration : BaseTestSuite
    {
        [Test]
        public void Test_MathExp()
        {
            SetTestDirectory();
            Assert.IsTrue(TestInput("MathExp", "2 + 5", "PLUS(NUMBER=\"2\" NUMBER=\"5\")"));
        }
    }
}
