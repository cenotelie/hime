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
using Hime.Redist;
using Hime.SDK;
using Hime.SDK.Input;
using Hime.SDK.Output;

namespace Hime.Tests.Driver
{
	/// <summary>
	/// Represents a multi-platform parsing test
	/// </summary>
	public class OutputTest : Test
	{
		/// <summary>
		/// The test specification
		/// </summary>
		private ASTNode node;
		/// <summary>
		/// The original input for the test specification
		/// </summary>
		private Text originalInput;

		/// <summary>
		/// Gets the test's name
		/// </summary>
		public override string Name { get { return node.Children[0].Value; } }

		/// <summary>
		/// Initializes this test
		/// </summary>
		/// <param name="node">The test specification</param>
		/// <param name="originalInput">The original input for the test specification</param>
		public OutputTest(ASTNode node, Text originalInput)
		{
			this.node = node;
			this.originalInput = originalInput;
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
		/// <param name="targets">The targets to execute on</param>
		/// <param name="fixture">The parent fixture's name</param>
		public override void Execute(Reporter reporter, List<Runtime> targets, string fixture)
		{
			// Export input
			string inputValue = node.Children[3].Value;
			inputValue = Hime.SDK.Grammars.Loader.ReplaceEscapees(inputValue.Substring(1, inputValue.Length - 2));
			System.IO.File.WriteAllText("input.txt", inputValue, new System.Text.UTF8Encoding(false));
			// Export expected output
			List<string> expected = new List<string>();
			for (int i = 4; i != node.Children.Count; i++)
			{
				string temp = node.Children[i].Value;
				temp = Hime.SDK.Grammars.Loader.ReplaceEscapees(temp.Substring(1, temp.Length - 2));
				temp = temp.Replace("\\\"", "\"");
				expected.Add(temp);
			}
			System.IO.File.WriteAllLines("expected.txt", expected, new System.Text.UTF8Encoding(false));
			// Execute for each runtime
			foreach (Runtime runtime in targets)
			{
				switch (runtime)
				{
					case Runtime.Net:
						results.Add(runtime, ExecuteOnNet(reporter, fixture));
						break;
					case Runtime.Java:
						results.Add(runtime, ExecuteOnJava(reporter, fixture));
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
				args.Append("Parser outputs");
				code = IsOnWindows() ? ExecuteCommand(reporter, "executor.exe", args.ToString(), output) : ExecuteCommand(reporter, "mono", "executor.exe " + args, output);
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
				args.Append("Parser outputs");
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