/*
 * Author: Charles Hymans
 * 
 */
using System;
using NUnit.Framework;
using Hime.Utils;
using Hime.Parsers;
using Hime.Utils.Resources;
using Hime.Utils.Reporting;
using System.IO;
using Hime.Redist.Parsers;

namespace Hime.Tests.Project0_CentralDogma
{
	[TestFixture]
	public class Suite03_ResourceLoader
	{	
		// TODO: think about it, but why is this class called ResourceLoader, this is really strange
		[Test]
        public void Test000_CompileData_ShouldHaveErrorsWhenSemiColonIsMissing()
        {
        	string grammar = 
        		"cf grammar Test { options { Axiom=\"exp\" } rules { exp -> 'x'; } }";

            Hime.Parsers.CompilationTask task = new CompilationTask(ParsingMethod.LR1);
            task.InputRawData.Add(grammar);
            Hime.Parsers.Compiler compiler = new Compiler(task);
            Assert.IsTrue(compiler.LoadInputs());
        }

		[Test]
        public void Test001_CompileData_ShouldHaveErrorsWhenSemiColonIsMissing()
        {
        	string grammar = 
        		"cf grammar Test { options { Axiom=\"exp\" } terminals { } rules { exp -> 'x'; } }";

            Hime.Parsers.CompilationTask task = new CompilationTask(ParsingMethod.LR1);
            task.InputRawData.Add(grammar);
            Hime.Parsers.Compiler compiler = new Compiler(task);
            Assert.IsTrue(compiler.LoadInputs());
        }
        
		[Test]
        public void Test002_CompileData_ShouldNotHaveAnyErrorWhenSectionTerminalsIsNotPresent()
        {
        	string grammar = 
        		"cf grammar Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";

            Hime.Parsers.CompilationTask task = new CompilationTask(ParsingMethod.LR1);
            task.InputRawData.Add(grammar);
            Hime.Parsers.Compiler compiler = new Compiler(task);
            Assert.IsFalse(compiler.LoadInputs());
        }

		// TODO: this test should not be in this test suite => move
		[Test]
        public void Test003_Compile_ShouldNotThrowExceptionWhenSectionTerminalsIsNotPresent()
        {
        	string grammar = 
        		"cf grammar Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";

            Hime.Parsers.CompilationTask task = new CompilationTask(ParsingMethod.LR1);
            task.InputRawData.Add(grammar);
            Hime.Parsers.Compiler compiler = new Compiler(task);
            compiler.LoadInputs();
        }
        
		// TODO: this test should not be in this test suite => move
        [Test]
        public void Test004_Compile_ShouldNotHaveAnyErrorWhenSectionTerminalsIsNotPresent()
        {
        	string grammar = 
        		"cf grammar Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";

            Hime.Parsers.CompilationTask task = new CompilationTask(ParsingMethod.LR1);
            task.InputRawData.Add(grammar);
            Hime.Parsers.Compiler compiler = new Compiler(task);
            Assert.IsFalse(compiler.LoadInputs());
        }
        
        // TODO: do a test with incorrect syntax but for which Compile returns false (saying it has no errors) even though errors are dumped
		// TODO: remarks on the syntax of grammars:
		// why is cf necessary => should just remove it
		// what does bin/text do
		// TODO: since the Axiom is necessary, it shouldn't be in the option section, it is too verbose
		// TODO: should have a simpler input syntax
		
		/* TODO: on this grammar the error message is not easy to understand
		 * 	
cf grammar test
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
