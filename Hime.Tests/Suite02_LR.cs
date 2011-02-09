using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class Suite02_LR
    {
        private static Hime.Kernel.Reporting.Reporter p_Reporter = new Hime.Kernel.Reporting.Reporter(typeof(Suite02_LR));

        public const string gram01_rules = "E->T Ep;"
                                                + "Ep->'+' T Ep;"
                                                + "Ep->'−' T Ep;"
                                                + "Ep->ε;"
                                                + "T->F Tp;"
                                                + "Tp->'∗' F Tp;"
                                                + "Tp->'/' F Tp;"
                                                + "Tp->ε;"
                                                + "F->'(' E ')';"
                                                + "F->'nb';";
        public const string gram01 = "public grammar cf Test { options{ Axiom=\"E\"; } terminals{} rules{ " + gram01_rules + " }  }";


        public static Hime.Parsers.CF.CFGrammar BuildGrammar(String name, String text)
        {
            Hime.Kernel.Namespace root = Hime.Kernel.Namespace.CreateRoot();
            Hime.Kernel.Resources.ResourceCompiler compiler = new Hime.Kernel.Resources.ResourceCompiler();
            compiler.AddInputRawText(text);
            compiler.Compile(root, p_Reporter);
            return (Hime.Parsers.CF.CFGrammar)root.ResolveName(Hime.Kernel.QualifiedName.ParseName(name));
        }

        [Test]
        public void Test001_ComputersFirsts()
        {
            Dictionary<string, string[]> expected = new Dictionary<string,string[]>();
            expected.Add("E", new string[]{"nb", "("});
            expected.Add("Ep", new string[]{"ε", "+" ,"−"});
            expected.Add("T", new string[]{"nb", "("});
            expected.Add("Tp", new string[]{"ε", "∗" , "/",});
            expected.Add("F", new string[]{ "nb", "(" });

            Hime.Parsers.CF.CFGrammar gram = BuildGrammar("Test", gram01);
            gram.GenerateParser("Test", new Hime.Parsers.CF.LR.MethodLALR1(), "Test.cs", p_Reporter);
            foreach (Hime.Parsers.CF.CFVariable var in gram.Variables)
            {
                if (!expected.ContainsKey(var.LocalName))
                    continue;
                string[] firsts = expected[var.LocalName];
                if (var.Firsts.Count != firsts.Length)
                    Assert.Fail("Firsts set of " + var.LocalName + " does not have a corret size");
                foreach (Hime.Parsers.Terminal term in var.Firsts)
                {
                    string name = term.LocalName;
                    if (name.StartsWith("_T["))
                        name = name.Substring(3, name.Length - 4);
                    bool found = false;
                    for (int i = 0; i != firsts.Length; i++)
                    {
                        if (firsts[i] == name)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                        Assert.Fail("Cannot find " + name + " in firsts of " + var.LocalName);
                }
            }
        }
    }
}
