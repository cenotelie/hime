/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System;
using System.IO;

namespace Hime.Kernel.Documentation
{
    internal class MHTMLSourceStream : MHTMLSource
    {
        internal MHTMLSourceStream(string mime, string location, Stream stream) : base(mime, location, stream)
        {
        }
    }
}