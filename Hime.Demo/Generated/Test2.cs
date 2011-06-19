using System.Collections.Generic;

namespace Analyser
{
    class Test2_Lexer : Hime.Redist.Parsers.LexerText
    {
        private static ushort[] staticSymbolsSID = { 0x6, 0x7, 0x9, 0xA, 0xB };
        private static string[] staticSymbolsName = { "_T[x]", "_T[.]", "_T[(]", "_T[)]", "_T[typeof]" };
        private static ushort[][] staticTransitions0 = { new ushort[3] { 0x78, 0x78, 0x6 }, new ushort[3] { 0x2E, 0x2E, 0x7 }, new ushort[3] { 0x28, 0x28, 0x8 }, new ushort[3] { 0x29, 0x29, 0x9 }, new ushort[3] { 0x74, 0x74, 0x1 } };
        private static ushort[][] staticTransitions1 = { new ushort[3] { 0x79, 0x79, 0x2 } };
        private static ushort[][] staticTransitions2 = { new ushort[3] { 0x70, 0x70, 0x3 } };
        private static ushort[][] staticTransitions3 = { new ushort[3] { 0x65, 0x65, 0x4 } };
        private static ushort[][] staticTransitions4 = { new ushort[3] { 0x6F, 0x6F, 0x5 } };
        private static ushort[][] staticTransitions5 = { new ushort[3] { 0x66, 0x66, 0xA } };
        private static ushort[][] staticTransitions6 = {  };
        private static ushort[][] staticTransitions7 = {  };
        private static ushort[][] staticTransitions8 = {  };
        private static ushort[][] staticTransitions9 = {  };
        private static ushort[][] staticTransitionsA = {  };
        private static ushort[][][] staticTransitions = { staticTransitions0, staticTransitions1, staticTransitions2, staticTransitions3, staticTransitions4, staticTransitions5, staticTransitions6, staticTransitions7, staticTransitions8, staticTransitions9, staticTransitionsA };
        private static int[] staticFinals = { -1, -1, -1, -1, -1, -1, 0, 1, 2, 3, 4 };
        protected override void setup() {
            symbolsSID = staticSymbolsSID;
            symbolsName = staticSymbolsName;
            symbolsSubGrammars = new Dictionary<ushort, MatchSubGrammar>();
            transitions = staticTransitions;
            finals = staticFinals;
        }
        public override Hime.Redist.Parsers.ILexer Clone() {
            return new Test2_Lexer(this);
        }
        public Test2_Lexer(string input) : base(new System.IO.StringReader(input)) {}
        public Test2_Lexer(System.IO.TextReader input) : base(input) {}
        public Test2_Lexer(Test2_Lexer original) : base(original) {}
    }
    class Test2_Parser : Hime.Redist.Parsers.BaseRNGLR1Parser
    {
        private static Hime.Redist.Parsers.SymbolTerminal[] staticTerminals = {
            new Hime.Redist.Parsers.SymbolTerminal("ε", 0x1)
            , new Hime.Redist.Parsers.SymbolTerminal("$", 0x2)
            , new Hime.Redist.Parsers.SymbolTerminal("_T[x]", 0x6)
            , new Hime.Redist.Parsers.SymbolTerminal("_T[.]", 0x7)
            , new Hime.Redist.Parsers.SymbolTerminal("_T[(]", 0x9)
            , new Hime.Redist.Parsers.SymbolTerminal("_T[)]", 0xA)
            , new Hime.Redist.Parsers.SymbolTerminal("_T[typeof]", 0xB)
        };
        private static Hime.Redist.Parsers.SymbolVariable[] staticVariables = {
            new Hime.Redist.Parsers.SymbolVariable(0x3, "e0")
            , new Hime.Redist.Parsers.SymbolVariable(0x4, "e")
            , new Hime.Redist.Parsers.SymbolVariable(0x5, "t")
            , new Hime.Redist.Parsers.SymbolVariable(0x13, "_Axiom_")
        };
        private static Hime.Redist.Parsers.SPPFNode[] staticNullVarsSPPF = {  };
        private static Hime.Redist.Parsers.SPPFNode[] staticNullChoicesSPPF = { new Hime.Redist.Parsers.SPPFNode(null, 0, Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace) };
        private static void Production_3_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_3_1 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            family.AddChild(nodes[1]);
            family.AddChild(nodes[2]);
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_3_2 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            family.AddChild(nodes[1]);
            family.AddChild(nodes[2]);
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_3_3 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            family.AddChild(nodes[1]);
            family.AddChild(nodes[2]);
            family.AddChild(nodes[3]);
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_4_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_4_1 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            family.AddChild(nodes[1]);
            family.AddChild(nodes[2]);
            family.AddChild(nodes[3]);
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_5_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_5_1 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            family.AddChild(nodes[1]);
            family.AddChild(nodes[2]);
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_13_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            nodes[0].Action = Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote;
            family.AddChild(nodes[0]);
            nodes[1].Action = Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop;
            family.AddChild(nodes[1]);
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static Rule[] staticRules = {
           new Rule(Production_3_0, staticVariables[0])
           , new Rule(Production_3_1, staticVariables[0])
           , new Rule(Production_3_2, staticVariables[0])
           , new Rule(Production_3_3, staticVariables[0])
           , new Rule(Production_4_0, staticVariables[1])
           , new Rule(Production_4_1, staticVariables[1])
           , new Rule(Production_5_0, staticVariables[2])
           , new Rule(Production_5_1, staticVariables[2])
           , new Rule(Production_13_0, staticVariables[3])
        };
        private static State[] staticStates = {
            new State(
               null,
               null,
               new ushort[3] {0x9, 0x6, 0xb},
               new ushort[3] {0x3, 0x4, 0x5},
               new ushort[2] {0x4, 0x3},
               new ushort[2] {0x1, 0x2},
               new Reduction[0] {})
            , new State(
               null,
               null,
               new ushort[1] {0x2},
               new ushort[1] {0x6},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               null,
               new ushort[1] {0x7},
               new ushort[1] {0x7},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[2] {new Reduction(0x2, staticRules[0x4], 0x1, staticNullChoicesSPPF[0x0]), new Reduction(0xa, staticRules[0x4], 0x1, staticNullChoicesSPPF[0x0])})
            , new State(
               null,
               null,
               new ushort[3] {0x6, 0x9, 0xb},
               new ushort[3] {0xA, 0x3, 0x5},
               new ushort[3] {0x5, 0x4, 0x3},
               new ushort[3] {0x8, 0x9, 0x2},
               new Reduction[0] {})
            , new State(
               null,
               null,
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0x2, staticRules[0x0], 0x1, staticNullChoicesSPPF[0x0]), new Reduction(0x7, staticRules[0x0], 0x1, staticNullChoicesSPPF[0x0]), new Reduction(0xa, staticRules[0x0], 0x1, staticNullChoicesSPPF[0x0])})
            , new State(
               null,
               null,
               new ushort[1] {0x9},
               new ushort[1] {0xB},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               null,
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[1] {new Reduction(0x1, staticRules[0x8], 0x2, staticNullChoicesSPPF[0x0])})
            , new State(
               null,
               null,
               new ushort[1] {0x6},
               new ushort[1] {0xC},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               null,
               new ushort[2] {0xa, 0x7},
               new ushort[2] {0xD, 0xE},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               null,
               new ushort[1] {0xa},
               new ushort[1] {0xF},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               null,
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[4] {new Reduction(0x7, staticRules[0x6], 0x1, staticNullChoicesSPPF[0x0]), new Reduction(0xa, staticRules[0x6], 0x1, staticNullChoicesSPPF[0x0]), new Reduction(0x7, staticRules[0x0], 0x1, staticNullChoicesSPPF[0x0]), new Reduction(0xa, staticRules[0x0], 0x1, staticNullChoicesSPPF[0x0])})
            , new State(
               null,
               null,
               new ushort[1] {0x6},
               new ushort[1] {0x11},
               new ushort[1] {0x5},
               new ushort[1] {0x10},
               new Reduction[0] {})
            , new State(
               null,
               null,
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0x2, staticRules[0x1], 0x3, staticNullChoicesSPPF[0x0]), new Reduction(0x7, staticRules[0x1], 0x3, staticNullChoicesSPPF[0x0]), new Reduction(0xa, staticRules[0x1], 0x3, staticNullChoicesSPPF[0x0])})
            , new State(
               null,
               null,
               new ushort[3] {0x9, 0x6, 0xb},
               new ushort[3] {0x3, 0x4, 0x5},
               new ushort[2] {0x4, 0x3},
               new ushort[2] {0x12, 0x2},
               new Reduction[0] {})
            , new State(
               null,
               null,
               new ushort[1] {0x6},
               new ushort[1] {0x13},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               null,
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0x2, staticRules[0x2], 0x3, staticNullChoicesSPPF[0x0]), new Reduction(0x7, staticRules[0x2], 0x3, staticNullChoicesSPPF[0x0]), new Reduction(0xa, staticRules[0x2], 0x3, staticNullChoicesSPPF[0x0])})
            , new State(
               null,
               null,
               new ushort[2] {0xa, 0x7},
               new ushort[2] {0x14, 0xE},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[0] {})
            , new State(
               null,
               null,
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[2] {new Reduction(0x7, staticRules[0x6], 0x1, staticNullChoicesSPPF[0x0]), new Reduction(0xa, staticRules[0x6], 0x1, staticNullChoicesSPPF[0x0])})
            , new State(
               null,
               null,
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[2] {new Reduction(0x2, staticRules[0x5], 0x4, staticNullChoicesSPPF[0x0]), new Reduction(0xa, staticRules[0x5], 0x4, staticNullChoicesSPPF[0x0])})
            , new State(
               null,
               null,
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[2] {new Reduction(0x7, staticRules[0x7], 0x3, staticNullChoicesSPPF[0x0]), new Reduction(0xa, staticRules[0x7], 0x3, staticNullChoicesSPPF[0x0])})
            , new State(
               null,
               null,
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new ushort[0] {},
               new Reduction[3] {new Reduction(0x2, staticRules[0x3], 0x4, staticNullChoicesSPPF[0x0]), new Reduction(0x7, staticRules[0x3], 0x4, staticNullChoicesSPPF[0x0]), new Reduction(0xa, staticRules[0x3], 0x4, staticNullChoicesSPPF[0x0])})
        };
        private static void BuildNullables() { 
            List<Hime.Redist.Parsers.SPPFNode> temp = new List<Hime.Redist.Parsers.SPPFNode>();
            staticNullChoicesSPPF[0].AddFamily(temp);
            temp.Clear();
        }
        protected override void setup()
        {
            nullVarsSPPF = staticNullVarsSPPF;
            nullChoicesSPPF = staticNullChoicesSPPF;
            rules = staticRules;
            states = staticStates;
            axiomID = 0x4;
            axiomNullSPPF = 0x4;
            axiomPrimeID = 0x13;
        }
        static Test2_Parser()
        {
            BuildNullables();
        }
        public Test2_Parser(Test2_Lexer lexer) : base (lexer) {}
    }
}
