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
using System.Xml;
using Hime.Redist;
using Hime.CentralDogma;
using Hime.CentralDogma.Output;

namespace Hime.Tests.Driver
{
	/// <summary>
	/// Represents a multi-platform test
	/// </summary>
	public class Test
	{
		public const string VERB_MATCHES = "matches";
		public const string VERB_NOMATCHES = "nomatches";
		public const string VERB_FAILS = "fails";
		public const string EXPECTED_PATH = "expected.xml";
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
		/// The expected tree as XML
		/// </summary>
		private XmlDocument expected;
		/// <summary>
		/// The results per platform
		/// </summary>
		private Dictionary<Runtime, TestResult> results;

		/// <summary>
		/// Gets the test's name
		/// </summary>
		public string Name { get { return node.Children[0].Symbol.Value; } }

		/// <summary>
		/// Initializes this test
		/// </summary>
		/// <param name="node">The test specification</param>
		/// <param name="originalInput">The original input for the test specification</param>
		public Test(ASTNode node, Text originalInput)
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
			this.results = new Dictionary<Runtime, TestResult>();
			if (node.Children.Count >= 5)
			{
				this.expected = new XmlDocument();
				this.expected.AppendChild(this.expected.CreateXmlDeclaration("1.0", "utf-8", null));
				this.expected.AppendChild(GetExpectedAsXML(node.Children[4]));
			}
		}

		/// <summary>
		/// Translates the specified AST node into the equivalent expected XML node
		/// </summary>
		/// <param name="node">An AST node</param>
		/// <returns>The expected XML node</returns>
		private XmlElement GetExpectedAsXML(ASTNode node)
		{
			XmlElement element = expected.CreateElement(node.Symbol.Value);
			if (node.Children[0].Children.Count != 0)
			{
				string test = node.Children[0].Children[0].Symbol.Value;
				string value = node.Children[0].Children[1].Symbol.Value;
				element.Attributes.Append(expected.CreateAttribute("test"));
				element.Attributes.Append(expected.CreateAttribute("value"));
				element.Attributes["test"].Value = (test == "=" ? VERB_MATCHES : VERB_NOMATCHES);
				element.Attributes["value"].Value = value;
			}
			foreach (ASTNode child in node.Children[1].Children)
				element.AppendChild(GetExpectedAsXML(child));
			return element;
		}

		/// <summary>
		/// Executes this test
		/// </summary>
		/// <param name="reporter">The reported to use</param>
		/// <param name="targets">The targets to execute on</param>
		public void Execute(Reporter reporter, List<Runtime> targets)
		{
			if (expected != null)
			{
				XmlWriter writer = new XmlTextWriter(EXPECTED_PATH, System.Text.Encoding.UTF8);
				expected.WriteTo(writer);
				writer.Close();
			}
			foreach (Runtime runtime in targets)
			{
				switch (runtime)
				{
					case Runtime.Net:
						this.results.Add(runtime, ExecuteOnNet(reporter));
						break;
					case Runtime.Java:
						this.results.Add(runtime, ExecuteOnJava(reporter));
						break;
				}
			}
		}

		/// <summary>
		/// Executes this test on the .Net runtime
		/// </summary>
		/// <param name="reporter">The reported to use</param>
		/// <returns>The test result</returns>
		private TestResult ExecuteOnNet(Reporter reporter)
		{
			TestResult result = new TestResult();
			List<string> output = new List<string>();
			int code = TestResult.RESULT_FAILURE_PARSING;
			try
			{
				string grammar = BuildParserForNet(reporter);
				code = ExecuteCommand(reporter, "mono", "executor.exe " + grammar + ".dll " + node.Children[3].Symbol.Value + " " + verb + " " + EXPECTED_PATH, output);
			}
			catch (Exception ex)
			{
				output.Add(ex.ToString());
			}
			result.Finish(code, output);
			return result;
		}

