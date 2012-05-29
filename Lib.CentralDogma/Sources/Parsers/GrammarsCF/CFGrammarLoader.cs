/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;
using Hime.Utils.Unicode;
using Hime.Utils.Resources;
using Hime.Utils.Reporting;
using Hime.Redist.Parsers;

namespace Hime.Parsers.ContextFree
{
    class CFGrammarLoader : GrammarLoader
    {
        private Reporter reporter;
        private SyntaxTreeNode syntaxRoot;
        private string name;
        private List<string> inherited;
        private CFGrammar grammar;

        public string Name { get { return name; } }
        public Grammar Grammar { get { return grammar; } }
        public bool IsSolved { get { return (inherited.Count == 0); } }

        public CFGrammarLoader(SyntaxTreeNode root, Reporter reporter)
        {
            this.reporter = reporter;
            this.syntaxRoot = root;
            this.name = ((SymbolTokenText)root.Children[0].Symbol).ValueText;
            this.inherited = new List<string>();
            foreach (SyntaxTreeNode child in syntaxRoot.Children[1].Children)
                inherited.Add(((SymbolTokenText)child.Symbol).ValueText);
            reporter.Info("Compiler", "Loading grammar " + name);
            this.grammar = new CFGrammar(name);
            if (inherited.Count == 0)
                Compile_Recognize_grammar_text(syntaxRoot);
        }

        public void Load(Dictionary<string, GrammarLoader> loaders)
        {
            List<string> temp = new List<string>(inherited);
            foreach (string parent in temp)
            {
                if (!loaders.ContainsKey(parent))
                {
                    reporter.Fatal("Compiler", "Grammar " + parent + " inherited by " + name + " cannot be found");
                    inherited.Remove(parent);
                }
                GrammarLoader loader = loaders[parent];
                if (!loader.IsSolved)
                    continue;
                this.grammar.Inherit(loader.Grammar);
                inherited.Remove(parent);
            }
            if (inherited.Count == 0)
                Compile_Recognize_grammar_text(syntaxRoot);
        }

        private GrammarSymbol Compile_Tool_NameToSymbol(string name, CompilerContext context)
        {
            if (context.IsReference(name))
                return context.GetReference(name);
            return grammar.GetSymbol(name);
        }

        private void Compile_Recognize_option(SyntaxTreeNode node)
        {
            string Name = ((Redist.Parsers.SymbolTokenText)node.Children[0].Symbol).ValueText;
            string Value = ((Redist.Parsers.SymbolTokenText)node.Children[1].Symbol).ValueText;
            Value = Value.Substring(1, Value.Length - 2);
            grammar.AddOption(Name, Value);
        }

        private Automata.NFA Compile_Recognize_terminal_def_atom_any(SyntaxTreeNode node)
        {
            Automata.NFA Final = new Hime.Parsers.Automata.NFA();
            Final.StateEntry = Final.AddNewState();
            Final.StateExit = Final.AddNewState();
            char begin = System.Convert.ToChar(0x0000);
            char end = System.Convert.ToChar(0xFFFF);
            Final.StateEntry.AddTransition(new Automata.CharSpan(begin, end), Final.StateExit);
            return Final;
        }
        private Automata.NFA Compile_Recognize_terminal_def_atom_unicode(SyntaxTreeNode node)
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
        private Automata.NFA Compile_Recognize_terminal_def_atom_text(SyntaxTreeNode node)
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
        private Automata.NFA Compile_Recognize_terminal_def_atom_set(SyntaxTreeNode node)
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
        private Automata.NFA Compile_Recognize_terminal_def_atom_ublock(SyntaxTreeNode node)
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
        private Automata.NFA Compile_Recognize_terminal_def_atom_ucat(SyntaxTreeNode node)
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
        private Automata.NFA Compile_Recognize_terminal_def_atom_span(SyntaxTreeNode node)
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
        private Automata.NFA Compile_Recognize_terminal_def_atom_name(SyntaxTreeNode node)
        {
            Redist.Parsers.SymbolTokenText token = (Redist.Parsers.SymbolTokenText)node.Symbol;
            Terminal Ref = grammar.GetTerminal(token.ValueText);
            if (Ref == null)
            {
                reporter.Error("Compiler", "@" + token.Line + " Cannot find terminal " + token.ValueText);
                Automata.NFA Final = new Hime.Parsers.Automata.NFA();
                Final.StateEntry = Final.AddNewState();
                Final.StateExit = Final.AddNewState();
                Final.StateEntry.AddTransition(Automata.NFA.Epsilon, Final.StateExit);
                return Final;
            }
            return ((TerminalText)Ref).NFA.Clone(false);
        }
		
