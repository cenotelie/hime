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
using Hime.CentralDogma;
using Hime.CentralDogma.Output;

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
			program.Execute();
		}

		/// <summary>
		/// The reporter
		/// </summary>
		private Reporter reporter;
		/// <summary>
		/// The targets
		/// </summary>
		private List<Runtime> targets;
		/// <summary>
		/// The tests
		/// </summary>
		private List<Fixture> fixtures;

		/// <summary>
		/// Execute this instance
		/// </summary>
		public void Execute()
		{
			Initialize();

			BuildTestParsers();

			// execute
			foreach (Fixture fixture in fixtures)
				fixture.Execute(reporter, targets);

			ExportReport();
		}

		/// <summary>
		/// Initializes this driver
		/// </summary>
		private void Initialize()
		{
			reporter = new Reporter();
			targets = new List<Runtime>();
			foreach (object v in Enum.GetValues(typeof(Runtime)))
				targets.Add((Runtime)v);

			// load fixtures
			fixtures = new List<Fixture>();
			string[] resources = (typeof(Program)).Assembly.GetManifestResourceNames();
			foreach (string resource in resources)
				if (resource.StartsWith("Hime.Tests.Driver.Resources.Suites."))
					fixtures.Add(new Fixture(reporter, resource));
		}

		/// <summary>
		/// Builds the test parsers
		/// </summary>
		private void BuildTestParsers()
		{
			// get all units from the tests
			List<Unit> units = new List<Unit>();
			foreach (Fixture fixture in fixtures)
				foreach (Test test in fixture.Tests)
					units.Add(test.GetUnit(fixture.Name));

			// build the unit for the expected trees
			System.IO.Stream stream1 = typeof(Program).Assembly.GetManifestResourceStream("Hime.Tests.Driver.Resources.ParsingFixture.gram");
			System.IO.Stream stream2 = typeof(CompilationTask).Assembly.GetManifestResourceStream("Hime.CentralDogma.Sources.Input.HimeGrammar.gram");
			Hime.CentralDogma.Input.Loader loader = new Hime.CentralDogma.Input.Loader();
			loader.AddInputRaw(stream1);
			loader.AddInputRaw(stream2);
			List<Hime.CentralDogma.Grammars.Grammar> grammars = loader.Load();
			foreach (Hime.CentralDogma.Grammars.Grammar grammar in grammars)
			{
				if (grammar.Name == "ExpectedTree")
				{
					units.Add(new Unit(grammar, ParsingMethod.LALR1, "Hime.Tests.Generated", Modifier.Public));
					break;
				}
			}

			EmitterForNet emitterNet = new EmitterForNet(units);
			emitterNet.Emit("", Mode.Assembly);
			EmitterForJava emitterJava = new EmitterForJava(units);
			emitterJava.Emit("", Mode.Assembly);
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
			XmlTextWriter writer = new XmlTextWriter("TestResults.xml", System.Text.Encoding.UTF8);
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
			root.Attributes["time"].Value = aggregated.spent.ToString();

			return aggregated;
		}
	}
}