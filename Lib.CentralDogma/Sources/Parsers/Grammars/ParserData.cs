/*
 * Author: Charles Hymans
 * Date: 06/08/2011
 * Time: 23:03
 * 
 */
using System.Xml;
using System.Collections.Generic;
using System.IO;
using Hime.Utils.Reporting;

namespace Hime.Parsers
{
    interface ParserData
    {
        void ExportCode(StreamWriter stream, string name, AccessModifier modifier, string resource, IList<Terminal> expected);
        void ExportData(BinaryWriter stream);
        void Document(string directory);
    }
}
