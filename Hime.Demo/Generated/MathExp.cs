namespace Analyser
{
    class MathExp_Lexer : Hime.Redist.Parsers.LexerText
    {
        private static ushort[] p_StaticSymbolsSID = { 0xC, 0xD, 0xE, 0xF, 0x10, 0x11, 0x5, 0x7 };
        private static string[] p_StaticSymbolsName = { "_T[(]", "_T[)]", "_T[*]", "_T[/]", "_T[+]", "_T[-]", "NUMBER", "SEPARATOR" };
        private static ushort[][] p_StaticTransitions0 = { new ushort[3] { 0x28, 0x28, 0x4 }, new ushort[3] { 0x29, 0x29, 0x5 }, new ushort[3] { 0x2A, 0x2A, 0x6 }, new ushort[3] { 0x2F, 0x2F, 0x7 }, new ushort[3] { 0x2B, 0x2B, 0x8 }, new ushort[3] { 0x2D, 0x2D, 0x9 }, new ushort[3] { 0x31, 0x39, 0xA }, new ushort[3] { 0x30, 0x30, 0xB }, new ushort[3] { 0x9, 0x9, 0xC }, new ushort[3] { 0xB, 0xC, 0xC }, new ushort[3] { 0x20, 0x20, 0xC }, new ushort[3] { 0x2E, 0x2E, 0x1 } };
        private static ushort[][] p_StaticTransitions1 = { new ushort[3] { 0x31, 0x39, 0xE }, new ushort[3] { 0x30, 0x30, 0xF } };
        private static ushort[][] p_StaticTransitions2 = { new ushort[3] { 0x31, 0x39, 0x10 }, new ushort[3] { 0x30, 0x30, 0x11 } };
        private static ushort[][] p_StaticTransitions3 = { new ushort[3] { 0x2B, 0x2B, 0x2 }, new ushort[3] { 0x2D, 0x2D, 0x2 }, new ushort[3] { 0x31, 0x39, 0x10 }, new ushort[3] { 0x30, 0x30, 0x11 } };
        private static ushort[][] p_StaticTransitions4 = {  };
        private static ushort[][] p_StaticTransitions5 = {  };
        private static ushort[][] p_StaticTransitions6 = {  };
        private static ushort[][] p_StaticTransitions7 = {  };
        private static ushort[][] p_StaticTransitions8 = {  };
        private static ushort[][] p_StaticTransitions9 = {  };
        private static ushort[][] p_StaticTransitionsA = { new ushort[3] { 0x30, 0x39, 0xA }, new ushort[3] { 0x45, 0x45, 0x3 }, new ushort[3] { 0x65, 0x65, 0x3 }, new ushort[3] { 0x2E, 0x2E, 0x1 } };
        private static ushort[][] p_StaticTransitionsB = { new ushort[3] { 0x45, 0x45, 0x3 }, new ushort[3] { 0x65, 0x65, 0x3 }, new ushort[3] { 0x2E, 0x2E, 0x1 } };
        private static ushort[][] p_StaticTransitionsC = { new ushort[3] { 0x9, 0x9, 0xD }, new ushort[3] { 0xB, 0xC, 0xD }, new ushort[3] { 0x20, 0x20, 0xD } };
        private static ushort[][] p_StaticTransitionsD = { new ushort[3] { 0x9, 0x9, 0xD }, new ushort[3] { 0xB, 0xC, 0xD }, new ushort[3] { 0x20, 0x20, 0xD } };
        private static ushort[][] p_StaticTransitionsE = { new ushort[3] { 0x30, 0x39, 0xE }, new ushort[3] { 0x45, 0x45, 0x3 }, new ushort[3] { 0x65, 0x65, 0x3 } };
        private static ushort[][] p_StaticTransitionsF = { new ushort[3] { 0x45, 0x45, 0x3 }, new ushort[3] { 0x65, 0x65, 0x3 } };
        private static ushort[][] p_StaticTransitions10 = { new ushort[3] { 0x30, 0x39, 0x10 } };
        private static ushort[][] p_StaticTransitions11 = {  };
        private static ushort[][][] p_StaticTransitions = { p_StaticTransitions0, p_StaticTransitions1, p_StaticTransitions2, p_StaticTransitions3, p_StaticTransitions4, p_StaticTransitions5, p_StaticTransitions6, p_StaticTransitions7, p_StaticTransitions8, p_StaticTransitions9, p_StaticTransitionsA, p_StaticTransitionsB, p_StaticTransitionsC, p_StaticTransitionsD, p_StaticTransitionsE, p_StaticTransitionsF, p_StaticTransitions10, p_StaticTransitions11 };
        private static int[] p_StaticFinals = { -1, -1, -1, -1, 0, 1, 2, 3, 4, 5, 6, 6, 7, 7, 6, 6, 6, 6 };
        protected override void setup() {
            p_SymbolsSID = p_StaticSymbolsSID;
            p_SymbolsName = p_StaticSymbolsName;
            p_SymbolsSubGrammars = new System.Collections.Generic.Dictionary<ushort, MatchSubGrammar>();
            p_Transitions = p_StaticTransitions;
            p_Finals = p_StaticFinals;
            p_SeparatorID = 0x7;
        }
        public override Hime.Redist.Parsers.ILexer Clone() {
            return new MathExp_Lexer(this);
        }
        public MathExp_Lexer(string input) : base(new System.IO.StringReader(input)) {}
        public MathExp_Lexer(System.IO.TextReader input) : base(input) {}
        public MathExp_Lexer(MathExp_Lexer original) : base(original) {}
    }
    class MathExp_Parser : Hime.Redist.Parsers.BaseRNGLR1Parser
    {
        private static Hime.Redist.Parsers.SPPFNode[] p_StaticNullVarsSPPF = {  };
        private static Hime.Redist.Parsers.SPPFNode[] p_StaticNullChoicesSPPF = { new Hime.Redist.Parsers.SPPFNode(null, 0, Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace) };
        private static void Production_8_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            family.AddChild(new Hime.Redist.Parsers.SPPFNode(new Hime.Redist.Parsers.SymbolAction("OnNumber", ((MathExp_Parser)parser).p_Actions.OnNumber), 0));
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_8_1 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            family.AddChild(nodes[1]);
            family.AddChild(nodes[2]);
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_9_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_9_1 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            family.AddChild(nodes[1]);
            family.AddChild(nodes[2]);
            family.AddChild(new Hime.Redist.Parsers.SPPFNode(new Hime.Redist.Parsers.SymbolAction("OnMult", ((MathExp_Parser)parser).p_Actions.OnMult), 0));
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_9_2 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            family.AddChild(nodes[1]);
            family.AddChild(nodes[2]);
            family.AddChild(new Hime.Redist.Parsers.SPPFNode(new Hime.Redist.Parsers.SymbolAction("OnDiv", ((MathExp_Parser)parser).p_Actions.OnDiv), 0));
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_A_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_A_1 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            family.AddChild(nodes[1]);
            family.AddChild(nodes[2]);
            family.AddChild(new Hime.Redist.Parsers.SPPFNode(new Hime.Redist.Parsers.SymbolAction("OnPlus", ((MathExp_Parser)parser).p_Actions.OnPlus), 0));
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_A_2 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            family.AddChild(nodes[1]);
            family.AddChild(nodes[2]);
            family.AddChild(new Hime.Redist.Parsers.SPPFNode(new Hime.Redist.Parsers.SymbolAction("OnMinus", ((MathExp_Parser)parser).p_Actions.OnMinus), 0));
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_B_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_12_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            nodes[0].Action = Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote;
            family.AddChild(nodes[0]);
            nodes[1].Action = Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop;
            family.AddChild(nodes[1]);
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static Rule[] p_StaticRules = {
           new Rule(Production_8_0, new Hime.Redist.Parsers.SymbolVariable(0x8, "exp_atom"))
           , new Rule(Production_8_1, new Hime.Redist.Parsers.SymbolVariable(0x8, "exp_atom"))
           , new Rule(Production_9_0, new Hime.Redist.Parsers.SymbolVariable(0x9, "exp_op0"))
           , new Rule(Production_9_1, new Hime.Redist.Parsers.SymbolVariable(0x9, "exp_op0"))
           , new Rule(Production_9_2, new Hime.Redist.Parsers.SymbolVariable(0x9, "exp_op0"))
           , new Rule(Production_A_0, new Hime.Redist.Parsers.SymbolVariable(0xA, "exp_op1"))
           , new Rule(Production_A_1, new Hime.Redist.Parsers.SymbolVariable(0xA, "exp_op1"))
           , new Rule(Production_A_2, new Hime.Redist.Parsers.SymbolVariable(0xA, "exp_op1"))
           , new Rule(Production_B_0, new Hime.Redist.Parsers.SymbolVariable(0xB, "exp"))
           , new Rule(Production_12_0, new Hime.Redist.Parsers.SymbolVariable(0x12, "_Axiom_"))
        };
        private static State[] p_StaticStates = {
            new State(
               new string[36] {"[_Axiom_ → • exp $, ε]", "[exp → • exp_op1, $]", "[exp_op1 → • exp_op0, $]", "[exp_op1 → • exp_op1 + exp_op0, $]", "[exp_op1 → • exp_op1 - exp_op0, $]", "[exp_op0 → • exp_atom, $]", "[exp_op0 → • exp_op0 * exp_atom, $]", "[exp_op0 → • exp_op0 / exp_atom, $]", "[exp_op1 → • exp_op0, +]", "[exp_op1 → • exp_op1 + exp_op0, +]", "[exp_op1 → • exp_op1 - exp_op0, +]", "[exp_op1 → • exp_op0, -]", "[exp_op1 → • exp_op1 + exp_op0, -]", "[exp_op1 → • exp_op1 - exp_op0, -]", "[exp_atom → • NUMBER, $]", "[exp_atom → • ( exp ), $]", "[exp_op0 → • exp_atom, *]", "[exp_op0 → • exp_op0 * exp_atom, *]", "[exp_op0 → • exp_op0 / exp_atom, *]", "[exp_op0 → • exp_atom, /]", "[exp_op0 → • exp_op0 * exp_atom, /]", "[exp_op0 → • exp_op0 / exp_atom, /]", "[exp_op0 → • exp_atom, +]", "[exp_op0 → • exp_op0 * exp_atom, +]", "[exp_op0 → • exp_op0 / exp_atom, +]", "[exp_op0 → • exp_atom, -]", "[exp_op0 → • exp_op0 * exp_atom, -]", "[exp_op0 → • exp_op0 / exp_atom, -]", "[exp_atom → • NUMBER, *]", "[exp_atom → • ( exp ), *]", "[exp_atom → • NUMBER, /]", "[exp_atom → • ( exp ), /]", "[exp_atom → • NUMBER, +]", "[exp_atom → • ( exp ), +]", "[exp_atom → • NUMBER, -]", "[exp_atom → • ( exp ), -]"},
               new Terminal[2] {new Terminal("NUMBER", 0x5), new Terminal("_T[(]", 0xC)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x5, 0x5 }, { 0xc, 0x6 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0xb, 0x1 }, { 0xa, 0x2 }, { 0x9, 0x3 }, { 0x8, 0x4 }},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[1] {"[_Axiom_ → exp • $, ε]"},
               new Terminal[1] {new Terminal("$", 0x2)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x2, 0x7 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[7] {"[exp → exp_op1 •, $]", "[exp_op1 → exp_op1 • + exp_op0, $]", "[exp_op1 → exp_op1 • - exp_op0, $]", "[exp_op1 → exp_op1 • + exp_op0, +]", "[exp_op1 → exp_op1 • - exp_op0, +]", "[exp_op1 → exp_op1 • + exp_op0, -]", "[exp_op1 → exp_op1 • - exp_op0, -]"},
               new Terminal[3] {new Terminal("$", 0x2), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x10, 0x8 }, { 0x11, 0x9 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x2, p_StaticRules[0x8], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[13] {"[exp_op1 → exp_op0 •, $]", "[exp_op0 → exp_op0 • * exp_atom, $]", "[exp_op0 → exp_op0 • / exp_atom, $]", "[exp_op1 → exp_op0 •, +]", "[exp_op1 → exp_op0 •, -]", "[exp_op0 → exp_op0 • * exp_atom, *]", "[exp_op0 → exp_op0 • / exp_atom, *]", "[exp_op0 → exp_op0 • * exp_atom, /]", "[exp_op0 → exp_op0 • / exp_atom, /]", "[exp_op0 → exp_op0 • * exp_atom, +]", "[exp_op0 → exp_op0 • / exp_atom, +]", "[exp_op0 → exp_op0 • * exp_atom, -]", "[exp_op0 → exp_op0 • / exp_atom, -]"},
               new Terminal[5] {new Terminal("$", 0x2), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0xe, 0xA }, { 0xf, 0xB }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x2, p_StaticRules[0x5], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x10, p_StaticRules[0x5], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x11, p_StaticRules[0x5], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[5] {"[exp_op0 → exp_atom •, $]", "[exp_op0 → exp_atom •, *]", "[exp_op0 → exp_atom •, /]", "[exp_op0 → exp_atom •, +]", "[exp_op0 → exp_atom •, -]"},
               new Terminal[5] {new Terminal("$", 0x2), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x2, p_StaticRules[0x2], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0xe, p_StaticRules[0x2], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0xf, p_StaticRules[0x2], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x10, p_StaticRules[0x2], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x11, p_StaticRules[0x2], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[5] {"[exp_atom → NUMBER •, $]", "[exp_atom → NUMBER •, *]", "[exp_atom → NUMBER •, /]", "[exp_atom → NUMBER •, +]", "[exp_atom → NUMBER •, -]"},
               new Terminal[5] {new Terminal("$", 0x2), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x2, p_StaticRules[0x0], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0xe, p_StaticRules[0x0], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0xf, p_StaticRules[0x0], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x10, p_StaticRules[0x0], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x11, p_StaticRules[0x0], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[40] {"[exp_atom → ( • exp ), $]", "[exp_atom → ( • exp ), *]", "[exp_atom → ( • exp ), /]", "[exp_atom → ( • exp ), +]", "[exp_atom → ( • exp ), -]", "[exp → • exp_op1, )]", "[exp_op1 → • exp_op0, )]", "[exp_op1 → • exp_op1 + exp_op0, )]", "[exp_op1 → • exp_op1 - exp_op0, )]", "[exp_op0 → • exp_atom, )]", "[exp_op0 → • exp_op0 * exp_atom, )]", "[exp_op0 → • exp_op0 / exp_atom, )]", "[exp_op1 → • exp_op0, +]", "[exp_op1 → • exp_op1 + exp_op0, +]", "[exp_op1 → • exp_op1 - exp_op0, +]", "[exp_op1 → • exp_op0, -]", "[exp_op1 → • exp_op1 + exp_op0, -]", "[exp_op1 → • exp_op1 - exp_op0, -]", "[exp_atom → • NUMBER, )]", "[exp_atom → • ( exp ), )]", "[exp_op0 → • exp_atom, *]", "[exp_op0 → • exp_op0 * exp_atom, *]", "[exp_op0 → • exp_op0 / exp_atom, *]", "[exp_op0 → • exp_atom, /]", "[exp_op0 → • exp_op0 * exp_atom, /]", "[exp_op0 → • exp_op0 / exp_atom, /]", "[exp_op0 → • exp_atom, +]", "[exp_op0 → • exp_op0 * exp_atom, +]", "[exp_op0 → • exp_op0 / exp_atom, +]", "[exp_op0 → • exp_atom, -]", "[exp_op0 → • exp_op0 * exp_atom, -]", "[exp_op0 → • exp_op0 / exp_atom, -]", "[exp_atom → • NUMBER, *]", "[exp_atom → • ( exp ), *]", "[exp_atom → • NUMBER, /]", "[exp_atom → • ( exp ), /]", "[exp_atom → • NUMBER, +]", "[exp_atom → • ( exp ), +]", "[exp_atom → • NUMBER, -]", "[exp_atom → • ( exp ), -]"},
               new Terminal[2] {new Terminal("NUMBER", 0x5), new Terminal("_T[(]", 0xC)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x5, 0x10 }, { 0xc, 0x11 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0xb, 0xC }, { 0xa, 0xD }, { 0x9, 0xE }, { 0x8, 0xF }},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[1] {"[_Axiom_ → exp $ •, ε]"},
               new Terminal[1] {new Terminal("ε", 0x1)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x1, p_StaticRules[0x9], 0x2, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[28] {"[exp_op1 → exp_op1 + • exp_op0, $]", "[exp_op1 → exp_op1 + • exp_op0, +]", "[exp_op1 → exp_op1 + • exp_op0, -]", "[exp_op0 → • exp_atom, $]", "[exp_op0 → • exp_op0 * exp_atom, $]", "[exp_op0 → • exp_op0 / exp_atom, $]", "[exp_op0 → • exp_atom, +]", "[exp_op0 → • exp_op0 * exp_atom, +]", "[exp_op0 → • exp_op0 / exp_atom, +]", "[exp_op0 → • exp_atom, -]", "[exp_op0 → • exp_op0 * exp_atom, -]", "[exp_op0 → • exp_op0 / exp_atom, -]", "[exp_atom → • NUMBER, $]", "[exp_atom → • ( exp ), $]", "[exp_op0 → • exp_atom, *]", "[exp_op0 → • exp_op0 * exp_atom, *]", "[exp_op0 → • exp_op0 / exp_atom, *]", "[exp_op0 → • exp_atom, /]", "[exp_op0 → • exp_op0 * exp_atom, /]", "[exp_op0 → • exp_op0 / exp_atom, /]", "[exp_atom → • NUMBER, +]", "[exp_atom → • ( exp ), +]", "[exp_atom → • NUMBER, -]", "[exp_atom → • ( exp ), -]", "[exp_atom → • NUMBER, *]", "[exp_atom → • ( exp ), *]", "[exp_atom → • NUMBER, /]", "[exp_atom → • ( exp ), /]"},
               new Terminal[2] {new Terminal("NUMBER", 0x5), new Terminal("_T[(]", 0xC)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x5, 0x5 }, { 0xc, 0x6 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x9, 0x12 }, { 0x8, 0x4 }},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[28] {"[exp_op1 → exp_op1 - • exp_op0, $]", "[exp_op1 → exp_op1 - • exp_op0, +]", "[exp_op1 → exp_op1 - • exp_op0, -]", "[exp_op0 → • exp_atom, $]", "[exp_op0 → • exp_op0 * exp_atom, $]", "[exp_op0 → • exp_op0 / exp_atom, $]", "[exp_op0 → • exp_atom, +]", "[exp_op0 → • exp_op0 * exp_atom, +]", "[exp_op0 → • exp_op0 / exp_atom, +]", "[exp_op0 → • exp_atom, -]", "[exp_op0 → • exp_op0 * exp_atom, -]", "[exp_op0 → • exp_op0 / exp_atom, -]", "[exp_atom → • NUMBER, $]", "[exp_atom → • ( exp ), $]", "[exp_op0 → • exp_atom, *]", "[exp_op0 → • exp_op0 * exp_atom, *]", "[exp_op0 → • exp_op0 / exp_atom, *]", "[exp_op0 → • exp_atom, /]", "[exp_op0 → • exp_op0 * exp_atom, /]", "[exp_op0 → • exp_op0 / exp_atom, /]", "[exp_atom → • NUMBER, +]", "[exp_atom → • ( exp ), +]", "[exp_atom → • NUMBER, -]", "[exp_atom → • ( exp ), -]", "[exp_atom → • NUMBER, *]", "[exp_atom → • ( exp ), *]", "[exp_atom → • NUMBER, /]", "[exp_atom → • ( exp ), /]"},
               new Terminal[2] {new Terminal("NUMBER", 0x5), new Terminal("_T[(]", 0xC)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x5, 0x5 }, { 0xc, 0x6 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x9, 0x13 }, { 0x8, 0x4 }},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[15] {"[exp_op0 → exp_op0 * • exp_atom, $]", "[exp_op0 → exp_op0 * • exp_atom, *]", "[exp_op0 → exp_op0 * • exp_atom, /]", "[exp_op0 → exp_op0 * • exp_atom, +]", "[exp_op0 → exp_op0 * • exp_atom, -]", "[exp_atom → • NUMBER, $]", "[exp_atom → • ( exp ), $]", "[exp_atom → • NUMBER, *]", "[exp_atom → • ( exp ), *]", "[exp_atom → • NUMBER, /]", "[exp_atom → • ( exp ), /]", "[exp_atom → • NUMBER, +]", "[exp_atom → • ( exp ), +]", "[exp_atom → • NUMBER, -]", "[exp_atom → • ( exp ), -]"},
               new Terminal[2] {new Terminal("NUMBER", 0x5), new Terminal("_T[(]", 0xC)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x5, 0x5 }, { 0xc, 0x6 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x8, 0x14 }},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[15] {"[exp_op0 → exp_op0 / • exp_atom, $]", "[exp_op0 → exp_op0 / • exp_atom, *]", "[exp_op0 → exp_op0 / • exp_atom, /]", "[exp_op0 → exp_op0 / • exp_atom, +]", "[exp_op0 → exp_op0 / • exp_atom, -]", "[exp_atom → • NUMBER, $]", "[exp_atom → • ( exp ), $]", "[exp_atom → • NUMBER, *]", "[exp_atom → • ( exp ), *]", "[exp_atom → • NUMBER, /]", "[exp_atom → • ( exp ), /]", "[exp_atom → • NUMBER, +]", "[exp_atom → • ( exp ), +]", "[exp_atom → • NUMBER, -]", "[exp_atom → • ( exp ), -]"},
               new Terminal[2] {new Terminal("NUMBER", 0x5), new Terminal("_T[(]", 0xC)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x5, 0x5 }, { 0xc, 0x6 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x8, 0x15 }},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[5] {"[exp_atom → ( exp • ), $]", "[exp_atom → ( exp • ), *]", "[exp_atom → ( exp • ), /]", "[exp_atom → ( exp • ), +]", "[exp_atom → ( exp • ), -]"},
               new Terminal[1] {new Terminal("_T[)]", 0xD)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0xd, 0x16 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[7] {"[exp → exp_op1 •, )]", "[exp_op1 → exp_op1 • + exp_op0, )]", "[exp_op1 → exp_op1 • - exp_op0, )]", "[exp_op1 → exp_op1 • + exp_op0, +]", "[exp_op1 → exp_op1 • - exp_op0, +]", "[exp_op1 → exp_op1 • + exp_op0, -]", "[exp_op1 → exp_op1 • - exp_op0, -]"},
               new Terminal[3] {new Terminal("_T[)]", 0xD), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x10, 0x17 }, { 0x11, 0x18 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0xd, p_StaticRules[0x8], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[13] {"[exp_op1 → exp_op0 •, )]", "[exp_op0 → exp_op0 • * exp_atom, )]", "[exp_op0 → exp_op0 • / exp_atom, )]", "[exp_op1 → exp_op0 •, +]", "[exp_op1 → exp_op0 •, -]", "[exp_op0 → exp_op0 • * exp_atom, *]", "[exp_op0 → exp_op0 • / exp_atom, *]", "[exp_op0 → exp_op0 • * exp_atom, /]", "[exp_op0 → exp_op0 • / exp_atom, /]", "[exp_op0 → exp_op0 • * exp_atom, +]", "[exp_op0 → exp_op0 • / exp_atom, +]", "[exp_op0 → exp_op0 • * exp_atom, -]", "[exp_op0 → exp_op0 • / exp_atom, -]"},
               new Terminal[5] {new Terminal("_T[)]", 0xD), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0xe, 0x19 }, { 0xf, 0x1A }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0xd, p_StaticRules[0x5], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x10, p_StaticRules[0x5], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x11, p_StaticRules[0x5], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[5] {"[exp_op0 → exp_atom •, )]", "[exp_op0 → exp_atom •, *]", "[exp_op0 → exp_atom •, /]", "[exp_op0 → exp_atom •, +]", "[exp_op0 → exp_atom •, -]"},
               new Terminal[5] {new Terminal("_T[)]", 0xD), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0xd, p_StaticRules[0x2], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0xe, p_StaticRules[0x2], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0xf, p_StaticRules[0x2], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x10, p_StaticRules[0x2], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x11, p_StaticRules[0x2], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[5] {"[exp_atom → NUMBER •, )]", "[exp_atom → NUMBER •, *]", "[exp_atom → NUMBER •, /]", "[exp_atom → NUMBER •, +]", "[exp_atom → NUMBER •, -]"},
               new Terminal[5] {new Terminal("_T[)]", 0xD), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0xd, p_StaticRules[0x0], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0xe, p_StaticRules[0x0], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0xf, p_StaticRules[0x0], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x10, p_StaticRules[0x0], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x11, p_StaticRules[0x0], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[40] {"[exp_atom → ( • exp ), )]", "[exp_atom → ( • exp ), *]", "[exp_atom → ( • exp ), /]", "[exp_atom → ( • exp ), +]", "[exp_atom → ( • exp ), -]", "[exp → • exp_op1, )]", "[exp_op1 → • exp_op0, )]", "[exp_op1 → • exp_op1 + exp_op0, )]", "[exp_op1 → • exp_op1 - exp_op0, )]", "[exp_op0 → • exp_atom, )]", "[exp_op0 → • exp_op0 * exp_atom, )]", "[exp_op0 → • exp_op0 / exp_atom, )]", "[exp_op1 → • exp_op0, +]", "[exp_op1 → • exp_op1 + exp_op0, +]", "[exp_op1 → • exp_op1 - exp_op0, +]", "[exp_op1 → • exp_op0, -]", "[exp_op1 → • exp_op1 + exp_op0, -]", "[exp_op1 → • exp_op1 - exp_op0, -]", "[exp_atom → • NUMBER, )]", "[exp_atom → • ( exp ), )]", "[exp_op0 → • exp_atom, *]", "[exp_op0 → • exp_op0 * exp_atom, *]", "[exp_op0 → • exp_op0 / exp_atom, *]", "[exp_op0 → • exp_atom, /]", "[exp_op0 → • exp_op0 * exp_atom, /]", "[exp_op0 → • exp_op0 / exp_atom, /]", "[exp_op0 → • exp_atom, +]", "[exp_op0 → • exp_op0 * exp_atom, +]", "[exp_op0 → • exp_op0 / exp_atom, +]", "[exp_op0 → • exp_atom, -]", "[exp_op0 → • exp_op0 * exp_atom, -]", "[exp_op0 → • exp_op0 / exp_atom, -]", "[exp_atom → • NUMBER, *]", "[exp_atom → • ( exp ), *]", "[exp_atom → • NUMBER, /]", "[exp_atom → • ( exp ), /]", "[exp_atom → • NUMBER, +]", "[exp_atom → • ( exp ), +]", "[exp_atom → • NUMBER, -]", "[exp_atom → • ( exp ), -]"},
               new Terminal[2] {new Terminal("NUMBER", 0x5), new Terminal("_T[(]", 0xC)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x5, 0x10 }, { 0xc, 0x11 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0xb, 0x1B }, { 0xa, 0xD }, { 0x9, 0xE }, { 0x8, 0xF }},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[13] {"[exp_op1 → exp_op1 + exp_op0 •, $]", "[exp_op1 → exp_op1 + exp_op0 •, +]", "[exp_op1 → exp_op1 + exp_op0 •, -]", "[exp_op0 → exp_op0 • * exp_atom, $]", "[exp_op0 → exp_op0 • / exp_atom, $]", "[exp_op0 → exp_op0 • * exp_atom, +]", "[exp_op0 → exp_op0 • / exp_atom, +]", "[exp_op0 → exp_op0 • * exp_atom, -]", "[exp_op0 → exp_op0 • / exp_atom, -]", "[exp_op0 → exp_op0 • * exp_atom, *]", "[exp_op0 → exp_op0 • / exp_atom, *]", "[exp_op0 → exp_op0 • * exp_atom, /]", "[exp_op0 → exp_op0 • / exp_atom, /]"},
               new Terminal[5] {new Terminal("$", 0x2), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0xe, 0xA }, { 0xf, 0xB }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x2, p_StaticRules[0x6], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x10, p_StaticRules[0x6], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x11, p_StaticRules[0x6], 0x3, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[13] {"[exp_op1 → exp_op1 - exp_op0 •, $]", "[exp_op1 → exp_op1 - exp_op0 •, +]", "[exp_op1 → exp_op1 - exp_op0 •, -]", "[exp_op0 → exp_op0 • * exp_atom, $]", "[exp_op0 → exp_op0 • / exp_atom, $]", "[exp_op0 → exp_op0 • * exp_atom, +]", "[exp_op0 → exp_op0 • / exp_atom, +]", "[exp_op0 → exp_op0 • * exp_atom, -]", "[exp_op0 → exp_op0 • / exp_atom, -]", "[exp_op0 → exp_op0 • * exp_atom, *]", "[exp_op0 → exp_op0 • / exp_atom, *]", "[exp_op0 → exp_op0 • * exp_atom, /]", "[exp_op0 → exp_op0 • / exp_atom, /]"},
               new Terminal[5] {new Terminal("$", 0x2), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0xe, 0xA }, { 0xf, 0xB }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x2, p_StaticRules[0x7], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x10, p_StaticRules[0x7], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x11, p_StaticRules[0x7], 0x3, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[5] {"[exp_op0 → exp_op0 * exp_atom •, $]", "[exp_op0 → exp_op0 * exp_atom •, *]", "[exp_op0 → exp_op0 * exp_atom •, /]", "[exp_op0 → exp_op0 * exp_atom •, +]", "[exp_op0 → exp_op0 * exp_atom •, -]"},
               new Terminal[5] {new Terminal("$", 0x2), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x2, p_StaticRules[0x3], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0xe, p_StaticRules[0x3], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0xf, p_StaticRules[0x3], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x10, p_StaticRules[0x3], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x11, p_StaticRules[0x3], 0x3, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[5] {"[exp_op0 → exp_op0 / exp_atom •, $]", "[exp_op0 → exp_op0 / exp_atom •, *]", "[exp_op0 → exp_op0 / exp_atom •, /]", "[exp_op0 → exp_op0 / exp_atom •, +]", "[exp_op0 → exp_op0 / exp_atom •, -]"},
               new Terminal[5] {new Terminal("$", 0x2), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x2, p_StaticRules[0x4], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0xe, p_StaticRules[0x4], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0xf, p_StaticRules[0x4], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x10, p_StaticRules[0x4], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x11, p_StaticRules[0x4], 0x3, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[5] {"[exp_atom → ( exp ) •, $]", "[exp_atom → ( exp ) •, *]", "[exp_atom → ( exp ) •, /]", "[exp_atom → ( exp ) •, +]", "[exp_atom → ( exp ) •, -]"},
               new Terminal[5] {new Terminal("$", 0x2), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x2, p_StaticRules[0x1], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0xe, p_StaticRules[0x1], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0xf, p_StaticRules[0x1], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x10, p_StaticRules[0x1], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x11, p_StaticRules[0x1], 0x3, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[28] {"[exp_op1 → exp_op1 + • exp_op0, )]", "[exp_op1 → exp_op1 + • exp_op0, +]", "[exp_op1 → exp_op1 + • exp_op0, -]", "[exp_op0 → • exp_atom, )]", "[exp_op0 → • exp_op0 * exp_atom, )]", "[exp_op0 → • exp_op0 / exp_atom, )]", "[exp_op0 → • exp_atom, +]", "[exp_op0 → • exp_op0 * exp_atom, +]", "[exp_op0 → • exp_op0 / exp_atom, +]", "[exp_op0 → • exp_atom, -]", "[exp_op0 → • exp_op0 * exp_atom, -]", "[exp_op0 → • exp_op0 / exp_atom, -]", "[exp_atom → • NUMBER, )]", "[exp_atom → • ( exp ), )]", "[exp_op0 → • exp_atom, *]", "[exp_op0 → • exp_op0 * exp_atom, *]", "[exp_op0 → • exp_op0 / exp_atom, *]", "[exp_op0 → • exp_atom, /]", "[exp_op0 → • exp_op0 * exp_atom, /]", "[exp_op0 → • exp_op0 / exp_atom, /]", "[exp_atom → • NUMBER, +]", "[exp_atom → • ( exp ), +]", "[exp_atom → • NUMBER, -]", "[exp_atom → • ( exp ), -]", "[exp_atom → • NUMBER, *]", "[exp_atom → • ( exp ), *]", "[exp_atom → • NUMBER, /]", "[exp_atom → • ( exp ), /]"},
               new Terminal[2] {new Terminal("NUMBER", 0x5), new Terminal("_T[(]", 0xC)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x5, 0x10 }, { 0xc, 0x11 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x9, 0x1C }, { 0x8, 0xF }},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[28] {"[exp_op1 → exp_op1 - • exp_op0, )]", "[exp_op1 → exp_op1 - • exp_op0, +]", "[exp_op1 → exp_op1 - • exp_op0, -]", "[exp_op0 → • exp_atom, )]", "[exp_op0 → • exp_op0 * exp_atom, )]", "[exp_op0 → • exp_op0 / exp_atom, )]", "[exp_op0 → • exp_atom, +]", "[exp_op0 → • exp_op0 * exp_atom, +]", "[exp_op0 → • exp_op0 / exp_atom, +]", "[exp_op0 → • exp_atom, -]", "[exp_op0 → • exp_op0 * exp_atom, -]", "[exp_op0 → • exp_op0 / exp_atom, -]", "[exp_atom → • NUMBER, )]", "[exp_atom → • ( exp ), )]", "[exp_op0 → • exp_atom, *]", "[exp_op0 → • exp_op0 * exp_atom, *]", "[exp_op0 → • exp_op0 / exp_atom, *]", "[exp_op0 → • exp_atom, /]", "[exp_op0 → • exp_op0 * exp_atom, /]", "[exp_op0 → • exp_op0 / exp_atom, /]", "[exp_atom → • NUMBER, +]", "[exp_atom → • ( exp ), +]", "[exp_atom → • NUMBER, -]", "[exp_atom → • ( exp ), -]", "[exp_atom → • NUMBER, *]", "[exp_atom → • ( exp ), *]", "[exp_atom → • NUMBER, /]", "[exp_atom → • ( exp ), /]"},
               new Terminal[2] {new Terminal("NUMBER", 0x5), new Terminal("_T[(]", 0xC)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x5, 0x10 }, { 0xc, 0x11 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x9, 0x1D }, { 0x8, 0xF }},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[15] {"[exp_op0 → exp_op0 * • exp_atom, )]", "[exp_op0 → exp_op0 * • exp_atom, *]", "[exp_op0 → exp_op0 * • exp_atom, /]", "[exp_op0 → exp_op0 * • exp_atom, +]", "[exp_op0 → exp_op0 * • exp_atom, -]", "[exp_atom → • NUMBER, )]", "[exp_atom → • ( exp ), )]", "[exp_atom → • NUMBER, *]", "[exp_atom → • ( exp ), *]", "[exp_atom → • NUMBER, /]", "[exp_atom → • ( exp ), /]", "[exp_atom → • NUMBER, +]", "[exp_atom → • ( exp ), +]", "[exp_atom → • NUMBER, -]", "[exp_atom → • ( exp ), -]"},
               new Terminal[2] {new Terminal("NUMBER", 0x5), new Terminal("_T[(]", 0xC)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x5, 0x10 }, { 0xc, 0x11 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x8, 0x1E }},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[15] {"[exp_op0 → exp_op0 / • exp_atom, )]", "[exp_op0 → exp_op0 / • exp_atom, *]", "[exp_op0 → exp_op0 / • exp_atom, /]", "[exp_op0 → exp_op0 / • exp_atom, +]", "[exp_op0 → exp_op0 / • exp_atom, -]", "[exp_atom → • NUMBER, )]", "[exp_atom → • ( exp ), )]", "[exp_atom → • NUMBER, *]", "[exp_atom → • ( exp ), *]", "[exp_atom → • NUMBER, /]", "[exp_atom → • ( exp ), /]", "[exp_atom → • NUMBER, +]", "[exp_atom → • ( exp ), +]", "[exp_atom → • NUMBER, -]", "[exp_atom → • ( exp ), -]"},
               new Terminal[2] {new Terminal("NUMBER", 0x5), new Terminal("_T[(]", 0xC)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x5, 0x10 }, { 0xc, 0x11 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x8, 0x1F }},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[5] {"[exp_atom → ( exp • ), )]", "[exp_atom → ( exp • ), *]", "[exp_atom → ( exp • ), /]", "[exp_atom → ( exp • ), +]", "[exp_atom → ( exp • ), -]"},
               new Terminal[1] {new Terminal("_T[)]", 0xD)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0xd, 0x20 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[13] {"[exp_op1 → exp_op1 + exp_op0 •, )]", "[exp_op1 → exp_op1 + exp_op0 •, +]", "[exp_op1 → exp_op1 + exp_op0 •, -]", "[exp_op0 → exp_op0 • * exp_atom, )]", "[exp_op0 → exp_op0 • / exp_atom, )]", "[exp_op0 → exp_op0 • * exp_atom, +]", "[exp_op0 → exp_op0 • / exp_atom, +]", "[exp_op0 → exp_op0 • * exp_atom, -]", "[exp_op0 → exp_op0 • / exp_atom, -]", "[exp_op0 → exp_op0 • * exp_atom, *]", "[exp_op0 → exp_op0 • / exp_atom, *]", "[exp_op0 → exp_op0 • * exp_atom, /]", "[exp_op0 → exp_op0 • / exp_atom, /]"},
               new Terminal[5] {new Terminal("_T[)]", 0xD), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0xe, 0x19 }, { 0xf, 0x1A }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0xd, p_StaticRules[0x6], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x10, p_StaticRules[0x6], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x11, p_StaticRules[0x6], 0x3, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[13] {"[exp_op1 → exp_op1 - exp_op0 •, )]", "[exp_op1 → exp_op1 - exp_op0 •, +]", "[exp_op1 → exp_op1 - exp_op0 •, -]", "[exp_op0 → exp_op0 • * exp_atom, )]", "[exp_op0 → exp_op0 • / exp_atom, )]", "[exp_op0 → exp_op0 • * exp_atom, +]", "[exp_op0 → exp_op0 • / exp_atom, +]", "[exp_op0 → exp_op0 • * exp_atom, -]", "[exp_op0 → exp_op0 • / exp_atom, -]", "[exp_op0 → exp_op0 • * exp_atom, *]", "[exp_op0 → exp_op0 • / exp_atom, *]", "[exp_op0 → exp_op0 • * exp_atom, /]", "[exp_op0 → exp_op0 • / exp_atom, /]"},
               new Terminal[5] {new Terminal("_T[)]", 0xD), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0xe, 0x19 }, { 0xf, 0x1A }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0xd, p_StaticRules[0x7], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x10, p_StaticRules[0x7], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x11, p_StaticRules[0x7], 0x3, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[5] {"[exp_op0 → exp_op0 * exp_atom •, )]", "[exp_op0 → exp_op0 * exp_atom •, *]", "[exp_op0 → exp_op0 * exp_atom •, /]", "[exp_op0 → exp_op0 * exp_atom •, +]", "[exp_op0 → exp_op0 * exp_atom •, -]"},
               new Terminal[5] {new Terminal("_T[)]", 0xD), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0xd, p_StaticRules[0x3], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0xe, p_StaticRules[0x3], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0xf, p_StaticRules[0x3], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x10, p_StaticRules[0x3], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x11, p_StaticRules[0x3], 0x3, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[5] {"[exp_op0 → exp_op0 / exp_atom •, )]", "[exp_op0 → exp_op0 / exp_atom •, *]", "[exp_op0 → exp_op0 / exp_atom •, /]", "[exp_op0 → exp_op0 / exp_atom •, +]", "[exp_op0 → exp_op0 / exp_atom •, -]"},
               new Terminal[5] {new Terminal("_T[)]", 0xD), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0xd, p_StaticRules[0x4], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0xe, p_StaticRules[0x4], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0xf, p_StaticRules[0x4], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x10, p_StaticRules[0x4], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x11, p_StaticRules[0x4], 0x3, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[5] {"[exp_atom → ( exp ) •, )]", "[exp_atom → ( exp ) •, *]", "[exp_atom → ( exp ) •, /]", "[exp_atom → ( exp ) •, +]", "[exp_atom → ( exp ) •, -]"},
               new Terminal[5] {new Terminal("_T[)]", 0xD), new Terminal("_T[*]", 0xE), new Terminal("_T[/]", 0xF), new Terminal("_T[+]", 0x10), new Terminal("_T[-]", 0x11)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0xd, p_StaticRules[0x1], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0xe, p_StaticRules[0x1], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0xf, p_StaticRules[0x1], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x10, p_StaticRules[0x1], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x11, p_StaticRules[0x1], 0x3, p_StaticNullChoicesSPPF[0x0])})
        };
        private static void BuildNullables() { 
            System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> temp = new System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode>();
            p_StaticNullChoicesSPPF[0].AddFamily(temp);
            temp.Clear();
        }
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
            p_NullVarsSPPF = p_StaticNullVarsSPPF;
            p_NullChoicesSPPF = p_StaticNullChoicesSPPF;
            p_Rules = p_StaticRules;
            p_States = p_StaticStates;
            p_AxiomID = 0xB;
            p_AxiomNullSPPF = 0xB;
            p_AxiomPrimeID = 0x12;
        }
        static MathExp_Parser()
        {
            BuildNullables();
        }
        private Actions p_Actions;
        public MathExp_Parser(MathExp_Lexer lexer, Actions actions) : base (lexer) { p_Actions = actions; }
    }
}
