using System.Collections.Generic;
using System.IO;

namespace Hime.CentralDogma.Grammars
{
    interface ParserData
    {
        void ExportCode(StreamWriter stream, string name, AccessModifier modifier, string resource, IList<Terminal> expected);
        void ExportData(BinaryWriter stream);
        void Document(string directory);
    }
}
