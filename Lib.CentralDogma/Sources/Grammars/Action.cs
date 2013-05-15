using System.Xml;
using System.Collections.Generic;

namespace Hime.CentralDogma.Grammars
{
    class Action : Symbol
    {
        public Action(string name) : base(0, name) { }

        protected override string Type { get { return "Action"; } }

        public override string ToString() { return "{" + Name + "}"; }
    }
}
