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

using System.Xml;

namespace Hime.CentralDogma.Grammars
{
    class TextTerminal : Terminal
    {
        private string outputValue;

        public Automata.NFA NFA { get; set; }
        public string Value { get; private set; }

        public TextTerminal(ushort sid, string name, string value, int priority, Automata.NFA nfa)
            : base(sid, name, priority)
        {
            this.NFA = nfa;
            this.Value = value;
            this.outputValue = SanitizedValue(value);
        }

        private string SanitizedValue(string value)
        {
            string result = value;
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

        public override string ToString() { return outputValue; }
    }
}