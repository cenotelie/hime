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
using Hime.CentralDogma.SDK;
using Hime.Redist;
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
		/// <param name="method">The parsing method to use</param>
		private void Test_Sequence(string input, string[] sequence, ParsingMethod method)
		{
			string grammar = GetResource("MathExp.gram");
			AssemblyReflection assembly = Build(grammar, "MathExp", method);
			Assert.IsNotNull(assembly, "Failed to compile the grammar");
			System.Type parserType = assembly.Parsers[0];
			ActionTracer tracer = new ActionTracer(parserType);
			Hime.Redist.Parsers.IParser parser = assembly.GetParser(parserType, input, tracer.Actions);
			ParseResult result = parser.Parse();
			Assert.IsTrue(result.IsSuccess, "Failed to parse the input");
			foreach (ParseError error in result.Errors)
				System.Console.WriteLine(error);
			Assert.IsTrue(result.Errors.Count == 0, "Failed to parse the input");
			Assert.AreEqual(sequence.Length, tracer.Trace.Count, "The traced sequence does not have the expected length");
			for (int i=0; i!=sequence.Length; i++)
				Assert.AreEqual(sequence[i], tracer.Trace[i], "The sequence diverges from the expected one at index " + i.ToString());
		}

		/// <summary>
		/// Tests the correct content of semantic actions parameters
		/// </summary>
		/// <param name="input">The input text</param>
		/// <param name="content">The expected content for semantic actions</param>
		/// <param name="method">The parsing method to use</param>
		private void Test_Content(string input, string content, ParsingMethod method)
		{
			ParseResult expectedResult = ParseTree(content);
			Assert.IsTrue(expectedResult.IsSuccess, "Failed to parse the expected content");
			foreach (ParseError error in expectedResult.Errors)
				System.Console.WriteLine(error);
			Assert.IsTrue(expectedResult.Errors.Count == 0, "Failed to parse the expected content");

			string grammar = GetResource("MathExp.gram");
			AssemblyReflection assembly = Build(grammar, "MathExp", method);
			Assert.IsNotNull(assembly, "Failed to compile the grammar");
			System.Type parserType = assembly.Parsers[0];
			ActionTracer tracer = new ActionTracer(parserType);
			TestContent test = new TestContent(expectedResult.Root);
			Dictionary<string, SemanticAction> actions = new Dictionary<string, SemanticAction>();
			foreach (string key in tracer.Actions.Keys)
				actions.Add(key, new SemanticAction(test.Callback));

			Hime.Redist.Parsers.IParser parser = assembly.GetParser(parserType, input, actions);
			ParseResult result = parser.Parse();
			Assert.IsTrue(result.IsSuccess, "Failed to parse the input");
			foreach (ParseError error in result.Errors)
				System.Console.WriteLine(error);
			Assert.IsTrue(result.Errors.Count == 0, "Failed to parse the input");
		}

		private class TestContent
		{
			private Hime.Redist.ASTNode root;
			private int index;
			public TestContent(Hime.Redist.ASTNode expected)
			{
				this.root = expected;
				this.index = 0;
			}
			public void Callback(Hime.Redist.Symbol head, Hime.Redist.SemanticBody body)
			{
				Hime.Redist.ASTNode sub = root.Children[1].Children[index++];
				Hime.Redist.ASTFamily children = sub.Children[1].Children;
				Assert.AreEqual(sub.Symbol.Value, head.Name, "Unexpected head");
				Assert.AreEqual(children.Count, body.Length, "Unexpected body length");
				for (int i=0; i!=body.Length; i++)
					Assert.AreEqual(children[i].Symbol.Value, body[i].Name, "Unexpected symbol in body at index " + i.ToString());
			}
		}


		/// <summary>
		/// Tests for the correct order of semantic actions
		/// </summary>
		[Test]
		public void Test_Order_Simple_LR()
		{
			SetTestDirectory();
			Test_Sequence("2 + 3", new string[] { "OnNumber", "OnNumber", "OnPlus" }, ParsingMethod.LALR1);
		}

		/// <summary>
		/// Tests for the correct order of semantic actions
		/// </summary>
		[Test]
		public void Test_Order_Simple_GLR()
		{
			SetTestDirectory();
			Test_Sequence("2 + 3", new string[] { "OnNumber", "OnNumber", "OnPlus" }, ParsingMethod.RNGLALR1);
		}

		/// <summary>
		/// Tests for the correct order of semantic actions
		/// </summary>
		[Test]
		public void Test_Order_Complex1_LR()
		{
			SetTestDirectory();
			Test_Sequence("(2 + 3) * 4", new string[] { "OnNumber", "OnNumber", "OnPlus", "OnNumber", "OnMult" }, ParsingMethod.LALR1);
		}

		/// <summary>
		/// Tests for the correct order of semantic actions
		/// </summary>
		[Test]
		public void Test_Order_Complex1_GLR()
		{
			SetTestDirectory();
			Test_Sequence("(2 + 3) * 4", new string[] { "OnNumber", "OnNumber", "OnPlus", "OnNumber", "OnMult" }, ParsingMethod.RNGLALR1);
		}

		/// <summary>
		/// Tests for the correct order of semantic actions
		/// </summary>
		[Test]
		public void Test_Order_Complex2_LR()
		{
			SetTestDirectory();
			Test_Sequence("2 + 3 * 4", new string[] { "OnNumber", "OnNumber", "OnNumber", "OnMult", "OnPlus" }, ParsingMethod.LALR1);
		}

		/// <summary>
		/// Tests for the correct order of semantic actions
		/// </summary>
		[Test]
		public void Test_Order_Complex2_GLR()
		{
			SetTestDirectory();
			Test_Sequence("2 + 3 * 4", new string[] { "OnNumber", "OnNumber", "OnNumber", "OnMult", "OnPlus" }, ParsingMethod.RNGLALR1);
		}

		/// <summary>
		/// Tests for the correct content of semantic actions
		/// </summary>
		[Test]
		public void Test_Content_Simple_LR()
		{
			SetTestDirectory();
			Test_Content("2 + 3", "top( exp_atom(NUMBER) exp_atom(NUMBER) exp_op1(NUMBER PLUS NUMBER) )", ParsingMethod.LALR1);
		}

		/// <summary>
		/// Tests for the correct content of semantic actions
		/// </summary>
		[Test]
		public void Test_Content_Simple_GLR()
		{
			SetTestDirectory();
			Test_Content("2 + 3", "top( exp_atom(NUMBER) exp_atom(NUMBER) exp_op1(NUMBER PLUS NUMBER) )", ParsingMethod.RNGLALR1);
		}
	}
}