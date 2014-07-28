/**********************************************************************
* Copyright (c) 2014 Laurent Wouters and others
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as
* published by the Free Software Foundation, either version 3
* of the License, or (at your option) any later version.
*
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
*
* You should have received a copy of the GNU Lesser General
* Public License along with this program.
* If not, see <http://www.gnu.org/licenses/>.
*
* Contributors:
*     Laurent Wouters - lwouters@xowl.org
**********************************************************************/
using System;
using System.Collections.Generic;
using System.Reflection;

using Hime.Redist;

namespace Hime.Tests.Executor
{
	/// <summary>
	/// The main program for the executor
	/// </summary>
	class Program
	{
		/// <summary>
		/// The parser must produce an AST that matches the expected one
		/// </summary>
		private const string VERB_MATCHES = "matches";
		/// <summary>
		/// The parser must produce an AST that do NOT match the expected one
		/// </summary>
		private const string VERB_NOMATCHES = "nomatches";
		/// <summary>
		/// The parser must fail
		/// </summary>
		private const string VERB_FAILS = "fails";

		/// <summary>
		/// The test was successful
		/// </summary>
		private const int RESULT_SUCCESS = 0;
		/// <summary>
		/// The test failed in the end
		/// </summary>
		private const int RESULT_FAILURE_VERB = 1;
		/// <summary>
		/// The test failed in its parsing phase
		/// </summary>
		private const int RESULT_FAILURE_PARSING = 2;

		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name="args">The command-line arguments.</param>
		/// <returns>The success or failure of the test</returns>
		/// <remarks>
		/// Expected arguments are:
		/// * The parser's name
		/// * A verb specifying the type of test, one of: matches, nomatches, fails
		/// </remarks>
		public static int Main(string[] args)
		{
			string parserName = args[0];
			string verb = args[1];
			Program program = new Program();
			return program.Execute(parserName, verb);
		}

		/// <summary>
		/// Executes the specified test
		/// </summary>
		/// <param name="parserName">The parser's name</param>
		/// <param name="verb">A verb specifying the type of test, one of: matches, nomatches, fails</param>
		public int Execute(string parserName, string verb)
		{
			string input = System.IO.File.ReadAllText("input.txt", System.Text.Encoding.UTF8);
			Hime.Redist.Parsers.IParser parser = GetParser(parserName, input);
			ASTNode expected = new ASTNode();
			if (verb != VERB_FAILS)
			{
				string expectedText = System.IO.File.ReadAllText("expected.txt", System.Text.Encoding.UTF8);
				Hime.Redist.Parsers.IParser expectedParser = GetParser("Hime.Tests.Generated.ExpectedTreeParser", expectedText);
				ParseResult result = expectedParser.Parse();
				foreach (ParseError error in result.Errors)
				{
					Console.WriteLine(error);
					string[] context = result.Input.GetContext(error.Position);
					Console.WriteLine(context[0]);
					Console.WriteLine(context[1]);
				}
				if (result.Errors.Count > 0)
					return RESULT_FAILURE_PARSING;
				expected = result.Root;
			}

			switch (verb)
			{
				case VERB_MATCHES:
					return TestMatches(parser, expected);
				case VERB_NOMATCHES:
					return TestNoMatches(parser, expected);
				case VERB_FAILS:
					return TestFails(parser);
			}
			return RESULT_FAILURE_VERB;
		}

		private int TestMatches(Hime.Redist.Parsers.IParser parser, ASTNode expected)
		{
			ParseResult result = parser.Parse();
			foreach (ParseError error in result.Errors)
			{
				Console.WriteLine(error);
				string[] context = result.Input.GetContext(error.Position);
				Console.WriteLine(context[0]);
				Console.WriteLine(context[1]);
			}
			if (!result.IsSuccess)
				return RESULT_FAILURE_PARSING;
			if (result.Errors.Count != 0)
				return RESULT_FAILURE_PARSING;
			bool comparison = Compare(expected, result.Root);
			return comparison ? RESULT_SUCCESS : RESULT_FAILURE_VERB;
		}

		private int TestNoMatches(Hime.Redist.Parsers.IParser parser, ASTNode expected)
		{
			ParseResult result = parser.Parse();
			foreach (ParseError error in result.Errors)
			{
				Console.WriteLine(error);
				string[] context = result.Input.GetContext(error.Position);
				Console.WriteLine(context[0]);
				Console.WriteLine(context[1]);
			}
			if (!result.IsSuccess)
				return RESULT_FAILURE_PARSING;
			if (result.Errors.Count != 0)
				return RESULT_FAILURE_PARSING;
			bool comparison = Compare(expected, result.Root);
			return comparison ? RESULT_FAILURE_VERB : RESULT_SUCCESS;
		}

		private int TestFails(Hime.Redist.Parsers.IParser parser)
		{
			ParseResult result = parser.Parse();
			if (!result.IsSuccess)
				return RESULT_SUCCESS;
			if (result.Errors.Count != 0)
				return RESULT_SUCCESS;
			return RESULT_FAILURE_VERB;
		}

		/// <summary>
		/// Compare the specified AST node to the expected node
		/// </summary>
		/// <param name="expected">The expected node</param>
		/// <param name="node">The AST node to compare</param>
		/// <returns><c>true</c> if the nodes match</returns>
		private bool Compare(ASTNode expected, ASTNode node)
		{
			if (node.Symbol.Name != expected.Symbol.Value)
				return false;
			if (expected.Children[0].Children.Count > 0)
			{
				string test = expected.Children[0].Children[0].Symbol.Value;
				string vRef = expected.Children[0].Children[1].Symbol.Value;
				vRef = vRef.Substring(1, vRef.Length - 2).Replace("\\'", "'").Replace("\\\\", "\\");
				string vReal = node.Symbol.Value;
				if (test == "=" && vReal != vRef)
					return false;
				if (test == "!=" && vReal == vRef)
					return false;
			}
			if (node.Children.Count != expected.Children[1].Children.Count)
				return false;
			for (int i = 0; i != node.Children.Count; i++)
				if (!Compare(expected.Children[1].Children[i], node.Children[i]))
					return false;
			return true;
		}

		/// <summary>
		/// Gets the parser for the specified assembly and input
		/// </summary>
		/// <param name="parserName">The parser's name</param>
		/// <param name="input">An input for the parser</param>
		/// <returns>The parser</returns>
		private Hime.Redist.Parsers.IParser GetParser(string parserName, string input)
		{
			Assembly assembly = Assembly.LoadFile("Parsers.dll");
			Type parserType = assembly.GetType(parserName);
			ConstructorInfo parserCtor = parserType.GetConstructors()[0];
			ParameterInfo[] parameters = parserCtor.GetParameters();
			Type lexerType = parameters[0].ParameterType;
			ConstructorInfo lexerCtor = lexerType.GetConstructor(new Type[] { typeof(string) });
			object lexer = lexerCtor.Invoke(new object[] { input });
			return (Hime.Redist.Parsers.IParser)parserCtor.Invoke(new object[] { lexer });
		}
	}
}