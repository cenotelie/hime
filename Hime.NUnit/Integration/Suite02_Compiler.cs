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

namespace Hime.NUnit.Integration
{
	[TestFixture]
	public class Suite02_Compiler
	{	
		[Test]
        public void Test000_CompileData_ShouldHaveErrorsWhenSemiColonIsMissing()
        {
        	string grammar = 
        		"public grammar cf Test { options { Axiom=\"exp\" } terminals { } rules { exp -> 'x'; } }";
        	
        	Reporter reporter = new Reporter();
        	ResourceCompiler compiler = new ResourceCompiler(reporter);
        	compiler.CompileData(new StringReader(grammar));
            Assert.IsTrue(reporter.Result.HasErrors);
        }
        
        [Test]
        public void Test001_CompileData_ShouldNotHaveAnyErrorWhenSectionTerminalsIsNotPresent()
        {
        	string grammar = 
        		"public grammar cf Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
        	
        	Reporter reporter = new Reporter();
        	ResourceCompiler compiler = new ResourceCompiler(reporter);
        	compiler.CompileData(new StringReader(grammar));
            Assert.IsFalse(reporter.Result.HasErrors);
        }

        // TODO:
        [Ignore]
        [Test]
        public void Test002_Compile_ShouldNotThrowExceptionWhenSectionTerminalsIsNotPresent()
        {
        	string grammar = 
        		"public grammar cf Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
        	
        	Reporter reporter = new Reporter();
        	ResourceCompiler compiler = new ResourceCompiler(reporter);
        	// TODO: this is a bit strange, think about it
        	compiler.AddInput(new StringReader(grammar));
        	compiler.Compile();
        }
        
        // TODO:
        [Ignore]
        [Test]
        public void Test003_Compile_ShouldNotHaveAnyErrorWhenSectionTerminalsIsNotPresent()
        {
        	string grammar = 
        		"public grammar cf Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
        	
        	Reporter reporter = new Reporter();
        	ResourceCompiler compiler = new ResourceCompiler(reporter);
        	// TODO: this is a bit strange, think about it
        	compiler.AddInput(new StringReader(grammar));
        	compiler.Compile();
            Assert.IsFalse(reporter.Result.HasErrors);
        }
        
        // TODO: do a test with incorrect syntax but for which Compile returns false (saying it has no errors) even though errors are dumped
	}
}
