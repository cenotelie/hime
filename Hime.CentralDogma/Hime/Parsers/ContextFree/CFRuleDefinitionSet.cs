/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.Parsers.ContextFree
{
    public sealed class CFRuleDefinitionSet : List<CFRuleDefinition>
    {
        public static CFRuleDefinitionSet operator +(CFRuleDefinitionSet left, CFRuleDefinitionSet right)
        {
            CFRuleDefinitionSet result = new CFRuleDefinitionSet();
            result.AddRange(left);
            result.AddRange(right);
            return result;
        }

        public static CFRuleDefinitionSet operator *(CFRuleDefinitionSet left, CFRuleDefinitionSet right)
        {
            CFRuleDefinitionSet result = new CFRuleDefinitionSet();
            foreach (CFRuleDefinition defLeft in left)
                foreach (CFRuleDefinition defRight in right)
                    result.Add(defLeft + defRight);
            return result;
        }

        public void SetActionPromote()
        {
            foreach (RuleDefinition def in this)
                foreach (RuleDefinitionPart part in def.Parts)
                    part.Action = RuleDefinitionPartAction.Promote;
        }

        public void SetActionDrop()
        {
            foreach (RuleDefinition def in this)
                foreach (RuleDefinitionPart part in def.Parts)
                    part.Action = RuleDefinitionPartAction.Drop;
        }
    }
}