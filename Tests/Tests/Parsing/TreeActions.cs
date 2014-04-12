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
	/// Tests the tree actions features
	/// </summary>
	[TestFixture]
	public class TreeActions : BaseParseSuite
	{
		[Test]
        public void Test_Promote_Simple()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->x; x->A^; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a", "e(A)");
        }

		[Test]
        public void Test_Promote_ToRoot()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A^; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a", "A");
        }

		[Test]
        public void Test_Promote_MultipleInSameRule()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a'; B->'b'; C->'c';} rules { e->A^ B^ C; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "abc", "B(A C)");
        }

		[Test]
        public void Test_Promote_Chaining()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a'; B->'b'; C->'c';} rules { e->x^ A; x->y^ B; y->C^; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "cba", "C(B A)");
        }

		[Test]
        public void Test_Drop_Simple()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a'; B->'b'; C->'c';} rules { e->A B! C; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "abc", "e(A C)");
        }

		[Test]
        public void Test_Drop_SubTree()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a'; B->'b'; C->'c';} rules { e->A x! C; x->B; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "abc", "e(A C)");
        }
	}
}


