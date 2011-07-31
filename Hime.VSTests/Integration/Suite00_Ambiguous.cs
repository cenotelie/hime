using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hime.VSTests.Integration
{
    [TestClass]
    public class Suite00_Ambiguous
    {
        [TestMethod]
        public void Test001_ReturnsFalseOnConflictuousGrammar_LALR1()
        {
            string grammar = "public cf text grammar Test { options{ Axiom=\"test\"; } terminals{} rules{ test->a|b; a->'x'; b->'x'; }  }";
            Assert.IsFalse(Tools.BuildRawText(grammar, Parsers.ParsingMethod.LALR1));
        }

        [TestMethod]
        public void Test002_ReturnsFalseOnConflictuousGrammar_LR1()
        {
            string grammar = "public cf text grammar Test { options{ Axiom=\"test\"; } terminals{} rules{ test->a|b; a->'x'; b->'x'; }  }";
            Assert.IsFalse(Tools.BuildRawText(grammar, Parsers.ParsingMethod.LR1));
        }

        [TestMethod]
        public void Test003_FindsLALR1AmbiguousAndLR1NonAmbiguous()
        {
            string grammar = "public cf text grammar Test { options{ Axiom=\"S\"; } terminals{} rules{ A->'d'; B->'d'; S->A'a'|'b'A'c'|B'c'|'b'B'a'; } }";
            Assert.IsFalse(Tools.BuildRawText(grammar, Parsers.ParsingMethod.LALR1));
            Assert.IsTrue(Tools.BuildRawText(grammar, Parsers.ParsingMethod.LR1));
        }

        [TestMethod]
        public void Test004_FindsShiftReduceForLALR1()
        {
            string grammar = "public cf text grammar Test { options{ Axiom=\"X\"; } terminals{} rules{ X->'a'X | 'a'X 'b'X;} }";
            Assert.IsFalse(Tools.BuildRawText(grammar, Parsers.ParsingMethod.LALR1));
        }

        [TestMethod]
        public void Test005_FindsShiftReduceForLR1()
        {
            string grammar = "public cf text grammar Test { options{ Axiom=\"X\"; } terminals{} rules{ X->'a'X | 'a'X 'b'X;} }";
            Assert.IsFalse(Tools.BuildRawText(grammar, Parsers.ParsingMethod.LR1));
        }

        [TestMethod]
        public void Test006_FindsAmbigousGrammarLALR1()
        {
            Assert.IsFalse(Tools.BuildResource("LALR1-ambiguous.gram", "AmbiguousLALR1", Parsers.ParsingMethod.LALR1));
        }

        [TestMethod]
        public void Test007_FindsAmbigousGrammarLR1()
        {
            Assert.IsFalse(Tools.BuildResource("LALR1-ambiguous.gram", "AmbiguousLALR1", Parsers.ParsingMethod.LR1));
        }
    }
}
