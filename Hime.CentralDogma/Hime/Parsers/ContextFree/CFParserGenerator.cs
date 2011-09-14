/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;
using Hime.Kernel.Reporting;

namespace Hime.Parsers.ContextFree
{
    public interface CFParserGenerator : ParserGenerator
    {
        ParserData Build(CFGrammar grammar, Hime.Kernel.Reporting.Reporter reporter);
    }
}