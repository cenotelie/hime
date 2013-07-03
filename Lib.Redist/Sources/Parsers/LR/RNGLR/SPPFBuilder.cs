using System;
using System.Collections.Generic;

namespace Hime.Redist.Parsers
{
	class SPPFBuilder
	{
		private const int handleSize = 1024;
        private const int estimationBias = 5;
		
		// Final AST
        private ParseTree tree;



        public SPPF AcquireNode(Symbols.Symbol symbol)
        {
            return null;
        }

        // Prepare the reduction of a null variable
		public void ReductionPrepare() { }

        // Prepare a normal reduction
        public void ReductionPrepare(GSSPath path, int length, SPPF first) { }

        public void ReductionPop(TreeAction action) { }

        public void ReductionVirtual(Symbols.Virtual symbol, TreeAction action) { }

        public void ReductionSemantic(SemanticAction callback) { }

        public void ReductionNullVariable(SPPF sub, TreeAction action) { }

        public void Reduce(SPPF root, bool isNodeNew) { }
        


        public ParseTree GetTree(SPPF root)
        {
            return tree;
        }
	}
}
