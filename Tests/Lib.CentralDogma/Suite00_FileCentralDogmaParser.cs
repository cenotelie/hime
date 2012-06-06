/*
 * Author: Charles Hymans
 * Date: 25/07/2011
 * Time: 19:11
 * 
 */
using System;
using System.IO;
using NUnit.Framework;
using Hime.Redist.Parsers;

namespace Hime.Tests.CentralDogma
{
	[TestFixture]
	public class Suite00_FileCentralDogmaParser
	{
		[Test]
		public void Test000_Analyse_ShouldReturnNullOnEmptyString()
        {
        	string grammar = "";

            Hime.Parsers.Input.FileCentralDogmaLexer lexer = new Hime.Parsers.Input.FileCentralDogmaLexer(grammar);
            Hime.Parsers.Input.FileCentralDogmaParser parser = new Hime.Parsers.Input.FileCentralDogmaParser(lexer);
            if (parser.Parse() != null)
                Assert.Fail();
        }

		[Test]
		public void Test001_Analyse_ShouldReturnNullWhenSemiColonIsMissing()
        {
        	string grammar = 
        		"cf grammar Test { options { Axiom=\"exp\" } terminals { } rules { exp -> 'x'; } }";

            Hime.Parsers.Input.FileCentralDogmaLexer lexer = new Hime.Parsers.Input.FileCentralDogmaLexer(grammar);
            Hime.Parsers.Input.FileCentralDogmaParser parser = new Hime.Parsers.Input.FileCentralDogmaParser(lexer);
            if (parser.Parse() != null)
                Assert.Fail();
        }
		
		[Test]
		public void Test002_Analyse_ShouldAcceptMissingSectionTerminal()
        {
        	string grammar = 
        		"cf grammar Test { options { Axiom=\"exp\"; } rules { exp -> 'x'; } }";

            Hime.Parsers.Input.FileCentralDogmaLexer lexer = new Hime.Parsers.Input.FileCentralDogmaLexer(grammar);
            Hime.Parsers.Input.FileCentralDogmaParser parser = new Hime.Parsers.Input.FileCentralDogmaParser(lexer);
            parser.Parse();
            Assert.AreEqual(0, parser.Errors.Count);
        }
	}
}
