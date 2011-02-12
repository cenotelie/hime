namespace Analyzer
{
    public class AmbiguousLALR1_Lexer : Hime.Redist.Parsers.LexerText
    {
        private static ushort[] p_StaticSymbolsSID = { 0x11, 0x13, 0x14, 0x17, 0x18, 0x3, 0x4 };
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
        private static void Production_5_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes, int length)
        {
        }
        private static void Production_6_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes, int length)
        {
        }
        private static void Production_7_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes, int length)
        {
        }
        private static void Production_7_1 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes, int length)
        {
        }
        private static void Production_8_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes, int length)
        {
        }
        private static void Production_9_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes, int length)
        {
        }
        private static void Production_A_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes, int length)
        {
        }
        private static void Production_A_1 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes, int length)
        {
        }
        private static void Production_B_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes, int length)
        {
        }
        private static void Production_B_1 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes, int length)
        {
        }
        private static void Production_C_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes, int length)
        {
        }
        private static void Production_D_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes, int length)
        {
        }
        private static void Production_E_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes, int length)
        {
        }
        private static void Production_F_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes, int length)
        {
        }
        private static void Production_F_1 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes, int length)
        {
        }
        private static void Production_10_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes, int length)
        {
        }
        private static void Production_12_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes, int length)
        {
        }
        private static void Production_12_1 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes, int length)
        {
        }
        private static void Production_19_0 (Hime.Redist.Parsers.BaseRNGLR1Parser parser, System.Collections.Generic.List<Hime.Redist.Parsers.SPPFNode> nodes, int length)
        {
        }
        private static Production[] p_StaticRules = { Production_5_0, Production_6_0, Production_7_0, Production_7_1, Production_8_0, Production_9_0, Production_A_0, Production_A_1, Production_B_0, Production_B_1, Production_C_0, Production_D_0, Production_E_0, Production_F_0, Production_F_1, Production_10_0, Production_12_0, Production_12_1, Production_19_0 };
        private static ushort[] p_StaticRulesHeadID = { 0x5, 0x6, 0x7, 0x7, 0x8, 0x9, 0xA, 0xA, 0xB, 0xB, 0xC, 0xD, 0xE, 0xF, 0xF, 0x10, 0x12, 0x12, 0x19 };
        private static string[] p_StaticRulesHeadName = { "qualified_name", "type", "exp_atom", "exp_atom", "exp_op0", "exp_op1_cast", "exp_op1", "exp_op1", "exp_op2", "exp_op2", "exp_op11", "exp_op12_conditional", "exp_op13_assigment", "expression", "expression", "compilation_unit", "_m18", "_m18", "_Axiom_" };
        private static ushort[] p_StaticRulesParserLength = { 0x2, 0x1, 0x1, 0x3, 0x1, 0x4, 0x1, 0x1, 0x1, 0x3, 0x1, 0x1, 0x3, 0x1, 0x1, 0x1, 0x3, 0x0, 0x2 };
        private static ushort[] p_StateExpectedIDs_0 = { 0x13, 0x4 };
        private static string[] p_StateExpectedNames_0 = { "_T[(]", "IDENTIFIER" };
        private static string[] p_StateItems_0 = { "[_Axiom_ -> . compilation_unit $]", "[compilation_unit -> . expression]", "[expression -> . exp_op12_conditional]", "[expression -> . exp_op13_assigment]", "[exp_op12_conditional -> . exp_op11]", "[exp_op13_assigment -> . exp_op1 = expression]", "[exp_op11 -> . exp_op2]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_op2 -> . exp_op1]", "[exp_op2 -> . exp_op2 * exp_op1]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_op2 -> . exp_op1]", "[exp_op2 -> . exp_op2 * exp_op1]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]" };
        private static ushort[][] p_StateShiftsOnTerminal_0 = { new ushort[2] { 0x13, 0xB }, new ushort[2] { 0x4, 0xC } };
        private static ushort[][] p_StateShiftsOnVariable_0 = { new ushort[2] { 0x10, 0x1 }, new ushort[2] { 0xf, 0x2 }, new ushort[2] { 0xd, 0x3 }, new ushort[2] { 0xe, 0x4 }, new ushort[2] { 0xc, 0x5 }, new ushort[2] { 0xa, 0x6 }, new ushort[2] { 0xb, 0x7 }, new ushort[2] { 0x8, 0x8 }, new ushort[2] { 0x9, 0x9 }, new ushort[2] { 0x7, 0xA } };
        private static ushort[][] p_StateReducsOnTerminal_0 = {  };
        private static ushort[] p_StateExpectedIDs_1 = { 0x2 };
        private static string[] p_StateExpectedNames_1 = { "$" };
        private static string[] p_StateItems_1 = { "[_Axiom_ -> compilation_unit . $]" };
        private static ushort[][] p_StateShiftsOnTerminal_1 = { new ushort[2] { 0x2, 0xD } };
        private static ushort[][] p_StateShiftsOnVariable_1 = {  };
        private static ushort[][] p_StateReducsOnTerminal_1 = {  };
        private static ushort[] p_StateExpectedIDs_2 = { 0x2 };
        private static string[] p_StateExpectedNames_2 = { "$" };
        private static string[] p_StateItems_2 = { "[compilation_unit -> expression . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_2 = {  };
        private static ushort[][] p_StateShiftsOnVariable_2 = {  };
        private static ushort[][] p_StateReducsOnTerminal_2 = { new ushort[3] { 0x2, 0xF, 0x1 } };
        private static ushort[] p_StateExpectedIDs_3 = { 0x2 };
        private static string[] p_StateExpectedNames_3 = { "$" };
        private static string[] p_StateItems_3 = { "[expression -> exp_op12_conditional . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_3 = {  };
        private static ushort[][] p_StateShiftsOnVariable_3 = {  };
        private static ushort[][] p_StateReducsOnTerminal_3 = { new ushort[3] { 0x2, 0xD, 0x1 } };
        private static ushort[] p_StateExpectedIDs_4 = { 0x2 };
        private static string[] p_StateExpectedNames_4 = { "$" };
        private static string[] p_StateItems_4 = { "[expression -> exp_op13_assigment . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_4 = {  };
        private static ushort[][] p_StateShiftsOnVariable_4 = {  };
        private static ushort[][] p_StateReducsOnTerminal_4 = { new ushort[3] { 0x2, 0xE, 0x1 } };
        private static ushort[] p_StateExpectedIDs_5 = { 0x2 };
        private static string[] p_StateExpectedNames_5 = { "$" };
        private static string[] p_StateItems_5 = { "[exp_op12_conditional -> exp_op11 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_5 = {  };
        private static ushort[][] p_StateShiftsOnVariable_5 = {  };
        private static ushort[][] p_StateReducsOnTerminal_5 = { new ushort[3] { 0x2, 0xB, 0x1 } };
        private static ushort[] p_StateExpectedIDs_6 = { 0x2, 0x17, 0x18 };
        private static string[] p_StateExpectedNames_6 = { "$", "_T[*]", "_T[=]" };
        private static string[] p_StateItems_6 = { "[exp_op13_assigment -> exp_op1 . = expression]", "[exp_op2 -> exp_op1 . ]", "[exp_op2 -> exp_op1 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_6 = { new ushort[2] { 0x18, 0xE } };
        private static ushort[][] p_StateShiftsOnVariable_6 = {  };
        private static ushort[][] p_StateReducsOnTerminal_6 = { new ushort[3] { 0x2, 0x8, 0x1 }, new ushort[3] { 0x17, 0x8, 0x1 } };
        private static ushort[] p_StateExpectedIDs_7 = { 0x2, 0x17 };
        private static string[] p_StateExpectedNames_7 = { "$", "_T[*]" };
        private static string[] p_StateItems_7 = { "[exp_op11 -> exp_op2 . ]", "[exp_op2 -> exp_op2 . * exp_op1]", "[exp_op2 -> exp_op2 . * exp_op1]" };
        private static ushort[][] p_StateShiftsOnTerminal_7 = { new ushort[2] { 0x17, 0xF } };
        private static ushort[][] p_StateShiftsOnVariable_7 = {  };
        private static ushort[][] p_StateReducsOnTerminal_7 = { new ushort[3] { 0x2, 0xA, 0x1 } };
        private static ushort[] p_StateExpectedIDs_8 = { 0x18, 0x2, 0x17 };
        private static string[] p_StateExpectedNames_8 = { "_T[=]", "$", "_T[*]" };
        private static string[] p_StateItems_8 = { "[exp_op1 -> exp_op0 . ]", "[exp_op1 -> exp_op0 . ]", "[exp_op1 -> exp_op0 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_8 = {  };
        private static ushort[][] p_StateShiftsOnVariable_8 = {  };
        private static ushort[][] p_StateReducsOnTerminal_8 = { new ushort[3] { 0x18, 0x6, 0x1 }, new ushort[3] { 0x2, 0x6, 0x1 }, new ushort[3] { 0x17, 0x6, 0x1 } };
        private static ushort[] p_StateExpectedIDs_9 = { 0x18, 0x2, 0x17 };
        private static string[] p_StateExpectedNames_9 = { "_T[=]", "$", "_T[*]" };
        private static string[] p_StateItems_9 = { "[exp_op1 -> exp_op1_cast . ]", "[exp_op1 -> exp_op1_cast . ]", "[exp_op1 -> exp_op1_cast . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_9 = {  };
        private static ushort[][] p_StateShiftsOnVariable_9 = {  };
        private static ushort[][] p_StateReducsOnTerminal_9 = { new ushort[3] { 0x18, 0x7, 0x1 }, new ushort[3] { 0x2, 0x7, 0x1 }, new ushort[3] { 0x17, 0x7, 0x1 } };
        private static ushort[] p_StateExpectedIDs_A = { 0x18, 0x2, 0x17 };
        private static string[] p_StateExpectedNames_A = { "_T[=]", "$", "_T[*]" };
        private static string[] p_StateItems_A = { "[exp_op0 -> exp_atom . ]", "[exp_op0 -> exp_atom . ]", "[exp_op0 -> exp_atom . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_A = {  };
        private static ushort[][] p_StateShiftsOnVariable_A = {  };
        private static ushort[][] p_StateReducsOnTerminal_A = { new ushort[3] { 0x18, 0x4, 0x1 }, new ushort[3] { 0x2, 0x4, 0x1 }, new ushort[3] { 0x17, 0x4, 0x1 } };
        private static ushort[] p_StateExpectedIDs_B = { 0x4, 0x13 };
        private static string[] p_StateExpectedNames_B = { "IDENTIFIER", "_T[(]" };
        private static string[] p_StateItems_B = { "[exp_op1_cast -> ( . type ) exp_op1]", "[exp_atom -> ( . expression )]", "[exp_op1_cast -> ( . type ) exp_op1]", "[exp_atom -> ( . expression )]", "[exp_op1_cast -> ( . type ) exp_op1]", "[exp_atom -> ( . expression )]", "[type -> . qualified_name]", "[expression -> . exp_op12_conditional]", "[expression -> . exp_op13_assigment]", "[qualified_name -> . IDENTIFIER _m18]", "[exp_op12_conditional -> . exp_op11]", "[exp_op13_assigment -> . exp_op1 = expression]", "[exp_op11 -> . exp_op2]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_op2 -> . exp_op1]", "[exp_op2 -> . exp_op2 * exp_op1]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_op2 -> . exp_op1]", "[exp_op2 -> . exp_op2 * exp_op1]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]" };
        private static ushort[][] p_StateShiftsOnTerminal_B = { new ushort[2] { 0x4, 0x15 }, new ushort[2] { 0x13, 0x1C } };
        private static ushort[][] p_StateShiftsOnVariable_B = { new ushort[2] { 0x6, 0x10 }, new ushort[2] { 0xf, 0x11 }, new ushort[2] { 0x5, 0x12 }, new ushort[2] { 0xd, 0x13 }, new ushort[2] { 0xe, 0x14 }, new ushort[2] { 0xc, 0x16 }, new ushort[2] { 0xa, 0x17 }, new ushort[2] { 0xb, 0x18 }, new ushort[2] { 0x8, 0x19 }, new ushort[2] { 0x9, 0x1A }, new ushort[2] { 0x7, 0x1B } };
        private static ushort[][] p_StateReducsOnTerminal_B = {  };
        private static ushort[] p_StateExpectedIDs_C = { 0x18, 0x2, 0x17 };
        private static string[] p_StateExpectedNames_C = { "_T[=]", "$", "_T[*]" };
        private static string[] p_StateItems_C = { "[exp_atom -> IDENTIFIER . ]", "[exp_atom -> IDENTIFIER . ]", "[exp_atom -> IDENTIFIER . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_C = {  };
        private static ushort[][] p_StateShiftsOnVariable_C = {  };
        private static ushort[][] p_StateReducsOnTerminal_C = { new ushort[3] { 0x18, 0x2, 0x1 }, new ushort[3] { 0x2, 0x2, 0x1 }, new ushort[3] { 0x17, 0x2, 0x1 } };
        private static ushort[] p_StateExpectedIDs_D = { 0x1 };
        private static string[] p_StateExpectedNames_D = { "ε" };
        private static string[] p_StateItems_D = { "[_Axiom_ -> compilation_unit $ . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_D = {  };
        private static ushort[][] p_StateShiftsOnVariable_D = {  };
        private static ushort[][] p_StateReducsOnTerminal_D = { new ushort[3] { 0x1, 0x12, 0x2 } };
        private static ushort[] p_StateExpectedIDs_E = { 0x13, 0x4 };
        private static string[] p_StateExpectedNames_E = { "_T[(]", "IDENTIFIER" };
        private static string[] p_StateItems_E = { "[exp_op13_assigment -> exp_op1 = . expression]", "[expression -> . exp_op12_conditional]", "[expression -> . exp_op13_assigment]", "[exp_op12_conditional -> . exp_op11]", "[exp_op13_assigment -> . exp_op1 = expression]", "[exp_op11 -> . exp_op2]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_op2 -> . exp_op1]", "[exp_op2 -> . exp_op2 * exp_op1]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_op2 -> . exp_op1]", "[exp_op2 -> . exp_op2 * exp_op1]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]" };
        private static ushort[][] p_StateShiftsOnTerminal_E = { new ushort[2] { 0x13, 0xB }, new ushort[2] { 0x4, 0xC } };
        private static ushort[][] p_StateShiftsOnVariable_E = { new ushort[2] { 0xf, 0x1D }, new ushort[2] { 0xd, 0x3 }, new ushort[2] { 0xe, 0x4 }, new ushort[2] { 0xc, 0x5 }, new ushort[2] { 0xa, 0x6 }, new ushort[2] { 0xb, 0x7 }, new ushort[2] { 0x8, 0x8 }, new ushort[2] { 0x9, 0x9 }, new ushort[2] { 0x7, 0xA } };
        private static ushort[][] p_StateReducsOnTerminal_E = {  };
        private static ushort[] p_StateExpectedIDs_F = { 0x13, 0x4 };
        private static string[] p_StateExpectedNames_F = { "_T[(]", "IDENTIFIER" };
        private static string[] p_StateItems_F = { "[exp_op2 -> exp_op2 * . exp_op1]", "[exp_op2 -> exp_op2 * . exp_op1]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]" };
        private static ushort[][] p_StateShiftsOnTerminal_F = { new ushort[2] { 0x13, 0x22 }, new ushort[2] { 0x4, 0x23 } };
        private static ushort[][] p_StateShiftsOnVariable_F = { new ushort[2] { 0xa, 0x1E }, new ushort[2] { 0x8, 0x1F }, new ushort[2] { 0x9, 0x20 }, new ushort[2] { 0x7, 0x21 } };
        private static ushort[][] p_StateReducsOnTerminal_F = {  };
        private static ushort[] p_StateExpectedIDs_10 = { 0x14 };
        private static string[] p_StateExpectedNames_10 = { "_T[)]" };
        private static string[] p_StateItems_10 = { "[exp_op1_cast -> ( type . ) exp_op1]", "[exp_op1_cast -> ( type . ) exp_op1]", "[exp_op1_cast -> ( type . ) exp_op1]" };
        private static ushort[][] p_StateShiftsOnTerminal_10 = { new ushort[2] { 0x14, 0x24 } };
        private static ushort[][] p_StateShiftsOnVariable_10 = {  };
        private static ushort[][] p_StateReducsOnTerminal_10 = {  };
        private static ushort[] p_StateExpectedIDs_11 = { 0x14 };
        private static string[] p_StateExpectedNames_11 = { "_T[)]" };
        private static string[] p_StateItems_11 = { "[exp_atom -> ( expression . )]", "[exp_atom -> ( expression . )]", "[exp_atom -> ( expression . )]" };
        private static ushort[][] p_StateShiftsOnTerminal_11 = { new ushort[2] { 0x14, 0x25 } };
        private static ushort[][] p_StateShiftsOnVariable_11 = {  };
        private static ushort[][] p_StateReducsOnTerminal_11 = {  };
        private static ushort[] p_StateExpectedIDs_12 = { 0x14 };
        private static string[] p_StateExpectedNames_12 = { "_T[)]" };
        private static string[] p_StateItems_12 = { "[type -> qualified_name . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_12 = {  };
        private static ushort[][] p_StateShiftsOnVariable_12 = {  };
        private static ushort[][] p_StateReducsOnTerminal_12 = { new ushort[3] { 0x14, 0x1, 0x1 } };
        private static ushort[] p_StateExpectedIDs_13 = { 0x14 };
        private static string[] p_StateExpectedNames_13 = { "_T[)]" };
        private static string[] p_StateItems_13 = { "[expression -> exp_op12_conditional . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_13 = {  };
        private static ushort[][] p_StateShiftsOnVariable_13 = {  };
        private static ushort[][] p_StateReducsOnTerminal_13 = { new ushort[3] { 0x14, 0xD, 0x1 } };
        private static ushort[] p_StateExpectedIDs_14 = { 0x14 };
        private static string[] p_StateExpectedNames_14 = { "_T[)]" };
        private static string[] p_StateItems_14 = { "[expression -> exp_op13_assigment . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_14 = {  };
        private static ushort[][] p_StateShiftsOnVariable_14 = {  };
        private static ushort[][] p_StateReducsOnTerminal_14 = { new ushort[3] { 0x14, 0xE, 0x1 } };
        private static ushort[] p_StateExpectedIDs_15 = { 0x14, 0x18, 0x17, 0x11 };
        private static string[] p_StateExpectedNames_15 = { "_T[)]", "_T[=]", "_T[*]", "_T[.]" };
        private static string[] p_StateItems_15 = { "[qualified_name -> IDENTIFIER . _m18]", "[exp_atom -> IDENTIFIER . ]", "[exp_atom -> IDENTIFIER . ]", "[exp_atom -> IDENTIFIER . ]", "[_m18 -> . . IDENTIFIER _m18]", "[_m18 -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_15 = { new ushort[2] { 0x11, 0x27 } };
        private static ushort[][] p_StateShiftsOnVariable_15 = { new ushort[2] { 0x12, 0x26 } };
        private static ushort[][] p_StateReducsOnTerminal_15 = { new ushort[3] { 0x14, 0x0, 0x1 }, new ushort[3] { 0x18, 0x2, 0x1 }, new ushort[3] { 0x14, 0x2, 0x1 }, new ushort[3] { 0x17, 0x2, 0x1 }, new ushort[3] { 0x14, 0x11, 0x0 } };
        private static ushort[] p_StateExpectedIDs_16 = { 0x14 };
        private static string[] p_StateExpectedNames_16 = { "_T[)]" };
        private static string[] p_StateItems_16 = { "[exp_op12_conditional -> exp_op11 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_16 = {  };
        private static ushort[][] p_StateShiftsOnVariable_16 = {  };
        private static ushort[][] p_StateReducsOnTerminal_16 = { new ushort[3] { 0x14, 0xB, 0x1 } };
        private static ushort[] p_StateExpectedIDs_17 = { 0x14, 0x17, 0x18 };
        private static string[] p_StateExpectedNames_17 = { "_T[)]", "_T[*]", "_T[=]" };
        private static string[] p_StateItems_17 = { "[exp_op13_assigment -> exp_op1 . = expression]", "[exp_op2 -> exp_op1 . ]", "[exp_op2 -> exp_op1 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_17 = { new ushort[2] { 0x18, 0x28 } };
        private static ushort[][] p_StateShiftsOnVariable_17 = {  };
        private static ushort[][] p_StateReducsOnTerminal_17 = { new ushort[3] { 0x14, 0x8, 0x1 }, new ushort[3] { 0x17, 0x8, 0x1 } };
        private static ushort[] p_StateExpectedIDs_18 = { 0x14, 0x17 };
        private static string[] p_StateExpectedNames_18 = { "_T[)]", "_T[*]" };
        private static string[] p_StateItems_18 = { "[exp_op11 -> exp_op2 . ]", "[exp_op2 -> exp_op2 . * exp_op1]", "[exp_op2 -> exp_op2 . * exp_op1]" };
        private static ushort[][] p_StateShiftsOnTerminal_18 = { new ushort[2] { 0x17, 0x29 } };
        private static ushort[][] p_StateShiftsOnVariable_18 = {  };
        private static ushort[][] p_StateReducsOnTerminal_18 = { new ushort[3] { 0x14, 0xA, 0x1 } };
        private static ushort[] p_StateExpectedIDs_19 = { 0x18, 0x14, 0x17 };
        private static string[] p_StateExpectedNames_19 = { "_T[=]", "_T[)]", "_T[*]" };
        private static string[] p_StateItems_19 = { "[exp_op1 -> exp_op0 . ]", "[exp_op1 -> exp_op0 . ]", "[exp_op1 -> exp_op0 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_19 = {  };
        private static ushort[][] p_StateShiftsOnVariable_19 = {  };
        private static ushort[][] p_StateReducsOnTerminal_19 = { new ushort[3] { 0x18, 0x6, 0x1 }, new ushort[3] { 0x14, 0x6, 0x1 }, new ushort[3] { 0x17, 0x6, 0x1 } };
        private static ushort[] p_StateExpectedIDs_1A = { 0x18, 0x14, 0x17 };
        private static string[] p_StateExpectedNames_1A = { "_T[=]", "_T[)]", "_T[*]" };
        private static string[] p_StateItems_1A = { "[exp_op1 -> exp_op1_cast . ]", "[exp_op1 -> exp_op1_cast . ]", "[exp_op1 -> exp_op1_cast . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_1A = {  };
        private static ushort[][] p_StateShiftsOnVariable_1A = {  };
        private static ushort[][] p_StateReducsOnTerminal_1A = { new ushort[3] { 0x18, 0x7, 0x1 }, new ushort[3] { 0x14, 0x7, 0x1 }, new ushort[3] { 0x17, 0x7, 0x1 } };
        private static ushort[] p_StateExpectedIDs_1B = { 0x18, 0x14, 0x17 };
        private static string[] p_StateExpectedNames_1B = { "_T[=]", "_T[)]", "_T[*]" };
        private static string[] p_StateItems_1B = { "[exp_op0 -> exp_atom . ]", "[exp_op0 -> exp_atom . ]", "[exp_op0 -> exp_atom . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_1B = {  };
        private static ushort[][] p_StateShiftsOnVariable_1B = {  };
        private static ushort[][] p_StateReducsOnTerminal_1B = { new ushort[3] { 0x18, 0x4, 0x1 }, new ushort[3] { 0x14, 0x4, 0x1 }, new ushort[3] { 0x17, 0x4, 0x1 } };
        private static ushort[] p_StateExpectedIDs_1C = { 0x4, 0x13 };
        private static string[] p_StateExpectedNames_1C = { "IDENTIFIER", "_T[(]" };
        private static string[] p_StateItems_1C = { "[exp_op1_cast -> ( . type ) exp_op1]", "[exp_atom -> ( . expression )]", "[exp_op1_cast -> ( . type ) exp_op1]", "[exp_atom -> ( . expression )]", "[exp_op1_cast -> ( . type ) exp_op1]", "[exp_atom -> ( . expression )]", "[type -> . qualified_name]", "[expression -> . exp_op12_conditional]", "[expression -> . exp_op13_assigment]", "[qualified_name -> . IDENTIFIER _m18]", "[exp_op12_conditional -> . exp_op11]", "[exp_op13_assigment -> . exp_op1 = expression]", "[exp_op11 -> . exp_op2]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_op2 -> . exp_op1]", "[exp_op2 -> . exp_op2 * exp_op1]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_op2 -> . exp_op1]", "[exp_op2 -> . exp_op2 * exp_op1]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]" };
        private static ushort[][] p_StateShiftsOnTerminal_1C = { new ushort[2] { 0x4, 0x15 }, new ushort[2] { 0x13, 0x1C } };
        private static ushort[][] p_StateShiftsOnVariable_1C = { new ushort[2] { 0x6, 0x2A }, new ushort[2] { 0xf, 0x2B }, new ushort[2] { 0x5, 0x12 }, new ushort[2] { 0xd, 0x13 }, new ushort[2] { 0xe, 0x14 }, new ushort[2] { 0xc, 0x16 }, new ushort[2] { 0xa, 0x17 }, new ushort[2] { 0xb, 0x18 }, new ushort[2] { 0x8, 0x19 }, new ushort[2] { 0x9, 0x1A }, new ushort[2] { 0x7, 0x1B } };
        private static ushort[][] p_StateReducsOnTerminal_1C = {  };
        private static ushort[] p_StateExpectedIDs_1D = { 0x2 };
        private static string[] p_StateExpectedNames_1D = { "$" };
        private static string[] p_StateItems_1D = { "[exp_op13_assigment -> exp_op1 = expression . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_1D = {  };
        private static ushort[][] p_StateShiftsOnVariable_1D = {  };
        private static ushort[][] p_StateReducsOnTerminal_1D = { new ushort[3] { 0x2, 0xC, 0x3 } };
        private static ushort[] p_StateExpectedIDs_1E = { 0x2, 0x17 };
        private static string[] p_StateExpectedNames_1E = { "$", "_T[*]" };
        private static string[] p_StateItems_1E = { "[exp_op2 -> exp_op2 * exp_op1 . ]", "[exp_op2 -> exp_op2 * exp_op1 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_1E = {  };
        private static ushort[][] p_StateShiftsOnVariable_1E = {  };
        private static ushort[][] p_StateReducsOnTerminal_1E = { new ushort[3] { 0x2, 0x9, 0x3 }, new ushort[3] { 0x17, 0x9, 0x3 } };
        private static ushort[] p_StateExpectedIDs_1F = { 0x2, 0x17 };
        private static string[] p_StateExpectedNames_1F = { "$", "_T[*]" };
        private static string[] p_StateItems_1F = { "[exp_op1 -> exp_op0 . ]", "[exp_op1 -> exp_op0 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_1F = {  };
        private static ushort[][] p_StateShiftsOnVariable_1F = {  };
        private static ushort[][] p_StateReducsOnTerminal_1F = { new ushort[3] { 0x2, 0x6, 0x1 }, new ushort[3] { 0x17, 0x6, 0x1 } };
        private static ushort[] p_StateExpectedIDs_20 = { 0x2, 0x17 };
        private static string[] p_StateExpectedNames_20 = { "$", "_T[*]" };
        private static string[] p_StateItems_20 = { "[exp_op1 -> exp_op1_cast . ]", "[exp_op1 -> exp_op1_cast . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_20 = {  };
        private static ushort[][] p_StateShiftsOnVariable_20 = {  };
        private static ushort[][] p_StateReducsOnTerminal_20 = { new ushort[3] { 0x2, 0x7, 0x1 }, new ushort[3] { 0x17, 0x7, 0x1 } };
        private static ushort[] p_StateExpectedIDs_21 = { 0x2, 0x17 };
        private static string[] p_StateExpectedNames_21 = { "$", "_T[*]" };
        private static string[] p_StateItems_21 = { "[exp_op0 -> exp_atom . ]", "[exp_op0 -> exp_atom . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_21 = {  };
        private static ushort[][] p_StateShiftsOnVariable_21 = {  };
        private static ushort[][] p_StateReducsOnTerminal_21 = { new ushort[3] { 0x2, 0x4, 0x1 }, new ushort[3] { 0x17, 0x4, 0x1 } };
        private static ushort[] p_StateExpectedIDs_22 = { 0x4, 0x13 };
        private static string[] p_StateExpectedNames_22 = { "IDENTIFIER", "_T[(]" };
        private static string[] p_StateItems_22 = { "[exp_op1_cast -> ( . type ) exp_op1]", "[exp_op1_cast -> ( . type ) exp_op1]", "[exp_atom -> ( . expression )]", "[exp_atom -> ( . expression )]", "[type -> . qualified_name]", "[expression -> . exp_op12_conditional]", "[expression -> . exp_op13_assigment]", "[qualified_name -> . IDENTIFIER _m18]", "[exp_op12_conditional -> . exp_op11]", "[exp_op13_assigment -> . exp_op1 = expression]", "[exp_op11 -> . exp_op2]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_op2 -> . exp_op1]", "[exp_op2 -> . exp_op2 * exp_op1]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_op2 -> . exp_op1]", "[exp_op2 -> . exp_op2 * exp_op1]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]" };
        private static ushort[][] p_StateShiftsOnTerminal_22 = { new ushort[2] { 0x4, 0x15 }, new ushort[2] { 0x13, 0x1C } };
        private static ushort[][] p_StateShiftsOnVariable_22 = { new ushort[2] { 0x6, 0x2C }, new ushort[2] { 0xf, 0x2D }, new ushort[2] { 0x5, 0x12 }, new ushort[2] { 0xd, 0x13 }, new ushort[2] { 0xe, 0x14 }, new ushort[2] { 0xc, 0x16 }, new ushort[2] { 0xa, 0x17 }, new ushort[2] { 0xb, 0x18 }, new ushort[2] { 0x8, 0x19 }, new ushort[2] { 0x9, 0x1A }, new ushort[2] { 0x7, 0x1B } };
        private static ushort[][] p_StateReducsOnTerminal_22 = {  };
        private static ushort[] p_StateExpectedIDs_23 = { 0x2, 0x17 };
        private static string[] p_StateExpectedNames_23 = { "$", "_T[*]" };
        private static string[] p_StateItems_23 = { "[exp_atom -> IDENTIFIER . ]", "[exp_atom -> IDENTIFIER . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_23 = {  };
        private static ushort[][] p_StateShiftsOnVariable_23 = {  };
        private static ushort[][] p_StateReducsOnTerminal_23 = { new ushort[3] { 0x2, 0x2, 0x1 }, new ushort[3] { 0x17, 0x2, 0x1 } };
        private static ushort[] p_StateExpectedIDs_24 = { 0x13, 0x4 };
        private static string[] p_StateExpectedNames_24 = { "_T[(]", "IDENTIFIER" };
        private static string[] p_StateItems_24 = { "[exp_op1_cast -> ( type ) . exp_op1]", "[exp_op1_cast -> ( type ) . exp_op1]", "[exp_op1_cast -> ( type ) . exp_op1]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]" };
        private static ushort[][] p_StateShiftsOnTerminal_24 = { new ushort[2] { 0x13, 0xB }, new ushort[2] { 0x4, 0xC } };
        private static ushort[][] p_StateShiftsOnVariable_24 = { new ushort[2] { 0xa, 0x2E }, new ushort[2] { 0x8, 0x8 }, new ushort[2] { 0x9, 0x9 }, new ushort[2] { 0x7, 0xA } };
        private static ushort[][] p_StateReducsOnTerminal_24 = {  };
        private static ushort[] p_StateExpectedIDs_25 = { 0x18, 0x2, 0x17 };
        private static string[] p_StateExpectedNames_25 = { "_T[=]", "$", "_T[*]" };
        private static string[] p_StateItems_25 = { "[exp_atom -> ( expression ) . ]", "[exp_atom -> ( expression ) . ]", "[exp_atom -> ( expression ) . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_25 = {  };
        private static ushort[][] p_StateShiftsOnVariable_25 = {  };
        private static ushort[][] p_StateReducsOnTerminal_25 = { new ushort[3] { 0x18, 0x3, 0x3 }, new ushort[3] { 0x2, 0x3, 0x3 }, new ushort[3] { 0x17, 0x3, 0x3 } };
        private static ushort[] p_StateExpectedIDs_26 = { 0x14 };
        private static string[] p_StateExpectedNames_26 = { "_T[)]" };
        private static string[] p_StateItems_26 = { "[qualified_name -> IDENTIFIER _m18 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_26 = {  };
        private static ushort[][] p_StateShiftsOnVariable_26 = {  };
        private static ushort[][] p_StateReducsOnTerminal_26 = { new ushort[3] { 0x14, 0x0, 0x2 } };
        private static ushort[] p_StateExpectedIDs_27 = { 0x4 };
        private static string[] p_StateExpectedNames_27 = { "IDENTIFIER" };
        private static string[] p_StateItems_27 = { "[_m18 -> . . IDENTIFIER _m18]" };
        private static ushort[][] p_StateShiftsOnTerminal_27 = { new ushort[2] { 0x4, 0x2F } };
        private static ushort[][] p_StateShiftsOnVariable_27 = {  };
        private static ushort[][] p_StateReducsOnTerminal_27 = {  };
        private static ushort[] p_StateExpectedIDs_28 = { 0x13, 0x4 };
        private static string[] p_StateExpectedNames_28 = { "_T[(]", "IDENTIFIER" };
        private static string[] p_StateItems_28 = { "[exp_op13_assigment -> exp_op1 = . expression]", "[expression -> . exp_op12_conditional]", "[expression -> . exp_op13_assigment]", "[exp_op12_conditional -> . exp_op11]", "[exp_op13_assigment -> . exp_op1 = expression]", "[exp_op11 -> . exp_op2]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_op2 -> . exp_op1]", "[exp_op2 -> . exp_op2 * exp_op1]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_op2 -> . exp_op1]", "[exp_op2 -> . exp_op2 * exp_op1]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]" };
        private static ushort[][] p_StateShiftsOnTerminal_28 = { new ushort[2] { 0x13, 0x1C }, new ushort[2] { 0x4, 0x31 } };
        private static ushort[][] p_StateShiftsOnVariable_28 = { new ushort[2] { 0xf, 0x30 }, new ushort[2] { 0xd, 0x13 }, new ushort[2] { 0xe, 0x14 }, new ushort[2] { 0xc, 0x16 }, new ushort[2] { 0xa, 0x17 }, new ushort[2] { 0xb, 0x18 }, new ushort[2] { 0x8, 0x19 }, new ushort[2] { 0x9, 0x1A }, new ushort[2] { 0x7, 0x1B } };
        private static ushort[][] p_StateReducsOnTerminal_28 = {  };
        private static ushort[] p_StateExpectedIDs_29 = { 0x13, 0x4 };
        private static string[] p_StateExpectedNames_29 = { "_T[(]", "IDENTIFIER" };
        private static string[] p_StateItems_29 = { "[exp_op2 -> exp_op2 * . exp_op1]", "[exp_op2 -> exp_op2 * . exp_op1]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]" };
        private static ushort[][] p_StateShiftsOnTerminal_29 = { new ushort[2] { 0x13, 0x36 }, new ushort[2] { 0x4, 0x37 } };
        private static ushort[][] p_StateShiftsOnVariable_29 = { new ushort[2] { 0xa, 0x32 }, new ushort[2] { 0x8, 0x33 }, new ushort[2] { 0x9, 0x34 }, new ushort[2] { 0x7, 0x35 } };
        private static ushort[][] p_StateReducsOnTerminal_29 = {  };
        private static ushort[] p_StateExpectedIDs_2A = { 0x14 };
        private static string[] p_StateExpectedNames_2A = { "_T[)]" };
        private static string[] p_StateItems_2A = { "[exp_op1_cast -> ( type . ) exp_op1]", "[exp_op1_cast -> ( type . ) exp_op1]", "[exp_op1_cast -> ( type . ) exp_op1]" };
        private static ushort[][] p_StateShiftsOnTerminal_2A = { new ushort[2] { 0x14, 0x38 } };
        private static ushort[][] p_StateShiftsOnVariable_2A = {  };
        private static ushort[][] p_StateReducsOnTerminal_2A = {  };
        private static ushort[] p_StateExpectedIDs_2B = { 0x14 };
        private static string[] p_StateExpectedNames_2B = { "_T[)]" };
        private static string[] p_StateItems_2B = { "[exp_atom -> ( expression . )]", "[exp_atom -> ( expression . )]", "[exp_atom -> ( expression . )]" };
        private static ushort[][] p_StateShiftsOnTerminal_2B = { new ushort[2] { 0x14, 0x39 } };
        private static ushort[][] p_StateShiftsOnVariable_2B = {  };
        private static ushort[][] p_StateReducsOnTerminal_2B = {  };
        private static ushort[] p_StateExpectedIDs_2C = { 0x14 };
        private static string[] p_StateExpectedNames_2C = { "_T[)]" };
        private static string[] p_StateItems_2C = { "[exp_op1_cast -> ( type . ) exp_op1]", "[exp_op1_cast -> ( type . ) exp_op1]" };
        private static ushort[][] p_StateShiftsOnTerminal_2C = { new ushort[2] { 0x14, 0x3A } };
        private static ushort[][] p_StateShiftsOnVariable_2C = {  };
        private static ushort[][] p_StateReducsOnTerminal_2C = {  };
        private static ushort[] p_StateExpectedIDs_2D = { 0x14 };
        private static string[] p_StateExpectedNames_2D = { "_T[)]" };
        private static string[] p_StateItems_2D = { "[exp_atom -> ( expression . )]", "[exp_atom -> ( expression . )]" };
        private static ushort[][] p_StateShiftsOnTerminal_2D = { new ushort[2] { 0x14, 0x3B } };
        private static ushort[][] p_StateShiftsOnVariable_2D = {  };
        private static ushort[][] p_StateReducsOnTerminal_2D = {  };
        private static ushort[] p_StateExpectedIDs_2E = { 0x18, 0x2, 0x17 };
        private static string[] p_StateExpectedNames_2E = { "_T[=]", "$", "_T[*]" };
        private static string[] p_StateItems_2E = { "[exp_op1_cast -> ( type ) exp_op1 . ]", "[exp_op1_cast -> ( type ) exp_op1 . ]", "[exp_op1_cast -> ( type ) exp_op1 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_2E = {  };
        private static ushort[][] p_StateShiftsOnVariable_2E = {  };
        private static ushort[][] p_StateReducsOnTerminal_2E = { new ushort[3] { 0x18, 0x5, 0x4 }, new ushort[3] { 0x2, 0x5, 0x4 }, new ushort[3] { 0x17, 0x5, 0x4 } };
        private static ushort[] p_StateExpectedIDs_2F = { 0x14, 0x11 };
        private static string[] p_StateExpectedNames_2F = { "_T[)]", "_T[.]" };
        private static string[] p_StateItems_2F = { "[_m18 -> . IDENTIFIER . _m18]", "[_m18 -> . . IDENTIFIER _m18]", "[_m18 -> . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_2F = { new ushort[2] { 0x11, 0x27 } };
        private static ushort[][] p_StateShiftsOnVariable_2F = { new ushort[2] { 0x12, 0x3C } };
        private static ushort[][] p_StateReducsOnTerminal_2F = { new ushort[3] { 0x14, 0x10, 0x2 }, new ushort[3] { 0x14, 0x11, 0x0 } };
        private static ushort[] p_StateExpectedIDs_30 = { 0x14 };
        private static string[] p_StateExpectedNames_30 = { "_T[)]" };
        private static string[] p_StateItems_30 = { "[exp_op13_assigment -> exp_op1 = expression . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_30 = {  };
        private static ushort[][] p_StateShiftsOnVariable_30 = {  };
        private static ushort[][] p_StateReducsOnTerminal_30 = { new ushort[3] { 0x14, 0xC, 0x3 } };
        private static ushort[] p_StateExpectedIDs_31 = { 0x18, 0x14, 0x17 };
        private static string[] p_StateExpectedNames_31 = { "_T[=]", "_T[)]", "_T[*]" };
        private static string[] p_StateItems_31 = { "[exp_atom -> IDENTIFIER . ]", "[exp_atom -> IDENTIFIER . ]", "[exp_atom -> IDENTIFIER . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_31 = {  };
        private static ushort[][] p_StateShiftsOnVariable_31 = {  };
        private static ushort[][] p_StateReducsOnTerminal_31 = { new ushort[3] { 0x18, 0x2, 0x1 }, new ushort[3] { 0x14, 0x2, 0x1 }, new ushort[3] { 0x17, 0x2, 0x1 } };
        private static ushort[] p_StateExpectedIDs_32 = { 0x14, 0x17 };
        private static string[] p_StateExpectedNames_32 = { "_T[)]", "_T[*]" };
        private static string[] p_StateItems_32 = { "[exp_op2 -> exp_op2 * exp_op1 . ]", "[exp_op2 -> exp_op2 * exp_op1 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_32 = {  };
        private static ushort[][] p_StateShiftsOnVariable_32 = {  };
        private static ushort[][] p_StateReducsOnTerminal_32 = { new ushort[3] { 0x14, 0x9, 0x3 }, new ushort[3] { 0x17, 0x9, 0x3 } };
        private static ushort[] p_StateExpectedIDs_33 = { 0x14, 0x17 };
        private static string[] p_StateExpectedNames_33 = { "_T[)]", "_T[*]" };
        private static string[] p_StateItems_33 = { "[exp_op1 -> exp_op0 . ]", "[exp_op1 -> exp_op0 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_33 = {  };
        private static ushort[][] p_StateShiftsOnVariable_33 = {  };
        private static ushort[][] p_StateReducsOnTerminal_33 = { new ushort[3] { 0x14, 0x6, 0x1 }, new ushort[3] { 0x17, 0x6, 0x1 } };
        private static ushort[] p_StateExpectedIDs_34 = { 0x14, 0x17 };
        private static string[] p_StateExpectedNames_34 = { "_T[)]", "_T[*]" };
        private static string[] p_StateItems_34 = { "[exp_op1 -> exp_op1_cast . ]", "[exp_op1 -> exp_op1_cast . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_34 = {  };
        private static ushort[][] p_StateShiftsOnVariable_34 = {  };
        private static ushort[][] p_StateReducsOnTerminal_34 = { new ushort[3] { 0x14, 0x7, 0x1 }, new ushort[3] { 0x17, 0x7, 0x1 } };
        private static ushort[] p_StateExpectedIDs_35 = { 0x14, 0x17 };
        private static string[] p_StateExpectedNames_35 = { "_T[)]", "_T[*]" };
        private static string[] p_StateItems_35 = { "[exp_op0 -> exp_atom . ]", "[exp_op0 -> exp_atom . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_35 = {  };
        private static ushort[][] p_StateShiftsOnVariable_35 = {  };
        private static ushort[][] p_StateReducsOnTerminal_35 = { new ushort[3] { 0x14, 0x4, 0x1 }, new ushort[3] { 0x17, 0x4, 0x1 } };
        private static ushort[] p_StateExpectedIDs_36 = { 0x4, 0x13 };
        private static string[] p_StateExpectedNames_36 = { "IDENTIFIER", "_T[(]" };
        private static string[] p_StateItems_36 = { "[exp_op1_cast -> ( . type ) exp_op1]", "[exp_op1_cast -> ( . type ) exp_op1]", "[exp_atom -> ( . expression )]", "[exp_atom -> ( . expression )]", "[type -> . qualified_name]", "[expression -> . exp_op12_conditional]", "[expression -> . exp_op13_assigment]", "[qualified_name -> . IDENTIFIER _m18]", "[exp_op12_conditional -> . exp_op11]", "[exp_op13_assigment -> . exp_op1 = expression]", "[exp_op11 -> . exp_op2]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_op2 -> . exp_op1]", "[exp_op2 -> . exp_op2 * exp_op1]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_op2 -> . exp_op1]", "[exp_op2 -> . exp_op2 * exp_op1]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]" };
        private static ushort[][] p_StateShiftsOnTerminal_36 = { new ushort[2] { 0x4, 0x15 }, new ushort[2] { 0x13, 0x1C } };
        private static ushort[][] p_StateShiftsOnVariable_36 = { new ushort[2] { 0x6, 0x3D }, new ushort[2] { 0xf, 0x3E }, new ushort[2] { 0x5, 0x12 }, new ushort[2] { 0xd, 0x13 }, new ushort[2] { 0xe, 0x14 }, new ushort[2] { 0xc, 0x16 }, new ushort[2] { 0xa, 0x17 }, new ushort[2] { 0xb, 0x18 }, new ushort[2] { 0x8, 0x19 }, new ushort[2] { 0x9, 0x1A }, new ushort[2] { 0x7, 0x1B } };
        private static ushort[][] p_StateReducsOnTerminal_36 = {  };
        private static ushort[] p_StateExpectedIDs_37 = { 0x14, 0x17 };
        private static string[] p_StateExpectedNames_37 = { "_T[)]", "_T[*]" };
        private static string[] p_StateItems_37 = { "[exp_atom -> IDENTIFIER . ]", "[exp_atom -> IDENTIFIER . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_37 = {  };
        private static ushort[][] p_StateShiftsOnVariable_37 = {  };
        private static ushort[][] p_StateReducsOnTerminal_37 = { new ushort[3] { 0x14, 0x2, 0x1 }, new ushort[3] { 0x17, 0x2, 0x1 } };
        private static ushort[] p_StateExpectedIDs_38 = { 0x13, 0x4 };
        private static string[] p_StateExpectedNames_38 = { "_T[(]", "IDENTIFIER" };
        private static string[] p_StateItems_38 = { "[exp_op1_cast -> ( type ) . exp_op1]", "[exp_op1_cast -> ( type ) . exp_op1]", "[exp_op1_cast -> ( type ) . exp_op1]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]" };
        private static ushort[][] p_StateShiftsOnTerminal_38 = { new ushort[2] { 0x13, 0x1C }, new ushort[2] { 0x4, 0x31 } };
        private static ushort[][] p_StateShiftsOnVariable_38 = { new ushort[2] { 0xa, 0x3F }, new ushort[2] { 0x8, 0x19 }, new ushort[2] { 0x9, 0x1A }, new ushort[2] { 0x7, 0x1B } };
        private static ushort[][] p_StateReducsOnTerminal_38 = {  };
        private static ushort[] p_StateExpectedIDs_39 = { 0x18, 0x14, 0x17 };
        private static string[] p_StateExpectedNames_39 = { "_T[=]", "_T[)]", "_T[*]" };
        private static string[] p_StateItems_39 = { "[exp_atom -> ( expression ) . ]", "[exp_atom -> ( expression ) . ]", "[exp_atom -> ( expression ) . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_39 = {  };
        private static ushort[][] p_StateShiftsOnVariable_39 = {  };
        private static ushort[][] p_StateReducsOnTerminal_39 = { new ushort[3] { 0x18, 0x3, 0x3 }, new ushort[3] { 0x14, 0x3, 0x3 }, new ushort[3] { 0x17, 0x3, 0x3 } };
        private static ushort[] p_StateExpectedIDs_3A = { 0x13, 0x4 };
        private static string[] p_StateExpectedNames_3A = { "_T[(]", "IDENTIFIER" };
        private static string[] p_StateItems_3A = { "[exp_op1_cast -> ( type ) . exp_op1]", "[exp_op1_cast -> ( type ) . exp_op1]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]" };
        private static ushort[][] p_StateShiftsOnTerminal_3A = { new ushort[2] { 0x13, 0x22 }, new ushort[2] { 0x4, 0x23 } };
        private static ushort[][] p_StateShiftsOnVariable_3A = { new ushort[2] { 0xa, 0x40 }, new ushort[2] { 0x8, 0x1F }, new ushort[2] { 0x9, 0x20 }, new ushort[2] { 0x7, 0x21 } };
        private static ushort[][] p_StateReducsOnTerminal_3A = {  };
        private static ushort[] p_StateExpectedIDs_3B = { 0x2, 0x17 };
        private static string[] p_StateExpectedNames_3B = { "$", "_T[*]" };
        private static string[] p_StateItems_3B = { "[exp_atom -> ( expression ) . ]", "[exp_atom -> ( expression ) . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_3B = {  };
        private static ushort[][] p_StateShiftsOnVariable_3B = {  };
        private static ushort[][] p_StateReducsOnTerminal_3B = { new ushort[3] { 0x2, 0x3, 0x3 }, new ushort[3] { 0x17, 0x3, 0x3 } };
        private static ushort[] p_StateExpectedIDs_3C = { 0x14 };
        private static string[] p_StateExpectedNames_3C = { "_T[)]" };
        private static string[] p_StateItems_3C = { "[_m18 -> . IDENTIFIER _m18 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_3C = {  };
        private static ushort[][] p_StateShiftsOnVariable_3C = {  };
        private static ushort[][] p_StateReducsOnTerminal_3C = { new ushort[3] { 0x14, 0x10, 0x3 } };
        private static ushort[] p_StateExpectedIDs_3D = { 0x14 };
        private static string[] p_StateExpectedNames_3D = { "_T[)]" };
        private static string[] p_StateItems_3D = { "[exp_op1_cast -> ( type . ) exp_op1]", "[exp_op1_cast -> ( type . ) exp_op1]" };
        private static ushort[][] p_StateShiftsOnTerminal_3D = { new ushort[2] { 0x14, 0x41 } };
        private static ushort[][] p_StateShiftsOnVariable_3D = {  };
        private static ushort[][] p_StateReducsOnTerminal_3D = {  };
        private static ushort[] p_StateExpectedIDs_3E = { 0x14 };
        private static string[] p_StateExpectedNames_3E = { "_T[)]" };
        private static string[] p_StateItems_3E = { "[exp_atom -> ( expression . )]", "[exp_atom -> ( expression . )]" };
        private static ushort[][] p_StateShiftsOnTerminal_3E = { new ushort[2] { 0x14, 0x42 } };
        private static ushort[][] p_StateShiftsOnVariable_3E = {  };
        private static ushort[][] p_StateReducsOnTerminal_3E = {  };
        private static ushort[] p_StateExpectedIDs_3F = { 0x18, 0x14, 0x17 };
        private static string[] p_StateExpectedNames_3F = { "_T[=]", "_T[)]", "_T[*]" };
        private static string[] p_StateItems_3F = { "[exp_op1_cast -> ( type ) exp_op1 . ]", "[exp_op1_cast -> ( type ) exp_op1 . ]", "[exp_op1_cast -> ( type ) exp_op1 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_3F = {  };
        private static ushort[][] p_StateShiftsOnVariable_3F = {  };
        private static ushort[][] p_StateReducsOnTerminal_3F = { new ushort[3] { 0x18, 0x5, 0x4 }, new ushort[3] { 0x14, 0x5, 0x4 }, new ushort[3] { 0x17, 0x5, 0x4 } };
        private static ushort[] p_StateExpectedIDs_40 = { 0x2, 0x17 };
        private static string[] p_StateExpectedNames_40 = { "$", "_T[*]" };
        private static string[] p_StateItems_40 = { "[exp_op1_cast -> ( type ) exp_op1 . ]", "[exp_op1_cast -> ( type ) exp_op1 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_40 = {  };
        private static ushort[][] p_StateShiftsOnVariable_40 = {  };
        private static ushort[][] p_StateReducsOnTerminal_40 = { new ushort[3] { 0x2, 0x5, 0x4 }, new ushort[3] { 0x17, 0x5, 0x4 } };
        private static ushort[] p_StateExpectedIDs_41 = { 0x13, 0x4 };
        private static string[] p_StateExpectedNames_41 = { "_T[(]", "IDENTIFIER" };
        private static string[] p_StateItems_41 = { "[exp_op1_cast -> ( type ) . exp_op1]", "[exp_op1_cast -> ( type ) . exp_op1]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1_cast]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_op0 -> . exp_atom]", "[exp_op1_cast -> . ( type ) exp_op1]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]", "[exp_atom -> . IDENTIFIER]", "[exp_atom -> . ( expression )]" };
        private static ushort[][] p_StateShiftsOnTerminal_41 = { new ushort[2] { 0x13, 0x36 }, new ushort[2] { 0x4, 0x37 } };
        private static ushort[][] p_StateShiftsOnVariable_41 = { new ushort[2] { 0xa, 0x43 }, new ushort[2] { 0x8, 0x33 }, new ushort[2] { 0x9, 0x34 }, new ushort[2] { 0x7, 0x35 } };
        private static ushort[][] p_StateReducsOnTerminal_41 = {  };
        private static ushort[] p_StateExpectedIDs_42 = { 0x14, 0x17 };
        private static string[] p_StateExpectedNames_42 = { "_T[)]", "_T[*]" };
        private static string[] p_StateItems_42 = { "[exp_atom -> ( expression ) . ]", "[exp_atom -> ( expression ) . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_42 = {  };
        private static ushort[][] p_StateShiftsOnVariable_42 = {  };
        private static ushort[][] p_StateReducsOnTerminal_42 = { new ushort[3] { 0x14, 0x3, 0x3 }, new ushort[3] { 0x17, 0x3, 0x3 } };
        private static ushort[] p_StateExpectedIDs_43 = { 0x14, 0x17 };
        private static string[] p_StateExpectedNames_43 = { "_T[)]", "_T[*]" };
        private static string[] p_StateItems_43 = { "[exp_op1_cast -> ( type ) exp_op1 . ]", "[exp_op1_cast -> ( type ) exp_op1 . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_43 = {  };
        private static ushort[][] p_StateShiftsOnVariable_43 = {  };
        private static ushort[][] p_StateReducsOnTerminal_43 = { new ushort[3] { 0x14, 0x5, 0x4 }, new ushort[3] { 0x17, 0x5, 0x4 } };
        private static ushort[][] p_StaticStateExpectedIDs = { p_StateExpectedIDs_0, p_StateExpectedIDs_1, p_StateExpectedIDs_2, p_StateExpectedIDs_3, p_StateExpectedIDs_4, p_StateExpectedIDs_5, p_StateExpectedIDs_6, p_StateExpectedIDs_7, p_StateExpectedIDs_8, p_StateExpectedIDs_9, p_StateExpectedIDs_A, p_StateExpectedIDs_B, p_StateExpectedIDs_C, p_StateExpectedIDs_D, p_StateExpectedIDs_E, p_StateExpectedIDs_F, p_StateExpectedIDs_10, p_StateExpectedIDs_11, p_StateExpectedIDs_12, p_StateExpectedIDs_13, p_StateExpectedIDs_14, p_StateExpectedIDs_15, p_StateExpectedIDs_16, p_StateExpectedIDs_17, p_StateExpectedIDs_18, p_StateExpectedIDs_19, p_StateExpectedIDs_1A, p_StateExpectedIDs_1B, p_StateExpectedIDs_1C, p_StateExpectedIDs_1D, p_StateExpectedIDs_1E, p_StateExpectedIDs_1F, p_StateExpectedIDs_20, p_StateExpectedIDs_21, p_StateExpectedIDs_22, p_StateExpectedIDs_23, p_StateExpectedIDs_24, p_StateExpectedIDs_25, p_StateExpectedIDs_26, p_StateExpectedIDs_27, p_StateExpectedIDs_28, p_StateExpectedIDs_29, p_StateExpectedIDs_2A, p_StateExpectedIDs_2B, p_StateExpectedIDs_2C, p_StateExpectedIDs_2D, p_StateExpectedIDs_2E, p_StateExpectedIDs_2F, p_StateExpectedIDs_30, p_StateExpectedIDs_31, p_StateExpectedIDs_32, p_StateExpectedIDs_33, p_StateExpectedIDs_34, p_StateExpectedIDs_35, p_StateExpectedIDs_36, p_StateExpectedIDs_37, p_StateExpectedIDs_38, p_StateExpectedIDs_39, p_StateExpectedIDs_3A, p_StateExpectedIDs_3B, p_StateExpectedIDs_3C, p_StateExpectedIDs_3D, p_StateExpectedIDs_3E, p_StateExpectedIDs_3F, p_StateExpectedIDs_40, p_StateExpectedIDs_41, p_StateExpectedIDs_42, p_StateExpectedIDs_43 };
        private static string[][] p_StaticStateExpectedNames = { p_StateExpectedNames_0, p_StateExpectedNames_1, p_StateExpectedNames_2, p_StateExpectedNames_3, p_StateExpectedNames_4, p_StateExpectedNames_5, p_StateExpectedNames_6, p_StateExpectedNames_7, p_StateExpectedNames_8, p_StateExpectedNames_9, p_StateExpectedNames_A, p_StateExpectedNames_B, p_StateExpectedNames_C, p_StateExpectedNames_D, p_StateExpectedNames_E, p_StateExpectedNames_F, p_StateExpectedNames_10, p_StateExpectedNames_11, p_StateExpectedNames_12, p_StateExpectedNames_13, p_StateExpectedNames_14, p_StateExpectedNames_15, p_StateExpectedNames_16, p_StateExpectedNames_17, p_StateExpectedNames_18, p_StateExpectedNames_19, p_StateExpectedNames_1A, p_StateExpectedNames_1B, p_StateExpectedNames_1C, p_StateExpectedNames_1D, p_StateExpectedNames_1E, p_StateExpectedNames_1F, p_StateExpectedNames_20, p_StateExpectedNames_21, p_StateExpectedNames_22, p_StateExpectedNames_23, p_StateExpectedNames_24, p_StateExpectedNames_25, p_StateExpectedNames_26, p_StateExpectedNames_27, p_StateExpectedNames_28, p_StateExpectedNames_29, p_StateExpectedNames_2A, p_StateExpectedNames_2B, p_StateExpectedNames_2C, p_StateExpectedNames_2D, p_StateExpectedNames_2E, p_StateExpectedNames_2F, p_StateExpectedNames_30, p_StateExpectedNames_31, p_StateExpectedNames_32, p_StateExpectedNames_33, p_StateExpectedNames_34, p_StateExpectedNames_35, p_StateExpectedNames_36, p_StateExpectedNames_37, p_StateExpectedNames_38, p_StateExpectedNames_39, p_StateExpectedNames_3A, p_StateExpectedNames_3B, p_StateExpectedNames_3C, p_StateExpectedNames_3D, p_StateExpectedNames_3E, p_StateExpectedNames_3F, p_StateExpectedNames_40, p_StateExpectedNames_41, p_StateExpectedNames_42, p_StateExpectedNames_43 };
        private static string[][] p_StaticStateItems = { p_StateItems_0, p_StateItems_1, p_StateItems_2, p_StateItems_3, p_StateItems_4, p_StateItems_5, p_StateItems_6, p_StateItems_7, p_StateItems_8, p_StateItems_9, p_StateItems_A, p_StateItems_B, p_StateItems_C, p_StateItems_D, p_StateItems_E, p_StateItems_F, p_StateItems_10, p_StateItems_11, p_StateItems_12, p_StateItems_13, p_StateItems_14, p_StateItems_15, p_StateItems_16, p_StateItems_17, p_StateItems_18, p_StateItems_19, p_StateItems_1A, p_StateItems_1B, p_StateItems_1C, p_StateItems_1D, p_StateItems_1E, p_StateItems_1F, p_StateItems_20, p_StateItems_21, p_StateItems_22, p_StateItems_23, p_StateItems_24, p_StateItems_25, p_StateItems_26, p_StateItems_27, p_StateItems_28, p_StateItems_29, p_StateItems_2A, p_StateItems_2B, p_StateItems_2C, p_StateItems_2D, p_StateItems_2E, p_StateItems_2F, p_StateItems_30, p_StateItems_31, p_StateItems_32, p_StateItems_33, p_StateItems_34, p_StateItems_35, p_StateItems_36, p_StateItems_37, p_StateItems_38, p_StateItems_39, p_StateItems_3A, p_StateItems_3B, p_StateItems_3C, p_StateItems_3D, p_StateItems_3E, p_StateItems_3F, p_StateItems_40, p_StateItems_41, p_StateItems_42, p_StateItems_43 };
        private static ushort[][][] p_StaticStateShiftsOnTerminal = { p_StateShiftsOnTerminal_0, p_StateShiftsOnTerminal_1, p_StateShiftsOnTerminal_2, p_StateShiftsOnTerminal_3, p_StateShiftsOnTerminal_4, p_StateShiftsOnTerminal_5, p_StateShiftsOnTerminal_6, p_StateShiftsOnTerminal_7, p_StateShiftsOnTerminal_8, p_StateShiftsOnTerminal_9, p_StateShiftsOnTerminal_A, p_StateShiftsOnTerminal_B, p_StateShiftsOnTerminal_C, p_StateShiftsOnTerminal_D, p_StateShiftsOnTerminal_E, p_StateShiftsOnTerminal_F, p_StateShiftsOnTerminal_10, p_StateShiftsOnTerminal_11, p_StateShiftsOnTerminal_12, p_StateShiftsOnTerminal_13, p_StateShiftsOnTerminal_14, p_StateShiftsOnTerminal_15, p_StateShiftsOnTerminal_16, p_StateShiftsOnTerminal_17, p_StateShiftsOnTerminal_18, p_StateShiftsOnTerminal_19, p_StateShiftsOnTerminal_1A, p_StateShiftsOnTerminal_1B, p_StateShiftsOnTerminal_1C, p_StateShiftsOnTerminal_1D, p_StateShiftsOnTerminal_1E, p_StateShiftsOnTerminal_1F, p_StateShiftsOnTerminal_20, p_StateShiftsOnTerminal_21, p_StateShiftsOnTerminal_22, p_StateShiftsOnTerminal_23, p_StateShiftsOnTerminal_24, p_StateShiftsOnTerminal_25, p_StateShiftsOnTerminal_26, p_StateShiftsOnTerminal_27, p_StateShiftsOnTerminal_28, p_StateShiftsOnTerminal_29, p_StateShiftsOnTerminal_2A, p_StateShiftsOnTerminal_2B, p_StateShiftsOnTerminal_2C, p_StateShiftsOnTerminal_2D, p_StateShiftsOnTerminal_2E, p_StateShiftsOnTerminal_2F, p_StateShiftsOnTerminal_30, p_StateShiftsOnTerminal_31, p_StateShiftsOnTerminal_32, p_StateShiftsOnTerminal_33, p_StateShiftsOnTerminal_34, p_StateShiftsOnTerminal_35, p_StateShiftsOnTerminal_36, p_StateShiftsOnTerminal_37, p_StateShiftsOnTerminal_38, p_StateShiftsOnTerminal_39, p_StateShiftsOnTerminal_3A, p_StateShiftsOnTerminal_3B, p_StateShiftsOnTerminal_3C, p_StateShiftsOnTerminal_3D, p_StateShiftsOnTerminal_3E, p_StateShiftsOnTerminal_3F, p_StateShiftsOnTerminal_40, p_StateShiftsOnTerminal_41, p_StateShiftsOnTerminal_42, p_StateShiftsOnTerminal_43 };
        private static ushort[][][] p_StaticStateShiftsOnVariable = { p_StateShiftsOnVariable_0, p_StateShiftsOnVariable_1, p_StateShiftsOnVariable_2, p_StateShiftsOnVariable_3, p_StateShiftsOnVariable_4, p_StateShiftsOnVariable_5, p_StateShiftsOnVariable_6, p_StateShiftsOnVariable_7, p_StateShiftsOnVariable_8, p_StateShiftsOnVariable_9, p_StateShiftsOnVariable_A, p_StateShiftsOnVariable_B, p_StateShiftsOnVariable_C, p_StateShiftsOnVariable_D, p_StateShiftsOnVariable_E, p_StateShiftsOnVariable_F, p_StateShiftsOnVariable_10, p_StateShiftsOnVariable_11, p_StateShiftsOnVariable_12, p_StateShiftsOnVariable_13, p_StateShiftsOnVariable_14, p_StateShiftsOnVariable_15, p_StateShiftsOnVariable_16, p_StateShiftsOnVariable_17, p_StateShiftsOnVariable_18, p_StateShiftsOnVariable_19, p_StateShiftsOnVariable_1A, p_StateShiftsOnVariable_1B, p_StateShiftsOnVariable_1C, p_StateShiftsOnVariable_1D, p_StateShiftsOnVariable_1E, p_StateShiftsOnVariable_1F, p_StateShiftsOnVariable_20, p_StateShiftsOnVariable_21, p_StateShiftsOnVariable_22, p_StateShiftsOnVariable_23, p_StateShiftsOnVariable_24, p_StateShiftsOnVariable_25, p_StateShiftsOnVariable_26, p_StateShiftsOnVariable_27, p_StateShiftsOnVariable_28, p_StateShiftsOnVariable_29, p_StateShiftsOnVariable_2A, p_StateShiftsOnVariable_2B, p_StateShiftsOnVariable_2C, p_StateShiftsOnVariable_2D, p_StateShiftsOnVariable_2E, p_StateShiftsOnVariable_2F, p_StateShiftsOnVariable_30, p_StateShiftsOnVariable_31, p_StateShiftsOnVariable_32, p_StateShiftsOnVariable_33, p_StateShiftsOnVariable_34, p_StateShiftsOnVariable_35, p_StateShiftsOnVariable_36, p_StateShiftsOnVariable_37, p_StateShiftsOnVariable_38, p_StateShiftsOnVariable_39, p_StateShiftsOnVariable_3A, p_StateShiftsOnVariable_3B, p_StateShiftsOnVariable_3C, p_StateShiftsOnVariable_3D, p_StateShiftsOnVariable_3E, p_StateShiftsOnVariable_3F, p_StateShiftsOnVariable_40, p_StateShiftsOnVariable_41, p_StateShiftsOnVariable_42, p_StateShiftsOnVariable_43 };
        private static ushort[][][] p_StaticStateReducsOnTerminal = { p_StateReducsOnTerminal_0, p_StateReducsOnTerminal_1, p_StateReducsOnTerminal_2, p_StateReducsOnTerminal_3, p_StateReducsOnTerminal_4, p_StateReducsOnTerminal_5, p_StateReducsOnTerminal_6, p_StateReducsOnTerminal_7, p_StateReducsOnTerminal_8, p_StateReducsOnTerminal_9, p_StateReducsOnTerminal_A, p_StateReducsOnTerminal_B, p_StateReducsOnTerminal_C, p_StateReducsOnTerminal_D, p_StateReducsOnTerminal_E, p_StateReducsOnTerminal_F, p_StateReducsOnTerminal_10, p_StateReducsOnTerminal_11, p_StateReducsOnTerminal_12, p_StateReducsOnTerminal_13, p_StateReducsOnTerminal_14, p_StateReducsOnTerminal_15, p_StateReducsOnTerminal_16, p_StateReducsOnTerminal_17, p_StateReducsOnTerminal_18, p_StateReducsOnTerminal_19, p_StateReducsOnTerminal_1A, p_StateReducsOnTerminal_1B, p_StateReducsOnTerminal_1C, p_StateReducsOnTerminal_1D, p_StateReducsOnTerminal_1E, p_StateReducsOnTerminal_1F, p_StateReducsOnTerminal_20, p_StateReducsOnTerminal_21, p_StateReducsOnTerminal_22, p_StateReducsOnTerminal_23, p_StateReducsOnTerminal_24, p_StateReducsOnTerminal_25, p_StateReducsOnTerminal_26, p_StateReducsOnTerminal_27, p_StateReducsOnTerminal_28, p_StateReducsOnTerminal_29, p_StateReducsOnTerminal_2A, p_StateReducsOnTerminal_2B, p_StateReducsOnTerminal_2C, p_StateReducsOnTerminal_2D, p_StateReducsOnTerminal_2E, p_StateReducsOnTerminal_2F, p_StateReducsOnTerminal_30, p_StateReducsOnTerminal_31, p_StateReducsOnTerminal_32, p_StateReducsOnTerminal_33, p_StateReducsOnTerminal_34, p_StateReducsOnTerminal_35, p_StateReducsOnTerminal_36, p_StateReducsOnTerminal_37, p_StateReducsOnTerminal_38, p_StateReducsOnTerminal_39, p_StateReducsOnTerminal_3A, p_StateReducsOnTerminal_3B, p_StateReducsOnTerminal_3C, p_StateReducsOnTerminal_3D, p_StateReducsOnTerminal_3E, p_StateReducsOnTerminal_3F, p_StateReducsOnTerminal_40, p_StateReducsOnTerminal_41, p_StateReducsOnTerminal_42, p_StateReducsOnTerminal_43 };
        public interface Actions
        {
        }
        protected override void setup()
        {
            p_Rules = p_StaticRules;
            p_RulesHeadID = p_StaticRulesHeadID;
            p_RulesHeadName = p_StaticRulesHeadName;
            p_RulesParserLength = p_StaticRulesParserLength;
            p_StateExpectedIDs = p_StaticStateExpectedIDs;
            p_StateExpectedNames = p_StaticStateExpectedNames;
            p_StateItems = p_StaticStateItems;
            p_StateShiftsOnTerminal = p_StaticStateShiftsOnTerminal;
            p_StateShiftsOnVariable = p_StaticStateShiftsOnVariable;
            p_StateReducsOnTerminal = p_StaticStateReducsOnTerminal;
            p_AxiomID = 0x10;
        }
        private Actions p_Actions;
        public AmbiguousLALR1_Parser(Actions actions, AmbiguousLALR1_Lexer lexer) : base (lexer) { p_Actions = actions; }
    }
}
