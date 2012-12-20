/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Xml;

namespace Hime.CentralDogma.Grammars
{
    class Virtual : Symbol
    {
        public Virtual(string name) : base(0, name) { }

        protected override string Type { get { return "Virtual"; } }

        public override string ToString() { return "\"" + Name + "\""; }
    }
}