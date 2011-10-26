/*
 * Author: Charles Hymans
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
	public class Suite03_ResourceLoader
	{	
		// TODO: think about it, but why is this class called ResourceLoader, this is really strange
		[Test]
        public void Test000_CompileData_ShouldHaveErrorsWhenSemiColonIsMissing()
        {
        	string grammar = 
        		"public cf text grammar Test { options { Axiom=\"exp\" } rules { exp -> 'x'; } }";
        	
        	Reporter reporter = new Reporter();
        	ResourceLoader loader = new ResourceLoader(reporter);
            using (StringReader reader = new StringReader(grammar))
			{
            	loader.CompileData(reader);
			}
            Assert.IsTrue(reporter.Result.HasErrors);
        }

		[Test]
        public void Test001_CompileData_ShouldHaveErrorsWhenSemiColonIsMissing()
        {
        	string grammar = 
        		"public cf text grammar Test { options { Axiom=\"exp\" } terminals { } rules { exp -> 'x'; } }";
        	
        	Reporter reporter = new Reporter();
        	ResourceLoader loader = new ResourceLoader(reporter);
            using (StringReader reader = new StringReader(grammar))
			{
            	loader.CompileData(reader);
			}
            Assert.IsTrue(reporter.Result.HasErrors);
        }
        
		[Test]
        public void Test002_CompileData_ShouldNotHaveAnyErrorWhenSectionTerminalsIsNotPresent()
        {
        	string grammar = 
        		"public cf text grammar Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
        	
        	Reporter reporter = new Reporter();
        	ResourceLoader loader = new ResourceLoader(reporter);
            using (StringReader reader = new StringReader(grammar))
			{
            	loader.CompileData(reader);
			}
            Assert.IsFalse(reporter.Result.HasErrors);
        }

		// TODO: this test should not be in this test suite => move
		[Test]
        public void Test003_Compile_ShouldNotThrowExceptionWhenSectionTerminalsIsNotPresent()
        {
        	string grammar = 
        		"public cf text grammar Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
        	
        	Reporter reporter = new Reporter();
        	ResourceLoader compiler = new ResourceLoader(reporter);
        	// TODO: this is a bit strange, think about it
            using (StringReader reader = new StringReader(grammar))
			{
	            compiler.AddInput(reader);
    	        compiler.Load();
			}
        }
        
		// TODO: this test should not be in this test suite => move
        [Test]
        public void Test004_Compile_ShouldNotHaveAnyErrorWhenSectionTerminalsIsNotPresent()
        {
        	string grammar = 
        		"public cf text grammar Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
        	
        	Reporter reporter = new Reporter();
        	ResourceLoader compiler = new ResourceLoader(reporter);
        	// TODO: this is a bit strange, think about it
            using (StringReader reader = new StringReader(grammar))
			{
	            compiler.AddInput(reader);
    	        compiler.Load();
			}
            Assert.IsFalse(reporter.Result.HasErrors);
        }
        
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
