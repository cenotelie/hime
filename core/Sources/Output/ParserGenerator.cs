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
using System.IO;

namespace Hime.CentralDogma.Output
{
	/// <summary>
	/// Represents a generator of data and code for a parser
	/// </summary>
	public abstract class ParserGenerator
	{
		/// <summary>
		/// The grammar to generate a parser for
		/// </summary>
		protected Grammars.Grammar grammar;
		/// <summary>
		/// LR graph for the parser
		/// </summary>
		protected Grammars.LR.Graph graph;
		/// <summary>
		/// The terminals matched by the associated lexer
		/// </summary>
		protected ROList<Grammars.Terminal> terminals;
		/// <summary>
		/// The variables to be exported
		/// </summary>
		protected List<Grammars.Variable> variables;
		/// <summary>
		/// The virtual symbols to be exported
		/// </summary>
		protected List<Grammars.Virtual> virtuals;
		/// <summary>
		/// The action symbols to be exported
		/// </summary>
		protected List<Grammars.Action> actions;
		/// <summary>
		/// The grammar rules
		/// </summary>
		protected List<Grammars.Rule> rules;

		/// <summary>
		/// Gets the name of the base parser class for the code generation
		/// </summary>
		protected abstract string BaseClassName { get; }

		/// <summary>
		/// Initializes this parser generator
		/// </summary>
		/// <param name="gram">The grammar to generate a parser for</param>
		/// <param name="graph">The LR graph to use</param>
		/// <param name="expected">The terminals matched by the associated lexer</param>
		public ParserGenerator(Grammars.Grammar gram, Grammars.LR.Graph graph, ROList<Grammars.Terminal> expected)
		{
			this.grammar = gram;
			this.graph = graph;
			this.terminals = expected;
			this.variables = new List<Grammars.Variable>(gram.Variables);
			this.virtuals = new List<Grammars.Virtual>(gram.Virtuals);
			this.actions = new List<Grammars.Action>(gram.Actions);
			this.rules = new List<Grammars.Rule>(gram.Rules);
		}

		/// <summary>
		/// Generates the parser's code
		/// </summary>
		/// <param name="stream">The output stream</param>
		/// <param name="name">The parser's name</param>
		/// <param name="modifier">The parser's visibility modifier</param>
		/// <param name="resource">The name of the associated binary data resource</param>
		public void GenerateCode(StreamWriter stream, string name, Modifier modifier, string resource)
		{
			stream.WriteLine("\t/// <summary>");
			stream.WriteLine("\t/// Represents a parser");
			stream.WriteLine("\t/// </summary>");
			stream.WriteLine("\t" + modifier.ToString().ToLower() + " class " + name + "Parser : " + this.BaseClassName);
			stream.WriteLine("\t{");
			GenerateCodeAutomaton(stream, name, resource);
			GenerateCodeSymbols(stream);
			GenerateCodeVariables(stream);
			GenerateCodeVirtuals(stream);
			GenerateCodeActions(stream);
			GeneratorCodeConstructors(stream, name);
			stream.WriteLine("\t}");
		}

		/// <summary>
		/// Generates the parser's binary data
		/// </summary>
		/// <param name="stream">The output stream</param>
		public abstract void GenerateData(BinaryWriter stream);

		/// <summary>
		/// Generates the code for the automaton
		/// </summary>
		/// <param name="stream">The output stream</param>
		/// <param name="name">The parser's name</param>
		/// <param name="resource">The name of the associated binary data resource</param>
		protected abstract void GenerateCodeAutomaton(StreamWriter stream, string name, string resource);

