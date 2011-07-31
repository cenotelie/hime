/*
 * Author: Charles Hymans
 * Date: 25/07/2011
 * Time: 19:11
 * 
 */
using System;
using NUnit.Framework;
using Hime.Kernel.Resources.Parser;
using Hime.Redist.Parsers;

namespace Hime.NUnit.Integration
{
	[TestFixture]
	public class Suite00_FileCentralDogmaParser
	{
		[Test]
		public void Test000_Analyse_ShouldNotThrowExceptionOnEmptyString()
        {
        	string grammar = "";
        	
        	FileCentralDogma_Lexer lexer = new FileCentralDogma_Lexer(grammar);
            FileCentralDogma_Parser parser = new FileCentralDogma_Parser(lexer);
            parser.Analyse();
        }

		[Test]
		public void Test001_Analyse_ShouldNotThrowExceptionWhenSemiColonIsMissing()
        {
        	string grammar = 
        		"public cf text grammar Test { options { Axiom=\"exp\" } terminals { } rules { exp -> 'x'; } }";
        	
        	FileCentralDogma_Lexer lexer = new FileCentralDogma_Lexer(grammar);
            FileCentralDogma_Parser parser = new FileCentralDogma_Parser(lexer);
            parser.Analyse();
        }
		
		[Test]
		public void Test002_Analyse_ShouldAcceptMissingSectionTerminal()
        {
        	string grammar = 
        		"public cf text grammar Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
        	
        	FileCentralDogma_Lexer lexer = new FileCentralDogma_Lexer(grammar);
            FileCentralDogma_Parser parser = new FileCentralDogma_Parser(lexer);
            parser.Analyse();
            Assert.AreEqual(0, parser.Errors.Count);
        }
	}
}
