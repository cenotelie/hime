using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;


namespace Tests
{
    // TODO: rename Suite so that it corresponds to the name of the class that is tested
    [TestFixture]
    public class Suite00_MethodLR1
    {
        private static Hime.Kernel.Reporting.Reporter p_Reporter = new Hime.Kernel.Reporting.Reporter(typeof(Suite00_MethodLR1));

        private bool BuildRawText(string text, Hime.Parsers.CF.CFParserGenerator method)
        {
            Hime.Kernel.Namespace root = Hime.Kernel.Namespace.CreateRoot();
            Hime.Kernel.Resources.ResourceCompiler compiler = new Hime.Kernel.Resources.ResourceCompiler();
            compiler.AddInputRawText(text);
            compiler.Compile(root, p_Reporter);
            Hime.Parsers.Grammar grammar = (Hime.Parsers.Grammar)root.ResolveName(Hime.Kernel.QualifiedName.ParseName("Test"));
            Hime.Parsers.GrammarBuildOptions options = new Hime.Parsers.GrammarBuildOptions(p_Reporter, "Analyzer", method, "TestAnalyze.cs");
            bool result = grammar.Build(options);
            options.Close();
            return result;
        }
        private bool BuildFile(string file, string name, Hime.Parsers.CF.CFParserGenerator method)
        {
            Hime.Kernel.Namespace root = Hime.Kernel.Namespace.CreateRoot();
            Hime.Kernel.Resources.ResourceCompiler compiler = new Hime.Kernel.Resources.ResourceCompiler();
            compiler.AddInputFile(file);
            compiler.Compile(root, p_Reporter);
            Hime.Parsers.Grammar grammar = (Hime.Parsers.Grammar)root.ResolveName(Hime.Kernel.QualifiedName.ParseName(name));
            Hime.Parsers.GrammarBuildOptions options = new Hime.Parsers.GrammarBuildOptions(p_Reporter, "Analyzer", method, "TestAnalyze.cs");
            bool result = grammar.Build(options);
            options.Close();
            return result;
        }

        // TODO: try to move down this test to the level of the method at fault in MethodLR1
        [Test]
        public void Test001_ReturnsFalseOnConflictuousGrammar_LALR1()
        {
            string grammar = "public grammar cf Test { options{ Axiom=\"test\"; } terminals{} rules{ test->a|b; a->'x'; b->'x'; }  }";
            Assert.IsFalse(BuildRawText(grammar, new Hime.Parsers.CF.LR.MethodLALR1()));
        }

        [Test]
        public void Test002_ReturnsFalseOnConflictuousGrammar_LR1()
        {
            string grammar = "public grammar cf Test { options{ Axiom=\"test\"; } terminals{} rules{ test->a|b; a->'x'; b->'x'; }  }";
            Assert.IsFalse(BuildRawText(grammar, new Hime.Parsers.CF.LR.MethodLR1()));
        }

        [Test]
        public void Test003_FindsLALR1AmbiguousAndLR1NonAmbiguous()
        {
            string grammar = "public grammar cf Test { options{ Axiom=\"S\"; } terminals{} rules{ A->'d'; B->'d'; S->A'a'|'b'A'c'|B'c'|'b'B'a'; } }";
            Assert.IsFalse(BuildRawText(grammar, new Hime.Parsers.CF.LR.MethodLALR1()));
            Assert.IsTrue(BuildRawText(grammar, new Hime.Parsers.CF.LR.MethodLR1()));
        }

        [Test]
        public void Test004_FindsShiftReduceForLALR1()
        {
            string grammar = "public grammar cf Test { options{ Axiom=\"X\"; } terminals{} rules{ X->'a'X | 'a'X 'b'X;} }";
            Assert.IsFalse(BuildRawText(grammar, new Hime.Parsers.CF.LR.MethodLALR1()));
        }

        [Test]
        public void Test005_FindsShiftReduceForLR1()
        {
            string grammar = "public grammar cf Test { options{ Axiom=\"X\"; } terminals{} rules{ X->'a'X | 'a'X 'b'X;} }";
            Assert.IsFalse(BuildRawText(grammar, new Hime.Parsers.CF.LR.MethodLR1()));
        }

        [Test]
        public void Test006_FindsAmbigousGrammarLALR1()
        {
            Assert.IsFalse(BuildFile("Languages\\LALR1-ambiguous.gram", "AmbiguousLALR1", new Hime.Parsers.CF.LR.MethodLALR1()));
        }

        [Test]
        public void Test007_FindsAmbigousGrammarLR1()
        {
            Assert.IsFalse(BuildFile("Languages\\LALR1-ambiguous.gram", "AmbiguousLALR1", new Hime.Parsers.CF.LR.MethodLR1()));
        }
    }
}
