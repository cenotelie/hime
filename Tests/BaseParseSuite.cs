/**********************************************************************
* Copyright (c) 2013 Laurent Wouters and others
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
using System.IO;
using System.Reflection;
using Hime.CentralDogma;
using Hime.CentralDogma.SDK;
using Hime.Redist;
using NUnit.Framework;

namespace Hime.Tests
{
	/// <summary>
	/// Represents a base test suite for integration parsing test
	/// </summary>
	public abstract class BaseParseSuite : BaseTestSuite
	{
		/// <summary>
		/// The global random source
		/// </summary>
		private static Random rand = new Random();

		/// <summary>
		/// Gets a unique prefix
		/// </summary>
		/// <returns>A unique prefix</returns>
		private static string GetUniquePrefix()
		{
			int i1 = rand.Next();
			int i2 = rand.Next();
			int i3 = rand.Next();
			int i4 = rand.Next();
			return i1.ToString("X8") + "_" + i2.ToString("X8") + "_" + i3.ToString("X8") + "_" + i4.ToString("X8");
		}

		/// <summary>
		/// The assembly for the parse tree parser
		/// </summary>
		private static AssemblyReflection parseTreeAssembly = BuildParseTreeParser();

		/// <summary>
		/// Builds the parse tree parser.
		/// </summary>
		/// <returns>The parse tree parser</returns>
		private static AssemblyReflection BuildParseTreeParser()
		{
			Stream stream = typeof(BaseParseSuite).Assembly.GetManifestResourceStream("Hime.Tests.Resources.ParseTree.gram");
			CompilationTask task = new CompilationTask();
			task.AddInputRaw(stream);
			task.GrammarName = "ParseTree";
			task.CodeAccess = Hime.CentralDogma.Output.Modifier.Public;
			task.Method = ParsingMethod.LALR1;
			task.Mode = Hime.CentralDogma.Output.Mode.Assembly;
			task.Namespace = "Hime.Tests.Generated";
			task.OutputPrefix = "ParseTree";
			task.Execute();
			return new AssemblyReflection("ParseTree.dll");
		}

		/// <summary>
		/// Parses the string representation of the given parse tree
		/// </summary>
		/// <param name="data">A string representation of a parse tree</param>
		/// <returns>The parse result</returns>
		private ParseResult ParseTree(string data)
		{
			Hime.Redist.Parsers.IParser parser = parseTreeAssembly.GetParser(data);
			return parser.Parse();
		}

		/// <summary>
		/// Compare two parse trees
		/// </summary>
		/// <param name="expected">The expected sub tree</param>
		/// <param name="node">The sub tree to compare</param>
		/// <returns>True if the two trees match</returns>
		private bool Compare(ASTNode expected, ASTNode node)
		{
			if (node.Symbol.Name != expected.Symbol.Value)
				return false;
			if (expected.Children[0].Children.Count != 0)
			{
				string vRef = expected.Children[0].Children[0].Symbol.Value;
				vRef = vRef.Substring(1, vRef.Length - 2);
				string vReal = node.Symbol.Value;
				if (vReal != vRef)
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
		/// Builds a parser
		/// </summary>
		/// <param name="grammars">Grammar content</param>
		/// <param name="top">The top grammar to compile</param>
		/// <param name="method">The parsing method to use</param>
		/// <returns>The parser</returns>
		protected AssemblyReflection Build(string grammars, string top, ParsingMethod method)
		{
			string prefix = GetUniquePrefix();
			string genNamespace = "Hime.Tests.Generated_" + prefix;

			CompilationTask task = new CompilationTask();
			task.AddInputRaw(grammars);
			task.GrammarName = top;
			task.CodeAccess = Hime.CentralDogma.Output.Modifier.Public;
			task.Method = method;
			task.Mode = Hime.CentralDogma.Output.Mode.Assembly;
			task.Namespace = genNamespace;
			task.OutputPrefix = prefix;
			Hime.CentralDogma.Report report = task.Execute();
			Assert.IsTrue(report.Errors.Count == 0, "Failed to compile the grammar");
			Assert.IsTrue(CheckFileExists(prefix + ".dll"), "Failed to produce the assembly");

			return new AssemblyReflection(Path.Combine(Environment.CurrentDirectory, prefix + ".dll"));
		}

		/// <summary>
		/// Builds a parser
		/// </summary>
		/// <param name="grammars">Grammar content</param>
		/// <param name="top">The top grammar to compile</param>
		/// <param name="method">The parsing method to use</param>
		/// <param name="input">The input text to parse</param>
		/// <returns>The parser</returns>
		protected Hime.Redist.Parsers.IParser GetParser(string grammars, string top, ParsingMethod method, string input)
		{
			AssemblyReflection assembly = Build(grammars, top, method);
			return assembly.GetParser(input);
		}

		/// <summary>
		/// Tests whether the given grammar parses the input as the expected AST
		/// </summary>
		/// <param name="grammars">Grammar content</param>
		/// <param name="top">The top grammar to compile</param>
		/// <param name="method">The parsing method to use</param>
		/// <param name="input">The input text to parse</param>
		/// <param name="expected">The expected AST</param>
		protected void ParsingMatches(string grammars, string top, ParsingMethod method, string input, string expected)
		{
			Hime.Redist.Parsers.IParser parser = GetParser(grammars, top, method, input);
			ParseResult inputResult = parser.Parse();
			foreach (ParseError error in inputResult.Errors)
				Console.WriteLine(error.ToString());
			Assert.IsTrue(inputResult.IsSuccess, "Failed to parse the input");
			Assert.AreEqual(0, inputResult.Errors.Count, "Failed to parse the input");
			ParseResult expectedResult = ParseTree(expected);
			Assert.IsTrue(expectedResult.IsSuccess, "Failed to parse the expected tree");
			Assert.AreEqual(0, expectedResult.Errors.Count, "Failed to parse the input");

			bool result = Compare(expectedResult.Root, inputResult.Root);
			Assert.IsTrue(result, "AST from input does not match the expected AST");
		}

		/// <summary>
		/// Tests whether the given grammar does not parse the input as the expected AST
		/// </summary>
		/// <param name="grammars">Grammar content</param>
		/// <param name="top">The top grammar to compile</param>
		/// <param name="method">The parsing method to use</param>
		/// <param name="input">The input text to parse</param>
		/// <param name="unexpected">The expected AST</param>
		protected void ParsingNotMatches(string grammars, string top, ParsingMethod method, string input, string unexpected)
		{
			Hime.Redist.Parsers.IParser parser = GetParser(grammars, top, method, input);
			ParseResult inputResult = parser.Parse();
			Assert.IsTrue(inputResult.IsSuccess, "Failed to parse the input");
			Assert.AreEqual(0, inputResult.Errors.Count, "Failed to parse the input");
			ParseResult unexpectedResult = ParseTree(unexpected);
			Assert.IsTrue(unexpectedResult.IsSuccess, "Failed to parse the expected tree");
			Assert.AreEqual(0, unexpectedResult.Errors.Count, "Failed to parse the input");

			bool result = Compare(unexpectedResult.Root, inputResult.Root);
			Assert.IsFalse(result, "AST from input matches the unexpected AST, should not");
		}

		/// <summary>
		/// Tests whether the given grammar fails to parse the input as the expected AST
		/// </summary>
		/// <param name="grammars">Grammar content</param>
		/// <param name="top">The top grammar to compile</param>
		/// <param name="method">The parsing method to use</param>
		/// <param name="input">The input text to parse</param>
		protected void ParsingFails(string grammars, string top, ParsingMethod method, string input)
		{
			Hime.Redist.Parsers.IParser parser = GetParser(grammars, top, method, input);
			ParseResult inputResult = parser.Parse();
			Assert.AreNotEqual(0, inputResult.Errors.Count, "Succeeded to parse the input, shouldn't");
		}
	}
}