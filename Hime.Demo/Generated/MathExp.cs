using System.Collections.Generic;

namespace Analyser
{
    class MathExp_Lexer : Hime.Redist.Parsers.LexerText
    {
        public static readonly Hime.Redist.Parsers.SymbolTerminal[] terminals = {
            new Hime.Redist.Parsers.SymbolTerminal("ε", 0x1),
            new Hime.Redist.Parsers.SymbolTerminal("$", 0x2),
            new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0xC),
            new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0xD),
            new Hime.Redist.Parsers.SymbolTerminal("_T[*]", 0xE),
            new Hime.Redist.Parsers.SymbolTerminal("_T[/]", 0xF),
            new Hime.Redist.Parsers.SymbolTerminal("_T[+]", 0x10),
            new Hime.Redist.Parsers.SymbolTerminal("_T[-]", 0x11),
            new Hime.Redist.Parsers.SymbolTerminal("NUMBER", 0x5),
            new Hime.Redist.Parsers.SymbolTerminal("SEPARATOR", 0x7) };
        private static State[] staticStates = { 
            new State(new ushort[][] {
                new ushort[3] { 0x28, 0x28, 0x4 },
                new ushort[3] { 0x29, 0x29, 0x5 },
                new ushort[3] { 0x2A, 0x2A, 0x6 },
                new ushort[3] { 0x2F, 0x2F, 0x7 },
                new ushort[3] { 0x2B, 0x2B, 0x8 },
                new ushort[3] { 0x2D, 0x2D, 0x9 },
                new ushort[3] { 0x31, 0x39, 0xA },
                new ushort[3] { 0x30, 0x30, 0xB },
                new ushort[3] { 0x9, 0x9, 0xC },
                new ushort[3] { 0xB, 0xC, 0xC },
                new ushort[3] { 0x20, 0x20, 0xC },
                new ushort[3] { 0x2E, 0x2E, 0x1 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x31, 0x39, 0xE },
                new ushort[3] { 0x30, 0x30, 0xF }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x31, 0x39, 0x10 },
                new ushort[3] { 0x30, 0x30, 0x11 }},
                null),
            new State(new ushort[][] {
                new ushort[3] { 0x2B, 0x2B, 0x2 },
                new ushort[3] { 0x2D, 0x2D, 0x2 },
                new ushort[3] { 0x31, 0x39, 0x10 },
                new ushort[3] { 0x30, 0x30, 0x11 }},
                null),
            new State(new ushort[][] {}, terminals[0x2]),
            new State(new ushort[][] {}, terminals[0x3]),
            new State(new ushort[][] {}, terminals[0x4]),
            new State(new ushort[][] {}, terminals[0x5]),
            new State(new ushort[][] {}, terminals[0x6]),
            new State(new ushort[][] {}, terminals[0x7]),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0xA },
                new ushort[3] { 0x45, 0x45, 0x3 },
                new ushort[3] { 0x65, 0x65, 0x3 },
                new ushort[3] { 0x2E, 0x2E, 0x1 }},
                terminals[0x8]),
            new State(new ushort[][] {
                new ushort[3] { 0x45, 0x45, 0x3 },
                new ushort[3] { 0x65, 0x65, 0x3 },
                new ushort[3] { 0x2E, 0x2E, 0x1 }},
                terminals[0x8]),
            new State(new ushort[][] {
                new ushort[3] { 0x9, 0x9, 0xD },
                new ushort[3] { 0xB, 0xC, 0xD },
                new ushort[3] { 0x20, 0x20, 0xD }},
                terminals[0x9]),
            new State(new ushort[][] {
                new ushort[3] { 0x9, 0x9, 0xD },
                new ushort[3] { 0xB, 0xC, 0xD },
                new ushort[3] { 0x20, 0x20, 0xD }},
                terminals[0x9]),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0xE },
                new ushort[3] { 0x45, 0x45, 0x3 },
                new ushort[3] { 0x65, 0x65, 0x3 }},
                terminals[0x8]),
            new State(new ushort[][] {
                new ushort[3] { 0x45, 0x45, 0x3 },
                new ushort[3] { 0x65, 0x65, 0x3 }},
                terminals[0x8]),
            new State(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x10 }},
                terminals[0x8]),
            new State(new ushort[][] {}, terminals[0x8]) };
        protected override void setup() {
            states = staticStates;
            subGrammars = new Dictionary<ushort, MatchSubGrammar>();
            separatorID = 0x7;
        }
        public override Hime.Redist.Parsers.ILexer Clone() {
            return new MathExp_Lexer(this);
        }
        public MathExp_Lexer(string input) : base(new System.IO.StringReader(input)) {}
        public MathExp_Lexer(System.IO.TextReader input) : base(input) {}
        public MathExp_Lexer(MathExp_Lexer original) : base(original) {}
    }
    class MathExp_Parser : Hime.Redist.Parsers.LR1TextParser
    {
        public static readonly Hime.Redist.Parsers.SymbolVariable[] variables = {
            new Hime.Redist.Parsers.SymbolVariable(0x8, "exp_atom"), 
            new Hime.Redist.Parsers.SymbolVariable(0x9, "exp_op0"), 
            new Hime.Redist.Parsers.SymbolVariable(0xA, "exp_op1"), 
            new Hime.Redist.Parsers.SymbolVariable(0xB, "exp"), 
            new Hime.Redist.Parsers.SymbolVariable(0x12, "_Axiom_") };
        private static void Production_8_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(variables[0]);
            SubRoot.AppendChild(Definition[0]);
            ((MathExp_Parser)parser).actions.OnNumber(SubRoot);
            nodes.Add(SubRoot);
        }
        private static void Production_8_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(variables[0]);
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            nodes.Add(SubRoot);
        }
        private static void Production_9_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(variables[1]);
            SubRoot.AppendChild(Definition[0]);
            nodes.Add(SubRoot);
        }
        private static void Production_9_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(variables[1]);
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            ((MathExp_Parser)parser).actions.OnMult(SubRoot);
            nodes.Add(SubRoot);
        }
        private static void Production_9_2 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(variables[1]);
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            ((MathExp_Parser)parser).actions.OnDiv(SubRoot);
            nodes.Add(SubRoot);
        }
        private static void Production_A_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(variables[2]);
            SubRoot.AppendChild(Definition[0]);
            nodes.Add(SubRoot);
        }
        private static void Production_A_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(variables[2]);
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            ((MathExp_Parser)parser).actions.OnPlus(SubRoot);
            nodes.Add(SubRoot);
        }
        private static void Production_A_2 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(variables[2]);
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            ((MathExp_Parser)parser).actions.OnMinus(SubRoot);
            nodes.Add(SubRoot);
        }
        private static void Production_B_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(variables[3]);
            SubRoot.AppendChild(Definition[0]);
            nodes.Add(SubRoot);
        }
        private static void Production_12_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(variables[4]);
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static Rule[] staticRules = {
           new Rule(Production_8_0, variables[0], 1)
           , new Rule(Production_8_1, variables[0], 3)
           , new Rule(Production_9_0, variables[1], 1)
           , new Rule(Production_9_1, variables[1], 3)
           , new Rule(Production_9_2, variables[1], 3)
           , new Rule(Production_A_0, variables[2], 1)
           , new Rule(Production_A_1, variables[2], 3)
           , new Rule(Production_A_2, variables[2], 3)
           , new Rule(Production_B_0, variables[3], 1)
           , new Rule(Production_12_0, variables[4], 2)
        };
        private static State[] staticStates = {
            new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[2] {MathExp_Lexer.terminals[8], MathExp_Lexer.terminals[2]},
               new ushort[2] {0x5, 0xc},
               new ushort[2] {0x5, 0x6},
               new ushort[4] {0xb, 0xa, 0x9, 0x8},
               new ushort[4] {0x1, 0x2, 0x3, 0x4},
               new Reduction[0] {})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[1] {MathExp_Lexer.terminals[1]},
               new ushort[1] {0x2},
               new ushort[1] {0x7},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[4] {MathExp_Lexer.terminals[1], MathExp_Lexer.terminals[3], MathExp_Lexer.terminals[6], MathExp_Lexer.terminals[7]},
               new ushort[2] {0x10, 0x11},
               new ushort[2] {0x8, 0x9},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[2] {new Reduction(0x2, staticRules[0x8]), new Reduction(0xd, staticRules[0x8])})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[6] {MathExp_Lexer.terminals[1], MathExp_Lexer.terminals[3], MathExp_Lexer.terminals[4], MathExp_Lexer.terminals[5], MathExp_Lexer.terminals[6], MathExp_Lexer.terminals[7]},
               new ushort[2] {0xe, 0xf},
               new ushort[2] {0xA, 0xB},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[4] {new Reduction(0x2, staticRules[0x5]), new Reduction(0xd, staticRules[0x5]), new Reduction(0x10, staticRules[0x5]), new Reduction(0x11, staticRules[0x5])})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[6] {MathExp_Lexer.terminals[1], MathExp_Lexer.terminals[3], MathExp_Lexer.terminals[4], MathExp_Lexer.terminals[5], MathExp_Lexer.terminals[6], MathExp_Lexer.terminals[7]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[6] {new Reduction(0x2, staticRules[0x2]), new Reduction(0xd, staticRules[0x2]), new Reduction(0xe, staticRules[0x2]), new Reduction(0xf, staticRules[0x2]), new Reduction(0x10, staticRules[0x2]), new Reduction(0x11, staticRules[0x2])})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[6] {MathExp_Lexer.terminals[1], MathExp_Lexer.terminals[3], MathExp_Lexer.terminals[4], MathExp_Lexer.terminals[5], MathExp_Lexer.terminals[6], MathExp_Lexer.terminals[7]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[6] {new Reduction(0x2, staticRules[0x0]), new Reduction(0xd, staticRules[0x0]), new Reduction(0xe, staticRules[0x0]), new Reduction(0xf, staticRules[0x0]), new Reduction(0x10, staticRules[0x0]), new Reduction(0x11, staticRules[0x0])})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[2] {MathExp_Lexer.terminals[8], MathExp_Lexer.terminals[2]},
               new ushort[2] {0x5, 0xc},
               new ushort[2] {0x5, 0x6},
               new ushort[4] {0xb, 0xa, 0x9, 0x8},
               new ushort[4] {0xC, 0x2, 0x3, 0x4},
               new Reduction[0] {})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[1] {MathExp_Lexer.terminals[0]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x1, staticRules[0x9])})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[2] {MathExp_Lexer.terminals[8], MathExp_Lexer.terminals[2]},
               new ushort[2] {0x5, 0xc},
               new ushort[2] {0x5, 0x6},
               new ushort[2] {0x9, 0x8},
               new ushort[2] {0xD, 0x4},
               new Reduction[0] {})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[2] {MathExp_Lexer.terminals[8], MathExp_Lexer.terminals[2]},
               new ushort[2] {0x5, 0xc},
               new ushort[2] {0x5, 0x6},
               new ushort[2] {0x9, 0x8},
               new ushort[2] {0xE, 0x4},
               new Reduction[0] {})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[2] {MathExp_Lexer.terminals[8], MathExp_Lexer.terminals[2]},
               new ushort[2] {0x5, 0xc},
               new ushort[2] {0x5, 0x6},
               new ushort[1] {0x8},
               new ushort[1] {0xF},
               new Reduction[0] {})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[2] {MathExp_Lexer.terminals[8], MathExp_Lexer.terminals[2]},
               new ushort[2] {0x5, 0xc},
               new ushort[2] {0x5, 0x6},
               new ushort[1] {0x8},
               new ushort[1] {0x10},
               new Reduction[0] {})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[1] {MathExp_Lexer.terminals[3]},
               new ushort[1] {0xd},
               new ushort[1] {0x11},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[6] {MathExp_Lexer.terminals[1], MathExp_Lexer.terminals[3], MathExp_Lexer.terminals[4], MathExp_Lexer.terminals[5], MathExp_Lexer.terminals[6], MathExp_Lexer.terminals[7]},
               new ushort[2] {0xe, 0xf},
               new ushort[2] {0xA, 0xB},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[4] {new Reduction(0x2, staticRules[0x6]), new Reduction(0xd, staticRules[0x6]), new Reduction(0x10, staticRules[0x6]), new Reduction(0x11, staticRules[0x6])})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[6] {MathExp_Lexer.terminals[1], MathExp_Lexer.terminals[3], MathExp_Lexer.terminals[4], MathExp_Lexer.terminals[5], MathExp_Lexer.terminals[6], MathExp_Lexer.terminals[7]},
               new ushort[2] {0xe, 0xf},
               new ushort[2] {0xA, 0xB},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[4] {new Reduction(0x2, staticRules[0x7]), new Reduction(0xd, staticRules[0x7]), new Reduction(0x10, staticRules[0x7]), new Reduction(0x11, staticRules[0x7])})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[6] {MathExp_Lexer.terminals[1], MathExp_Lexer.terminals[3], MathExp_Lexer.terminals[4], MathExp_Lexer.terminals[5], MathExp_Lexer.terminals[6], MathExp_Lexer.terminals[7]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[6] {new Reduction(0x2, staticRules[0x3]), new Reduction(0xd, staticRules[0x3]), new Reduction(0xe, staticRules[0x3]), new Reduction(0xf, staticRules[0x3]), new Reduction(0x10, staticRules[0x3]), new Reduction(0x11, staticRules[0x3])})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[6] {MathExp_Lexer.terminals[1], MathExp_Lexer.terminals[3], MathExp_Lexer.terminals[4], MathExp_Lexer.terminals[5], MathExp_Lexer.terminals[6], MathExp_Lexer.terminals[7]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[6] {new Reduction(0x2, staticRules[0x4]), new Reduction(0xd, staticRules[0x4]), new Reduction(0xe, staticRules[0x4]), new Reduction(0xf, staticRules[0x4]), new Reduction(0x10, staticRules[0x4]), new Reduction(0x11, staticRules[0x4])})
            , new State(
               null,
               new Hime.Redist.Parsers.SymbolTerminal[6] {MathExp_Lexer.terminals[1], MathExp_Lexer.terminals[3], MathExp_Lexer.terminals[4], MathExp_Lexer.terminals[5], MathExp_Lexer.terminals[6], MathExp_Lexer.terminals[7]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[6] {new Reduction(0x2, staticRules[0x1]), new Reduction(0xd, staticRules[0x1]), new Reduction(0xe, staticRules[0x1]), new Reduction(0xf, staticRules[0x1]), new Reduction(0x10, staticRules[0x1]), new Reduction(0x11, staticRules[0x1])})
        };
        public interface Actions
        {
            void OnNumber(Hime.Redist.Parsers.SyntaxTreeNode SubRoot);
            void OnMult(Hime.Redist.Parsers.SyntaxTreeNode SubRoot);
            void OnDiv(Hime.Redist.Parsers.SyntaxTreeNode SubRoot);
            void OnPlus(Hime.Redist.Parsers.SyntaxTreeNode SubRoot);
            void OnMinus(Hime.Redist.Parsers.SyntaxTreeNode SubRoot);
        }
        protected override void setup()
        {
            rules = staticRules;
            states = staticStates;
            errorSimulationLength = 3;
        }
        private Actions actions;
        public MathExp_Parser(MathExp_Lexer lexer, Actions actions) : base (lexer) { this.actions = actions; }
    }
}