		/// <summary>
		/// Generates the code for the symbols
		/// </summary>
		/// <param name="stream">The output stream</param>
		protected void GenerateCodeSymbols(StreamWriter stream)
		{
			stream.WriteLine("\t\t/// <summary>");
			stream.WriteLine("\t\t/// Contains the constant IDs for the variables and virtuals in this parser");
			stream.WriteLine("\t\t/// </summary>");
			stream.WriteLine("\t\tpublic sealed class ID");
			stream.WriteLine("\t\t{");
			foreach (Grammars.Variable var in variables)
			{
				if (var.Name.StartsWith(Grammars.Grammar.prefixGeneratedVariable))
					continue;
				stream.WriteLine("\t\t\t/// <summary>");
				stream.WriteLine("\t\t\t/// The unique identifier for variable " + var.Name);
				stream.WriteLine("\t\t\t/// </summary>");
				stream.WriteLine("\t\t\tpublic const int {0} = 0x{1};", Helper.SanitizeName(var), var.ID.ToString("X4"));
			}
			foreach (Grammars.Virtual var in virtuals)
			{
				stream.WriteLine("\t\t\t/// <summary>");
				stream.WriteLine("\t\t\t/// The unique identifier for virtual " + var.Name);
				stream.WriteLine("\t\t\t/// </summary>");
				stream.WriteLine("\t\t\tpublic const int {0} = 0x{1};", Helper.SanitizeName(var), var.ID.ToString("X4"));
			}
			stream.WriteLine("\t\t}");
		}

		/// <summary>
		/// Generates the code for the variables
		/// </summary>
		/// <param name="stream">The output stream</param>
		protected void GenerateCodeVariables(StreamWriter stream)
		{
			stream.WriteLine("\t\t/// <summary>");
			stream.WriteLine("\t\t/// The collection of variables matched by this parser");
			stream.WriteLine("\t\t/// </summary>");
			stream.WriteLine("\t\t/// <remarks>");
			stream.WriteLine("\t\t/// The variables are in an order consistent with the automaton,");
			stream.WriteLine("\t\t/// so that variable indices in the automaton can be used to retrieve the variables in this table");
			stream.WriteLine("\t\t/// </remarks>");
			stream.WriteLine("\t\tprivate static readonly Symbol[] variables = {");
			bool first = true;
			foreach (Grammars.Variable var in variables)
			{
				if (!first)
					stream.WriteLine(", ");
				stream.Write("\t\t\t");
				stream.Write("new Symbol(0x" + var.ID.ToString("X4") + ", \"" + var.Name + "\")");
				first = false;
			}
			stream.WriteLine(" };");
		}

		/// <summary>
		/// Generates the code for the virtual symbols
		/// </summary>
		/// <param name="stream">The output stream</param>
		protected void GenerateCodeVirtuals(StreamWriter stream)
		{
			stream.WriteLine("\t\t/// <summary>");
			stream.WriteLine("\t\t/// The collection of virtuals matched by this parser");
			stream.WriteLine("\t\t/// </summary>");
			stream.WriteLine("\t\t/// <remarks>");
			stream.WriteLine("\t\t/// The virtuals are in an order consistent with the automaton,");
			stream.WriteLine("\t\t/// so that virtual indices in the automaton can be used to retrieve the virtuals in this table");
			stream.WriteLine("\t\t/// </remarks>");
			stream.WriteLine("\t\tprivate static readonly Symbol[] virtuals = {");
			bool first = true;
			foreach (Grammars.Virtual v in virtuals)
			{
				if (!first)
					stream.WriteLine(", ");
				stream.Write("\t\t\t");
				stream.Write("new Symbol(0x" + v.ID.ToString("X4") + ", \"" + v.Name + "\")");
				first = false;
			}
			stream.WriteLine(" };");
		}

