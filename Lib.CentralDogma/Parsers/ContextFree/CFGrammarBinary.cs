/*
 * Author: Charles Hymans
 * Date: 18/07/2011
 * Time: 23:19
 * 
 */
using System;
using System.Collections.Generic;
using Hime.Kernel.Reporting;

namespace Hime.Parsers.ContextFree
{
    public sealed class CFGrammarBinary : CFGrammar
    {
        public CFGrammarBinary(string name) : base(name) { }

        protected override void InheritTerminals(CFGrammar parent)
        {
            foreach (TerminalBin terminal in parent.Terminals)
                AddTerminalBin(terminal.Type, terminal.LocalName);
        }

        protected override Grammar CreateCopy() { return new CFGrammarBinary(name); }

        public override LexerData GetLexerData(Reporter reporter)
        {
            throw new NotImplementedException();
        }
    }
}
