using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class Suite02_LR
    {

        public static const string gram01_rules = "E->T Ep;"
                                                + "Ep->'+' T Ep;"
                                                + "Ep->'−' T Ep;"
                                                + "Ep->ε;"
                                                + "T->F Tp;"
                                                + "Tp->'∗' F Tp;"
                                                + "Tp->'/' F Tp;"
                                                + "Tp->ε;"
                                                + "F->'(' E ')';"
                                                + "F->'nb';";
        public static const string gram01 = "public grammar cf Test { options{ Axiom=\"E\"; } terminals{} rules{ " + gram01_rules + " }  }";


        public static Hime.Generators.Parsers.Grammar BuildGrammar(String name, String text)
        {
            Hime.Kernel.Namespace root = Hime.Kernel.Namespace.CreateRoot();
            Hime.Kernel.Resources.ResourceCompiler compiler = new Hime.Kernel.Resources.ResourceCompiler();
            compiler.AddInputRawText(text);
            compiler.Compile(root, Hime.Kernel.Logs.LogConsole.Instance);
            return (Hime.Generators.Parsers.Grammar)root.ResolveName(Hime.Kernel.QualifiedName.ParseName(name));
        }

        [Test]
        public void Test001_ComputersFirsts()
        {
            Hime.Generators.Parsers.Grammar gram = BuildGrammar("test", gram01);
            gram.GenerateParser("Test", Hime.Generators.Parsers.GrammarParseMethod.LALR1, "Test.cs", Hime.Kernel.Logs.LogConsole.Instance);

        }
    }
}
