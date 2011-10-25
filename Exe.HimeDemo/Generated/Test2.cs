using System.Collections.Generic;
using Hime.Redist.Parsers;

namespace Analyser
{
    class Test2_Lexer : LexerText
    {
        public static readonly SymbolTerminal[] terminals = {
            new SymbolTerminal(0x1, "ε"),
            new SymbolTerminal(0x2, "$"),
            new SymbolTerminal(0x7, "_T[x]"),
            new SymbolTerminal(0x8, "_T[.]"),
            new SymbolTerminal(0xA, "_T[(]"),
            new SymbolTerminal(0xB, "_T[)]"),
            new SymbolTerminal(0x14, "_T[|]"),
            new SymbolTerminal(0xC, "_T[typeof]") };
        private static LexerDFAState[] staticStates = { 
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x78, 0x78, 0x6 },
                new ushort[3] { 0x2E, 0x2E, 0x7 },
                new ushort[3] { 0x28, 0x28, 0x8 },
                new ushort[3] { 0x29, 0x29, 0x9 },
                new ushort[3] { 0x74, 0x74, 0x1 },
                new ushort[3] { 0x7C, 0x7C, 0xA }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x79, 0x79, 0x2 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x70, 0x70, 0x3 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x65, 0x65, 0x4 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x6F, 0x6F, 0x5 }},
                null),
            new LexerDFAState(new ushort[][] {
                new ushort[3] { 0x66, 0x66, 0xB }},
                null),
            new LexerDFAState(new ushort[][] {}, terminals[0x2]),
            new LexerDFAState(new ushort[][] {}, terminals[0x3]),
            new LexerDFAState(new ushort[][] {}, terminals[0x4]),
            new LexerDFAState(new ushort[][] {}, terminals[0x5]),
            new LexerDFAState(new ushort[][] {}, terminals[0x6]),
            new LexerDFAState(new ushort[][] {}, terminals[0x7]) };
        protected override void setup() {
            states = staticStates;
            subGrammars = new Dictionary<ushort, MatchSubGrammar>();
        }
        public override ILexer Clone() {
            return new Test2_Lexer(this);
        }
        public Test2_Lexer(string input) : base(new System.IO.StringReader(input)) {}
        public Test2_Lexer(System.IO.TextReader input) : base(input) {}
        public Test2_Lexer(Test2_Lexer original) : base(original) {}
    }
    class Test2_Parser : LRStarBaseParser
    {
        public static readonly SymbolVariable[] variables = {
            new SymbolVariable(0x3, "e0"), 
            new SymbolVariable(0x4, "e"), 
            new SymbolVariable(0x5, "t"), 
            new SymbolVariable(0x6, "unit"), 
            new SymbolVariable(0x15, "_m21"), 
            new SymbolVariable(0x16, "_Axiom_") };
        private static SyntaxTreeNode Production_3_0 (LRParser baseParser)
        {
            Test2_Parser parser = baseParser as Test2_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[0]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_3_1 (LRParser baseParser)
        {
            Test2_Parser parser = baseParser as Test2_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[0]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(new SyntaxTreeNode(new SymbolVirtual("member_access"), SyntaxTreeNodeAction.Promote));
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_3_2 (LRParser baseParser)
        {
            Test2_Parser parser = baseParser as Test2_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[0]);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_3_3 (LRParser baseParser)
        {
            Test2_Parser parser = baseParser as Test2_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[0]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_4_0 (LRParser baseParser)
        {
            Test2_Parser parser = baseParser as Test2_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[1]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_4_1 (LRParser baseParser)
        {
            Test2_Parser parser = baseParser as Test2_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[1]);
            root.AppendChild(new SyntaxTreeNode(new SymbolVirtual("cast"), SyntaxTreeNodeAction.Promote));
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_5_0 (LRParser baseParser)
        {
            Test2_Parser parser = baseParser as Test2_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[2]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_5_1 (LRParser baseParser)
        {
            Test2_Parser parser = baseParser as Test2_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[2]);
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            root.AppendChild(new SyntaxTreeNode(new SymbolVirtual("name_qualification"), SyntaxTreeNodeAction.Promote));
            root.AppendChild(current.Value);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static SyntaxTreeNode Production_6_0 (LRParser baseParser)
        {
            Test2_Parser parser = baseParser as Test2_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[3]);
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
        private static SyntaxTreeNode Production_15_0 (LRParser baseParser)
        {
            Test2_Parser parser = baseParser as Test2_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[4], SyntaxTreeNodeAction.Replace);
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
        private static SyntaxTreeNode Production_15_1 (LRParser baseParser)
        {
            return new SyntaxTreeNode(variables[4], SyntaxTreeNodeAction.Replace);
        }
        private static SyntaxTreeNode Production_16_0 (LRParser baseParser)
        {
            Test2_Parser parser = baseParser as Test2_Parser;
            LinkedListNode<SyntaxTreeNode> current = parser.nodes.Last;
            LinkedListNode<SyntaxTreeNode> temp = null;
            current = current.Previous;
            SyntaxTreeNode root = new SyntaxTreeNode(variables[5]);
            root.AppendChild(current.Value, SyntaxTreeNodeAction.Promote);
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            temp = current.Next;
            parser.nodes.Remove(current);
            current = temp;
            return root;
        }
        private static LRRule[] staticRules = {
           new LRRule(Production_3_0, variables[0], 1)
           , new LRRule(Production_3_1, variables[0], 3)
           , new LRRule(Production_3_2, variables[0], 3)
           , new LRRule(Production_3_3, variables[0], 4)
           , new LRRule(Production_4_0, variables[1], 1)
           , new LRRule(Production_4_1, variables[1], 4)
           , new LRRule(Production_5_0, variables[2], 1)
           , new LRRule(Production_5_1, variables[2], 3)
           , new LRRule(Production_6_0, variables[3], 2)
           , new LRRule(Production_15_0, variables[4], 3)
           , new LRRule(Production_15_1, variables[4], 0)
           , new LRRule(Production_16_0, variables[5], 2)
        };
        private static LRStarState[] staticStates = {
            new LRStarState(
               null,
               new SymbolTerminal[3] {Test2_Lexer.terminals[2], Test2_Lexer.terminals[4], Test2_Lexer.terminals[7]},
               new DeciderState[4] {
                   new DeciderState(
                   new ushort[3] {0xA, 0x7, 0xC},
                   new ushort[3] {0x1, 0x2, 0x3}, 0xFFFF, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x4, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x5, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x6, null)
               },
               new ushort[3] {0x6, 0x4, 0x3},
               new ushort[3] {0x1, 0x2, 0x3})
            , new LRStarState(
               null,
               new SymbolTerminal[1] {Test2_Lexer.terminals[1]},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[1] {0x2},
                   new ushort[1] {0x1}, 0xFFFF, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x7, null)
               },
               new ushort[0] {},
               new ushort[0] {})
            , new LRStarState(
               null,
               new SymbolTerminal[2] {Test2_Lexer.terminals[1], Test2_Lexer.terminals[6]},
               new DeciderState[3] {
                   new DeciderState(
                   new ushort[2] {0x14, 0x2},
                   new ushort[2] {0x1, 0x2}, 0xFFFF, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x9, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0xA])
               },
               new ushort[1] {0x15},
               new ushort[1] {0x8})
            , new LRStarState(
               null,
               new SymbolTerminal[4] {Test2_Lexer.terminals[1], Test2_Lexer.terminals[3], Test2_Lexer.terminals[5], Test2_Lexer.terminals[6]},
               new DeciderState[3] {
                   new DeciderState(
                   new ushort[4] {0x2, 0xB, 0x14, 0x8},
                   new ushort[4] {0x1, 0x1, 0x1, 0x2}, 0xFFFF, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0x4])
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xA, null)
               },
               new ushort[0] {},
               new ushort[0] {})
            , new LRStarState(
               null,
               new SymbolTerminal[3] {Test2_Lexer.terminals[2], Test2_Lexer.terminals[4], Test2_Lexer.terminals[7]},
               new DeciderState[4] {
                   new DeciderState(
                   new ushort[3] {0x7, 0xA, 0xC},
                   new ushort[3] {0x1, 0x2, 0x3}, 0xFFFF, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xD, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x4, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x6, null)
               },
               new ushort[3] {0x5, 0x4, 0x3},
               new ushort[3] {0xB, 0xC, 0x3})
            , new LRStarState(
               null,
               new SymbolTerminal[4] {Test2_Lexer.terminals[1], Test2_Lexer.terminals[3], Test2_Lexer.terminals[5], Test2_Lexer.terminals[6]},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[4] {0x2, 0x8, 0xB, 0x14},
                   new ushort[4] {0x1, 0x1, 0x1, 0x1}, 0xFFFF, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0x0])
               },
               new ushort[0] {},
               new ushort[0] {})
            , new LRStarState(
               null,
               new SymbolTerminal[1] {Test2_Lexer.terminals[4]},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[1] {0xA},
                   new ushort[1] {0x1}, 0xFFFF, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xE, null)
               },
               new ushort[0] {},
               new ushort[0] {})
            , new LRStarState(
               null,
               new SymbolTerminal[1] {Test2_Lexer.terminals[0]},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[1] {0x1},
                   new ushort[1] {0x1}, 0xFFFF, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0xB])
               },
               new ushort[0] {},
               new ushort[0] {})
            , new LRStarState(
               null,
               new SymbolTerminal[1] {Test2_Lexer.terminals[1]},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[1] {0x2},
                   new ushort[1] {0x1}, 0xFFFF, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0x8])
               },
               new ushort[0] {},
               new ushort[0] {})
            , new LRStarState(
               null,
               new SymbolTerminal[3] {Test2_Lexer.terminals[2], Test2_Lexer.terminals[4], Test2_Lexer.terminals[7]},
               new DeciderState[4] {
                   new DeciderState(
                   new ushort[3] {0xA, 0x7, 0xC},
                   new ushort[3] {0x1, 0x2, 0x3}, 0xFFFF, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x4, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x5, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x6, null)
               },
               new ushort[2] {0x4, 0x3},
               new ushort[2] {0xF, 0x3})
            , new LRStarState(
               null,
               new SymbolTerminal[1] {Test2_Lexer.terminals[2]},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[1] {0x7},
                   new ushort[1] {0x1}, 0xFFFF, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x10, null)
               },
               new ushort[0] {},
               new ushort[0] {})
            , new LRStarState(
               null,
               new SymbolTerminal[2] {Test2_Lexer.terminals[3], Test2_Lexer.terminals[5]},
               new DeciderState[3] {
                   new DeciderState(
                   new ushort[2] {0xB, 0x8},
                   new ushort[2] {0x1, 0x2}, 0xFFFF, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x11, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x12, null)
               },
               new ushort[0] {},
               new ushort[0] {})
            , new LRStarState(
               null,
               new SymbolTerminal[1] {Test2_Lexer.terminals[5]},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[1] {0xB},
                   new ushort[1] {0x1}, 0xFFFF, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x13, null)
               },
               new ushort[0] {},
               new ushort[0] {})
            , new LRStarState(
               null,
               new SymbolTerminal[2] {Test2_Lexer.terminals[3], Test2_Lexer.terminals[5]},
               new DeciderState[6] {
                   new DeciderState(
                   new ushort[2] {0x8, 0xB},
                   new ushort[2] {0x1, 0x2}, 0xFFFF, null)
                   , new DeciderState(
                   new ushort[1] {0x7},
                   new ushort[1] {0x3}, 0xFFFF, null)
                   , new DeciderState(
                   new ushort[7] {0xA, 0x7, 0xC, 0x2, 0x8, 0xB, 0x14},
                   new ushort[7] {0x4, 0x4, 0x4, 0x5, 0x5, 0x5, 0x5}, 0xFFFF, null)
                   , new DeciderState(
                   new ushort[4] {0x2, 0x14, 0x8, 0xB},
                   new ushort[4] {0x5, 0x5, 0x1, 0x2}, 0xFFFF, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0x6])
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0x0])
               },
               new ushort[0] {},
               new ushort[0] {})
            , new LRStarState(
               null,
               new SymbolTerminal[1] {Test2_Lexer.terminals[2]},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[1] {0x7},
                   new ushort[1] {0x1}, 0xFFFF, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x15, null)
               },
               new ushort[1] {0x5},
               new ushort[1] {0x14})
            , new LRStarState(
               null,
               new SymbolTerminal[2] {Test2_Lexer.terminals[1], Test2_Lexer.terminals[6]},
               new DeciderState[3] {
                   new DeciderState(
                   new ushort[2] {0x14, 0x2},
                   new ushort[2] {0x1, 0x2}, 0xFFFF, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x9, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0xA])
               },
               new ushort[1] {0x15},
               new ushort[1] {0x16})
            , new LRStarState(
               null,
               new SymbolTerminal[4] {Test2_Lexer.terminals[1], Test2_Lexer.terminals[3], Test2_Lexer.terminals[5], Test2_Lexer.terminals[6]},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[4] {0x2, 0x8, 0xB, 0x14},
                   new ushort[4] {0x1, 0x1, 0x1, 0x1}, 0xFFFF, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0x1])
               },
               new ushort[0] {},
               new ushort[0] {})
            , new LRStarState(
               null,
               new SymbolTerminal[3] {Test2_Lexer.terminals[2], Test2_Lexer.terminals[4], Test2_Lexer.terminals[7]},
               new DeciderState[4] {
                   new DeciderState(
                   new ushort[3] {0xA, 0x7, 0xC},
                   new ushort[3] {0x1, 0x2, 0x3}, 0xFFFF, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x4, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x5, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x6, null)
               },
               new ushort[2] {0x4, 0x3},
               new ushort[2] {0x17, 0x3})
            , new LRStarState(
               null,
               new SymbolTerminal[1] {Test2_Lexer.terminals[2]},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[1] {0x7},
                   new ushort[1] {0x1}, 0xFFFF, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x18, null)
               },
               new ushort[0] {},
               new ushort[0] {})
            , new LRStarState(
               null,
               new SymbolTerminal[4] {Test2_Lexer.terminals[1], Test2_Lexer.terminals[3], Test2_Lexer.terminals[5], Test2_Lexer.terminals[6]},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[4] {0x2, 0x8, 0xB, 0x14},
                   new ushort[4] {0x1, 0x1, 0x1, 0x1}, 0xFFFF, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0x2])
               },
               new ushort[0] {},
               new ushort[0] {})
            , new LRStarState(
               null,
               new SymbolTerminal[2] {Test2_Lexer.terminals[3], Test2_Lexer.terminals[5]},
               new DeciderState[3] {
                   new DeciderState(
                   new ushort[2] {0xB, 0x8},
                   new ushort[2] {0x1, 0x2}, 0xFFFF, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x19, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0x12, null)
               },
               new ushort[0] {},
               new ushort[0] {})
            , new LRStarState(
               null,
               new SymbolTerminal[2] {Test2_Lexer.terminals[3], Test2_Lexer.terminals[5]},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[2] {0x8, 0xB},
                   new ushort[2] {0x1, 0x1}, 0xFFFF, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0x6])
               },
               new ushort[0] {},
               new ushort[0] {})
            , new LRStarState(
               null,
               new SymbolTerminal[1] {Test2_Lexer.terminals[1]},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[1] {0x2},
                   new ushort[1] {0x1}, 0xFFFF, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0x9])
               },
               new ushort[0] {},
               new ushort[0] {})
            , new LRStarState(
               null,
               new SymbolTerminal[3] {Test2_Lexer.terminals[1], Test2_Lexer.terminals[5], Test2_Lexer.terminals[6]},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[3] {0x2, 0xB, 0x14},
                   new ushort[3] {0x1, 0x1, 0x1}, 0xFFFF, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0x5])
               },
               new ushort[0] {},
               new ushort[0] {})
            , new LRStarState(
               null,
               new SymbolTerminal[2] {Test2_Lexer.terminals[3], Test2_Lexer.terminals[5]},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[2] {0x8, 0xB},
                   new ushort[2] {0x1, 0x1}, 0xFFFF, null)
                   , new DeciderState(
                   new ushort[0] {},
                   new ushort[0] {}, 0xFFFF, staticRules[0x7])
               },
               new ushort[0] {},
               new ushort[0] {})
            , new LRStarState(
               null,
               new SymbolTerminal[4] {Test2_Lexer.terminals[1], Test2_Lexer.terminals[3], Test2_Lexer.terminals[5], Test2_Lexer.terminals[6]},
               new DeciderState[2] {
                   new DeciderState(
                   new ushort[4] {0x2, 0x8, 0xB, 0x14},
                   new ushort[4] {0x1, 0x1, 0x1, 0x1}, 0xFFFF, null)
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
