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

        public override void Inherit(Grammar parent)
        {
            InheritOptions(parent);
            InheritActions(parent);
            InheritVirtuals(parent);
            foreach (TerminalBin terminal in parent.Terminals)
                AddTerminalBin(terminal.Type, terminal.LocalName);
            InheritTemplateRules(parent as CFGrammar);
            InheritVariables(parent as CFGrammar);
        }
        // TODO: factor code with CFGrammarText
        public override Grammar Clone()
        {
            CFGrammar result = new CFGrammarBinary(name);
            result.Inherit(this);
            return result;
        }

        public override LexerData GetLexerData(Reporter reporter)
        {
            throw new NotImplementedException();
        }
    }
}
