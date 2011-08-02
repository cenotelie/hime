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

namespace Hime.NUnit.Integration
{
	[TestFixture]
	public class Suite03_CompilationTask
	{
		[Test]
        public void Test000_ExecuteBody_LR0_ShouldNotFailOnSimpleGrammar()
        {
        	string grammar = 
        		"public cf text grammar Test { options { Axiom=\"exp\"; } terminals { } rules { exp -> 'x'; } }";
            CompilationTask task = new CompilationTask();
            task.InputRawData.Add(grammar);
			task.GrammarName = "Test";
            task.Method = Parsers.ParsingMethod.LR0;
            task.ExecuteBody();
        }

        [Test]
        public void Test001_ExecuteLoadData_ShouldNotHaveAnyErrorWhenSectionTerminalsIsNotPresent()
        {
        	string grammar = 
        		"public cf text grammar Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
            CompilationTask task = new CompilationTask();
            task.InputRawData.Add(grammar);
			task.GrammarName = "Test";
            task.Method = Parsers.ParsingMethod.LR0;
            task.ExecuteLoadData();
            Assert.IsFalse(task.Result.HasErrors);
        }
        
        // should stop earlier on error of the lexer => do a test with an syntax error in the input. See if it stops early
        [Test]
        public void Test002_Execute_ShouldNotHaveAnyErrorWhenSectionTerminalsIsNotPresent()
        {
        	string grammar = 
        		"public cf text grammar Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
            CompilationTask task = new CompilationTask();
            task.InputRawData.Add(grammar);
			task.GrammarName = "Test";
            task.Method = Parsers.ParsingMethod.LR0;
            Report result = task.Execute();
            Assert.IsFalse(result.HasErrors);
        }
        
        // TODO:
        [Ignore]
        [Test]
        public void Test003_Execute_LexerFileShouldBeGrammarNameSuffixLexer_Issue198()
        {
        	string grammar = 
        		"public cf text grammar Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
            CompilationTask task = new CompilationTask();
            task.InputRawData.Add(grammar);
			task.GrammarName = "Test";
            task.Method = Parsers.ParsingMethod.LR0;
            Report result = task.Execute();
            Assert.AreEqual("TestLexer.cs", task.LexerFile);
        }
        
        [Ignore]
        [Test]
        public void Test004_Execute_ParserFileShouldBeGrammarNameSuffixLexer_Issue198()
        {
        	string grammar = 
        		"public cf text grammar Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
            CompilationTask task = new CompilationTask();
            task.InputRawData.Add(grammar);
			task.GrammarName = "Test";
            task.Method = Parsers.ParsingMethod.LR0;
            Report result = task.Execute();
            Assert.AreEqual("TestParser.cs", task.ParserFile);
        }
	}
}