		/// <summary>
		/// Generates the code for the semantic actions
		/// </summary>
		/// <param name="stream">The output stream</param>
		protected void GenerateCodeActions(StreamWriter stream)
		{
			if (actions.Count == 0)
				return;
			stream.WriteLine("\t\t/// <summary>");
			stream.WriteLine("\t\t/// Represents a set of semantic actions in this parser");
			stream.WriteLine("\t\t/// </summary>");
			stream.WriteLine("\t\tpublic class Actions");
			stream.WriteLine("\t\t{");
			foreach (Grammars.Action action in actions)
			{
				stream.WriteLine("\t\t\t/// <summary>");
				stream.WriteLine("\t\t\t/// The " + action.Name + " semantic action");
				stream.WriteLine("\t\t\t/// </summary>");
				stream.WriteLine("\t\t\tpublic virtual void " + action.Name + "(Symbol head, SemanticBody body) { }");
			}
			stream.WriteLine();
			stream.WriteLine("\t\t}");

			stream.WriteLine("\t\t/// <summary>");
			stream.WriteLine("\t\t/// Represents a set of empty semantic actions (do nothing)");
			stream.WriteLine("\t\t/// </summary>");
			stream.WriteLine("\t\tprivate static readonly Actions noActions = new Actions();");

			stream.WriteLine("\t\t/// <summary>");
			stream.WriteLine("\t\t/// Gets the set of semantic actions in the form a table consistent with the automaton");
			stream.WriteLine("\t\t/// </summary>");
			stream.WriteLine("\t\t/// <param name=\"input\">A set of semantic actions</param>");
			stream.WriteLine("\t\t/// <returns>A table of semantic actions</returns>");
			stream.WriteLine("\t\tprivate static SemanticAction[] GetUserActions(Actions input)");
			stream.WriteLine("\t\t{");
			stream.WriteLine("\t\t\tSemanticAction[] result = new SemanticAction[" + actions.Count + "];");
			for (int i = 0; i != actions.Count; i++)
				stream.WriteLine("\t\t\tresult[" + i + "] = new SemanticAction(input." + actions[i].Name + ");");
			stream.WriteLine("\t\t\treturn result;");
			stream.WriteLine("\t\t}");

			stream.WriteLine("\t\t/// <summary>");
			stream.WriteLine("\t\t/// Gets the set of semantic actions in the form a table consistent with the automaton");
			stream.WriteLine("\t\t/// </summary>");
			stream.WriteLine("\t\t/// <param name=\"input\">A set of semantic actions</param>");
			stream.WriteLine("\t\t/// <returns>A table of semantic actions</returns>");
			stream.WriteLine("\t\tprivate static SemanticAction[] GetUserActions(Dictionary<string, SemanticAction> input)");
			stream.WriteLine("\t\t{");
			stream.WriteLine("\t\t\tSemanticAction[] result = new SemanticAction[" + actions.Count + "];");
			for (int i = 0; i != actions.Count; i++)
				stream.WriteLine("\t\t\tresult[" + i + "] = input[\"" + actions[i].Name + "\"];");
			stream.WriteLine("\t\t\treturn result;");
			stream.WriteLine("\t\t}");
		}

		/// <summary>
		/// Generates the code for the constructors
		/// </summary>
		/// <param name="stream">The output stream</param>
		/// <param name="name">The parser's name</param>
		protected virtual void GeneratorCodeConstructors(StreamWriter stream, string name)
		{
			if (actions.Count == 0)
			{
				stream.WriteLine("\t\t/// <summary>");
				stream.WriteLine("\t\t/// Initializes a new instance of the parser");
				stream.WriteLine("\t\t/// </summary>");
				stream.WriteLine("\t\t/// <param name=\"lexer\">The input lexer</param>");
				stream.WriteLine("\t\tpublic " + name + "Parser(" + name + "Lexer lexer) : base (automaton, variables, virtuals, null, lexer) { }");
			}
			else
			{
				stream.WriteLine("\t\t/// <summary>");
				stream.WriteLine("\t\t/// Initializes a new instance of the parser");
				stream.WriteLine("\t\t/// </summary>");
				stream.WriteLine("\t\t/// <param name=\"lexer\">The input lexer</param>");
				stream.WriteLine("\t\tpublic " + name + "Parser(" + name + "Lexer lexer) : base (automaton, variables, virtuals, GetUserActions(noActions), lexer) { }");

				stream.WriteLine("\t\t/// <summary>");
				stream.WriteLine("\t\t/// Initializes a new instance of the parser");
				stream.WriteLine("\t\t/// </summary>");
				stream.WriteLine("\t\t/// <param name=\"lexer\">The input lexer</param>");
				stream.WriteLine("\t\t/// <param name=\"actions\">The set of semantic actions</param>");
				stream.WriteLine("\t\tpublic " + name + "Parser(" + name + "Lexer lexer, Actions actions) : base (automaton, variables, virtuals, GetUserActions(actions), lexer) { }");

				stream.WriteLine("\t\t/// <summary>");
				stream.WriteLine("\t\t/// Initializes a new instance of the parser");
				stream.WriteLine("\t\t/// </summary>");
				stream.WriteLine("\t\t/// <param name=\"lexer\">The input lexer</param>");
				stream.WriteLine("\t\t/// <param name=\"actions\">The set of semantic actions</param>");
				stream.WriteLine("\t\tpublic " + name + "Parser(" + name + "Lexer lexer, Dictionary<string, SemanticAction> actions) : base (automaton, variables, virtuals, GetUserActions(actions), lexer) { }");
			}
		}
	}
}
