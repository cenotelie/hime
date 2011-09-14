/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;
using Hime.Kernel.Unicode;

namespace Hime.Parsers.ContextFree
{
    class CFGrammarCompiler : Kernel.Resources.IResourceCompiler
    {
        private static string[] resourcesNames = new string[] { "cf_grammar_text", "cf_grammar_bin" };
        private const string subruleHeadRadical = "";
        private const string subruleHeadRadicalMultiplicity = subruleHeadRadical + "_m";
        private const string subruleHeadRadicalRestrict = subruleHeadRadical + "_r";
        private Hime.Kernel.Reporting.Reporter log;
        private bool hasErrors;

        public string CompilerName { get { return "HimeSystems.CentralDogma.ContextFreeGrammarCompiler"; } }
        public int CompilerVersionMajor { get { return 1; } }
        public int CompilerVersionMinor { get { return 0; } }
        public string[] ResourceNames { get { return resourcesNames; } }

        public CFGrammarCompiler() { }

        public void CreateResource(Kernel.Naming.Symbol container, Redist.Parsers.SyntaxTreeNode syntaxNode, Kernel.Resources.ResourceGraph graph, Hime.Kernel.Reporting.Reporter log)
        {
            Kernel.Naming.SymbolAccess Access = Kernel.Resources.ResourceCompiler.CompileSymbolAccess(syntaxNode.Children[0]);
            string Name = ((Redist.Parsers.SymbolTokenText)syntaxNode.Children[1].Symbol).ValueText;
            CFGrammar Grammar = null;
            Kernel.Resources.Resource Resource = null;
            
            if (syntaxNode.Symbol.Name == "cf_grammar_text")
                Grammar = new CFGrammarText(Name);
            else if (syntaxNode.Symbol.Name == "cf_grammar_bin")
                Grammar = new CFGrammarBinary(Name);
            Grammar.Access = Access;
            Resource = new Hime.Kernel.Resources.Resource(Grammar, syntaxNode, this);
            container.SymbolAddChild(Grammar);
            graph.AddResource(Resource);
        }
        public void CreateDependencies(Kernel.Resources.Resource resource, Kernel.Resources.ResourceGraph graph, Hime.Kernel.Reporting.Reporter log)
        {
            foreach (Redist.Parsers.SyntaxTreeNode Parent in resource.SyntaxNode.Children[2].Children)
            {
                Kernel.Naming.QualifiedName Name = Kernel.Resources.ResourceCompiler.CompileQualifiedName(Parent);
                Kernel.Naming.Symbol Symbol = resource.Symbol.ResolveName(Name);
                if (resource.Symbol is CFGrammarText && Symbol is CFGrammarText)
                    resource.AddDependency("parent", graph.GetResource(Symbol));
                else if (resource.Symbol is CFGrammarBinary && Symbol is CFGrammarBinary)
                    resource.AddDependency("parent", graph.GetResource(Symbol));
            }
            if (resource.Symbol is CFGrammarText)
            {
                for (int i = 3; i < resource.SyntaxNode.Children.Count; i++)
                {
                    Redist.Parsers.SyntaxTreeNode node = resource.SyntaxNode.Children[i];
                    if (node.Symbol.Name == "terminals")
                    {
                        foreach (Redist.Parsers.SyntaxTreeNode Terminal in node.Children)
                        {
                            if (Terminal.Children[2].Children.Count == 1)
                            {
                                Kernel.Naming.QualifiedName Name = Kernel.Resources.ResourceCompiler.CompileQualifiedName(Terminal.Children[2].Children[0]);
                                Kernel.Naming.Symbol Symbol = resource.Symbol.ResolveName(Name);
                                if (Symbol is Grammar)
                                {
                                    bool Found = false;
                                    foreach (KeyValuePair<string, Kernel.Resources.Resource> D in resource.Dependencies)
                                    {
                                        if (D.Value.Symbol == Symbol)
                                        {
                                            Found = true;
                                            break;
                                        }
                                    }
                                    if (!Found)
                                        resource.AddDependency("subgrammar", graph.GetResource(Symbol));
                                }
                            }
                        }
                    }
                }
            }
        }
        public int CompileSolveDependencies(Kernel.Resources.Resource resource, Hime.Kernel.Reporting.Reporter log)
        {
            CFGrammar grammar = resource.Symbol as CFGrammar;
            int Solved = 0;
            for (int i = 0; i != resource.Dependencies.Count; i++)
            {
                if (resource.Dependencies[i].Value.IsCompiled)
                {
                    if (resource.Dependencies[i].Key == "parent")
                        grammar.Inherit((CFGrammar)resource.Dependencies[i].Value.Symbol);
                    resource.Dependencies.RemoveAt(i);
                    i--;
                    Solved++;
                }
            }
            return Solved;
        }
        public bool Compile(Kernel.Resources.Resource resource, Hime.Kernel.Reporting.Reporter log)
        {
            this.log = log;
            this.hasErrors = false;
            if (resource.Symbol is CFGrammarText)
            {
                Compile_Recognize_grammar_text((CFGrammar)resource.Symbol, resource.SyntaxNode);
                resource.IsCompiled = true;
            }
            else if (resource.Symbol is CFGrammarBinary)
            {
                Compile_Recognize_grammar_bin((CFGrammar)resource.Symbol, resource.SyntaxNode);
                resource.IsCompiled = true;
            }
            return (!hasErrors);
        }
        public override string ToString()
        {
            return CompilerName + " " + CompilerVersionMajor.ToString() + "." + CompilerVersionMinor.ToString();
        }



