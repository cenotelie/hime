/**********************************************************************
* Copyright (c) 2014 Laurent Wouters and others
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
using System.Reflection;
using Hime.Redist;
using Hime.Redist.Utils;

namespace Hime.SDK.Reflection
{
	/// <summary>
	/// Utilities to decompile a parser produced by Central Dogma
	/// </summary>
	public class ParserReflection
	{
		/// <summary>
		/// List of the terminals matched by the associated lexer
		/// </summary>
		private IList<Symbol> terminals;
		/// <summary>
		/// List of the variables in this parser
		/// </summary>
		private IList<Symbol> variables;
		/// <summary>
		/// List of the virtuals in this parser
		/// </summary>
		private IList<Symbol> virtuals;
		/// <summary>
		/// The LR automaton used by the parser
		/// </summary>
		private Automata.LRAutomaton automaton;

		/// <summary>
		/// Gets the terminals that can be matched by the associated lexer
		/// </summary>
		public ROList<Symbol> Terminals { get { return new ROList<Symbol>(terminals); } }
		/// <summary>
		/// Gets the variable symbols used by this parser
		/// </summary>
		public ROList<Symbol> Variables { get { return new ROList<Symbol>(variables); } }
		/// <summary>
		/// Gets the virtual symbols used by this parser
		/// </summary>
		public ROList<Symbol> Virtuals { get { return new ROList<Symbol>(virtuals); } }
		/// <summary>
		/// Gets this parser's LR automaton
		/// </summary>
		public Automata.LRAutomaton Automaton { get { return automaton; } }

		/// <summary>
		/// Initializes this parser reflection
		/// </summary>
		/// <param name="parserType">The parser's type</param>
		public ParserReflection(System.Type parserType)
		{
			string input = "";
			ConstructorInfo[] ctors = parserType.GetConstructors();
			ConstructorInfo parserCtor = null;
			System.Type lexerType = null;
			ConstructorInfo lexerCtor = null;
			for (int i=0; i!=ctors.Length; i++)
			{
				ParameterInfo[] parameters = ctors[i].GetParameters();
				if (parameters.Length == 1)
				{
					parserCtor = ctors[i];
					lexerType = parameters[0].ParameterType;
					break;
				}
			}
			lexerCtor = lexerType.GetConstructor(new System.Type[] { typeof(string) });
			object lexer = lexerCtor.Invoke(new object[] { input });
			object parser = parserCtor.Invoke(new object[] { lexer });

			this.terminals = (lexer as Hime.Redist.Lexer.ILexer).Terminals;
			this.variables = (parser as Hime.Redist.Parsers.IParser).Variables;
			this.virtuals = (parser as Hime.Redist.Parsers.IParser).Virtuals;
			this.automaton = new Automata.LRAutomaton();

			string[] resources = parserType.Assembly.GetManifestResourceNames();
			Stream stream = null;
			foreach (string existing in resources)
			{
				if (existing.EndsWith(parserType.Name + ".bin"))
				{
					stream = lexerType.Assembly.GetManifestResourceStream(existing);
					break;
				}
			}

			BinaryReader reader = new BinaryReader(stream);
			if ((typeof(Hime.Redist.Parsers.LRkParser).IsAssignableFrom(parserType)))
			    LoadLRk(reader);
			  else
				LoadRNGLR(reader);
			reader.Close();
		}

		/// <summary>
		/// Loads the automaton as a LR(k) automaton
		/// </summary>
		/// <param name="reader">The input reader</param>
		private void LoadLRk(BinaryReader reader)
		{
			Hime.Redist.Parsers.LRkAutomaton temp = new Hime.Redist.Parsers.LRkAutomaton(reader);
			for (int i=0; i!=temp.StatesCount; i++)
				this.automaton.AddState(new Automata.LRState(i));
			for (int i=0; i!=temp.StatesCount; i++)
			{
				Automata.LRState state = automaton.States[i];
				foreach (Symbol terminal in terminals)
				{
					Hime.Redist.Parsers.LRAction action = temp.GetAction(i, terminal.ID);
					switch (action.Code)
					{
						case Hime.Redist.Parsers.LRActionCode.Accept:
							state.IsAccept = true;
							break;
						case Hime.Redist.Parsers.LRActionCode.Shift:
							state.AddTransition(new Automata.LRTransition(terminal, automaton.States[action.Data]));
							break;
						case Hime.Redist.Parsers.LRActionCode.Reduce:
							Hime.Redist.Parsers.LRProduction prod = temp.GetProduction(action.Data);
							state.AddReduction(new Automata.LRReduction(terminal, variables[prod.Head], prod.ReductionLength));
							break;
					}
				}
				foreach (Symbol variable in variables)
				{
					Hime.Redist.Parsers.LRAction action = temp.GetAction(i, variable.ID);
					if (action.Code == Hime.Redist.Parsers.LRActionCode.Shift)
					{
						state.AddTransition(new Automata.LRTransition(variable, automaton.States[action.Data]));
					}
				}
			}
		}

		/// <summary>
		/// Loads the automaton as a RNGLR automaton
		/// </summary>
		/// <param name="reader">The input reader</param>
		private void LoadRNGLR(BinaryReader reader)
		{
			Hime.Redist.Parsers.RNGLRAutomaton temp = new Hime.Redist.Parsers.RNGLRAutomaton(reader);
			for (int i=0; i!=temp.StatesCount; i++)
				this.automaton.AddState(new Automata.LRState(i));
			for (int i=0; i!=temp.StatesCount; i++)
			{
				Automata.LRState state = automaton.States[i];
				foreach (Symbol terminal in terminals)
				{
					int count = temp.GetActionsCount(i, terminal.ID);
					for (int j=0; j!=count; j++)
					{
						Hime.Redist.Parsers.LRAction action = temp.GetAction(i, terminal.ID, j);
						switch (action.Code)
						{
							case Hime.Redist.Parsers.LRActionCode.Accept:
								state.IsAccept = true;
								break;
							case Hime.Redist.Parsers.LRActionCode.Shift:
								state.AddTransition(new Automata.LRTransition(terminal, automaton.States[action.Data]));
								break;
							case Hime.Redist.Parsers.LRActionCode.Reduce:
								Hime.Redist.Parsers.LRProduction prod = temp.GetProduction(action.Data);
								state.AddReduction(new Automata.LRReduction(terminal, variables[prod.Head], prod.ReductionLength));
								break;
						}
					}
				}
				foreach (Symbol variable in variables)
				{
					int count = temp.GetActionsCount(i, variable.ID);
					for (int j=0; j!=count; j++)
					{
						Hime.Redist.Parsers.LRAction action = temp.GetAction(i, variable.ID, j);
						if (action.Code == Hime.Redist.Parsers.LRActionCode.Shift)
						{
							state.AddTransition(new Automata.LRTransition(variable, automaton.States[action.Data]));
						}
					}
				}
			}
		}
	}
}