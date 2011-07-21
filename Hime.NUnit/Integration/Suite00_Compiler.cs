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

namespace Hime.NUnit.Integration
{
	[TestFixture]
	public class Suite00_Compiler
	{	
		[Ignore]
        [Test]
        public void Test001_ExecuteLoadData_ShouldNotHaveAnyErrorWhenSectionTerminalsIsNotPresent()
        {
        	string grammar = 
        		"public grammar cf Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
        	
        	Reporter reporter = new Reporter();
        	ResourceCompiler compiler = new ResourceCompiler(reporter);
        	compiler.AddInputRawText(grammar);
        	compiler.Compile();
            Assert.IsFalse(reporter.Result.HasErrors());
        }
	}
}
