/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using Hime.Kernel.Reporting;

namespace Hime.Parsers
{
    public interface ParserGenerator
    {
        string Name { get; }
        ParserData Build(Grammar grammar, Reporter reporter);
    }
}