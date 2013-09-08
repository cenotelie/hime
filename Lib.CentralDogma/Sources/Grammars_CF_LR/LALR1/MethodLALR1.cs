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
    class MethodLALR1 : ParserGenerator
    {
        public MethodLALR1(Reporting.Reporter reporter)
            : base("LALR(1)", reporter)
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

		// TODO: remove static methods
        public static Graph ConstructGraph(CFGrammar Grammar)
        {
            Graph GraphLR0 = MethodLR0.ConstructGraph(Grammar);
            KernelGraph Kernels = new KernelGraph(GraphLR0);
            return Kernels.GetGraphLALR1();
        }
    }
}
