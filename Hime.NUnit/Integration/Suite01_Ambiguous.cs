using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Hime.Parsers;

namespace Hime.NUnit.Integration
{
    [TestFixture]
    public class Suite01_Ambiguous
    {
    	private Tools tools = new Tools();
    	
        [Test]
        public void Test001_ReturnsFalseOnConflictuousGrammar_LALR1()
        {
            string grammar = "public grammar cf Test { options{ Axiom=\"test\"; } terminals{} rules{ test->a|b; a->'x'; b->'x'; }  }";
            Assert.IsFalse(tools.BuildRawText(grammar, Parsers.ParsingMethod.LALR1));
        }

        [Test]
        public void Test002_ReturnsFalseOnConflictuousGrammar_LR1()
        {
            string grammar = "public grammar cf Test { options{ Axiom=\"test\"; } terminals{} rules{ test->a|b; a->'x'; b->'x'; }  }";
            Assert.IsFalse(this.tools.BuildRawText(grammar, Parsers.ParsingMethod.LR1));
        }

        [Test]
        public void Test003_FindsLALR1AmbiguousAndLR1NonAmbiguous()
        {
            string grammar = "public grammar cf Test { options{ Axiom=\"S\"; } terminals{} rules{ A->'d'; B->'d'; S->A'a'|'b'A'c'|B'c'|'b'B'a'; } }";
            Assert.IsFalse(this.tools.BuildRawText(grammar, Parsers.ParsingMethod.LALR1));
            Assert.IsTrue(this.tools.BuildRawText(grammar, Parsers.ParsingMethod.LR1));
        }

        [Test]
        public void Test004_ShouldNotStackOverflow()
        {
            string grammar = "public grammar cf Test { options{ Axiom=\"X\"; } terminals{} rules{ X -> X; } }";
            CompilationTask task = new CompilationTask();
            task.InputRawData.Add(grammar);
            task.GrammarName = "Test";
            task.Method = Parsers.ParsingMethod.LALR1;
            task.Namespace = "Analyze";
            task.ParserFile = "TestAnalyze.cs";
            task.Execute();
        }

        [Test]
        public void Test005_FindsShiftReduceForLALR1()
        {
            string grammar = "public grammar cf Test { options{ Axiom=\"X\"; } terminals{} rules{ X->'a'X | 'a'X 'b'X;} }";
            Assert.IsFalse(this.tools.BuildRawText(grammar, Parsers.ParsingMethod.LALR1));
        }

        [Test]
        public void Test006_FindsShiftReduceForLR1()
        {
            string grammar = "public grammar cf Test { options{ Axiom=\"X\"; } terminals{} rules{ X->'a'X | 'a'X 'b'X;} }";
            Assert.IsFalse(this.tools.BuildRawText(grammar, Parsers.ParsingMethod.LR1));
        }

        [Test]
        public void Test007_FindsAmbigousGrammarLALR1()
        {
            Assert.IsFalse(this.tools.BuildResource("LALR1-ambiguous.gram", Parsers.ParsingMethod.LALR1));
        }

        [Test]
        public void Test008_FindsAmbigousGrammarLR1()
        {
            Assert.IsFalse(this.tools.BuildResource("LALR1-ambiguous.gram", Parsers.ParsingMethod.LR1));
        }
        
        [Ignore]
        [Test]
        public void Test009_ShouldAcceptWhenNoTerminalsArePresent_Item415()
        {
        	string grammar = 
        		"public grammar cf Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
        	Assert.IsTrue(this.tools.BuildRawText(grammar, Parsers.ParsingMethod.LR0));
        }

        [Ignore]
        [Test]
        public void Test010_ShouldAcceptWhenNoTerminalsArePresent_Item415()
        {
        	string grammar = 
        		"public grammar cf Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
        	Assert.IsTrue(this.tools.BuildRawText(grammar, Parsers.ParsingMethod.LR1));
        }

        // TODO: fix this bug
        [Ignore]
        [Test]
        public void Test011_FindsAmbiguousGrammarLR1_Item418()
        {
        	string grammar = 
        		"public grammar cf Test { options { Axiom=\"exp\"; } terminals {} rules { exp -> 'x' | 'x'; } }";
        	Assert.IsFalse(this.tools.BuildRawText(grammar, Parsers.ParsingMethod.LR1));
        }
        
    }
}
