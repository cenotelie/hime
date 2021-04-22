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
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using Hime.Redist;
using Hime.SDK;
using Hime.SDK.Grammars;
using Hime.SDK.Input;
using Hime.SDK.Output;

namespace Hime.Tests.Driver
{
	/// <summary>
	/// The main program for the executor
	/// </summary>
	public class Program
	{
		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		/// <param name="args">The command-line arguments.</param>
		public static void Main(string[] args)
		{
			Program program = new Program();
			program.Execute(args);
		}

		/// <summary>
		/// The reporter
		/// </summary>
		private Reporter reporter;
		/// <summary>
		/// The tests
		/// </summary>
		private List<Fixture> fixtures;

		/// <summary>
		/// Execute this instance
		/// </summary>
		/// <param name="args">The command-line arguments.</param>
		public void Execute(string[] args)
		{
			// Initializes the parameters
			reporter = new Reporter();
			fixtures = new List<Fixture>();
			// Loads the fixtures
			string[] resources = (typeof(Program)).Assembly.GetManifestResourceNames();
			foreach (string resource in resources)
				if (resource.StartsWith("Hime.Tests.Driver.Resources.Suites."))
					fixtures.Add(new Fixture(reporter, resource));

			if (args.Length == 3 && args[0] == "--single")
			{
				// build a single test and execute
				foreach (Fixture fixture in fixtures)
				{
					if (fixture.Name != args[1])
						continue;
					foreach (Test test in fixture.Tests)
					{
						if (test.Name != args[2])
							continue;
						BuildTestParsers(args[1], args[2]);
						test.Execute(reporter, fixture.Name);
						return;
					}
				}
			}
			else if (args.Length == 1 && args[0] == "--all")
			{
				// Build all tests
				BuildTestParsers(null, null);
				// Execute the tests
				foreach (Fixture fixture in fixtures)
					fixture.Execute(reporter);
				// Export the test report
				ExportReport();
			}
		}

		/// <summary>
		/// Builds the test parsers
		/// </summary>
		private void BuildTestParsers(string fixtureName, string testName)
		{
			// get all units from the tests
			List<Unit> units = new List<Unit>();

			// add the unit for the expected tree parser
			Stream stream1 = typeof(Program).Assembly.GetManifestResourceStream("Hime.Tests.Driver.Resources.Fixture.gram");
			Stream stream2 = typeof(CompilationTask).Assembly.GetManifestResourceStream("Hime.SDK.Sources.Input.HimeGrammar.gram");
			Hime.SDK.Input.Loader loader = new Hime.SDK.Input.Loader();
			loader.AddInputRaw(stream1);
			loader.AddInputRaw(stream2);
			List<Grammar> grammars = loader.Load();
			foreach (Grammar grammar in grammars)
			{
				if (grammar.Name == "ExpectedTree")
				{
					units.Add(new Unit(grammar, "", Mode.Assembly, ParsingMethod.LALR1, "Hime.Tests.Generated", Modifier.Public));
					break;
				}
			}

			// add the unit for the parsers
			foreach (Fixture fixture in fixtures)
			{
				if (fixtureName != null && fixture.Name != fixtureName)
					continue;
				foreach (Test test in fixture.Tests)
				{
					if (testName != null && test.Name != testName)
						continue;
					units.Add(test.GetUnit(fixture.Name));
				}
			}

			// emit the artifacts
			(new EmitterForNet(reporter, units)).Emit();
			File.Move("Parsers.dll", "parsers-net.dll");
			(new EmitterForJava(reporter, units)).Emit();
			File.Move("Parsers.jar", "parsers-java.jar");
			EmitterForRust emitterForRust = new EmitterForRust(reporter, units, Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)), "runtime-rust"));
			emitterForRust.Emit();
			File.Move("Parsers.crate", "parsers-rust.crate");
			File.Move("Parsers" + emitterForRust.SuffixSystemAssembly, "parsers-rust" + emitterForRust.SuffixSystemAssembly);
		}

		/// <summary>
		/// Exports the tests report
		/// </summary>
		private void ExportReport()
		{
			XmlDocument doc = new XmlDocument();
			doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", null));
			ReportData aggregated = GetXMLReport(doc);
			doc.AppendChild(aggregated.child);

			// export document
			XmlTextWriter writer = new XmlTextWriter("TestResults.xml", Encoding.UTF8);
			writer.Formatting = Formatting.Indented;
			writer.Indentation = 1;
			writer.IndentChar = '\t';
			doc.WriteTo(writer);
			writer.Close();

			int total = (aggregated.passed + aggregated.errors + aggregated.failed);
			reporter.Info(string.Format("{0} tests executed, {1} failed, {2} errors", total, aggregated.failed, aggregated.errors));
		}

		/// <summary>
		/// Gets the XML report
		/// </summary>
		/// <returns>The XML report</returns>
		/// <param name="doc">The parent XML document</param>
		private ReportData GetXMLReport(XmlDocument doc)
		{
			XmlElement root = doc.CreateElement("testsuite");
			root.Attributes.Append(doc.CreateAttribute("name"));
			root.Attributes.Append(doc.CreateAttribute("timestamp"));
			root.Attributes.Append(doc.CreateAttribute("tests"));
			root.Attributes.Append(doc.CreateAttribute("failures"));
			root.Attributes.Append(doc.CreateAttribute("errors"));
			root.Attributes.Append(doc.CreateAttribute("time"));

			ReportData aggregated = new ReportData();
			foreach (Fixture fixture in fixtures)
			{
				ReportData data = fixture.GetXMLReport(doc);
				aggregated = aggregated + data;
				root.AppendChild(data.child);
			}
			aggregated.child = root;

			root.Attributes["name"].Value = "All tests";
			root.Attributes["tests"].Value = (aggregated.passed + aggregated.errors + aggregated.failed).ToString();
			root.Attributes["failures"].Value = aggregated.failed.ToString();
			root.Attributes["errors"].Value = aggregated.errors.ToString();
			root.Attributes["time"].Value = aggregated.spent.TotalSeconds.ToString(CultureInfo.InvariantCulture);

			return aggregated;
		}
	}
}