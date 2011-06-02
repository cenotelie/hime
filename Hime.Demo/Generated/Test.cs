using System.Collections.Generic;

namespace Analyser
{
    class Test_Lexer : Hime.Redist.Parsers.LexerText
    {
        private static ushort[] staticSymbolsSID = { 0x6, 0x7, 0x9, 0xA };
        private static string[] staticSymbolsName = { "_T[x]", "_T[.]", "_T[(]", "_T[)]" };
        private static ushort[][] staticTransitions0 = { new ushort[3] { 0x78, 0x78, 0x1 }, new ushort[3] { 0x2E, 0x2E, 0x2 }, new ushort[3] { 0x28, 0x28, 0x3 }, new ushort[3] { 0x29, 0x29, 0x4 } };
        private static ushort[][] staticTransitions1 = {  };
        private static ushort[][] staticTransitions2 = {  };
        private static ushort[][] staticTransitions3 = {  };
        private static ushort[][] staticTransitions4 = {  };
        private static ushort[][][] staticTransitions = { staticTransitions0, staticTransitions1, staticTransitions2, staticTransitions3, staticTransitions4 };
        private static int[] staticFinals = { -1, 0, 1, 2, 3 };
        protected override void setup() {
            symbolsSID = staticSymbolsSID;
            symbolsName = staticSymbolsName;
            symbolsSubGrammars = new Dictionary<ushort, MatchSubGrammar>();
            transitions = staticTransitions;
            finals = staticFinals;
        }
        public override Hime.Redist.Parsers.ILexer Clone() {
            return new Test_Lexer(this);
        }
        public Test_Lexer(string input) : base(new System.IO.StringReader(input)) {}
        public Test_Lexer(System.IO.TextReader input) : base(input) {}
        public Test_Lexer(Test_Lexer original) : base(original) {}
    }
    class Test_Parser : Hime.Redist.Parsers.BaseLRAParser
    {
        private static void Production_3_0 (Hime.Redist.Parsers.BaseLRAParser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x3, "e0"));
            SubRoot.AppendChild(Definition[0]);
            nodes.Add(SubRoot);
        }
        private static void Production_3_1 (Hime.Redist.Parsers.BaseLRAParser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x3, "e0"));
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            nodes.Add(SubRoot);
        }
        private static void Production_4_0 (Hime.Redist.Parsers.BaseLRAParser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x4, "e"));
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            nodes.Add(SubRoot);
        }
        private static void Production_4_1 (Hime.Redist.Parsers.BaseLRAParser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 4, 4);
            nodes.RemoveRange(nodes.Count - 4, 4);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x4, "e"));
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            SubRoot.AppendChild(Definition[3]);
            nodes.Add(SubRoot);
        }
        private static void Production_4_2 (Hime.Redist.Parsers.BaseLRAParser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x4, "e"));
            SubRoot.AppendChild(Definition[0]);
            nodes.Add(SubRoot);
        }
        private static void Production_5_0 (Hime.Redist.Parsers.BaseLRAParser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x5, "t"));
            SubRoot.AppendChild(Definition[0]);
            nodes.Add(SubRoot);
        }
        private static void Production_5_1 (Hime.Redist.Parsers.BaseLRAParser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x5, "t"));
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            nodes.Add(SubRoot);
        }
        private static void Production_10_0 (Hime.Redist.Parsers.BaseLRAParser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x10, "_Axiom_"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static Rule[] staticRules = {
           new Rule(Production_3_0, new Hime.Redist.Parsers.SymbolVariable(0x3, "e0"), 1)
           , new Rule(Production_3_1, new Hime.Redist.Parsers.SymbolVariable(0x3, "e0"), 3)
           , new Rule(Production_4_0, new Hime.Redist.Parsers.SymbolVariable(0x4, "e"), 3)
           , new Rule(Production_4_1, new Hime.Redist.Parsers.SymbolVariable(0x4, "e"), 4)
           , new Rule(Production_4_2, new Hime.Redist.Parsers.SymbolVariable(0x4, "e"), 1)
           , new Rule(Production_5_0, new Hime.Redist.Parsers.SymbolVariable(0x5, "t"), 1)
           , new Rule(Production_5_1, new Hime.Redist.Parsers.SymbolVariable(0x5, "t"), 3)
           , new Rule(Production_10_0, new Hime.Redist.Parsers.SymbolVariable(0x10, "_Axiom_"), 2)
        };
        private static State[] staticStates = {
            new State(
               new string[6] {"[_Axiom_ → • e $, ε]", "[e → • ( e ), $]", "[e → • ( t ) e, $]", "[e → • e0, $]", "[e0 → • x, $/.]", "[e0 → • e0 . x, $/.]"},
               new Terminal[2] {new Terminal("_T[x]", 0x6), new Terminal("_T[(]", 0x9)},
               new DeciderState[3] {
                   new DeciderState(
                   new ushort[2] {0x9, 0x6},
                   new ushort[2] {0x1, 0x2}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x2, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x4, new Rule())
               },
               new ushort[2] {0x4, 0x3},
               new ushort[2] {0x1, 0x3})
            , new State(
               new string[1] {"[_Axiom_ → e • $, ε]"},
               new Terminal[1] {new Terminal("$", 0x2)},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[1] {0x2},
                   new ushort[1] {0x1}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x5, new Rule())
               },
               new ushort[0] {},
               new ushort[0] {})
            , new State(
               new string[9] {"[e → ( • e ), $/)]", "[e → ( • t ) e, $/)]", "[e → • ( e ), )]", "[e → • ( t ) e, )]", "[e → • e0, )]", "[t → • x, ./)]", "[t → • t . x, ./)]", "[e0 → • x, ./)]", "[e0 → • e0 . x, ./)]"},
               new Terminal[2] {new Terminal("_T[x]", 0x6), new Terminal("_T[(]", 0x9)},
               new DeciderState[3] {
                   new DeciderState(
                   new ushort[2] {0x9, 0x6},
                   new ushort[2] {0x1, 0x2}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x2, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x8, new Rule())
               },
               new ushort[3] {0x4, 0x5, 0x3},
               new ushort[3] {0x6, 0x7, 0x3})
            , new State(
               new string[2] {"[e → e0 •, $/)]", "[e0 → e0 • . x, $/./)]"},
               new Terminal[3] {new Terminal("$", 0x2), new Terminal("_T[.]", 0x7), new Terminal("_T[)]", 0xA)},
               new DeciderState[3] {
                   new DeciderState(
                   new ushort[3] {0x2, 0xA, 0x7},
                   new ushort[3] {0x1, 0x1, 0x2}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0x4])
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x9, new Rule())
               },
               new ushort[0] {},
               new ushort[0] {})
            , new State(
               new string[1] {"[e0 → x •, $/./)]"},
               new Terminal[3] {new Terminal("$", 0x2), new Terminal("_T[.]", 0x7), new Terminal("_T[)]", 0xA)},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[3] {0x2, 0x7, 0xA},
                   new ushort[3] {0x1, 0x1, 0x1}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0x0])
               },
               new ushort[0] {},
               new ushort[0] {})
            , new State(
               new string[1] {"[_Axiom_ → e $ •, ε]"},
               new Terminal[1] {new Terminal("ε", 0x1)},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[1] {0x1},
                   new ushort[1] {0x1}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0x7])
               },
               new ushort[0] {},
               new ushort[0] {})
            , new State(
               new string[1] {"[e → ( e • ), $/)]"},
               new Terminal[1] {new Terminal("_T[)]", 0xA)},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[1] {0xA},
                   new ushort[1] {0x1}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xA, new Rule())
               },
               new ushort[0] {},
               new ushort[0] {})
            , new State(
               new string[2] {"[e → ( t • ) e, $/)]", "[t → t • . x, ./)]"},
               new Terminal[2] {new Terminal("_T[.]", 0x7), new Terminal("_T[)]", 0xA)},
               new DeciderState[3] {
                   new DeciderState(
                   new ushort[2] {0xA, 0x7},
                   new ushort[2] {0x1, 0x2}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xB, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xC, new Rule())
               },
               new ushort[0] {},
               new ushort[0] {})
            , new State(
               new string[2] {"[t → x •, ./)]", "[e0 → x •, ./)]"},
               new Terminal[2] {new Terminal("_T[.]", 0x7), new Terminal("_T[)]", 0xA)},
               new DeciderState[7] {
                   new DeciderState(
                   new ushort[2] {0x7, 0xA},
                   new ushort[2] {0x3, 0x6}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0x0])
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0x5])
                   , new DeciderState(
                   new ushort[1] {0x6},
                   new ushort[1] {0x4}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[3] {0x2, 0x7, 0xA},
                   new ushort[3] {0x1, 0x3, 0x5}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[4] {0x6, 0x9, 0x2, 0xA},
                   new ushort[4] {0x2, 0x2, 0x1, 0x1}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[4] {0x6, 0x9, 0x2, 0xA},
                   new ushort[4] {0x2, 0x2, 0x1, 0x1}, 0xFFFF, new Rule())
               },
               new ushort[0] {},
               new ushort[0] {})
            , new State(
               new string[1] {"[e0 → e0 . • x, $/./)]"},
               new Terminal[1] {new Terminal("_T[x]", 0x6)},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[1] {0x6},
                   new ushort[1] {0x1}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xD, new Rule())
               },
               new ushort[0] {},
               new ushort[0] {})
            , new State(
               new string[1] {"[e → ( e ) •, $/)]"},
               new Terminal[2] {new Terminal("$", 0x2), new Terminal("_T[)]", 0xA)},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[2] {0x2, 0xA},
                   new ushort[2] {0x1, 0x1}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0x2])
               },
               new ushort[0] {},
               new ushort[0] {})
            , new State(
               new string[6] {"[e → ( t ) • e, $/)]", "[e → • ( e ), $/)]", "[e → • ( t ) e, $/)]", "[e → • e0, $/)]", "[e0 → • x, $/./)]", "[e0 → • e0 . x, $/./)]"},
               new Terminal[2] {new Terminal("_T[x]", 0x6), new Terminal("_T[(]", 0x9)},
               new DeciderState[3] {
                   new DeciderState(
                   new ushort[2] {0x9, 0x6},
                   new ushort[2] {0x1, 0x2}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x2, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x4, new Rule())
               },
               new ushort[2] {0x4, 0x3},
               new ushort[2] {0xE, 0x3})
            , new State(
               new string[1] {"[t → t . • x, ./)]"},
               new Terminal[1] {new Terminal("_T[x]", 0x6)},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[1] {0x6},
                   new ushort[1] {0x1}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xF, new Rule())
               },
               new ushort[0] {},
               new ushort[0] {})
            , new State(
               new string[1] {"[e0 → e0 . x •, $/./)]"},
               new Terminal[3] {new Terminal("$", 0x2), new Terminal("_T[.]", 0x7), new Terminal("_T[)]", 0xA)},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[3] {0x2, 0x7, 0xA},
                   new ushort[3] {0x1, 0x1, 0x1}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0x1])
               },
               new ushort[0] {},
               new ushort[0] {})
            , new State(
               new string[1] {"[e → ( t ) e •, $/)]"},
               new Terminal[2] {new Terminal("$", 0x2), new Terminal("_T[)]", 0xA)},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[2] {0x2, 0xA},
                   new ushort[2] {0x1, 0x1}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0x3])
               },
               new ushort[0] {},
               new ushort[0] {})
            , new State(
               new string[1] {"[t → t . x •, ./)]"},
               new Terminal[2] {new Terminal("_T[.]", 0x7), new Terminal("_T[)]", 0xA)},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[2] {0x7, 0xA},
                   new ushort[2] {0x1, 0x1}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0x6])
               },
               new ushort[0] {},
               new ushort[0] {})
        };
        protected override void setup()
        {
            rules = staticRules;
            states = staticStates;
            errorSimulationLength = 3;
        }
        public Test_Parser(Test_Lexer lexer) : base (lexer) {}
    }
}
