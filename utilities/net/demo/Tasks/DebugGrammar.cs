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

namespace Hime.Demo.Tasks
{
	/// <summary>
	/// This task demonstrates how to manually use the SDK API to output debug information on a grammar
	/// </summary>
	class DebugGrammar : IExecutable
	{
		/// <summary>
		///  Execute this instance.
		/// </summary>
		public void Execute()
		{
			string inlineGrammar = "grammar Demo { options {Axiom=\"e\";} terminals {K->ub{Katakana}; H->ub{Hiragana};} rules { e->K H; } }";
			// load the grammar
			Hime.SDK.Input.Loader loader = new Hime.SDK.Input.Loader();
			loader.AddInputRaw(inlineGrammar);
			List<Hime.SDK.Grammars.Grammar> grammars = loader.Load();
			// prepare
			Hime.SDK.Grammars.Grammar grammar = grammars[0];
			grammar.Prepare();
			Hime.SDK.Reflection.Serializers.Export(grammar, "Demo.Grammar.txt");
			// get the NFA for a single lexical rule
			Hime.SDK.Grammars.Terminal terminalK = grammar.GetTerminalByName("K");
			Hime.SDK.Reflection.Serializers.ExportDOT(terminalK.NFA, "Demo.NFA_K.dot");
			// build the global DFA an output it
			Hime.SDK.Automata.DFA dfa = grammar.BuildDFA();
			Hime.SDK.Reflection.Serializers.ExportDOT(dfa, "Demo.DFA.dot");
			// build the LALR(1) automaton and output it
			Hime.SDK.Grammars.LR.Builder builder = new Hime.SDK.Grammars.LR.Builder(grammar);
			Hime.SDK.Grammars.LR.Graph graph = builder.Build(Hime.SDK.ParsingMethod.LALR1);
			foreach (Hime.SDK.Grammars.LR.Conflict conflict in builder.Conflicts)
				System.Console.WriteLine(conflict);
			Hime.SDK.Reflection.Serializers.ExportDOT(graph, "Demo.LR.dot");
			Hime.SDK.Reflection.Serializers.Export(graph, "Demo.LR.txt");
		}
	}
}