        private Symbol Compile_Tool_NameToSymbol(string Name, CFGrammar Data, CFGrammarCompilerContext Context)
        {
            if (Context.IsReference(Name))
                return Context.GetReference(Name);
            return Data.GetSymbol(Name);
        }

        private void Compile_Recognize_option(CFGrammar data, Redist.Parsers.SyntaxTreeNode Node)
        {
            string Name = ((Redist.Parsers.SymbolTokenText)Node.Children[0].Symbol).ValueText;
            string Value = ((Redist.Parsers.SymbolTokenText)Node.Children[1].Symbol).ValueText;
            Value = Value.Substring(1, Value.Length - 2);
            data.AddOption(Name, Value);
        }

        private Automata.NFA Compile_Recognize_terminal_def_atom_any(Redist.Parsers.SyntaxTreeNode Node)
        {
            Automata.NFA Final = new Hime.Parsers.Automata.NFA();
            Final.StateEntry = Final.AddNewState();
            Final.StateExit = Final.AddNewState();
            char begin = System.Convert.ToChar(0x0000);
            char end = System.Convert.ToChar(0xFFFF);
            Final.StateEntry.AddTransition(new Automata.CharSpan(begin, end), Final.StateExit);
            return Final;
        }
        private Automata.NFA Compile_Recognize_terminal_def_atom_unicode(Redist.Parsers.SyntaxTreeNode Node)
        {
            Automata.NFA Final = new Hime.Parsers.Automata.NFA();
            Final.StateEntry = Final.AddNewState();
            Final.StateExit = Final.AddNewState();
            string Value = ((Redist.Parsers.SymbolTokenText)Node.Symbol).ValueText;
            Value = Value.Substring(2, Value.Length - 2);
            int CharInt = System.Convert.ToInt32(Value, 16);
            char Char = System.Convert.ToChar(CharInt);
            Final.StateEntry.AddTransition(new Automata.CharSpan(Char, Char), Final.StateExit);
            return Final;
        }
        private string ReplaceEscapees(string value)
        {
            if (!value.Contains("\\"))
                return value;
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            for (int i = 0; i != value.Length; i++)
            {
                char c = value[i];
                if (c != '\\')
                {
                    builder.Append(c);
                    continue;
                }
                i++;
                char next = value[i];
                if (next == '\\') builder.Append(next);
                else if (next == '0') builder.Append('\0'); /*Unicode character 0*/
                else if (next == 'a') builder.Append('\a'); /*Alert (character 7)*/
                else if (next == 'b') builder.Append('\b'); /*Backspace (character 8)*/
                else if (next == 'f') builder.Append('\f'); /*Form feed (character 12)*/
                else if (next == 'n') builder.Append('\n'); /*New line (character 10)*/
                else if (next == 'r') builder.Append('\r'); /*Carriage return (character 13)*/
                else if (next == 't') builder.Append('\t'); /*Horizontal tab (character 9)*/
                else if (next == 'v') builder.Append('\v'); /*Vertical quote (character 11)*/
                else builder.Append("\\" + next);
            }
            return builder.ToString();
        }
        private Automata.NFA Compile_Recognize_terminal_def_atom_text(Redist.Parsers.SyntaxTreeNode Node)
        {
            Automata.NFA Final = new Hime.Parsers.Automata.NFA();
            Final.StateEntry = Final.AddNewState();
            Final.StateExit = Final.StateEntry;
            string Value = ((Redist.Parsers.SymbolTokenText)Node.Symbol).ValueText;
            Value = Value.Substring(1, Value.Length - 2);
            Value = ReplaceEscapees(Value).Replace("\\'", "'");
            foreach (char c in Value)
            {
                Automata.NFAState Temp = Final.AddNewState();
                Final.StateExit.AddTransition(new Automata.CharSpan(c, c), Temp);
                Final.StateExit = Temp;
            }
            return Final;
        }
        private Automata.NFA Compile_Recognize_terminal_def_atom_set(Redist.Parsers.SyntaxTreeNode Node)
        {
            Automata.NFA Final = new Hime.Parsers.Automata.NFA();
            Final.StateEntry = Final.AddNewState();
            Final.StateExit = Final.AddNewState();
            string Value = ((Redist.Parsers.SymbolTokenText)Node.Symbol).ValueText;
            Value = Value.Substring(1, Value.Length - 2);
            Value = ReplaceEscapees(Value).Replace("\\[", "[").Replace("\\]", "]");
            bool Positive = true;
            if (Value[0] == '^')
            {
                Value = Value.Substring(1);
                Positive = false;
            }
            List<Automata.CharSpan> Spans = new List<Automata.CharSpan>();
            for (int i = 0; i != Value.Length; i++)
            {
                if ((i != Value.Length - 1) && (Value[i + 1] == '-'))
                {
                    Spans.Add(new Automata.CharSpan(Value[i], Value[i + 2]));
                    i += 2;
                }
                else
                    Spans.Add(new Automata.CharSpan(Value[i], Value[i]));
            }
            if (Positive)
            {
                foreach (Automata.CharSpan Span in Spans)
                    Final.StateEntry.AddTransition(Span, Final.StateExit);
            }
            else
            {
                Spans.Sort(new System.Comparison<Automata.CharSpan>(Automata.CharSpan.Compare));
                char b = char.MinValue;
                for (int i = 0; i != Spans.Count; i++)
                {
                    if (Spans[i].Begin > b)
                        Final.StateEntry.AddTransition(new Automata.CharSpan(b, System.Convert.ToChar(Spans[i].Begin - 1)), Final.StateExit);
                    b = System.Convert.ToChar(Spans[i].End + 1);
                }
                Final.StateEntry.AddTransition(new Automata.CharSpan(b, char.MaxValue), Final.StateExit);
            }
            return Final;
        }
        private Automata.NFA Compile_Recognize_terminal_def_atom_ublock(Redist.Parsers.SyntaxTreeNode Node)
        {
            Automata.NFA Final = new Hime.Parsers.Automata.NFA();
            Final.StateEntry = Final.AddNewState();
            Final.StateExit = Final.AddNewState();
            Redist.Parsers.SymbolTokenText token = (Redist.Parsers.SymbolTokenText)Node.Symbol;
            string value = token.ValueText.Substring(4, token.ValueText.Length - 5);
            UnicodeBlock block = UnicodeBlock.Categories[value];
            // Create transition and return
            Final.StateEntry.AddTransition(new Automata.CharSpan(System.Convert.ToChar(block.Begin), System.Convert.ToChar(block.End)), Final.StateExit);
            return Final;
        }
        private Automata.NFA Compile_Recognize_terminal_def_atom_ucat(Redist.Parsers.SyntaxTreeNode Node)
        {
            Automata.NFA Final = new Hime.Parsers.Automata.NFA();
            Final.StateEntry = Final.AddNewState();
            Final.StateExit = Final.AddNewState();
            Redist.Parsers.SymbolTokenText token = (Redist.Parsers.SymbolTokenText)Node.Symbol;
            string value = token.ValueText.Substring(4, token.ValueText.Length - 5);
            UnicodeCategory category = UnicodeCategory.Classes[value];
            // Create transitions and return
            foreach (UnicodeSpan span in category.Spans)
                Final.StateEntry.AddTransition(new Automata.CharSpan(System.Convert.ToChar(span.Begin), System.Convert.ToChar(span.End)), Final.StateExit);
            return Final;
        }
        private Automata.NFA Compile_Recognize_terminal_def_atom_span(Redist.Parsers.SyntaxTreeNode Node)
        {
            Automata.NFA Final = new Hime.Parsers.Automata.NFA();
            Final.StateEntry = Final.AddNewState();
            Final.StateExit = Final.AddNewState();
            int SpanBegin = 0;
            int SpanEnd = 0;
            // Get span begin
            Redist.Parsers.SymbolTokenText Child = (Redist.Parsers.SymbolTokenText)Node.Children[0].Symbol;
            SpanBegin = System.Convert.ToInt32(Child.ValueText.Substring(2), 16);
            // Get span end
            Child = (Redist.Parsers.SymbolTokenText)Node.Children[1].Symbol;
            SpanEnd = System.Convert.ToInt32(Child.ValueText.Substring(2), 16);
            // If span end is before beginning: reverse
            if (SpanBegin > SpanEnd)
            {
                int Temp = SpanEnd;
                SpanEnd = SpanBegin;
                SpanBegin = Temp;
            }
            // Create transition and return
            Final.StateEntry.AddTransition(new Automata.CharSpan(System.Convert.ToChar(SpanBegin), System.Convert.ToChar(SpanEnd)), Final.StateExit);
            return Final;
        }
        private Automata.NFA Compile_Recognize_terminal_def_atom_name(CFGrammar data, Redist.Parsers.SyntaxTreeNode node)
        {
            Redist.Parsers.SymbolTokenText token = (Redist.Parsers.SymbolTokenText)node.Symbol;
            Terminal Ref = data.GetTerminal(token.ValueText);
            if (Ref == null)
            {
                log.Error("Compiler", "@" + token.Line + " Cannot find terminal " + token.ValueText);
                hasErrors = true;
                Automata.NFA Final = new Hime.Parsers.Automata.NFA();
                Final.StateEntry = Final.AddNewState();
                Final.StateExit = Final.AddNewState();
                Final.StateEntry.AddTransition(Automata.NFA.Epsilon, Final.StateExit);
                return Final;
            }
            return ((TerminalText)Ref).NFA.Clone(false);
        }
        private Automata.NFA Compile_Recognize_terminal_definition(CFGrammar Data, Redist.Parsers.SyntaxTreeNode Node)
        {
            // Symbol is a token
            if (Node.Symbol is Redist.Parsers.SymbolTokenText)
            {
                Redist.Parsers.SymbolTokenText Token = (Redist.Parsers.SymbolTokenText)Node.Symbol;
                if (Token.ValueText == "?")
                {
                    Automata.NFA Inner = Compile_Recognize_terminal_definition(Data, Node.Children[0]);
                    return Automata.NFA.OperatorOption(Inner, false);
                }
                if (Token.ValueText == "*")
                {
                    Automata.NFA Inner = Compile_Recognize_terminal_definition(Data, Node.Children[0]);
                    return Automata.NFA.OperatorStar(Inner, false);
                }
                if (Token.ValueText == "+")
                {
                    Automata.NFA Inner = Compile_Recognize_terminal_definition(Data, Node.Children[0]);
                    return Automata.NFA.OperatorPlus(Inner, false);
                }
                if (Token.ValueText == "|")
                {
                    Automata.NFA Left = Compile_Recognize_terminal_definition(Data, Node.Children[0]);
                    Automata.NFA Right = Compile_Recognize_terminal_definition(Data, Node.Children[1]);
                    return Automata.NFA.OperatorUnion(Left, Right, false);
                }
                if (Token.ValueText == "-")
                {
                    Automata.NFA Left = Compile_Recognize_terminal_definition(Data, Node.Children[0]);
                    Automata.NFA Right = Compile_Recognize_terminal_definition(Data, Node.Children[1]);
                    return Automata.NFA.OperatorDifference(Left, Right, false);
                }
                if (Token.ValueText == ".")
                    return Compile_Recognize_terminal_def_atom_any(Node);
                if (Token.Name == "SYMBOL_VALUE_UINT8")
                    return Compile_Recognize_terminal_def_atom_unicode(Node);
                if (Token.Name == "SYMBOL_VALUE_UINT16")
                    return Compile_Recognize_terminal_def_atom_unicode(Node);
                if (Token.Name == "SYMBOL_TERMINAL_TEXT")
                    return Compile_Recognize_terminal_def_atom_text(Node);
                if (Token.Name == "SYMBOL_TERMINAL_SET")
                    return Compile_Recognize_terminal_def_atom_set(Node);
                if (Token.Name == "SYMBOL_TERMINAL_UCAT")
                    return Compile_Recognize_terminal_def_atom_ucat(Node);
                if (Token.Name == "SYMBOL_TERMINAL_UBLOCK")
                    return Compile_Recognize_terminal_def_atom_ublock(Node);
                if (Token.ValueText == "..")
                    return Compile_Recognize_terminal_def_atom_span(Node);
                if (Token.Name == "NAME")
                    return Compile_Recognize_terminal_def_atom_name(Data, Node);
                Automata.NFA Final = new Hime.Parsers.Automata.NFA();
                Final.StateEntry = Final.AddNewState();
                Final.StateExit = Final.AddNewState();
                Final.StateEntry.AddTransition(Automata.NFA.Epsilon, Final.StateExit);
                return Final;
            }
            else if (Node.Symbol is Redist.Parsers.SymbolVirtual)
            {
                Redist.Parsers.SymbolVirtual Token = (Redist.Parsers.SymbolVirtual)Node.Symbol;
                if (Node.Symbol.Name == "range")
                {
                    Automata.NFA Inner = Compile_Recognize_terminal_definition(Data, Node.Children[0]);
                    uint min = System.Convert.ToUInt32(((Redist.Parsers.SymbolTokenText)Node.Children[1].Symbol).ValueText);
                    uint max = min;
                    if (Node.Children.Count > 2)
                        max = System.Convert.ToUInt32(((Redist.Parsers.SymbolTokenText)Node.Children[2].Symbol).ValueText);
                    return Automata.NFA.OperatorRange(Inner, false, min, max);
                }
                else if (Node.Symbol.Name == "concat")
                {
                    Automata.NFA Left = Compile_Recognize_terminal_definition(Data, Node.Children[0]);
                    Automata.NFA Right = Compile_Recognize_terminal_definition(Data, Node.Children[1]);
                    return Automata.NFA.OperatorConcat(Left, Right, false);
                }
            }
            return null;
        }
        private Terminal Compile_Recognize_terminal(CFGrammar Data, Redist.Parsers.SyntaxTreeNode Node)
        {
            string Name = ((Redist.Parsers.SymbolTokenText)Node.Children[0].Symbol).ValueText;
            Automata.NFA NFA = Compile_Recognize_terminal_definition(Data, Node.Children[1]);
            Grammar SubGrammar = null;
            if (Node.Children[2].Children.Count == 1)
            {
                Kernel.Naming.QualifiedName SubGrammarName = Kernel.Resources.ResourceCompiler.CompileQualifiedName(Node.Children[2].Children[0]);
                SubGrammar = (Grammar)Data.ResolveName(SubGrammarName);
            }
            Terminal Terminal = Data.AddTerminalText(Name, NFA, SubGrammar);
            ((TerminalText)Terminal).SubGrammar = SubGrammar;
            NFA.StateExit.Final = Terminal;
            return Terminal;
        }


