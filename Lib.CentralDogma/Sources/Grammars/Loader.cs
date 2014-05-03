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
using Hime.CentralDogma.Input;
using Hime.Redist.Parsers;
using Hime.Redist;

namespace Hime.CentralDogma.Grammars
{
	/// <summary>
	/// Represents a loader for a grammar
	/// </summary>
	public class Loader
	{
		/// <summary>
		/// The name of the resource containing the data that are loaded by this instance
		/// </summary>
		private string resource;
		/// <summary>
		/// The root to load from
		/// </summary>
		private ASTNode root;
		/// <summary>
		/// The log
		/// </summary>
		private Reporter reporter;
		/// <summary>
		/// Lists of the inherited grammars
		/// </summary>
		private List<string> inherited;
		/// <summary>
		/// The resulting grammar
		/// </summary>
		private Grammar grammar;
		/// <summary>
		/// Flag for the global casing of the grammar
		/// </summary>
		private bool caseInsensitive;

		/// <summary>
		/// Gets the result of this loader
		/// </summary>
		public Grammar Grammar { get { return grammar; } }
		/// <summary>
		/// Gets a value indicating whether all dependencies are solved
		/// </summary>
		public bool IsSolved { get { return (inherited.Count == 0); } }

		/// <summary>
		/// Initializes this load
		/// </summary>
		/// <param name="resName">The name of the resource</param>
		/// <param name="root">The root AST</param>
		/// <param name="reporter">The log</param>
		public Loader(string resName, ASTNode root, Reporter reporter)
		{
			this.reporter = reporter;
			this.root = root;
			this.resource = resName;
			this.inherited = new List<string>();
			foreach (ASTNode child in root.Children[1].Children)
				inherited.Add(child.Symbol.Value);
			this.grammar = new Grammar(root.Children[0].Symbol.Value);
			this.caseInsensitive = false;
			if (inherited.Count == 0)
				LoadGrammarContent(root);
		}

		/// <summary>
		/// Loads the specified data
		/// </summary>
		/// <param name="siblings">The siblings of this loader</param>
		public void Load(Dictionary<string, Loader> siblings)
		{
			List<string> temp = new List<string>(inherited);
			foreach (string parent in temp)
			{
				if (!siblings.ContainsKey(parent))
				{
					reporter.Error(string.Format("{0} Grammar {1} inherited by {2} cannot be found", resource, parent, grammar.Name));
					inherited.Remove(parent);
				}
				Loader loader = siblings[parent];
				if (!loader.IsSolved)
					continue;
				this.grammar.Inherit(loader.Grammar);
				inherited.Remove(parent);
			}
			if (inherited.Count == 0)
				LoadGrammarContent(root);
		}

		/// <summary>
		/// Gets the symbol corresponding to the given name, depending on the context
		/// </summary>
		/// <param name="name">A name</param>
		/// <param name="context">The current context</param>
		private Symbol ResolveSymbol(string name, Context context)
		{
			// is this a reference to a template parameter?
			if (context.IsBound(name))
				return context.GetBinding(name);
			// then this is a reference to a normal grammar symbol
			return grammar.GetSymbol(name);
		}

