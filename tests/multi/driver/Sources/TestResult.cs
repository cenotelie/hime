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

namespace Hime.Tests.Driver
{
	/// <summary>
	/// Represents a test result
	/// </summary>
	public class TestResult
	{
		/// <summary>
		/// The test was successful
		/// </summary>
		public const int RESULT_SUCCESS = 0;
		/// <summary>
		/// The test failed in the end
		/// </summary>
		public const int RESULT_FAILURE_VERB = 1;
		/// <summary>
		/// The test failed in its parsing phase
		/// </summary>
		public const int RESULT_FAILURE_PARSING = 2;

		/// <summary>
		/// The execution timestamp
		/// </summary>
		private DateTime startTime;
		/// <summary>
		/// The total time spent on the test
		/// </summary>
		private TimeSpan spentTime;
		/// <summary>
		/// The result returned by the executor
		/// </summary>
		private int executorResult;
		/// <summary>
		/// The console output during the test
		/// </summary>
		private List<string> output;

		/// <summary>
		/// Starts the test
		/// </summary>
		public TestResult()
		{
			this.startTime = DateTime.Now;
		}

		/// <summary>
		/// Finishes this test
		/// </summary>
		/// <param name="result">The test result returned by the executor</param>
		/// <param name="output">The test console output</param>
		public void Finish(int result, List<string> output)
		{
			this.spentTime = (DateTime.Now - this.startTime);
			this.executorResult = result;
			this.output = output;
		}

		/// <summary>
		/// Gets the XML serialization of this result
		/// </summary>
		/// <param name="doc">The parent XML document</param>
		/// <returns>The element</returns>
		public ReportData GetXML(XmlDocument doc)
		{
			XmlElement root = doc.CreateElement("UnitTestResult");
			root.AppendChild(GetElement(doc, "TestDate", this.startTime.ToString()));
			root.AppendChild(GetElement(doc, "Status", null));
			root.AppendChild(GetElement(doc, "Passed", executorResult == RESULT_SUCCESS ? "1" : "0"));
			root.AppendChild(GetElement(doc, "Errors", executorResult == RESULT_FAILURE_PARSING ? "1" : "0"));
			root.AppendChild(GetElement(doc, "Failures", executorResult == RESULT_FAILURE_VERB ? "1" : "0"));
			root.AppendChild(GetElement(doc, "Inconclusive", null));
			root.AppendChild(GetElement(doc, "NotRunnable", null));
			root.AppendChild(GetElement(doc, "Skipped", null));
			root.AppendChild(GetElement(doc, "Ignored", null));
			root.AppendChild(GetElement(doc, "Time", this.spentTime.ToString()));
			root.AppendChild(GetElement(doc, "Message", null));
			System.Text.StringBuilder builder = new System.Text.StringBuilder();
			foreach (string line in this.output)
			{
				builder.Append(line);
				builder.Append(Environment.NewLine);
			}
			root.AppendChild(GetElement(doc, "ConsoleOutput", builder.ToString()));
			root.AppendChild(GetElement(doc, "ConsoleError", null));
			ReportData data = new ReportData();
			data.spent = this.spentTime;
			data.passed = executorResult == RESULT_SUCCESS ? 1 : 0;
			data.errors = executorResult == RESULT_FAILURE_PARSING ? 1 : 0;
			data.failed = executorResult == RESULT_FAILURE_VERB ? 1 : 0;
			data.child = root;
			return data;
		}

		/// <summary>
		/// Gets an XML element with a content
		/// </summary>
		/// <param name="doc">The parent XML document</param>
		/// <param name="name">The element's name</param>
		/// <param name="content">The element's content</param>
		/// <returns>The element</returns>
		public static XmlElement GetElement(XmlDocument doc, string name, string content)
		{
			XmlElement elem = doc.CreateElement(name);
			if (content != null)
				elem.AppendChild(doc.CreateTextNode(content));
			return elem;
		}
	}
}