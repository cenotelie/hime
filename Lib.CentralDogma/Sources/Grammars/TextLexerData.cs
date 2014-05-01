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
using System.IO;

namespace Hime.CentralDogma.Grammars
{
    class TextLexerData : LexerData
    {
        private Automata.DFA dfa;
        private List<Terminal> terminals;
        private Terminal separator;

        public IList<Terminal> Expected { get { return terminals; } }

        internal TextLexerData(Automata.DFA dfa, Terminal separator)
        {
            this.dfa = dfa;
            this.terminals = new List<Terminal>();
            this.separator = separator;
            this.terminals.Add(Epsilon.Instance);
            this.terminals.Add(Dollar.Instance);
            foreach (Automata.DFAState state in dfa.States)
            {
                if (state.TopItem != null)
                    if (!terminals.Contains(state.TopItem as Terminal))
                        terminals.Add(state.TopItem as Terminal);
            }
        }

        public void ExportCode(StreamWriter stream, string name, AccessModifier modifier, string resource)
        {
            stream.WriteLine("\t/// <summary>");
			stream.WriteLine("\t/// Represents a lexer");
			stream.WriteLine("\t/// </summary>");
			stream.WriteLine("\t" + modifier.ToString().ToLower() + " class " + name + "Lexer : Lexer");
            stream.WriteLine("\t{");
            ExportStatics(stream, name, resource);
            ExportConstructor(stream, name);
            stream.WriteLine("\t}");
        }
        public void ExportData(BinaryWriter stream)
        {
            ExportStates(stream);
        }

        private void ExportConstructor(StreamWriter stream, string name)
        {
            string sep = "FFFF";
            if (separator != null)
				sep = separator.SID.ToString("X");
			stream.WriteLine("\t\t/// <summary>");
			stream.WriteLine("\t\t/// Initializes a new instance of the lexer");
			stream.WriteLine("\t\t/// </summary>");
			stream.WriteLine("\t\t/// <param name=\"input\">The lexer's input</param>");
            stream.WriteLine("\t\tpublic " + name + "Lexer(string input) : base(automaton, terminals, 0x" + sep + ", new System.IO.StringReader(input)) {}");
            stream.WriteLine("\t\t/// <summary>");
			stream.WriteLine("\t\t/// Initializes a new instance of the lexer");
			stream.WriteLine("\t\t/// </summary>");
			stream.WriteLine("\t\t/// <param name=\"input\">The lexer's input</param>");
            stream.WriteLine("\t\tpublic " + name + "Lexer(System.IO.TextReader input) : base(automaton, terminals, 0x" + sep + ", input) {}");
        }
        private void ExportStatics(StreamWriter stream, string name, string resource)
		{
			stream.WriteLine("\t\t/// <summary>");
			stream.WriteLine("\t\t/// The automaton for this lexer");
			stream.WriteLine("\t\t/// </summary>");
			stream.WriteLine("\t\tprivate static readonly Automaton automaton = Automaton.Find(typeof(" + name + "Lexer), \"" + resource + "\");");
            
			for (int i = 2; i != terminals.Count; i++)
			{
				Terminal terminal = terminals[i];
				stream.WriteLine("\t\t/// <summary>");
				stream.WriteLine("\t\t/// The unique identifier for terminal " + terminal.Name);
				stream.WriteLine("\t\t/// </summary>");
				stream.WriteLine("\t\tpublic const int {0} = 0x{1};", terminal.Name, terminal.SID.ToString("X"));
			}

			stream.WriteLine("\t\t/// <summary>");
			stream.WriteLine("\t\t/// The collection of terminals matched by this lexer");
			stream.WriteLine("\t\t/// </summary>");
			stream.WriteLine("\t\t/// <remarks>");
			stream.WriteLine("\t\t/// The terminals are in an order consistent with the automaton,");
			stream.WriteLine("\t\t/// so that terminal indices in the automaton can be used to retrieve the terminals in this table");
			stream.WriteLine("\t\t/// </remarks>");
			stream.WriteLine("\t\tprivate static readonly Symbol[] terminals = {");
            bool first = true;
            foreach (Terminal terminal in terminals)
            {
                if (!first) stream.WriteLine(",");
                stream.Write("\t\t\t");
                stream.Write("new Symbol(0x" + terminal.SID.ToString("X") + ", \"" + terminal.ToString().Replace("\"", "\\\"") + "\")");
                first = false;
            }
            stream.WriteLine(" };");
        }
        private void ExportState(BinaryWriter stream, Automata.DFAState state)
        {
            ushort[] cache = new ushort[256];
            for (int i = 0; i != 256; i++)
                cache[i] = 0xFFFF;
            ushort cached = 0;
            ushort slow = 0;
            foreach (CharSpan span in state.Transitions.Keys)
            {
                if (span.Begin <= 255)
                {
                    cached++;
                    int end = span.End;
                    if (end >= 256)
                    {
                        end = 255;
                        slow++;
                    }
                    for (int i = span.Begin; i <= end; i++)
                        cache[i] = (ushort)state.Transitions[span].ID;
                }
                else
                    slow++;
            }

            if (state.TopItem != null)
                stream.Write((ushort)terminals.IndexOf(state.TopItem as Terminal));
            else
                stream.Write((ushort)0xFFFF);
            stream.Write((ushort)(slow + cached));
            stream.Write(slow);

            for (int i = 0; i != 256; i++)
                stream.Write(cache[i]);

            List<CharSpan> keys = new List<CharSpan>(state.Transitions.Keys);
            keys.Sort(new Comparison<CharSpan>(CharSpan.CompareReverse));
            foreach (CharSpan span in keys)
            {
                if (span.End <= 255)
                    break; // the rest of the transitions are in the cache
                ushort begin = span.Begin;
                if (begin <= 255)
                    begin = 256;
                stream.Write(begin);
                stream.Write(System.Convert.ToUInt16(span.End));
                stream.Write((ushort)state.Transitions[span].ID);
                slow--;
            }
        }
        private void ExportStates(BinaryWriter stream)
        {
            stream.Write((uint)dfa.StatesCount);
            uint offset = 0;
            foreach (Automata.DFAState state in dfa.States)
            {
                stream.Write(offset);
                offset += 3 + 256;
                foreach (CharSpan key in state.Transitions.Keys)
                    if (key.End >= 256)
                        offset += 3;
            }
            foreach (Automata.DFAState state in dfa.States)
                ExportState(stream, state);
        }
    }
}
