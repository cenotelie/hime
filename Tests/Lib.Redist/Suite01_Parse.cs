/*
 * Author: Charles Hymans
 * */
using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using NUnit.Framework;
using Hime.Parsers;
using Hime.Utils.Reporting;
using Hime.Redist.Parsers;
using System.IO;
using System.CodeDom.Compiler;

namespace Hime.Tests.Redist
{
    [TestFixture]
    public class Suite01_Parse : BaseTestSuite
    {
        private const string grammar0 = "cf grammar Test { options{ Axiom=\"S\"; } terminals{} rules{ S->'a'S'b'T|'c'T|'d'; T->'a'T|'b'S|'c'; } }";
        private const string grammar1 = "cf grammar Test { options{ Axiom=\"test\"; } terminals{} rules{ test->'x'*; } }";
        private const string grammar2 = "cf grammar Test { options{ Axiom=\"S\"; } terminals {a->'a'; b->'b';} rules{ S->A b | a b b; A->a; } }";
        private const string grammar3 = "cf grammar Test { options{ Axiom=\"S\"; } terminals {a->'a'; b->'b';} rules{ S->a b A a|a B A a|a b a; A->a|a A; B->b; } }";
        
        private CSTNode TestGrammar(string grammar, ParsingMethod method, string input)
        {
            Assert.IsFalse(CompileRaw(grammar, method).HasErrors, "Grammar compilation failed!");
            Assembly assembly = Build();
            bool errors = false;
            CSTNode node = Parse(assembly, input, out errors);
            Assert.NotNull(node, "Failed to parse input!");
            Assert.IsFalse(errors, "Parsing errors!");
            return node;
        }

		[Test]
        public void Test000_SimpleGrammar_LR0()
        {
            string dir = GetTestDirectory();
            TestGrammar(grammar0, ParsingMethod.LR0, "adbc");
        }
		
		[Test]
        public void Test001_SimpleList_LR1()
        {
            string dir = GetTestDirectory();
            TestGrammar(grammar1, ParsingMethod.LR1, "xxx");
        }

        [Test]
        public void Test002_SimpleList_LALR1()
        {
            string dir = GetTestDirectory();
            TestGrammar(grammar1, ParsingMethod.LALR1, "xxx");
        }

		[Test]
        public void Test003_SimpleList_LRStar()
        {
            string dir = GetTestDirectory();
            TestGrammar(grammar1, ParsingMethod.LRStar, "xxx");
        }

        [Test]
        public void Test004_Simple_RNGLR()
        {
            string dir = GetTestDirectory();
            TestGrammar(grammar2, ParsingMethod.RNGLALR1, "abb");
        }
    }
}