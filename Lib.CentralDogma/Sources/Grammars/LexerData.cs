using System;
using System.Collections.Generic;
using System.IO;

namespace Hime.CentralDogma.Grammars
{
    interface LexerData
    {
        IList<Terminal> Expected { get; }
        void ExportCode(StreamWriter stream, string name, AccessModifier modifier, string resource);
        void ExportData(BinaryWriter stream);
    }
}