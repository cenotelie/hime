/**********************************************************************
* Copyright (c) 2013 Laurent Wouters and others
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
using System.IO;
using System.Reflection;
using Hime.CentralDogma;
using Hime.Redist.AST;
using Hime.Redist.Lexer;
using Hime.Redist.Parsers;
using Hime.Redist.Symbols;
using NUnit.Framework;

namespace Hime.Tests
{
	/// <summary>
	/// Integration test suite for high-level parsing features
	/// </summary>
    [TestFixture]
    public class HimeIntegration : BaseParseSuite
    {
		/// <summary>
		/// Tests parsing of empty input
		/// </summary>
		[Test]
        public void Test_Minimal_Empty()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"a\";} rules { a->; } }";
			TestMatch(grammar, "Test", ParsingMethod.LALR1, "", "a");
        }

		/// <summary>
		/// Tests parsing of minimal non-empty input
		/// </summary>
		[Test]
        public void Test_Minimal_Single()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A; } }";
			TestMatch(grammar, "Test", ParsingMethod.LALR1, "a", "e(A='a')");
        }


		/// <summary>
		/// Tests parsing of simple math expression
		/// </summary>
        [Test]
        public void Test_SimpleExpression()
        {
			SetTestDirectory();
			string grammar = accessor.GetAllTextFor("MathExp.gram");
			TestMatch(grammar, "MathExp", ParsingMethod.LALR1, "3 + 5", "PLUS(NUMBER='3' NUMBER='5')");
        }
    }
}
