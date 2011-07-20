using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using NUnit.Framework;
using Hime.Parsers;
using Hime.Kernel.Reporting;
using Hime.Redist.Parsers;

namespace Hime.NUnit.Integration
{
    [TestFixture]
    public class Suite02_Parse : BaseTestSuite
    {
        private const string grammar1 = "public grammar cf Test { options{ Axiom=\"test\"; } terminals{} rules{ test->'x'*; } }";

        private void TestGrammar1(ParsingMethod method, string input)
        {
            Assert.IsFalse(CompileRaw(grammar1, method).HasErrors());
            Assembly assembly = Build();
            SyntaxTreeNode node = Parse(assembly, input);
            Assert.NotNull(node);
            Assert.AreEqual(node.Children.Count, input.Length);
        }

        [Test]
        public void Test001_SimpleList_LR1()
        {
            TestGrammar1(ParsingMethod.LR1, "xxx");
        }

        [Test]
        public void Test002_SimpleList_LALR1()
        {
            TestGrammar1(ParsingMethod.LALR1, "xxx");
        }

        [Test]
        public void Test003_SimpleList_LRStar()
        {
            TestGrammar1(ParsingMethod.LRStar, "xxx");
        }
    }
}