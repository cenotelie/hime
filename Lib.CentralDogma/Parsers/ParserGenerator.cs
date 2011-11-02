/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using Hime.Kernel.Reporting;
using Hime.Parsers.ContextFree;

namespace Hime.Parsers
{
	// TODO: replace this by BaseMethod and rename BaseMethod into ParserGenerator
    public interface ParserGenerator
    {
		// TODO: check Name is really necessary?
        string Name { get; }
		// TODO: remove this method
        ParserData Build(Grammar grammar, Reporter reporter);
		ParserData Build(CFGrammar grammar);
    }
}