		/// <summary>
		/// Executes this test on the Java runtime
		/// </summary>
		/// <param name="reporter">The reported to use</param>
		/// <returns>The test result</returns>
		private TestResult ExecuteOnJava(Reporter reporter)
		{
			return null;
		}

		/// <summary>
		/// Builds the parser for the .Net runtime
		/// </summary>
		/// <param name="reporter">The reported to use</param>
		/// <returns>The name of the compiled grammar</returns>
		private string BuildParserForNet(Reporter reporter)
		{
			CompilationTask task = new CompilationTask(reporter);
			task.AddInput(node.Children[1], originalInput);
			task.CodeAccess = Modifier.Public;
			task.Method = (ParsingMethod)Enum.Parse(typeof(ParsingMethod), node.Children[2].Symbol.Value);
			task.Mode = Mode.Assembly;
			task.Namespace = "Hime.Tests.Generated";
			task.Execute();
			return node.Children[1].Children[0].Symbol.Value;
		}
		// <summary>
		/// Executes the specified command
		/// </summary>
		/// <param name="reporter">The reported to use</param>
		/// <param name="verb">The program to execute</param>
		/// <param name="arguments">The arguments</param>
		/// <param name="output">Storage for the console output lines</param>
		/// <returns>The command exit code</returns>
		private int ExecuteCommand(Reporter reporter, string command, string arguments, List<string> output)
		{
			reporter.Info("Executing command: " + command + " " + arguments);
			System.Diagnostics.Process process = new System.Diagnostics.Process();
			process.StartInfo.FileName = command;
			process.StartInfo.Arguments = arguments;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.UseShellExecute = false;
			process.Start();
			while (true)
			{
				string line = process.StandardOutput.ReadLine();
				if (line == null || line.Length == 0)
					break;
				output.Add(line);
				if (line.StartsWith("[ERROR]"))
					reporter.Error(line.Substring(8));
				else if (line.StartsWith("[WARNING]"))
					reporter.Warn(line.Substring(10));
				else if (line.StartsWith("[INFO]"))
					reporter.Info(line.Substring(7));
				else
					reporter.Info(line);
			}
			process.WaitForExit();
			return process.ExitCode;
		}

		/// <summary>
		/// Gets the XML report for this test
		/// </summary>
		/// <param name="doc">The parent XML document</param>
		/// <returns>The XML report</returns>
		public ReportData GetXMLReport(XmlDocument doc)
		{
			XmlElement root = doc.CreateElement("TestRecord");
			root.Attributes.Append(doc.CreateAttribute("Name"));
			root.Attributes["Name"].Value = Name;

			XmlElement nodeResults = doc.CreateElement("Results");
			XmlElement nodeTests = doc.CreateElement("Tests");
			root.AppendChild(nodeResults);
			root.AppendChild(nodeTests);
			ReportData aggregated = new ReportData();

			foreach (Runtime target in results.Keys)
			{
				ReportData data = GetXMLReport(doc, target);
				aggregated = aggregated + data;
				nodeTests.AppendChild(data.child);
			}

			nodeResults.AppendChild(aggregated.GetXML(doc));
			aggregated.child = root;
			return aggregated;
		}

		/// <summary>
		/// Gets the XML report for the specified target
		/// </summary>
		/// <param name="doc">The parent XML document</param>
		/// <param name="target">The target</param>
		/// <returns>The XML report</returns>
		public ReportData GetXMLReport(XmlDocument doc, Runtime target)
		{
			XmlElement nodeRecord = doc.CreateElement("TestRecord");
			XmlElement nodeResult = doc.CreateElement("Results");
			ReportData data = results[target].GetXML(doc);
			nodeResult.AppendChild(data.child);
			nodeRecord.AppendChild(nodeResult);
			nodeRecord.Attributes.Append(doc.CreateAttribute("Name"));
			nodeRecord.Attributes["Name"].Value = target.ToString();
			data.child = nodeRecord;
			return data;
		}
	}
}