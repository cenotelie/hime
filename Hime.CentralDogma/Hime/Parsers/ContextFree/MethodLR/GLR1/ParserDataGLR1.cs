/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System;
using System.IO;
using System.Collections.Generic;
using Hime.Kernel.Reporting;

namespace Hime.Parsers.ContextFree.LR
{
    class ParserDataGLR1 : ParserDataLR
    {
        public ParserDataGLR1(Reporter reporter, CFGrammar gram, Graph graph) : base(reporter, gram, graph) { }

		internal protected override string GetBaseClassName 
		{
			// TODO: why is it a NotImplementedException here? Will it be implemented some day, or is it a problem in the class hierarchy?
			get { throw new NotImplementedException ();	}
		}
		
		// TODO: think about it, but shouldn't stream be a field of the class? or create a new class?
        public override void Export(StreamWriter stream, string className, AccessModifier modifier, string lexerClassName, IList<Terminal> expected, bool exportDebug)
        {
			// TODO: why is it a NotImplementedException here? Will it be implemented some day, or is it a problem in the class hierarchy?
            throw new NotImplementedException();
        }
		
		protected override void ExportRules (StreamWriter stream)
		{
			// TODO: why is it a NotImplementedException here? Will it be implemented some day, or is it a problem in the class hierarchy?
			throw new NotImplementedException ();
		}
		
		protected override void ExportStates (StreamWriter stream)
		{
			throw new NotImplementedException ();
		}
		
		protected override void ExportActions (StreamWriter stream)
		{
			throw new NotImplementedException ();
		}
		
		protected override void ExportSetup (StreamWriter stream)
		{
			throw new NotImplementedException ();
		}
    }
}
