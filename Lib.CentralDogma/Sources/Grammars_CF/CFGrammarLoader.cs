/**********************************************************************
* Copyright (c) 2013 Laurent Wouters and others
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as
* published by the Free Software Foundation, either version 3
* of the License, or (at your option) any later version.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
* 
* You should have received a copy of the GNU Lesser General
* Public License along with this program.
* If not, see <http://www.gnu.org/licenses/>.
* 
* Contributors:
*     Laurent Wouters - lwouters@xowl.org
**********************************************************************/

using System.Collections.Generic;
using Hime.Redist.Parsers;
using Hime.Redist.AST;
using Hime.Redist.Symbols;

namespace Hime.CentralDogma.Grammars.ContextFree
{
    class CFGrammarLoader : GrammarLoader
    {
        private string resName;
        private ASTNode syntaxRoot;
        private Reporting.Reporter reporter;
        private List<string> inherited;
        private CFGrammar grammar;
        private bool caseInsensitive;

        public Grammar Grammar { get { return grammar; } }
        public bool IsSolved { get { return (inherited.Count == 0); } }

        public CFGrammarLoader(string resName, ASTNode root, Reporting.Reporter reporter)
        {
            this.reporter = reporter;
            this.syntaxRoot = root;
            this.resName = resName;
            this.inherited = new List<string>();
            foreach (ASTNode child in syntaxRoot.Children[1].Children)
                inherited.Add(((TextToken)child.Symbol).Value);
            this.grammar = new CFGrammar(((TextToken)root.Children[0].Symbol).Value);
            this.caseInsensitive = false;
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
                    reporter.Error("Grammar " + parent + " inherited by " + grammar.Name + " cannot be found");
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

		private void ReportErrorAt(ASTNode node, string message)
		{
			TextToken token = node.Symbol as TextToken;
			if (token  == null)
				reporter.Error(resName + " @(?,?) " + message);
			else
				reporter.Error(resName + " @(" + token.Line + ", " + token.Column + ") " + message);
		}

        private Symbol Compile_Tool_NameToSymbol(string name, CompilerContext context)
        {
            if (context.IsReference(name))
                return context.GetReference(name);
            return grammar.GetSymbol(name);
        }

        private void Compile_Recognize_option(ASTNode node)
        {
            string Name = ((TextToken)node.Children[0].Symbol).Value;
            string Value = ((TextToken)node.Children[1].Symbol).Value;
            Value = Value.Substring(1, Value.Length - 2);
            grammar.AddOption(Name, Value);
        }

        private Automata.NFA Compile_Recognize_terminal_def_atom_any(ASTNode node)
        {
            Automata.NFA automata = Automata.NFA.NewMinimal();
			// plane 0 transitions
			automata.StateEntry.AddTransition(new CharSpan((char)0x0000, (char)0xD7FF), automata.StateExit);
			automata.StateEntry.AddTransition(new CharSpan((char)0xE000, (char)0xFFFF), automata.StateExit);
			// surrogate pairs
			Automata.NFAState intermediate = automata.AddNewState();
			automata.StateEntry.AddTransition(new CharSpan((char)0xD800, (char)0xDBFF), intermediate);
			intermediate.AddTransition(new CharSpan((char)0xDC00, (char)0xDFFF), automata.StateExit);
			return automata;
        }
        private Automata.NFA Compile_Recognize_terminal_def_atom_unicode(ASTNode node)
		{
			Automata.NFA automata = Automata.NFA.NewMinimal();
			string value = ((TextToken)node.Symbol).Value;
			value = value.Substring(2, value.Length - 2);
			UnicodeCodePoint cp = new UnicodeCodePoint(System.Convert.ToInt32(value, 16));
			char[] data = cp.GetUTF16();
			if (data.Length == 1)
			{
				automata.StateEntry.AddTransition(new CharSpan(data[0], data[0]), automata.StateExit);
			}
			else
			{
				Automata.NFAState intermediate = automata.AddNewState();
				automata.StateEntry.AddTransition(new CharSpan(data[0], data[0]), intermediate);
				intermediate.AddTransition(new CharSpan(data[1], data[1]), automata.StateExit);
			}
			return automata;
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
        private Automata.NFA Compile_Recognize_terminal_def_atom_text(ASTNode node)
        {
        	Automata.NFA automata = Automata.NFA.NewMinimal();
            automata.StateExit = automata.StateEntry;
            string value = ((TextToken)node.Children[node.Children.Count-1].Symbol).Value;
            bool insensitive = caseInsensitive || (node.Children.Count > 1);
            value = value.Substring(1, value.Length - 2);
            value = ReplaceEscapees(value).Replace("\\'", "'");
            foreach (char c in value)
            {
                Automata.NFAState temp = automata.AddNewState();
                if (insensitive && char.IsLetter(c))
                {
                    char c2 = char.IsLower(c) ? char.ToUpper(c) : char.ToLower(c);
                    automata.StateExit.AddTransition(new CharSpan(c, c), temp);
                    automata.StateExit.AddTransition(new CharSpan(c2, c2), temp);
                }
                else
                    automata.StateExit.AddTransition(new CharSpan(c, c), temp);
                automata.StateExit = temp;
            }
            return automata;
        }
        private Automata.NFA Compile_Recognize_terminal_def_atom_set(ASTNode node)
        {
            Automata.NFA automata = Automata.NFA.NewMinimal();
            string value = ((TextToken)node.Symbol).Value;
            value = value.Substring(1, value.Length - 2);
            value = ReplaceEscapees(value).Replace("\\[", "[").Replace("\\]", "]");
            bool positive = true;
            if (value[0] == '^')
            {
                value = value.Substring(1);
                positive = false;
            }

            List<CharSpan> spans = new List<CharSpan>();
			for (int i = 0; i != value.Length; i++)
            {
				// read the first full unicode character
				char b = value[i];
				if (b >= 0xD800 && b <= 0xDFFF)
				{
					ReportErrorAt(node, "Unsupported non-plane 0 Unicode character (" + b + value[i + 1] + ") in character class");
					return automata;
				}

                if ((i != value.Length - 1) && (value[i + 1] == '-'))
                {
					// this is a range, match the '-'
					i += 2;
					char e = value[i];
					if (e >= 0xD800 && e <= 0xDFFF)
					{
						ReportErrorAt(node, "Unsupported non-plane 0 Unicode character (" + e + value[i + 1] + ") in character class");
						return automata;
					}
					if (b < 0xD800 && e > 0xDFFF)
					{
						// oooh you ...
						spans.Add(new CharSpan(b, (char)0xD7FF));
						spans.Add(new CharSpan((char)0xE000, e));
					}
					else
					{
						spans.Add(new CharSpan(b, e));
					}
                }
                else
				{
					// this is a normal character
					spans.Add(new CharSpan(b, b));
				}
            }
            if (positive)
            {
                foreach (CharSpan span in spans)
                    automata.StateEntry.AddTransition(span, automata.StateExit);
            }
            else
            {
                spans.Sort(new System.Comparison<CharSpan>(CharSpan.Compare));
				// TODO: Check for span intersections and overflow of b (when a span ends on 0xFFFF)
                char b = (char)0;
                for (int i = 0; i != spans.Count; i++)
                {
                    if (spans[i].Begin > b)
                        automata.StateEntry.AddTransition(new CharSpan(b, (char)(spans[i].Begin - 1)), automata.StateExit);
					b = (char)(spans[i].End + 1);
					// skip the surrogate encoding points
					if (b >= 0xD800 && b <= 0xDFFF)
						b = (char)0xE000;
                }
				if (b <= 0xD7FF)
				{
					automata.StateEntry.AddTransition(new CharSpan(b, (char)0xD7FF), automata.StateExit);
					automata.StateEntry.AddTransition(new CharSpan((char)0xE000, (char)0xFFFF), automata.StateExit);
				}
				else if (b != 0xFFFF)
				{
					// here b >= 0xE000
					automata.StateEntry.AddTransition(new CharSpan(b, (char)0xFFFF), automata.StateExit);
				}
				// surrogate pairs
				Automata.NFAState intermediate = automata.AddNewState();
				automata.StateEntry.AddTransition(new CharSpan((char)0xD800, (char)0xDBFF), intermediate);
				intermediate.AddTransition(new CharSpan((char)0xDC00, (char)0xDFFF), automata.StateExit);
            }
            return automata;
        }
		private void AddUnicodeSpanToNFA(Automata.NFA automata, Automata.NFAState intermediate, UnicodeSpan span)
		{
			char[] b = span.Begin.GetUTF16();
			char[] e = span.End.GetUTF16();
			if (e.Length == 1)
			{
				// this span is entirely in plane 0
				automata.StateEntry.AddTransition(new CharSpan(b[0], e[0]), automata.StateExit);
			}
			else if (b.Length == 2)
			{
				// this span has no part in plane 0
				automata.StateEntry.AddTransition(new CharSpan(b[0], e[0]), intermediate);
				intermediate.AddTransition(new CharSpan(b[1], e[1]), automata.StateExit);
			}
			else
			{
				// this span has only a part in plane 0
				if (b[0] < 0xD800)
				{
					automata.StateEntry.AddTransition(new CharSpan(b[0], (char)0xD7FF), automata.StateExit);
					automata.StateEntry.AddTransition(new CharSpan((char)0xE000, (char)0xFFFF), automata.StateExit);
				}
				else
				{
					automata.StateEntry.AddTransition(new CharSpan(b[0], (char)0xFFFF), automata.StateExit);
				}
				automata.StateEntry.AddTransition(new CharSpan((char)0xD800, e[0]), intermediate);
				intermediate.AddTransition(new CharSpan((char)0xDC00, e[1]), automata.StateExit);
			}
		}
        private Automata.NFA Compile_Recognize_terminal_def_atom_ublock(ASTNode node)
		{
			Automata.NFA automata = Automata.NFA.NewMinimal();
			TextToken token = (TextToken)node.Symbol;
			string value = token.Value.Substring(3, token.Value.Length - 4);
			UnicodeBlock block = UnicodeBlocks.GetBlock(value);
			if (block == null)
			{
				ReportErrorAt(node, "Unkown Unicode block " + value);
				return automata;
			}
			Automata.NFAState intermediate = automata.AddNewState();
			AddUnicodeSpanToNFA(automata, intermediate, block.Span);
            return automata;
        }
        private Automata.NFA Compile_Recognize_terminal_def_atom_ucat(ASTNode node)
        {
            Automata.NFA automata = Automata.NFA.NewMinimal();
            TextToken token = (TextToken)node.Symbol;
            string value = token.Value.Substring(3, token.Value.Length - 4);
            UnicodeCategory category = UnicodeCategory.GetCateogry(value);
            if (category == null)
			{
				ReportErrorAt(node, "Unkown Unicode category " + value);
				return automata;
			}
			Automata.NFAState intermediate = automata.AddNewState();
			foreach (UnicodeSpan span in category.Spans)
				AddUnicodeSpanToNFA(automata, intermediate, span);
            return automata;
        }
        private Automata.NFA Compile_Recognize_terminal_def_atom_span(ASTNode node)
        {
            Automata.NFA automata = Automata.NFA.NewMinimal();
            int spanBegin = 0;
            int spanEnd = 0;
            // Get span begin
            TextToken child = (TextToken)node.Children[0].Symbol;
            spanBegin = System.Convert.ToInt32(child.Value.Substring(2), 16);
            // Get span end
            child = (TextToken)node.Children[1].Symbol;
            spanEnd = System.Convert.ToInt32(child.Value.Substring(2), 16);
            // If span end is before beginning: reverse
            if (spanBegin > spanEnd)
            {
				ReportErrorAt(node.Children[0], "Invalid span (end is before beginning)");
				return automata;
            }
			Automata.NFAState intermediate = automata.AddNewState();
			AddUnicodeSpanToNFA(automata, intermediate, new UnicodeSpan(spanBegin, spanEnd));
            return automata;
        }
        private Automata.NFA Compile_Recognize_terminal_def_atom_name(ASTNode node)
        {
            TextToken token = (TextToken)node.Symbol;
            Terminal Ref = grammar.GetTerminalByName(token.Value);
            if (Ref == null)
            {
                ReportErrorAt(node, "Cannot find terminal " + token.Value);
                Automata.NFA final = Automata.NFA.NewMinimal();
                final.StateEntry.AddTransition(Automata.NFA.Epsilon, final.StateExit);
                return final;
            }
            return ((TextTerminal)Ref).NFA.Clone(false);
        }
		
        private Automata.NFA Compile_Recognize_terminal_definition(ASTNode node)
        {
			Hime.Redist.Symbols.Symbol symbol = node.Symbol;
            // Symbol is a token
			// TODO: this check is not nice, 
			// also it is strange to use here some many Redist objects 
			// (shouldn't the work be done in the other namespace?)
            if (symbol is Hime.Redist.Symbols.TextToken)
            {
                TextToken token = (TextToken)node.Symbol;
                if (token.Value == "?")
                {
                    Automata.NFA inner = Compile_Recognize_terminal_definition(node.Children[0]);
                    return Automata.NFA.NewOptional(inner, false);
                }
                if (token.Value == "*")
                {
                    Automata.NFA inner = Compile_Recognize_terminal_definition(node.Children[0]);
                    return Automata.NFA.NewRepeatZeroMore(inner, false);
                }
                if (token.Value == "+")
                {
                    Automata.NFA inner = Compile_Recognize_terminal_definition(node.Children[0]);
                    return Automata.NFA.NewRepeatOneOrMore(inner, false);
                }
                if (token.Value == "|")
                {
                    Automata.NFA left = Compile_Recognize_terminal_definition(node.Children[0]);
                    Automata.NFA right = Compile_Recognize_terminal_definition(node.Children[1]);
                    return Automata.NFA.NewUnion(left, right, false);
                }
                if (token.Value == "-")
                {
                    Automata.NFA left = Compile_Recognize_terminal_definition(node.Children[0]);
                    Automata.NFA right = Compile_Recognize_terminal_definition(node.Children[1]);
                    return Automata.NFA.NewDifference(left, right, false);
                }
                if (token.Value == ".")
                    return Compile_Recognize_terminal_def_atom_any(node);
                if (token.Name == "SYMBOL_VALUE_UINT8")
                    return Compile_Recognize_terminal_def_atom_unicode(node);
                if (token.Name == "SYMBOL_VALUE_UINT16")
                    return Compile_Recognize_terminal_def_atom_unicode(node);
                if (token.Name == "SYMBOL_TERMINAL_SET")
                    return Compile_Recognize_terminal_def_atom_set(node);
                if (token.Name == "SYMBOL_TERMINAL_UCAT")
                    return Compile_Recognize_terminal_def_atom_ucat(node);
                if (token.Name == "SYMBOL_TERMINAL_UBLOCK")
                    return Compile_Recognize_terminal_def_atom_ublock(node);
                if (token.Value == "..")
                    return Compile_Recognize_terminal_def_atom_span(node);
                if (token.Name == "NAME")
                    return Compile_Recognize_terminal_def_atom_name(node);
                Automata.NFA final = Automata.NFA.NewMinimal();
                final.StateEntry.AddTransition(Automata.NFA.Epsilon, final.StateExit);
                return final;
            }
            else if (symbol is Hime.Redist.Symbols.Variable)
            {
                if (symbol.Name == "terminal_def_atom_text")
                    return Compile_Recognize_terminal_def_atom_text(node);
            }
            else if (symbol is Hime.Redist.Symbols.Virtual)
            {
                if (symbol.Name == "range")
                {
                    Automata.NFA inner = Compile_Recognize_terminal_definition(node.Children[0]);
                    int min = System.Convert.ToInt32(((TextToken)node.Children[1].Symbol).Value);
                    int max = min;
                    if (node.Children.Count > 2)
                        max = System.Convert.ToInt32(((TextToken)node.Children[2].Symbol).Value);
                    return Automata.NFA.NewRepeatRange(inner, false, min, max);
                }
                if (symbol.Name == "concat")
                {
                    Automata.NFA left = Compile_Recognize_terminal_definition(node.Children[0]);
                    Automata.NFA right = Compile_Recognize_terminal_definition(node.Children[1]);
                    return Automata.NFA.NewConcatenation(left, right, false);
                }
            }
            return null;
        }
		
        private Terminal Compile_Recognize_terminal(ASTNode node)
        {
            TextToken token = node.Children[0].Symbol as TextToken;
            TextTerminal terminal = grammar.GetTerminalByName(token.Value) as TextTerminal;
            if (terminal == null)
            {
                Automata.NFA nfa = Compile_Recognize_terminal_definition(node.Children[1]);
                terminal = grammar.AddTerminalNamed(token.Value, nfa);
                nfa.StateExit.Item = terminal;
            }
            else
            {
                ReportErrorAt(node.Children[0], "Overriding the definition of terminal " + token.Value);
            }
            return terminal;
        }

        private CFRuleBodySet Compile_Recognize_grammar_text_terminal(ASTNode node)
        {
            // Construct the terminal name
            string value = ((TextToken)node.Children[node.Children.Count - 1].Symbol).Value;
            value = value.Substring(1, value.Length - 2);
            value = ReplaceEscapees(value).Replace("\\'", "'");
            // Check for previous instance in the grammar
            Terminal terminal = grammar.GetTerminalByValue(value);
            if (terminal == null)
            {
                // Create the terminal
                Automata.NFA nfa = Compile_Recognize_terminal_def_atom_text(node);
                terminal = grammar.AddTerminalAnon(value, nfa);
                nfa.StateExit.Item = terminal;
            }
            // Create the definition set
            CFRuleBodySet set = new CFRuleBodySet();
            set.Add(new CFRuleBody(terminal));
            return set;
        }

        private CFRuleBodySet Compile_Recognize_rule_sym_action(ASTNode node)
        {
            CFRuleBodySet set = new CFRuleBodySet();
            string name = ((TextToken)node.Children[0].Symbol).Value;
            Action action = grammar.GetAction(name);
            if (action == null)
                action = grammar.AddAction(name);
            set.Add(new CFRuleBody(action));
            return set;
        }
        private CFRuleBodySet Compile_Recognize_rule_sym_virtual(ASTNode node)
        {
            CFRuleBodySet set = new CFRuleBodySet();
            string name = ((TextToken)node.Children[0].Symbol).Value;
            name = name.Substring(1, name.Length - 2);
            Virtual vir = grammar.GetVirtual(name);
            if (vir == null)
                vir = grammar.AddVirtual(name);
            set.Add(new CFRuleBody(vir));
            return set;
        }
        private CFRuleBodySet Compile_Recognize_rule_sym_ref_simple(CompilerContext context, ASTNode node)
        {
            TextToken token = ((TextToken)node.Children[0].Symbol);
            CFRuleBodySet defs = new CFRuleBodySet();
            if (token.Value == "ε")
            {
                defs.Add(new CFRuleBody());
            }
            else
            {
                Symbol symbol = Compile_Tool_NameToSymbol(token.Value, context);
                if (symbol != null)
                    defs.Add(new CFRuleBody(symbol));
                else
                {
                    ReportErrorAt(node.Children[0], "Unrecognized symbol " + token.Value + " in rule definition");
                    defs.Add(new CFRuleBody());
                }
            }
            return defs;
        }
        private CFRuleBodySet Compile_Recognize_rule_sym_ref_template(CompilerContext context, ASTNode node)
        {
            TextToken token = ((TextToken)node.Children[0].Symbol);
            CFRuleBodySet defs = new CFRuleBodySet();
            // Get the information
            string name = token.Value;
            int paramCount = node.Children[1].Children.Count;
            // check for meta-rule existence
            if (!context.IsTemplateRule(name, paramCount))
            {
                ReportErrorAt(node.Children[0], "Meta-rule " + name + " does not exist with " + paramCount.ToString() + " parameters");
                defs.Add(new CFRuleBody());
                return defs;
            }
            // Recognize the parameters
            List<Symbol> parameters = new List<Symbol>();
            foreach (ASTNode symbolNode in node.Children[1].Children)
                parameters.Add(Compile_Recognize_rule_def_atom(context, symbolNode)[0].Parts[0].Symbol);
            // Get the corresponding variable
            Variable variable = context.GetVariableFromMetaRule(name, parameters, context);
            // Create the definition
            defs.Add(new CFRuleBody(variable));
            return defs;
        }

        private CFRuleBodySet Compile_Recognize_rule_def_atom(CompilerContext context, ASTNode node)
        {
            if (node.Symbol.Name == "rule_sym_action")
                return Compile_Recognize_rule_sym_action(node);
            if (node.Symbol.Name == "rule_sym_virtual")
                return Compile_Recognize_rule_sym_virtual(node);
            if (node.Symbol.Name == "rule_sym_ref_simple")
                return Compile_Recognize_rule_sym_ref_simple(context, node);
            if (node.Symbol.Name.StartsWith("rule_sym_ref_template"))
                return Compile_Recognize_rule_sym_ref_template(context, node);
            if (node.Symbol.Name == "terminal_def_atom_text")
                return Compile_Recognize_grammar_text_terminal(node);
            return null;
        }
        public CFRuleBodySet Compile_Recognize_rule_definition(CompilerContext context, ASTNode node)
        {
            if (node.Symbol is TextToken)
            {
                TextToken token = (TextToken)node.Symbol;
                if (token.Value == "?")
                {
                    CFRuleBodySet setInner = Compile_Recognize_rule_definition(context, node.Children[0]);
                    setInner.Insert(0, new CFRuleBody());
                    return setInner;
                }
                else if (token.Value == "*")
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
                else if (token.Value == "+")
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
                else if (token.Value == "^")
                {
                    CFRuleBodySet setInner = Compile_Recognize_rule_definition(context, node.Children[0]);
                    setInner.SetActionPromote();
                    return setInner;
                }
                else if (token.Value == "!")
                {
                    CFRuleBodySet setInner = Compile_Recognize_rule_definition(context, node.Children[0]);
                    setInner.SetActionDrop();
                    return setInner;
                }
                else if (token.Value == "|")
                {
                    CFRuleBodySet setLeft = Compile_Recognize_rule_definition(context, node.Children[0]);
                    CFRuleBodySet setRight = Compile_Recognize_rule_definition(context, node.Children[1]);
                    return (setLeft + setRight);
                }
                else if (token.Value == "-")
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
        private void Compile_Recognize_rule(CompilerContext context, ASTNode node)
        {
            string name = ((TextToken)node.Children[0].Symbol).Value;
            CFVariable var = grammar.GetCFVariable(name);
            CFRuleBodySet defs = Compile_Recognize_rule_definition(context, node.Children[1]);
            foreach (CFRuleBody def in defs)
                var.AddRule(new CFRule(var, def, false));
        }

        private void Compile_Recognize_grammar_options(ASTNode optionsNode)
        {
            foreach (ASTNode node in optionsNode.Children)
                Compile_Recognize_option(node);
            caseInsensitive = (grammar.HasOption("CaseSensitive") && grammar.GetOption("CaseSensitive").Equals("false", System.StringComparison.InvariantCultureIgnoreCase));
        }
        private void Compile_Recognize_grammar_terminals(ASTNode terminalsNode)
        {
            foreach (ASTNode node in terminalsNode.Children)
                Compile_Recognize_terminal(node);
        }
        private void Compile_Recognize_grammar_rules(ASTNode rulesNode)
        {
            // Create a new context
            CompilerContext context = new CompilerContext(this);
            // Add existing meta-rules that may have been inherited
            foreach (TemplateRule templateRule in grammar.TemplateRules)
                context.AddTemplateRule(templateRule);
            // Load new variables for the rules' head
            foreach (ASTNode node in rulesNode.Children)
            {
                if (node.Symbol.Name.StartsWith("cf_rule_simple"))
                {
                    string name = ((TextToken)node.Children[0].Symbol).Value;
                    Variable var = grammar.GetVariable(name);
                    if (var == null)
                        var = grammar.AddVariable(name);
                }
                else if (node.Symbol.Name.StartsWith("cf_rule_template"))
                    context.AddTemplateRule(grammar.AddTemplateRule(node));
            }
            // Load the grammar rules
            foreach (ASTNode node in rulesNode.Children)
            {
                if (node.Symbol.Name.StartsWith("cf_rule_simple"))
                    Compile_Recognize_rule(context, node);
            }
        }

        private void Compile_Recognize_grammar_text(ASTNode node)
        {
            for (int i = 2; i < node.Children.Count; i++)
            {
                ASTNode child = node.Children[i];
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
