﻿using NUnit.Framework;

namespace Hime.Tests
{
    [TestFixture]
    public class HimeIntegration : BaseTestSuite
    {
        [Test]
        public void Test_MathExp()
        {
            SetTestDirectory();
            TestInput("MathExp", "2 + 5", "PLUS(NUMBER NUMBER)");
        }
    }
}
