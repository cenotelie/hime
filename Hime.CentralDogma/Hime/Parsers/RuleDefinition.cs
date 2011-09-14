/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers
{
    public abstract class RuleDefinition
    {
        protected List<RuleDefinitionPart> parts;

        public int Length { get { return parts.Count; } }
        public List<RuleDefinitionPart> Parts { get { return parts; } }


        public RuleDefinition() { parts = new List<RuleDefinitionPart>(); }
        public RuleDefinition(ICollection<RuleDefinitionPart> parts)
        {
            this.parts = new List<RuleDefinitionPart>();
            this.parts.AddRange(parts);
        }
        public RuleDefinition(Symbol symbol)
        {
            parts = new List<RuleDefinitionPart>();
            parts.Add(new RuleDefinitionPart(symbol, RuleDefinitionPartAction.Nothing));
        }

        public abstract Symbol GetSymbolAt(int index);


        public override int GetHashCode() { return base.GetHashCode(); }
        public override bool Equals(object obj)
        {
            RuleDefinition def = obj as RuleDefinition;
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
            foreach (RuleDefinitionPart part in parts)
            {
                builder.Append(" ");
                builder.Append(part.ToString());
            }
            return builder.ToString();
        }
    }
}
