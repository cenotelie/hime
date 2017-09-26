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
using System.Text.RegularExpressions;
using System.Xml;
using Hime.SDK;
using Hime.SDK.Output;
using Hime.Redist;

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
		/// The targets
		/// </summary>
		private List<Runtime> targets;
		/// <summary>
		/// The tests
		/// </summary>
		private List<Fixture> fixtures;
		/// <summary>
		/// Filter for the enable tests
		/// </summary>
		private Regex filter;

		/// <summary>
		/// Execute this instance
		/// </summary>
		/// <param name="args">The command-line arguments.</param>
		public void Execute(string[] args)
		{
			// If no argument is given, print the help screen and return OK
			if (args == null || args.Length == 0)
			{
				PrintHelp();
				return;
			}

			// Parse the arguments
			ParseResult result = Hime.SDK.Input.CommandLine.ParseArguments(args);
			if (!result.IsSuccess || result.Errors.Count > 0)
			{
				Console.WriteLine("[ERROR] Could not parse the arguments");
				PrintHelp();
				return;
			}

			// Initializes the parameters
			reporter = new Reporter();
			targets = new List<Runtime>();
			fixtures = new List<Fixture>();
			filter = new Regex(".*");

			// Loads the arguments from the command line
			foreach (ASTNode arg in result.Root.Children[1].Children)
			{
				switch (arg.Value)
				{
					case "--targets":
						foreach (string name in Hime.SDK.Input.CommandLine.GetValues(arg))
							targets.Add((Runtime)Enum.Parse(typeof(Runtime), name));
						break;
					case "--filter":
						filter = new Regex(Hime.SDK.Input.CommandLine.GetValue(arg));
						break;
					default:
						Console.WriteLine("Unknown argument " + arg.Value);
						break;
				}
			}

			string[] resources = (typeof(Program)).Assembly.GetManifestResourceNames();
			foreach (string resource in resources)
				if (resource.StartsWith("Hime.Tests.Driver.Resources.Suites."))
					fixtures.Add(new Fixture(reporter, resource, filter));

			BuildTestParsers();

			// execute
			foreach (Fixture fixture in fixtures)
				fixture.Execute(reporter, targets);

			ExportReport();
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
			System.IO.Stream stream1 = typeof(Program).Assembly.GetManifestResourceStream("Hime.Tests.Driver.Resources.Fixture.gram");
			System.IO.Stream stream2 = typeof(CompilationTask).Assembly.GetManifestResourceStream("Hime.SDK.Sources.Input.HimeGrammar.gram");
			Hime.SDK.Input.Loader loader = new Hime.SDK.Input.Loader();
			loader.AddInputRaw(stream1);
			loader.AddInputRaw(stream2);
			List<Hime.SDK.Grammars.Grammar> grammars = loader.Load();
			foreach (Hime.SDK.Grammars.Grammar grammar in grammars)
			{
				if (grammar.Name == "ExpectedTree")
				{
					units.Add(new Unit(grammar, "", Mode.Assembly, ParsingMethod.LALR1, "Hime.Tests.Generated", Modifier.Public));
					break;
				}
			}

			foreach (Runtime target in targets)
			{
				EmitterBase emitter = null;
				switch (target)
				{
					case Runtime.Net:
						emitter = new EmitterForNet(reporter, units);
						break;
					case Runtime.Java:
						emitter = new EmitterForJava(reporter, units);
						break;
				}
				emitter.Emit();
			}
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
			root.Attributes["time"].Value = aggregated.spent.TotalSeconds.ToString(System.Globalization.CultureInfo.InvariantCulture);

			return aggregated;
		}

		/// <summary>
		/// Prints the help screen for this program
		/// </summary>
		private static void PrintHelp()
		{
			Console.WriteLine("driver " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " (LGPL 3)");
			Console.WriteLine("Tests driver for the multiplatform tests of the Hime parser generator");
			Console.WriteLine();
			Console.WriteLine("usage: mono driver.exe --targets <NAMES> [--filter REGEXP]");
			Console.WriteLine();
			Console.WriteLine("\t--targets <NAMES>");
			Console.WriteLine("\t\tExecute tests only for the provided platforms");
			Console.WriteLine("\t\tSupported platforms:");
			Console.Write("\t\t");
			bool first = true;
			foreach (object v in Enum.GetValues(typeof(Runtime)))
			{
				if (!first)
					Console.Write(", ");
				Console.Write(v);
				first = false;
			}
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine("\t--filter REGEXP");
			Console.WriteLine("\t\tOnly execute tests with name matching the provided expression");
			Console.WriteLine("\t\tBy default, execute all tests");
		}
	}
}