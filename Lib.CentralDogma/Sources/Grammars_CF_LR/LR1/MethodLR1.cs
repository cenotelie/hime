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

namespace Hime.CentralDogma.Grammars.ContextFree.LR
{
    internal class MethodLR1 : ParserGenerator
    {
        internal MethodLR1(Reporting.Reporter reporter) : base("LR(1)", reporter)
		{
		}

		protected override Graph BuildGraph (CFGrammar grammar)
		{
			return ConstructGraph(grammar);
		}
		
		protected override ParserData BuildParserData (CFGrammar grammar)
		{
			return new ParserDataLRk(this.reporter, grammar, this.graph);
		}
		
		// TODO: try to remove static methods
        internal static Graph ConstructGraph(CFGrammar grammar)
        {
            // Create the first set
            CFVariable AxiomVar = grammar.GetCFVariable("_Axiom_");
            ItemLR1 AxiomItem = new ItemLR1(AxiomVar.CFRules[0], 0, Epsilon.Instance);
            StateKernel AxiomKernel = new StateKernel();
            AxiomKernel.AddItem(AxiomItem);
            State AxiomSet = AxiomKernel.GetClosure();
            Graph graph = new Graph(AxiomSet);
            // Construct the graph
            foreach (State Set in graph.States)
			{
                Set.BuildReductions(new StateReductionsLR1());
			}
            return graph;
        }
    }
}