        private CFRuleDefinitionSet Compile_Recognize_grammar_bin_terminal_data(CFGrammar Data, Redist.Parsers.SyntaxTreeNode Node)
        {
            CFRuleDefinitionSet Set = new CFRuleDefinitionSet();
            string Name = ((Redist.Parsers.SymbolTokenText)Node.Symbol).ValueText;
            Terminal Terminal = Data.GetTerminal(Name);
            if (Terminal == null)
                Terminal = Data.AddTerminalBin((TerminalBinType)System.Enum.Parse(typeof(TerminalBinType), Node.Symbol.Name), Name);
            Set.Add(new CFRuleDefinition(Terminal));
            return Set;
        }
        private CFRuleDefinitionSet Compile_Recognize_grammar_bin_terminal(CFGrammar Data, Redist.Parsers.SyntaxTreeNode Node)
        {
            Node = Node.Children[0];
            if (Node.Symbol.Name == "SYMBOL_VALUE_UINT8")
                return Compile_Recognize_grammar_bin_terminal_data(Data, Node);
            if (Node.Symbol.Name == "SYMBOL_VALUE_UINT16")
                return Compile_Recognize_grammar_bin_terminal_data(Data, Node);
            if (Node.Symbol.Name == "SYMBOL_VALUE_UINT32")
                return Compile_Recognize_grammar_bin_terminal_data(Data, Node);
            if (Node.Symbol.Name == "SYMBOL_VALUE_UINT64")
                return Compile_Recognize_grammar_bin_terminal_data(Data, Node);
            if (Node.Symbol.Name == "SYMBOL_VALUE_UINT128")
                return Compile_Recognize_grammar_bin_terminal_data(Data, Node);
            if (Node.Symbol.Name == "SYMBOL_VALUE_BINARY")
                return Compile_Recognize_grammar_bin_terminal_data(Data, Node);
            if (Node.Symbol.Name == "SYMBOL_JOKER_UINT8")
                return Compile_Recognize_grammar_bin_terminal_data(Data, Node);
            if (Node.Symbol.Name == "SYMBOL_JOKER_UINT16")
                return Compile_Recognize_grammar_bin_terminal_data(Data, Node);
            if (Node.Symbol.Name == "SYMBOL_JOKER_UINT32")
                return Compile_Recognize_grammar_bin_terminal_data(Data, Node);
            if (Node.Symbol.Name == "SYMBOL_JOKER_UINT64")
                return Compile_Recognize_grammar_bin_terminal_data(Data, Node);
            if (Node.Symbol.Name == "SYMBOL_JOKER_UINT128")
                return Compile_Recognize_grammar_bin_terminal_data(Data, Node);
            if (Node.Symbol.Name == "SYMBOL_JOKER_BINARY")
                return Compile_Recognize_grammar_bin_terminal_data(Data, Node);
            return new CFRuleDefinitionSet();
        }
        private CFRuleDefinitionSet Compile_Recognize_grammar_text_terminal(CFGrammar Data, Redist.Parsers.SyntaxTreeNode Node)
        {
            // Construct the terminal name
            string Value = ((Redist.Parsers.SymbolTokenText)Node.Children[0].Symbol).ValueText;
            Value = Value.Substring(1, Value.Length - 2);
            Value = "@\"" + Value.Replace("\\'", "'").Replace("\\\\", "\\") + "\"";
            // Check for previous instance in the grammar's data
            Terminal Terminal = Data.GetTerminal(Value);
            if (Terminal == null)
            {
                // Create the terminal
                Automata.NFA NFA = Compile_Recognize_terminal_def_atom_text(Node.Children[0]);
                Terminal = Data.AddTerminalText(Value, NFA, null);
                NFA.StateExit.Final = Terminal;
            }
            // Create the definition set
            CFRuleDefinitionSet Set = new CFRuleDefinitionSet();
            Set.Add(new CFRuleDefinition(Terminal));
            return Set;
        }


