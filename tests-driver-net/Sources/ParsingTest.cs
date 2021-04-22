/*******************************************************************************
 * Copyright (c) 2017 Association Cénotélie (cenotelie.fr)
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
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Hime.Redist;
using Hime.SDK;
using Hime.SDK.Grammars;
using Hime.SDK.Input;
using Hime.SDK.Output;

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
		public override string Name { get { return node.Children[0].Value; } }

		/// <summary>
		/// Initializes this test
		/// </summary>
		/// <param name="node">The test specification</param>
		/// <param name="originalInput">The original input for the test specification</param>
		public ParsingTest(ASTNode node, Text originalInput)
		{
			this.node = node;
			this.originalInput = originalInput;
			switch (node.Symbol.Name)
			{
				case "test_matches":
					verb = VERB_MATCHES;
					break;
				case "test_no_match":
					verb = VERB_NOMATCHES;
					break;
				case "test_fails":
					verb = VERB_FAILS;
					break;
			}
			if (node.Children.Count >= 5)
			{
				StringBuilder builder = new StringBuilder();
				BuildExpected(builder, node.Children[4]);
				expected = builder.ToString();
			}
		}

		/// <summary>
		/// Builds the string representation of the expected AST
		/// </summary>
		/// <param name="builder">A text builder</param>
		/// <param name="node">The current node to build</param>
		private void BuildExpected(StringBuilder builder, ASTNode node)
		{
			builder.Append(node.Value);
			if (node.Children[0].Children.Count > 0)
			{
				builder.Append(node.Children[0].Children[0].Value);
				builder.Append("'");
				string value = node.Children[0].Children[1].Value;
				// Decode the read value by replacing all the escape sequences
				value = Hime.SDK.Grammars.Loader.ReplaceEscapees(value.Substring(1, value.Length - 2)).Replace("\\'", "'");
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
			Hime.SDK.Input.Loader loader = new Hime.SDK.Input.Loader();
			loader.AddInput(node.Children[1], originalInput);
			return new Unit(
				loader.Load()[0],
				"",
				Mode.Assembly,
				(ParsingMethod)Enum.Parse(typeof(ParsingMethod), node.Children[2].Value),
				"Hime.Tests.Generated." + fixture,
				Modifier.Public);
		}

		/// <summary>
		/// Executes this test
		/// </summary>
		/// <param name="reporter">The reported to use</param>
		/// <param name="fixture">The parent fixture's name</param>
		public override void Execute(Reporter reporter, string fixture)
		{
			// Export input
			string inputValue = node.Children[3].Value;
			inputValue = Hime.SDK.Grammars.Loader.ReplaceEscapees(inputValue.Substring(1, inputValue.Length - 2));
			File.WriteAllText("input.txt", inputValue, new UTF8Encoding(false));
			// Export expected AST
			if (expected != null)
				File.WriteAllText("expected.txt", expected, new UTF8Encoding(false));
			// Execute for each runtime
			results.Add(Runtime.Net, ExecuteOnNet(reporter, fixture));
			results.Add(Runtime.Java, ExecuteOnJava(reporter, fixture));
			results.Add(Runtime.Rust, ExecuteOnRust(reporter, fixture));
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
				StringBuilder args = new StringBuilder("Hime.Tests.Generated.");
				args.Append(Helper.ToUpperCamelCase(fixture));
				args.Append(".");
				args.Append(Name);
				args.Append("Parser");
				// add verb argument
				args.Append(" ");
				args.Append(verb);
				code = ExecuteCommand(reporter, "mono", "executor-net.exe " + args, output);
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
				StringBuilder args = new StringBuilder("-jar executor-java.jar");
				// add parser name argument
				args.Append(" hime.tests.generated.");
				args.Append(Helper.ToSnakeCase(fixture));
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

		/// <summary>
		/// Executes this test on the Rust runtime
		/// </summary>
		/// <param name="reporter">The reported to use</param>
		/// <param name="fixture">The parent fixture's name</param>
		/// <returns>The test result</returns>
		private TestResult ExecuteOnRust(Reporter reporter, string fixture)
		{
			TestResult result = new TestResult();
			List<string> output = new List<string>();
			int code = TestResult.RESULT_FAILURE_PARSING;
			try
			{
				StringBuilder args = new StringBuilder(Helper.ToSnakeCase(Name));
				// add verb argument
				args.Append(" ");
				args.Append(verb);
				string command = "executor-rust";
				PlatformID platform = Environment.OSVersion.Platform;
				if (platform != PlatformID.Unix && platform != PlatformID.MacOSX)
					command += ".exe";
				code = ExecuteCommand(reporter, command, args.ToString(), output);
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
				default:
					reporter.Info("\t=> Error");
					break;
			}
			return result;
		}
	}
}