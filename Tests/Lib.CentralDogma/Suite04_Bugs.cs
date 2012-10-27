using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using NUnit.Framework;
using Hime.Parsers;
using Hime.Utils.Reporting;
using Hime.Redist.Parsers;

namespace Hime.Tests.CentralDogma
{
    [TestFixture]
    public class Suite04_Bugs : BaseTestSuite
    {
        [Test]
        public void Test001_Bug_InlineEndLine()
        {
            string dir = GetTestDirectory();
            string grammar = "cf grammar Test { options{ Axiom=\"test\"; } terminals{} rules{ test->'\\n'; }  }";
            Assert.IsFalse(CompileRaw(grammar, ParsingMethod.LALR1).HasErrors);
            Assert.IsNotNull(Build());
        }
    }
}