		/// <summary>
		/// Replaces the escape sequences in the given piece of text by their value
		/// </summary>
		/// <param name="value">A string</param>
		/// <returns>The string with the escape sequences replaced by their value</returns>
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
				if (next == '\\')
					builder.Append(next);
				else if (next == '0')
					builder.Append('\0'); /*Unicode character 0*/
                else if (next == 'a')
					builder.Append('\a'); /*Alert (character 7)*/
                else if (next == 'b')
					builder.Append('\b'); /*Backspace (character 8)*/
                else if (next == 'f')
					builder.Append('\f'); /*Form feed (character 12)*/
                else if (next == 'n')
					builder.Append('\n'); /*New line (character 10)*/
                else if (next == 'r')
					builder.Append('\r'); /*Carriage return (character 13)*/
                else if (next == 't')
					builder.Append('\t'); /*Horizontal tab (character 9)*/
                else if (next == 'v')
					builder.Append('\v'); /*Vertical quote (character 11)*/
                else
					builder.Append("\\" + next);
			}
			return builder.ToString();
		}

		/// <summary>
		/// Adds a unicode character span to an existing NFA automaton
		/// </summary>
		/// <param name="automata">The target NFA</param>
		/// <param name="intermediate">The intermediate state for characters outside plane 0</param>
		/// <param name="span">The unicode span to add</param>
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

		/// <summary>
		/// Loads the content of the grammar in the given AST
		/// </summary>
		/// <param name="node">The AST node representing a grammar</param>
		private void LoadGrammarContent(ASTNode node)
		{
			for (int i = 2; i < node.Children.Count; i++)
			{
				ASTNode child = node.Children[i];
				if (child.Symbol.ID == HimeGrammarLexer.ID.BLOCK_OPTIONS)
					LoadBlockOptions(child);
				else if (child.Symbol.ID == HimeGrammarLexer.ID.BLOCK_TERMINALS)
					LoadBlockTerminals(child);
				else if (child.Symbol.ID == HimeGrammarLexer.ID.BLOCK_RULES)
					LoadBlockRules(child);
			}
		}

		/// <summary>
		/// Loads the options block of a grammar
		/// </summary>
		/// <param name="node">The options block's AST node</param>
		private void LoadBlockOptions(ASTNode node)
		{
			foreach (ASTNode child in node.Children)
				LoadOption(child);
			// determine the casing of the grammar
			string value = grammar.GetOption("CaseSensitive");
			caseInsensitive = (value != null && value.Equals("false", System.StringComparison.InvariantCultureIgnoreCase));
		}

		/// <summary>
		/// Loads the grammar option in the given AST
		/// </summary>
		/// <param name="node">The AST node of a grammar option</param>
		private void LoadOption(ASTNode node)
		{
			string name = node.Children[0].Symbol.Value;
			string value = node.Children[1].Symbol.Value;
			value = value.Substring(1, value.Length - 2);
			grammar.AddOption(name, value);
		}

		/// <summary>
		/// Loads the terminal blocks of a grammar
		/// </summary>
		/// <param name="node">The terminal block's AST node</param>
		private void LoadBlockTerminals(ASTNode node)
		{
			foreach (ASTNode child in node.Children)
				LoadTerminal(child);
		}

		/// <summary>
		/// Loads the terminal in the given AST
		/// </summary>
		/// <param name="node">The AST node of a terminal rule</param>
		private void LoadTerminal(ASTNode node)
		{
			ASTNode nameNode = node.Children[0];
			// Resolve the terminal
			Terminal terminal = grammar.GetTerminalByName(nameNode.Symbol.Value);
			if (terminal == null)
			{
				// The terminal does not already exists
				// Build the NFA
				Automata.NFA nfa = BuildNFA(node.Children[1]);
				// Create the terminal in the grammar
				terminal = grammar.AddTerminalNamed(nameNode.Symbol.Value, nfa);
				// Marks the final NFA state with the new terminal
				nfa.StateExit.Item = terminal;
			}
			else
			{
				// Tried to override the terminal
				reporter.Error(string.Format("{0}{1} Overriding the definition of terminal {2}", resource, nameNode.Position, nameNode.Symbol.Value));
			}
		}

		/// <summary>
		/// Builds the NFA represented by the AST node
		/// </summary>
		/// <param name="node">An AST node representing a NFA</param>
		/// <returns>The equivalent NFA</returns>
		private Automata.NFA BuildNFA(ASTNode node)
		{
			Hime.Redist.Symbol symbol = node.Symbol;

			if (symbol.ID == HimeGrammarLexer.ID.LITERAL_TEXT)
				return BuildNFAFromText(node);
			if (symbol.ID == HimeGrammarLexer.ID.UNICODE_CODEPOINT)
				return BuildNFAFromCodepoint(node);
			if (symbol.ID == HimeGrammarLexer.ID.LITERAL_CLASS)
				return BuildNFAFromClass(node);
			if (symbol.ID == HimeGrammarLexer.ID.UNICODE_CATEGORY)
				return BuildNFAFromUnicodeCategory(node);
			if (symbol.ID == HimeGrammarLexer.ID.UNICODE_BLOCK)
				return BuildNFAFromUnicodeBlock(node);
			if (symbol.ID == HimeGrammarLexer.ID.UNICODE_SPAN_MARKER)
				return BuildNFAFromUnicodeSpan(node);
			if (symbol.ID == HimeGrammarLexer.ID.LITERAL_ANY)
				return BuildNFAFromAny(node);

			if (symbol.ID == HimeGrammarLexer.ID.NAME)
				return BuildNFAFromReference(node);

			if (symbol.ID == HimeGrammarLexer.ID.OPERATOR_OPTIONAL)
			{
				Automata.NFA inner = BuildNFA(node.Children[0]);
				return Automata.NFA.NewOptional(inner, false);
			}
			if (symbol.ID == HimeGrammarLexer.ID.OPERATOR_ZEROMORE)
			{
				Automata.NFA inner = BuildNFA(node.Children[0]);
				return Automata.NFA.NewRepeatZeroMore(inner, false);
			}
			if (symbol.ID == HimeGrammarLexer.ID.OPERATOR_ONEMORE)
			{
				Automata.NFA inner = BuildNFA(node.Children[0]);
				return Automata.NFA.NewRepeatOneOrMore(inner, false);
			}
			if (symbol.ID == HimeGrammarLexer.ID.OPERATOR_UNION)
			{
				Automata.NFA left = BuildNFA(node.Children[0]);
				Automata.NFA right = BuildNFA(node.Children[1]);
				return Automata.NFA.NewUnion(left, right, false);
			}
			if (symbol.ID == HimeGrammarLexer.ID.OPERATOR_DIFFERENCE)
			{
				Automata.NFA left = BuildNFA(node.Children[0]);
				Automata.NFA right = BuildNFA(node.Children[1]);
				return Automata.NFA.NewDifference(left, right, false);
			}
			if (symbol.Name == "range")
			{
				Automata.NFA inner = BuildNFA(node.Children[0]);
				int min = System.Convert.ToInt32(node.Children[1].Symbol.Value);
				int max = min;
				if (node.Children.Count > 2)
					max = System.Convert.ToInt32(node.Children[2].Symbol.Value);
				return Automata.NFA.NewRepeatRange(inner, false, min, max);
			}
			if (symbol.Name == "concat")
			{
				Automata.NFA left = BuildNFA(node.Children[0]);
				Automata.NFA right = BuildNFA(node.Children[1]);
				return Automata.NFA.NewConcatenation(left, right, false);
			}

			// nothing found ...
			reporter.Error(string.Format("{0}{1} Failed to recognize lexical rule", resource, node.Position));
			// return an empty NFA
			return BuildEpsilonNFA();
		}

		/// <summary>
		/// Builds a NFA that does nothing
		/// </summary>
		/// <returns>A NFA</returns>
		private Automata.NFA BuildEpsilonNFA()
		{
			Automata.NFA final = Automata.NFA.NewMinimal();
			final.StateEntry.AddTransition(Automata.NFA.Epsilon, final.StateExit);
			return final;
		}

		/// <summary>
		/// Builds a NFA from a piece of text
		/// </summary>
		/// <param name="node">An AST node representing a NFA</param>
		/// <returns>The equivalent NFA</returns>
		private Automata.NFA BuildNFAFromText(ASTNode node)
		{
			Automata.NFA automata = Automata.NFA.NewMinimal();
			automata.StateExit = automata.StateEntry;

			// build the raw piece of text
			string value = node.Symbol.Value;
			bool insensitive = caseInsensitive;
			if (value.StartsWith("~"))
			{
				insensitive = true;
				value = value.Substring(2, value.Length - 3);
			}
			else
			{
				value = value.Substring(1, value.Length - 2);
			}
			value = ReplaceEscapees(value).Replace("\\'", "'");

			// build the result
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

		/// <summary>
		/// Builds a NFA from a unicode code point
		/// </summary>
		/// <param name="node">An AST node representing a NFA</param>
		/// <returns>The equivalent NFA</returns>
		private Automata.NFA BuildNFAFromCodepoint(ASTNode node)
		{
			// extract the code point value
			string value = node.Symbol.Value;
			value = value.Substring(2, value.Length - 2);
			int cpValue = System.Convert.ToInt32(value, 16);
			if (cpValue < 0 || (cpValue >= 0xD800 && cpValue <= 0xDFFF) || cpValue >= 0x110000)
			{
				reporter.Error(string.Format("{0}{1} The value U+{2} is not a supported unicode code point", resource, node.Position, cpValue.ToString("X")));
				return BuildEpsilonNFA();
			}
			UnicodeCodePoint cp = new UnicodeCodePoint(cpValue);
			// build the NFA
			Automata.NFA automata = Automata.NFA.NewMinimal();
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

		/// <summary>
		/// Builds a NFA from a character class
		/// </summary>
		/// <param name="node">An AST node representing a NFA</param>
		/// <returns>The equivalent NFA</returns>
		private Automata.NFA BuildNFAFromClass(ASTNode node)
		{
			// extract the value
			string value = node.Symbol.Value;
			value = value.Substring(1, value.Length - 2);
			value = ReplaceEscapees(value).Replace("\\[", "[").Replace("\\]", "]");
			bool positive = true;
			if (value[0] == '^')
			{
				value = value.Substring(1);
				positive = false;
			}
			// build the character spans
			List<CharSpan> spans = new List<CharSpan>();
			for (int i = 0; i != value.Length; i++)
			{
				// read the first full unicode character
				char b = value[i];
				if (b >= 0xD800 && b <= 0xDFFF)
				{
					reporter.Error(string.Format("{0}{1} Unsupported non-plane 0 Unicode character ({2}) in character class", resource, node.Position, b + value[i + 1]));
					return BuildEpsilonNFA();
				}
				if ((i != value.Length - 1) && (value[i + 1] == '-'))
				{
					// this is a range, match the '-'
					i += 2;
					char e = value[i];
					if (e >= 0xD800 && e <= 0xDFFF)
					{
						reporter.Error(string.Format("{0}{1} Unsupported non-plane 0 Unicode character ({2}) in character class", resource, node.Position, e + value[i + 1]));
						return BuildEpsilonNFA();
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
			// build the result
			Automata.NFA automata = Automata.NFA.NewMinimal();
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

		/// <summary>
		/// Builds a NFA from a unicode category
		/// </summary>
		/// <param name="node">An AST node representing a NFA</param>
		/// <returns>The equivalent NFA</returns>
		private Automata.NFA BuildNFAFromUnicodeCategory(ASTNode node)
		{
			// extract the value
			string value = node.Symbol.Value.Substring(3, node.Symbol.Value.Length - 4);
			UnicodeCategory category = UnicodeCategories.GetCategory(value);
			if (category == null)
			{
				reporter.Error(string.Format("{0}{1} Unknown unicode category {2}", resource, node.Position, value));
				return BuildEpsilonNFA();
			}
			// build the result
			Automata.NFA automata = Automata.NFA.NewMinimal();
			Automata.NFAState intermediate = automata.AddNewState();
			foreach (UnicodeSpan span in category.Spans)
				AddUnicodeSpanToNFA(automata, intermediate, span);
			return automata;
		}

		/// <summary>
		/// Builds a NFA from a unicode block
		/// </summary>
		/// <param name="node">An AST node representing a NFA</param>
		/// <returns>The equivalent NFA</returns>
		private Automata.NFA BuildNFAFromUnicodeBlock(ASTNode node)
		{
			// extract the value
			string value = node.Symbol.Value.Substring(3, node.Symbol.Value.Length - 4);
			UnicodeBlock block = UnicodeBlocks.GetBlock(value);
			if (block == null)
			{
				reporter.Error(string.Format("{0}{1} Unknown unicode block {2}", resource, node.Position, value));
				return BuildEpsilonNFA();
			}
			// build the result
			Automata.NFA automata = Automata.NFA.NewMinimal();
			Automata.NFAState intermediate = automata.AddNewState();
			AddUnicodeSpanToNFA(automata, intermediate, block.Span);
			return automata;
		}

		/// <summary>
		/// Builds a NFA from a unicode character span
		/// </summary>
		/// <param name="node">An AST node representing a NFA</param>
		/// <returns>The equivalent NFA</returns>
		private Automata.NFA BuildNFAFromUnicodeSpan(ASTNode node)
		{
			// extract the values
			int spanBegin = 0;
			int spanEnd = 0;
			spanBegin = System.Convert.ToInt32(node.Children[0].Symbol.Value.Substring(2), 16);
			spanEnd = System.Convert.ToInt32(node.Children[1].Symbol.Value.Substring(2), 16);
			if (spanBegin > spanEnd)
			{
				reporter.Error(string.Format("{0}{1} Invalid unicode character span, the end is before the beginning", resource, node.Position));
				return BuildEpsilonNFA();
			}
			// build the result
			Automata.NFA automata = Automata.NFA.NewMinimal();
			Automata.NFAState intermediate = automata.AddNewState();
			AddUnicodeSpanToNFA(automata, intermediate, new UnicodeSpan(spanBegin, spanEnd));
			return automata;
		}

		/// <summary>
		/// Builds a NFA that matches everything (a single character)
		/// </summary>
		/// <param name="node">An AST node representing a NFA</param>
		/// <returns>The equivalent NFA</returns>
		private Automata.NFA BuildNFAFromAny(ASTNode node)
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

		/// <summary>
		/// Builds a NFA from a referenced terminal
		/// </summary>
		/// <param name="node">An AST node representing a NFA</param>
		/// <returns>The equivalent NFA</returns>
		private Automata.NFA BuildNFAFromReference(ASTNode node)
		{
			// rerieve the reference
			Terminal reference = grammar.GetTerminalByName(node.Symbol.Value);
			if (reference == null)
			{
				reporter.Error(string.Format("{0}{1} Reference to unknown terminal {2}", resource, node.Position, node.Symbol.Value));
				return BuildEpsilonNFA();
			}
			return reference.NFA.Clone(false);
		}

		/// <summary>
		/// Loads the rules block of a grammar
		/// </summary>
		/// <param name="node">The AST node of a rules block</param>
		private void LoadBlockRules(ASTNode node)
		{
			// Create a new context
			Context context = new Context(this);
			// Add existing meta-rules that may have been inherited
			foreach (TemplateRule templateRule in grammar.TemplateRules)
				context.AddTemplateRule(templateRule);
			// Load new variables for the rules' head
			foreach (ASTNode child in node.Children)
			{
				if (child.Symbol.ID == HimeGrammarParser.ID.cf_rule_simple)
				{
					string name = child.Children[0].Symbol.Value;
					Variable var = grammar.GetVariable(name);
					if (var == null)
						var = grammar.AddVariable(name);
				}
				else if (child.Symbol.ID == HimeGrammarParser.ID.cf_rule_template)
					context.AddTemplateRule(grammar.AddTemplateRule(child));
			}
			// Load the grammar rules
			foreach (ASTNode child in node.Children)
			{
				if (child.Symbol.ID == HimeGrammarParser.ID.cf_rule_simple)
					LoadRule(context, child);
			}
		}

		/// <summary>
		/// Loads the syntactic rule in the given AST
		/// </summary>
		/// <param name="context">The current context</param>
		/// <param name="node">The AST node of a syntactic rule</param>
		private void LoadRule(Context context, ASTNode node)
		{
			string name = node.Children[0].Symbol.Value;
			Variable var = grammar.GetVariable(name);
			RuleBodySet defs = BuildDefinitions(context, node.Children[1]);
			foreach (RuleBody def in defs)
				var.AddRule(new Rule(var, def, false));
		}

		/// <summary>
		/// Builds the set of rule definitions that are represented by the given AST
		/// </summary>
		/// <param name="context">The current context</param>
		/// <param name="node">The AST node of a syntactic rule</param>
		/// <returns>The set of possible rule definitions</returns>
		public RuleBodySet BuildDefinitions(Context context, ASTNode node)
		{
			if (node.Symbol.ID == HimeGrammarLexer.ID.OPERATOR_OPTIONAL)
			{
				RuleBodySet setInner = BuildDefinitions(context, node.Children[0]);
				setInner.Insert(0, new RuleBody());
				return setInner;
			}
			else if (node.Symbol.ID == HimeGrammarLexer.ID.OPERATOR_ZEROMORE)
			{
				RuleBodySet setInner = BuildDefinitions(context, node.Children[0]);
				Variable subVar = grammar.GenerateVariable();
				foreach (RuleBody def in setInner)
					subVar.AddRule(new Rule(subVar, def, true));
				RuleBodySet setVar = new RuleBodySet();
				setVar.Add(new RuleBody(subVar));
				setVar = RuleBodySet.Multiply(setVar, setInner);
				foreach (RuleBody def in setVar)
					subVar.AddRule(new Rule(subVar, def, true));
				setVar = new RuleBodySet();
				setVar.Add(new RuleBody());
				setVar.Add(new RuleBody(subVar));
				return setVar;
			}
			else if (node.Symbol.ID == HimeGrammarLexer.ID.OPERATOR_ONEMORE)
			{
				RuleBodySet setInner = BuildDefinitions(context, node.Children[0]);
				Variable subVar = grammar.GenerateVariable();
				foreach (RuleBody def in setInner)
					subVar.AddRule(new Rule(subVar, def, true));
				RuleBodySet setVar = new RuleBodySet();
				setVar.Add(new RuleBody(subVar));
				setVar = RuleBodySet.Multiply(setVar, setInner);
				foreach (RuleBody def in setVar)
					subVar.AddRule(new Rule(subVar, def, true));
				setVar = new RuleBodySet();
				setVar.Add(new RuleBody(subVar));
				return setVar;
			}
			else if (node.Symbol.ID == HimeGrammarLexer.ID.OPERATOR_UNION)
			{
				RuleBodySet setLeft = BuildDefinitions(context, node.Children[0]);
				RuleBodySet setRight = BuildDefinitions(context, node.Children[1]);
				return RuleBodySet.Union(setLeft, setRight);
			}
			else if (node.Symbol.ID == HimeGrammarLexer.ID.TREE_ACTION_PROMOTE)
			{
				RuleBodySet setInner = BuildDefinitions(context, node.Children[0]);
				setInner.ApplyAction(TreeAction.Promote);
				return setInner;
			}
			else if (node.Symbol.ID == HimeGrammarLexer.ID.TREE_ACTION_DROP)
			{
				RuleBodySet setInner = BuildDefinitions(context, node.Children[0]);
				setInner.ApplyAction(TreeAction.Drop);
				return setInner;
			}
			else if (node.Symbol.Name == "concat")
			{
				RuleBodySet setLeft = BuildDefinitions(context, node.Children[0]);
				RuleBodySet setRight = BuildDefinitions(context, node.Children[1]);
				return RuleBodySet.Multiply(setLeft, setRight);
			}
			else if (node.Symbol.Name == "emptypart")
			{
				RuleBodySet set = new RuleBodySet();
				set.Add(new RuleBody());
				return set;
			}
			return BuildAtomicDefinition(context, node);
		}

		/// <summary>
		/// Builds the set of rule definitions that are represented by the given AST
		/// </summary>
		/// <param name="context">The current context</param>
		/// <param name="node">The AST node of a syntactic rule</param>
		/// <returns>The set of possible rule definitions</returns>
		private RuleBodySet BuildAtomicDefinition(Context context, ASTNode node)
		{
			if (node.Symbol.ID == HimeGrammarParser.ID.rule_sym_action)
				return BuildAtomicAction(node);
			if (node.Symbol.ID == HimeGrammarParser.ID.rule_sym_virtual)
				return BuildAtomicVirtual(node);
			if (node.Symbol.ID == HimeGrammarParser.ID.rule_sym_ref_simple)
				return BuildAtomicSimpleReference(context, node);
			if (node.Symbol.ID == HimeGrammarParser.ID.rule_sym_ref_template)
				return BuildAtomicTemplateReference(context, node);
			if (node.Symbol.ID == HimeGrammarLexer.ID.LITERAL_TEXT)
				return BuildAtomicInlineText(node);
			// nothing found ...
			reporter.Error(string.Format("{0}{1} Failed to recognize syntactic rule", resource, node.Position));
			RuleBodySet set = new RuleBodySet();
			set.Add(new RuleBody());
			return set;
		}

		/// <summary>
		/// Builds the set of rule definitions that represents a single semantic action
		/// </summary>
		/// <param name="context">The current context</param>
		/// <param name="node">The AST node of a syntactic rule</param>
		/// <returns>The set of possible rule definitions</returns>
		private RuleBodySet BuildAtomicAction(ASTNode node)
		{
			RuleBodySet set = new RuleBodySet();
			string name = node.Children[0].Symbol.Value;
			Action action = grammar.GetAction(name);
			if (action == null)
				action = grammar.AddAction(name);
			set.Add(new RuleBody(action));
			return set;
		}

		/// <summary>
		/// Builds the set of rule definitions that represents a single virtual symbol
		/// </summary>
		/// <param name="context">The current context</param>
		/// <param name="node">The AST node of a syntactic rule</param>
		/// <returns>The set of possible rule definitions</returns>
		private RuleBodySet BuildAtomicVirtual(ASTNode node)
		{
			RuleBodySet set = new RuleBodySet();
			string name = node.Children[0].Symbol.Value;
			name = name.Substring(1, name.Length - 2);
			Virtual vir = grammar.GetVirtual(name);
			if (vir == null)
				vir = grammar.AddVirtual(name);
			set.Add(new RuleBody(vir));
			return set;
		}

		/// <summary>
		/// Builds the set of rule definitions that represents a single reference to a simple variable
		/// </summary>
		/// <param name="context">The current context</param>
		/// <param name="node">The AST node of a syntactic rule</param>
		/// <returns>The set of possible rule definitions</returns>
		private RuleBodySet BuildAtomicSimpleReference(Context context, ASTNode node)
		{
			RuleBodySet defs = new RuleBodySet();
			Symbol symbol = ResolveSymbol(node.Children[0].Symbol.Value, context);
			if (symbol == null)
			{
				reporter.Error(string.Format("{0}{1} Unknown symbol {2} in rule definition", resource, node.Children[0].Position, node.Children[0].Symbol.Value));
				defs.Add(new RuleBody());
			}
			else
			{
				defs.Add(new RuleBody(symbol));
			}
			return defs;
		}

		/// <summary>
		/// Builds the set of rule definitions that represents a single reference to a template variable
		/// </summary>
		/// <param name="context">The current context</param>
		/// <param name="node">The AST node of a syntactic rule</param>
		/// <returns>The set of possible rule definitions</returns>
		private RuleBodySet BuildAtomicTemplateReference(Context context, ASTNode node)
		{
			RuleBodySet defs = new RuleBodySet();
			// Get the information
			string name = node.Children[0].Symbol.Value;
			int paramCount = node.Children[1].Children.Count;
			// check for meta-rule existence
			if (!context.IsTemplateRule(name, paramCount))
			{
				reporter.Error(string.Format("{0}{1} Unknown meta-rule {2}<{3}> in rule definition", resource, node.Children[0].Position, name, paramCount));
				defs.Add(new RuleBody());
				return defs;
			}
			// Recognize the parameters
			List<Symbol> parameters = new List<Symbol>();
			foreach (ASTNode symbolNode in node.Children[1].Children)
				parameters.Add(BuildAtomicDefinition(context, symbolNode)[0][0].Symbol);
			// Get the corresponding variable
			Variable variable = context.InstantiateMetaRule(name, parameters);
			// Create the definition
			defs.Add(new RuleBody(variable));
			return defs;
		}

		/// <summary>
		/// Builds the set of rule definitions that represents a single inline piece of text
		/// </summary>
		/// <param name="context">The current context</param>
		/// <param name="node">The AST node of a syntactic rule</param>
		/// <returns>The set of possible rule definitions</returns>
		private RuleBodySet BuildAtomicInlineText(ASTNode node)
		{
			// Construct the terminal name
			string value = node.Symbol.Value;
			value = value.Substring(1, value.Length - 2);
			value = ReplaceEscapees(value).Replace("\\'", "'");
			// Check for previous instance in the grammar
			Terminal terminal = grammar.GetTerminalByValue(value);
			if (terminal == null)
			{
				// Create the terminal
				Automata.NFA nfa = BuildNFAFromText(node);
				terminal = grammar.AddTerminalAnon(value, nfa);
				nfa.StateExit.Item = terminal;
			}
			// Create the definition set
			RuleBodySet set = new RuleBodySet();
			set.Add(new RuleBody(terminal));
			return set;
		}
	}
}