        private CFRuleDefinitionSet Compile_Recognize_rule_sym_action(CFGrammar Data, Redist.Parsers.SyntaxTreeNode Node)
        {
            CFRuleDefinitionSet Set = new CFRuleDefinitionSet();
            string name = ((Redist.Parsers.SymbolTokenText)Node.Children[0].Symbol).ValueText;
            Action Action = Data.GetAction(name);
            if (Action == null)
                Action = Data.AddAction(name);
            Set.Add(new CFRuleDefinition(Action));
            return Set;
        }
        private CFRuleDefinitionSet Compile_Recognize_rule_sym_virtual(CFGrammar Data, Redist.Parsers.SyntaxTreeNode Node)
        {
            CFRuleDefinitionSet Set = new CFRuleDefinitionSet();
            string Name = ((Redist.Parsers.SymbolTokenText)Node.Children[0].Symbol).ValueText;
            Name = Name.Substring(1, Name.Length - 2);
            Virtual Virtual = Data.GetVirtual(Name);
            if (Virtual == null)
                Virtual = Data.AddVirtual(Name);
            Set.Add(new CFRuleDefinition(Virtual));
            return Set;
        }
        private CFRuleDefinitionSet Compile_Recognize_rule_sym_ref_simple(CFGrammar data, CFGrammarCompilerContext context, Redist.Parsers.SyntaxTreeNode node)
        {
            Redist.Parsers.SymbolTokenText token = ((Redist.Parsers.SymbolTokenText)node.Children[0].Symbol);
            CFRuleDefinitionSet Defs = new CFRuleDefinitionSet();
            if (token.ValueText == "ε")
            {
                Defs.Add(new CFRuleDefinition());
            }
            else
            {
                Symbol Symbol = Compile_Tool_NameToSymbol(token.ValueText, data, context);
                if (Symbol != null)
                    Defs.Add(new CFRuleDefinition(Symbol));
                else
                {
                    log.Error("Compiler", "@" + token.Line + " Unrecognized symbol " + token.ValueText + " in rule definition");
                    hasErrors = true;
                    Defs.Add(new CFRuleDefinition());
                }
            }
            return Defs;
        }
        private CFRuleDefinitionSet Compile_Recognize_rule_sym_ref_template(CFGrammar Data, CFGrammarCompilerContext Context, Redist.Parsers.SyntaxTreeNode Node)
        {
            Redist.Parsers.SymbolTokenText Token = ((Redist.Parsers.SymbolTokenText)Node.Children[0].Symbol);
            CFRuleDefinitionSet Defs = new CFRuleDefinitionSet();
            // Get the information
            string Name = Token.ValueText;
            int ParamCount = Node.Children[1].Children.Count;
            // check for meta-rule existence
            if (!Context.IsTemplateRule(Name, ParamCount))
            {
                log.Error("Compiler", "Meta-rule " + Name + " does not exist with " + ParamCount.ToString() + " parameters");
                Defs.Add(new CFRuleDefinition());
                return Defs;
            }
            // Recognize the parameters
            CFGrammarTemplateRuleParameters Parameters = new CFGrammarTemplateRuleParameters();
            foreach (Redist.Parsers.SyntaxTreeNode SymbolNode in Node.Children[1].Children)
                Parameters.Add(Compile_Recognize_rule_def_atom(Data, Context, SymbolNode)[0].Parts[0].Symbol);
            // Get the corresponding variable
            Variable Variable = Context.GetVariableFromMetaRule(Name, Parameters, Context);
            // Create the definition
            Defs.Add(new CFRuleDefinition(Variable));
            return Defs;
        }

