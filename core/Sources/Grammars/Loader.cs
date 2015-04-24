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
using Hime.Redist;
using Hime.Redist.Parsers;
using Hime.Redist.Utils;
using Hime.SDK.Input;

namespace Hime.SDK.Grammars
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
		/// The input from which the grammar is loaded
		/// </summary>
		private Text input;
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
		/// Gets the remaining unsolved dependencies
		/// </summary>
		/// <value>The remaining unsolved dependencies</value>
		public ROList<string> Dependencies { get { return new ROList<string>(inherited); } }

		/// <summary>
		/// Initializes this load
		/// </summary>
		/// <param name="resName">The name of the resource</param>
		/// <param name="input">The input from which the grammar is loaded</param>
		/// <param name="root">The root AST</param>
		/// <param name="reporter">The log</param>
		public Loader(string resName, Text input, ASTNode root, Reporter reporter)
		{
			this.reporter = reporter;
			this.root = root;
			this.resource = resName;
			this.input = input;
			this.inherited = new List<string>();
			foreach (ASTNode child in root.Children[1].Children)
				inherited.Add(child.Value);
			this.grammar = new Grammar(root.Children[0].Value);
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
					OnErrorNoContext("Grammar {0} inherited by {1} cannot be found", parent, grammar.Name);
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
		/// Raises an error in this loader
		/// </summary>
		/// <param name="message">The error's message</param>
		/// <param name="args">The message's arguments</param>
		private void OnErrorNoContext(string message, params object[] args)
		{
			OnError(new TextPosition(0, 0), message, args);
		}

		/// <summary>
		/// Raises an error in this loader
		/// </summary>
		/// <param name="position">The error's position in the input</param>
		/// <param name="message">The error's message</param>
		/// <param name="args">The message's arguments</param>
		private void OnError(TextPosition position, string message, params object[] args)
		{
			if (position.Line == 0 && position.Column == 0)
				reporter.Error(string.Format("{0}@{1} {2}", resource, new TextPosition(0, 0), string.Format(message, args)));
			else
				reporter.Error(string.Format("{0}@{1} {2}", resource, position, string.Format(message, args)), input, position);
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
		/// Adds a unicode character span to an existing NFA automaton
		/// </summary>
		/// <param name="automata">The target NFA</param>
		/// <param name="span">The unicode span to add</param>
		private void AddUnicodeSpanToNFA(Automata.NFA automata, UnicodeSpan span)
		{
			char[] b = span.Begin.GetUTF16();
			char[] e = span.End.GetUTF16();

			if (span.IsPlane0)
			{
				// this span is entirely in plane 0
				automata.StateEntry.AddTransition(new CharSpan(b[0], e[0]), automata.StateExit);
			}
			else if (span.Begin.IsPlane0)
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
				Automata.NFAState intermediate = automata.AddNewState();
				automata.StateEntry.AddTransition(new CharSpan((char)0xD800, e[0]), intermediate);
				intermediate.AddTransition(new CharSpan((char)0xDC00, e[1]), automata.StateExit);
			}
			else
			{
				// this span has no part in plane 0
				if (b[0] == e[0])
				{
					// same first surrogate
					Automata.NFAState intermediate = automata.AddNewState();
					automata.StateEntry.AddTransition(new CharSpan(b[0], b[0]), intermediate);
					intermediate.AddTransition(new CharSpan(b[1], e[1]), automata.StateExit);
				}
				else if (e[0] == b[0] + 1)
				{
					// the first surrogates are consecutive encodings
					// build lower half
					Automata.NFAState i1 = automata.AddNewState();
					automata.StateEntry.AddTransition(new CharSpan(b[0], b[0]), i1);
					i1.AddTransition(new CharSpan(b[1], (char)0xDFFF), automata.StateExit);
					// build upper half
					Automata.NFAState i2 = automata.AddNewState();
					automata.StateEntry.AddTransition(new CharSpan(e[0], e[0]), i2);
					i2.AddTransition(new CharSpan((char)0xDC00, e[1]), automata.StateExit);
				}
				else
				{
					// there is at least one surrogate value between the first surrogates of begin and end
					// build lower part
					Automata.NFAState ia = automata.AddNewState();
					automata.StateEntry.AddTransition(new CharSpan(b[0], b[0]), ia);
					ia.AddTransition(new CharSpan(b[1], (char)0xDFFF), automata.StateExit);
					// build intermediate part
					Automata.NFAState im = automata.AddNewState();
					automata.StateEntry.AddTransition(new CharSpan((char)(b[0] + 1), (char)(e[0] - 1)), im);
					im.AddTransition(new CharSpan((char)0xDC00, (char)0xDFFF), automata.StateExit);
					// build upper part
					Automata.NFAState iz = automata.AddNewState();
					automata.StateEntry.AddTransition(new CharSpan(e[0], e[0]), iz);
					iz.AddTransition(new CharSpan((char)0xDC00, e[1]), automata.StateExit);
				}
			}
		}

		/// <summary>
		/// Loads the content of the grammar in the given AST
		/// </summary>
		/// <param name="node">The AST node representing a grammar</param>
		private void LoadGrammarContent(ASTNode node)
		{
			reporter.Info("Loading grammar " + grammar.Name + " ...");
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
			string name = node.Children[0].Value;
			string value = node.Children[1].Value;
			value = ReplaceEscapees(value.Substring(1, value.Length - 2));
			grammar.AddOption(name, value);
		}

		/// <summary>
		/// Loads the terminal blocks of a grammar
		/// </summary>
		/// <param name="node">The terminal block's AST node</param>
		private void LoadBlockTerminals(ASTNode node)
		{
			foreach (ASTNode child in node.Children)
			{
				switch (child.Symbol.ID)
				{
					case HimeGrammarLexer.ID.BLOCK_CONTEXT:
						LoadTerminalContext(child);
						break;
					case HimeGrammarParser.ID.terminal_rule:
						LoadTerminalRule(child, null);
						break;
				}
			}
		}

		/// <summary>
		/// Loads the terminal context in the given AST
		/// </summary>
		/// <param name="node">The AST node of a terminal context</param>
		private void LoadTerminalContext(ASTNode node)
		{
			ASTFamily children = node.Children;
			string name = children[0].Value;
			for (int i=1; i!=children.Count; i++)
				LoadTerminalRule(children[i], name);
		}

		/// <summary>
		/// Loads the terminal rule in the given AST
		/// </summary>
		/// <param name="node">The AST node of a terminal rule</param>
		/// <param name="context">The current context</param>
		private void LoadTerminalRule(ASTNode node, string context)
		{
			ASTNode nameNode = node.Children[0];
			// Resolve the terminal
			Terminal terminal = grammar.GetTerminalByName(nameNode.Value);
			if (terminal == null)
			{
				// The terminal does not already exists
				// Build the NFA
				Automata.NFA nfa = BuildNFA(node.Children[1]);
				// Create the terminal in the grammar
				terminal = grammar.AddTerminalNamed(nameNode.Value, nfa, context);
				// Marks the final NFA state with the new terminal
				nfa.StateExit.AddItem(terminal);
			}
			else
			{
				// Tried to override the terminal
				OnError(nameNode.Position, "Overriding the definition of terminal {0}", nameNode.Value);
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
				int min = System.Convert.ToInt32(node.Children[1].Value);
				int max = min;
				if (node.Children.Count > 2)
					max = System.Convert.ToInt32(node.Children[2].Value);
				return Automata.NFA.NewRepeatRange(inner, false, min, max);
			}
			if (symbol.Name == "concat")
			{
				Automata.NFA left = BuildNFA(node.Children[0]);
				Automata.NFA right = BuildNFA(node.Children[1]);
				return Automata.NFA.NewConcatenation(left, right, false);
			}

			// nothing found ...
			OnError(node.Position, "Failed to recognize lexical rule");
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
			string value = node.Value;
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
			string value = node.Value;
			value = value.Substring(2, value.Length - 2);
			int cpValue = System.Convert.ToInt32(value, 16);
			if (cpValue < 0 || (cpValue >= 0xD800 && cpValue <= 0xDFFF) || cpValue >= 0x110000)
			{
				OnError(node.Position, "The value U+{0} is not a supported unicode code point", cpValue.ToString("X"));
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
			string value = node.Value;
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
					OnError(node.Position, "Unsupported non-plane 0 Unicode character ({0}) in character class", b + value[i + 1]);
					return BuildEpsilonNFA();
				}
				if ((i != value.Length - 1) && (value[i + 1] == '-'))
				{
					// this is a range, match the '-'
					i += 2;
					char e = value[i];
					if (e >= 0xD800 && e <= 0xDFFF)
					{
						OnError(node.Position, "Unsupported non-plane 0 Unicode character ({0}) in character class", e + value[i + 1]);
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
			string value = node.Value.Substring(3, node.Value.Length - 4);
			UnicodeCategory category = UnicodeCategories.GetCategory(value);
			if (category == null)
			{
				OnError(node.Position, "Unknown unicode category {0}", value);
				return BuildEpsilonNFA();
			}
			// build the result
			Automata.NFA automata = Automata.NFA.NewMinimal();
			foreach (UnicodeSpan span in category.Spans)
				AddUnicodeSpanToNFA(automata, span);
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
			string value = node.Value.Substring(3, node.Value.Length - 4);
			UnicodeBlock block = UnicodeBlocks.GetBlock(value);
			if (block == null)
			{
				OnError(node.Position, "Unknown unicode block {0}", value);
				return BuildEpsilonNFA();
			}
			// build the result
			Automata.NFA automata = Automata.NFA.NewMinimal();
			AddUnicodeSpanToNFA(automata, block.Span);
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
			spanBegin = System.Convert.ToInt32(node.Children[0].Value.Substring(2), 16);
			spanEnd = System.Convert.ToInt32(node.Children[1].Value.Substring(2), 16);
			if (spanBegin > spanEnd)
			{
				OnError(node.Position, "Invalid unicode character span, the end is before the beginning");
				return BuildEpsilonNFA();
			}
			// build the result
			Automata.NFA automata = Automata.NFA.NewMinimal();
			AddUnicodeSpanToNFA(automata, new UnicodeSpan(spanBegin, spanEnd));
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
			Terminal reference = grammar.GetTerminalByName(node.Value);
			if (reference == null)
			{
				OnError(node.Position, "Reference to unknown terminal {0}", node.Value);
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
					string name = child.Children[0].Value;
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
			string name = node.Children[0].Value;
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
			OnError(node.Position, "Failed to recognize syntactic rule");
			RuleBodySet set = new RuleBodySet();
			set.Add(new RuleBody());
			return set;
		}

		/// <summary>
		/// Builds the set of rule definitions that represents a single semantic action
		/// </summary>
		/// <param name="node">The AST node of a syntactic rule</param>
		/// <returns>The set of possible rule definitions</returns>
		private RuleBodySet BuildAtomicAction(ASTNode node)
		{
			RuleBodySet set = new RuleBodySet();
			string name = node.Children[0].Value;
			Action action = grammar.GetAction(name);
			if (action == null)
				action = grammar.AddAction(name);
			set.Add(new RuleBody(action));
			return set;
		}

		/// <summary>
		/// Builds the set of rule definitions that represents a single virtual symbol
		/// </summary>
		/// <param name="node">The AST node of a syntactic rule</param>
		/// <returns>The set of possible rule definitions</returns>
		private RuleBodySet BuildAtomicVirtual(ASTNode node)
		{
			RuleBodySet set = new RuleBodySet();
			string name = node.Children[0].Value;
			name = ReplaceEscapees(name.Substring(1, name.Length - 2));
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
			Symbol symbol = ResolveSymbol(node.Children[0].Value, context);
			if (symbol == null)
			{
				OnError(node.Children[0].Position, "Unknown symbol {0} in rule definition", node.Children[0].Value);
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
			string name = node.Children[0].Value;
			int paramCount = node.Children[1].Children.Count;
			// check for meta-rule existence
			if (!context.IsTemplateRule(name, paramCount))
			{
				OnError(node.Children[0].Position, "Unknown meta-rule {0}<{1}> in rule definition", name, paramCount);
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
		/// <param name="node">The AST node of a syntactic rule</param>
		/// <returns>The set of possible rule definitions</returns>
		private RuleBodySet BuildAtomicInlineText(ASTNode node)
		{
			// Construct the terminal name
			string value = node.Value;
			value = value.Substring(1, value.Length - 2);
			value = ReplaceEscapees(value).Replace("\\'", "'");
			// Check for previous instance in the grammar
			Terminal terminal = grammar.GetTerminalByValue(value);
			if (terminal == null)
			{
				// Create the terminal
				Automata.NFA nfa = BuildNFAFromText(node);
				terminal = grammar.AddTerminalAnon(value, nfa);
				nfa.StateExit.AddItem(terminal);
			}
			// Create the definition set
			RuleBodySet set = new RuleBodySet();
			set.Add(new RuleBody(terminal));
			return set;
		}

		/// <summary>
		/// Replaces the escape sequences in the given piece of text by their value
		/// </summary>
		/// <param name="value">A string</param>
		/// <returns>The string with the escape sequences replaced by their value</returns>
		public static string ReplaceEscapees(string value)
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
				c = value[i];
				if (c == '\\')
					builder.Append(c);
				else if (c == '0')
					builder.Append('\0'); /*Unicode character 0*/
				else if (c == 'a')
					builder.Append('\a'); /*Alert (character 7)*/
				else if (c == 'b')
					builder.Append('\b'); /*Backspace (character 8)*/
				else if (c == 'f')
					builder.Append('\f'); /*Form feed (character 12)*/
				else if (c == 'n')
					builder.Append('\n'); /*New line (character 10)*/
				else if (c == 'r')
					builder.Append('\r'); /*Carriage return (character 13)*/
				else if (c == 't')
					builder.Append('\t'); /*Horizontal tab (character 9)*/
				else if (c == 'v')
					builder.Append('\v'); /*Vertical quote (character 11)*/
				else if (c == 'u')
				{
					int l = 0;
					while (i + 1 + l < value.Length)
					{
						c = value[i + 1 + l];
						if ((c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F'))
							l++;
						else
							break;
					}
					if (l >= 8)
						l = 8;
					else if (l > 4)
						l = 4;
					int cp = System.Convert.ToInt32(value.Substring(i + 1, l), 16);
					builder.Append((new UnicodeCodePoint(cp)).GetUTF16());
					i += l;
				}
				else
					builder.Append("\\" + c);
			}
			return builder.ToString();
		}
	}
}