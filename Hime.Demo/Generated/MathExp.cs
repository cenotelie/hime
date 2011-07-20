using System.Collections.Generic;
using Hime.Redist.Parsers;

namespace Analyser
{
    class MathExp_Lexer : LexerText
    {
        public static readonly SymbolTerminal[] terminals = {
            new SymbolTerminal(0x1, "ε"),
            new SymbolTerminal(0x2, "$"),
            new SymbolTerminal(0xC, "_T[(]"),
            new SymbolTerminal(0xD, "_T[)]"),
            new SymbolTerminal(0xE, "_T[*]"),
            new SymbolTerminal(0xF, "_T[/]"),
            new SymbolTerminal(0x10, "_T[+]"),
            new SymbolTerminal(0x11, "_T[-]"),
            new SymbolTerminal(0x5, "NUMBER"),
            new SymbolTerminal(0x7, "SEPARATOR") };
        private static LexerDFAState[] staticStates = { 
            new LexerDFAState(new ushort[][] {
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
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x31, 0x39, 0xE },
                new ushort[3] { 0x30, 0x30, 0xF }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x31, 0x39, 0x10 },
                new ushort[3] { 0x30, 0x30, 0x11 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x2B, 0x2B, 0x2 },
                new ushort[3] { 0x2D, 0x2D, 0x2 },
                new ushort[3] { 0x31, 0x39, 0x10 },
                new ushort[3] { 0x30, 0x30, 0x11 }},
                null),
            new LexerDFAState(new ushort[][] {}, terminals[0x2]),
            new LexerDFAState(new ushort[][] {}, terminals[0x3]),
            new LexerDFAState(new ushort[][] {}, terminals[0x4]),
            new LexerDFAState(new ushort[][] {}, terminals[0x5]),
            new LexerDFAState(new ushort[][] {}, terminals[0x6]),
            new LexerDFAState(new ushort[][] {}, terminals[0x7]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0xA },
                new ushort[3] { 0x45, 0x45, 0x3 },
                new ushort[3] { 0x65, 0x65, 0x3 },
                new ushort[3] { 0x2E, 0x2E, 0x1 }},
                terminals[0x8]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x45, 0x45, 0x3 },
                new ushort[3] { 0x65, 0x65, 0x3 },
                new ushort[3] { 0x2E, 0x2E, 0x1 }},
                terminals[0x8]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x9, 0x9, 0xD },
                new ushort[3] { 0xB, 0xC, 0xD },
                new ushort[3] { 0x20, 0x20, 0xD }},
                terminals[0x9]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x9, 0x9, 0xD },
                new ushort[3] { 0xB, 0xC, 0xD },
                new ushort[3] { 0x20, 0x20, 0xD }},
                terminals[0x9]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0xE },
                new ushort[3] { 0x45, 0x45, 0x3 },
                new ushort[3] { 0x65, 0x65, 0x3 }},
                terminals[0x8]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x45, 0x45, 0x3 },
                new ushort[3] { 0x65, 0x65, 0x3 }},
                terminals[0x8]),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x30, 0x39, 0x10 }},
                terminals[0x8]),
            new LexerDFAState(new ushort[][] {}, terminals[0x8]) };
        protected override void setup() {
            states = staticStates;
            subGrammars = new Dictionary<ushort, MatchSubGrammar>();
            separatorID = 0x7;
        }
        public override ILexer Clone() {
            return new MathExp_Lexer(this);
        }
        public MathExp_Lexer(string input) : base(new System.IO.StringReader(input)) {}
        public MathExp_Lexer(System.IO.TextReader input) : base(input) {}
        public MathExp_Lexer(MathExp_Lexer original) : base(original) {}
    }
    class MathExp_Parser : LR1TextParser
    {
        public static readonly SymbolVariable[] variables = {
            new SymbolVariable(0x8, "exp_atom"), 
            new SymbolVariable(0x9, "exp_op0"), 
            new SymbolVariable(0xA, "exp_op1"), 
            new SymbolVariable(0xB, "exp"), 
            new SymbolVariable(0x12, "_Axiom_") };
        private static SyntaxTreeNode Production_8_0 (BaseLR1Parser baseParser)
        {
            MathExp_Parser parser = baseParser as MathExp_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[0]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            parser.actions.OnNumber(root);
            return root;
        }
        private static SyntaxTreeNode Production_8_1 (BaseLR1Parser baseParser)
        {
            MathExp_Parser parser = baseParser as MathExp_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[0]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_9_0 (BaseLR1Parser baseParser)
        {
            MathExp_Parser parser = baseParser as MathExp_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[1]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_9_1 (BaseLR1Parser baseParser)
        {
            MathExp_Parser parser = baseParser as MathExp_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[1]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            parser.actions.OnMult(root);
            return root;
        }
        private static SyntaxTreeNode Production_9_2 (BaseLR1Parser baseParser)
        {
            MathExp_Parser parser = baseParser as MathExp_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[1]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            parser.actions.OnDiv(root);
            return root;
        }
        private static SyntaxTreeNode Production_A_0 (BaseLR1Parser baseParser)
        {
            MathExp_Parser parser = baseParser as MathExp_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[2]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_A_1 (BaseLR1Parser baseParser)
        {
            MathExp_Parser parser = baseParser as MathExp_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[2]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            parser.actions.OnPlus(root);
            return root;
        }
        private static SyntaxTreeNode Production_A_2 (BaseLR1Parser baseParser)
        {
            MathExp_Parser parser = baseParser as MathExp_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[2]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            parser.actions.OnMinus(root);
            return root;
        }
        private static SyntaxTreeNode Production_B_0 (BaseLR1Parser baseParser)
        {
            MathExp_Parser parser = baseParser as MathExp_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[3]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_12_0 (BaseLR1Parser baseParser)
        {
            MathExp_Parser parser = baseParser as MathExp_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[4]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
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
               new SymbolTerminal[2] {MathExp_Lexer.terminals[8], MathExp_Lexer.terminals[2]},
               new ushort[2] {0x5, 0xc},
               new ushort[2] {0x5, 0x6},
               new ushort[4] {0xb, 0xa, 0x9, 0x8},
               new ushort[4] {0x1, 0x2, 0x3, 0x4},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {MathExp_Lexer.terminals[1]},
               new ushort[1] {0x2},
               new ushort[1] {0x7},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[4] {MathExp_Lexer.terminals[1], MathExp_Lexer.terminals[3], MathExp_Lexer.terminals[6], MathExp_Lexer.terminals[7]},
               new ushort[2] {0x10, 0x11},
               new ushort[2] {0x8, 0x9},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[2] {new Reduction(0x2, staticRules[0x8]), new Reduction(0xd, staticRules[0x8])})
            , new State(
               null,
               new SymbolTerminal[6] {MathExp_Lexer.terminals[1], MathExp_Lexer.terminals[3], MathExp_Lexer.terminals[4], MathExp_Lexer.terminals[5], MathExp_Lexer.terminals[6], MathExp_Lexer.terminals[7]},
               new ushort[2] {0xe, 0xf},
               new ushort[2] {0xA, 0xB},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[4] {new Reduction(0x2, staticRules[0x5]), new Reduction(0xd, staticRules[0x5]), new Reduction(0x10, staticRules[0x5]), new Reduction(0x11, staticRules[0x5])})
            , new State(
               null,
               new SymbolTerminal[6] {MathExp_Lexer.terminals[1], MathExp_Lexer.terminals[3], MathExp_Lexer.terminals[4], MathExp_Lexer.terminals[5], MathExp_Lexer.terminals[6], MathExp_Lexer.terminals[7]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[6] {new Reduction(0x2, staticRules[0x2]), new Reduction(0xd, staticRules[0x2]), new Reduction(0xe, staticRules[0x2]), new Reduction(0xf, staticRules[0x2]), new Reduction(0x10, staticRules[0x2]), new Reduction(0x11, staticRules[0x2])})
            , new State(
               null,
               new SymbolTerminal[6] {MathExp_Lexer.terminals[1], MathExp_Lexer.terminals[3], MathExp_Lexer.terminals[4], MathExp_Lexer.terminals[5], MathExp_Lexer.terminals[6], MathExp_Lexer.terminals[7]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[6] {new Reduction(0x2, staticRules[0x0]), new Reduction(0xd, staticRules[0x0]), new Reduction(0xe, staticRules[0x0]), new Reduction(0xf, staticRules[0x0]), new Reduction(0x10, staticRules[0x0]), new Reduction(0x11, staticRules[0x0])})
            , new State(
               null,
               new SymbolTerminal[2] {MathExp_Lexer.terminals[8], MathExp_Lexer.terminals[2]},
               new ushort[2] {0x5, 0xc},
               new ushort[2] {0x5, 0x6},
               new ushort[4] {0xb, 0xa, 0x9, 0x8},
               new ushort[4] {0xC, 0x2, 0x3, 0x4},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {MathExp_Lexer.terminals[0]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x1, staticRules[0x9])})
            , new State(
               null,
               new SymbolTerminal[2] {MathExp_Lexer.terminals[8], MathExp_Lexer.terminals[2]},
               new ushort[2] {0x5, 0xc},
               new ushort[2] {0x5, 0x6},
               new ushort[2] {0x9, 0x8},
               new ushort[2] {0xD, 0x4},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[2] {MathExp_Lexer.terminals[8], MathExp_Lexer.terminals[2]},
               new ushort[2] {0x5, 0xc},
               new ushort[2] {0x5, 0x6},
               new ushort[2] {0x9, 0x8},
               new ushort[2] {0xE, 0x4},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[2] {MathExp_Lexer.terminals[8], MathExp_Lexer.terminals[2]},
               new ushort[2] {0x5, 0xc},
               new ushort[2] {0x5, 0x6},
               new ushort[1] {0x8},
               new ushort[1] {0xF},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[2] {MathExp_Lexer.terminals[8], MathExp_Lexer.terminals[2]},
               new ushort[2] {0x5, 0xc},
               new ushort[2] {0x5, 0x6},
               new ushort[1] {0x8},
               new ushort[1] {0x10},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[1] {MathExp_Lexer.terminals[3]},
               new ushort[1] {0xd},
               new ushort[1] {0x11},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               new SymbolTerminal[6] {MathExp_Lexer.terminals[1], MathExp_Lexer.terminals[3], MathExp_Lexer.terminals[4], MathExp_Lexer.terminals[5], MathExp_Lexer.terminals[6], MathExp_Lexer.terminals[7]},
               new ushort[2] {0xe, 0xf},
               new ushort[2] {0xA, 0xB},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[4] {new Reduction(0x2, staticRules[0x6]), new Reduction(0xd, staticRules[0x6]), new Reduction(0x10, staticRules[0x6]), new Reduction(0x11, staticRules[0x6])})
            , new State(
               null,
               new SymbolTerminal[6] {MathExp_Lexer.terminals[1], MathExp_Lexer.terminals[3], MathExp_Lexer.terminals[4], MathExp_Lexer.terminals[5], MathExp_Lexer.terminals[6], MathExp_Lexer.terminals[7]},
               new ushort[2] {0xe, 0xf},
               new ushort[2] {0xA, 0xB},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[4] {new Reduction(0x2, staticRules[0x7]), new Reduction(0xd, staticRules[0x7]), new Reduction(0x10, staticRules[0x7]), new Reduction(0x11, staticRules[0x7])})
            , new State(
               null,
               new SymbolTerminal[6] {MathExp_Lexer.terminals[1], MathExp_Lexer.terminals[3], MathExp_Lexer.terminals[4], MathExp_Lexer.terminals[5], MathExp_Lexer.terminals[6], MathExp_Lexer.terminals[7]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[6] {new Reduction(0x2, staticRules[0x3]), new Reduction(0xd, staticRules[0x3]), new Reduction(0xe, staticRules[0x3]), new Reduction(0xf, staticRules[0x3]), new Reduction(0x10, staticRules[0x3]), new Reduction(0x11, staticRules[0x3])})
            , new State(
               null,
               new SymbolTerminal[6] {MathExp_Lexer.terminals[1], MathExp_Lexer.terminals[3], MathExp_Lexer.terminals[4], MathExp_Lexer.terminals[5], MathExp_Lexer.terminals[6], MathExp_Lexer.terminals[7]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[6] {new Reduction(0x2, staticRules[0x4]), new Reduction(0xd, staticRules[0x4]), new Reduction(0xe, staticRules[0x4]), new Reduction(0xf, staticRules[0x4]), new Reduction(0x10, staticRules[0x4]), new Reduction(0x11, staticRules[0x4])})
            , new State(
               null,
               new SymbolTerminal[6] {MathExp_Lexer.terminals[1], MathExp_Lexer.terminals[3], MathExp_Lexer.terminals[4], MathExp_Lexer.terminals[5], MathExp_Lexer.terminals[6], MathExp_Lexer.terminals[7]},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[6] {new Reduction(0x2, staticRules[0x1]), new Reduction(0xd, staticRules[0x1]), new Reduction(0xe, staticRules[0x1]), new Reduction(0xf, staticRules[0x1]), new Reduction(0x10, staticRules[0x1]), new Reduction(0x11, staticRules[0x1])})
        };
        public interface Actions
        {
            void OnNumber(SyntaxTreeNode SubRoot);
            void OnMult(SyntaxTreeNode SubRoot);
            void OnDiv(SyntaxTreeNode SubRoot);
            void OnPlus(SyntaxTreeNode SubRoot);
            void OnMinus(SyntaxTreeNode SubRoot);
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
