using System.Collections.Generic;

namespace Analyser
{
    class Test2_Lexer : Hime.Redist.Parsers.LexerText
    {
        public static readonly Hime.Redist.Parsers.SymbolTerminal[] terminals = {
            new Hime.Redist.Parsers.SymbolTerminal("ε", 0x1),
            new Hime.Redist.Parsers.SymbolTerminal("$", 0x2),
            new Hime.Redist.Parsers.SymbolTerminal("_T[x]", 0x6),
            new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0x7),
            new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x9),
            new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0xA),
            new Hime.Redist.Parsers.SymbolTerminal("_T[typeof]", 0xB) };
        private static State[] staticStates = { 
            new State(new ushort[][] {
                new ushort[3] { 0x78, 0x78, 0x6 },
                new ushort[3] { 0x2E, 0x2E, 0x7 },
                new ushort[3] { 0x28, 0x28, 0x8 },
                new ushort[3] { 0x29, 0x29, 0x9 },
                new ushort[3] { 0x74, 0x74, 0x1 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x79, 0x79, 0x2 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x70, 0x70, 0x3 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x65, 0x65, 0x4 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x6F, 0x6F, 0x5 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x66, 0x66, 0xA }},
                null),
            new State(new ushort[][] {}, terminals[0x2]),
            new State(new ushort[][] {}, terminals[0x3]),
            new State(new ushort[][] {}, terminals[0x4]),
            new State(new ushort[][] {}, terminals[0x5]),
            new State(new ushort[][] {}, terminals[0x6]) };
        protected override void setup() {
            states = staticStates;
            subGrammars = new Dictionary<ushort, MatchSubGrammar>();
        }
        public override Hime.Redist.Parsers.ILexer Clone() {
            return new Test2_Lexer(this);
        }
        public Test2_Lexer(string input) : base(new System.IO.StringReader(input)) {}
        public Test2_Lexer(System.IO.TextReader input) : base(input) {}
        public Test2_Lexer(Test2_Lexer original) : base(original) {}
    }
    class Test2_Parser : Hime.Redist.Parsers.BaseLRStarParser
    {
        public static readonly Hime.Redist.Parsers.SymbolVariable[] variables = {
            new Hime.Redist.Parsers.SymbolVariable(0x3, "e0"), 
            new Hime.Redist.Parsers.SymbolVariable(0x4, "e"), 
            new Hime.Redist.Parsers.SymbolVariable(0x5, "t"), 
            new Hime.Redist.Parsers.SymbolVariable(0x13, "_Axiom_") };
        private static void Production_3_0 (Hime.Redist.Parsers.BaseLRStarParser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(variables[0]);
            SubRoot.AppendChild(Definition[0]);
            nodes.Add(SubRoot);
        }
        private static void Production_3_1 (Hime.Redist.Parsers.BaseLRStarParser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(variables[0]);
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            nodes.Add(SubRoot);
        }
        private static void Production_3_2 (Hime.Redist.Parsers.BaseLRStarParser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(variables[0]);
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            nodes.Add(SubRoot);
        }
        private static void Production_3_3 (Hime.Redist.Parsers.BaseLRStarParser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 4, 4);
            nodes.RemoveRange(nodes.Count - 4, 4);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(variables[0]);
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            SubRoot.AppendChild(Definition[3]);
            nodes.Add(SubRoot);
        }
        private static void Production_4_0 (Hime.Redist.Parsers.BaseLRStarParser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(variables[1]);
            SubRoot.AppendChild(Definition[0]);
            nodes.Add(SubRoot);
        }
        private static void Production_4_1 (Hime.Redist.Parsers.BaseLRStarParser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 4, 4);
            nodes.RemoveRange(nodes.Count - 4, 4);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(variables[1]);
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            SubRoot.AppendChild(Definition[3]);
            nodes.Add(SubRoot);
        }
        private static void Production_5_0 (Hime.Redist.Parsers.BaseLRStarParser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(variables[2]);
            SubRoot.AppendChild(Definition[0]);
            nodes.Add(SubRoot);
        }
        private static void Production_5_1 (Hime.Redist.Parsers.BaseLRStarParser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(variables[2]);
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            nodes.Add(SubRoot);
        }
        private static void Production_13_0 (Hime.Redist.Parsers.BaseLRStarParser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(variables[3]);
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static Rule[] staticRules = {
           new Rule(Production_3_0, variables[0], 1)
           , new Rule(Production_3_1, variables[0], 3)
           , new Rule(Production_3_2, variables[0], 3)
           , new Rule(Production_3_3, variables[0], 4)
           , new Rule(Production_4_0, variables[1], 1)
           , new Rule(Production_4_1, variables[1], 4)
           , new Rule(Production_5_0, variables[2], 1)
           , new Rule(Production_5_1, variables[2], 3)
           , new Rule(Production_13_0, variables[3], 2)
        };
        private static State[] staticStates = {
            new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[3] {Test2_Lexer.terminals[2], Test2_Lexer.terminals[4], Test2_Lexer.terminals[6]},
               new DeciderState[4] {
                   new DeciderState(
                   new ushort[3] {0x9, 0x6, 0xB},
                   new ushort[3] {0x1, 0x2, 0x3}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x3, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x4, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x5, new Rule())
               },
               new ushort[2] {0x4, 0x3},
               new ushort[2] {0x1, 0x2})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[1] {Test2_Lexer.terminals[1]},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[1] {0x2},
                   new ushort[1] {0x1}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x6, new Rule())
               },
               new ushort[0] {},
               new ushort[0] {})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[3] {Test2_Lexer.terminals[1], Test2_Lexer.terminals[3], Test2_Lexer.terminals[5]},
               new DeciderState[3] {
                   new DeciderState(
                   new ushort[3] {0x2, 0xA, 0x7},
                   new ushort[3] {0x1, 0x1, 0x2}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0x4])
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x7, new Rule())
               },
               new ushort[0] {},
               new ushort[0] {})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[3] {Test2_Lexer.terminals[2], Test2_Lexer.terminals[4], Test2_Lexer.terminals[6]},
               new DeciderState[4] {
                   new DeciderState(
                   new ushort[3] {0x6, 0x9, 0xB},
                   new ushort[3] {0x1, 0x2, 0x3}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xA, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x3, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x5, new Rule())
               },
               new ushort[3] {0x5, 0x4, 0x3},
               new ushort[3] {0x8, 0x9, 0x2})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[3] {Test2_Lexer.terminals[1], Test2_Lexer.terminals[3], Test2_Lexer.terminals[5]},
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
               null,
               new Hime.Redist.Parsers.SymbolTerminal[1] {Test2_Lexer.terminals[4]},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[1] {0x9},
                   new ushort[1] {0x1}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xB, new Rule())
               },
               new ushort[0] {},
               new ushort[0] {})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[1] {Test2_Lexer.terminals[0]},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[1] {0x1},
                   new ushort[1] {0x1}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0x8])
               },
               new ushort[0] {},
               new ushort[0] {})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[1] {Test2_Lexer.terminals[2]},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[1] {0x6},
                   new ushort[1] {0x1}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xC, new Rule())
               },
               new ushort[0] {},
               new ushort[0] {})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[2] {Test2_Lexer.terminals[3], Test2_Lexer.terminals[5]},
               new DeciderState[3] {
                   new DeciderState(
                   new ushort[2] {0xA, 0x7},
                   new ushort[2] {0x1, 0x2}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xD, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xE, new Rule())
               },
               new ushort[0] {},
               new ushort[0] {})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[1] {Test2_Lexer.terminals[5]},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[1] {0xA},
                   new ushort[1] {0x1}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xF, new Rule())
               },
               new ushort[0] {},
               new ushort[0] {})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[2] {Test2_Lexer.terminals[3], Test2_Lexer.terminals[5]},
               new DeciderState[6] {
                   new DeciderState(
                   new ushort[2] {0x7, 0xA},
                   new ushort[2] {0x1, 0x2}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[1] {0x6},
                   new ushort[1] {0x3}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[6] {0x9, 0x6, 0xB, 0x2, 0x7, 0xA},
                   new ushort[6] {0x4, 0x4, 0x4, 0x5, 0x5, 0x5}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[3] {0x2, 0x7, 0xA},
                   new ushort[3] {0x5, 0x1, 0x2}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0x6])
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0x0])
               },
               new ushort[0] {},
               new ushort[0] {})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[1] {Test2_Lexer.terminals[2]},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[1] {0x6},
                   new ushort[1] {0x1}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x11, new Rule())
               },
               new ushort[1] {0x5},
               new ushort[1] {0x10})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[3] {Test2_Lexer.terminals[1], Test2_Lexer.terminals[3], Test2_Lexer.terminals[5]},
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
               null,
               new Hime.Redist.Parsers.SymbolTerminal[3] {Test2_Lexer.terminals[2], Test2_Lexer.terminals[4], Test2_Lexer.terminals[6]},
               new DeciderState[4] {
                   new DeciderState(
                   new ushort[3] {0x9, 0x6, 0xB},
                   new ushort[3] {0x1, 0x2, 0x3}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x3, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x4, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x5, new Rule())
               },
               new ushort[2] {0x4, 0x3},
               new ushort[2] {0x12, 0x2})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[1] {Test2_Lexer.terminals[2]},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[1] {0x6},
                   new ushort[1] {0x1}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x13, new Rule())
               },
               new ushort[0] {},
               new ushort[0] {})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[3] {Test2_Lexer.terminals[1], Test2_Lexer.terminals[3], Test2_Lexer.terminals[5]},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[3] {0x2, 0x7, 0xA},
                   new ushort[3] {0x1, 0x1, 0x1}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0x2])
               },
               new ushort[0] {},
               new ushort[0] {})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[2] {Test2_Lexer.terminals[3], Test2_Lexer.terminals[5]},
               new DeciderState[3] {
                   new DeciderState(
                   new ushort[2] {0xA, 0x7},
                   new ushort[2] {0x1, 0x2}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x14, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xE, new Rule())
               },
               new ushort[0] {},
               new ushort[0] {})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[2] {Test2_Lexer.terminals[3], Test2_Lexer.terminals[5]},
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
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[2] {Test2_Lexer.terminals[1], Test2_Lexer.terminals[5]},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[2] {0x2, 0xA},
                   new ushort[2] {0x1, 0x1}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0x5])
               },
               new ushort[0] {},
               new ushort[0] {})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[2] {Test2_Lexer.terminals[3], Test2_Lexer.terminals[5]},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[2] {0x7, 0xA},
                   new ushort[2] {0x1, 0x1}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0x7])
               },
               new ushort[0] {},
               new ushort[0] {})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[3] {Test2_Lexer.terminals[1], Test2_Lexer.terminals[3], Test2_Lexer.terminals[5]},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[3] {0x2, 0x7, 0xA},
                   new ushort[3] {0x1, 0x1, 0x1}, 0xFFFF, new Rule())
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0x3])
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
        public Test2_Parser(Test2_Lexer lexer) : base (lexer) {}
    }
}
