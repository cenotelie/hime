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
using Hime.Redist.Utils;

namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents a base for all RNGLR parsers
	/// </summary>
	public class RNGLRParser : BaseLRParser, Lexer.IContextProvider
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
			public readonly int node;
			/// <summary>
			/// The LR production for the reduction
			/// </summary>
			public readonly LRProduction prod;
			/// <summary>
			/// The first label in the GSS
			/// </summary>
			public readonly GSSLabel first;

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
			public readonly int from;
			/// <summary>
			/// The target RNGLR state
			/// </summary>
			public readonly int to;

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
		private readonly RNGLRAutomaton parserAutomaton;
		/// <summary>
		/// The GSS for this parser
		/// </summary>
		private readonly GSS gss;
		/// <summary>
		/// The SPPF being built
		/// </summary>
		private readonly SPPFBuilder sppf;
		/// <summary>
		/// The sub-trees for the constant nullable variables
		/// </summary>
		private readonly GSSLabel[] nullables;
		/// <summary>
		/// The next token
		/// </summary>
		private Lexer.TokenKernel nextToken;
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
		public RNGLRParser(RNGLRAutomaton automaton, Symbol[] variables, Symbol[] virtuals, SemanticAction[] actions, Lexer.BaseLexer lexer)
			: base(variables, virtuals, actions, lexer)
		{
			parserAutomaton = automaton;
			gss = new GSS();
			sppf = new SPPFBuilder(lexer.tokens, symVariables, symVirtuals);
			nullables = new GSSLabel[variables.Length];
			BuildNullables(variables.Length);
			sppf.ClearHistory();
		}

		/// <summary>
		/// Gets whether the terminal with the specified ID is expected
		/// </summary>
		/// <param name="terminalID">The identifier of a terminal</param>
		/// <returns>true if the corresponding terminal is expected</returns>
		public bool IsExpected(int terminalID)
		{
			if (lexer.tokens.Size == 0)
				// this is for the first token
				return parserAutomaton.GetActionsCount(0, terminalID) > 0;
			// not the first token, look on the outstanding shift actions
			foreach (Shift shift in shifts)
				if (parserAutomaton.GetActionsCount(shift.to, terminalID) > 0)
					return true;
			return false;
		}

		/// <summary>
		/// Gets whether the specified context in in effect
		/// </summary>
		/// <param name="context">A context</param>
		/// <param name="onTerminalID">The identifier of a terminal</param>
		/// <returns>true if the context is in effect</returns>
		public bool IsInContext(int context, int onTerminalID)
		{
			if (lexer.tokens.Size == 0)
				// this is for the first token
				return (context == Lexer.Automaton.DEFAULT_CONTEXT || parserAutomaton.GetContexts(0).Contains(context));

			List<int> queue = new List<int>();
			// try to only look at stack heads that expect the terminal
			foreach (Shift shift in shifts)
			{
				if (parserAutomaton.GetActionsCount(shift.to, onTerminalID) > 0)
				{
					if (context == Lexer.Automaton.DEFAULT_CONTEXT || parserAutomaton.GetContexts(shift.to).Contains(context) || parserAutomaton.GetContexts(gss.GetRepresentedState(shift.from)).Contains(context))
						// the context at the specified state is there
						return true;
					// enqueue
					queue.Add(shift.from);
				}
			}
			if (queue.Count == 0)
			{
				// the track is empty, the terminal is unexpected
				// still look for the correct context
				foreach (Shift shift in shifts)
					queue.Add(shift.from);
			}
			// explore the GSS to find the specified context
			for (int i = 0; i != queue.Count; i++)
			{
				int count;
				GSSPath[] paths = gss.GetPaths(queue[i], 1, out count);
				for (int p = 0; p != count; p++)
				{
					int to = paths[p].Last;
					if (!queue.Contains(to))
					{
						// this target node is not yet explored
						LRContexts contexts = parserAutomaton.GetContexts(gss.GetRepresentedState(to));
						if (contexts.Contains(context))
							// found it!
							return true;
						// enqueue for further exploration
						queue.Add(to);
					}
				}
			}
			// not found
			return false;
		}

		/// <summary>
		/// Builds the constant sub-trees of nullable variables
		/// </summary>
		/// <param name="varCount">The total number of variables</param>
		private void BuildNullables(int varCount)
		{
			// Get the dependency table
			List<int>[] dependencies = BuildNullableDependencies(varCount);
			// Solve and build
			int remaining = 1;
			while (remaining > 0)
			{
				remaining = 0;
				int solved = 0;
				for (int i = 0; i != varCount; i++)
				{
					List<int> dep = dependencies[i];
					if (dep != null)
					{
						bool ok = true;
						foreach (int r in dep)
							ok = ok && (dependencies[r] == null);
						if (ok)
						{
							LRProduction prod = parserAutomaton.GetNullableProduction(i);
							nullables[i] = BuildSPPF(0, prod, sppf.Epsilon, null);
							dependencies[i] = null;
							solved++;
						}
						else
						{
							remaining++;
						}
					}
				}
				if (solved == 0 && remaining > 0)
				{
					// There is dependency cycle ...
					// That should not be possible ...
					throw new Exception("Failed to initialize the parser, found a cycle in the nullable variables");
				}
			}
		}

		/// <summary>
		/// Builds the dependency table between nullable variables
		/// </summary>
		/// <param name="varCount">The total number of variables</param>
		/// <returns>The dependency table</returns>
		private List<int>[] BuildNullableDependencies(int varCount)
		{
			List<int>[] result = new List<int>[varCount];
			for (int i = 0; i != varCount; i++)
			{
				LRProduction prod = parserAutomaton.GetNullableProduction(i);
				if (prod != null)
					result[i] = GetNullableDependencies(prod);
			}
			return result;
		}

		/// <summary>
		/// Gets the dependencies on nullable variables
		/// </summary>
		/// <param name="production">The production of a nullable variable</param>
		/// <returns>The list of the nullable variables' indices that this production depends on</returns>
		private static List<int> GetNullableDependencies(LRProduction production)
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

		/// <summary>
		/// Raises an error on an unexepcted token
		/// </summary>
		/// <param name="stem">The size of the generation's stem</param>
		private void OnUnexpectedToken(int stem)
		{
			// build the list of expected terminals
			List<Symbol> expected = new List<Symbol>();
			GSSGeneration genData = gss.GetGeneration();
			for (int i = 0; i != genData.Count; i++)
			{
				LRExpected expectedOnHead = parserAutomaton.GetExpected(gss.GetRepresentedState(i + genData.Start), lexer.Terminals);
				// register the terminals for shift actions
				foreach (Symbol terminal in expectedOnHead.Shifts)
					if (!expected.Contains(terminal))
						expected.Add(terminal);
				if (i < stem)
				{
					// the state was in the stem, also look for reductions
					foreach (Symbol terminal in expectedOnHead.Reductions)
						if (!expected.Contains(terminal) && CheckIsExpected(i + genData.Start, terminal))
							expected.Add(terminal);
				}
			}
			// register the error
			UnexpectedTokenError error = new UnexpectedTokenError(lexer.tokens[nextToken.Index], new ROList<Symbol>(expected));
			allErrors.Add(error);
			if (ModeDebug)
			{
				Console.WriteLine("==== RNGLR parsing error:");
				Console.Write("\t");
				Console.WriteLine(error);
				TextContext context = lexer.Input.GetContext(error.Position);
				Console.Write("\t");
				Console.WriteLine(context.Content);
				Console.Write("\t");
				Console.WriteLine(context.Pointer);
				gss.Print();
			}
		}

		/// <summary>
		/// Checks whether the specified terminal is indeed expected for a reduction
		/// </summary>
		/// <param name="gssNode">The GSS node from which to reduce</param>
		/// <param name="terminal">The terminal to check</param>
		/// <returns><code>true</code> if the terminal is really expected</returns>
		/// <remarks>
		/// This check is required because in the case of a base LALR graph,
		/// some terminals expected for reduction in the automaton are coming from other paths.
		/// </remarks>
		private bool CheckIsExpected(int gssNode, Symbol terminal)
		{
			// queue of GLR states to inspect:
			List<int> queueGSSHead = new List<int>();   // the related GSS head
			List<int[]> queueVStack = new List<int[]>(); // the virtual stack

			// first reduction
			{
				int count = parserAutomaton.GetActionsCount(gss.GetRepresentedState(gssNode), terminal.ID);
				for (int j = 0; j != count; j++)
				{
					LRAction action = parserAutomaton.GetAction(gss.GetRepresentedState(gssNode), terminal.ID, j);
					if (action.Code == LRActionCode.Reduce)
					{
						// execute the reduction
						LRProduction production = parserAutomaton.GetProduction(action.Data);
						int nbPaths;
						GSSPath[] paths = gss.GetPaths(gssNode, production.ReductionLength, out nbPaths);
						for (int k = 0; k != nbPaths; k++)
						{
							GSSPath path = paths[k];
							// get the target GLR state
							int next = GetNextByVar(gss.GetRepresentedState(path.Last), symVariables[production.Head].ID);
							// enqueue the info, top GSS stack node and target GLR state
							queueGSSHead.Add(path.Last);
							queueVStack.Add(new [] { next });
						}
					}
				}
			}

			// now, close the queue
			for (int i = 0; i != queueGSSHead.Count; i++)
			{
				int head = queueVStack[i][queueVStack[i].Length - 1];
				int count = parserAutomaton.GetActionsCount(head, terminal.ID);
				if (count == 0)
					continue;
				for (int j = 0; j != count; j++)
				{
					LRAction action = parserAutomaton.GetAction(head, terminal.ID, j);
					if (action.Code == LRActionCode.Shift)
						// yep, the terminal was expected
						return true;
					if (action.Code == LRActionCode.Reduce)
					{
						// execute the reduction
						LRProduction production = parserAutomaton.GetProduction(action.Data);
						if (production.ReductionLength == 0)
						{
							// 0-length reduction => start from the current head
							int[] virtualStack = new int[queueVStack[i].Length + 1];
							Array.Copy(queueVStack[i], virtualStack, queueVStack[i].Length);
							virtualStack[virtualStack.Length - 1] = GetNextByVar(head, symVariables[production.Head].ID);
							// enqueue
							queueGSSHead.Add(queueGSSHead[i]);
							queueVStack.Add(virtualStack);
						}
						else if (production.ReductionLength < queueVStack[i].Length)
						{
							// we are still the virtual stack
							int[] virtualStack = new int[queueVStack[i].Length - production.ReductionLength + 1];
							Array.Copy(queueVStack[i], virtualStack, virtualStack.Length - 1);
							virtualStack[virtualStack.Length - 1] = GetNextByVar(virtualStack[virtualStack.Length - 2], symVariables[production.Head].ID);
							// enqueue
							queueGSSHead.Add(queueGSSHead[i]);
							queueVStack.Add(virtualStack);
						}
						else
						{
							// we reach the GSS
							int nbPaths;
							GSSPath[] paths = gss.GetPaths(queueGSSHead[i], production.ReductionLength - queueVStack[i].Length, out nbPaths);
							for (int k = 0; k != nbPaths; k++)
							{
								GSSPath path = paths[k];
								// get the target GLR state
								int next = GetNextByVar(gss.GetRepresentedState(path.Last), symVariables[production.Head].ID);
								// enqueue the info, top GSS stack node and target GLR state
								queueGSSHead.Add(path.Last);
								queueVStack.Add(new [] { next });
							}
						}
					}
				}
			}

			// nope, that was a pathological case in a LALR graph
			return false;
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
			Symbol variable = symVariables[production.Head];
			sppf.ReductionPrepare(first, path, production.ReductionLength);
			for (int i = 0; i != production.BytecodeLength; i++)
			{
				LROpCode op = production[i];
				switch (op.Base)
				{
				case LROpCodeBase.SemanticAction:
					{
						SemanticAction action = symActions[production[i + 1].DataValue];
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
			return sppf.Reduce(generation, production.Head, production.HeadAction == TreeAction.Replace);
		}

		/// <summary>
		/// Parses the input and returns the produced AST
		/// </summary>
		/// <returns>AST produced by the parser representing the input, or null if unrecoverable errors were encountered</returns>
		public override ParseResult Parse()
		{
			reductions = new Queue<Reduction>();
			shifts = new Queue<Shift>();
			int Ui = gss.CreateGeneration();
			int v0 = gss.CreateNode(0);
			nextToken = lexer.GetNextToken(this);

			// bootstrap the shifts and reductions queues
			int count = parserAutomaton.GetActionsCount(0, nextToken.TerminalID);
			for (int i = 0; i != count; i++)
			{
				LRAction action = parserAutomaton.GetAction(0, nextToken.TerminalID, i);
				if (action.Code == LRActionCode.Shift)
					shifts.Enqueue(new Shift(v0, action.Data));
				else if (action.Code == LRActionCode.Reduce)
					reductions.Enqueue(new Reduction(v0, parserAutomaton.GetProduction(action.Data), sppf.Epsilon));
			}

			while (nextToken.TerminalID != Symbol.SID_EPSILON) // Wait for ε token
			{
				// the stem length (initial number of nodes in the generation before reductions)
				int stem = gss.GetGeneration(Ui).Count;
				// apply all reduction actions
				Reducer(Ui);
				// no scheduled shift actions?
				if (shifts.Count == 0)
				{
					// the next token was not expected
					OnUnexpectedToken(stem);
					return new ParseResult(new ROList<ParseError>(allErrors), lexer.Input);
				}
				// look for the next next-token
				Lexer.TokenKernel oldtoken = nextToken;
				nextToken = lexer.GetNextToken(this);
				// apply the scheduled shift actions
				Ui = Shifter(oldtoken);
			}

			GSSGeneration genData = gss.GetGeneration(Ui);
			for (int i = genData.Start; i != genData.Start + genData.Count; i++)
			{
				int state = gss.GetRepresentedState(i);
				if (parserAutomaton.IsAcceptingState(state))
				{
					// Has reduction _Axiom_ -> axiom $ . on ε
					GSSPath[] paths = gss.GetPaths(i, 2, out count);
					return new ParseResult(new ROList<ParseError>(allErrors), lexer.Input, sppf.GetTree(paths[0][1]));
				}
			}
			// At end of input but was still waiting for tokens
			return new ParseResult(new ROList<ParseError>(allErrors), lexer.Input);
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
			int count;
			GSSPath[] paths;
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
			Symbol head = symVariables[reduction.prod.Head];
			// Resolve the sub-root
			GSSLabel label = sppf.GetLabelFor(path.Generation, new TableElemRef(TableType.Variable, reduction.prod.Head));
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
						int count = parserAutomaton.GetActionsCount(to, nextToken.TerminalID);
						for (int i = 0; i != count; i++)
						{
							LRAction action = parserAutomaton.GetAction(to, nextToken.TerminalID, i);
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
				int count = parserAutomaton.GetActionsCount(to, nextToken.TerminalID);
				for (int i = 0; i != count; i++)
				{
					LRAction action = parserAutomaton.GetAction(to, nextToken.TerminalID, i);
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
		private int Shifter(Lexer.TokenKernel oldtoken)
		{
			// Create next generation
			int gen = gss.CreateGeneration();

			// Create the GSS label to be used for the transitions
			TableElemRef sym = new TableElemRef(TableType.Token, oldtoken.Index);
			GSSLabel label = new GSSLabel(sym, sppf.GetSingleNode(sym));

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
				int count = parserAutomaton.GetActionsCount(shift.to, nextToken.TerminalID);
				for (int i = 0; i != count; i++)
				{
					LRAction action = parserAutomaton.GetAction(shift.to, nextToken.TerminalID, i);
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
				int count = parserAutomaton.GetActionsCount(shift.to, nextToken.TerminalID);
				for (int i = 0; i != count; i++)
				{
					LRAction action = parserAutomaton.GetAction(shift.to, nextToken.TerminalID, i);
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
