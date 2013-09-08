/**********************************************************************
* Copyright (c) 2013 Laurent Wouters and others
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as
* published by the Free Software Foundation, either version 3
* of the License, or (at your option) any later version.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
* 
* You should have received a copy of the GNU Lesser General
* Public License along with this program.
* If not, see <http://www.gnu.org/licenses/>.
* 
* Contributors:
*     Laurent Wouters - lwouters@xowl.org
**********************************************************************/

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