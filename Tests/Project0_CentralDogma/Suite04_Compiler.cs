/*
 * Author: Charles Hymans
 * Date: 21/07/2011
 * Time: 22:20
 * 
 */
using System;
using NUnit.Framework;
using Hime.Kernel;
using Hime.Parsers;
using Hime.Kernel.Resources;
using Hime.Kernel.Reporting;
using System.IO;
using Hime.Kernel.Resources.Parser;
using Hime.Redist.Parsers;

namespace Hime.Tests.Project0_CentralDogma
{
	[TestFixture]
	public class Suite04_Compiler
	{	
		[Test]
		public void Test000_Execute_ShouldNotFailWhenExportLogIsSet()
		{
			string grammar = 
        		"public cf text grammar Test { options { Axiom=\"exp\"; } terminals { } rules { exp -> 'x'; } }";
			
            CompilationTask task = new CompilationTask(ParsingMethod.LALR1);
			task.InputRawData.Add(grammar);
			
         	task.ExportLog = true;

			Compiler compiler = new Compiler(task);
       	    Report result = compiler.Execute();
			Assert.IsFalse(result.HasErrors);
		}

		[Test]
		public void Test001_Execute_ShouldReturnOnlyOneErrorOnUnrecoverableSyntaxError_Issue414()
		{
			string grammar = 
        		"public text grammar test { options { Axiom = \"exp\"; } rules { exp -> 'x'; } }";
			
            CompilationTask task = new CompilationTask(ParsingMethod.LALR1);
			task.InputRawData.Add(grammar);
			
        	Compiler compiler = new Compiler(task);
       	    Report result = compiler.Execute();
			Assert.AreEqual(1, result.ErrorCount);
		}
		
		// TODO: help screen is incomplete => add tests
		// TODO: format of errors: maybe should start with line number??
        // TODO: do a test with incorrect syntax but for which Compile returns false (saying it has no errors) even though errors are dumped
		// TODO: remarks on the syntax of grammars:
		// why is cf necessary => should just remove it
		// what does bin/text do
		// TODO: since the Axiom is necessary, it shouldn't be in the option section, it is too verbose
		// TODO: should have a simpler input syntax
		
		/* TODO: on this grammar the error message is not easy to understand
		 * 	
public cf text grammar test
{
	options
	{
	}
	rules
	{
		exp -> 'x';
	}
}
*/
	}
}
