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

namespace Hime.Tests.Project0_CentralDogma
{
	[TestFixture]
	public class Suite02_Compiler
	{	
		// TODO: this test should not be in this test suite => move
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
        
		// TODO: this test should not be in this test suite => move
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

		// TODO: this test should not be in this test suite => move
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
        
		// TODO: this test should not be in this test suite => move
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
        
		// TODO: fix this
		[Test, Ignore]
		public void Test004_Execute_ShouldNoteFailWhenExportLogIsSet()
		{
			string grammar = 
        		"public cf text grammar Test { options { Axiom=\"exp\"; } terminals { } rules { exp -> 'x'; } }";
			
            CompilationTask task = new CompilationTask(ParsingMethod.LALR1);
			task.InputRawData.Add(grammar);
			
         	task.ExportLog = true;
			// TODO: build the compiler with the task as argument??
        	Compiler compiler = new Compiler(task);
       	    Report result = compiler.Execute();
			Assert.IsFalse(result.HasErrors);
		}
		
        // TODO: do a test with incorrect syntax but for which Compile returns false (saying it has no errors) even though errors are dumped
	}
}
