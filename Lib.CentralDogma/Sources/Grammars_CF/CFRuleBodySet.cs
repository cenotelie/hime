using System.Collections.Generic;

namespace Hime.CentralDogma.Grammars.ContextFree
{
    sealed class CFRuleBodySet : List<CFRuleBody>
    {
        public static CFRuleBodySet operator +(CFRuleBodySet left, CFRuleBodySet right)
        {
            CFRuleBodySet result = new CFRuleBodySet();
            result.AddRange(left);
            result.AddRange(right);
            return result;
        }

        public static CFRuleBodySet operator *(CFRuleBodySet left, CFRuleBodySet right)
        {
            CFRuleBodySet result = new CFRuleBodySet();
            foreach (CFRuleBody defLeft in left)
                foreach (CFRuleBody defRight in right)
                    result.Add(defLeft + defRight);
            return result;
        }

        public void SetActionPromote()
        {
            foreach (RuleBody def in this)
                foreach (RuleBodyElement part in def.Parts)
                    part.Action = RuleBodyElementAction.Promote;
        }

        public void SetActionDrop()
        {
            foreach (RuleBody def in this)
                foreach (RuleBodyElement part in def.Parts)
                    part.Action = RuleBodyElementAction.Drop;
        }
    }
}