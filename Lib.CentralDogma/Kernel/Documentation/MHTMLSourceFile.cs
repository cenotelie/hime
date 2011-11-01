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
    internal class MHTMLSourceFile : MHTMLSource
    {
        internal MHTMLSourceFile(string mime, string location, string file) : base(mime, location, new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.None))
        {
        }
    }
}