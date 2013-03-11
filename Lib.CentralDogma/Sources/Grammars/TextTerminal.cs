/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
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