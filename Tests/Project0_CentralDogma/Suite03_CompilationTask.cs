/*
 * Author: Charles Hymans
 * Date: 18/07/2011
 * Time: 20:59
 * 
 */
using System;
using NUnit.Framework;
using Hime.Parsers;
using Hime.Kernel.Reporting;
using System.IO;

namespace Hime.Tests.Project0_CentralDogma
{
	[TestFixture]
	public class Suite03_CompilationTask
	{
		[Test]
        public void Test000_ExecuteBody_LR0_ShouldNotFailOnSimpleGrammar()
        {
        	string grammar = 
        		"public cf text grammar Test { options { Axiom=\"exp\"; } terminals { } rules { exp -> 'x'; } }";
            CompilationTask task = new CompilationTask(ParsingMethod.LR0);
            task.InputRawData.Add(grammar);
			task.GrammarName = "Test";
            (new Compiler()).ExecuteDo(task);
        }

        [Test]
        public void Test001_ExecuteLoadData_ShouldNotHaveAnyErrorWhenSectionTerminalsIsNotPresent()
        {
        	string grammar = 
        		"public cf text grammar Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
            CompilationTask task = new CompilationTask(ParsingMethod.LR0);
            task.InputRawData.Add(grammar);
			task.GrammarName = "Test";
            Compiler compiler = new Compiler();
            compiler.LoadData(task.InputFiles, task.InputRawData);
            Assert.IsFalse(compiler.Reporter.Result.HasErrors);
        }
        
        // should stop earlier on error of the lexer => do a test with an syntax error in the input. See if it stops early
        [Test]
        public void Test002_Execute_ShouldNotHaveAnyErrorWhenSectionTerminalsIsNotPresent()
        {
        	string grammar = 
        		"public cf text grammar Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
            CompilationTask task = new CompilationTask(ParsingMethod.LR0);
            task.InputRawData.Add(grammar);
			task.GrammarName = "Test";
            Report result = (new Compiler()).Execute(task);
            Assert.IsFalse(result.HasErrors);
        }
        
        [Test]
        public void Test003_Execute_LexerFileShouldBeGrammarNameSuffixLexer_Issue198()
        {
        	string grammar = 
        		"public cf text grammar Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
            CompilationTask task = new CompilationTask(ParsingMethod.LR0);
            task.InputRawData.Add(grammar);
			task.GrammarName = "Test";
			Compiler compiler = new Compiler();
            compiler.Execute(task);
            Assert.IsTrue(File.Exists("TestLexer.cs"));
        }
        
        [Test]
        public void Test004_Execute_ParserFileShouldBeGrammarNameSuffixLexer_Issue198()
        {
        	string grammar = 
        		"public cf text grammar Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
            CompilationTask task = new CompilationTask(ParsingMethod.LR0);
            task.InputRawData.Add(grammar);
			task.GrammarName = "Test";
			Compiler compiler = new Compiler();
            compiler.Execute(task);
            Assert.IsTrue(System.IO.File.Exists("TestParser.cs"));
        }
	}
}