        private Automata.NFA Compile_Recognize_terminal_definition(SyntaxTreeNode node)
        {
			Redist.Parsers.Symbol symbol = node.Symbol;
			
            // Symbol is a token
			// TODO: this check is not nice, 
			// also it is strange to use here some many Redist.Parsers objects 
			// (shouldn't the work be done in the other namespace?)
			if (symbol is Redist.Parsers.SymbolTokenText)
            {
                Redist.Parsers.SymbolTokenText token = (Redist.Parsers.SymbolTokenText)node.Symbol;
                if (token.ValueText == "?")
                {
                    Automata.NFA inner = Compile_Recognize_terminal_definition(node.Children[0]);
                    return Automata.NFA.OperatorOption(inner, false);
                }
                if (token.ValueText == "*")
                {
                    Automata.NFA inner = Compile_Recognize_terminal_definition(node.Children[0]);
                    return Automata.NFA.OperatorStar(inner, false);
                }
                if (token.ValueText == "+")
                {
                    Automata.NFA inner = Compile_Recognize_terminal_definition(node.Children[0]);
                    return Automata.NFA.OperatorPlus(inner, false);
                }
                if (token.ValueText == "|")
                {
                    Automata.NFA left = Compile_Recognize_terminal_definition(node.Children[0]);
                    Automata.NFA right = Compile_Recognize_terminal_definition(node.Children[1]);
                    return Automata.NFA.OperatorUnion(left, right, false);
                }
                if (token.ValueText == "-")
                {
                    Automata.NFA left = Compile_Recognize_terminal_definition(node.Children[0]);
                    Automata.NFA right = Compile_Recognize_terminal_definition(node.Children[1]);
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
                    return Compile_Recognize_terminal_def_atom_name(node);
                Automata.NFA final = new Hime.Parsers.Automata.NFA();
                final.StateEntry = final.AddNewState();
                final.StateExit = final.AddNewState();
                final.StateEntry.AddTransition(Automata.NFA.Epsilon, final.StateExit);
                return final;
            }
			// TODO: this cast is not nice!!
            else if (symbol is Redist.Parsers.SymbolVirtual)
            {
                if (symbol.Name == "range")
                {
                    Automata.NFA inner = Compile_Recognize_terminal_definition(node.Children[0]);
                    uint min = System.Convert.ToUInt32(((Redist.Parsers.SymbolTokenText)node.Children[1].Symbol).ValueText);
                    uint max = min;
                    if (node.Children.Count > 2)
                        max = System.Convert.ToUInt32(((Redist.Parsers.SymbolTokenText)node.Children[2].Symbol).ValueText);
                    return Automata.NFA.OperatorRange(inner, false, min, max);
                }
                if (symbol.Name == "concat")
                {
                    Automata.NFA left = Compile_Recognize_terminal_definition(node.Children[0]);
                    Automata.NFA right = Compile_Recognize_terminal_definition(node.Children[1]);
                    return Automata.NFA.OperatorConcat(left, right, false);
                }
            }
            return null;
        }
		
        private Terminal Compile_Recognize_terminal(SyntaxTreeNode node)
        {
            string name = ((Redist.Parsers.SymbolTokenText)node.Children[0].Symbol).ValueText;
            Automata.NFA nfa = Compile_Recognize_terminal_definition(node.Children[1]);
            Terminal terminal = grammar.AddTerminalText(name, nfa);
            nfa.StateExit.Final = terminal;
            return terminal;
        }

        private CFRuleBodySet Compile_Recognize_grammar_text_terminal(SyntaxTreeNode node)
        {
            // Construct the terminal name
            string value = ((Redist.Parsers.SymbolTokenText)node.Symbol).ValueText;
            value = value.Substring(1, value.Length - 2);
            value = ReplaceEscapees(value).Replace("\\'", "'");
            // Check for previous instance in the grammar's grammar
            Terminal terminal = grammar.GetTerminal(value);
            if (terminal == null)
            {
                // Create the terminal
                Automata.NFA nfa = Compile_Recognize_terminal_def_atom_text(node);
                terminal = grammar.AddTerminalText(value, nfa);
                nfa.StateExit.Final = terminal;
            }
            // Create the definition set
            CFRuleBodySet set = new CFRuleBodySet();
            set.Add(new CFRuleBody(terminal));
            return set;
        }

        private CFRuleBodySet Compile_Recognize_rule_sym_action(SyntaxTreeNode node)
        {
            CFRuleBodySet set = new CFRuleBodySet();
            string name = ((Redist.Parsers.SymbolTokenText)node.Children[0].Symbol).ValueText;
            Action action = grammar.GetAction(name);
            if (action == null)
                action = grammar.AddAction(name);
            set.Add(new CFRuleBody(action));
            return set;
        }
        private CFRuleBodySet Compile_Recognize_rule_sym_virtual(SyntaxTreeNode node)
        {
            CFRuleBodySet set = new CFRuleBodySet();
            string name = ((Redist.Parsers.SymbolTokenText)node.Children[0].Symbol).ValueText;
            name = name.Substring(1, name.Length - 2);
            Virtual vir = grammar.GetVirtual(name);
            if (vir == null)
                vir = grammar.AddVirtual(name);
            set.Add(new CFRuleBody(vir));
            return set;
        }
        private CFRuleBodySet Compile_Recognize_rule_sym_ref_simple(CompilerContext context, SyntaxTreeNode node)
        {
            Redist.Parsers.SymbolTokenText token = ((Redist.Parsers.SymbolTokenText)node.Children[0].Symbol);
            CFRuleBodySet defs = new CFRuleBodySet();
            if (token.ValueText == "ε")
            {
                defs.Add(new CFRuleBody());
            }
            else
            {
                GrammarSymbol symbol = Compile_Tool_NameToSymbol(token.ValueText, context);
                if (symbol != null)
                    defs.Add(new CFRuleBody(symbol));
                else
                {
                    reporter.Error("Compiler", "@" + token.Line + " Unrecognized symbol " + token.ValueText + " in rule definition");
                    defs.Add(new CFRuleBody());
                }
            }
            return defs;
        }
        private CFRuleBodySet Compile_Recognize_rule_sym_ref_template(CompilerContext context, SyntaxTreeNode node)
        {
            Redist.Parsers.SymbolTokenText token = ((Redist.Parsers.SymbolTokenText)node.Children[0].Symbol);
            CFRuleBodySet defs = new CFRuleBodySet();
            // Get the information
            string name = token.ValueText;
            int paramCount = node.Children[1].Children.Count;
            // check for meta-rule existence
            if (!context.IsTemplateRule(name, paramCount))
            {
                reporter.Error("Compiler", "Meta-rule " + name + " does not exist with " + paramCount.ToString() + " parameters");
                defs.Add(new CFRuleBody());
                return defs;
            }
            // Recognize the parameters
            List<GrammarSymbol> parameters = new List<GrammarSymbol>();
            foreach (SyntaxTreeNode symbolNode in node.Children[1].Children)
                parameters.Add(Compile_Recognize_rule_def_atom(context, symbolNode)[0].Parts[0].Symbol);
            // Get the corresponding variable
            Variable variable = context.GetVariableFromMetaRule(name, parameters, context);
            // Create the definition
            defs.Add(new CFRuleBody(variable));
            return defs;
        }

        private CFRuleBodySet Compile_Recognize_rule_def_atom(CompilerContext context, SyntaxTreeNode node)
        {
            if (node.Symbol.Name == "rule_sym_action")
                return Compile_Recognize_rule_sym_action(node);
            if (node.Symbol.Name == "rule_sym_virtual")
                return Compile_Recognize_rule_sym_virtual(node);
            if (node.Symbol.Name == "rule_sym_ref_simple")
                return Compile_Recognize_rule_sym_ref_simple(context, node);
            if (node.Symbol.Name.StartsWith("rule_sym_ref_template"))
                return Compile_Recognize_rule_sym_ref_template(context, node);
            if (node.Symbol.Name == "SYMBOL_TERMINAL_TEXT")
                return Compile_Recognize_grammar_text_terminal(node);
            return null;
        }
        public CFRuleBodySet Compile_Recognize_rule_definition(CompilerContext context, SyntaxTreeNode node)
        {
            if (node.Symbol is Redist.Parsers.SymbolTokenText)
            {
                Redist.Parsers.SymbolTokenText token = (Redist.Parsers.SymbolTokenText)node.Symbol;
                if (token.ValueText == "?")
                {
                    CFRuleBodySet setInner = Compile_Recognize_rule_definition(context, node.Children[0]);
                    setInner.Insert(0, new CFRuleBody());
                    return setInner;
                }
                else if (token.ValueText == "*")
                {
                    CFRuleBodySet setInner = Compile_Recognize_rule_definition(context, node.Children[0]);
                    CFVariable subVar = grammar.NewCFVariable();
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
                    CFRuleBodySet setInner = Compile_Recognize_rule_definition(context, node.Children[0]);
                    CFVariable subVar = grammar.NewCFVariable();
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
                    CFRuleBodySet setInner = Compile_Recognize_rule_definition(context, node.Children[0]);
                    setInner.SetActionPromote();
                    return setInner;
                }
                else if (token.ValueText == "!")
                {
                    CFRuleBodySet setInner = Compile_Recognize_rule_definition(context, node.Children[0]);
                    setInner.SetActionDrop();
                    return setInner;
                }
                else if (token.ValueText == "|")
                {
                    CFRuleBodySet setLeft = Compile_Recognize_rule_definition(context, node.Children[0]);
                    CFRuleBodySet setRight = Compile_Recognize_rule_definition(context, node.Children[1]);
                    return (setLeft + setRight);
                }
                else if (token.ValueText == "-")
                {
                    CFRuleBodySet setLeft = Compile_Recognize_rule_definition(context, node.Children[0]);
                    CFRuleBodySet setRight = Compile_Recognize_rule_definition(context, node.Children[1]);
                    CFVariable subVar = grammar.NewCFVariable();
                    foreach (CFRuleBody def in setLeft)
                        subVar.AddRule(new CFRule(subVar, def, true, subVar.SID));
                    foreach (CFRuleBody def in setRight)
                        subVar.AddRule(new CFRule(subVar, def, true, -subVar.SID));
                    CFRuleBodySet setFinal = new CFRuleBodySet();
                    setFinal.Add(new CFRuleBody(subVar));
                    return setFinal;
                }
                return Compile_Recognize_rule_def_atom(context, node);
            }
            else if (node.Symbol.Name == "emptypart")
            {
                CFRuleBodySet set = new CFRuleBodySet();
                set.Add(new CFRuleBody());
                return set;
            }
            else if (node.Symbol.Name == "concat")
            {
                CFRuleBodySet setLeft = Compile_Recognize_rule_definition(context, node.Children[0]);
                CFRuleBodySet setRight = Compile_Recognize_rule_definition(context, node.Children[1]);
                return (setLeft * setRight);
            }
            else
                return Compile_Recognize_rule_def_atom(context, node);
        }
        private void Compile_Recognize_rule(CompilerContext context, SyntaxTreeNode node)
        {
            string name = ((Redist.Parsers.SymbolTokenText)node.Children[0].Symbol).ValueText;
            CFVariable var = grammar.GetCFVariable(name);
            CFRuleBodySet defs = Compile_Recognize_rule_definition(context, node.Children[1]);
            foreach (CFRuleBody def in defs)
                var.AddRule(new CFRule(var, def, false));
        }

        private void Compile_Recognize_grammar_options(SyntaxTreeNode optionsNode)
        {
            foreach (SyntaxTreeNode node in optionsNode.Children)
                Compile_Recognize_option(node);
        }
        private void Compile_Recognize_grammar_terminals(SyntaxTreeNode terminalsNode)
        {
            foreach (SyntaxTreeNode node in terminalsNode.Children)
                Compile_Recognize_terminal(node);
        }
        private void Compile_Recognize_grammar_rules(SyntaxTreeNode rulesNode)
        {
            // Create a new context
            CompilerContext context = new CompilerContext(this);
            // Add existing meta-rules that may have been inherited
            foreach (TemplateRule templateRule in grammar.TemplateRules)
                context.AddTemplateRule(templateRule);
            // Load new variables for the rules' head
            foreach (SyntaxTreeNode node in rulesNode.Children)
            {
                if (node.Symbol.Name.StartsWith("cf_rule_simple"))
                {
                    string name = ((Redist.Parsers.SymbolTokenText)node.Children[0].Symbol).ValueText;
                    Variable var = grammar.GetVariable(name);
                    if (var == null)
                        var = grammar.AddVariable(name);
                }
                else if (node.Symbol.Name.StartsWith("cf_rule_template"))
                    context.AddTemplateRule(grammar.AddTemplateRule(node));
            }
            // Load the grammar rules
            foreach (SyntaxTreeNode node in rulesNode.Children)
            {
                if (node.Symbol.Name.StartsWith("cf_rule_simple"))
                    Compile_Recognize_rule(context, node);
            }
        }

        private void Compile_Recognize_grammar_text(SyntaxTreeNode node)
        {
            for (int i = 2; i < node.Children.Count; i++)
            {
                SyntaxTreeNode child = node.Children[i];
                if (child.Symbol.Name == "options")
                    Compile_Recognize_grammar_options(child);
                else if (child.Symbol.Name == "terminals")
                    Compile_Recognize_grammar_terminals(child);
                else if (child.Symbol.Name == "rules")
                    Compile_Recognize_grammar_rules(child);
            }
        }
    }
}
