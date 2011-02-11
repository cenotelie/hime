namespace Analyzer
{
    public class Test_Lexer : Hime.Redist.Parsers.LexerText
    {
        private static ushort[] p_StaticSymbolsSID = { 0xC, 0xD, 0xE, 0xF, 0x10, 0x12, 0x5, 0x7 };
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
        private static ushort p_StaticSeparator = 0x7;
        protected override void setup() {
            p_SymbolsSID = p_StaticSymbolsSID;
            p_SymbolsName = p_StaticSymbolsName;
            p_SymbolsSubGrammars = new System.Collections.Generic.Dictionary<ushort, MatchSubGrammar>();
            p_Transitions = p_StaticTransitions;
            p_Finals = p_StaticFinals;
            p_SeparatorID = 0x7;
        }
        public override Hime.Redist.Parsers.ILexer Clone() {
            return new Test_Lexer(p_Input, p_CurrentPosition, p_Line, p_Errors);
        }
        public Test_Lexer(string input) : base(input) {}
        public Test_Lexer(string input, int position, int line, System.Collections.Generic.List<Hime.Redist.Parsers.LexerTextError> errors) : base(input, position, line, errors) {}
    }
    public class Test_Parser : Hime.Redist.Parsers.LR1TextParser
    {
        private static void Production_8_0 (Hime.Redist.Parsers.BaseLR1Parser parser, Hime.Redist.Parsers.SyntaxTreeNodeCollection nodes)
        {
            System.Collections.Generic.List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x8, "exp_atom"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Nothing);
            nodes.Add(SubRoot);
        }
        private static void Production_8_1 (Hime.Redist.Parsers.BaseLR1Parser parser, Hime.Redist.Parsers.SyntaxTreeNodeCollection nodes)
        {
            System.Collections.Generic.List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x8, "exp_atom"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Nothing);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Nothing);
            SubRoot.AppendChild(Definition[2], Hime.Redist.Parsers.SyntaxTreeNodeAction.Nothing);
            nodes.Add(SubRoot);
        }
        private static void Production_9_0 (Hime.Redist.Parsers.BaseLR1Parser parser, Hime.Redist.Parsers.SyntaxTreeNodeCollection nodes)
        {
            System.Collections.Generic.List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x9, "exp_op0"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Nothing);
            nodes.Add(SubRoot);
        }
        private static void Production_9_1 (Hime.Redist.Parsers.BaseLR1Parser parser, Hime.Redist.Parsers.SyntaxTreeNodeCollection nodes)
        {
            System.Collections.Generic.List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x9, "exp_op0"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Nothing);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Nothing);
            SubRoot.AppendChild(Definition[2], Hime.Redist.Parsers.SyntaxTreeNodeAction.Nothing);
            nodes.Add(SubRoot);
        }
        private static void Production_9_2 (Hime.Redist.Parsers.BaseLR1Parser parser, Hime.Redist.Parsers.SyntaxTreeNodeCollection nodes)
        {
            System.Collections.Generic.List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x9, "exp_op0"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Nothing);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Nothing);
            SubRoot.AppendChild(Definition[2], Hime.Redist.Parsers.SyntaxTreeNodeAction.Nothing);
            nodes.Add(SubRoot);
        }
        private static void Production_A_0 (Hime.Redist.Parsers.BaseLR1Parser parser, Hime.Redist.Parsers.SyntaxTreeNodeCollection nodes)
        {
            System.Collections.Generic.List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xA, "exp_op1"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Nothing);
            nodes.Add(SubRoot);
        }
        private static void Production_A_1 (Hime.Redist.Parsers.BaseLR1Parser parser, Hime.Redist.Parsers.SyntaxTreeNodeCollection nodes)
        {
            System.Collections.Generic.List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xA, "exp_op1"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Nothing);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Nothing);
            ((Test_Parser)parser).p_Actions.OnPlus(SubRoot);
            SubRoot.AppendChild(Definition[2], Hime.Redist.Parsers.SyntaxTreeNodeAction.Nothing);
            nodes.Add(SubRoot);
        }
        private static void Production_A_2 (Hime.Redist.Parsers.BaseLR1Parser parser, Hime.Redist.Parsers.SyntaxTreeNodeCollection nodes)
        {
            System.Collections.Generic.List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 3, 3);
            nodes.RemoveRange(nodes.Count - 3, 3);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xA, "exp_op1"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Nothing);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Nothing);
            SubRoot.AppendChild(Definition[2], Hime.Redist.Parsers.SyntaxTreeNodeAction.Nothing);
            nodes.Add(SubRoot);
        }
        private static void Production_B_0 (Hime.Redist.Parsers.BaseLR1Parser parser, Hime.Redist.Parsers.SyntaxTreeNodeCollection nodes)
        {
            System.Collections.Generic.List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 1, 1);
            nodes.RemoveRange(nodes.Count - 1, 1);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0xB, "exp"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Nothing);
            nodes.Add(SubRoot);
        }
        private static void Production_13_0 (Hime.Redist.Parsers.BaseLR1Parser parser, Hime.Redist.Parsers.SyntaxTreeNodeCollection nodes)
        {
            System.Collections.Generic.List<Hime.Redist.Parsers.SyntaxTreeNode> Definition = nodes.GetRange(nodes.Count - 2, 2);
            nodes.RemoveRange(nodes.Count - 2, 2);
            Hime.Redist.Parsers.SyntaxTreeNode SubRoot = new Hime.Redist.Parsers.SyntaxTreeNode(new Hime.Redist.Parsers.SymbolVariable(0x13, "_Axiom_"));
            SubRoot.AppendChild(Definition[0], Hime.Redist.Parsers.SyntaxTreeNodeAction.Promote);
            SubRoot.AppendChild(Definition[1], Hime.Redist.Parsers.SyntaxTreeNodeAction.Drop);
            nodes.Add(SubRoot);
        }
        private static Production[] p_StaticRules = { Production_8_0, Production_8_1, Production_9_0, Production_9_1, Production_9_2, Production_A_0, Production_A_1, Production_A_2, Production_B_0, Production_13_0 };
        private static ushort[] p_StaticRulesHeadID = { 0x8, 0x8, 0x9, 0x9, 0x9, 0xA, 0xA, 0xA, 0xB, 0x13 };
        private static string[] p_StaticRulesHeadName = { "exp_atom", "exp_atom", "exp_op0", "exp_op0", "exp_op0", "exp_op1", "exp_op1", "exp_op1", "exp", "_Axiom_" };
        private static ushort[] p_StaticRulesParserLength = { 0x1, 0x3, 0x1, 0x3, 0x3, 0x1, 0x3, 0x3, 0x1, 0x2 };
        private static ushort[] p_StateExpectedIDs_0 = { 0x5, 0xC };
        private static string[] p_StateExpectedNames_0 = { "NUMBER", "_T[(]" };
        private static string[] p_StateItems_0 = { "[_Axiom_ -> . exp $]", "[exp -> . exp_op1]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1 + exp_op0]", "[exp_op1 -> . exp_op1 - exp_op0]", "[exp_op0 -> . exp_atom]", "[exp_op0 -> . exp_op0 * exp_atom]", "[exp_op0 -> . exp_op0 / exp_atom]", "[exp_atom -> . NUMBER]", "[exp_atom -> . ( exp )]" };
        private static ushort[][] p_StateShiftsOnTerminal_0 = { new ushort[2] { 0x5, 0x5 }, new ushort[2] { 0xc, 0x6 } };
        private static ushort[][] p_StateShiftsOnVariable_0 = { new ushort[2] { 0xb, 0x1 }, new ushort[2] { 0xa, 0x2 }, new ushort[2] { 0x9, 0x3 }, new ushort[2] { 0x8, 0x4 } };
        private static ushort[][] p_StateReducsOnTerminal_0 = {  };
        private static ushort[] p_StateExpectedIDs_1 = { 0x2 };
        private static string[] p_StateExpectedNames_1 = { "$" };
        private static string[] p_StateItems_1 = { "[_Axiom_ -> exp . $]" };
        private static ushort[][] p_StateShiftsOnTerminal_1 = { new ushort[2] { 0x2, 0x7 } };
        private static ushort[][] p_StateShiftsOnVariable_1 = {  };
        private static ushort[][] p_StateReducsOnTerminal_1 = {  };
        private static ushort[] p_StateExpectedIDs_2 = { 0x2, 0xD, 0x10, 0x12 };
        private static string[] p_StateExpectedNames_2 = { "$", "_T[)]", "_T[+]", "_T[-]" };
        private static string[] p_StateItems_2 = { "[exp -> exp_op1 . ]", "[exp_op1 -> exp_op1 . + exp_op0]", "[exp_op1 -> exp_op1 . - exp_op0]" };
        private static ushort[][] p_StateShiftsOnTerminal_2 = { new ushort[2] { 0x10, 0x8 }, new ushort[2] { 0x12, 0x9 } };
        private static ushort[][] p_StateShiftsOnVariable_2 = {  };
        private static ushort[][] p_StateReducsOnTerminal_2 = { new ushort[2] { 0x2, 0x8 }, new ushort[2] { 0xd, 0x8 } };
        private static ushort[] p_StateExpectedIDs_3 = { 0x2, 0x10, 0x12, 0xD, 0xE, 0xF };
        private static string[] p_StateExpectedNames_3 = { "$", "_T[+]", "_T[-]", "_T[)]", "_T[*]", "_T[/]" };
        private static string[] p_StateItems_3 = { "[exp_op1 -> exp_op0 . ]", "[exp_op0 -> exp_op0 . * exp_atom]", "[exp_op0 -> exp_op0 . / exp_atom]" };
        private static ushort[][] p_StateShiftsOnTerminal_3 = { new ushort[2] { 0xe, 0xA }, new ushort[2] { 0xf, 0xB } };
        private static ushort[][] p_StateShiftsOnVariable_3 = {  };
        private static ushort[][] p_StateReducsOnTerminal_3 = { new ushort[2] { 0x2, 0x5 }, new ushort[2] { 0x10, 0x5 }, new ushort[2] { 0x12, 0x5 }, new ushort[2] { 0xd, 0x5 } };
        private static ushort[] p_StateExpectedIDs_4 = { 0x2, 0xE, 0xF, 0xD, 0x10, 0x12 };
        private static string[] p_StateExpectedNames_4 = { "$", "_T[*]", "_T[/]", "_T[)]", "_T[+]", "_T[-]" };
        private static string[] p_StateItems_4 = { "[exp_op0 -> exp_atom . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_4 = {  };
        private static ushort[][] p_StateShiftsOnVariable_4 = {  };
        private static ushort[][] p_StateReducsOnTerminal_4 = { new ushort[2] { 0x2, 0x2 }, new ushort[2] { 0xe, 0x2 }, new ushort[2] { 0xf, 0x2 }, new ushort[2] { 0xd, 0x2 }, new ushort[2] { 0x10, 0x2 }, new ushort[2] { 0x12, 0x2 } };
        private static ushort[] p_StateExpectedIDs_5 = { 0x2, 0xD, 0x10, 0x12, 0xE, 0xF };
        private static string[] p_StateExpectedNames_5 = { "$", "_T[)]", "_T[+]", "_T[-]", "_T[*]", "_T[/]" };
        private static string[] p_StateItems_5 = { "[exp_atom -> NUMBER . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_5 = {  };
        private static ushort[][] p_StateShiftsOnVariable_5 = {  };
        private static ushort[][] p_StateReducsOnTerminal_5 = { new ushort[2] { 0x2, 0x0 }, new ushort[2] { 0xd, 0x0 }, new ushort[2] { 0x10, 0x0 }, new ushort[2] { 0x12, 0x0 }, new ushort[2] { 0xe, 0x0 }, new ushort[2] { 0xf, 0x0 } };
        private static ushort[] p_StateExpectedIDs_6 = { 0x5, 0xC };
        private static string[] p_StateExpectedNames_6 = { "NUMBER", "_T[(]" };
        private static string[] p_StateItems_6 = { "[exp_atom -> ( . exp )]", "[exp -> . exp_op1]", "[exp_op1 -> . exp_op0]", "[exp_op1 -> . exp_op1 + exp_op0]", "[exp_op1 -> . exp_op1 - exp_op0]", "[exp_op0 -> . exp_atom]", "[exp_op0 -> . exp_op0 * exp_atom]", "[exp_op0 -> . exp_op0 / exp_atom]", "[exp_atom -> . NUMBER]", "[exp_atom -> . ( exp )]" };
        private static ushort[][] p_StateShiftsOnTerminal_6 = { new ushort[2] { 0x5, 0x5 }, new ushort[2] { 0xc, 0x6 } };
        private static ushort[][] p_StateShiftsOnVariable_6 = { new ushort[2] { 0xb, 0xC }, new ushort[2] { 0xa, 0x2 }, new ushort[2] { 0x9, 0x3 }, new ushort[2] { 0x8, 0x4 } };
        private static ushort[][] p_StateReducsOnTerminal_6 = {  };
        private static ushort[] p_StateExpectedIDs_7 = { 0x1 };
        private static string[] p_StateExpectedNames_7 = { "ε" };
        private static string[] p_StateItems_7 = { "[_Axiom_ -> exp $ . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_7 = {  };
        private static ushort[][] p_StateShiftsOnVariable_7 = {  };
        private static ushort[][] p_StateReducsOnTerminal_7 = { new ushort[2] { 0x1, 0x9 } };
        private static ushort[] p_StateExpectedIDs_8 = { 0x5, 0xC };
        private static string[] p_StateExpectedNames_8 = { "NUMBER", "_T[(]" };
        private static string[] p_StateItems_8 = { "[exp_op1 -> exp_op1 + . exp_op0]", "[exp_op0 -> . exp_atom]", "[exp_op0 -> . exp_op0 * exp_atom]", "[exp_op0 -> . exp_op0 / exp_atom]", "[exp_atom -> . NUMBER]", "[exp_atom -> . ( exp )]" };
        private static ushort[][] p_StateShiftsOnTerminal_8 = { new ushort[2] { 0x5, 0x5 }, new ushort[2] { 0xc, 0x6 } };
        private static ushort[][] p_StateShiftsOnVariable_8 = { new ushort[2] { 0x9, 0xD }, new ushort[2] { 0x8, 0x4 } };
        private static ushort[][] p_StateReducsOnTerminal_8 = {  };
        private static ushort[] p_StateExpectedIDs_9 = { 0x5, 0xC };
        private static string[] p_StateExpectedNames_9 = { "NUMBER", "_T[(]" };
        private static string[] p_StateItems_9 = { "[exp_op1 -> exp_op1 - . exp_op0]", "[exp_op0 -> . exp_atom]", "[exp_op0 -> . exp_op0 * exp_atom]", "[exp_op0 -> . exp_op0 / exp_atom]", "[exp_atom -> . NUMBER]", "[exp_atom -> . ( exp )]" };
        private static ushort[][] p_StateShiftsOnTerminal_9 = { new ushort[2] { 0x5, 0x5 }, new ushort[2] { 0xc, 0x6 } };
        private static ushort[][] p_StateShiftsOnVariable_9 = { new ushort[2] { 0x9, 0xE }, new ushort[2] { 0x8, 0x4 } };
        private static ushort[][] p_StateReducsOnTerminal_9 = {  };
        private static ushort[] p_StateExpectedIDs_A = { 0x5, 0xC };
        private static string[] p_StateExpectedNames_A = { "NUMBER", "_T[(]" };
        private static string[] p_StateItems_A = { "[exp_op0 -> exp_op0 * . exp_atom]", "[exp_atom -> . NUMBER]", "[exp_atom -> . ( exp )]" };
        private static ushort[][] p_StateShiftsOnTerminal_A = { new ushort[2] { 0x5, 0x5 }, new ushort[2] { 0xc, 0x6 } };
        private static ushort[][] p_StateShiftsOnVariable_A = { new ushort[2] { 0x8, 0xF } };
        private static ushort[][] p_StateReducsOnTerminal_A = {  };
        private static ushort[] p_StateExpectedIDs_B = { 0x5, 0xC };
        private static string[] p_StateExpectedNames_B = { "NUMBER", "_T[(]" };
        private static string[] p_StateItems_B = { "[exp_op0 -> exp_op0 / . exp_atom]", "[exp_atom -> . NUMBER]", "[exp_atom -> . ( exp )]" };
        private static ushort[][] p_StateShiftsOnTerminal_B = { new ushort[2] { 0x5, 0x5 }, new ushort[2] { 0xc, 0x6 } };
        private static ushort[][] p_StateShiftsOnVariable_B = { new ushort[2] { 0x8, 0x10 } };
        private static ushort[][] p_StateReducsOnTerminal_B = {  };
        private static ushort[] p_StateExpectedIDs_C = { 0xD };
        private static string[] p_StateExpectedNames_C = { "_T[)]" };
        private static string[] p_StateItems_C = { "[exp_atom -> ( exp . )]" };
        private static ushort[][] p_StateShiftsOnTerminal_C = { new ushort[2] { 0xd, 0x11 } };
        private static ushort[][] p_StateShiftsOnVariable_C = {  };
        private static ushort[][] p_StateReducsOnTerminal_C = {  };
        private static ushort[] p_StateExpectedIDs_D = { 0x2, 0x10, 0x12, 0xD, 0xE, 0xF };
        private static string[] p_StateExpectedNames_D = { "$", "_T[+]", "_T[-]", "_T[)]", "_T[*]", "_T[/]" };
        private static string[] p_StateItems_D = { "[exp_op1 -> exp_op1 + exp_op0 . ]", "[exp_op0 -> exp_op0 . * exp_atom]", "[exp_op0 -> exp_op0 . / exp_atom]" };
        private static ushort[][] p_StateShiftsOnTerminal_D = { new ushort[2] { 0xe, 0xA }, new ushort[2] { 0xf, 0xB } };
        private static ushort[][] p_StateShiftsOnVariable_D = {  };
        private static ushort[][] p_StateReducsOnTerminal_D = { new ushort[2] { 0x2, 0x6 }, new ushort[2] { 0x10, 0x6 }, new ushort[2] { 0x12, 0x6 }, new ushort[2] { 0xd, 0x6 } };
        private static ushort[] p_StateExpectedIDs_E = { 0x2, 0x10, 0x12, 0xD, 0xE, 0xF };
        private static string[] p_StateExpectedNames_E = { "$", "_T[+]", "_T[-]", "_T[)]", "_T[*]", "_T[/]" };
        private static string[] p_StateItems_E = { "[exp_op1 -> exp_op1 - exp_op0 . ]", "[exp_op0 -> exp_op0 . * exp_atom]", "[exp_op0 -> exp_op0 . / exp_atom]" };
        private static ushort[][] p_StateShiftsOnTerminal_E = { new ushort[2] { 0xe, 0xA }, new ushort[2] { 0xf, 0xB } };
        private static ushort[][] p_StateShiftsOnVariable_E = {  };
        private static ushort[][] p_StateReducsOnTerminal_E = { new ushort[2] { 0x2, 0x7 }, new ushort[2] { 0x10, 0x7 }, new ushort[2] { 0x12, 0x7 }, new ushort[2] { 0xd, 0x7 } };
        private static ushort[] p_StateExpectedIDs_F = { 0x2, 0xE, 0xF, 0xD, 0x10, 0x12 };
        private static string[] p_StateExpectedNames_F = { "$", "_T[*]", "_T[/]", "_T[)]", "_T[+]", "_T[-]" };
        private static string[] p_StateItems_F = { "[exp_op0 -> exp_op0 * exp_atom . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_F = {  };
        private static ushort[][] p_StateShiftsOnVariable_F = {  };
        private static ushort[][] p_StateReducsOnTerminal_F = { new ushort[2] { 0x2, 0x3 }, new ushort[2] { 0xe, 0x3 }, new ushort[2] { 0xf, 0x3 }, new ushort[2] { 0xd, 0x3 }, new ushort[2] { 0x10, 0x3 }, new ushort[2] { 0x12, 0x3 } };
        private static ushort[] p_StateExpectedIDs_10 = { 0x2, 0xE, 0xF, 0xD, 0x10, 0x12 };
        private static string[] p_StateExpectedNames_10 = { "$", "_T[*]", "_T[/]", "_T[)]", "_T[+]", "_T[-]" };
        private static string[] p_StateItems_10 = { "[exp_op0 -> exp_op0 / exp_atom . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_10 = {  };
        private static ushort[][] p_StateShiftsOnVariable_10 = {  };
        private static ushort[][] p_StateReducsOnTerminal_10 = { new ushort[2] { 0x2, 0x4 }, new ushort[2] { 0xe, 0x4 }, new ushort[2] { 0xf, 0x4 }, new ushort[2] { 0xd, 0x4 }, new ushort[2] { 0x10, 0x4 }, new ushort[2] { 0x12, 0x4 } };
        private static ushort[] p_StateExpectedIDs_11 = { 0x2, 0xD, 0x10, 0x12, 0xE, 0xF };
        private static string[] p_StateExpectedNames_11 = { "$", "_T[)]", "_T[+]", "_T[-]", "_T[*]", "_T[/]" };
        private static string[] p_StateItems_11 = { "[exp_atom -> ( exp ) . ]" };
        private static ushort[][] p_StateShiftsOnTerminal_11 = {  };
        private static ushort[][] p_StateShiftsOnVariable_11 = {  };
        private static ushort[][] p_StateReducsOnTerminal_11 = { new ushort[2] { 0x2, 0x1 }, new ushort[2] { 0xd, 0x1 }, new ushort[2] { 0x10, 0x1 }, new ushort[2] { 0x12, 0x1 }, new ushort[2] { 0xe, 0x1 }, new ushort[2] { 0xf, 0x1 } };
        private static ushort[][] p_StaticStateExpectedIDs = { p_StateExpectedIDs_0, p_StateExpectedIDs_1, p_StateExpectedIDs_2, p_StateExpectedIDs_3, p_StateExpectedIDs_4, p_StateExpectedIDs_5, p_StateExpectedIDs_6, p_StateExpectedIDs_7, p_StateExpectedIDs_8, p_StateExpectedIDs_9, p_StateExpectedIDs_A, p_StateExpectedIDs_B, p_StateExpectedIDs_C, p_StateExpectedIDs_D, p_StateExpectedIDs_E, p_StateExpectedIDs_F, p_StateExpectedIDs_10, p_StateExpectedIDs_11 };
        private static string[][] p_StaticStateExpectedNames = { p_StateExpectedNames_0, p_StateExpectedNames_1, p_StateExpectedNames_2, p_StateExpectedNames_3, p_StateExpectedNames_4, p_StateExpectedNames_5, p_StateExpectedNames_6, p_StateExpectedNames_7, p_StateExpectedNames_8, p_StateExpectedNames_9, p_StateExpectedNames_A, p_StateExpectedNames_B, p_StateExpectedNames_C, p_StateExpectedNames_D, p_StateExpectedNames_E, p_StateExpectedNames_F, p_StateExpectedNames_10, p_StateExpectedNames_11 };
        private static string[][] p_StaticStateItems = { p_StateItems_0, p_StateItems_1, p_StateItems_2, p_StateItems_3, p_StateItems_4, p_StateItems_5, p_StateItems_6, p_StateItems_7, p_StateItems_8, p_StateItems_9, p_StateItems_A, p_StateItems_B, p_StateItems_C, p_StateItems_D, p_StateItems_E, p_StateItems_F, p_StateItems_10, p_StateItems_11 };
        private static ushort[][][] p_StaticStateShiftsOnTerminal = { p_StateShiftsOnTerminal_0, p_StateShiftsOnTerminal_1, p_StateShiftsOnTerminal_2, p_StateShiftsOnTerminal_3, p_StateShiftsOnTerminal_4, p_StateShiftsOnTerminal_5, p_StateShiftsOnTerminal_6, p_StateShiftsOnTerminal_7, p_StateShiftsOnTerminal_8, p_StateShiftsOnTerminal_9, p_StateShiftsOnTerminal_A, p_StateShiftsOnTerminal_B, p_StateShiftsOnTerminal_C, p_StateShiftsOnTerminal_D, p_StateShiftsOnTerminal_E, p_StateShiftsOnTerminal_F, p_StateShiftsOnTerminal_10, p_StateShiftsOnTerminal_11 };
        private static ushort[][][] p_StaticStateShiftsOnVariable = { p_StateShiftsOnVariable_0, p_StateShiftsOnVariable_1, p_StateShiftsOnVariable_2, p_StateShiftsOnVariable_3, p_StateShiftsOnVariable_4, p_StateShiftsOnVariable_5, p_StateShiftsOnVariable_6, p_StateShiftsOnVariable_7, p_StateShiftsOnVariable_8, p_StateShiftsOnVariable_9, p_StateShiftsOnVariable_A, p_StateShiftsOnVariable_B, p_StateShiftsOnVariable_C, p_StateShiftsOnVariable_D, p_StateShiftsOnVariable_E, p_StateShiftsOnVariable_F, p_StateShiftsOnVariable_10, p_StateShiftsOnVariable_11 };
        private static ushort[][][] p_StaticStateReducsOnTerminal = { p_StateReducsOnTerminal_0, p_StateReducsOnTerminal_1, p_StateReducsOnTerminal_2, p_StateReducsOnTerminal_3, p_StateReducsOnTerminal_4, p_StateReducsOnTerminal_5, p_StateReducsOnTerminal_6, p_StateReducsOnTerminal_7, p_StateReducsOnTerminal_8, p_StateReducsOnTerminal_9, p_StateReducsOnTerminal_A, p_StateReducsOnTerminal_B, p_StateReducsOnTerminal_C, p_StateReducsOnTerminal_D, p_StateReducsOnTerminal_E, p_StateReducsOnTerminal_F, p_StateReducsOnTerminal_10, p_StateReducsOnTerminal_11 };
        public interface Actions
        {
        void OnPlus(Hime.Redist.Parsers.SyntaxTreeNode SubRoot);
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
            p_ErrorSimulationLength = 3;
        }
        private Actions p_Actions;
        public Test_Parser(Actions actions, Test_Lexer lexer) : base (lexer) { p_Actions = actions; }
    }
}