        private CFRuleDefinitionSet Compile_Recognize_rule_def_atom(CFGrammar Data, CFGrammarCompilerContext Context, Redist.Parsers.SyntaxTreeNode Node)
        {
            if (Node.Symbol.Name == "rule_sym_action")
                return Compile_Recognize_rule_sym_action(Data, Node);
            if (Node.Symbol.Name == "rule_sym_virtual")
                return Compile_Recognize_rule_sym_virtual(Data, Node);
            if (Node.Symbol.Name == "rule_sym_ref_simple")
                return Compile_Recognize_rule_sym_ref_simple(Data, Context, Node);
            if (Node.Symbol.Name.StartsWith("rule_sym_ref_template"))
                return Compile_Recognize_rule_sym_ref_template(Data, Context, Node);
            if (Node.Symbol.Name == "grammar_text_terminal")
                return Compile_Recognize_grammar_text_terminal(Data, Node);
            if (Node.Symbol.Name == "grammar_bin_terminal")
                return Compile_Recognize_grammar_bin_terminal(Data, Node);
            return null;
        }
        public CFRuleDefinitionSet Compile_Recognize_rule_definition(CFGrammar Data, CFGrammarCompilerContext Context, Redist.Parsers.SyntaxTreeNode Node)
        {
            if (Node.Symbol is Redist.Parsers.SymbolTokenText)
            {
                Redist.Parsers.SymbolTokenText Token = (Redist.Parsers.SymbolTokenText)Node.Symbol;
                if (Token.ValueText == "?")
                {
                    CFRuleDefinitionSet SetInner = Compile_Recognize_rule_definition(Data, Context, Node.Children[0]);
                    SetInner.Insert(0, new CFRuleDefinition());
                    return SetInner;
                }
                else if (Token.ValueText == "*")
                {
                    CFRuleDefinitionSet SetInner = Compile_Recognize_rule_definition(Data, Context, Node.Children[0]);
                    Variable SubVar = Data.AddVariable(subruleHeadRadicalMultiplicity + Data.NextSID.ToString());
                    foreach (CFRuleDefinition Def in SetInner)
                        SubVar.AddRule(new CFRule(SubVar, Def, true));
                    CFRuleDefinitionSet SetVar = new CFRuleDefinitionSet();
                    SetVar.Add(new CFRuleDefinition(SubVar));
                    SetVar = SetVar * SetInner;
                    foreach (CFRuleDefinition Def in SetVar)
                        SubVar.AddRule(new CFRule(SubVar, Def, true));
                    SetVar = new CFRuleDefinitionSet();
                    SetVar.Add(new CFRuleDefinition());
                    SetVar.Add(new CFRuleDefinition(SubVar));
                    return SetVar;
                }
                else if (Token.ValueText == "+")
                {
                    CFRuleDefinitionSet SetInner = Compile_Recognize_rule_definition(Data, Context, Node.Children[0]);
                    Variable SubVar = Data.AddVariable(subruleHeadRadicalMultiplicity + Data.NextSID.ToString());
                    foreach (CFRuleDefinition Def in SetInner)
                        SubVar.AddRule(new CFRule(SubVar, Def, true));
                    CFRuleDefinitionSet SetVar = new CFRuleDefinitionSet();
                    SetVar.Add(new CFRuleDefinition(SubVar));
                    SetVar = SetVar * SetInner;
                    foreach (CFRuleDefinition Def in SetVar)
                        SubVar.AddRule(new CFRule(SubVar, Def, true));
                    SetVar = new CFRuleDefinitionSet();
                    SetVar.Add(new CFRuleDefinition(SubVar));
                    return SetVar;
                }
                else if (Token.ValueText == "^")
                {
                    CFRuleDefinitionSet SetInner = Compile_Recognize_rule_definition(Data, Context, Node.Children[0]);
                    SetInner.SetActionPromote();
                    return SetInner;
                }
                else if (Token.ValueText == "!")
                {
                    CFRuleDefinitionSet SetInner = Compile_Recognize_rule_definition(Data, Context, Node.Children[0]);
                    SetInner.SetActionDrop();
                    return SetInner;
                }
                else if (Token.ValueText == "|")
                {
                    CFRuleDefinitionSet SetLeft = Compile_Recognize_rule_definition(Data, Context, Node.Children[0]);
                    CFRuleDefinitionSet SetRight = Compile_Recognize_rule_definition(Data, Context, Node.Children[1]);
                    return (SetLeft + SetRight);
                }
                else if (Token.ValueText == "-")
                {
                    CFRuleDefinitionSet SetLeft = Compile_Recognize_rule_definition(Data, Context, Node.Children[0]);
                    CFRuleDefinitionSet SetRight = Compile_Recognize_rule_definition(Data, Context, Node.Children[1]);
                    Variable SubVar = Data.AddVariable(subruleHeadRadicalRestrict + Data.NextSID.ToString());
                    foreach (CFRuleDefinition Def in SetLeft)
                        SubVar.AddRule(new CFRule(SubVar, Def, true, SubVar.SID));
                    foreach (CFRuleDefinition Def in SetRight)
                        SubVar.AddRule(new CFRule(SubVar, Def, true, -SubVar.SID));
                    CFRuleDefinitionSet SetFinal = new CFRuleDefinitionSet();
                    SetFinal.Add(new CFRuleDefinition(SubVar));
                    return SetFinal;
                }
                return Compile_Recognize_rule_def_atom(Data, Context, Node);
            }
            else if (Node.Symbol.Name == "emptypart")
            {
                CFRuleDefinitionSet set = new CFRuleDefinitionSet();
                set.Add(new CFRuleDefinition());
                return set;
            }
            else if (Node.Symbol.Name == "concat")
            {
                CFRuleDefinitionSet SetLeft = Compile_Recognize_rule_definition(Data, Context, Node.Children[0]);
                CFRuleDefinitionSet SetRight = Compile_Recognize_rule_definition(Data, Context, Node.Children[1]);
                return (SetLeft * SetRight);
            }
            else
                return Compile_Recognize_rule_def_atom(Data, Context, Node);
        }
        private void Compile_Recognize_rule(CFGrammar Data, CFGrammarCompilerContext Context, Redist.Parsers.SyntaxTreeNode Node)
        {
            string Name = ((Redist.Parsers.SymbolTokenText)Node.Children[0].Symbol).ValueText;
            Variable Var = Data.GetVariable(Name);
            CFRuleDefinitionSet Defs = Compile_Recognize_rule_definition(Data, Context, Node.Children[1]);
            foreach (CFRuleDefinition Def in Defs)
                Var.AddRule(new CFRule(Var, Def, false));
        }

