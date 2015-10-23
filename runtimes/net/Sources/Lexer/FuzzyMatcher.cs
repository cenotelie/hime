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
	/// This matcher uses the Levenshtein distance to match the input ahead against the current DFA automaton.
	/// The matcher favors solutions that are the closest to the original input.
	/// When multiple solutions are at the same Levenshtein distance to the input, the longest one is preferred.
	/// </summary>
	class FuzzyMatcher
	{
		/// <summary>
		/// Represents a DFA stack head
		/// </summary>
		private struct Head
		{
			/// <summary>
			/// The data representing this head
			/// </summary>
			private readonly int[] data;

			/// <summary>
			/// Gets the associated DFA state
			/// </summary>
			public int State { get { return data[0]; } }

			/// <summary>
			/// Gets the Levenshtein distance of this head form the input
			/// </summary>
			public int Distance { get { return data.Length - 1; } }

			/// <summary>
			/// Initializes this head with a state and a 0 distance
			/// </summary>
			/// <param name="state">The associated DFA state</param>
			public Head(int state)
			{
				data = new int[1];
				data[0] = state;
			}

			/// <summary>
			/// Initializes this head from a previous one
			/// </summary>
			/// <param name="previous">The previous head</param>
			/// <param name="state">The associated DFA state</param>
			public Head(Head previous, int state)
			{
				data = (int[])previous.data.Clone();
				data[0] = state;
			}

			/// <summary>
			/// Initializes this head from a previous one
			/// </summary>
			/// <param name="previous">The previous head</param>
			/// <param name="state">The associated DFA state</param>
			/// <param name="errorIndex">The index of the new error</param>
			public Head(Head previous, int state, int errorIndex)
			{
				data = new int[previous.data.Length + 1];
				data[0] = state;
				data[data.Length - 1] = errorIndex;
				System.Array.Copy(previous.data, 1, data, 1, previous.data.Length - 1);
			}

			/// <summary>
			/// Initializes this head from a previous one
			/// </summary>
			/// <param name="previous">The previous head</param>
			/// <param name="state">The associated DFA state</param>
			/// <param name="errorIndex">The index of the new error</param>
			/// <param name="distance">The distance to reach</param>
			public Head(Head previous, int state, int errorIndex, int distance)
			{
				if (distance < previous.Distance + 1)
					throw new System.ArgumentException("The distance for the new head must be at least one more than the distance of the previous head", "distance");
				data = new int[distance + 1];
				data[0] = state;
				data[data.Length - 1] = errorIndex;
				System.Array.Copy(previous.data, 1, data, 1, previous.data.Length - 1);
				for (int i = previous.data.Length; i != data.Length; i++)
					data[i] = errorIndex;
			}

			/// <summary>
			/// Gets the index in the input of the i-th lexical error on this head
			/// </summary>
			/// <param name="i">Index of the error</param>
			/// <returns>The index of the i-th error in the input</returns>
			public int GetError(int i)
			{
				return data[i + 1];
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
		/// The current heads
		/// </summary>
		private List<Head> heads;
		/// <summary>
		/// Buffer of DFA states for the computation of character insertions
		/// </summary>
		private int[] insertions;
		/// <summary>
		/// The current number of insertions in the buffer
		/// </summary>
		private int insertionsCount;
		/// <summary>
		/// The current matching head, if any
		/// </summary>
		private Head matchHead;
		/// <summary>
		/// The current matching length
		/// </summary>
		private int matchLength;

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
		}

		/// <summary>
		/// Runs this matcher
		/// </summary>
		/// <returns>The solution</returns>
		public TokenMatch Run()
		{
			heads = new List<Head>();
			insertions = new int[16];
			insertionsCount = 0;
			matchHead = new Head(0);
			matchLength = 0;
			int length = 0;
			int index = originIndex;
			bool atEnd = text.IsEnd(index);
			char current = atEnd ? '\0' : text.GetValue(index);
			if (atEnd)
				InspectAtEnd(matchHead, index, length);
			else
				Inspect(matchHead, index, length, current);
			while (heads.Count != 0)
			{
				length++;
				index++;
				atEnd = text.IsEnd(index);
				current = atEnd ? '\0' : text.GetValue(index);
				List<Head> temp = new List<Head>(heads);
				heads.Clear();
				foreach (Head head in temp)
				{
					if (atEnd)
						InspectAtEnd(head, index, length);
					else
						Inspect(head, index, length, current);
				}
			}
			return matchLength == 0 ? OnFailure() : OnSuccess();
		}

		/// <summary>
		/// Constructs the solution when succeeded to fix the error
		/// </summary>
		/// <returns>The constructed solution</returns>
		private TokenMatch OnSuccess()
		{
			int lastErrorIndex = -1;
			for (int i = 0; i != matchHead.Distance; i++)
			{
				int errorIndex = matchHead.GetError(i);
				if (errorIndex != lastErrorIndex)
					OnError(errorIndex);
				lastErrorIndex = errorIndex;
			}
			return new TokenMatch(matchHead.State, matchLength);
		}

		/// <summary>
		/// Reports on the lexical error at the specified index
		/// </summary>
		/// <param name="index">The index in the input where the error occurs</param>
		private void OnError(int index)
		{
			ParseErrorType errorType = ParseErrorType.UnexpectedChar;
			bool atEnd = text.IsEnd(index);
			string value = "";
			if (atEnd)
			{
				// the end of input was not expected
				// there is necessarily some input before because an empty input would have matched the $
				char c = text.GetValue(index - 1);
				if (c >= 0xD800 && c <= 0xDBFF)
				{
					// a trailing UTF-16 high surrogate
					index--;
					errorType = ParseErrorType.IncorrectUTF16NoLowSurrogate;
				}
				else
					errorType = ParseErrorType.UnexpectedEndOfInput;
			}
			else
			{
				char c = text.GetValue(index);
				if (c >= 0xD800 && c <= 0xDBFF && !text.IsEnd(index + 1))
				{
					// a UTF-16 high surrogate
					// if next next character is a low surrogate, also get it
					char c2 = text.GetValue(index + 1);
					if (c2 >= 0xDC00 && c2 <= 0xDFFF)
						value = new string(new [] { c, c2 });
					else
						errorType = ParseErrorType.IncorrectUTF16NoLowSurrogate;
				}
				else if (c >= 0xDC00 && c <= 0xDFFF && index > 0)
				{
					// a UTF-16 low surrogate
					// if the previous character is a high surrogate, also get it
					char c2 = text.GetValue(index - 1);
					if (c2 >= 0xD800 && c2 <= 0xDBFF)
					{
						index--;
						value = new string(new [] { c2, c });
					}
					else
						errorType = ParseErrorType.IncorrectUTF16NoHighSurrogate;
				}
				if (value.Length == 0)
					value = c.ToString();
			}
			switch (errorType)
			{
			case ParseErrorType.UnexpectedEndOfInput:
				errors(new UnexpectedEndOfInput(text.GetPositionAt(index)));
				break;
			case ParseErrorType.UnexpectedChar:
				errors(new UnexpectedCharError(value, text.GetPositionAt(index)));
				break;
			case ParseErrorType.IncorrectUTF16NoHighSurrogate:
			case ParseErrorType.IncorrectUTF16NoLowSurrogate:
				errors(new IncorrectEncodingSequence(text.GetPositionAt(index), text.GetValue(index), errorType));
				break;
			}
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
		/// Pushes a new head onto the the queue
		/// </summary>
		/// <param name="previous">The previous head</param>
		/// <param name="state">The associated DFA state</param>
		private void PushHead(Head previous, int state)
		{
			PushHead(previous, state, -1, previous.Distance);
		}

		/// <summary>
		/// Pushes a new head onto the the queue
		/// </summary>
		/// <param name="previous">The previous head</param>
		/// <param name="state">The associated DFA state</param>
		/// <param name="errorIndex">The index of the new error</param>
		private void PushHead(Head previous, int state, int errorIndex)
		{
			PushHead(previous, state, errorIndex, previous.Distance + 1);
		}

		/// <summary>
		/// Pushes a new head onto the the queue
		/// </summary>
		/// <param name="previous">The previous head</param>
		/// <param name="state">The associated DFA state</param>
		/// <param name="errorIndex">The index of the new error</param>
		/// <param name="distance">The distance to reach</param>
		private void PushHead(Head previous, int state, int errorIndex, int distance)
		{
			for (int i = heads.Count - 1; i != -1; i--)
				if (heads[i].State == state && heads[i].Distance <= distance)
					return;
			if (errorIndex == -1)
				heads.Add(new Head(previous, state));
			else
				heads.Add(new Head(previous, state, errorIndex, distance));
		}

		/// <summary>
		/// Inspects a head while at the end of the input
		/// </summary>
		/// <param name="head">The head to inspect</param>
		/// <param name="index">The current index in the input</param>
		/// <param name="length">The current length</param>
		private void InspectAtEnd(Head head, int index, int length)
		{
			AutomatonState stateData = automaton.GetState(head.State);
			// is it a matching state
			if (stateData.TerminalsCount != 0)
				OnMatchingHead(head, length);
			if (head.Distance >= maxDistance || stateData.IsDeadEnd)
				// cannot stray further
				return;
			// lookup the transitions
			ExploreTransitions(head, stateData, index, true);
			ExploreInsertions(head, index, length, true, '\0');
		}

		/// <summary>
		/// Inspects a head with a specified character ahead
		/// </summary>
		/// <param name="head">The head to inspect</param>
		/// <param name="index">The current index in the input</param>
		/// <param name="length">The current length from the original index</param>
		/// <param name="current">The leading character in the input</param>
		private void Inspect(Head head, int index, int length, char current)
		{
			AutomatonState stateData = automaton.GetState(head.State);
			// is it a matching state
			if (stateData.TerminalsCount != 0)
				OnMatchingHead(head, length);
			if (head.Distance >= maxDistance || stateData.IsDeadEnd)
				// cannot stray further
				return;
			// could be a straight match
			int target = stateData.GetTargetBy(current);
			if (target != Automaton.DEAD_STATE)
			{
				// it is!
				PushHead(head, target);
				return;
			}
			// could try a drop
			PushHead(head, head.State, index);
			// lookup the transitions
			ExploreTransitions(head, stateData, index, false);
			ExploreInsertions(head, index, length, false, current);
		}

		/// <summary>
		/// Explores a state transition
		/// </summary>
		/// <param name="head">The current head</param>
		/// <param name="stateData">The data of the DFA state</param>
		/// <param name="index">The current index in the input</param>
		/// <param name="atEnd">Whether the current index is at the end of the input</param>
		private void ExploreTransitions(Head head, AutomatonState stateData, int index, bool atEnd)
		{
			for (int i = 0; i != 256; i++)
			{
				int target = stateData.GetCachedTransition(i);
				if (target == Automaton.DEAD_STATE)
					continue;
				ExploreTransitionToTarget(head, target, index, atEnd);
			}
			for (int i = 0; i != stateData.BulkTransitionsCount; i++)
			{
				ExploreTransitionToTarget(head, stateData.GetBulkTransition(i).Target, index, atEnd);
			}
		}

		/// <summary>
		/// Explores a state transition
		/// </summary>
		/// <param name="head">The current head</param>
		/// <param name="target">The target DFA state</param>
		/// <param name="index">The current index in the input</param>
		/// <param name="atEnd">Whether the current index is at the end of the input</param>
		private void ExploreTransitionToTarget(Head head, int target, int index, bool atEnd)
		{
			if (!atEnd)
			{
				// try replace
				PushHead(head, target, index);
			}
			// try to insert
			bool found = false;
			for (int i = insertionsCount - 1; i != -1; i--)
			{
				if (insertions[i] == target)
				{
					found = true;
					break;
				}
			}
			if (!found)
			{
				if (insertionsCount == insertions.Length)
					System.Array.Resize(ref insertions, insertions.Length * 2);
				insertions[insertionsCount++] = target;
			}
		}

		/// <summary>
		/// Explores the current insertions
		/// </summary>
		/// <param name="head">The head to inspect</param>
		/// <param name="index">The current index in the input</param>
		/// <param name="length">The current length from the original index</param>
		/// <param name="atEnd">Whether the current index is at the end of the input</param>
		/// <param name="current">The leading character in the input</param>
		private void ExploreInsertions(Head head, int index, int length, bool atEnd, char current)
		{
			// setup the first round
			int distance = head.Distance + 1;
			int end = insertionsCount;
			int start = 0;
			// while there are insertions to examine in a round
			while (start != insertionsCount)
			{
				for (int i = start; i != end; i++)
				{
					// examine insertion i
					ExploreInsertion(head, index, length, atEnd, current, insertions[i], distance);
				}
				// prepare next round
				distance++;
				start = end;
				end = insertionsCount;
			}
			// reset the insertions data
			insertionsCount = 0;
		}

		/// <summary>
		/// Explores an insertion
		/// </summary>
		/// <param name="head">The head to inspect</param>
		/// <param name="index">The current index in the input</param>
		/// <param name="length">The current length from the original index</param>
		/// <param name="atEnd">Whether the current index is at the end of the input</param>
		/// <param name="current">The leading character in the input</param>
		/// <param name="state">The DFA state for the insertion</param>
		/// <param name="distance">The distance associated to this insertion</param>
		private void ExploreInsertion(Head head, int index, int length, bool atEnd, char current, int state, int distance)
		{
			AutomatonState stateData = automaton.GetState(state);
			if (stateData.TerminalsCount != 0)
				OnMatchingInsertion(head, index, state, distance, length);
			if (!atEnd)
			{
				int target = stateData.GetTargetBy(current);
				if (target != Automaton.DEAD_STATE)
				{
					PushHead(head, target, index, distance);
					return;
				}
			}
			if (distance >= maxDistance)
				return;
			// continue insertion
			ExploreTransitions(head, stateData, index, true);
		}

		/// <summary>
		/// When a matching head is encountered
		/// </summary>
		/// <param name="head">The matching head</param>
		/// <param name="length">The lenght of the match in the input</param>
		private void OnMatchingHead(Head head, int length)
		{
			if (length > matchLength)
			{
				matchHead = head;
				matchLength = length;
			}
		}

		/// <summary>
		/// When a matching insertion is encountered
		/// </summary>
		/// <param name="previous">The previous head</param>
		/// <param name="index">The current index in the input</param>
		/// <param name="target">The DFA state for the insertion</param>
		/// <param name="distance">The distance associated to this insertion</param>
		/// <param name="length">The current length from the original index</param>
		private void OnMatchingInsertion(Head previous, int index, int target, int distance, int length)
		{
			if (length > matchLength)
			{
				matchHead = new Head(previous, target, index, distance);
				matchLength = length;
			}
		}
	}
}