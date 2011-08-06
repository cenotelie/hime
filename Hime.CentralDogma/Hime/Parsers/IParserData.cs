/*
 * Author: Charles Hymans
 * Date: 06/08/2011
 * Time: 23:03
 * 
 */
using System.Xml;
using System.Collections.Generic;

namespace Hime.Parsers
{
    public interface IParserData
    {
        ParserGenerator Generator { get; }
        bool Export(IList<Terminal> expected, CompilationTask options);
        System.Xml.XmlNode SerializeXML(XmlDocument Document);
        List<string> SerializeVisuals(string directory, CompilationTask options);
    }
}
