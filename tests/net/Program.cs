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
		/// The parser have the specified output
		/// </summary>
		private const string VERB_OUTPUTS = "outputs";

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
		/// <param name="verb">A verb specifying the type of test</param>
		/// <returns>The test result</returns>
		public int Execute(string parserName, string verb)
		{
			byte[] buffer = System.IO.File.ReadAllBytes("input.txt");
			string input = new string(System.Text.Encoding.UTF8.GetChars(buffer));
			Hime.Redist.Parsers.BaseLRParser parser = GetParser(parserName, input);
			switch (verb)
			{
				case VERB_MATCHES:
					return TestMatches(parser);
				case VERB_NOMATCHES:
					return TestNoMatches(parser);
				case VERB_FAILS:
					return TestFails(parser);
				case VERB_OUTPUTS:
					return TestOutputs(parser);
			}
			return RESULT_FAILURE_VERB;
		}

		/// <summary>
		/// Gets the serialized expected AST
		/// </summary>
		/// <returns>The expected AST, or null if an error occurred</returns>
		private static ASTNode? GetExpectedAST()
		{
			string expectedText = System.IO.File.ReadAllText("expected.txt", System.Text.Encoding.UTF8);
			Hime.Redist.Parsers.BaseLRParser expectedParser = GetParser("Hime.Tests.Generated.ExpectedTreeParser", expectedText);
			ParseResult result = expectedParser.Parse();
			foreach (ParseError error in result.Errors)
			{
				Console.WriteLine(error);
				TextContext context = result.Input.GetContext(error.Position);
				Console.WriteLine(context.Content);
				Console.WriteLine(context.Pointer);
			}
			return result.Errors.Count > 0 ? new ASTNode?() : (result.Root);
		}

		/// <summary>
		/// Gets the serialized expected output
		/// </summary>
		/// <returns>The expected output lines</returns>
		private static string[] GetExpectedOutput()
		{
			return System.IO.File.ReadAllLines("expected.txt", System.Text.Encoding.UTF8);
		}

		/// <summary>
		/// Executes the test as a parsing test with a matching condition
		/// </summary>
		/// <param name="parser">The parser to use</param>
		/// <returns>The test result</returns>
		private int TestMatches(Hime.Redist.Parsers.BaseLRParser parser)
		{
			ASTNode? expected = GetExpectedAST();
			if (!expected.HasValue)
			{
				Console.WriteLine("Failed to parse the expected AST");
				return RESULT_FAILURE_PARSING;
			}
			ParseResult result = parser.Parse();
			foreach (ParseError error in result.Errors)
			{
				Console.WriteLine(error);
				TextContext context = result.Input.GetContext(error.Position);
				Console.WriteLine(context.Content);
				Console.WriteLine(context.Pointer);
			}
			if (!result.IsSuccess)
			{
				Console.WriteLine("Failed to parse the input");
				return RESULT_FAILURE_PARSING;
			}
			if (result.Errors.Count != 0)
			{
				Console.WriteLine("Some errors while parsing the input");
				return RESULT_FAILURE_PARSING;
			}
			if (Compare(expected.Value, result.Root))
			{
				return RESULT_SUCCESS;
			}
			else
			{
				Console.WriteLine("Produced AST does not match the expected one");
				return RESULT_FAILURE_VERB;
			}
		}

		/// <summary>
		/// Executes the test as a parsing test with a non-matching condition
		/// </summary>
		/// <param name="parser">The parser to use</param>
		/// <returns>The test result</returns>
		private int TestNoMatches(Hime.Redist.Parsers.BaseLRParser parser)
		{
			ASTNode? expected = GetExpectedAST();
			if (!expected.HasValue)
			{
				Console.WriteLine("Failed to parse the expected AST");
				return RESULT_FAILURE_PARSING;
			}
			ParseResult result = parser.Parse();
			foreach (ParseError error in result.Errors)
			{
				Console.WriteLine(error);
				TextContext context = result.Input.GetContext(error.Position);
				Console.WriteLine(context.Content);
				Console.WriteLine(context.Pointer);
			}
			if (!result.IsSuccess)
			{
				Console.WriteLine("Failed to parse the input");
				return RESULT_FAILURE_PARSING;
			}
			if (result.Errors.Count != 0)
			{
				Console.WriteLine("Some errors while parsing the input");
				return RESULT_FAILURE_PARSING;
			}
			if (Compare(expected.Value, result.Root))
			{
				Console.WriteLine("Produced AST incorrectly matches the specified expectation");
				return RESULT_FAILURE_VERB;
			}
			else
			{
				return RESULT_SUCCESS;
			}
		}

		/// <summary>
		/// Executes the test as a parsing test with a failing condition
		/// </summary>
		/// <param name="parser">The parser to use</param>
		/// <returns>The test result</returns>
		private static int TestFails(Hime.Redist.Parsers.BaseLRParser parser)
		{
			ParseResult result = parser.Parse();
			if (!result.IsSuccess)
				return RESULT_SUCCESS;
			if (result.Errors.Count != 0)
				return RESULT_SUCCESS;
			Console.WriteLine("No error found while parsing, while some were expected");
			return RESULT_FAILURE_VERB;
		}

		/// <summary>
		/// Executes the test as an output test
		/// </summary>
		/// <param name="parser">The parser to use</param>
		/// <returns>The test result</returns>
		private static int TestOutputs(Hime.Redist.Parsers.BaseLRParser parser)
		{
			string[] output = GetExpectedOutput();
			ParseResult result = parser.Parse();
			if (output.Length == 0 || (output.Length == 1 && output[0].Length == 0))
			{
				if (result.IsSuccess && result.Errors.Count == 0)
					return RESULT_SUCCESS;
				foreach (ParseError error in result.Errors)
				{
					Console.WriteLine(error);
					TextContext context = result.Input.GetContext(error.Position);
					Console.WriteLine(context.Content);
					Console.WriteLine(context.Pointer);
				}
				Console.WriteLine("Expected an empty output but some error where found while parsing");
				return RESULT_FAILURE_VERB;
			}
			int i = 0;
			foreach (ParseError error in result.Errors)
			{
				string message = error.ToString();
				TextContext context = result.Input.GetContext(error.Position);
				if (i + 2 >= output.Length)
				{
					Console.WriteLine("Unexpected error:");
					Console.WriteLine(message);
					Console.WriteLine(context.Content);
					Console.WriteLine(context.Pointer);
					return RESULT_FAILURE_VERB;
				}
				if (!message.StartsWith(output[i]))
				{
					Console.WriteLine("Unexpected output: " + message);
					Console.WriteLine("Expected prefix  : " + output[i]);
					return RESULT_FAILURE_VERB;
				}
				if (!context.Content.StartsWith(output[i + 1]))
				{
					Console.WriteLine("Unexpected output: " + context.Content);
					Console.WriteLine("Expected prefix  : " + output[i + 1]);
					return RESULT_FAILURE_VERB;
				}
				if (!context.Pointer.StartsWith(output[i + 2]))
				{
					Console.WriteLine("Unexpected output: " + context.Pointer);
					Console.WriteLine("Expected prefix  : " + output[i + 2]);
					return RESULT_FAILURE_VERB;
				}
				i += 3;
			}
			if (i == output.Length)
				return RESULT_SUCCESS;
			for (int j = i; j != output.Length; j++)
				Console.WriteLine("Missing output: " + output[j]);
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
			if (node.Symbol.Name != expected.Value)
				return false;
			if (expected.Children[0].Children.Count > 0)
			{
				string test = expected.Children[0].Children[0].Value;
				string vRef = expected.Children[0].Children[1].Value;
				vRef = vRef.Substring(1, vRef.Length - 2).Replace("\\'", "'").Replace("\\\\", "\\");
				string vReal = node.Value;
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
		private static Hime.Redist.Parsers.BaseLRParser GetParser(string parserName, string input)
		{
			Assembly assembly = Assembly.LoadFile(System.IO.Path.GetFullPath("Parsers.dll"));
			Type parserType = assembly.GetType(parserName);
			ConstructorInfo parserCtor = parserType.GetConstructors()[0];
			ParameterInfo[] parameters = parserCtor.GetParameters();
			Type lexerType = parameters[0].ParameterType;
			ConstructorInfo lexerCtor = lexerType.GetConstructor(new [] { typeof(string) });
			object lexer = lexerCtor.Invoke(new object[] { input });
			return (Hime.Redist.Parsers.BaseLRParser)parserCtor.Invoke(new [] { lexer });
		}
	}
}