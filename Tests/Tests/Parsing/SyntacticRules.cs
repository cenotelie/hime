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
		public void Test_Minimal_Empty_LR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"a\";} rules { a->; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "", "a");
		}

		/// <summary>
		/// Tests parsing of empty input
		/// </summary>
		[Test]
		public void Test_Minimal_Empty_GLR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"a\";} rules { a->; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.RNGLALR1, "", "a");
		}

		/// <summary>
		/// Tests parsing of minimal non-empty input
		/// </summary>
		[Test]
		public void Test_Minimal_Single_MatchOne_LR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a", "e(A='a')");
		}

		/// <summary>
		/// Tests parsing of minimal non-empty input
		/// </summary>
		[Test]
		public void Test_Minimal_Single_MatchOne_GLR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.RNGLALR1, "a", "e(A='a')");
		}

		/// <summary>
		/// Tests parsing fails on empty input when single element was expected
		/// </summary>
		[Test]
		public void Test_Minimal_Single_FailsEmpty_LR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A; } }";
			ParsingFails(grammar, "Test", ParsingMethod.LALR1, "");
		}

		/// <summary>
		/// Tests parsing fails on empty input when single element was expected
		/// </summary>
		[Test]
		public void Test_Minimal_Single_FailsEmpty_GLR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A; } }";
			ParsingFails(grammar, "Test", ParsingMethod.RNGLALR1, "");
		}

		/// <summary>
		/// Tests parsing fails on wrong input when single element was expected
		/// </summary>
		[Test]
		public void Test_Minimal_Single_FailsWrongInput_LR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A; } }";
			ParsingFails(grammar, "Test", ParsingMethod.LALR1, "b");
		}

		/// <summary>
		/// Tests parsing fails on wrong input when single element was expected
		/// </summary>
		[Test]
		public void Test_Minimal_Single_FailsWrongInput_GLR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A; } }";
			ParsingFails(grammar, "Test", ParsingMethod.RNGLALR1, "b");
		}

		/// <summary>
		/// Tests parsing with the optional operator on empty input
		/// </summary>
		[Test]
		public void Test_Grammar_OptionalOperator_MatchZero_LR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A?; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "", "e");
		}

		/// <summary>
		/// Tests parsing with the optional operator on empty input
		/// </summary>
		[Test]
		public void Test_Grammar_OptionalOperator_MatchZero_GLR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A?; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.RNGLALR1, "", "e");
		}

		/// <summary>
		/// Tests parsing with the optional operator with a single input
		/// </summary>
		[Test]
		public void Test_Grammar_OptionalOperator_MatchOne_LR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A?; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a", "e(A)");
		}

		/// <summary>
		/// Tests parsing with the optional operator with a single input
		/// </summary>
		[Test]
		public void Test_Grammar_OptionalOperator_MatchOne_GLR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A?; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.RNGLALR1, "a", "e(A)");
		}

		/// <summary>
		/// Tests parsing fails with the optional operator with more than one input
		/// </summary>
		[Test]
		public void Test_Grammar_OptionalOperator_FailsOnTwo_LR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A?; } }";
			ParsingFails(grammar, "Test", ParsingMethod.LALR1, "aa");
		}

		/// <summary>
		/// Tests parsing fails with the optional operator with more than one input
		/// </summary>
		[Test]
		public void Test_Grammar_OptionalOperator_FailsOnTwo_GLR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A?; } }";
			ParsingFails(grammar, "Test", ParsingMethod.RNGLALR1, "aa");
		}

		/// <summary>
		/// Tests parsing with the star operator on empty input
		/// </summary>
		[Test]
		public void Test_Grammar_StarOperator_MatchZero_LR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A*; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "", "e");
		}

		/// <summary>
		/// Tests parsing with the star operator on empty input
		/// </summary>
		[Test]
		public void Test_Grammar_StarOperator_MatchZero_GLR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A*; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.RNGLALR1, "", "e");
		}

		/// <summary>
		/// Tests parsing with the star operator with a single input
		/// </summary>
		[Test]
		public void Test_Grammar_StarOperator_MatchOne_LR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A*; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a", "e(A)");
		}

		/// <summary>
		/// Tests parsing with the star operator with a single input
		/// </summary>
		[Test]
		public void Test_Grammar_StarOperator_MatchOne_GLR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A*; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.RNGLALR1, "a", "e(A)");
		}

		/// <summary>
		/// Tests parsing with the star operator with more than one input
		/// </summary>
		[Test]
		public void Test_Grammar_StarOperator_MatchMoreThanOne_LR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A*; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "aaa", "e(A A A)");
		}

		/// <summary>
		/// Tests parsing with the star operator with more than one input
		/// </summary>
		[Test]
		public void Test_Grammar_StarOperator_MatchMoreThanOne_GLR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A*; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.RNGLALR1, "aaa", "e(A A A)");
		}

		/// <summary>
		/// Tests parsing fails with the plus operator on empty input
		/// </summary>
		[Test]
		public void Test_Grammar_PlusOperator_FailsOnZeo_LR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A+; } }";
			ParsingFails(grammar, "Test", ParsingMethod.LALR1, "");
		}

		/// <summary>
		/// Tests parsing fails with the plus operator on empty input
		/// </summary>
		[Test]
		public void Test_Grammar_PlusOperator_FailsOnZeo_RNGLR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A+; } }";
			ParsingFails(grammar, "Test", ParsingMethod.RNGLALR1, "");
		}

		/// <summary>
		/// Tests parsing with the plus operator with a single input
		/// </summary>
		[Test]
		public void Test_Grammar_PlusOperator_MatchOne_LR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A+; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a", "e(A)");
		}

		/// <summary>
		/// Tests parsing with the plus operator with a single input
		/// </summary>
		[Test]
		public void Test_Grammar_PlusOperator_MatchOne_GLR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A+; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.RNGLALR1, "a", "e(A)");
		}

		/// <summary>
		/// Tests parsing with the plus operator with more than one input
		/// </summary>
		[Test]
		public void Test_Grammar_PlusOperator_MatchMoreThanOne_LR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A+; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "aaa", "e(A A A)");
		}

		/// <summary>
		/// Tests parsing with the plus operator with more than one input
		/// </summary>
		[Test]
		public void Test_Grammar_PlusOperator_MatchMoreThanOne_GLR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A+; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.RNGLALR1, "aaa", "e(A A A)");
		}

		/// <summary>
		/// Tests parsing with the union operator, left side
		/// </summary>
		[Test]
		public void Test_Grammar_UnionOperator_Left_LR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a'; B->'b';} rules { e->A|B; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a", "e(A)");
		}

		/// <summary>
		/// Tests parsing with the union operator, left side
		/// </summary>
		[Test]
		public void Test_Grammar_UnionOperator_Left_GLR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a'; B->'b';} rules { e->A|B; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.RNGLALR1, "a", "e(A)");
		}

		/// <summary>
		/// Tests parsing with the union operator, right side
		/// </summary>
		[Test]
		public void Test_Grammar_UnionOperator_Right_LR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a'; B->'b';} rules { e->A|B; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "b", "e(B)");
		}

		/// <summary>
		/// Tests parsing with the union operator, right side
		/// </summary>
		[Test]
		public void Test_Grammar_UnionOperator_Right_GLR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a'; B->'b';} rules { e->A|B; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.RNGLALR1, "b", "e(B)");
		}

		/// <summary>
		/// Tests parsing with composed operators
		/// </summary>
		[Test]
		public void Test_Grammar_OperatorComposition0_LR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a'; B->'b';} rules { e->(A|B)*; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "", "e");
		}

		/// <summary>
		/// Tests parsing with composed operators
		/// </summary>
		[Test]
		public void Test_Grammar_OperatorComposition0_GLR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a'; B->'b';} rules { e->(A|B)*; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.RNGLALR1, "", "e");
		}

		/// <summary>
		/// Tests parsing with composed operators
		/// </summary>
		[Test]
		public void Test_Grammar_OperatorComposition1_LR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a'; B->'b';} rules { e->(A|B)*; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a", "e(A)");
		}

		/// <summary>
		/// Tests parsing with composed operators
		/// </summary>
		[Test]
		public void Test_Grammar_OperatorComposition1_GLR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a'; B->'b';} rules { e->(A|B)*; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.RNGLALR1, "a", "e(A)");
		}

		/// <summary>
		/// Tests parsing with composed operators
		/// </summary>
		[Test]
		public void Test_Grammar_OperatorComposition2_LR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a'; B->'b';} rules { e->(A|B)*; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "b", "e(B)");
		}

		/// <summary>
		/// Tests parsing with composed operators
		/// </summary>
		[Test]
		public void Test_Grammar_OperatorComposition2_GLR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a'; B->'b';} rules { e->(A|B)*; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.RNGLALR1, "b", "e(B)");
		}

		/// <summary>
		/// Tests parsing with composed operators
		/// </summary>
		[Test]
		public void Test_Grammar_OperatorComposition3_LR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a'; B->'b';} rules { e->(A|B)*; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "aba", "e(A B A)");
		}

		/// <summary>
		/// Tests parsing with composed operators
		/// </summary>
		[Test]
		public void Test_Grammar_OperatorComposition3_GLR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a'; B->'b';} rules { e->(A|B)*; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.RNGLALR1, "aba", "e(A B A)");
		}

		/// <summary>
		/// Tests parsing with composed operators
		/// </summary>
		[Test]
		public void Test_Grammar_OperatorComposition4_LR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a'; B->'b';} rules { e->(A|B)*; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "bab", "e(B A B)");
		}

		/// <summary>
		/// Tests parsing with composed operators
		/// </summary>
		[Test]
		public void Test_Grammar_OperatorComposition4_GLR()
		{
			SetTestDirectory();
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {A->'a'; B->'b';} rules { e->(A|B)*; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.RNGLALR1, "bab", "e(B A B)");
		}
	}
}