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
			// init
			reporter = new Reporter();
			targets = new List<Runtime>();
			targets.Add(Runtime.Net);
			targets.Add(Runtime.Java);

			// load fixtures
			fixtures = new List<Fixture>();
			fixtures.Add(new Fixture(reporter, "GrammarOptions"));

			// execute
			foreach (Fixture fixture in fixtures)
				fixture.Execute(reporter, targets);

			// create report
			XmlDocument doc = new XmlDocument();
			doc.AppendChild(doc.CreateXmlDeclaration("1.0", "utf-8", null));

			XmlNode current = doc;
			string[] nmspace = new string[] { null, "Hime", "Tests", "Runtimes" };
			for (int i=0; i!=nmspace.Length; i++)
			{
				XmlElement node1 = doc.CreateElement("TestRecord");
				XmlElement node2 = doc.CreateElement("Tests");
				string name = nmspace[i];
				if (name != null)
				{
					node1.Attributes.Append(doc.CreateAttribute("Name"));
					node1.Attributes["Name"].Value = name;
				}
				node1.AppendChild(node2);
				current.AppendChild(node1);
				current = node2;
			}

			foreach (Fixture fixture in fixtures)
				current.AppendChild(fixture.GetXMLReport(doc).child);

			// export document
			XmlTextWriter writer = new XmlTextWriter("TestResults.xml", System.Text.Encoding.UTF8);
			writer.Formatting = Formatting.Indented;
			writer.Indentation = 1;
			writer.IndentChar = '\t';
			doc.WriteTo(writer);
			writer.Close();
		}
	}
}