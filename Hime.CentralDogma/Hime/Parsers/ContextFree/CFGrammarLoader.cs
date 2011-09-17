/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;
using Hime.Kernel.Unicode;
using Hime.Kernel.Naming;
using Hime.Kernel.Resources;
using Hime.Kernel.Reporting;

namespace Hime.Parsers.ContextFree
{
    class CFGrammarLoader : LoaderPlugin
    {
        private static string[] resourcesNames = new string[] { "cf_grammar_text", "cf_grammar_bin" };

        private Reporter log;
        private bool hasErrors;

        public string Name { get { return (typeof(CFGrammarLoader)).FullName; } }
        public string[] ResourceNames { get { return resourcesNames; } }

        public CFGrammarLoader() { }

        public void CreateResource(Symbol container, Redist.Parsers.SyntaxTreeNode syntaxNode, ResourceGraph graph, Reporter log)
        {
            SymbolAccess access = ResourceLoader.CompileSymbolAccess(syntaxNode.Children[0]);
            string name = ((Redist.Parsers.SymbolTokenText)syntaxNode.Children[1].Symbol).ValueText;
            CFGrammar grammar = null;
            Resource resource = null;
            
            if (syntaxNode.Symbol.Name == "cf_grammar_text")
                grammar = new CFGrammarText(name);
            else if (syntaxNode.Symbol.Name == "cf_grammar_bin")
                grammar = new CFGrammarBinary(name);
            grammar.Access = access;
            resource = new Resource(grammar, syntaxNode, this);
            container.SymbolAddChild(grammar);
            graph.AddResource(resource);
        }
        public void CreateDependencies(Resource resource, ResourceGraph graph, Reporter log)
        {
            foreach (Redist.Parsers.SyntaxTreeNode Parent in resource.SyntaxNode.Children[2].Children)
            {
                QualifiedName Name = ResourceLoader.CompileQualifiedName(Parent);
                Symbol Symbol = resource.Symbol.ResolveName(Name);
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
                        foreach (Redist.Parsers.SyntaxTreeNode terminal in node.Children)
                        {
                            if (terminal.Children[2].Children.Count == 1)
                            {
                                QualifiedName Name = ResourceLoader.CompileQualifiedName(terminal.Children[2].Children[0]);
                                Symbol Symbol = resource.Symbol.ResolveName(Name);
                                if (Symbol is Grammar)
                                {
                                    bool found = false;
                                    foreach (KeyValuePair<string, Resource> dependency in resource.Dependencies)
                                    {
                                        if (dependency.Value.Symbol == Symbol)
                                        {
                                            found = true;
                                            break;
                                        }
                                    }
                                    if (!found)
                                        resource.AddDependency("subgrammar", graph.GetResource(Symbol));
                                }
                            }
                        }
                    }
                }
            }
        }
        public int CompileSolveDependencies(Resource resource, Reporter log)
        {
            CFGrammar grammar = resource.Symbol as CFGrammar;
            int Solved = 0;
            for (int i = 0; i != resource.Dependencies.Count; i++)
            {
                if (resource.Dependencies[i].Value.IsLoaded)
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
        public bool Compile(Resource resource, Reporter log)
        {
            this.log = log;
            this.hasErrors = false;
            if (resource.Symbol is CFGrammarText)
            {
                Compile_Recognize_grammar_text((CFGrammar)resource.Symbol, resource.SyntaxNode);
                resource.IsLoaded = true;
            }
            else if (resource.Symbol is CFGrammarBinary)
            {
                Compile_Recognize_grammar_bin((CFGrammar)resource.Symbol, resource.SyntaxNode);
                resource.IsLoaded = true;
            }
            return (!hasErrors);
        }



        private GrammarSymbol Compile_Tool_NameToSymbol(string name, CFGrammar data, CompilerContext context)
        {
            if (context.IsReference(name))
                return context.GetReference(name);
            return data.GetSymbol(name);
        }

        private void Compile_Recognize_option(CFGrammar data, Redist.Parsers.SyntaxTreeNode node)
        {
            string Name = ((Redist.Parsers.SymbolTokenText)node.Children[0].Symbol).ValueText;
            string Value = ((Redist.Parsers.SymbolTokenText)node.Children[1].Symbol).ValueText;
            Value = Value.Substring(1, Value.Length - 2);
            data.AddOption(Name, Value);
        }

        private Automata.NFA Compile_Recognize_terminal_def_atom_any(Redist.Parsers.SyntaxTreeNode node)
        {
            Automata.NFA Final = new Hime.Parsers.Automata.NFA();
            Final.StateEntry = Final.AddNewState();
            Final.StateExit = Final.AddNewState();
            char begin = System.Convert.ToChar(0x0000);
            char end = System.Convert.ToChar(0xFFFF);
            Final.StateEntry.AddTransition(new Automata.CharSpan(begin, end), Final.StateExit);
            return Final;
        }
        private Automata.NFA Compile_Recognize_terminal_def_atom_unicode(Redist.Parsers.SyntaxTreeNode node)
        {
            Automata.NFA Final = new Hime.Parsers.Automata.NFA();
            Final.StateEntry = Final.AddNewState();
            Final.StateExit = Final.AddNewState();
            string value = ((Redist.Parsers.SymbolTokenText)node.Symbol).ValueText;
            value = value.Substring(2, value.Length - 2);
            int charInt = System.Convert.ToInt32(value, 16);
            char c = System.Convert.ToChar(charInt);
            Final.StateEntry.AddTransition(new Automata.CharSpan(c, c), Final.StateExit);
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
        private Automata.NFA Compile_Recognize_terminal_def_atom_text(Redist.Parsers.SyntaxTreeNode node)
        {
            Automata.NFA final = new Hime.Parsers.Automata.NFA();
            final.StateEntry = final.AddNewState();
            final.StateExit = final.StateEntry;
            string value = ((Redist.Parsers.SymbolTokenText)node.Symbol).ValueText;
            value = value.Substring(1, value.Length - 2);
            value = ReplaceEscapees(value).Replace("\\'", "'");
            foreach (char c in value)
            {
                Automata.NFAState Temp = final.AddNewState();
                final.StateExit.AddTransition(new Automata.CharSpan(c, c), Temp);
                final.StateExit = Temp;
            }
            return final;
        }
        private Automata.NFA Compile_Recognize_terminal_def_atom_set(Redist.Parsers.SyntaxTreeNode node)
        {
            Automata.NFA final = new Hime.Parsers.Automata.NFA();
            final.StateEntry = final.AddNewState();
            final.StateExit = final.AddNewState();
            string value = ((Redist.Parsers.SymbolTokenText)node.Symbol).ValueText;
            value = value.Substring(1, value.Length - 2);
            value = ReplaceEscapees(value).Replace("\\[", "[").Replace("\\]", "]");
            bool positive = true;
            if (value[0] == '^')
            {
                value = value.Substring(1);
                positive = false;
            }
            List<Automata.CharSpan> spans = new List<Automata.CharSpan>();
            for (int i = 0; i != value.Length; i++)
            {
                if ((i != value.Length - 1) && (value[i + 1] == '-'))
                {
                    spans.Add(new Automata.CharSpan(value[i], value[i + 2]));
                    i += 2;
                }
                else
                    spans.Add(new Automata.CharSpan(value[i], value[i]));
            }
            if (positive)
            {
                foreach (Automata.CharSpan span in spans)
                    final.StateEntry.AddTransition(span, final.StateExit);
            }
            else
            {
                spans.Sort(new System.Comparison<Automata.CharSpan>(Automata.CharSpan.Compare));
                char b = char.MinValue;
                for (int i = 0; i != spans.Count; i++)
                {
                    if (spans[i].Begin > b)
                        final.StateEntry.AddTransition(new Automata.CharSpan(b, System.Convert.ToChar(spans[i].Begin - 1)), final.StateExit);
                    b = System.Convert.ToChar(spans[i].End + 1);
                }
                final.StateEntry.AddTransition(new Automata.CharSpan(b, char.MaxValue), final.StateExit);
            }
            return final;
        }
        private Automata.NFA Compile_Recognize_terminal_def_atom_ublock(Redist.Parsers.SyntaxTreeNode node)
        {
            Automata.NFA final = new Hime.Parsers.Automata.NFA();
            final.StateEntry = final.AddNewState();
            final.StateExit = final.AddNewState();
            Redist.Parsers.SymbolTokenText token = (Redist.Parsers.SymbolTokenText)node.Symbol;
            string value = token.ValueText.Substring(4, token.ValueText.Length - 5);
            UnicodeBlock block = UnicodeBlock.Categories[value];
            // Create transition and return
            final.StateEntry.AddTransition(new Automata.CharSpan(System.Convert.ToChar(block.Begin), System.Convert.ToChar(block.End)), final.StateExit);
            return final;
        }
        private Automata.NFA Compile_Recognize_terminal_def_atom_ucat(Redist.Parsers.SyntaxTreeNode node)
        {
            Automata.NFA final = new Hime.Parsers.Automata.NFA();
            final.StateEntry = final.AddNewState();
            final.StateExit = final.AddNewState();
            Redist.Parsers.SymbolTokenText token = (Redist.Parsers.SymbolTokenText)node.Symbol;
            string value = token.ValueText.Substring(4, token.ValueText.Length - 5);
            UnicodeCategory category = UnicodeCategory.Classes[value];
            // Create transitions and return
            foreach (UnicodeSpan span in category.Spans)
                final.StateEntry.AddTransition(new Automata.CharSpan(System.Convert.ToChar(span.Begin), System.Convert.ToChar(span.End)), final.StateExit);
            return final;
        }
        private Automata.NFA Compile_Recognize_terminal_def_atom_span(Redist.Parsers.SyntaxTreeNode node)
        {
            Automata.NFA final = new Hime.Parsers.Automata.NFA();
            final.StateEntry = final.AddNewState();
            final.StateExit = final.AddNewState();
            int spanBegin = 0;
            int spanEnd = 0;
            // Get span begin
            Redist.Parsers.SymbolTokenText child = (Redist.Parsers.SymbolTokenText)node.Children[0].Symbol;
            spanBegin = System.Convert.ToInt32(child.ValueText.Substring(2), 16);
            // Get span end
            child = (Redist.Parsers.SymbolTokenText)node.Children[1].Symbol;
            spanEnd = System.Convert.ToInt32(child.ValueText.Substring(2), 16);
            // If span end is before beginning: reverse
            if (spanBegin > spanEnd)
            {
                int Temp = spanEnd;
                spanEnd = spanBegin;
                spanBegin = Temp;
            }
            // Create transition and return
            final.StateEntry.AddTransition(new Automata.CharSpan(System.Convert.ToChar(spanBegin), System.Convert.ToChar(spanEnd)), final.StateExit);
            return final;
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
        private Automata.NFA Compile_Recognize_terminal_definition(CFGrammar Data, Redist.Parsers.SyntaxTreeNode node)
        {
            // Symbol is a token
            if (node.Symbol is Redist.Parsers.SymbolTokenText)
            {
                Redist.Parsers.SymbolTokenText token = (Redist.Parsers.SymbolTokenText)node.Symbol;
                if (token.ValueText == "?")
                {
                    Automata.NFA inner = Compile_Recognize_terminal_definition(Data, node.Children[0]);
                    return Automata.NFA.OperatorOption(inner, false);
                }
                if (token.ValueText == "*")
                {
                    Automata.NFA inner = Compile_Recognize_terminal_definition(Data, node.Children[0]);
                    return Automata.NFA.OperatorStar(inner, false);
                }
                if (token.ValueText == "+")
                {
                    Automata.NFA inner = Compile_Recognize_terminal_definition(Data, node.Children[0]);
                    return Automata.NFA.OperatorPlus(inner, false);
                }
                if (token.ValueText == "|")
                {
                    Automata.NFA left = Compile_Recognize_terminal_definition(Data, node.Children[0]);
                    Automata.NFA right = Compile_Recognize_terminal_definition(Data, node.Children[1]);
                    return Automata.NFA.OperatorUnion(left, right, false);
                }
                if (token.ValueText == "-")
                {
                    Automata.NFA left = Compile_Recognize_terminal_definition(Data, node.Children[0]);
                    Automata.NFA right = Compile_Recognize_terminal_definition(Data, node.Children[1]);
                    return Automata.NFA.OperatorDifference(left, right, false);
                }
                if (token.ValueText == ".")
                    return Compile_Recognize_terminal_def_atom_any(node);
                if (token.Name == "SYMBOL_VALUE_UINT8")
                    return Compile_Recognize_terminal_def_atom_unicode(node);
                if (token.Name == "SYMBOL_VALUE_UINT16")
                    return Compile_Recognize_terminal_def_atom_unicode(node);
                if (token.Name == "SYMBOL_TERMINAL_TEXT")
                    return Compile_Recognize_terminal_def_atom_text(node);
                if (token.Name == "SYMBOL_TERMINAL_SET")
                    return Compile_Recognize_terminal_def_atom_set(node);
                if (token.Name == "SYMBOL_TERMINAL_UCAT")
                    return Compile_Recognize_terminal_def_atom_ucat(node);
                if (token.Name == "SYMBOL_TERMINAL_UBLOCK")
                    return Compile_Recognize_terminal_def_atom_ublock(node);
                if (token.ValueText == "..")
                    return Compile_Recognize_terminal_def_atom_span(node);
                if (token.Name == "NAME")
                    return Compile_Recognize_terminal_def_atom_name(Data, node);
                Automata.NFA final = new Hime.Parsers.Automata.NFA();
                final.StateEntry = final.AddNewState();
                final.StateExit = final.AddNewState();
                final.StateEntry.AddTransition(Automata.NFA.Epsilon, final.StateExit);
                return final;
            }
            else if (node.Symbol is Redist.Parsers.SymbolVirtual)
            {
                Redist.Parsers.SymbolVirtual token = (Redist.Parsers.SymbolVirtual)node.Symbol;
                if (node.Symbol.Name == "range")
                {
                    Automata.NFA inner = Compile_Recognize_terminal_definition(Data, node.Children[0]);
                    uint min = System.Convert.ToUInt32(((Redist.Parsers.SymbolTokenText)node.Children[1].Symbol).ValueText);
                    uint max = min;
                    if (node.Children.Count > 2)
                        max = System.Convert.ToUInt32(((Redist.Parsers.SymbolTokenText)node.Children[2].Symbol).ValueText);
                    return Automata.NFA.OperatorRange(inner, false, min, max);
                }
                else if (node.Symbol.Name == "concat")
                {
                    Automata.NFA left = Compile_Recognize_terminal_definition(Data, node.Children[0]);
                    Automata.NFA right = Compile_Recognize_terminal_definition(Data, node.Children[1]);
                    return Automata.NFA.OperatorConcat(left, right, false);
                }
            }
            return null;
        }
        private Terminal Compile_Recognize_terminal(CFGrammar data, Redist.Parsers.SyntaxTreeNode node)
        {
            string name = ((Redist.Parsers.SymbolTokenText)node.Children[0].Symbol).ValueText;
            Automata.NFA nfa = Compile_Recognize_terminal_definition(data, node.Children[1]);
            Grammar subGrammar = null;
            if (node.Children[2].Children.Count == 1)
            {
                QualifiedName subGramName = ResourceLoader.CompileQualifiedName(node.Children[2].Children[0]);
                subGrammar = (Grammar)data.ResolveName(subGramName);
            }
            Terminal terminal = data.AddTerminalText(name, nfa, subGrammar);
            ((TerminalText)terminal).SubGrammar = subGrammar;
            nfa.StateExit.Final = terminal;
            return terminal;
        }


        private CFRuleBodySet Compile_Recognize_grammar_bin_terminal_data(CFGrammar data, Redist.Parsers.SyntaxTreeNode node)
        {
            CFRuleBodySet set = new CFRuleBodySet();
            string name = ((Redist.Parsers.SymbolTokenText)node.Symbol).ValueText;
            Terminal terminal = data.GetTerminal(name);
            if (terminal == null)
                terminal = data.AddTerminalBin((TerminalBinType)System.Enum.Parse(typeof(TerminalBinType), node.Symbol.Name), name);
            set.Add(new CFRuleBody(terminal));
            return set;
        }
        private CFRuleBodySet Compile_Recognize_grammar_bin_terminal(CFGrammar data, Redist.Parsers.SyntaxTreeNode node)
        {
            node = node.Children[0];
            if (node.Symbol.Name == "SYMBOL_VALUE_UINT8")
                return Compile_Recognize_grammar_bin_terminal_data(data, node);
            if (node.Symbol.Name == "SYMBOL_VALUE_UINT16")
                return Compile_Recognize_grammar_bin_terminal_data(data, node);
            if (node.Symbol.Name == "SYMBOL_VALUE_UINT32")
                return Compile_Recognize_grammar_bin_terminal_data(data, node);
            if (node.Symbol.Name == "SYMBOL_VALUE_UINT64")
                return Compile_Recognize_grammar_bin_terminal_data(data, node);
            if (node.Symbol.Name == "SYMBOL_VALUE_UINT128")
                return Compile_Recognize_grammar_bin_terminal_data(data, node);
            if (node.Symbol.Name == "SYMBOL_VALUE_BINARY")
                return Compile_Recognize_grammar_bin_terminal_data(data, node);
            if (node.Symbol.Name == "SYMBOL_JOKER_UINT8")
                return Compile_Recognize_grammar_bin_terminal_data(data, node);
            if (node.Symbol.Name == "SYMBOL_JOKER_UINT16")
                return Compile_Recognize_grammar_bin_terminal_data(data, node);
            if (node.Symbol.Name == "SYMBOL_JOKER_UINT32")
                return Compile_Recognize_grammar_bin_terminal_data(data, node);
            if (node.Symbol.Name == "SYMBOL_JOKER_UINT64")
                return Compile_Recognize_grammar_bin_terminal_data(data, node);
            if (node.Symbol.Name == "SYMBOL_JOKER_UINT128")
                return Compile_Recognize_grammar_bin_terminal_data(data, node);
            if (node.Symbol.Name == "SYMBOL_JOKER_BINARY")
                return Compile_Recognize_grammar_bin_terminal_data(data, node);
            return new CFRuleBodySet();
        }
        private CFRuleBodySet Compile_Recognize_grammar_text_terminal(CFGrammar data, Redist.Parsers.SyntaxTreeNode node)
        {
            // Construct the terminal name
            string value = ((Redist.Parsers.SymbolTokenText)node.Children[0].Symbol).ValueText;
            value = value.Substring(1, value.Length - 2);
            value = "@\"" + value.Replace("\\'", "'").Replace("\\\\", "\\") + "\"";
            // Check for previous instance in the grammar's data
            Terminal terminal = data.GetTerminal(value);
            if (terminal == null)
            {
                // Create the terminal
                Automata.NFA nfa = Compile_Recognize_terminal_def_atom_text(node.Children[0]);
                terminal = data.AddTerminalText(value, nfa, null);
                nfa.StateExit.Final = terminal;
            }
            // Create the definition set
            CFRuleBodySet set = new CFRuleBodySet();
            set.Add(new CFRuleBody(terminal));
            return set;
        }

        private CFRuleBodySet Compile_Recognize_rule_sym_action(CFGrammar data, Redist.Parsers.SyntaxTreeNode node)
        {
            CFRuleBodySet set = new CFRuleBodySet();
            string name = ((Redist.Parsers.SymbolTokenText)node.Children[0].Symbol).ValueText;
            Action action = data.GetAction(name);
            if (action == null)
                action = data.AddAction(name);
            set.Add(new CFRuleBody(action));
            return set;
        }
        private CFRuleBodySet Compile_Recognize_rule_sym_virtual(CFGrammar data, Redist.Parsers.SyntaxTreeNode node)
        {
            CFRuleBodySet set = new CFRuleBodySet();
            string name = ((Redist.Parsers.SymbolTokenText)node.Children[0].Symbol).ValueText;
            name = name.Substring(1, name.Length - 2);
            Virtual vir = data.GetVirtual(name);
            if (vir == null)
                vir = data.AddVirtual(name);
            set.Add(new CFRuleBody(vir));
            return set;
        }
        private CFRuleBodySet Compile_Recognize_rule_sym_ref_simple(CFGrammar data, CompilerContext context, Redist.Parsers.SyntaxTreeNode node)
        {
            Redist.Parsers.SymbolTokenText token = ((Redist.Parsers.SymbolTokenText)node.Children[0].Symbol);
            CFRuleBodySet defs = new CFRuleBodySet();
            if (token.ValueText == "ε")
            {
                defs.Add(new CFRuleBody());
            }
            else
            {
                GrammarSymbol symbol = Compile_Tool_NameToSymbol(token.ValueText, data, context);
                if (symbol != null)
                    defs.Add(new CFRuleBody(symbol));
                else
                {
                    log.Error("Compiler", "@" + token.Line + " Unrecognized symbol " + token.ValueText + " in rule definition");
                    hasErrors = true;
                    defs.Add(new CFRuleBody());
                }
            }
            return defs;
        }
        private CFRuleBodySet Compile_Recognize_rule_sym_ref_template(CFGrammar data, CompilerContext context, Redist.Parsers.SyntaxTreeNode node)
        {
            Redist.Parsers.SymbolTokenText token = ((Redist.Parsers.SymbolTokenText)node.Children[0].Symbol);
            CFRuleBodySet defs = new CFRuleBodySet();
            // Get the information
            string name = token.ValueText;
            int paramCount = node.Children[1].Children.Count;
            // check for meta-rule existence
            if (!context.IsTemplateRule(name, paramCount))
            {
                log.Error("Compiler", "Meta-rule " + name + " does not exist with " + paramCount.ToString() + " parameters");
                defs.Add(new CFRuleBody());
                return defs;
            }
            // Recognize the parameters
            List<GrammarSymbol> parameters = new List<GrammarSymbol>();
            foreach (Redist.Parsers.SyntaxTreeNode symbolNode in node.Children[1].Children)
                parameters.Add(Compile_Recognize_rule_def_atom(data, context, symbolNode)[0].Parts[0].Symbol);
            // Get the corresponding variable
            Variable variable = context.GetVariableFromMetaRule(name, parameters, context);
            // Create the definition
            defs.Add(new CFRuleBody(variable));
            return defs;
        }

        private CFRuleBodySet Compile_Recognize_rule_def_atom(CFGrammar data, CompilerContext context, Redist.Parsers.SyntaxTreeNode node)
        {
            if (node.Symbol.Name == "rule_sym_action")
                return Compile_Recognize_rule_sym_action(data, node);
            if (node.Symbol.Name == "rule_sym_virtual")
                return Compile_Recognize_rule_sym_virtual(data, node);
            if (node.Symbol.Name == "rule_sym_ref_simple")
                return Compile_Recognize_rule_sym_ref_simple(data, context, node);
            if (node.Symbol.Name.StartsWith("rule_sym_ref_template"))
                return Compile_Recognize_rule_sym_ref_template(data, context, node);
            if (node.Symbol.Name == "grammar_text_terminal")
                return Compile_Recognize_grammar_text_terminal(data, node);
            if (node.Symbol.Name == "grammar_bin_terminal")
                return Compile_Recognize_grammar_bin_terminal(data, node);
            return null;
        }
        public CFRuleBodySet Compile_Recognize_rule_definition(CFGrammar data, CompilerContext context, Redist.Parsers.SyntaxTreeNode node)
        {
            if (node.Symbol is Redist.Parsers.SymbolTokenText)
            {
                Redist.Parsers.SymbolTokenText token = (Redist.Parsers.SymbolTokenText)node.Symbol;
                if (token.ValueText == "?")
                {
                    CFRuleBodySet setInner = Compile_Recognize_rule_definition(data, context, node.Children[0]);
                    setInner.Insert(0, new CFRuleBody());
                    return setInner;
                }
                else if (token.ValueText == "*")
                {
                    CFRuleBodySet setInner = Compile_Recognize_rule_definition(data, context, node.Children[0]);
                    CFVariable subVar = data.NewCFVariable();
                    foreach (CFRuleBody def in setInner)
                        subVar.AddRule(new CFRule(subVar, def, true));
                    CFRuleBodySet setVar = new CFRuleBodySet();
                    setVar.Add(new CFRuleBody(subVar));
                    setVar = setVar * setInner;
                    foreach (CFRuleBody def in setVar)
                        subVar.AddRule(new CFRule(subVar, def, true));
                    setVar = new CFRuleBodySet();
                    setVar.Add(new CFRuleBody());
                    setVar.Add(new CFRuleBody(subVar));
                    return setVar;
                }
                else if (token.ValueText == "+")
                {
                    CFRuleBodySet setInner = Compile_Recognize_rule_definition(data, context, node.Children[0]);
                    CFVariable subVar = data.NewCFVariable();
                    foreach (CFRuleBody def in setInner)
                        subVar.AddRule(new CFRule(subVar, def, true));
                    CFRuleBodySet setVar = new CFRuleBodySet();
                    setVar.Add(new CFRuleBody(subVar));
                    setVar = setVar * setInner;
                    foreach (CFRuleBody Def in setVar)
                        subVar.AddRule(new CFRule(subVar, Def, true));
                    setVar = new CFRuleBodySet();
                    setVar.Add(new CFRuleBody(subVar));
                    return setVar;
                }
                else if (token.ValueText == "^")
                {
                    CFRuleBodySet setInner = Compile_Recognize_rule_definition(data, context, node.Children[0]);
                    setInner.SetActionPromote();
                    return setInner;
                }
                else if (token.ValueText == "!")
                {
                    CFRuleBodySet setInner = Compile_Recognize_rule_definition(data, context, node.Children[0]);
                    setInner.SetActionDrop();
                    return setInner;
                }
                else if (token.ValueText == "|")
                {
                    CFRuleBodySet setLeft = Compile_Recognize_rule_definition(data, context, node.Children[0]);
                    CFRuleBodySet setRight = Compile_Recognize_rule_definition(data, context, node.Children[1]);
                    return (setLeft + setRight);
                }
                else if (token.ValueText == "-")
                {
                    CFRuleBodySet setLeft = Compile_Recognize_rule_definition(data, context, node.Children[0]);
                    CFRuleBodySet setRight = Compile_Recognize_rule_definition(data, context, node.Children[1]);
                    CFVariable subVar = data.NewCFVariable();
                    foreach (CFRuleBody def in setLeft)
                        subVar.AddRule(new CFRule(subVar, def, true, subVar.SID));
                    foreach (CFRuleBody def in setRight)
                        subVar.AddRule(new CFRule(subVar, def, true, -subVar.SID));
                    CFRuleBodySet setFinal = new CFRuleBodySet();
                    setFinal.Add(new CFRuleBody(subVar));
                    return setFinal;
                }
                return Compile_Recognize_rule_def_atom(data, context, node);
            }
            else if (node.Symbol.Name == "emptypart")
            {
                CFRuleBodySet set = new CFRuleBodySet();
                set.Add(new CFRuleBody());
                return set;
            }
            else if (node.Symbol.Name == "concat")
            {
                CFRuleBodySet setLeft = Compile_Recognize_rule_definition(data, context, node.Children[0]);
                CFRuleBodySet setRight = Compile_Recognize_rule_definition(data, context, node.Children[1]);
                return (setLeft * setRight);
            }
            else
                return Compile_Recognize_rule_def_atom(data, context, node);
        }
        private void Compile_Recognize_rule(CFGrammar data, CompilerContext context, Redist.Parsers.SyntaxTreeNode node)
        {
            string name = ((Redist.Parsers.SymbolTokenText)node.Children[0].Symbol).ValueText;
            CFVariable var = data.GetCFVariable(name);
            CFRuleBodySet defs = Compile_Recognize_rule_definition(data, context, node.Children[1]);
            foreach (CFRuleBody def in defs)
                var.AddRule(new CFRule(var, def, false));
        }

        private void Compile_Recognize_grammar_options(CFGrammar data, Redist.Parsers.SyntaxTreeNode optionsNode)
        {
            foreach (Redist.Parsers.SyntaxTreeNode node in optionsNode.Children)
                Compile_Recognize_option(data, node);
        }
        private void Compile_Recognize_grammar_terminals(CFGrammar data, Redist.Parsers.SyntaxTreeNode terminalsNode)
        {
            foreach (Redist.Parsers.SyntaxTreeNode node in terminalsNode.Children)
                Compile_Recognize_terminal(data, node);
        }
        private void Compile_Recognize_grammar_rules(CFGrammar data, Redist.Parsers.SyntaxTreeNode rulesNode)
        {
            // Create a new context
            CompilerContext context = new CompilerContext(this);
            // Add existing meta-rules that may have been inherited
            foreach (TemplateRule templateRule in data.TemplateRules)
                context.AddTemplateRule(templateRule);
            // Load new variables for the rules' head
            foreach (Redist.Parsers.SyntaxTreeNode node in rulesNode.Children)
            {
                if (node.Symbol.Name.StartsWith("cf_rule_simple"))
                {
                    string name = ((Redist.Parsers.SymbolTokenText)node.Children[0].Symbol).ValueText;
                    Variable var = data.GetVariable(name);
                    if (var == null)
                        var = data.AddVariable(name);
                }
                else if (node.Symbol.Name.StartsWith("cf_rule_template"))
                    context.AddTemplateRule(data.AddTemplateRule(node));
            }
            // Load the grammar rules
            foreach (Redist.Parsers.SyntaxTreeNode node in rulesNode.Children)
            {
                if (node.Symbol.Name.StartsWith("cf_rule_simple"))
                    Compile_Recognize_rule(data, context, node);
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
