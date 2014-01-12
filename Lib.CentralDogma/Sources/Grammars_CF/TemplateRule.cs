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
using Hime.Redist;

namespace Hime.CentralDogma.Grammars.ContextFree
{
    class TemplateRule
    {
        private List<TemplateRuleInstance> instances;
        private CFGrammar grammar;

        internal string HeadName { get; private set; }
        internal List<string> Parameters { get; private set; }
        internal int ParametersCount { get { return this.Parameters.Count; } }
		internal ASTNode RuleNode { get; private set; }
        internal ASTNode DefinitionNode { get; private set; }

        public TemplateRule(CFGrammar grammar, ASTNode ruleNode)
        {
            this.HeadName = ruleNode.Children[0].Symbol.Value;
            this.Parameters = new List<string>();
            this.instances = new List<TemplateRuleInstance>();
            this.grammar = grammar;
            this.RuleNode = ruleNode;
            this.DefinitionNode = ruleNode.Children[2];
            foreach (ASTNode Node in ruleNode.Children[1].Children)
			{
                this.Parameters.Add(Node.Symbol.Value);
			}
        }

        public TemplateRule(TemplateRule copied, CFGrammar data)
        {
            this.HeadName = copied.HeadName;
            this.Parameters = new List<string>(copied.Parameters);
            instances = new List<TemplateRuleInstance>();
            grammar = data;
            this.RuleNode = copied.RuleNode;
            this.DefinitionNode = copied.DefinitionNode;
            foreach (TemplateRuleInstance instance in copied.instances)
            {
                CFVariable headVar = data.GetCFVariable(instance.HeadVariable.Name);
                List<Symbol> Params = new List<Symbol>();
                foreach (Symbol symbol in instance.Parameters)
                    Params.Add(data.GetSymbol(symbol.Name));
                instances.Add(new TemplateRuleInstance(this, Params, headVar));
            }
        }

        public Variable GetVariable(CompilerContext context, List<Symbol> parameters)
        {
            foreach (TemplateRuleInstance instance in instances)
            {
                if (instance.MatchParameters(parameters))
                    return instance.HeadVariable;
            }
            TemplateRuleInstance newInstance = new TemplateRuleInstance(this, parameters, grammar);
            instances.Add(newInstance);
            newInstance.Compile(grammar, context);
            return newInstance.HeadVariable;
        }
    }
}
