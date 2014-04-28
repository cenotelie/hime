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
	/// Tests the setting of grammar options
	/// </summary>
	[TestFixture]
	public class GrammarOptions : BaseParseSuite
	{
		/// <summary>
		/// Tests that the correct grammar variable is selected as the axiom of the grammar
		/// </summary>
		[Test]
		public void Test_Option_Axiom_CorrectVariable()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} rules { b->; e->; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "", "e");
		}

		/// <summary>
		/// Tests the Separator option with spaces
		/// </summary>
		[Test]
		public void Test_Option_Separator_Space()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\"; Separator=\"SEPARATOR\";} terminals {SEPARATOR->' '+; A->'a'; } rules { e->A+; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a a   a     a", "e(A A A A)");
		}

		/// <summary>
		/// Tests the Separator option with an arbitrary character
		/// </summary>
		[Test]
		public void Test_Option_Separator_ArbitraryCharacter()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\"; Separator=\"SEPARATOR\";} terminals {SEPARATOR->'x'+; A->'a'; } rules { e->A+; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "axaxxxaxxxxa", "e(A A A A)");
		}
	}
}