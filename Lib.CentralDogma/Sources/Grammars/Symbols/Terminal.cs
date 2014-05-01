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

namespace Hime.CentralDogma.Grammars
{
	/// <summary>
	/// Represents a terminal symbol in a grammar
	/// </summary>
    public class Terminal : Symbol, Automata.FinalItem
    {
		/// <summary>
		/// Gets or sets the inline value of this terminal
		/// </summary>
		public string Value { get; protected set; }

		/// <summary>
		/// Gets or sets the NFA that is used to match this terminal
		/// </summary>
        public Automata.NFA NFA { get; protected set; }

		/// <summary>
		///  Gets the priority of this marker 
		/// </summary>
		public int Priority { get { return ID; } }

		/// <summary>
		/// Initializes this symbol
		/// </summary>
		/// <param name="sid">The symbol's unique identifier</param>
		/// <param name="name">The symbol's name</param>
		/// <param name="value">The symbol's inline value</param>
		/// <param name="nfa">The symbol's name</param>
        public Terminal(int sid, string name, string value, Automata.NFA nfa)
            : base(sid, name)
        {
			this.Value = value;
            this.NFA = nfa;
        }

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the current <see cref="Hime.CentralDogma.Grammars.Terminal"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the current <see cref="Hime.CentralDogma.Grammars.Terminal"/>.
		/// </returns>
        public override string ToString()
		{
			string result = Value;
            result = result.Replace("\\", "\\\\");
            result = result.Replace("\0", "\\0");
            result = result.Replace("\a", "\\a");
            result = result.Replace("\b", "\\b");
            result = result.Replace("\f", "\\f");
            result = result.Replace("\n", "\\n");
            result = result.Replace("\r", "\\r");
            result = result.Replace("\t", "\\t");
            result = result.Replace("\v", "\\v");
            return result;
		}

		/// <summary>
		/// Represents a comparer that works on the priority of the terminals
		/// </summary>
		public sealed class PriorityComparer : System.Collections.Generic.IComparer<Terminal>
        {
            public int Compare(Terminal x, Terminal y) { return (x.Priority - y.Priority); }
        }
    }
}
