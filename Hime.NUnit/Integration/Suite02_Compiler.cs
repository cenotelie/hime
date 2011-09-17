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
        		"public cf text grammar Test { options { Axiom=\"exp\" } terminals { } rules { exp -> 'x'; } }";
        	
        	Reporter reporter = new Reporter();
        	ResourceLoader compiler = new ResourceLoader(reporter);
            StringReader reader = new StringReader(grammar);
            compiler.CompileData(reader);
            reader.Close();
            Assert.IsTrue(reporter.Result.HasErrors);
        }
        
        [Test]
        public void Test001_CompileData_ShouldNotHaveAnyErrorWhenSectionTerminalsIsNotPresent()
        {
        	string grammar = 
        		"public cf text grammar Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
        	
        	Reporter reporter = new Reporter();
        	ResourceLoader compiler = new ResourceLoader(reporter);
            StringReader reader = new StringReader(grammar);
            compiler.CompileData(reader);
            reader.Close();
            Assert.IsFalse(reporter.Result.HasErrors);
        }

        // TODO:
        [Test]
        public void Test002_Compile_ShouldNotThrowExceptionWhenSectionTerminalsIsNotPresent()
        {
        	string grammar = 
        		"public cf text grammar Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
        	
        	Reporter reporter = new Reporter();
        	ResourceLoader compiler = new ResourceLoader(reporter);
        	// TODO: this is a bit strange, think about it
            StringReader reader = new StringReader(grammar);
            compiler.AddInput(reader);
            compiler.Load();
            reader.Close();
        }
        
        // TODO:
        [Test]
        public void Test003_Compile_ShouldNotHaveAnyErrorWhenSectionTerminalsIsNotPresent()
        {
        	string grammar = 
        		"public cf text grammar Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
        	
        	Reporter reporter = new Reporter();
        	ResourceLoader compiler = new ResourceLoader(reporter);
        	// TODO: this is a bit strange, think about it
            StringReader reader = new StringReader(grammar);
            compiler.AddInput(reader);
            compiler.Load();
            reader.Close();
            Assert.IsFalse(reporter.Result.HasErrors);
        }
        
        // TODO: do a test with incorrect syntax but for which Compile returns false (saying it has no errors) even though errors are dumped
	}
}