        private void Compile_Recognize_grammar_options(CFGrammar Data, Redist.Parsers.SyntaxTreeNode OptionsNode)
        {
            foreach (Redist.Parsers.SyntaxTreeNode Node in OptionsNode.Children)
                Compile_Recognize_option(Data, Node);
        }
        private void Compile_Recognize_grammar_terminals(CFGrammar Data, Redist.Parsers.SyntaxTreeNode TerminalsNode)
        {
            foreach (Redist.Parsers.SyntaxTreeNode Node in TerminalsNode.Children)
                Compile_Recognize_terminal(Data, Node);
        }
        private void Compile_Recognize_grammar_rules(CFGrammar Data, Redist.Parsers.SyntaxTreeNode RulesNode)
        {
            // Create a new context
            CFGrammarCompilerContext Context = new CFGrammarCompilerContext(this);
            // Add existing meta-rules that may have been inherited
            foreach (CFGrammarTemplateRule TemplateRule in Data.TemplateRules)
                Context.AddTemplateRule(TemplateRule);
            // Load new variables for the rules' head and the meta-rules themselves
            foreach (Redist.Parsers.SyntaxTreeNode Node in RulesNode.Children)
            {
                if (Node.Symbol.Name.StartsWith("cf_rule_simple"))
                {
                    string Name = ((Redist.Parsers.SymbolTokenText)Node.Children[0].Symbol).ValueText;
                    Variable Var = Data.GetVariable(Name);
                    if (Var == null)
                        Var = Data.AddVariable(Name);
                }
                else if (Node.Symbol.Name.StartsWith("cf_rule_template"))
                    Context.AddTemplateRule(Data.AddTemplateRule(Node, this));
            }
            // Load the grammar rules
            foreach (Redist.Parsers.SyntaxTreeNode Node in RulesNode.Children)
            {
                if (Node.Symbol.Name.StartsWith("cf_rule_simple"))
                    Compile_Recognize_rule(Data, Context, Node);
            }
        }

