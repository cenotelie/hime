/*
 * Author: Charles Hymans
 * Date: 06/08/2011
 * Time: 23:03
 * 
 */
using System.Xml;
using System.Collections.Generic;
using System.IO;
using Hime.Kernel.Reporting;
using Hime.Parsers.ContextFree.LR;

namespace Hime.Parsers
{
    public interface ParserData
    {
        void Export(StreamWriter stream, string className, AccessModifier modifier, string lexerClassName, IList<Terminal> expected, bool exportDebug);
        void Document(string file, bool exportVisuals, string dotBin);
    }
}
