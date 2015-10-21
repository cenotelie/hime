/**********************************************************************
* Copyright (c) 2015 Laurent Wouters
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

namespace Hime.Redist.Lexer
{
	/// <summary>
	/// A fuzzy DFA matcher
	/// This matcher uses the Levenshtein distance to match the input ahead against the current DFA automaton
	/// </summary>
	class FuzzyMatcher
	{
		/// <summary>
		/// A DFA stack node
		/// </summary>
		private class Node
		{
			/// <summary>
			/// The previous node
			/// </summary>
			public readonly Node previous;
			/// <summary>
			/// The represented DFA state
			/// </summary>
			public readonly int state;
			/// <summary>
			/// The required length in the input to reach this node
			/// </summary>
			public readonly int length;
			/// <summary>
			/// The Levenshtein distance of this node from the input
			/// </summary>
			public readonly int distance;
			/// <summary>
			/// The raised lexical error to reach this state
			/// </summary>
			public readonly ParseError error;


			/// <summary>
			/// Initializes this node
			/// </summary>
			/// <param name="previous">The previous node</param>
			/// <param name="state">The represented DFA state</param>
			/// <param name="length">The required length in the input to reach this node</param>
			/// <param name="distance">The Levenshtein distance of this node from the input</param>
			public Node(Node previous, int state, int length, int distance)
			{
				this.previous = previous;
				this.state = state;
				this.length = length;
				this.distance = distance;
				error = null;
			}

			/// <summary>
			/// Initializes this node
			/// </summary>
			/// <param name="previous">The previous node</param>
			/// <param name="state">The represented DFA state</param>
			/// <param name="length">The required length in the input to reach this node</param>
			/// <param name="distance">The Levenshtein distance of this node from the input</param>
			/// <param name="error">The raised lexical error to reach this state</param>
			public Node(Node previous, int state, int length, int distance, ParseError error)
			{
				this.previous = previous;
				this.state = state;
				this.length = length;
				this.distance = distance;
				this.error = null;
				this.error = error;
			}
		}

		/// <summary>
		/// This lexer's automaton
		/// </summary>
		private readonly Automaton automaton;
		/// <summary>
		/// The input text
		/// </summary>
		private readonly BaseText text;
		/// <summary>
		/// Delegate for raising errors
		/// </summary>
		private readonly AddLexicalError errors;
		/// <summary>
		/// The maximum Levenshtein distance between the input and the DFA
		/// </summary>
		private readonly int maxDistance;
		/// <summary>
		/// The index in the input from wich the error was raised
		/// </summary>
		private readonly int originIndex;
		/// <summary>
		/// The queue of DFA stack heads to inspect
		/// </summary>
		private readonly List<Node> queue;
		/// <summary>
		/// The longest matching node
		/// </summary>
		private Node matching;

		/// <summary>
		/// Initializes this matcher
		/// </summary>
		/// <param name="automaton">This lexer's automaton</param>
		/// <param name="text">The input text</param>
		/// <param name="errors">Delegate for raising errors</param>
		/// <param name="maxDistance">The maximum Levenshtein distance between the input and the DFA</param>
		/// <param name="index">The index in the input from wich the error was raised</param>
		public FuzzyMatcher(Automaton automaton, BaseText text, AddLexicalError errors, int maxDistance, int index)
		{
			this.automaton = automaton;
			this.text = text;
			this.errors = errors;
			this.maxDistance = maxDistance;
			originIndex = index;
			queue = new List<Node>();
			matching = null;
		}

		/// <summary>
		/// Runs this matcher
		/// </summary>
		/// <returns>The solution</returns>
		public TokenMatch Run()
		{
			InspectFix(new Node(null, 0, 0, 0));
			// we do not expect a match on the first try because we are supposed to be on an error here
			for (int i = 0; i != queue.Count; i++)
				Inspect(queue[i]);
			return matching != null ? OnSuccess(matching) : OnFailure();
		}

		/// <summary>
		/// Constructs the solution when succeeded to fix the error
		/// </summary>
		/// <param name="node">The node with the solution</param>
		/// <returns>The constructed solution</returns>
		private TokenMatch OnSuccess(Node node)
		{
			List<ParseError> myErrors = new List<ParseError>();
			Node current = node;
			while (current != null)
			{
				if (current.error != null)
					myErrors.Add(current.error);
				current = current.previous;
			}
			for (int i = myErrors.Count - 1; i != -1; i--)
				errors(myErrors[i]);
			return new TokenMatch(node.state, node.length);
		}

		/// <summary>
		/// Constructs the solution when failed to fix the error
		/// </summary>
		private TokenMatch OnFailure()
		{
			errors(new UnexpectedCharError(text.GetValue(originIndex).ToString(), text.GetPositionAt(originIndex)));
			return new TokenMatch(1);
		}


		/// <summary>
		/// Inspects the current stack head
		/// </summary>
		/// <param name="head">The head of a DFA stack</param>
		private void Inspect(Node head)
		{
			int index = originIndex + head.length;
			AutomatonState stateData = automaton.GetState(head.state);
			if (stateData.TerminalsCount > 0)
			{
				if (matching == null || head.length > matching.length)
					matching = head;
			}
			if (head.distance >= maxDistance || stateData.IsDeadEnd || text.IsEnd(index))
			{
				// reached max Levenshtein distance, or the state is a dead end, or reached the end of input
				return;
			}

			// try to drop the unexpected character
			InspectFix(new Node(head,
				head.state,
				head.length + 1,
				head.distance + 1,
				new UnexpectedCharError(text.GetValue(index).ToString(), text.GetPositionAt(index))));

			for (int i = 0; i != 256; i++)
			{
				int target = stateData.GetCachedTransition(i);
				if (target == Automaton.DEAD_STATE)
					continue;
				// try to replace the next character by an expected one
				InspectFix(new Node(head,
					target,
					head.length + 1,
					head.distance + 1,
					new UnexpectedCharError(text.GetValue(index).ToString(), text.GetPositionAt(index))));
				// try to insert the expected character
				InspectFix(new Node(head,
					target,
					head.length,
					head.distance + 1,
					new UnexpectedCharError(text.GetValue(index).ToString(), text.GetPositionAt(index))));
			}

			for (int i = 0; i != stateData.BulkTransitionsCount; i++)
			{
				int target = stateData.GetBulkTransition(i).Target;
				// try to replace the next character by an expected one
				InspectFix(new Node(head,
					target,
					head.length + 1,
					head.distance + 1,
					new UnexpectedCharError(text.GetValue(index).ToString(), text.GetPositionAt(index))));
				// try to insert the expected character
				InspectFix(new Node(head,
					target,
					head.length,
					head.distance + 1,
					new UnexpectedCharError(text.GetValue(index).ToString(), text.GetPositionAt(index))));
			}
			// low (value >= 0xDC00 && value <= 0xDFFF)
			// high (value >= 0xD800 && value <= 0xDBFF)
		}

		/// <summary>
		/// Inspects the fixing node
		/// </summary>
		/// <param name="node">A fixing node</param>
		private void InspectFix(Node node)
		{
			queue.Add(node);
			Node node2 = BuildDFAStack(node);
			if (node2 != node)
				queue.Add(node2);
		}


		/// <summary>
		/// Tries to fix the current unexpected low surrogate
		/// </summary>
		/// <returns>true if the issue is fixed</returns>
		private bool FixIncorrectLowSurrogate()
		{
			return false;
			// is it preceded by a high surrogate?
			/*char value = text.GetValue(currentIndex);
			char previous = currentIndex > originIndex ? text.GetValue(currentIndex - 1) : '\0';
			TokenMatch head = stack[stack.Count - 1];
			if (currentIndex == originIndex || previous < 0xD800 || previous > 0xDBFF)
			{
				// no preceding high surrogate
				AutomatonState stateData = automaton.GetState(head.state);
				errors(new IncorrectEncodingSequence(text.GetPositionAt(currentIndex), new [] { value }, ParseErrorType.IncorrectUTF16NoHighSurrogate));
				// try to fix this by simulating the presence of a high surrogate before the current character
				for (int k = 0; k != stateData.BulkTransitionsCount; k++)
				{
					AutomatonTransition transition = stateData.GetBulkTransition(k);
					if ((transition.Start >= 0xD800 && transition.Start <= 0xDBFF)
					    && (transition.End >= 0xD800 && transition.End <= 0xDBFF)
					    && automaton.GetState(transition.Target).GetTargetBy(value) != Automaton.DEAD_STATE)
					{
						stack.Add(new TokenMatch(transition.Target, head.length));
						distance.Add(distance[distance.Count - 1] + 1); // an insertion
						return true;
					}
				}
				// failed, try to drop the low surrogate
				return FixDropHead();
			}
			else
			{
				// preceded by a high surrogate that was expected
				// try to fix this by replacing the unexpected low surrogate by an expected one
				errors(new UnexpectedCharError(new string(new [] { previous, value }), text.GetPositionAt(currentIndex - 1)));
				AutomatonState stateData = automaton.GetState(head.state);
				if (text.IsEnd(currentIndex + 1))
				{
					// end of input, simply replace
					for (int k = 0; k != stateData.BulkTransitionsCount; k++)
					{
						AutomatonTransition transition = stateData.GetBulkTransition(k);
						if ((transition.Start >= 0xDC00 && transition.Start <= 0xDFFF)
						    && (transition.End >= 0xDC00 && transition.End <= 0xDFFF)
						    && automaton.GetState(transition.Target).TerminalsCount > 0)
						{
							stack.Add(new TokenMatch(transition.Target, head.length + 1));
							distance.Add(distance[distance.Count - 1] + 1); // a substitution
							return true;
						}
					}
					// no transition for a low surrogate to a final matching state, this is weird
					return false;
				}
				// look for a suitable replacement
				char next = text.GetValue(currentIndex + 1);
				for (int k = 0; k != stateData.BulkTransitionsCount; k++)
				{
					AutomatonTransition transition = stateData.GetBulkTransition(k);
					if ((transition.Start >= 0xDC00 && transition.Start <= 0xDFFF)
					    && (transition.End >= 0xDC00 && transition.End <= 0xDFFF))
					{
						AutomatonState nextStateData = automaton.GetState(transition.Target);
						if (nextStateData.TerminalsCount > 0 || nextStateData.GetTargetBy(next) != Automaton.DEAD_STATE)
						{
							// either we reached a matching state, or the next character is expected, this could be the solution
							stack.Add(new TokenMatch(transition.Target, head.length + 1));
							distance.Add(distance[distance.Count - 1] + 1); // a substitution
							return true;
						}
					}
				}
				// could not find a transition with a suitable replacement
				return false;
			}*/
		}

		/// <summary>
		/// Builds the DFA stack for matching a token in the input ahead
		/// </summary>
		/// <param name="head">The stack node to begin from</param>
		/// <returns>The new head</returns>
		private Node BuildDFAStack(Node head)
		{
			int index = originIndex + head.length;
			while (true)
			{
				AutomatonState stateData = automaton.GetState(head.state);
				if (stateData.TerminalsCount != 0)
				{
					if (matching == null || head.length > matching.length)
						matching = head;
				}
				// No further transition => exit
				if (stateData.IsDeadEnd)
					break;
				// At the end of the buffer
				if (text.IsEnd(index))
					break;
				char current = text.GetValue(index);
				index++;
				// Try to find a transition from this state with the read character
				int nextState = stateData.GetTargetBy(current);
				if (nextState == Automaton.DEAD_STATE)
					break;
				// ok, continue
				head = new Node(head, nextState, head.length + 1, head.distance);
			}
			return head;
		}
	}
}