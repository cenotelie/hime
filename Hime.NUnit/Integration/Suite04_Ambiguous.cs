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
    public class Suite04_Ambiguous : BaseTestSuite
    {
    	[Test]
        public void Test001_ConflictuousGrammar_LALR1()
        {
            string grammar = "public cf text grammar Test { options{ Axiom=\"test\"; } terminals{} rules{ test->a|b; a->'x'; b->'x'; }  }";
            Assert.IsTrue(CompileRaw(grammar, Parsers.ParsingMethod.LALR1).HasErrors);
        }

        [Test]
        public void Test002_ConflictuousGrammar_LR1()
        {
            string grammar = "public cf text grammar Test { options{ Axiom=\"test\"; } terminals{} rules{ test->a|b; a->'x'; b->'x'; }  }";
            Assert.IsTrue(CompileRaw(grammar, Parsers.ParsingMethod.LR1).HasErrors);
        }

        // TODO: think about it, but it seems this test sometimes fails, but not always!!!
        // do this test inside a loop
        [Test]
        public void Test003_FindsLALR1AmbiguousAndLR1NonAmbiguous()
        {
            string grammar = "public cf text grammar Test { options{ Axiom=\"S\"; } terminals{} rules{ A->'d'; B->'d'; S->A'a'|'b'A'c'|B'c'|'b'B'a'; } }";
            Assert.IsTrue(CompileRaw(grammar, Parsers.ParsingMethod.LALR1).HasErrors);
            Assert.IsFalse(CompileRaw(grammar, Parsers.ParsingMethod.LR1).HasErrors);
        }

        [Test]
        public void Test004_ShouldNotStackOverflow()
        {
            string grammar = "public cf text grammar Test { options{ Axiom=\"X\"; } terminals{} rules{ X -> X; } }";
            Assert.IsTrue(CompileRaw(grammar, Parsers.ParsingMethod.LALR1).HasErrors);
        }

        [Test]
        public void Test005_FindsShiftReduceForLALR1()
        {
            string grammar = "public cf text grammar Test { options{ Axiom=\"X\"; } terminals{} rules{ X->'a'X | 'a'X 'b'X;} }";
            Assert.IsTrue(CompileRaw(grammar, Parsers.ParsingMethod.LALR1).HasErrors);
        }

        [Test]
        public void Test006_FindsShiftReduceForLR1()
        {
            string grammar = "public cf text grammar Test { options{ Axiom=\"X\"; } terminals{} rules{ X->'a'X | 'a'X 'b'X;} }";
            Assert.IsTrue(CompileRaw(grammar, Parsers.ParsingMethod.LR1).HasErrors);
        }

        [Test]
        public void Test007_FindsAmbigousGrammarLALR1()
        {
            Assert.IsTrue(CompileResource("AmbiguousLR1.gram", Parsers.ParsingMethod.LALR1).HasErrors);
        }

        [Test]
        public void Test008_FindsAmbigousGrammarLR1()
        {
            Assert.IsTrue(CompileResource("AmbiguousLR1.gram", Parsers.ParsingMethod.LR1).HasErrors);
        }
        
        [Ignore]
        [Test]
        public void Test009_ShouldAcceptWhenNoTerminalsArePresent_Item415()
        {
        	string grammar = "public cf text grammar Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
            Assert.IsFalse(CompileRaw(grammar, Parsers.ParsingMethod.LR0).HasErrors);
        }

        [Ignore]
        [Test]
        public void Test010_ShouldAcceptWhenNoTerminalsArePresent_Item415()
        {
        	string grammar = "public cf text grammar Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
            Assert.IsFalse(CompileRaw(grammar, Parsers.ParsingMethod.LR1).HasErrors);
        }

        // TODO: fix this bug
        [Test]
        public void Test011_FindsAmbiguousGrammarLR1_Item418()
        {
        	string grammar = "public cf text grammar Test { options { Axiom=\"exp\"; } terminals {} rules { exp -> 'x' | 'x'; } }";
            Assert.IsFalse(CompileRaw(grammar, Parsers.ParsingMethod.LR1).HasErrors);
        }
    }
}