        private void Compile_Recognize_grammar_text(CFGrammar grammar, Redist.Parsers.SyntaxTreeNode node)
        {
            log.Info("Compiler", "Compiling grammar " + grammar.LocalName);
            log.Info("Compiler", "Grammar takes text as input");
            for (int i = 3; i < node.Children.Count; i++)
            {
                Redist.Parsers.SyntaxTreeNode child = node.Children[i];
                if (child.Symbol.Name == "options")
                    Compile_Recognize_grammar_options(grammar, child);
                else if (child.Symbol.Name == "terminals")
                    Compile_Recognize_grammar_terminals(grammar, child);
                else if (child.Symbol.Name == "rules")
                    Compile_Recognize_grammar_rules(grammar, child);
            }
        }
        private void Compile_Recognize_grammar_bin(CFGrammar grammar, Redist.Parsers.SyntaxTreeNode node)
        {
            log.Info("Compiler", "Compiling grammar " + grammar.LocalName);
            log.Info("Compiler", "Grammar takes binary as input");
            for (int i = 3; i < node.Children.Count; i++)
            {
                Redist.Parsers.SyntaxTreeNode child = node.Children[i];
                if (child.Symbol.Name == "options")
                    Compile_Recognize_grammar_options(grammar, child);
                else if (child.Symbol.Name == "rules")
                    Compile_Recognize_grammar_rules(grammar, child);
            }
        }
    }
}
