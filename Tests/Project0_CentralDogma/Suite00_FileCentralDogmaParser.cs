/*
 * Author: Charles Hymans
 * Date: 25/07/2011
 * Time: 19:11
 * 
 */
using System;
using System.IO;
using NUnit.Framework;
using Hime.Kernel.Resources.Parser;
using Hime.Redist.Parsers;

namespace Hime.Tests.Project0_CentralDogma
{
	[TestFixture]
	public class Suite00_FileCentralDogmaParser
	{
		[Test]
		public void Test000_Analyse_ShouldThrowParserExceptionOnEmptyString()
        {
        	string grammar = "";
        	
        	FileCentralDogmaLexer lexer = new FileCentralDogmaLexer(grammar);
            FileCentralDogmaParser parser = new FileCentralDogmaParser(lexer);
			try
			{
            	parser.Analyse();
				Assert.Fail();
			} catch (ParserException)
			{
			}
        }

		[Test]
		public void Test001_Analyse_ShouldThrowParserExceptionWhenSemiColonIsMissing()
        {
        	string grammar = 
        		"public cf text grammar Test { options { Axiom=\"exp\" } terminals { } rules { exp -> 'x'; } }";
        	
        	FileCentralDogmaLexer lexer = new FileCentralDogmaLexer(grammar);
            FileCentralDogmaParser parser = new FileCentralDogmaParser(lexer);
			try
			{
            	parser.Analyse();
				Assert.Fail();
			} catch (ParserException)
			{
			}
        }
		
		[Test]
		public void Test002_Analyse_ShouldAcceptMissingSectionTerminal()
        {
        	string grammar = 
        		"public cf text grammar Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";
        	
        	FileCentralDogmaLexer lexer = new FileCentralDogmaLexer(grammar);
            FileCentralDogmaParser parser = new FileCentralDogmaParser(lexer);
            parser.Analyse();
            Assert.AreEqual(0, parser.Errors.Count);
        }
	}
}
