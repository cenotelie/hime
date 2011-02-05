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
        // TODO: try to move down this test to the level of the method at fault in MethodLR1
        [Test]
        public void Test001_ReturnsFalseOnConflictuousGrammar_LALR1()
        {
            Hime.Kernel.Namespace root = Hime.Kernel.Namespace.CreateRoot();
            Hime.Kernel.Resources.ResourceCompiler compiler = new Hime.Kernel.Resources.ResourceCompiler();
            compiler.AddInputRawText("public grammar cf Test { options{ Axiom=\"test\"; } terminals{} rules{ test->a|b; a->'x'; b->'x'; }  }");
            compiler.Compile(root, log4net.LogManager.GetLogger("Tests"));
            Hime.Generators.Parsers.Grammar grammar = (Hime.Generators.Parsers.Grammar)root.ResolveName(Hime.Kernel.QualifiedName.ParseName("Test"));
            bool result = grammar.GenerateParser("Analyzer", Hime.Generators.Parsers.GrammarParseMethod.LALR1, "TestAnalyze.cs", log4net.LogManager.GetLogger("Tests"), false);
            Assert.IsFalse(result);
        }

        [Test]
        public void Test002_ReturnsFalseOnConflictuousGrammar_LR1()
        {
            Hime.Kernel.Namespace root = Hime.Kernel.Namespace.CreateRoot();
            Hime.Kernel.Resources.ResourceCompiler compiler = new Hime.Kernel.Resources.ResourceCompiler();
            compiler.AddInputRawText("public grammar cf Test { options{ Axiom=\"test\"; } terminals{} rules{ test->a|b; a->'x'; b->'x'; }  }");
            compiler.Compile(root, log4net.LogManager.GetLogger("Tests"));
            Hime.Generators.Parsers.Grammar grammar = (Hime.Generators.Parsers.Grammar)root.ResolveName(Hime.Kernel.QualifiedName.ParseName("Test"));
            bool result = grammar.GenerateParser("Analyzer", Hime.Generators.Parsers.GrammarParseMethod.LR1, "TestAnalyze.cs", log4net.LogManager.GetLogger("Tests"), false);
            Assert.IsFalse(result);
        }

        [Test]
        public void Test003_FindsLALR1AmbiguousAndLR1NonAmgiguous()
        {
            Hime.Kernel.Namespace root = Hime.Kernel.Namespace.CreateRoot();
            Hime.Kernel.Resources.ResourceCompiler compiler = new Hime.Kernel.Resources.ResourceCompiler();
            compiler.AddInputRawText("public grammar cf Test { options{ Axiom=\"S\"; } terminals{} rules{ S->'a'A'd'|'a'B'e'|'b'A'e'|'b'B'd'; A->'c'; B->'c';} }");
            compiler.Compile(root, log4net.LogManager.GetLogger("Tests"));
            Hime.Generators.Parsers.Grammar grammar = (Hime.Generators.Parsers.Grammar)root.ResolveName(Hime.Kernel.QualifiedName.ParseName("Test"));
            bool result = grammar.GenerateParser("Analyzer", Hime.Generators.Parsers.GrammarParseMethod.LALR1, "TestAnalyze.cs", log4net.LogManager.GetLogger("Tests"), false);
            Assert.IsFalse(result);
            result = grammar.GenerateParser("Analyzer", Hime.Generators.Parsers.GrammarParseMethod.LR1, "TestAnalyze.cs", log4net.LogManager.GetLogger("Tests"), false);
            Assert.IsTrue(result);
        }

        [Test]
        public void Test004_FindsShiftReduceForLALR1()
        {
            Hime.Kernel.Namespace root = Hime.Kernel.Namespace.CreateRoot();
            Hime.Kernel.Resources.ResourceCompiler compiler = new Hime.Kernel.Resources.ResourceCompiler();
            compiler.AddInputRawText("public grammar cf Test { options{ Axiom=\"X\"; } terminals{} rules{ X->'a'X | 'a'X 'b'X;} }");
            compiler.Compile(root, log4net.LogManager.GetLogger("Tests"));
            Hime.Generators.Parsers.Grammar grammar = (Hime.Generators.Parsers.Grammar)root.ResolveName(Hime.Kernel.QualifiedName.ParseName("Test"));
            bool result = grammar.GenerateParser("Analyzer", Hime.Generators.Parsers.GrammarParseMethod.LALR1, "TestAnalyze.cs", log4net.LogManager.GetLogger("Tests"), false);
            Assert.IsFalse(result);
        }

        [Test]
        public void Test005_FindsShiftReduceForLR1()
        {
            Hime.Kernel.Namespace root = Hime.Kernel.Namespace.CreateRoot();
            Hime.Kernel.Resources.ResourceCompiler compiler = new Hime.Kernel.Resources.ResourceCompiler();
            compiler.AddInputRawText("public grammar cf Test { options{ Axiom=\"X\"; } terminals{} rules{ X->'a'X | 'a'X 'b'X;} }");
            compiler.Compile(root, log4net.LogManager.GetLogger("Tests"));
            Hime.Generators.Parsers.Grammar grammar = (Hime.Generators.Parsers.Grammar)root.ResolveName(Hime.Kernel.QualifiedName.ParseName("Test"));
            bool result = grammar.GenerateParser("Analyzer", Hime.Generators.Parsers.GrammarParseMethod.LR1, "TestAnalyze.cs", log4net.LogManager.GetLogger("Tests"), false);
            Assert.IsFalse(result);
        }

        [Test]
        public void Test006_FindsAmbigousGrammarLALR1()
        {
            Hime.Kernel.Namespace root = Hime.Kernel.Namespace.CreateRoot();
            Hime.Kernel.Resources.ResourceCompiler compiler = new Hime.Kernel.Resources.ResourceCompiler();
            compiler.AddInputFile("Languages\\LALR1-ambiguous.gram");
            compiler.Compile(root, log4net.LogManager.GetLogger("Tests"));
            Hime.Generators.Parsers.Grammar grammar = (Hime.Generators.Parsers.Grammar)root.ResolveName(Hime.Kernel.QualifiedName.ParseName("AmbiguousLALR1"));
            bool result = grammar.GenerateParser("Analyzer", Hime.Generators.Parsers.GrammarParseMethod.LALR1, "TestAnalyze.cs", log4net.LogManager.GetLogger("Tests"), false);
            Assert.IsFalse(result);
        }

        [Test]
        public void Test007_FindsAmbigousGrammarLR1()
        {
            Hime.Kernel.Namespace root = Hime.Kernel.Namespace.CreateRoot();
            Hime.Kernel.Resources.ResourceCompiler compiler = new Hime.Kernel.Resources.ResourceCompiler();
            compiler.AddInputFile("Languages\\LALR1-ambiguous.gram");
            compiler.Compile(root, log4net.LogManager.GetLogger("Tests"));
            Hime.Generators.Parsers.Grammar grammar = (Hime.Generators.Parsers.Grammar)root.ResolveName(Hime.Kernel.QualifiedName.ParseName("AmbiguousLALR1"));
            bool result = grammar.GenerateParser("Analyzer", Hime.Generators.Parsers.GrammarParseMethod.LR1, "TestAnalyze.cs", log4net.LogManager.GetLogger("Tests"), false);
            Assert.IsFalse(result);
        }
    }
}
