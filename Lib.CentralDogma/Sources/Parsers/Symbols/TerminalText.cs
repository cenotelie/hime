/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Xml;

namespace Hime.Parsers
{
    class TerminalText : Terminal
    {
        public Automata.NFA NFA { get; set; }
        public string Value { get; private set; }

        public TerminalText(ushort sid, string name, int priority, Automata.NFA nfa)
            : base(sid, name, priority)
        {
            this.NFA = nfa;
            this.Value = name;
            if (this.Value.StartsWith("@\""))
                this.Value = this.Value.Substring(2, this.Value.Length - 3);
        }

        public override string ToString() { return Value; }
    }
}