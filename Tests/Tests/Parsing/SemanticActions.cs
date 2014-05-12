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
using System.Collections.Generic;
using Hime.CentralDogma;
using NUnit.Framework;

namespace Hime.Tests.Parsing
{
	/// <summary>
	/// Tests the semantic actions features
	/// </summary>
	[TestFixture]
	public class SemanticActions : BaseParseSuite
	{
		/// <summary>
		/// Tests the correct sequence of semantic actions
		/// </summary>
		/// <param name="input">The input text</param>
		/// <param name="sequence">The expected sequence of semantic actions</param>
		private void Test_Input(string input, string[] sequence)
		{
			string grammar = GetResource("MathExp.gram");
			Hime.CentralDogma.SDK.AssemblyReflection assembly = Build(grammar, "MathExp", ParsingMethod.LALR1);
			Assert.IsNotNull(assembly, "Failed to compile the grammar");
			System.Type parserType = assembly.Parsers[0];
			Hime.CentralDogma.SDK.ActionTracer tracer = new Hime.CentralDogma.SDK.ActionTracer(parserType);
			Hime.Redist.Parsers.IParser parser = assembly.GetParser(parserType, input, tracer.Actions);
			Hime.Redist.ParseResult result = parser.Parse();
			Assert.IsTrue(result.IsSuccess, "Failed to parse the input");
			foreach (Hime.Redist.ParseError error in result.Errors)
				System.Console.WriteLine(error);
			Assert.IsTrue(result.Errors.Count == 0, "Failed to parse the input");
			Assert.AreEqual(sequence.Length, tracer.Trace.Count, "The traced sequence does not have the expected length");
			for (int i=0; i!=sequence.Length; i++)
				Assert.AreEqual(sequence[i], tracer.Trace[i], "The sequence diverges from the expected one at index " + i.ToString());
		}

		/// <summary>
		/// Tests the application of a simple promote action
		/// </summary>
		[Test]
		public void Test_Order()
		{
			SetTestDirectory();
			Test_Input("2 + 3", new string[] { "OnNumber", "OnNumber", "OnPlus" });
		}
	}
}