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
    public class Suite03_Ambiguous : BaseTestSuite
    {
        private Report DoCompile(string grammar, ParsingMethod method, string dir)
        {
            string lexer = "lexer.cs";
            string parser = "parser.cs";
            return CompileRaw(grammar, method, lexer, parser);
        }

    	[Test]
        public void Test001_ConflictuousGrammar_LALR1()
        {
            string dir = GetTestDirectory();
            string grammar = "cf grammar Test { options{ Axiom=\"test\"; } terminals{} rules{ test->a|b; a->'x'; b->'x'; }  }";
            Assert.IsTrue(DoCompile(grammar, ParsingMethod.LALR1, dir).HasErrors);
        }

        [Test]
        public void Test002_ConflictuousGrammar_LR1()
        {
            string dir = GetTestDirectory();
            string grammar = "cf grammar Test { options{ Axiom=\"test\"; } terminals{} rules{ test->a|b; a->'x'; b->'x'; }  }";
            Assert.IsTrue(DoCompile(grammar, ParsingMethod.LR1, dir).HasErrors);
        }

        // TODO: think about it, but it seems this test sometimes fails, but not always!!!
        // do this test inside a loop
        [Test]
        public void Test003_FindsLALR1AmbiguousAndLR1NonAmbiguous()
        {
            string dir = GetTestDirectory();
            string grammar = "cf grammar Test { options{ Axiom=\"S\"; } terminals{} rules{ A->'d'; B->'d'; S->A'a'|'b'A'c'|B'c'|'b'B'a'; } }";
            Assert.IsTrue(DoCompile(grammar, ParsingMethod.LALR1, dir).HasErrors);
            Assert.IsFalse(DoCompile(grammar, ParsingMethod.LR1, dir).HasErrors);
        }

        [Test]
        public void Test004_ShouldNotStackOverflow()
        {
            string dir = GetTestDirectory();
            string grammar = "cf grammar Test { options{ Axiom=\"X\"; } terminals{} rules{ X -> X; } }";
            Assert.IsTrue(DoCompile(grammar, ParsingMethod.LALR1, dir).HasErrors);
        }

        [Test]
        public void Test005_FindsShiftReduceForLALR1()
        {
            string dir = GetTestDirectory();
            string grammar = "cf grammar Test { options{ Axiom=\"X\"; } terminals{} rules{ X->'a'X | 'a'X 'b'X;} }";
            Assert.IsTrue(DoCompile(grammar, ParsingMethod.LALR1, dir).HasErrors);
        }

        [Test]
        public void Test006_FindsShiftReduceForLR1()
        {
            string dir = GetTestDirectory();
            string grammar = "cf grammar Test { options{ Axiom=\"X\"; } terminals{} rules{ X->'a'X | 'a'X 'b'X;} }";
            Assert.IsTrue(DoCompile(grammar, ParsingMethod.LR1, dir).HasErrors);
        }

        [Test]
        public void Test007_FindsAmbigousGrammarLALR1()
        {
            string dir = GetTestDirectory();
            string lexer = "lexer.cs";
            string parser = "parser.cs";
            Assert.IsTrue(CompileResource("Lib.CentralDogma.AmbiguousLR1.gram", ParsingMethod.LALR1, lexer, parser).HasErrors);
        }

        [Test]
        public void Test008_FindsAmbigousGrammarLR1()
        {
            string dir = GetTestDirectory();
            string lexer = "lexer.cs";
            string parser = "parser.cs";
            Assert.IsTrue(CompileResource("Lib.CentralDogma.AmbiguousLR1.gram", ParsingMethod.LR1, lexer, parser).HasErrors);
        }
        
        [Test]
        public void Test009_ShouldAcceptWhenNoTerminalsArePresent_Item415()
        {
            string dir = GetTestDirectory();
            string grammar = "cf grammar Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
            Assert.IsFalse(DoCompile(grammar, ParsingMethod.LR0, dir).HasErrors);
        }

        [Test]
        public void Test010_ShouldAcceptWhenNoTerminalsArePresent_Item415()
        {
            string dir = GetTestDirectory();
            string grammar = "cf grammar Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
            Assert.IsFalse(DoCompile(grammar, ParsingMethod.LR1, dir).HasErrors);
        }

        // TODO: fix this bug
        [Test]
        public void Test011_FindsAmbiguousGrammarLR1_Item418()
        {
            string dir = GetTestDirectory();
            string grammar = "cf grammar Test { options { Axiom=\"exp\"; } terminals {} rules { exp -> 'x' | 'x'; } }";
            Assert.IsFalse(DoCompile(grammar, ParsingMethod.LR1, dir).HasErrors);
        }
    }
}
