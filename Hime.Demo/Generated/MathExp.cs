using System.Collections.Generic;

namespace Analyser
{
    class MathExp_Lexer : Hime.Redist.Parsers.LexerText
    {
        private static ushort[] staticSymbolsSID = { 0xC, 0xD, 0xE, 0xF, 0x10, 0x11, 0x5, 0x7 };
        private static string[] staticSymbolsName = { "_T[(]", "_T[)]", "_T[*]", "_T[/]", "_T[+]", "_T[-]", "NUMBER", "SEPARATOR" };
        private static ushort[][] staticTransitions0 = { new ushort[3] { 0x28, 0x28, 0x4 }, new ushort[3] { 0x29, 0x29, 0x5 }, new ushort[3] { 0x2A, 0x2A, 0x6 }, new ushort[3] { 0x2F, 0x2F, 0x7 }, new ushort[3] { 0x2B, 0x2B, 0x8 }, new ushort[3] { 0x2D, 0x2D, 0x9 }, new ushort[3] { 0x31, 0x39, 0xA }, new ushort[3] { 0x30, 0x30, 0xB }, new ushort[3] { 0x9, 0x9, 0xC }, new ushort[3] { 0xB, 0xC, 0xC }, new ushort[3] { 0x20, 0x20, 0xC }, new ushort[3] { 0x2E, 0x2E, 0x1 } };
        private static ushort[][] staticTransitions1 = { new ushort[3] { 0x31, 0x39, 0xE }, new ushort[3] { 0x30, 0x30, 0xF } };
        private static ushort[][] staticTransitions2 = { new ushort[3] { 0x31, 0x39, 0x10 }, new ushort[3] { 0x30, 0x30, 0x11 } };
        private static ushort[][] staticTransitions3 = { new ushort[3] { 0x2B, 0x2B, 0x2 }, new ushort[3] { 0x2D, 0x2D, 0x2 }, new ushort[3] { 0x31, 0x39, 0x10 }, new ushort[3] { 0x30, 0x30, 0x11 } };
        private static ushort[][] staticTransitions4 = {  };
        private static ushort[][] staticTransitions5 = {  };
        private static ushort[][] staticTransitions6 = {  };
        private static ushort[][] staticTransitions7 = {  };
        private static ushort[][] staticTransitions8 = {  };
        private static ushort[][] staticTransitions9 = {  };
        private static ushort[][] staticTransitionsA = { new ushort[3] { 0x30, 0x39, 0xA }, new ushort[3] { 0x45, 0x45, 0x3 }, new ushort[3] { 0x65, 0x65, 0x3 }, new ushort[3] { 0x2E, 0x2E, 0x1 } };
        private static ushort[][] staticTransitionsB = { new ushort[3] { 0x45, 0x45, 0x3 }, new ushort[3] { 0x65, 0x65, 0x3 }, new ushort[3] { 0x2E, 0x2E, 0x1 } };
        private static ushort[][] staticTransitionsC = { new ushort[3] { 0x9, 0x9, 0xD }, new ushort[3] { 0xB, 0xC, 0xD }, new ushort[3] { 0x20, 0x20, 0xD } };
        private static ushort[][] staticTransitionsD = { new ushort[3] { 0x9, 0x9, 0xD }, new ushort[3] { 0xB, 0xC, 0xD }, new ushort[3] { 0x20, 0x20, 0xD } };
        private static ushort[][] staticTransitionsE = { new ushort[3] { 0x30, 0x39, 0xE }, new ushort[3] { 0x45, 0x45, 0x3 }, new ushort[3] { 0x65, 0x65, 0x3 } };
        private static ushort[][] staticTransitionsF = { new ushort[3] { 0x45, 0x45, 0x3 }, new ushort[3] { 0x65, 0x65, 0x3 } };
        private static ushort[][] staticTransitions10 = { new ushort[3] { 0x30, 0x39, 0x10 } };
        private static ushort[][] staticTransitions11 = {  };
        private static ushort[][][] staticTransitions = { staticTransitions0, staticTransitions1, staticTransitions2, staticTransitions3, staticTransitions4, staticTransitions5, staticTransitions6, staticTransitions7, staticTransitions8, staticTransitions9, staticTransitionsA, staticTransitionsB, staticTransitionsC, staticTransitionsD, staticTransitionsE, staticTransitionsF, staticTransitions10, staticTransitions11 };
        private static int[] staticFinals = { -1, -1, -1, -1, 0, 1, 2, 3, 4, 5, 6, 6, 7, 7, 6, 6, 6, 6 };
        protected override void setup() {
            symbolsSID = staticSymbolsSID;
            symbolsName = staticSymbolsName;
            symbolsSubGrammars = new Dictionary<ushort, MatchSubGrammar>();
            transitions = staticTransitions;
            finals = staticFinals;
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
        private static void Production_8_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x8, "exp_atom"));
            SubRoot.AppendChild(Definition[0]);
            ((MathExp_Parser)parser).actions.OnNumber(SubRoot);
            nodes.Add(SubRoot);
        }
        private static void Production_8_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x8, "exp_atom"));
            SubRoot.AppendChild(Definition[0]);
            SubRoot.AppendChild(Definition[1]);
            SubRoot.AppendChild(Definition[2]);
            nodes.Add(SubRoot);
        }
        private static void Production_9_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x9, "exp_op0"));
            SubRoot.AppendChild(Definition[0]);
            nodes.Add(SubRoot);
        }
        private static void Production_9_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x9, "exp_op0"));
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
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x9, "exp_op0"));
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
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xA, "exp_op1"));
            SubRoot.AppendChild(Definition[0]);
            nodes.Add(SubRoot);
        }
        private static void Production_A_1 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xA, "exp_op1"));
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
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xA, "exp_op1"));
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
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xB, "exp"));
            SubRoot.AppendChild(Definition[0]);
            nodes.Add(SubRoot);
        }
        private static void Production_12_0 (Hime.Redist.Parsers.BaseLR1Parser parser, List<Hime.Redist.Parsers.SyntaxTreeNode> nodes)
        {
            List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x12, "_Axiom_"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static Rule[] staticRules = {
           new Rule(Production_8_0, new Hime.Redist.Parsers.SymbolVariable(0x8, "exp_atom"), 1)
           , new Rule(Production_8_1, new Hime.Redist.Parsers.SymbolVariable(0x8, "exp_atom"), 3)
           , new Rule(Production_9_0, new Hime.Redist.Parsers.SymbolVariable(0x9, "exp_op0"), 1)
           , new Rule(Production_9_1, new Hime.Redist.Parsers.SymbolVariable(0x9, "exp_op0"), 3)
           , new Rule(Production_9_2, new Hime.Redist.Parsers.SymbolVariable(0x9, "exp_op0"), 3)
           , new Rule(Production_A_0, new Hime.Redist.Parsers.SymbolVariable(0xA, "exp_op1"), 1)
           , new Rule(Production_A_1, new Hime.Redist.Parsers.SymbolVariable(0xA, "exp_op1"), 3)
           , new Rule(Production_A_2, new Hime.Redist.Parsers.SymbolVariable(0xA, "exp_op1"), 3)
           , new Rule(Production_B_0, new Hime.Redist.Parsers.SymbolVariable(0xB, "exp"), 1)
           , new Rule(Production_12_0, new Hime.Redist.Parsers.SymbolVariable(0x12, "_Axiom_"), 2)
        };
        private static State[] staticStates = {
            new State(
               new string[36] {"[_Axiom_ → • exp $, ε]", "[exp → • exp_op1, $]", "[exp_op1 → • exp_op0, $]", "[exp_op1 → • exp_op1 + exp_op0, $]", "[exp_op1 → • exp_op1 - exp_op0, $]", "[exp_op0 → • exp_atom, $]", "[exp_op0 → • exp_op0 * exp_atom, $]", "[exp_op0 → • exp_op0 / exp_atom, $]", "[exp_op1 → • exp_op0, +]", "[exp_op1 → • exp_op1 + exp_op0, +]", "[exp_op1 → • exp_op1 - exp_op0, +]", "[exp_op1 → • exp_op0, -]", "[exp_op1 → • exp_op1 + exp_op0, -]", "[exp_op1 → • exp_op1 - exp_op0, -]", "[exp_atom → • NUMBER, $]", "[exp_atom → • ( exp ), $]", "[exp_op0 → • exp_atom, *]", "[exp_op0 → • exp_op0 * exp_atom, *]", "[exp_op0 → • exp_op0 / exp_atom, *]", "[exp_op0 → • exp_atom, /]", "[exp_op0 → • exp_op0 * exp_atom, /]", "[exp_op0 → • exp_op0 / exp_atom, /]", "[exp_op0 → • exp_atom, +]", "[exp_op0 → • exp_op0 * exp_atom, +]", "[exp_op0 → • exp_op0 / exp_atom, +]", "[exp_op0 → • exp_atom, -]", "[exp_op0 → • exp_op0 * exp_atom, -]", "[exp_op0 → • exp_op0 / exp_atom, -]", "[exp_atom → • NUMBER, *]", "[exp_atom → • ( exp ), *]", "[exp_atom → • NUMBER, /]", "[exp_atom → • ( exp ), /]", "[exp_atom → • NUMBER, +]", "[exp_atom → • ( exp ), +]", "[exp_atom → • NUMBER, -]", "[exp_atom → • ( exp ), -]"},
               new Terminal[2] {new Terminal("NUMBER", 0x5), new Terminal("_T[(]", 0xC)},
               new ushort[2] {0x5, 0xc},
               new ushort[2] {0x5, 0x6},
               new ushort[4] {0xb, 0xa, 0x9, 0x8},
               new ushort[4] {0x1, 0x2, 0x3, 0x4},
               new Reduction[0] {})
            , new State(
               new string[1] {"[_Axiom_ → exp • $, ε]"},
               new Terminal[1] {new Terminal("$", 0x2)},
               new ushort[1] {0x2},
               new ushort[1] {0x7},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[7] {"[exp → exp_op1 •, $]", "[exp_op1 → exp_op1 • + exp_op0, $]", "[exp_op1 → exp_op1 • - exp_op0, $]", "[exp_op1 → exp_op1 • + exp_op0, +]", "[exp_op1 → exp_op1 • - exp_op0, +]", "[exp_op1 → exp_op1 • + exp_op0, -]", "[exp_op1 → exp_op1 • - exp_op0, -]"},
               new Terminal[3] {new Terminal("$", 0x2), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11)},
               new ushort[2] {0x10, 0x11},
               new ushort[2] {0x8, 0x9},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x2, staticRules[0x8])})
            , new State(
               new string[13] {"[exp_op1 → exp_op0 •, $]", "[exp_op0 → exp_op0 • * exp_atom, $]", "[exp_op0 → exp_op0 • / exp_atom, $]", "[exp_op1 → exp_op0 •, +]", "[exp_op1 → exp_op0 •, -]", "[exp_op0 → exp_op0 • * exp_atom, *]", "[exp_op0 → exp_op0 • / exp_atom, *]", "[exp_op0 → exp_op0 • * exp_atom, /]", "[exp_op0 → exp_op0 • / exp_atom, /]", "[exp_op0 → exp_op0 • * exp_atom, +]", "[exp_op0 → exp_op0 • / exp_atom, +]", "[exp_op0 → exp_op0 • * exp_atom, -]", "[exp_op0 → exp_op0 • / exp_atom, -]"},
               new Terminal[5] {new Terminal("$", 0x2), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11)},
               new ushort[2] {0xe, 0xf},
               new ushort[2] {0xA, 0xB},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0x2, staticRules[0x5]), new Reduction(0x10, staticRules[0x5]), new Reduction(0x11, staticRules[0x5])})
            , new State(
               new string[5] {"[exp_op0 → exp_atom •, $]", "[exp_op0 → exp_atom •, *]", "[exp_op0 → exp_atom •, /]", "[exp_op0 → exp_atom •, +]", "[exp_op0 → exp_atom •, -]"},
               new Terminal[5] {new Terminal("$", 0x2), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[5] {new Reduction(0x2, staticRules[0x2]), new Reduction(0xe, staticRules[0x2]), new Reduction(0xf, staticRules[0x2]), new Reduction(0x10, staticRules[0x2]), new Reduction(0x11, staticRules[0x2])})
            , new State(
               new string[5] {"[exp_atom → NUMBER •, $]", "[exp_atom → NUMBER •, *]", "[exp_atom → NUMBER •, /]", "[exp_atom → NUMBER •, +]", "[exp_atom → NUMBER •, -]"},
               new Terminal[5] {new Terminal("$", 0x2), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[5] {new Reduction(0x2, staticRules[0x0]), new Reduction(0xe, staticRules[0x0]), new Reduction(0xf, staticRules[0x0]), new Reduction(0x10, staticRules[0x0]), new Reduction(0x11, staticRules[0x0])})
            , new State(
               new string[40] {"[exp_atom → ( • exp ), $]", "[exp_atom → ( • exp ), *]", "[exp_atom → ( • exp ), /]", "[exp_atom → ( • exp ), +]", "[exp_atom → ( • exp ), -]", "[exp → • exp_op1, )]", "[exp_op1 → • exp_op0, )]", "[exp_op1 → • exp_op1 + exp_op0, )]", "[exp_op1 → • exp_op1 - exp_op0, )]", "[exp_op0 → • exp_atom, )]", "[exp_op0 → • exp_op0 * exp_atom, )]", "[exp_op0 → • exp_op0 / exp_atom, )]", "[exp_op1 → • exp_op0, +]", "[exp_op1 → • exp_op1 + exp_op0, +]", "[exp_op1 → • exp_op1 - exp_op0, +]", "[exp_op1 → • exp_op0, -]", "[exp_op1 → • exp_op1 + exp_op0, -]", "[exp_op1 → • exp_op1 - exp_op0, -]", "[exp_atom → • NUMBER, )]", "[exp_atom → • ( exp ), )]", "[exp_op0 → • exp_atom, *]", "[exp_op0 → • exp_op0 * exp_atom, *]", "[exp_op0 → • exp_op0 / exp_atom, *]", "[exp_op0 → • exp_atom, /]", "[exp_op0 → • exp_op0 * exp_atom, /]", "[exp_op0 → • exp_op0 / exp_atom, /]", "[exp_op0 → • exp_atom, +]", "[exp_op0 → • exp_op0 * exp_atom, +]", "[exp_op0 → • exp_op0 / exp_atom, +]", "[exp_op0 → • exp_atom, -]", "[exp_op0 → • exp_op0 * exp_atom, -]", "[exp_op0 → • exp_op0 / exp_atom, -]", "[exp_atom → • NUMBER, *]", "[exp_atom → • ( exp ), *]", "[exp_atom → • NUMBER, /]", "[exp_atom → • ( exp ), /]", "[exp_atom → • NUMBER, +]", "[exp_atom → • ( exp ), +]", "[exp_atom → • NUMBER, -]", "[exp_atom → • ( exp ), -]"},
               new Terminal[2] {new Terminal("NUMBER", 0x5), new Terminal("_T[(]", 0xC)},
               new ushort[2] {0x5, 0xc},
               new ushort[2] {0x10, 0x11},
               new ushort[4] {0xb, 0xa, 0x9, 0x8},
               new ushort[4] {0xC, 0xD, 0xE, 0xF},
               new Reduction[0] {})
            , new State(
               new string[1] {"[_Axiom_ → exp $ •, ε]"},
               new Terminal[1] {new Terminal("ε", 0x1)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x1, staticRules[0x9])})
            , new State(
               new string[28] {"[exp_op1 → exp_op1 + • exp_op0, $]", "[exp_op1 → exp_op1 + • exp_op0, +]", "[exp_op1 → exp_op1 + • exp_op0, -]", "[exp_op0 → • exp_atom, $]", "[exp_op0 → • exp_op0 * exp_atom, $]", "[exp_op0 → • exp_op0 / exp_atom, $]", "[exp_op0 → • exp_atom, +]", "[exp_op0 → • exp_op0 * exp_atom, +]", "[exp_op0 → • exp_op0 / exp_atom, +]", "[exp_op0 → • exp_atom, -]", "[exp_op0 → • exp_op0 * exp_atom, -]", "[exp_op0 → • exp_op0 / exp_atom, -]", "[exp_atom → • NUMBER, $]", "[exp_atom → • ( exp ), $]", "[exp_op0 → • exp_atom, *]", "[exp_op0 → • exp_op0 * exp_atom, *]", "[exp_op0 → • exp_op0 / exp_atom, *]", "[exp_op0 → • exp_atom, /]", "[exp_op0 → • exp_op0 * exp_atom, /]", "[exp_op0 → • exp_op0 / exp_atom, /]", "[exp_atom → • NUMBER, +]", "[exp_atom → • ( exp ), +]", "[exp_atom → • NUMBER, -]", "[exp_atom → • ( exp ), -]", "[exp_atom → • NUMBER, *]", "[exp_atom → • ( exp ), *]", "[exp_atom → • NUMBER, /]", "[exp_atom → • ( exp ), /]"},
               new Terminal[2] {new Terminal("NUMBER", 0x5), new Terminal("_T[(]", 0xC)},
               new ushort[2] {0x5, 0xc},
               new ushort[2] {0x5, 0x6},
               new ushort[2] {0x9, 0x8},
               new ushort[2] {0x12, 0x4},
               new Reduction[0] {})
            , new State(
               new string[28] {"[exp_op1 → exp_op1 - • exp_op0, $]", "[exp_op1 → exp_op1 - • exp_op0, +]", "[exp_op1 → exp_op1 - • exp_op0, -]", "[exp_op0 → • exp_atom, $]", "[exp_op0 → • exp_op0 * exp_atom, $]", "[exp_op0 → • exp_op0 / exp_atom, $]", "[exp_op0 → • exp_atom, +]", "[exp_op0 → • exp_op0 * exp_atom, +]", "[exp_op0 → • exp_op0 / exp_atom, +]", "[exp_op0 → • exp_atom, -]", "[exp_op0 → • exp_op0 * exp_atom, -]", "[exp_op0 → • exp_op0 / exp_atom, -]", "[exp_atom → • NUMBER, $]", "[exp_atom → • ( exp ), $]", "[exp_op0 → • exp_atom, *]", "[exp_op0 → • exp_op0 * exp_atom, *]", "[exp_op0 → • exp_op0 / exp_atom, *]", "[exp_op0 → • exp_atom, /]", "[exp_op0 → • exp_op0 * exp_atom, /]", "[exp_op0 → • exp_op0 / exp_atom, /]", "[exp_atom → • NUMBER, +]", "[exp_atom → • ( exp ), +]", "[exp_atom → • NUMBER, -]", "[exp_atom → • ( exp ), -]", "[exp_atom → • NUMBER, *]", "[exp_atom → • ( exp ), *]", "[exp_atom → • NUMBER, /]", "[exp_atom → • ( exp ), /]"},
               new Terminal[2] {new Terminal("NUMBER", 0x5), new Terminal("_T[(]", 0xC)},
               new ushort[2] {0x5, 0xc},
               new ushort[2] {0x5, 0x6},
               new ushort[2] {0x9, 0x8},
               new ushort[2] {0x13, 0x4},
               new Reduction[0] {})
            , new State(
               new string[15] {"[exp_op0 → exp_op0 * • exp_atom, $]", "[exp_op0 → exp_op0 * • exp_atom, *]", "[exp_op0 → exp_op0 * • exp_atom, /]", "[exp_op0 → exp_op0 * • exp_atom, +]", "[exp_op0 → exp_op0 * • exp_atom, -]", "[exp_atom → • NUMBER, $]", "[exp_atom → • ( exp ), $]", "[exp_atom → • NUMBER, *]", "[exp_atom → • ( exp ), *]", "[exp_atom → • NUMBER, /]", "[exp_atom → • ( exp ), /]", "[exp_atom → • NUMBER, +]", "[exp_atom → • ( exp ), +]", "[exp_atom → • NUMBER, -]", "[exp_atom → • ( exp ), -]"},
               new Terminal[2] {new Terminal("NUMBER", 0x5), new Terminal("_T[(]", 0xC)},
               new ushort[2] {0x5, 0xc},
               new ushort[2] {0x5, 0x6},
               new ushort[1] {0x8},
               new ushort[1] {0x14},
               new Reduction[0] {})
            , new State(
               new string[15] {"[exp_op0 → exp_op0 / • exp_atom, $]", "[exp_op0 → exp_op0 / • exp_atom, *]", "[exp_op0 → exp_op0 / • exp_atom, /]", "[exp_op0 → exp_op0 / • exp_atom, +]", "[exp_op0 → exp_op0 / • exp_atom, -]", "[exp_atom → • NUMBER, $]", "[exp_atom → • ( exp ), $]", "[exp_atom → • NUMBER, *]", "[exp_atom → • ( exp ), *]", "[exp_atom → • NUMBER, /]", "[exp_atom → • ( exp ), /]", "[exp_atom → • NUMBER, +]", "[exp_atom → • ( exp ), +]", "[exp_atom → • NUMBER, -]", "[exp_atom → • ( exp ), -]"},
               new Terminal[2] {new Terminal("NUMBER", 0x5), new Terminal("_T[(]", 0xC)},
               new ushort[2] {0x5, 0xc},
               new ushort[2] {0x5, 0x6},
               new ushort[1] {0x8},
               new ushort[1] {0x15},
               new Reduction[0] {})
            , new State(
               new string[5] {"[exp_atom → ( exp • ), $]", "[exp_atom → ( exp • ), *]", "[exp_atom → ( exp • ), /]", "[exp_atom → ( exp • ), +]", "[exp_atom → ( exp • ), -]"},
               new Terminal[1] {new Terminal("_T[)]", 0xD)},
               new ushort[1] {0xd},
               new ushort[1] {0x16},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[7] {"[exp → exp_op1 •, )]", "[exp_op1 → exp_op1 • + exp_op0, )]", "[exp_op1 → exp_op1 • - exp_op0, )]", "[exp_op1 → exp_op1 • + exp_op0, +]", "[exp_op1 → exp_op1 • - exp_op0, +]", "[exp_op1 → exp_op1 • + exp_op0, -]", "[exp_op1 → exp_op1 • - exp_op0, -]"},
               new Terminal[3] {new Terminal("_T[)]", 0xD), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11)},
               new ushort[2] {0x10, 0x11},
               new ushort[2] {0x17, 0x18},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0xd, staticRules[0x8])})
            , new State(
               new string[13] {"[exp_op1 → exp_op0 •, )]", "[exp_op0 → exp_op0 • * exp_atom, )]", "[exp_op0 → exp_op0 • / exp_atom, )]", "[exp_op1 → exp_op0 •, +]", "[exp_op1 → exp_op0 •, -]", "[exp_op0 → exp_op0 • * exp_atom, *]", "[exp_op0 → exp_op0 • / exp_atom, *]", "[exp_op0 → exp_op0 • * exp_atom, /]", "[exp_op0 → exp_op0 • / exp_atom, /]", "[exp_op0 → exp_op0 • * exp_atom, +]", "[exp_op0 → exp_op0 • / exp_atom, +]", "[exp_op0 → exp_op0 • * exp_atom, -]", "[exp_op0 → exp_op0 • / exp_atom, -]"},
               new Terminal[5] {new Terminal("_T[)]", 0xD), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11)},
               new ushort[2] {0xe, 0xf},
               new ushort[2] {0x19, 0x1A},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0xd, staticRules[0x5]), new Reduction(0x10, staticRules[0x5]), new Reduction(0x11, staticRules[0x5])})
            , new State(
               new string[5] {"[exp_op0 → exp_atom •, )]", "[exp_op0 → exp_atom •, *]", "[exp_op0 → exp_atom •, /]", "[exp_op0 → exp_atom •, +]", "[exp_op0 → exp_atom •, -]"},
               new Terminal[5] {new Terminal("_T[)]", 0xD), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[5] {new Reduction(0xd, staticRules[0x2]), new Reduction(0xe, staticRules[0x2]), new Reduction(0xf, staticRules[0x2]), new Reduction(0x10, staticRules[0x2]), new Reduction(0x11, staticRules[0x2])})
            , new State(
               new string[5] {"[exp_atom → NUMBER •, )]", "[exp_atom → NUMBER •, *]", "[exp_atom → NUMBER •, /]", "[exp_atom → NUMBER •, +]", "[exp_atom → NUMBER •, -]"},
               new Terminal[5] {new Terminal("_T[)]", 0xD), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[5] {new Reduction(0xd, staticRules[0x0]), new Reduction(0xe, staticRules[0x0]), new Reduction(0xf, staticRules[0x0]), new Reduction(0x10, staticRules[0x0]), new Reduction(0x11, staticRules[0x0])})
            , new State(
               new string[40] {"[exp_atom → ( • exp ), )]", "[exp_atom → ( • exp ), *]", "[exp_atom → ( • exp ), /]", "[exp_atom → ( • exp ), +]", "[exp_atom → ( • exp ), -]", "[exp → • exp_op1, )]", "[exp_op1 → • exp_op0, )]", "[exp_op1 → • exp_op1 + exp_op0, )]", "[exp_op1 → • exp_op1 - exp_op0, )]", "[exp_op0 → • exp_atom, )]", "[exp_op0 → • exp_op0 * exp_atom, )]", "[exp_op0 → • exp_op0 / exp_atom, )]", "[exp_op1 → • exp_op0, +]", "[exp_op1 → • exp_op1 + exp_op0, +]", "[exp_op1 → • exp_op1 - exp_op0, +]", "[exp_op1 → • exp_op0, -]", "[exp_op1 → • exp_op1 + exp_op0, -]", "[exp_op1 → • exp_op1 - exp_op0, -]", "[exp_atom → • NUMBER, )]", "[exp_atom → • ( exp ), )]", "[exp_op0 → • exp_atom, *]", "[exp_op0 → • exp_op0 * exp_atom, *]", "[exp_op0 → • exp_op0 / exp_atom, *]", "[exp_op0 → • exp_atom, /]", "[exp_op0 → • exp_op0 * exp_atom, /]", "[exp_op0 → • exp_op0 / exp_atom, /]", "[exp_op0 → • exp_atom, +]", "[exp_op0 → • exp_op0 * exp_atom, +]", "[exp_op0 → • exp_op0 / exp_atom, +]", "[exp_op0 → • exp_atom, -]", "[exp_op0 → • exp_op0 * exp_atom, -]", "[exp_op0 → • exp_op0 / exp_atom, -]", "[exp_atom → • NUMBER, *]", "[exp_atom → • ( exp ), *]", "[exp_atom → • NUMBER, /]", "[exp_atom → • ( exp ), /]", "[exp_atom → • NUMBER, +]", "[exp_atom → • ( exp ), +]", "[exp_atom → • NUMBER, -]", "[exp_atom → • ( exp ), -]"},
               new Terminal[2] {new Terminal("NUMBER", 0x5), new Terminal("_T[(]", 0xC)},
               new ushort[2] {0x5, 0xc},
               new ushort[2] {0x10, 0x11},
               new ushort[4] {0xb, 0xa, 0x9, 0x8},
               new ushort[4] {0x1B, 0xD, 0xE, 0xF},
               new Reduction[0] {})
            , new State(
               new string[13] {"[exp_op1 → exp_op1 + exp_op0 •, $]", "[exp_op1 → exp_op1 + exp_op0 •, +]", "[exp_op1 → exp_op1 + exp_op0 •, -]", "[exp_op0 → exp_op0 • * exp_atom, $]", "[exp_op0 → exp_op0 • / exp_atom, $]", "[exp_op0 → exp_op0 • * exp_atom, +]", "[exp_op0 → exp_op0 • / exp_atom, +]", "[exp_op0 → exp_op0 • * exp_atom, -]", "[exp_op0 → exp_op0 • / exp_atom, -]", "[exp_op0 → exp_op0 • * exp_atom, *]", "[exp_op0 → exp_op0 • / exp_atom, *]", "[exp_op0 → exp_op0 • * exp_atom, /]", "[exp_op0 → exp_op0 • / exp_atom, /]"},
               new Terminal[5] {new Terminal("$", 0x2), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11)},
               new ushort[2] {0xe, 0xf},
               new ushort[2] {0xA, 0xB},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0x2, staticRules[0x6]), new Reduction(0x10, staticRules[0x6]), new Reduction(0x11, staticRules[0x6])})
            , new State(
               new string[13] {"[exp_op1 → exp_op1 - exp_op0 •, $]", "[exp_op1 → exp_op1 - exp_op0 •, +]", "[exp_op1 → exp_op1 - exp_op0 •, -]", "[exp_op0 → exp_op0 • * exp_atom, $]", "[exp_op0 → exp_op0 • / exp_atom, $]", "[exp_op0 → exp_op0 • * exp_atom, +]", "[exp_op0 → exp_op0 • / exp_atom, +]", "[exp_op0 → exp_op0 • * exp_atom, -]", "[exp_op0 → exp_op0 • / exp_atom, -]", "[exp_op0 → exp_op0 • * exp_atom, *]", "[exp_op0 → exp_op0 • / exp_atom, *]", "[exp_op0 → exp_op0 • * exp_atom, /]", "[exp_op0 → exp_op0 • / exp_atom, /]"},
               new Terminal[5] {new Terminal("$", 0x2), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11)},
               new ushort[2] {0xe, 0xf},
               new ushort[2] {0xA, 0xB},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0x2, staticRules[0x7]), new Reduction(0x10, staticRules[0x7]), new Reduction(0x11, staticRules[0x7])})
            , new State(
               new string[5] {"[exp_op0 → exp_op0 * exp_atom •, $]", "[exp_op0 → exp_op0 * exp_atom •, *]", "[exp_op0 → exp_op0 * exp_atom •, /]", "[exp_op0 → exp_op0 * exp_atom •, +]", "[exp_op0 → exp_op0 * exp_atom •, -]"},
               new Terminal[5] {new Terminal("$", 0x2), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[5] {new Reduction(0x2, staticRules[0x3]), new Reduction(0xe, staticRules[0x3]), new Reduction(0xf, staticRules[0x3]), new Reduction(0x10, staticRules[0x3]), new Reduction(0x11, staticRules[0x3])})
            , new State(
               new string[5] {"[exp_op0 → exp_op0 / exp_atom •, $]", "[exp_op0 → exp_op0 / exp_atom •, *]", "[exp_op0 → exp_op0 / exp_atom •, /]", "[exp_op0 → exp_op0 / exp_atom •, +]", "[exp_op0 → exp_op0 / exp_atom •, -]"},
               new Terminal[5] {new Terminal("$", 0x2), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[5] {new Reduction(0x2, staticRules[0x4]), new Reduction(0xe, staticRules[0x4]), new Reduction(0xf, staticRules[0x4]), new Reduction(0x10, staticRules[0x4]), new Reduction(0x11, staticRules[0x4])})
            , new State(
               new string[5] {"[exp_atom → ( exp ) •, $]", "[exp_atom → ( exp ) •, *]", "[exp_atom → ( exp ) •, /]", "[exp_atom → ( exp ) •, +]", "[exp_atom → ( exp ) •, -]"},
               new Terminal[5] {new Terminal("$", 0x2), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[5] {new Reduction(0x2, staticRules[0x1]), new Reduction(0xe, staticRules[0x1]), new Reduction(0xf, staticRules[0x1]), new Reduction(0x10, staticRules[0x1]), new Reduction(0x11, staticRules[0x1])})
            , new State(
               new string[28] {"[exp_op1 → exp_op1 + • exp_op0, )]", "[exp_op1 → exp_op1 + • exp_op0, +]", "[exp_op1 → exp_op1 + • exp_op0, -]", "[exp_op0 → • exp_atom, )]", "[exp_op0 → • exp_op0 * exp_atom, )]", "[exp_op0 → • exp_op0 / exp_atom, )]", "[exp_op0 → • exp_atom, +]", "[exp_op0 → • exp_op0 * exp_atom, +]", "[exp_op0 → • exp_op0 / exp_atom, +]", "[exp_op0 → • exp_atom, -]", "[exp_op0 → • exp_op0 * exp_atom, -]", "[exp_op0 → • exp_op0 / exp_atom, -]", "[exp_atom → • NUMBER, )]", "[exp_atom → • ( exp ), )]", "[exp_op0 → • exp_atom, *]", "[exp_op0 → • exp_op0 * exp_atom, *]", "[exp_op0 → • exp_op0 / exp_atom, *]", "[exp_op0 → • exp_atom, /]", "[exp_op0 → • exp_op0 * exp_atom, /]", "[exp_op0 → • exp_op0 / exp_atom, /]", "[exp_atom → • NUMBER, +]", "[exp_atom → • ( exp ), +]", "[exp_atom → • NUMBER, -]", "[exp_atom → • ( exp ), -]", "[exp_atom → • NUMBER, *]", "[exp_atom → • ( exp ), *]", "[exp_atom → • NUMBER, /]", "[exp_atom → • ( exp ), /]"},
               new Terminal[2] {new Terminal("NUMBER", 0x5), new Terminal("_T[(]", 0xC)},
               new ushort[2] {0x5, 0xc},
               new ushort[2] {0x10, 0x11},
               new ushort[2] {0x9, 0x8},
               new ushort[2] {0x1C, 0xF},
               new Reduction[0] {})
            , new State(
               new string[28] {"[exp_op1 → exp_op1 - • exp_op0, )]", "[exp_op1 → exp_op1 - • exp_op0, +]", "[exp_op1 → exp_op1 - • exp_op0, -]", "[exp_op0 → • exp_atom, )]", "[exp_op0 → • exp_op0 * exp_atom, )]", "[exp_op0 → • exp_op0 / exp_atom, )]", "[exp_op0 → • exp_atom, +]", "[exp_op0 → • exp_op0 * exp_atom, +]", "[exp_op0 → • exp_op0 / exp_atom, +]", "[exp_op0 → • exp_atom, -]", "[exp_op0 → • exp_op0 * exp_atom, -]", "[exp_op0 → • exp_op0 / exp_atom, -]", "[exp_atom → • NUMBER, )]", "[exp_atom → • ( exp ), )]", "[exp_op0 → • exp_atom, *]", "[exp_op0 → • exp_op0 * exp_atom, *]", "[exp_op0 → • exp_op0 / exp_atom, *]", "[exp_op0 → • exp_atom, /]", "[exp_op0 → • exp_op0 * exp_atom, /]", "[exp_op0 → • exp_op0 / exp_atom, /]", "[exp_atom → • NUMBER, +]", "[exp_atom → • ( exp ), +]", "[exp_atom → • NUMBER, -]", "[exp_atom → • ( exp ), -]", "[exp_atom → • NUMBER, *]", "[exp_atom → • ( exp ), *]", "[exp_atom → • NUMBER, /]", "[exp_atom → • ( exp ), /]"},
               new Terminal[2] {new Terminal("NUMBER", 0x5), new Terminal("_T[(]", 0xC)},
               new ushort[2] {0x5, 0xc},
               new ushort[2] {0x10, 0x11},
               new ushort[2] {0x9, 0x8},
               new ushort[2] {0x1D, 0xF},
               new Reduction[0] {})
            , new State(
               new string[15] {"[exp_op0 → exp_op0 * • exp_atom, )]", "[exp_op0 → exp_op0 * • exp_atom, *]", "[exp_op0 → exp_op0 * • exp_atom, /]", "[exp_op0 → exp_op0 * • exp_atom, +]", "[exp_op0 → exp_op0 * • exp_atom, -]", "[exp_atom → • NUMBER, )]", "[exp_atom → • ( exp ), )]", "[exp_atom → • NUMBER, *]", "[exp_atom → • ( exp ), *]", "[exp_atom → • NUMBER, /]", "[exp_atom → • ( exp ), /]", "[exp_atom → • NUMBER, +]", "[exp_atom → • ( exp ), +]", "[exp_atom → • NUMBER, -]", "[exp_atom → • ( exp ), -]"},
               new Terminal[2] {new Terminal("NUMBER", 0x5), new Terminal("_T[(]", 0xC)},
               new ushort[2] {0x5, 0xc},
               new ushort[2] {0x10, 0x11},
               new ushort[1] {0x8},
               new ushort[1] {0x1E},
               new Reduction[0] {})
            , new State(
               new string[15] {"[exp_op0 → exp_op0 / • exp_atom, )]", "[exp_op0 → exp_op0 / • exp_atom, *]", "[exp_op0 → exp_op0 / • exp_atom, /]", "[exp_op0 → exp_op0 / • exp_atom, +]", "[exp_op0 → exp_op0 / • exp_atom, -]", "[exp_atom → • NUMBER, )]", "[exp_atom → • ( exp ), )]", "[exp_atom → • NUMBER, *]", "[exp_atom → • ( exp ), *]", "[exp_atom → • NUMBER, /]", "[exp_atom → • ( exp ), /]", "[exp_atom → • NUMBER, +]", "[exp_atom → • ( exp ), +]", "[exp_atom → • NUMBER, -]", "[exp_atom → • ( exp ), -]"},
               new Terminal[2] {new Terminal("NUMBER", 0x5), new Terminal("_T[(]", 0xC)},
               new ushort[2] {0x5, 0xc},
               new ushort[2] {0x10, 0x11},
               new ushort[1] {0x8},
               new ushort[1] {0x1F},
               new Reduction[0] {})
            , new State(
               new string[5] {"[exp_atom → ( exp • ), )]", "[exp_atom → ( exp • ), *]", "[exp_atom → ( exp • ), /]", "[exp_atom → ( exp • ), +]", "[exp_atom → ( exp • ), -]"},
               new Terminal[1] {new Terminal("_T[)]", 0xD)},
               new ushort[1] {0xd},
               new ushort[1] {0x20},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               new string[13] {"[exp_op1 → exp_op1 + exp_op0 •, )]", "[exp_op1 → exp_op1 + exp_op0 •, +]", "[exp_op1 → exp_op1 + exp_op0 •, -]", "[exp_op0 → exp_op0 • * exp_atom, )]", "[exp_op0 → exp_op0 • / exp_atom, )]", "[exp_op0 → exp_op0 • * exp_atom, +]", "[exp_op0 → exp_op0 • / exp_atom, +]", "[exp_op0 → exp_op0 • * exp_atom, -]", "[exp_op0 → exp_op0 • / exp_atom, -]", "[exp_op0 → exp_op0 • * exp_atom, *]", "[exp_op0 → exp_op0 • / exp_atom, *]", "[exp_op0 → exp_op0 • * exp_atom, /]", "[exp_op0 → exp_op0 • / exp_atom, /]"},
               new Terminal[5] {new Terminal("_T[)]", 0xD), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11)},
               new ushort[2] {0xe, 0xf},
               new ushort[2] {0x19, 0x1A},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0xd, staticRules[0x6]), new Reduction(0x10, staticRules[0x6]), new Reduction(0x11, staticRules[0x6])})
            , new State(
               new string[13] {"[exp_op1 → exp_op1 - exp_op0 •, )]", "[exp_op1 → exp_op1 - exp_op0 •, +]", "[exp_op1 → exp_op1 - exp_op0 •, -]", "[exp_op0 → exp_op0 • * exp_atom, )]", "[exp_op0 → exp_op0 • / exp_atom, )]", "[exp_op0 → exp_op0 • * exp_atom, +]", "[exp_op0 → exp_op0 • / exp_atom, +]", "[exp_op0 → exp_op0 • * exp_atom, -]", "[exp_op0 → exp_op0 • / exp_atom, -]", "[exp_op0 → exp_op0 • * exp_atom, *]", "[exp_op0 → exp_op0 • / exp_atom, *]", "[exp_op0 → exp_op0 • * exp_atom, /]", "[exp_op0 → exp_op0 • / exp_atom, /]"},
               new Terminal[5] {new Terminal("_T[)]", 0xD), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11)},
               new ushort[2] {0xe, 0xf},
               new ushort[2] {0x19, 0x1A},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0xd, staticRules[0x7]), new Reduction(0x10, staticRules[0x7]), new Reduction(0x11, staticRules[0x7])})
            , new State(
               new string[5] {"[exp_op0 → exp_op0 * exp_atom •, )]", "[exp_op0 → exp_op0 * exp_atom •, *]", "[exp_op0 → exp_op0 * exp_atom •, /]", "[exp_op0 → exp_op0 * exp_atom •, +]", "[exp_op0 → exp_op0 * exp_atom •, -]"},
               new Terminal[5] {new Terminal("_T[)]", 0xD), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[5] {new Reduction(0xd, staticRules[0x3]), new Reduction(0xe, staticRules[0x3]), new Reduction(0xf, staticRules[0x3]), new Reduction(0x10, staticRules[0x3]), new Reduction(0x11, staticRules[0x3])})
            , new State(
               new string[5] {"[exp_op0 → exp_op0 / exp_atom •, )]", "[exp_op0 → exp_op0 / exp_atom •, *]", "[exp_op0 → exp_op0 / exp_atom •, /]", "[exp_op0 → exp_op0 / exp_atom •, +]", "[exp_op0 → exp_op0 / exp_atom •, -]"},
               new Terminal[5] {new Terminal("_T[)]", 0xD), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[5] {new Reduction(0xd, staticRules[0x4]), new Reduction(0xe, staticRules[0x4]), new Reduction(0xf, staticRules[0x4]), new Reduction(0x10, staticRules[0x4]), new Reduction(0x11, staticRules[0x4])})
            , new State(
               new string[5] {"[exp_atom → ( exp ) •, )]", "[exp_atom → ( exp ) •, *]", "[exp_atom → ( exp ) •, /]", "[exp_atom → ( exp ) •, +]", "[exp_atom → ( exp ) •, -]"},
               new Terminal[5] {new Terminal("_T[)]", 0xD), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11)},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[5] {new Reduction(0xd, staticRules[0x1]), new Reduction(0xe, staticRules[0x1]), new Reduction(0xf, staticRules[0x1]), new Reduction(0x10, staticRules[0x1]), new Reduction(0x11, staticRules[0x1])})
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
