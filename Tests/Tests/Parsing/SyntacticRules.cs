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
using Hime.CentralDogma;
using NUnit.Framework;

namespace Hime.Tests.Parsing
{
	/// <summary>
	/// Tests the features for syntactic rules
	/// </summary>
	[TestFixture]
	public class SyntacticRules : BaseParseSuite
	{
		/// <summary>
		/// Tests parsing of empty input
		/// </summary>
		[Test]
		public void Test_Minimal_Empty()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"a\";} rules { a->; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "", "a");
		}

		/// <summary>
		/// Tests parsing of minimal non-empty input
		/// </summary>
		[Test]
		public void Test_Minimal_Single_MatchOne()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a", "e(A='a')");
		}

		/// <summary>
		/// Tests parsing fails on empty input when single element was expected
		/// </summary>
		[Test]
		public void Test_Minimal_Single_FailsEmpty()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A; } }";
			ParsingFails(grammar, "Test", ParsingMethod.LALR1, "");
		}

		/// <summary>
		/// Tests parsing fails on wrong input when single element was expected
		/// </summary>
		[Test]
		public void Test_Minimal_Single_FailsWrongInput()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A; } }";
			ParsingFails(grammar, "Test", ParsingMethod.LALR1, "b");
		}

		/// <summary>
		/// Tests parsing with the optional operator on empty input
		/// </summary>
		[Test]
		public void Test_Grammar_OptionalOperator_MatchZero()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A?; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "", "e");
		}

		[Test]
		public void Test_Grammar_OptionalOperator_MatchOne()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A?; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a", "e(A)");
		}

		[Test]
		public void Test_Grammar_StarOperator_MatchZero()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A*; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "", "e");
		}

		[Test]
		public void Test_Grammar_StarOperator_MatchOne()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A*; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a", "e(A)");
		}

		[Test]
		public void Test_Grammar_StarOperator_MatchMoreThanOne()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A*; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "aaa", "e(A A A)");
		}

		[Test]
		public void Test_Grammar_PlusOperator_FailsOnZeo()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A+; } }";
			ParsingFails(grammar, "Test", ParsingMethod.LALR1, "");
		}

		[Test]
		public void Test_Grammar_PlusOperator_MatchOne()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A+; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a", "e(A)");
		}

		[Test]
		public void Test_Grammar_PlusOperator_MatchMoreThanOne()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A+; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "aaa", "e(A A A)");
		}

		[Test]
		public void Test_Grammar_UnionOperator_Left()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a'; B->'b';} rules { e->A|B; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a", "e(A)");
		}

		[Test]
		public void Test_Grammar_UnionOperator_Right()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a'; B->'b';} rules { e->A|B; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "b", "e(B)");
		}

		[Test]
		public void Test_Grammar_OperatorComposition0()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a'; B->'b';} rules { e->(A|B)*; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "", "e");
		}

		[Test]
		public void Test_Grammar_OperatorComposition1()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a'; B->'b';} rules { e->(A|B)*; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a", "e(A)");
		}

		[Test]
		public void Test_Grammar_OperatorComposition2()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a'; B->'b';} rules { e->(A|B)*; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "b", "e(B)");
		}

		[Test]
		public void Test_Grammar_OperatorComposition3()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a'; B->'b';} rules { e->(A|B)*; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "aba", "e(A B A)");
		}

		[Test]
		public void Test_Grammar_OperatorComposition4()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a'; B->'b';} rules { e->(A|B)*; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "bab", "e(B A B)");
		}

	}
}