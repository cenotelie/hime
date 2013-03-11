/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.CentralDogma.Grammars
{
    abstract class RuleBody
    {
        protected List<RuleBodyElement> parts;

        public int Length { get { return parts.Count; } }
        public List<RuleBodyElement> Parts { get { return parts; } }


        public RuleBody() { parts = new List<RuleBodyElement>(); }
        public RuleBody(ICollection<RuleBodyElement> parts)
        {
            this.parts = new List<RuleBodyElement>();
            this.parts.AddRange(parts);
        }
        public RuleBody(Symbol symbol)
        {
            parts = new List<RuleBodyElement>();
            parts.Add(new RuleBodyElement(symbol, RuleBodyElementAction.Nothing));
        }

        public abstract Symbol GetSymbolAt(int index);

        public abstract System.Xml.XmlNode GetXMLNode(System.Xml.XmlDocument document);

        public override int GetHashCode() { return base.GetHashCode(); }
        public override bool Equals(object obj)
        {
            RuleBody def = obj as RuleBody;
            if (this.parts.Count != def.parts.Count)
                return false;
            for (int i = 0; i != this.parts.Count; i++)
                if (!this.parts[i].Equals(def.parts[i]))
                    return false;
            return true;
        }
        public override string ToString()
        {
            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            foreach (RuleBodyElement part in parts)
            {
                builder.Append(" ");
                builder.Append(part.ToString());
            }
            return builder.ToString();
        }
    }
}
