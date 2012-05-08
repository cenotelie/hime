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
        
        private void TestGrammar(string dir, string grammar, ParsingMethod method, string input)
        {
            string lexer = System.IO.Path.Combine(dir, "lexer.cs");
            string parser = System.IO.Path.Combine(dir, "parser.cs");
            Assert.IsFalse(CompileRaw(grammar, method, lexer, parser).HasErrors, "Grammar compilation failed!");
            Assembly assembly = Build(lexer, parser);
            bool errors = false;
            SyntaxTreeNode node = Parse(assembly, input, out errors);
            Assert.NotNull(node, "Failed to parse input!");
            Assert.IsFalse(errors, "Parsing errors!");
        }

		[Test]
        public void Test000_SimpleGrammar_LR0()
        {
            string dir = GetTestDirectory();
            TestGrammar(dir, grammar0, ParsingMethod.LR0, "adbc");
        }
		
		[Test]
        public void Test001_SimpleList_LR1()
        {
            string dir = GetTestDirectory();
            TestGrammar(dir, grammar1, ParsingMethod.LR1, "xxx");
        }

        [Test]
        public void Test002_SimpleList_LALR1()
        {
            string dir = GetTestDirectory();
            TestGrammar(dir, grammar1, ParsingMethod.LALR1, "xxx");
        }

		[Test]
        public void Test003_SimpleList_LRStar()
        {
            string dir = GetTestDirectory();
            TestGrammar(dir, grammar1, ParsingMethod.LRStar, "xxx");
        }
    }
}