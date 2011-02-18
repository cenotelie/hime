namespace Analyzer
{
    public class AmbiguousLALR1_Lexer : Hime.Redist.Parsers.LexerText
    {
        private static ushort[] p_StaticSymbolsSID = { 0x11, 0x13, 0x14, 0x18, 0x19, 0x3, 0x4 };
        private static string[] p_StaticSymbolsName = { "_T[.]", "_T[(]", "_T[)]", "_T[*]", "_T[=]", "WHITE_SPACE", "IDENTIFIER" };
        private static ushort[][] p_StaticTransitions0 = { new ushort[3] { 0x2E, 0x2E, 0x2 }, new ushort[3] { 0x28, 0x28, 0x3 }, new ushort[3] { 0x29, 0x29, 0x4 }, new ushort[3] { 0x2A, 0x2A, 0x5 }, new ushort[3] { 0x3D, 0x3D, 0x6 }, new ushort[3] { 0x9, 0x9, 0x7 }, new ushort[3] { 0xB, 0xC, 0x7 }, new ushort[3] { 0x20, 0x20, 0x7 }, new ushort[3] { 0x40, 0x40, 0x1 }, new ushort[3] { 0x41, 0x5A, 0x8 }, new ushort[3] { 0x61, 0x7A, 0x8 } };
        private static ushort[][] p_StaticTransitions1 = { new ushort[3] { 0x41, 0x5A, 0x8 }, new ushort[3] { 0x61, 0x7A, 0x8 } };
        private static ushort[][] p_StaticTransitions2 = {  };
        private static ushort[][] p_StaticTransitions3 = {  };
        private static ushort[][] p_StaticTransitions4 = {  };
        private static ushort[][] p_StaticTransitions5 = {  };
        private static ushort[][] p_StaticTransitions6 = {  };
        private static ushort[][] p_StaticTransitions7 = {  };
        private static ushort[][] p_StaticTransitions8 = { new ushort[3] { 0x30, 0x39, 0x8 }, new ushort[3] { 0x41, 0x5A, 0x8 }, new ushort[3] { 0x5F, 0x5F, 0x8 }, new ushort[3] { 0x61, 0x7A, 0x8 } };
        private static ushort[][][] p_StaticTransitions = { p_StaticTransitions0, p_StaticTransitions1, p_StaticTransitions2, p_StaticTransitions3, p_StaticTransitions4, p_StaticTransitions5, p_StaticTransitions6, p_StaticTransitions7, p_StaticTransitions8 };
        private static int[] p_StaticFinals = { -1, -1, 0, 1, 2, 3, 4, 5, 6 };
        private static ushort p_StaticSeparator = 0x3;
        protected override void setup() {
            p_SymbolsSID = p_StaticSymbolsSID;
            p_SymbolsName = p_StaticSymbolsName;
            p_SymbolsSubGrammars = new System.Collections.Generic.Dictionary<ushort, MatchSubGrammar>();
            p_Transitions = p_StaticTransitions;
            p_Finals = p_StaticFinals;
            p_SeparatorID = 0x3;
        }
        public override Hime.Redist.Parsers.ILexer Clone() {
            return new AmbiguousLALR1_Lexer(p_Input, p_CurrentPosition, p_Line, p_Errors);
        }
        public AmbiguousLALR1_Lexer(string input) : base(input) {}
        public AmbiguousLALR1_Lexer(string input, int position, int line, System.Collections.Generic.List<Hime.Redist.Parsers.LexerTextError> errors) : base(input, position, line, errors) {}
    }
    public class AmbiguousLALR1_Parser : Hime.Redist.Parsers.BaseRNGLR1Parser
    {
        private static Hime.Redist.Parsers.SPPFNode[] p_StaticNullVarsSPPF = { new Hime.Redist.Parsers.SPPFNode(new Hime.Redist.Parsers.SymbolVariable(0x12, "_m18"), 0) };
        private static Hime.Redist.Parsers.SPPFNode[] p_StaticNullChoicesSPPF = { new Hime.Redist.Parsers.SPPFNode(null, 0), new Hime.Redist.Parsers.SPPFNode(null, 0), new Hime.Redist.Parsers.SPPFNode(null, 0) };
        private static void Production_5_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            family.AddChild(nodes[1]);
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_6_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_7_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_7_1 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            family.AddChild(nodes[1]);
            family.AddChild(nodes[2]);
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_8_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_9_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            family.AddChild(nodes[1]);
            family.AddChild(nodes[2]);
            family.AddChild(nodes[3]);
            family.AddChild(new Hime.Redist.Parsers.SPPFNode(new Hime.Redist.Parsers.SymbolAction("OnCast", ((AmbiguousLALR1_Parser)parser).p_Actions.OnCast), 0));
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
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_B_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_B_1 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            family.AddChild(nodes[1]);
            family.AddChild(nodes[2]);
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_C_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_D_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_E_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            family.AddChild(nodes[1]);
            family.AddChild(nodes[2]);
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_F_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_F_1 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_10_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_12_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            root.Action = Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace;
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            family.AddChild(nodes[0]);
            family.AddChild(nodes[1]);
            family.AddChild(nodes[2]);
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_12_1 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            root.Action = Hime.Redist.Parsers.SyntaxTreeNodeAction.Replace;
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static void Production_1A_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, Hime.Redist.Parsers.SPPFNode root, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes)
        {
            Hime.Redist.Parsers.SPPFNodeFamily family = new Hime.Redist.Parsers.SPPFNodeFamily(root);
            nodes[0].Action = Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote;
            family.AddChild(nodes[0]);
            nodes[1].Action = Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop;
            family.AddChild(nodes[1]);
            if (!root.HasEquivalentFamily(family)) root.AddFamily(family);
        }
        private static Rule[] p_StaticRules = {
           new Rule(Production_5_0, new Hime.Redist.Parsers.SymbolVariable(0x5, "qualified_name"))
           , new Rule(Production_6_0, new Hime.Redist.Parsers.SymbolVariable(0x6, "type"))
           , new Rule(Production_7_0, new Hime.Redist.Parsers.SymbolVariable(0x7, "exp_atom"))
           , new Rule(Production_7_1, new Hime.Redist.Parsers.SymbolVariable(0x7, "exp_atom"))
           , new Rule(Production_8_0, new Hime.Redist.Parsers.SymbolVariable(0x8, "exp_op0"))
           , new Rule(Production_9_0, new Hime.Redist.Parsers.SymbolVariable(0x9, "exp_op1_cast"))
           , new Rule(Production_A_0, new Hime.Redist.Parsers.SymbolVariable(0xA, "exp_op1"))
           , new Rule(Production_A_1, new Hime.Redist.Parsers.SymbolVariable(0xA, "exp_op1"))
           , new Rule(Production_B_0, new Hime.Redist.Parsers.SymbolVariable(0xB, "exp_op2"))
           , new Rule(Production_B_1, new Hime.Redist.Parsers.SymbolVariable(0xB, "exp_op2"))
           , new Rule(Production_C_0, new Hime.Redist.Parsers.SymbolVariable(0xC, "exp_op11"))
           , new Rule(Production_D_0, new Hime.Redist.Parsers.SymbolVariable(0xD, "exp_op12_conditional"))
           , new Rule(Production_E_0, new Hime.Redist.Parsers.SymbolVariable(0xE, "exp_op13_assigment"))
           , new Rule(Production_F_0, new Hime.Redist.Parsers.SymbolVariable(0xF, "expression"))
           , new Rule(Production_F_1, new Hime.Redist.Parsers.SymbolVariable(0xF, "expression"))
           , new Rule(Production_10_0, new Hime.Redist.Parsers.SymbolVariable(0x10, "compilation_unit"))
           , new Rule(Production_12_0, new Hime.Redist.Parsers.SymbolVariable(0x12, "_m18"))
           , new Rule(Production_12_1, new Hime.Redist.Parsers.SymbolVariable(0x12, "_m18"))
           , new Rule(Production_1A_0, new Hime.Redist.Parsers.SymbolVariable(0x1A, "_Axiom_"))
 };
        private static State[] p_StaticStates = {
            new State(
               new string[29] {"[_Axiom_ -> . compilation_unit $, ε]", "[compilation_unit -> . expression, $]", "[expression -> . exp_op12_conditional, $]", "[expression -> . exp_op13_assigment, $]", "[exp_op12_conditional -> . exp_op11, $]", "[exp_op13_assigment -> . exp_op1 = expression, $]", "[exp_op11 -> . exp_op2, $]", "[exp_op1 -> . exp_op0, =]", "[exp_op1 -> . exp_op1_cast, =]", "[exp_op2 -> . exp_op1, $]", "[exp_op2 -> . exp_op2 * exp_op1, $]", "[exp_op0 -> . exp_atom, =]", "[exp_op1_cast -> . ( type ) exp_op1, =]", "[exp_op1 -> . exp_op0, $]", "[exp_op1 -> . exp_op1_cast, $]", "[exp_op2 -> . exp_op1, *]", "[exp_op2 -> . exp_op2 * exp_op1, *]", "[exp_atom -> . IDENTIFIER, =]", "[exp_atom -> . ( expression ), =]", "[exp_op0 -> . exp_atom, $]", "[exp_op1_cast -> . ( type ) exp_op1, $]", "[exp_op1 -> . exp_op0, *]", "[exp_op1 -> . exp_op1_cast, *]", "[exp_atom -> . IDENTIFIER, $]", "[exp_atom -> . ( expression ), $]", "[exp_op0 -> . exp_atom, *]", "[exp_op1_cast -> . ( type ) exp_op1, *]", "[exp_atom -> . IDENTIFIER, *]", "[exp_atom -> . ( expression ), *]"},
               new Terminal[2] {new Terminal("_T[(]", 0x13), new Terminal("IDENTIFIER", 0x4)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x13, 0xB }, { 0x4, 0xC }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x10, 0x1 }, { 0xf, 0x2 }, { 0xd, 0x3 }, { 0xe, 0x4 }, { 0xc, 0x5 }, { 0xa, 0x6 }, { 0xb, 0x7 }, { 0x8, 0x8 }, { 0x9, 0x9 }, { 0x7, 0xA }},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[1] {"[_Axiom_ -> compilation_unit . $, ε]"},
               new Terminal[1] {new Terminal("$", 0x2)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x2, 0xD }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[1] {"[compilation_unit -> expression . , $]"},
               new Terminal[1] {new Terminal("$", 0x2)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x2, p_StaticRules[0xF], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[1] {"[expression -> exp_op12_conditional . , $]"},
               new Terminal[1] {new Terminal("$", 0x2)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x2, p_StaticRules[0xD], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[1] {"[expression -> exp_op13_assigment . , $]"},
               new Terminal[1] {new Terminal("$", 0x2)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x2, p_StaticRules[0xE], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[1] {"[exp_op12_conditional -> exp_op11 . , $]"},
               new Terminal[1] {new Terminal("$", 0x2)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x2, p_StaticRules[0xB], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[3] {"[exp_op13_assigment -> exp_op1 . = expression, $]", "[exp_op2 -> exp_op1 . , $]", "[exp_op2 -> exp_op1 . , *]"},
               new Terminal[3] {new Terminal("$", 0x2), new Terminal("_T[*]", 0x18), new Terminal("_T[=]", 0x19)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x19, 0xE }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x2, p_StaticRules[0x8], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x18, p_StaticRules[0x8], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[3] {"[exp_op11 -> exp_op2 . , $]", "[exp_op2 -> exp_op2 . * exp_op1, $]", "[exp_op2 -> exp_op2 . * exp_op1, *]"},
               new Terminal[2] {new Terminal("$", 0x2), new Terminal("_T[*]", 0x18)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x18, 0xF }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x2, p_StaticRules[0xA], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[3] {"[exp_op1 -> exp_op0 . , =]", "[exp_op1 -> exp_op0 . , $]", "[exp_op1 -> exp_op0 . , *]"},
               new Terminal[3] {new Terminal("_T[=]", 0x19), new Terminal("$", 0x2), new Terminal("_T[*]", 0x18)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x19, p_StaticRules[0x6], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x2, p_StaticRules[0x6], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x18, p_StaticRules[0x6], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[3] {"[exp_op1 -> exp_op1_cast . , =]", "[exp_op1 -> exp_op1_cast . , $]", "[exp_op1 -> exp_op1_cast . , *]"},
               new Terminal[3] {new Terminal("_T[=]", 0x19), new Terminal("$", 0x2), new Terminal("_T[*]", 0x18)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x19, p_StaticRules[0x7], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x2, p_StaticRules[0x7], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x18, p_StaticRules[0x7], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[3] {"[exp_op0 -> exp_atom . , =]", "[exp_op0 -> exp_atom . , $]", "[exp_op0 -> exp_atom . , *]"},
               new Terminal[3] {new Terminal("_T[=]", 0x19), new Terminal("$", 0x2), new Terminal("_T[*]", 0x18)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x19, p_StaticRules[0x4], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x2, p_StaticRules[0x4], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x18, p_StaticRules[0x4], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[35] {"[exp_op1_cast -> ( . type ) exp_op1, =]", "[exp_atom -> ( . expression ), =]", "[exp_op1_cast -> ( . type ) exp_op1, $]", "[exp_atom -> ( . expression ), $]", "[exp_op1_cast -> ( . type ) exp_op1, *]", "[exp_atom -> ( . expression ), *]", "[type -> . qualified_name, )]", "[expression -> . exp_op12_conditional, )]", "[expression -> . exp_op13_assigment, )]", "[qualified_name -> . IDENTIFIER _m18, )]", "[exp_op12_conditional -> . exp_op11, )]", "[exp_op13_assigment -> . exp_op1 = expression, )]", "[exp_op11 -> . exp_op2, )]", "[exp_op1 -> . exp_op0, =]", "[exp_op1 -> . exp_op1_cast, =]", "[exp_op2 -> . exp_op1, )]", "[exp_op2 -> . exp_op2 * exp_op1, )]", "[exp_op0 -> . exp_atom, =]", "[exp_op1_cast -> . ( type ) exp_op1, =]", "[exp_op1 -> . exp_op0, )]", "[exp_op1 -> . exp_op1_cast, )]", "[exp_op2 -> . exp_op1, *]", "[exp_op2 -> . exp_op2 * exp_op1, *]", "[exp_atom -> . IDENTIFIER, =]", "[exp_atom -> . ( expression ), =]", "[exp_op0 -> . exp_atom, )]", "[exp_op1_cast -> . ( type ) exp_op1, )]", "[exp_op1 -> . exp_op0, *]", "[exp_op1 -> . exp_op1_cast, *]", "[exp_atom -> . IDENTIFIER, )]", "[exp_atom -> . ( expression ), )]", "[exp_op0 -> . exp_atom, *]", "[exp_op1_cast -> . ( type ) exp_op1, *]", "[exp_atom -> . IDENTIFIER, *]", "[exp_atom -> . ( expression ), *]"},
               new Terminal[2] {new Terminal("IDENTIFIER", 0x4), new Terminal("_T[(]", 0x13)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x4, 0x15 }, { 0x13, 0x1C }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x6, 0x10 }, { 0xf, 0x11 }, { 0x5, 0x12 }, { 0xd, 0x13 }, { 0xe, 0x14 }, { 0xc, 0x16 }, { 0xa, 0x17 }, { 0xb, 0x18 }, { 0x8, 0x19 }, { 0x9, 0x1A }, { 0x7, 0x1B }},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[3] {"[exp_atom -> IDENTIFIER . , =]", "[exp_atom -> IDENTIFIER . , $]", "[exp_atom -> IDENTIFIER . , *]"},
               new Terminal[3] {new Terminal("_T[=]", 0x19), new Terminal("$", 0x2), new Terminal("_T[*]", 0x18)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x19, p_StaticRules[0x2], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x2, p_StaticRules[0x2], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x18, p_StaticRules[0x2], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[1] {"[_Axiom_ -> compilation_unit $ . , ε]"},
               new Terminal[1] {new Terminal("ε", 0x1)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x1, p_StaticRules[0x12], 0x2, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[28] {"[exp_op13_assigment -> exp_op1 = . expression, $]", "[expression -> . exp_op12_conditional, $]", "[expression -> . exp_op13_assigment, $]", "[exp_op12_conditional -> . exp_op11, $]", "[exp_op13_assigment -> . exp_op1 = expression, $]", "[exp_op11 -> . exp_op2, $]", "[exp_op1 -> . exp_op0, =]", "[exp_op1 -> . exp_op1_cast, =]", "[exp_op2 -> . exp_op1, $]", "[exp_op2 -> . exp_op2 * exp_op1, $]", "[exp_op0 -> . exp_atom, =]", "[exp_op1_cast -> . ( type ) exp_op1, =]", "[exp_op1 -> . exp_op0, $]", "[exp_op1 -> . exp_op1_cast, $]", "[exp_op2 -> . exp_op1, *]", "[exp_op2 -> . exp_op2 * exp_op1, *]", "[exp_atom -> . IDENTIFIER, =]", "[exp_atom -> . ( expression ), =]", "[exp_op0 -> . exp_atom, $]", "[exp_op1_cast -> . ( type ) exp_op1, $]", "[exp_op1 -> . exp_op0, *]", "[exp_op1 -> . exp_op1_cast, *]", "[exp_atom -> . IDENTIFIER, $]", "[exp_atom -> . ( expression ), $]", "[exp_op0 -> . exp_atom, *]", "[exp_op1_cast -> . ( type ) exp_op1, *]", "[exp_atom -> . IDENTIFIER, *]", "[exp_atom -> . ( expression ), *]"},
               new Terminal[2] {new Terminal("_T[(]", 0x13), new Terminal("IDENTIFIER", 0x4)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x13, 0xB }, { 0x4, 0xC }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0xf, 0x1D }, { 0xd, 0x3 }, { 0xe, 0x4 }, { 0xc, 0x5 }, { 0xa, 0x6 }, { 0xb, 0x7 }, { 0x8, 0x8 }, { 0x9, 0x9 }, { 0x7, 0xA }},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[14] {"[exp_op2 -> exp_op2 * . exp_op1, $]", "[exp_op2 -> exp_op2 * . exp_op1, *]", "[exp_op1 -> . exp_op0, $]", "[exp_op1 -> . exp_op1_cast, $]", "[exp_op1 -> . exp_op0, *]", "[exp_op1 -> . exp_op1_cast, *]", "[exp_op0 -> . exp_atom, $]", "[exp_op1_cast -> . ( type ) exp_op1, $]", "[exp_op0 -> . exp_atom, *]", "[exp_op1_cast -> . ( type ) exp_op1, *]", "[exp_atom -> . IDENTIFIER, $]", "[exp_atom -> . ( expression ), $]", "[exp_atom -> . IDENTIFIER, *]", "[exp_atom -> . ( expression ), *]"},
               new Terminal[2] {new Terminal("_T[(]", 0x13), new Terminal("IDENTIFIER", 0x4)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x13, 0x22 }, { 0x4, 0x23 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0xa, 0x1E }, { 0x8, 0x1F }, { 0x9, 0x20 }, { 0x7, 0x21 }},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[3] {"[exp_op1_cast -> ( type . ) exp_op1, =]", "[exp_op1_cast -> ( type . ) exp_op1, $]", "[exp_op1_cast -> ( type . ) exp_op1, *]"},
               new Terminal[1] {new Terminal("_T[)]", 0x14)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x14, 0x24 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[3] {"[exp_atom -> ( expression . ), =]", "[exp_atom -> ( expression . ), $]", "[exp_atom -> ( expression . ), *]"},
               new Terminal[1] {new Terminal("_T[)]", 0x14)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x14, 0x25 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[1] {"[type -> qualified_name . , )]"},
               new Terminal[1] {new Terminal("_T[)]", 0x14)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x14, p_StaticRules[0x1], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[1] {"[expression -> exp_op12_conditional . , )]"},
               new Terminal[1] {new Terminal("_T[)]", 0x14)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x14, p_StaticRules[0xD], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[1] {"[expression -> exp_op13_assigment . , )]"},
               new Terminal[1] {new Terminal("_T[)]", 0x14)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x14, p_StaticRules[0xE], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[6] {"[qualified_name -> IDENTIFIER . _m18, )]", "[exp_atom -> IDENTIFIER . , =]", "[exp_atom -> IDENTIFIER . , )]", "[exp_atom -> IDENTIFIER . , *]", "[_m18 -> . . IDENTIFIER _m18, )]", "[_m18 -> . , )]"},
               new Terminal[4] {new Terminal("_T[)]", 0x14), new Terminal("_T[=]", 0x19), new Terminal("_T[*]", 0x18), new Terminal("_T[.]", 0x11)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x11, 0x27 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x12, 0x26 }},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x14, p_StaticRules[0x0], 0x1, p_StaticNullChoicesSPPF[0x1]), new Reduction(0x19, p_StaticRules[0x2], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x14, p_StaticRules[0x2], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x18, p_StaticRules[0x2], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x14, p_StaticRules[0x11], 0x0, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[1] {"[exp_op12_conditional -> exp_op11 . , )]"},
               new Terminal[1] {new Terminal("_T[)]", 0x14)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x14, p_StaticRules[0xB], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[3] {"[exp_op13_assigment -> exp_op1 . = expression, )]", "[exp_op2 -> exp_op1 . , )]", "[exp_op2 -> exp_op1 . , *]"},
               new Terminal[3] {new Terminal("_T[)]", 0x14), new Terminal("_T[*]", 0x18), new Terminal("_T[=]", 0x19)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x19, 0x28 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x14, p_StaticRules[0x8], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x18, p_StaticRules[0x8], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[3] {"[exp_op11 -> exp_op2 . , )]", "[exp_op2 -> exp_op2 . * exp_op1, )]", "[exp_op2 -> exp_op2 . * exp_op1, *]"},
               new Terminal[2] {new Terminal("_T[)]", 0x14), new Terminal("_T[*]", 0x18)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x18, 0x29 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x14, p_StaticRules[0xA], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[3] {"[exp_op1 -> exp_op0 . , =]", "[exp_op1 -> exp_op0 . , )]", "[exp_op1 -> exp_op0 . , *]"},
               new Terminal[3] {new Terminal("_T[=]", 0x19), new Terminal("_T[)]", 0x14), new Terminal("_T[*]", 0x18)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x19, p_StaticRules[0x6], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x14, p_StaticRules[0x6], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x18, p_StaticRules[0x6], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[3] {"[exp_op1 -> exp_op1_cast . , =]", "[exp_op1 -> exp_op1_cast . , )]", "[exp_op1 -> exp_op1_cast . , *]"},
               new Terminal[3] {new Terminal("_T[=]", 0x19), new Terminal("_T[)]", 0x14), new Terminal("_T[*]", 0x18)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x19, p_StaticRules[0x7], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x14, p_StaticRules[0x7], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x18, p_StaticRules[0x7], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[3] {"[exp_op0 -> exp_atom . , =]", "[exp_op0 -> exp_atom . , )]", "[exp_op0 -> exp_atom . , *]"},
               new Terminal[3] {new Terminal("_T[=]", 0x19), new Terminal("_T[)]", 0x14), new Terminal("_T[*]", 0x18)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x19, p_StaticRules[0x4], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x14, p_StaticRules[0x4], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x18, p_StaticRules[0x4], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[35] {"[exp_op1_cast -> ( . type ) exp_op1, =]", "[exp_atom -> ( . expression ), =]", "[exp_op1_cast -> ( . type ) exp_op1, )]", "[exp_atom -> ( . expression ), )]", "[exp_op1_cast -> ( . type ) exp_op1, *]", "[exp_atom -> ( . expression ), *]", "[type -> . qualified_name, )]", "[expression -> . exp_op12_conditional, )]", "[expression -> . exp_op13_assigment, )]", "[qualified_name -> . IDENTIFIER _m18, )]", "[exp_op12_conditional -> . exp_op11, )]", "[exp_op13_assigment -> . exp_op1 = expression, )]", "[exp_op11 -> . exp_op2, )]", "[exp_op1 -> . exp_op0, =]", "[exp_op1 -> . exp_op1_cast, =]", "[exp_op2 -> . exp_op1, )]", "[exp_op2 -> . exp_op2 * exp_op1, )]", "[exp_op0 -> . exp_atom, =]", "[exp_op1_cast -> . ( type ) exp_op1, =]", "[exp_op1 -> . exp_op0, )]", "[exp_op1 -> . exp_op1_cast, )]", "[exp_op2 -> . exp_op1, *]", "[exp_op2 -> . exp_op2 * exp_op1, *]", "[exp_atom -> . IDENTIFIER, =]", "[exp_atom -> . ( expression ), =]", "[exp_op0 -> . exp_atom, )]", "[exp_op1_cast -> . ( type ) exp_op1, )]", "[exp_op1 -> . exp_op0, *]", "[exp_op1 -> . exp_op1_cast, *]", "[exp_atom -> . IDENTIFIER, )]", "[exp_atom -> . ( expression ), )]", "[exp_op0 -> . exp_atom, *]", "[exp_op1_cast -> . ( type ) exp_op1, *]", "[exp_atom -> . IDENTIFIER, *]", "[exp_atom -> . ( expression ), *]"},
               new Terminal[2] {new Terminal("IDENTIFIER", 0x4), new Terminal("_T[(]", 0x13)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x4, 0x15 }, { 0x13, 0x1C }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x6, 0x2A }, { 0xf, 0x2B }, { 0x5, 0x12 }, { 0xd, 0x13 }, { 0xe, 0x14 }, { 0xc, 0x16 }, { 0xa, 0x17 }, { 0xb, 0x18 }, { 0x8, 0x19 }, { 0x9, 0x1A }, { 0x7, 0x1B }},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[1] {"[exp_op13_assigment -> exp_op1 = expression . , $]"},
               new Terminal[1] {new Terminal("$", 0x2)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x2, p_StaticRules[0xC], 0x3, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[2] {"[exp_op2 -> exp_op2 * exp_op1 . , $]", "[exp_op2 -> exp_op2 * exp_op1 . , *]"},
               new Terminal[2] {new Terminal("$", 0x2), new Terminal("_T[*]", 0x18)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x2, p_StaticRules[0x9], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x18, p_StaticRules[0x9], 0x3, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[2] {"[exp_op1 -> exp_op0 . , $]", "[exp_op1 -> exp_op0 . , *]"},
               new Terminal[2] {new Terminal("$", 0x2), new Terminal("_T[*]", 0x18)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x2, p_StaticRules[0x6], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x18, p_StaticRules[0x6], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[2] {"[exp_op1 -> exp_op1_cast . , $]", "[exp_op1 -> exp_op1_cast . , *]"},
               new Terminal[2] {new Terminal("$", 0x2), new Terminal("_T[*]", 0x18)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x2, p_StaticRules[0x7], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x18, p_StaticRules[0x7], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[2] {"[exp_op0 -> exp_atom . , $]", "[exp_op0 -> exp_atom . , *]"},
               new Terminal[2] {new Terminal("$", 0x2), new Terminal("_T[*]", 0x18)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x2, p_StaticRules[0x4], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x18, p_StaticRules[0x4], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[33] {"[exp_op1_cast -> ( . type ) exp_op1, $]", "[exp_op1_cast -> ( . type ) exp_op1, *]", "[exp_atom -> ( . expression ), $]", "[exp_atom -> ( . expression ), *]", "[type -> . qualified_name, )]", "[expression -> . exp_op12_conditional, )]", "[expression -> . exp_op13_assigment, )]", "[qualified_name -> . IDENTIFIER _m18, )]", "[exp_op12_conditional -> . exp_op11, )]", "[exp_op13_assigment -> . exp_op1 = expression, )]", "[exp_op11 -> . exp_op2, )]", "[exp_op1 -> . exp_op0, =]", "[exp_op1 -> . exp_op1_cast, =]", "[exp_op2 -> . exp_op1, )]", "[exp_op2 -> . exp_op2 * exp_op1, )]", "[exp_op0 -> . exp_atom, =]", "[exp_op1_cast -> . ( type ) exp_op1, =]", "[exp_op1 -> . exp_op0, )]", "[exp_op1 -> . exp_op1_cast, )]", "[exp_op2 -> . exp_op1, *]", "[exp_op2 -> . exp_op2 * exp_op1, *]", "[exp_atom -> . IDENTIFIER, =]", "[exp_atom -> . ( expression ), =]", "[exp_op0 -> . exp_atom, )]", "[exp_op1_cast -> . ( type ) exp_op1, )]", "[exp_op1 -> . exp_op0, *]", "[exp_op1 -> . exp_op1_cast, *]", "[exp_atom -> . IDENTIFIER, )]", "[exp_atom -> . ( expression ), )]", "[exp_op0 -> . exp_atom, *]", "[exp_op1_cast -> . ( type ) exp_op1, *]", "[exp_atom -> . IDENTIFIER, *]", "[exp_atom -> . ( expression ), *]"},
               new Terminal[2] {new Terminal("IDENTIFIER", 0x4), new Terminal("_T[(]", 0x13)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x4, 0x15 }, { 0x13, 0x1C }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x6, 0x2C }, { 0xf, 0x2D }, { 0x5, 0x12 }, { 0xd, 0x13 }, { 0xe, 0x14 }, { 0xc, 0x16 }, { 0xa, 0x17 }, { 0xb, 0x18 }, { 0x8, 0x19 }, { 0x9, 0x1A }, { 0x7, 0x1B }},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[2] {"[exp_atom -> IDENTIFIER . , $]", "[exp_atom -> IDENTIFIER . , *]"},
               new Terminal[2] {new Terminal("$", 0x2), new Terminal("_T[*]", 0x18)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x2, p_StaticRules[0x2], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x18, p_StaticRules[0x2], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[21] {"[exp_op1_cast -> ( type ) . exp_op1, =]", "[exp_op1_cast -> ( type ) . exp_op1, $]", "[exp_op1_cast -> ( type ) . exp_op1, *]", "[exp_op1 -> . exp_op0, =]", "[exp_op1 -> . exp_op1_cast, =]", "[exp_op1 -> . exp_op0, $]", "[exp_op1 -> . exp_op1_cast, $]", "[exp_op1 -> . exp_op0, *]", "[exp_op1 -> . exp_op1_cast, *]", "[exp_op0 -> . exp_atom, =]", "[exp_op1_cast -> . ( type ) exp_op1, =]", "[exp_op0 -> . exp_atom, $]", "[exp_op1_cast -> . ( type ) exp_op1, $]", "[exp_op0 -> . exp_atom, *]", "[exp_op1_cast -> . ( type ) exp_op1, *]", "[exp_atom -> . IDENTIFIER, =]", "[exp_atom -> . ( expression ), =]", "[exp_atom -> . IDENTIFIER, $]", "[exp_atom -> . ( expression ), $]", "[exp_atom -> . IDENTIFIER, *]", "[exp_atom -> . ( expression ), *]"},
               new Terminal[2] {new Terminal("_T[(]", 0x13), new Terminal("IDENTIFIER", 0x4)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x13, 0xB }, { 0x4, 0xC }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0xa, 0x2E }, { 0x8, 0x8 }, { 0x9, 0x9 }, { 0x7, 0xA }},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[3] {"[exp_atom -> ( expression ) . , =]", "[exp_atom -> ( expression ) . , $]", "[exp_atom -> ( expression ) . , *]"},
               new Terminal[3] {new Terminal("_T[=]", 0x19), new Terminal("$", 0x2), new Terminal("_T[*]", 0x18)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x19, p_StaticRules[0x3], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x2, p_StaticRules[0x3], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x18, p_StaticRules[0x3], 0x3, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[1] {"[qualified_name -> IDENTIFIER _m18 . , )]"},
               new Terminal[1] {new Terminal("_T[)]", 0x14)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x14, p_StaticRules[0x0], 0x2, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[1] {"[_m18 -> . . IDENTIFIER _m18, )]"},
               new Terminal[1] {new Terminal("IDENTIFIER", 0x4)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x4, 0x2F }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[28] {"[exp_op13_assigment -> exp_op1 = . expression, )]", "[expression -> . exp_op12_conditional, )]", "[expression -> . exp_op13_assigment, )]", "[exp_op12_conditional -> . exp_op11, )]", "[exp_op13_assigment -> . exp_op1 = expression, )]", "[exp_op11 -> . exp_op2, )]", "[exp_op1 -> . exp_op0, =]", "[exp_op1 -> . exp_op1_cast, =]", "[exp_op2 -> . exp_op1, )]", "[exp_op2 -> . exp_op2 * exp_op1, )]", "[exp_op0 -> . exp_atom, =]", "[exp_op1_cast -> . ( type ) exp_op1, =]", "[exp_op1 -> . exp_op0, )]", "[exp_op1 -> . exp_op1_cast, )]", "[exp_op2 -> . exp_op1, *]", "[exp_op2 -> . exp_op2 * exp_op1, *]", "[exp_atom -> . IDENTIFIER, =]", "[exp_atom -> . ( expression ), =]", "[exp_op0 -> . exp_atom, )]", "[exp_op1_cast -> . ( type ) exp_op1, )]", "[exp_op1 -> . exp_op0, *]", "[exp_op1 -> . exp_op1_cast, *]", "[exp_atom -> . IDENTIFIER, )]", "[exp_atom -> . ( expression ), )]", "[exp_op0 -> . exp_atom, *]", "[exp_op1_cast -> . ( type ) exp_op1, *]", "[exp_atom -> . IDENTIFIER, *]", "[exp_atom -> . ( expression ), *]"},
               new Terminal[2] {new Terminal("_T[(]", 0x13), new Terminal("IDENTIFIER", 0x4)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x13, 0x1C }, { 0x4, 0x31 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0xf, 0x30 }, { 0xd, 0x13 }, { 0xe, 0x14 }, { 0xc, 0x16 }, { 0xa, 0x17 }, { 0xb, 0x18 }, { 0x8, 0x19 }, { 0x9, 0x1A }, { 0x7, 0x1B }},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[14] {"[exp_op2 -> exp_op2 * . exp_op1, )]", "[exp_op2 -> exp_op2 * . exp_op1, *]", "[exp_op1 -> . exp_op0, )]", "[exp_op1 -> . exp_op1_cast, )]", "[exp_op1 -> . exp_op0, *]", "[exp_op1 -> . exp_op1_cast, *]", "[exp_op0 -> . exp_atom, )]", "[exp_op1_cast -> . ( type ) exp_op1, )]", "[exp_op0 -> . exp_atom, *]", "[exp_op1_cast -> . ( type ) exp_op1, *]", "[exp_atom -> . IDENTIFIER, )]", "[exp_atom -> . ( expression ), )]", "[exp_atom -> . IDENTIFIER, *]", "[exp_atom -> . ( expression ), *]"},
               new Terminal[2] {new Terminal("_T[(]", 0x13), new Terminal("IDENTIFIER", 0x4)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x13, 0x36 }, { 0x4, 0x37 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0xa, 0x32 }, { 0x8, 0x33 }, { 0x9, 0x34 }, { 0x7, 0x35 }},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[3] {"[exp_op1_cast -> ( type . ) exp_op1, =]", "[exp_op1_cast -> ( type . ) exp_op1, )]", "[exp_op1_cast -> ( type . ) exp_op1, *]"},
               new Terminal[1] {new Terminal("_T[)]", 0x14)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x14, 0x38 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[3] {"[exp_atom -> ( expression . ), =]", "[exp_atom -> ( expression . ), )]", "[exp_atom -> ( expression . ), *]"},
               new Terminal[1] {new Terminal("_T[)]", 0x14)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x14, 0x39 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[2] {"[exp_op1_cast -> ( type . ) exp_op1, $]", "[exp_op1_cast -> ( type . ) exp_op1, *]"},
               new Terminal[1] {new Terminal("_T[)]", 0x14)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x14, 0x3A }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[2] {"[exp_atom -> ( expression . ), $]", "[exp_atom -> ( expression . ), *]"},
               new Terminal[1] {new Terminal("_T[)]", 0x14)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x14, 0x3B }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[3] {"[exp_op1_cast -> ( type ) exp_op1 . , =]", "[exp_op1_cast -> ( type ) exp_op1 . , $]", "[exp_op1_cast -> ( type ) exp_op1 . , *]"},
               new Terminal[3] {new Terminal("_T[=]", 0x19), new Terminal("$", 0x2), new Terminal("_T[*]", 0x18)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x19, p_StaticRules[0x5], 0x4, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x2, p_StaticRules[0x5], 0x4, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x18, p_StaticRules[0x5], 0x4, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[3] {"[_m18 -> . IDENTIFIER . _m18, )]", "[_m18 -> . . IDENTIFIER _m18, )]", "[_m18 -> . , )]"},
               new Terminal[2] {new Terminal("_T[)]", 0x14), new Terminal("_T[.]", 0x11)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x11, 0x27 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x12, 0x3C }},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x14, p_StaticRules[0x10], 0x2, p_StaticNullChoicesSPPF[0x1]), new Reduction(0x14, p_StaticRules[0x11], 0x0, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[1] {"[exp_op13_assigment -> exp_op1 = expression . , )]"},
               new Terminal[1] {new Terminal("_T[)]", 0x14)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x14, p_StaticRules[0xC], 0x3, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[3] {"[exp_atom -> IDENTIFIER . , =]", "[exp_atom -> IDENTIFIER . , )]", "[exp_atom -> IDENTIFIER . , *]"},
               new Terminal[3] {new Terminal("_T[=]", 0x19), new Terminal("_T[)]", 0x14), new Terminal("_T[*]", 0x18)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x19, p_StaticRules[0x2], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x14, p_StaticRules[0x2], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x18, p_StaticRules[0x2], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[2] {"[exp_op2 -> exp_op2 * exp_op1 . , )]", "[exp_op2 -> exp_op2 * exp_op1 . , *]"},
               new Terminal[2] {new Terminal("_T[)]", 0x14), new Terminal("_T[*]", 0x18)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x14, p_StaticRules[0x9], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x18, p_StaticRules[0x9], 0x3, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[2] {"[exp_op1 -> exp_op0 . , )]", "[exp_op1 -> exp_op0 . , *]"},
               new Terminal[2] {new Terminal("_T[)]", 0x14), new Terminal("_T[*]", 0x18)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x14, p_StaticRules[0x6], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x18, p_StaticRules[0x6], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[2] {"[exp_op1 -> exp_op1_cast . , )]", "[exp_op1 -> exp_op1_cast . , *]"},
               new Terminal[2] {new Terminal("_T[)]", 0x14), new Terminal("_T[*]", 0x18)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x14, p_StaticRules[0x7], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x18, p_StaticRules[0x7], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[2] {"[exp_op0 -> exp_atom . , )]", "[exp_op0 -> exp_atom . , *]"},
               new Terminal[2] {new Terminal("_T[)]", 0x14), new Terminal("_T[*]", 0x18)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x14, p_StaticRules[0x4], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x18, p_StaticRules[0x4], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[33] {"[exp_op1_cast -> ( . type ) exp_op1, )]", "[exp_op1_cast -> ( . type ) exp_op1, *]", "[exp_atom -> ( . expression ), )]", "[exp_atom -> ( . expression ), *]", "[type -> . qualified_name, )]", "[expression -> . exp_op12_conditional, )]", "[expression -> . exp_op13_assigment, )]", "[qualified_name -> . IDENTIFIER _m18, )]", "[exp_op12_conditional -> . exp_op11, )]", "[exp_op13_assigment -> . exp_op1 = expression, )]", "[exp_op11 -> . exp_op2, )]", "[exp_op1 -> . exp_op0, =]", "[exp_op1 -> . exp_op1_cast, =]", "[exp_op2 -> . exp_op1, )]", "[exp_op2 -> . exp_op2 * exp_op1, )]", "[exp_op0 -> . exp_atom, =]", "[exp_op1_cast -> . ( type ) exp_op1, =]", "[exp_op1 -> . exp_op0, )]", "[exp_op1 -> . exp_op1_cast, )]", "[exp_op2 -> . exp_op1, *]", "[exp_op2 -> . exp_op2 * exp_op1, *]", "[exp_atom -> . IDENTIFIER, =]", "[exp_atom -> . ( expression ), =]", "[exp_op0 -> . exp_atom, )]", "[exp_op1_cast -> . ( type ) exp_op1, )]", "[exp_op1 -> . exp_op0, *]", "[exp_op1 -> . exp_op1_cast, *]", "[exp_atom -> . IDENTIFIER, )]", "[exp_atom -> . ( expression ), )]", "[exp_op0 -> . exp_atom, *]", "[exp_op1_cast -> . ( type ) exp_op1, *]", "[exp_atom -> . IDENTIFIER, *]", "[exp_atom -> . ( expression ), *]"},
               new Terminal[2] {new Terminal("IDENTIFIER", 0x4), new Terminal("_T[(]", 0x13)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x4, 0x15 }, { 0x13, 0x1C }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x6, 0x3D }, { 0xf, 0x3E }, { 0x5, 0x12 }, { 0xd, 0x13 }, { 0xe, 0x14 }, { 0xc, 0x16 }, { 0xa, 0x17 }, { 0xb, 0x18 }, { 0x8, 0x19 }, { 0x9, 0x1A }, { 0x7, 0x1B }},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[2] {"[exp_atom -> IDENTIFIER . , )]", "[exp_atom -> IDENTIFIER . , *]"},
               new Terminal[2] {new Terminal("_T[)]", 0x14), new Terminal("_T[*]", 0x18)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x14, p_StaticRules[0x2], 0x1, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x18, p_StaticRules[0x2], 0x1, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[21] {"[exp_op1_cast -> ( type ) . exp_op1, =]", "[exp_op1_cast -> ( type ) . exp_op1, )]", "[exp_op1_cast -> ( type ) . exp_op1, *]", "[exp_op1 -> . exp_op0, =]", "[exp_op1 -> . exp_op1_cast, =]", "[exp_op1 -> . exp_op0, )]", "[exp_op1 -> . exp_op1_cast, )]", "[exp_op1 -> . exp_op0, *]", "[exp_op1 -> . exp_op1_cast, *]", "[exp_op0 -> . exp_atom, =]", "[exp_op1_cast -> . ( type ) exp_op1, =]", "[exp_op0 -> . exp_atom, )]", "[exp_op1_cast -> . ( type ) exp_op1, )]", "[exp_op0 -> . exp_atom, *]", "[exp_op1_cast -> . ( type ) exp_op1, *]", "[exp_atom -> . IDENTIFIER, =]", "[exp_atom -> . ( expression ), =]", "[exp_atom -> . IDENTIFIER, )]", "[exp_atom -> . ( expression ), )]", "[exp_atom -> . IDENTIFIER, *]", "[exp_atom -> . ( expression ), *]"},
               new Terminal[2] {new Terminal("_T[(]", 0x13), new Terminal("IDENTIFIER", 0x4)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x13, 0x1C }, { 0x4, 0x31 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0xa, 0x3F }, { 0x8, 0x19 }, { 0x9, 0x1A }, { 0x7, 0x1B }},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[3] {"[exp_atom -> ( expression ) . , =]", "[exp_atom -> ( expression ) . , )]", "[exp_atom -> ( expression ) . , *]"},
               new Terminal[3] {new Terminal("_T[=]", 0x19), new Terminal("_T[)]", 0x14), new Terminal("_T[*]", 0x18)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x19, p_StaticRules[0x3], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x14, p_StaticRules[0x3], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x18, p_StaticRules[0x3], 0x3, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[14] {"[exp_op1_cast -> ( type ) . exp_op1, $]", "[exp_op1_cast -> ( type ) . exp_op1, *]", "[exp_op1 -> . exp_op0, $]", "[exp_op1 -> . exp_op1_cast, $]", "[exp_op1 -> . exp_op0, *]", "[exp_op1 -> . exp_op1_cast, *]", "[exp_op0 -> . exp_atom, $]", "[exp_op1_cast -> . ( type ) exp_op1, $]", "[exp_op0 -> . exp_atom, *]", "[exp_op1_cast -> . ( type ) exp_op1, *]", "[exp_atom -> . IDENTIFIER, $]", "[exp_atom -> . ( expression ), $]", "[exp_atom -> . IDENTIFIER, *]", "[exp_atom -> . ( expression ), *]"},
               new Terminal[2] {new Terminal("_T[(]", 0x13), new Terminal("IDENTIFIER", 0x4)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x13, 0x22 }, { 0x4, 0x23 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0xa, 0x40 }, { 0x8, 0x1F }, { 0x9, 0x20 }, { 0x7, 0x21 }},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[2] {"[exp_atom -> ( expression ) . , $]", "[exp_atom -> ( expression ) . , *]"},
               new Terminal[2] {new Terminal("$", 0x2), new Terminal("_T[*]", 0x18)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x2, p_StaticRules[0x3], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x18, p_StaticRules[0x3], 0x3, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[1] {"[_m18 -> . IDENTIFIER _m18 . , )]"},
               new Terminal[1] {new Terminal("_T[)]", 0x14)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x14, p_StaticRules[0x10], 0x3, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[2] {"[exp_op1_cast -> ( type . ) exp_op1, )]", "[exp_op1_cast -> ( type . ) exp_op1, *]"},
               new Terminal[1] {new Terminal("_T[)]", 0x14)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x14, 0x41 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[2] {"[exp_atom -> ( expression . ), )]", "[exp_atom -> ( expression . ), *]"},
               new Terminal[1] {new Terminal("_T[)]", 0x14)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x14, 0x42 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[3] {"[exp_op1_cast -> ( type ) exp_op1 . , =]", "[exp_op1_cast -> ( type ) exp_op1 . , )]", "[exp_op1_cast -> ( type ) exp_op1 . , *]"},
               new Terminal[3] {new Terminal("_T[=]", 0x19), new Terminal("_T[)]", 0x14), new Terminal("_T[*]", 0x18)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x19, p_StaticRules[0x5], 0x4, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x14, p_StaticRules[0x5], 0x4, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x18, p_StaticRules[0x5], 0x4, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[2] {"[exp_op1_cast -> ( type ) exp_op1 . , $]", "[exp_op1_cast -> ( type ) exp_op1 . , *]"},
               new Terminal[2] {new Terminal("$", 0x2), new Terminal("_T[*]", 0x18)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x2, p_StaticRules[0x5], 0x4, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x18, p_StaticRules[0x5], 0x4, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[14] {"[exp_op1_cast -> ( type ) . exp_op1, )]", "[exp_op1_cast -> ( type ) . exp_op1, *]", "[exp_op1 -> . exp_op0, )]", "[exp_op1 -> . exp_op1_cast, )]", "[exp_op1 -> . exp_op0, *]", "[exp_op1 -> . exp_op1_cast, *]", "[exp_op0 -> . exp_atom, )]", "[exp_op1_cast -> . ( type ) exp_op1, )]", "[exp_op0 -> . exp_atom, *]", "[exp_op1_cast -> . ( type ) exp_op1, *]", "[exp_atom -> . IDENTIFIER, )]", "[exp_atom -> . ( expression ), )]", "[exp_atom -> . IDENTIFIER, *]", "[exp_atom -> . ( expression ), *]"},
               new Terminal[2] {new Terminal("_T[(]", 0x13), new Terminal("IDENTIFIER", 0x4)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0x13, 0x36 }, { 0x4, 0x37 }},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {{ 0xa, 0x43 }, { 0x8, 0x33 }, { 0x9, 0x34 }, { 0x7, 0x35 }},
               new System.Collections.Generic.List<Reduction>() {})
            , new State(
               new string[2] {"[exp_atom -> ( expression ) . , )]", "[exp_atom -> ( expression ) . , *]"},
               new Terminal[2] {new Terminal("_T[)]", 0x14), new Terminal("_T[*]", 0x18)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x14, p_StaticRules[0x3], 0x3, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x18, p_StaticRules[0x3], 0x3, p_StaticNullChoicesSPPF[0x0])})
            , new State(
               new string[2] {"[exp_op1_cast -> ( type ) exp_op1 . , )]", "[exp_op1_cast -> ( type ) exp_op1 . , *]"},
               new Terminal[2] {new Terminal("_T[)]", 0x14), new Terminal("_T[*]", 0x18)},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.Dictionary<ushort, ushort>() {},
               new System.Collections.Generic.List<Reduction>() {new Reduction(0x14, p_StaticRules[0x5], 0x4, p_StaticNullChoicesSPPF[0x0]), new Reduction(0x18, p_StaticRules[0x5], 0x4, p_StaticNullChoicesSPPF[0x0])})
 };
        private static void BuildNullables() { 
            System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> temp = new System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode>();
            p_StaticNullChoicesSPPF[0].AddFamily(temp);
            temp.Clear();
            temp.Add(p_StaticNullVarsSPPF[0]);
            p_StaticNullChoicesSPPF[1].AddFamily(temp);
            temp.Clear();
            temp.Add(p_StaticNullVarsSPPF[0]);
            p_StaticNullChoicesSPPF[2].AddFamily(temp);
            temp.Clear();
            p_StaticNullVarsSPPF[0].AddFamily(temp);
            temp.Clear();
        }
        public interface Actions
        {
        void OnCast(Hime.Redist.Parsers.SyntaxTreeNode SubRoot);
        }
        protected override void setup()
        {
            p_NullVarsSPPF = p_StaticNullVarsSPPF;
            p_NullChoicesSPPF = p_StaticNullChoicesSPPF;
            p_Rules = p_StaticRules;
            p_States = p_StaticStates;
            p_AxiomID = 0x10;
            p_AxiomNullSPPF = 0x10;
            p_AxiomPrimeID = 0x1A;
        }
        static AmbiguousLALR1_Parser()
        {
            BuildNullables();
        }
        private Actions p_Actions;
        public AmbiguousLALR1_Parser(Actions actions, AmbiguousLALR1_Lexer lexer) : base (lexer) { p_Actions = actions; }
    }
}
