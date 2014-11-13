﻿/**********************************************************************
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
using System.Xml;
using Hime.Redist;
using Hime.CentralDogma;
using Hime.CentralDogma.Input;
using Hime.CentralDogma.Output;

namespace Hime.Tests.Driver
{
	/// <summary>
	/// Represents a multi-platform parsing test
	/// </summary>
	public class ParsingTest : Test
	{
		/// <summary>
		/// The parser must produce an AST that matches the expected one
		/// </summary>
		public const string VERB_MATCHES = "matches";
		/// <summary>
		/// The parser must produce an AST that do NOT match the expected one
		/// </summary>
		public const string VERB_NOMATCHES = "nomatches";
		/// <summary>
		/// The parser must fail
		/// </summary>
		public const string VERB_FAILS = "fails";

		/// <summary>
		/// The test specification
		/// </summary>
		private ASTNode node;
		/// <summary>
		/// The original input for the test specification
		/// </summary>
		private Text originalInput;
		/// <summary>
		/// The verb for this test
		/// </summary>
		private string verb;
		/// <summary>
		/// The expected AST
		/// </summary>
		private string expected;

		/// <summary>
		/// Gets the test's name
		/// </summary>
		public override string Name { get { return node.Children[0].Symbol.Value; } }

		/// <summary>
		/// Initializes this test
		/// </summary>
		/// <param name="node">The test specification</param>
		/// <param name="originalInput">The original input for the test specification</param>
		public ParsingTest(ASTNode node, Text originalInput) : base()
		{
			this.node = node;
			this.originalInput = originalInput;
			switch (node.Symbol.Name)
			{
				case "test_matches":
					this.verb = VERB_MATCHES;
					break;
				case "test_no_match":
					this.verb = VERB_NOMATCHES;
					break;
				case "test_fails":
					this.verb = VERB_FAILS;
					break;
			}
			if (node.Children.Count >= 5)
			{
				System.Text.StringBuilder builder = new System.Text.StringBuilder();
				BuildExpected(builder, node.Children[4]);
				this.expected = builder.ToString();
			}
		}

		/// <summary>
		/// Builds the string representation of the expected AST
		/// </summary>
		/// <param name="builder">A text builder</param>
		/// <param name="node">The current node to build</param>
		private void BuildExpected(System.Text.StringBuilder builder, ASTNode node)
		{
			builder.Append(node.Symbol.Value);
			if (node.Children[0].Children.Count > 0)
			{
				builder.Append(node.Children[0].Children[0].Symbol.Value);
				builder.Append("'");
				string value = node.Children[0].Children[1].Symbol.Value;
				// Decode the read value by replacing all the escape sequences
				value = Hime.CentralDogma.Grammars.Loader.ReplaceEscapees(value.Substring(1, value.Length - 2)).Replace("\\'", "'");
				// Reset escape sequences for single quotes and backslashes
				value = value.Replace("\\", "\\\\").Replace("'", "\\'");
				builder.Append(value);
				builder.Append("'");
			}
			if (node.Children[1].Children.Count > 0)
			{
				builder.Append('(');
				foreach (ASTNode child in node.Children[1].Children)
				{
					builder.Append(' ');
					BuildExpected(builder, child);
				}
				builder.Append(')');
			}
		}

		/// <summary>
		/// Gets the compilation unit for this test
		/// </summary>
		/// <returns>The compilation unit</returns>
		public override Unit GetUnit(string fixture)
		{
			Loader loader = new Loader();
			loader.AddInput(node.Children[1], originalInput);
			return new Unit(
				loader.Load()[0],
				(ParsingMethod)Enum.Parse(typeof(ParsingMethod), node.Children[2].Symbol.Value),
				"Hime.Tests.Generated." + fixture,
				Modifier.Public);
		}

		/// <summary>
		/// Executes this test
		/// </summary>
		/// <param name="reporter">The reported to use</param>
		/// <param name="targets">The targets to execute on</param>
		/// <param name="fixture">The parent fixture's name</param>
		public override void Execute(Reporter reporter, List<Runtime> targets, string fixture)
		{
			// Export input
			string inputValue = node.Children[3].Symbol.Value;
			inputValue =  Hime.CentralDogma.Grammars.Loader.ReplaceEscapees(inputValue.Substring(1, inputValue.Length - 2));
			System.IO.File.WriteAllText("input.txt", inputValue, new System.Text.UTF8Encoding(false));
			// Export expected AST
			if (expected != null)
				System.IO.File.WriteAllText("expected.txt", expected, new System.Text.UTF8Encoding(false));
			// Execute for each runtime
			foreach (Runtime runtime in targets)
			{
				switch (runtime)
				{
					case Runtime.Net:
						this.results.Add(runtime, ExecuteOnNet(reporter, fixture));
						break;
					case Runtime.Java:
						this.results.Add(runtime, ExecuteOnJava(reporter, fixture));
						break;
				}
			}
		}

		/// <summary>
		/// Executes this test on the .Net runtime
		/// </summary>
		/// <param name="reporter">The reported to use</param>
		/// <param name="fixture">The parent fixture's name</param>
		/// <returns>The test result</returns>
		private TestResult ExecuteOnNet(Reporter reporter, string fixture)
		{
			TestResult result = new TestResult();
			List<string> output = new List<string>();
			int code = TestResult.RESULT_FAILURE_PARSING;
			try
			{
				System.Text.StringBuilder args = new System.Text.StringBuilder("Hime.Tests.Generated.");
				args.Append(fixture);
				args.Append(".");
				args.Append(Name);
				args.Append("Parser");
				// add verb argument
				args.Append(" ");
				args.Append(verb);
				if (IsOnWindows())
					code = ExecuteCommand(reporter, "executor.exe", args.ToString(), output);
				else
					code = ExecuteCommand(reporter, "mono", "executor.exe " + args.ToString(), output);
			}
			catch (Exception ex)
			{
				output.Add(ex.ToString());
			}
			result.Finish(code, output);
			switch (code)
			{
				case TestResult.RESULT_SUCCESS:
					reporter.Info("\t=> Success");
					break;
				case TestResult.RESULT_FAILURE_PARSING:
					reporter.Info("\t=> Error");
					break;
				case TestResult.RESULT_FAILURE_VERB:
					reporter.Info("\t=> Failure");
					break;
			}
			return result;
		}

		/// <summary>
		/// Executes this test on the Java runtime
		/// </summary>
		/// <param name="reporter">The reported to use</param>
		/// <param name="fixture">The parent fixture's name</param>
		/// <returns>The test result</returns>
		private TestResult ExecuteOnJava(Reporter reporter, string fixture)
		{
			TestResult result = new TestResult();
			List<string> output = new List<string>();
			int code = TestResult.RESULT_FAILURE_PARSING;
			try
			{
				System.Text.StringBuilder args = new System.Text.StringBuilder("-jar executor.jar");
				// add parser name argument
				args.Append(" Hime.Tests.Generated.");
				args.Append(fixture);
				args.Append(".");
				args.Append(Name);
				args.Append("Parser");
				// add verb argument
				args.Append(" ");
				args.Append(verb);
				code = ExecuteCommand(reporter, "java", args.ToString(), output);
			}
			catch (Exception ex)
			{
				output.Add(ex.ToString());
			}
			result.Finish(code, output);
			switch (code)
			{
				case TestResult.RESULT_SUCCESS:
					reporter.Info("\t=> Success");
					break;
				case TestResult.RESULT_FAILURE_PARSING:
					reporter.Info("\t=> Error");
					break;
				case TestResult.RESULT_FAILURE_VERB:
					reporter.Info("\t=> Failure");
					break;
			}
			return result;
		}
	}
}