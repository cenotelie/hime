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
	public class Suite00_CompilationTask
	{
		[Ignore]
		[Test]
        public void Test000_ShouldNotFailWhenSectionTerminalsIsNotPresent()
        {
        	string grammar = 
        		"public grammar cf Test { options { Axiom=\"exp\"; } terminals { } rules { exp -> 'x'; } }";
            CompilationTask task = new CompilationTask();
            task.InputRawData.Add(grammar);
			task.GrammarName = "Test";
            task.Method = Parsers.ParsingMethod.LR0;
            task.ExecuteBody();
        }

        [Ignore]
        [Test]
        public void Test001_ShouldNotFailWhenSectionTerminalsIsNotPresent()
        {
        	string grammar = 
        		"public grammar cf Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
            CompilationTask task = new CompilationTask();
            task.InputRawData.Add(grammar);
			task.GrammarName = "Test";
            task.Method = Parsers.ParsingMethod.LALR1;
            task.ExecuteBody();
        }
	}
}
