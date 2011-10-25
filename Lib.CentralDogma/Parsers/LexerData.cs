/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System;
using System.Collections.Generic;
using System.IO;
using Hime.Kernel.Reporting;

namespace Hime.Parsers
{
    public interface LexerData
    {
        IList<Terminal> Expected { get; }
        void Export(StreamWriter stream, string className, AccessModifier modifier);
    }
}