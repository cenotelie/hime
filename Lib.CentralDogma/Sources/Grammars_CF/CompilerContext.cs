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
    class CompilerContext
    {
        private CFGrammarLoader compiler;
        private List<TemplateRule> templateRules;
        private Dictionary<string, Symbol> references;

        public CFGrammarLoader Compiler { get { return compiler; } }

        public CompilerContext(CFGrammarLoader compiler)
        {
            this.compiler = compiler;
            this.templateRules = new List<TemplateRule>();
            this.references = new Dictionary<string, Symbol>();
        }

        public CompilerContext(CompilerContext copied)
        {
            compiler = copied.Compiler;
            templateRules = new List<TemplateRule>(copied.templateRules);
            references = new Dictionary<string, Symbol>();
        }

        public void AddTemplateRule(TemplateRule templateRule) { templateRules.Add(templateRule); }

        public bool IsTemplateRule(string name, int paramCount)
        {
            foreach (TemplateRule tRule in templateRules)
                if ((tRule.HeadName == name) && (tRule.ParametersCount == paramCount))
                    return true;
            return false;
        }

        public Variable GetVariableFromMetaRule(string name, List<Symbol> parameters, CompilerContext context)
        {
            foreach (TemplateRule tRule in templateRules)
            {
                if ((tRule.HeadName == name) && (tRule.ParametersCount == parameters.Count))
                    return tRule.GetVariable(context, parameters);
            }
            return null;
        }

        public void AddReference(string name, Symbol symbol) { references.Add(name, symbol); }
        public bool IsReference(string name) { return references.ContainsKey(name); }
        public Symbol GetReference(string name)
        {
            if (references.ContainsKey(name))
                return references[name];
            return null;
        }
    }
}
