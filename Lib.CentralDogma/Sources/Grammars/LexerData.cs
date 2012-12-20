/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
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