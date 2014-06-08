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
using System.Xml;

using Hime.Redist;

namespace Hime.Tests.Executor
{
	/// <summary>
	/// The main program for the executor
	/// </summary>
	class Program
	{
		private const string VERB_MATCHES = "matches";
		private const string VERB_NOMATCHES = "nomatches";
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
		/// * The path to the compiled assembly containing the parser to test
		/// * A quoted string with the input to be parsed
		/// * A verb specifying the type of test, one of: matches, nomatches, fails
		/// * Optionnally, the path to the XML file containing the expected parse tree
		/// </remarks>
		public static int Main(string[] args)
		{
			string pathToAssembly = GetValue(args[0]);
			string input = GetValue(args[1]);
			string verb = args[2];
			string pathToExpected = null;
			if (verb != VERB_FAILS)
				pathToExpected = GetValue(args[3]);
			Program program = new Program();
			return program.Execute(pathToAssembly, input, verb, pathToExpected);
		}

		private static string GetValue(string arg)
		{
			if (arg.StartsWith("\""))
				return arg.Substring(1, arg.Length - 2);
			return arg;
		}

		/// <summary>
		/// Executes the specified test
		/// </summary>
		/// <param name="pathToAssembly">The path to the compiled assembly containing the parser to test</param>
		/// <param name="input">A string with the input to be parsed</param>
		/// <param name="verb">A verb specifying the type of test, one of: matches, nomatches, fails</param>
		/// <param name="pathToExpected">The path to the XML file containing the expected parse tree, or null</param>
		public int Execute(string pathToAssembly, string input, string verb, string pathToExpected)
		{
			Hime.Redist.Parsers.IParser parser = GetParser(pathToAssembly, input);
			XmlDocument expected = null;
			if (pathToExpected != null)
			{
				expected = new XmlDocument();
				expected.LoadXml(System.IO.File.ReadAllText(pathToExpected, System.Text.Encoding.UTF8));
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

		private int TestMatches(Hime.Redist.Parsers.IParser parser, XmlDocument expected)
		{
			ParseResult result = parser.Parse();
			if (!result.IsSuccess)
				return RESULT_FAILURE_PARSING;
			if (result.Errors.Count != 0)
				return RESULT_FAILURE_PARSING;
			bool comparison = Compare((XmlElement)expected.ChildNodes[1], result.Root);
			return comparison ? RESULT_SUCCESS : RESULT_FAILURE_VERB;
		}

		private int TestNoMatches(Hime.Redist.Parsers.IParser parser, XmlDocument expected)
		{
			ParseResult result = parser.Parse();
			if (!result.IsSuccess)
				return RESULT_FAILURE_PARSING;
			if (result.Errors.Count != 0)
				return RESULT_FAILURE_PARSING;
			bool comparison = Compare((XmlElement)expected.ChildNodes[1], result.Root);
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
		/// Compare the specified AST node to the expected XML node
		/// </summary>
		/// <param name="expected">The expected node as a XML node</param>
		/// <param name="node">The AST node to compare</param>
		/// <returns><c>true</c> if the nodes match</returns>
		private bool Compare(XmlElement expected, ASTNode node)
		{
			if (node.Symbol.Name != expected.Name)
				return false;
			if (expected.HasAttribute("test"))
			{
				string test = expected.Attributes["test"].Value;
				string vRef = expected.Attributes["value"].Value;
				string vReal = node.Symbol.Value;
				if (test == VERB_MATCHES && vReal != vRef)
					return false;
				if (test == VERB_NOMATCHES && vReal == vRef)
					return false;
			}
			if (node.Children.Count != expected.ChildNodes.Count)
				return false;
			for (int i = 0; i != node.Children.Count; i++)
				if (!Compare((XmlElement)expected.ChildNodes.Item(i), node.Children[i]))
					return false;
			return true;
		}

		/// <summary>
		/// Gets the parser for the specified assembly and input
		/// </summary>
		/// <param name="pathToAssembly">Path to an assembly</param>
		/// <param name="input">An input for the parser</param>
		/// <returns>The parser</returns>
		private Hime.Redist.Parsers.IParser GetParser(string pathToAssembly, string input)
		{
			Assembly assembly = Assembly.LoadFile(pathToAssembly);
			Type parserType = null;
			Type baseParser = typeof(Hime.Redist.Parsers.IParser);
			foreach (Type t in assembly.GetTypes())
			{
				if (t.IsClass && baseParser.IsAssignableFrom(t))
				{
					parserType = t;
					break;
				}
			}
			ConstructorInfo parserCtor = parserType.GetConstructors()[0];
			ParameterInfo[] parameters = parserCtor.GetParameters();
			Type lexerType = parameters[0].ParameterType;
			ConstructorInfo lexerCtor = lexerType.GetConstructor(new Type[] { typeof(string) });
			object lexer = lexerCtor.Invoke(new object[] { input });
			return (Hime.Redist.Parsers.IParser)parserCtor.Invoke(new object[] { lexer });
		}
	}
}