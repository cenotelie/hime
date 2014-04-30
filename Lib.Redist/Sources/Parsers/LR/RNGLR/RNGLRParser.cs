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
using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents a base for all RNGLR parsers
	/// </summary>
	public class RNGLRParser : BaseLRParser
	{
		/// <summary>
		/// Represents a reduction operation to be performed
		/// </summary>
		/// <remarks>
		/// For reduction of length 0, the node is the GSS node on which it is applied, the first label then is epsilon
		/// For others, the node is the SECOND GSS node on the path, not the head. The first label is then the label on the transition from the head
		/// </remarks>
		private struct Reduction
		{
			/// <summary>
			/// The GSS node to reduce from
			/// </summary>
			public int node;
			/// <summary>
			/// The LR production for the reduction
			/// </summary>
			public LRProduction prod;
			/// <summary>
			/// The first label in the GSS
			/// </summary>
			public GSSLabel first;
			/// <summary>
			/// Initializes this operation
			/// </summary>
			/// <param name="node">The GSS node to reduce from</param>
			/// <param name="prod">The LR production for the reduction</param>
			/// <param name="first">The first label in the GSS</param>
			public Reduction(int node, LRProduction prod, GSSLabel first)
			{
				this.node = node;
				this.prod = prod;
				this.first = first;
			}
		}

		/// <summary>
		/// Represents a shift operation to be performed
		/// </summary>
		private struct Shift
		{
			/// <summary>
			/// GSS node to shift from
			/// </summary>
			public int from;
			/// <summary>
			/// The target RNGLR state
			/// </summary>
			public int to;
			/// <summary>
			/// Initializes this operation
			/// </summary>
			/// <param name="from">The GSS node to shift from</param>
			/// <param name="to">The target RNGLR state</param>
			public Shift(int from, int to)
			{
				this.from = from;
				this.to = to;
			}
		}

		/// <summary>
		/// The parser automaton
		/// </summary>
		private RNGLRAutomaton parserAutomaton;
		/// <summary>
		/// The GSS for this parser
		/// </summary>
		private GSS gss;
		/// <summary>
		/// The SPPF being built
		/// </summary>
		private SPPF sppf;
		/// <summary>
		/// The sub-trees for the constant nullable variables
		/// </summary>
		private SubTree[] nullables;
		/// <summary>
		/// The next token
		/// </summary>
		private Token nextToken;
		/// <summary>
		/// The queue of reduction operations
		/// </summary>
		private Queue<Reduction> reductions;
		/// <summary>
		/// The queue of shift operations
		/// </summary>
		private Queue<Shift> shifts;

		/// <summary>
		/// Initializes a new instance of the LRkParser class with the given lexer
		/// </summary>
		/// <param name="automaton">The parser's automaton</param>
		/// <param name="variables">The parser's variables</param>
		/// <param name="virtuals">The parser's virtuals</param>
		/// <param name="actions">The parser's actions</param>
		/// <param name="lexer">The input lexer</param>
		public RNGLRParser(RNGLRAutomaton automaton, Symbol[] variables, Symbol[] virtuals, UserAction[] actions, Lexer.Lexer lexer)
            : base(variables, virtuals, actions, lexer)
		{
			this.parserAutomaton = automaton;
			this.gss = new GSS();
			this.sppf = new SPPF(maxStackSize, lexer.Output, parserVariables, parserVirtuals);
			//BuildNullables();
		}

		/*private void BuildNullables()
		{
			this.nullables = new SubTree[parserAutomaton.Nullables.Count];
			List<int>[] dependencies = BuildNullableDependencies();
			// Build nullables without dependencies
			for (int i = 0; i != parserAutomaton.Nullables.Count; i++)
			{
				List<int> dep = dependencies[i];
				if (dep != null && dep.Count == 0)
				{
					this.nullables[i] = BuildNullable(parserAutomaton.GetProduction(parserAutomaton.Nullables[i]));
					dependencies[i] = null;
				}
			}
		}

		private SubTree BuildNullable(LRProduction production)
		{
			int size = GetNullableSize(production);
			SubTree result = new SubTree(null, size);
			result.SetupRoot(new SymbolRef(SymbolType.Variable, production.Head), production.HeadAction);
			for (int i = 0; i != production.BytecodeLength; i++)
			{
				LROpCode op = production[i];
				switch (op.Base)
				{
					case LROpCodeBase.SemanticAction:
					{
						i++;
						break;
					}
					case LROpCodeBase.AddVirtual:
					{
						i++;
						break;
					}
					case LROpCodeBase.AddNullVariable:
					{
						i++;
						break;
					}
				}
			}
			return result;
		}

		private List<int>[] BuildNullableDependencies()
		{
			List<int>[] result = new List<int>[parserAutomaton.Nullables.Count];
			for (int i = 0; i != parserAutomaton.Nullables.Count; i++)
			{
				ushort index = parserAutomaton.Nullables[i];
				if (index != 0xFFFF)
				{
					LRProduction prod = parserAutomaton.GetProduction(index);
					result[i] = GetNullableDependencies(prod);
				}
			}
			return result;
		}

		private List<int> GetNullableDependencies(LRProduction production)
		{
			List<int> result = new List<int>();
			for (int i = 0; i != production.BytecodeLength; i++)
			{
				LROpCode op = production[i];
				switch (op.Base)
				{
					case LROpCodeBase.SemanticAction:
					{
						i++;
						break;
					}
					case LROpCodeBase.AddVirtual:
					{
						i++;
						break;
					}
					case LROpCodeBase.AddNullVariable:
					{
						result.Add(production[i + 1].DataValue);
						i++;
						break;
					}
				}
			}
			return result;
		}

		private int GetNullableSize(LRProduction production)
		{
			int result = 0;
			for (int i = 0; i != production.BytecodeLength; i++)
			{
				LROpCode op = production[i];
				switch (op.Base)
				{
					case LROpCodeBase.SemanticAction:
					{
						i++;
						break;
					}
					case LROpCodeBase.AddVirtual:
					{
						i++;
						result++;
						break;
					}
					case LROpCodeBase.AddNullVariable:
					{
						result += nullables[production[i + 1].DataValue].GetSize();
						i++;
						break;
					}
				}
			}
			return result;
		}*/

		/// <summary>
		/// Raises an error on an unexepcted token
		/// </summary>
		/// <param name="gen">The current GSS generation</param>
		/// <param name="token">The unexpected token</param>
		private void OnUnexpectedToken(int gen, Token token)
		{
			List<int> indices = new List<int>();
			List<Symbol> expected = new List<Symbol>();
			int count = 0;
			int start = gss.GetGeneration(gen, out count);
			for (int i=start; i!=start+count; i++)
			{
				ICollection<int> temp = parserAutomaton.GetExpected(gss.GetRepresentedState(i), lexer.Terminals.Count);
				foreach (int index in temp)
				{
					if (!indices.Contains(index))
					{
						indices.Add(index);
						expected.Add(lexer.Terminals[index]);
					}
				}
			}
			allErrors.Add(new UnexpectedTokenError(token, expected, lexer.Output));
		}

		/// <summary>
		/// Builds the SPPF
		/// </summary>
		/// <param name="generation">The current GSS generation</param>
		/// <param name="production">The LR production</param>
		/// <param name="first">The first label of the path</param>
		/// <param name="path">The reduction path</param>
		/// <returns>The corresponding SPPF part</returns>
		private GSSLabel BuildSPPF(int generation, LRProduction production, GSSLabel first, GSSPath path)
		{
			Symbol variable = parserVariables[production.Head];
			sppf.ReductionPrepare(production.Head, production.HeadAction, first, path, production.ReductionLength);
			for (int i = 0; i != production.BytecodeLength; i++)
			{
				LROpCode op = production[i];
				switch (op.Base)
				{
					case LROpCodeBase.SemanticAction:
						{
							UserAction action = parserActions[production[i + 1].DataValue];
							i++;
							action.Invoke(variable, sppf);
							break;
						}
					case LROpCodeBase.AddVirtual:
						{
							int index = production[i + 1].DataValue;
							sppf.ReductionAddVirtual(index, op.TreeAction);
							i++;
							break;
						}
					case LROpCodeBase.AddNullVariable:
						{
							int index = production[i + 1].DataValue;
							sppf.ReductionAddNullable(nullables[index], op.TreeAction);
							i++;
							break;
						}
					default:
						sppf.ReductionPop(op.TreeAction);
						break;
				}
			}
			return sppf.Reduce(generation, production.Head);
		}

		/// <summary>
		/// Parses the input and returns the produced AST
		/// </summary>
		/// <returns>AST produced by the parser representing the input, or null if unrecoverable errors were encountered</returns>
		public override ParseResult Parse()
		{
			nextToken = lexer.GetNextToken();
			if (nextToken.SymbolID == Lexer.Lexer.sidDollar)
			{
				// the input is empty!
				if (parserAutomaton.IsAcceptingState(0))
				{
					SubTree st = nullables[parserAutomaton.Axiom];
					// TODO: commit and build final AST here
					return null;
				}
				return null;
			}

			reductions = new Queue<Reduction>();
			shifts = new Queue<Shift>();
			int Ui = gss.CreateGeneration();
			int v0 = gss.CreateNode(0);

			int count = parserAutomaton.GetActionsCount(0, nextToken.SymbolID);
			for (int i = 0; i != count; i++)
			{
				LRAction action = parserAutomaton.GetAction(0, nextToken.SymbolID, i);
				if (action.Code == LRActionCode.Shift)
					shifts.Enqueue(new Shift(v0, action.Data));
				else if (action.Code == LRActionCode.Reduce)
					reductions.Enqueue(new Reduction(v0, parserAutomaton.GetProduction(action.Data), sppf.Epsilon));
			}

			while (nextToken.SymbolID != Lexer.Lexer.sidEpsilon) // Wait for ε token
			{
				Reducer(Ui);
				Token oldtoken = nextToken;
				nextToken = lexer.GetNextToken();
				int Uj = Shifter(oldtoken);
				gss.GetGeneration(Uj, out count);
				if (count == 0)
				{
					// Generation is empty !
					OnUnexpectedToken(Ui, oldtoken);
					return null;
				}
				Ui = Uj;
			}

			int start = gss.GetGeneration(Ui, out count);
			for (int i=start; i!=start+count; i++)
			{
				int state = gss.GetRepresentedState(i);
				if (parserAutomaton.IsAcceptingState(state))
				{
					// Has reduction _Axiom_ -> axiom $ . on ε
					GSSPath[] paths = gss.GetPaths(i, 2, out count);
					return null;
					//return new ParseResult(allErrors, lexer.Input, sppf.BuildTreeFrom(paths[0][1]));
				}
			}
			// At end of input but was still waiting for tokens
			return new ParseResult(allErrors, lexer.Output);
		}

		/// <summary>
		/// Executes the reduction operations from the given GSS generation
		/// </summary>
		/// <param name="generation">The current GSS generation</param>
		private void Reducer(int generation)
		{
			sppf.ClearHistory();
			while (reductions.Count != 0)
				ExecuteReduction(generation, reductions.Dequeue());
		}

		/// <summary>
		/// Executes a reduction operation for all found path
		/// </summary>
		/// <param name="generation">The current GSS generation</param>
		/// <param name="reduction">The reduction operation</param>
		private void ExecuteReduction(int generation, Reduction reduction)
		{
			// Get all path from the reduction node
			int count = 0;
			GSSPath[] paths = null;
			if (reduction.prod.ReductionLength == 0)
				paths = gss.GetPaths(reduction.node, 0, out count);
			else
                // The given GSS node is the second on the path, so start from it with length - 1
				paths = gss.GetPaths(reduction.node, reduction.prod.ReductionLength - 1, out count);

			// Execute the reduction on all paths
			for (int i = 0; i != count; i++)
				ExecuteReduction(generation, reduction, paths[i]);
		}

		/// <summary>
		/// Executes a reduction operation for a given path
		/// </summary>
		/// <param name="generation">The current GSS generation</param>
		/// <param name="reduction">The reduction operation</param>
		/// <param name="path">The GSS path to use for the reduction</param>
		private void ExecuteReduction(int generation, Reduction reduction, GSSPath path)
		{
			// Get the rule's head
			Symbol head = parserVariables[reduction.prod.Head];
			// Resolve the sub-root
			GSSLabel label = sppf.GetLabelFor(generation, new SymbolRef(SymbolType.Variable, reduction.prod.Head));
			if (label.IsEpsilon)
			{
				// not in history, build the SPPF here
				label = BuildSPPF(generation, reduction.prod, reduction.first, path);
			}

			// Get the target state by transition on the rule's head
			int to = GetNextByVar(gss.GetRepresentedState(path.Last), head.ID);
			// Find a node for the target state in the GSS
			int w = gss.FindNode(generation, to);
			if (w != -1)
			{
				// A node for the target state is already in the GSS
				if (!gss.HasEdge(generation, w, path.Last))
				{
					// But the new edge does not exist
					gss.CreateEdge(w, path.Last, label);
					// Look for the new reductions at this state
					if (reduction.prod.ReductionLength != 0)
					{
						int count = parserAutomaton.GetActionsCount(to, nextToken.SymbolID);
						for (int i = 0; i != count; i++)
						{
							LRAction action = parserAutomaton.GetAction(to, nextToken.SymbolID, i);
							if (action.Code == LRActionCode.Reduce)
							{
								LRProduction prod = parserAutomaton.GetProduction(action.Data);
								// length 0 reduction are not considered here because they already exist at this point
								if (prod.ReductionLength != 0)
									reductions.Enqueue(new Reduction(path.Last, prod, label));
							}
						}
					}
				}
			}
			else
			{
				// Create the new corresponding node in the GSS
				w = gss.CreateNode(to);
				gss.CreateEdge(w, path.Last, label);
				// Look for all the reductions and shifts at this state
				int count = parserAutomaton.GetActionsCount(to, nextToken.SymbolID);
				for (int i = 0; i != count; i++)
				{
					LRAction action = parserAutomaton.GetAction(to, nextToken.SymbolID, i);
					if (action.Code == LRActionCode.Shift)
					{
						shifts.Enqueue(new Shift(w, action.Data));
					}
					else if (action.Code == LRActionCode.Reduce)
					{
						LRProduction prod = parserAutomaton.GetProduction(action.Data);
						if (prod.ReductionLength == 0)
							reductions.Enqueue(new Reduction(w, prod, sppf.Epsilon));
						else if (reduction.prod.ReductionLength != 0)
							reductions.Enqueue(new Reduction(path.Last, prod, label));
					}
				}
			}
		}

		/// <summary>
		/// Executes the shift operations for the given token
		/// </summary>
		/// <param name="oldtoken">A token</param>
		/// <returns>The next generation</returns>
		private int Shifter(Token oldtoken)
		{
			// Create next generation
			int gen = gss.CreateGeneration();

			// Create the GSS label to be used for the transitions
			SubTree st = sppf.GetSingleNode();
			st.SetupRoot(new SymbolRef(SymbolType.Token, oldtoken.Index), TreeAction.None);
			GSSLabel label = new GSSLabel(st);

			// Execute all shifts in the queue at this point
			int count = shifts.Count;
			for (int i = 0; i != count; i++)
				ExecuteShift(gen, label, shifts.Dequeue());
			return gen;
		}

		/// <summary>
		/// Executes a shift operation
		/// </summary>
		/// <param name="gen">The GSS generation to start from</param>
		/// <param name="label">The GSS label to use for the new GSS edges</param>
		/// <param name="shift">The shift operation</param>
		private void ExecuteShift(int gen, GSSLabel label, Shift shift)
		{
			int w = gss.FindNode(gen, shift.to);
			if (w != -1)
			{
				// A node for the target state is already in the GSS
				gss.CreateEdge(w, shift.from, label);
				// Look for the new reductions at this state
				int count = parserAutomaton.GetActionsCount(shift.to, nextToken.SymbolID);
				for (int i = 0; i != count; i++)
				{
					LRAction action = parserAutomaton.GetAction(shift.to, nextToken.SymbolID, i);
					if (action.Code == LRActionCode.Reduce)
					{
						LRProduction prod = parserAutomaton.GetProduction(action.Data);
						// length 0 reduction are not considered here because they already exist at this point
						if (prod.ReductionLength != 0)
							reductions.Enqueue(new Reduction(shift.from, prod, label));
					}
				}
			}
			else
			{
				// Create the new corresponding node in the GSS
				w = gss.CreateNode(shift.to);
				gss.CreateEdge(w, shift.from, label);
				// Look for all the reductions and shifts at this state
				int count = parserAutomaton.GetActionsCount(shift.to, nextToken.SymbolID);
				for (int i = 0; i != count; i++)
				{
					LRAction action = parserAutomaton.GetAction(shift.to, nextToken.SymbolID, i);
					if (action.Code == LRActionCode.Shift)
						shifts.Enqueue(new Shift(w, action.Data));
					else if (action.Code == LRActionCode.Reduce)
					{
						LRProduction prod = parserAutomaton.GetProduction(action.Data);
						if (prod.ReductionLength == 0) // Length 0 => reduce from the head
							reductions.Enqueue(new Reduction(w, prod, sppf.Epsilon));
						else // reduce from the second node on the path
							reductions.Enqueue(new Reduction(shift.from, prod, label));
					}
				}
			}
		}

		/// <summary>
		/// Gets the next RNGLR state by a shift with the given variable ID
		/// </summary>
		/// <param name="state">A RNGLR state</param>
		/// <param name="var">A variable ID</param>
		/// <returns>The next RNGLR state, or 0xFFFF if no transition is found</returns>
		private int GetNextByVar(int state, int var)
		{
			int ac = parserAutomaton.GetActionsCount(state, var);
			for (int i = 0; i != ac; i++)
			{
				LRAction action = parserAutomaton.GetAction(state, var, i);
				if (action.Code == LRActionCode.Shift)
					return action.Data;
			}
			return 0xFFFF;
		}
	}
}
