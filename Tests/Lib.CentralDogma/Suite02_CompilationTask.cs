/*
 * Author: Charles Hymans
 * Date: 18/07/2011
 * Time: 20:59
 * 
 */
using System;
using NUnit.Framework;
using Hime.Parsers;
using Hime.Utils.Reporting;
using System.IO;

namespace Hime.Tests.CentralDogma
{
	[TestFixture]
	public class Suite02_CompilationTask : BaseTestSuite
	{
		[Test]
        public void Test000_Execute_LR0_ShouldNotFailOnSimpleGrammar()
        {
            string dir = GetTestDirectory();
        	string grammar = 
        		"cf grammar Test { options { Axiom=\"exp\"; } terminals { } rules { exp -> 'x'; } }";
            CompileRaw(grammar, ParsingMethod.LR0);
        }

        // should stop earlier on error of the lexer => do a test with an syntax error in the input. See if it stops early
        [Test]
        public void Test001_Execute_ShouldNotHaveAnyErrorWhenSectionTerminalsIsNotPresent()
        {
            string dir = GetTestDirectory();
            string grammar = 
        		"cf grammar Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
            Report result = CompileRaw(grammar, ParsingMethod.LR0);
            Assert.IsFalse(result.HasErrors);
        }
        
        [Test]
        public void Test002_Execute_LexerFileShouldBeGrammarNameSuffixLexer_Issue198()
        {
            string dir = GetTestDirectory();
            string grammar = 
        		"cf grammar Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
            CompilationTask task = new CompilationTask();
            task.Method = ParsingMethod.LR0;
            task.InputRawData.Add(grammar);
            task.Execute();
            Assert.IsTrue(File.Exists("TestLexer.cs"));
        }
        
        [Test]
        public void Test003_Execute_ParserFileShouldBeGrammarNameSuffixLexer_Issue198()
        {
            string dir = GetTestDirectory();
            string grammar = 
        		"cf grammar Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
            CompilationTask task = new CompilationTask();
            task.Method = ParsingMethod.LR0;
            task.InputRawData.Add(grammar);
            task.Execute();
            Assert.IsTrue(File.Exists("TestParser.cs"));
        }

        [Test]
        public void Test004_Execute_ShouldNotFailWhenExportLogIsSet()
        {
            string dir = GetTestDirectory();
            string grammar =
                "cf grammar Test { options { Axiom=\"exp\"; } terminals { } rules { exp -> 'x'; } }";
            Report result = CompileRaw(grammar, ParsingMethod.LALR1, true);
            Assert.IsFalse(result.HasErrors);
        }

        [Test]
        public void Test005_Execute_ShouldReturnOnlyOneErrorOnUnrecoverableSyntaxError_Issue414()
        {
            string dir = GetTestDirectory();
            string grammar =
                "public text grammar test { options { Axiom = \"exp\"; } rules { exp -> 'x'; } }";
            Report result = CompileRaw(grammar, ParsingMethod.LALR1);
            Assert.AreEqual(1, result.ErrorCount);
        }

        [Test]
        public void Test006_Execute_TheErrorShouldIndicateLineNumber_Issue414()
        {
            // TODO: in that case, error message is not that nice could be improved
            // remplace ; with ,
            // do not put { cf } but "cf" (there is only one case and it is consistent with "text"
            string dir = GetTestDirectory();
            string grammar =
                "grammar test { options { Axiom = \"exp\"; } rules { exp -> 'x'; } }";
            Report result = CompileRaw(grammar, ParsingMethod.LALR1);
            foreach (Entry error in result.Errors)
            {
                // TODO: here should be FATAL: Parser: ...
                Assert.IsTrue(error.ToString().StartsWith("Error: Parser: @(1, 8)"));
            }
        }

        [Test]
        public void Test007_LoadInputs_ShouldNotHaveAnyErrorWhenSectionTerminalsIsNotPresent()
        {
            string dir = GetTestDirectory();
            string grammar =
                "cf grammar Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
            Report result = CompileRaw(grammar, ParsingMethod.LR0);
            Assert.AreEqual(result.ErrorCount, 0);
        }

        [Test]
        public void Test008_CompileData_ShouldHaveErrorsWhenSemiColonIsMissing()
        {
            string dir = GetTestDirectory();
            string grammar =
                "cf grammar Test { options { Axiom=\"exp\" } rules { exp -> 'x'; } }";
            Report result = CompileRaw(grammar, ParsingMethod.LR1);
            Assert.AreNotEqual(result.ErrorCount, 0);
        }

        [Test]
        public void Test009_CompileData_ShouldNotHaveAnyErrorWhenSectionTerminalsIsNotPresent()
        {
            string dir = GetTestDirectory();
            string grammar =
                "cf grammar Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
            Report result = CompileRaw(grammar, ParsingMethod.LR1);
            Assert.AreEqual(result.ErrorCount, 0);
        }

        [Test]
        public void Test010_Compile_ShouldNotThrowExceptionWhenSectionTerminalsIsNotPresent()
        {
            string dir = GetTestDirectory();
            string grammar =
                "cf grammar Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
            Report result = CompileRaw(grammar, ParsingMethod.LR1);
            Assert.AreEqual(result.ErrorCount, 0);
        }

        // TODO: do a test with incorrect syntax but for which Compile returns false (saying it has no errors) even though errors are dumped
        // TODO: remarks on the syntax of grammars:
        // why is cf necessary => should just remove it
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
