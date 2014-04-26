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
	/// Tests the features for terminal rules
	/// </summary>
	[TestFixture]
	public class TerminalRules : BaseParseSuite
	{
		/// <summary>
		/// Tests parsing of a single-character terminal
		/// </summary>
		[Test]
        public void Test_Terminals_SingleCharacter()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a';} rules { e->A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a", "e(A='a')");
        }
		
        /// <summary>
		/// Tests parsing of terminals with the escape sequence for backslash
		/// </summary>
		[Test]
        public void Test_Terminals_EscapeSequenceBackslash()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a'; X->'\\\\';} rules { e->A X A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a\\a", "e(A X='\\' A)");
        }
        
        /// <summary>
		/// Tests parsing of terminals with the escape sequence for single quote
		/// </summary>
		[Test]
        public void Test_Terminals_EscapeSequenceSingleQuote()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a'; X->'\\'';} rules { e->A X A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a'a", "e(A X A)");
        }
		
        /// <summary>
		/// Tests parsing of terminals with the escape sequence for Unicode character 0
		/// </summary>
		[Test]
        public void Test_Terminals_EscapeSequenceUnicode0()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a'; X->'\\0';} rules { e->A X A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a\u0000a", "e(A X='\u0000' A)");
        }
        
        /// <summary>
		/// Tests parsing of terminals with the escape sequence for Unicode character 7 (Alert)
		/// </summary>
		[Test]
        public void Test_Terminals_EscapeSequenceUnicode7()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a'; X->'\\a';} rules { e->A X A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a\u0007a", "e(A X='\u0007' A)");
        }
        
        /// <summary>
		/// Tests parsing of terminals with the escape sequence for Unicode character 8 (Backspace)
		/// </summary>
		[Test]
        public void Test_Terminals_EscapeSequenceUnicode8()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a'; X->'\\b';} rules { e->A X A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a\u0008a", "e(A X='\u0008' A)");
        }
        
        /// <summary>
		/// Tests parsing of terminals with the escape sequence for Unicode character 9 (Horizontal tab)
		/// </summary>
		[Test]
        public void Test_Terminals_EscapeSequenceUnicode9()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a'; X->'\\t';} rules { e->A X A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a\ta", "e(A X='\t' A)");
        }
        
        /// <summary>
		/// Tests parsing of terminals with the escape sequence for Unicode character 10 (New Line)
		/// </summary>
		[Test]
        public void Test_Terminals_EscapeSequenceUnicode10()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a'; X->'\\n';} rules { e->A X A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a\na", "e(A X='\n' A)");
        }
        
        /// <summary>
		/// Tests parsing of terminals with the escape sequence for Unicode character 11 (Vertical tab)
		/// </summary>
		[Test]
        public void Test_Terminals_EscapeSequenceUnicode11()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a'; X->'\\v';} rules { e->A X A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a\va", "e(A X='\v' A)");
        }
        
        /// <summary>
		/// Tests parsing of terminals with the escape sequence for Unicode character 12 (form feed)
		/// </summary>
		[Test]
        public void Test_Terminals_EscapeSequenceUnicode12()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a'; X->'\\f';} rules { e->A X A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a\fa", "e(A X='\f' A)");
        }
        
        /// <summary>
		/// Tests parsing of terminals with the escape sequence for Unicode character 13 (Carriage return)
		/// </summary>
		[Test]
        public void Test_Terminals_EscapeSequenceUnicode13()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a'; X->'\\r';} rules { e->A X A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a\ra", "e(A X='\r' A)");
        }
        
        /// <summary>
		/// Tests parsing of terminals with more than one character
		/// </summary>
		[Test]
        public void Test_Terminals_MultipleCharacters()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'abc';} rules { e->A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "abc", "e(A='abc')");
        }
        
        /// <summary>
		/// Tests parsing of terminals with unicode code point
		/// </summary>
		[Test]
        public void Test_Terminals_UnicodeCodepoint_1234()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a'; X->0x1234;} rules { e->A X A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a\u1234a", "e(A X='\u1234' A)");
        }
        
        /// <summary>
		/// Tests parsing of terminals with unicode code point
		/// </summary>
		[Test]
        public void Test_Terminals_UnicodeCodepoint_Range()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a'; X->0x1234 .. 0x5678;} rules { e->X X X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1234\u3333\u5678", "e(X='\u1234' X='\u3333' X='\u5678')");
        }
        
		/// <summary>
		/// Tests parsing of terminals with single wildcard
		/// </summary>
		[Test]
        public void Test_Terminals_Wildcard_Single()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->.; A->'a';} rules { e->A X A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "aba", "e(A X='b' A)");
        }
		
        /// <summary>
		/// Tests parsing of terminals with single wildcard
		/// </summary>
		[Test]
        public void Test_Terminals_Wildcard_SpecialCharacter_0()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->.; A->'a';} rules { e->A X A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a\u0000a", "e(A X='\u0000' A)");
        }
        
        /// <summary>
		/// Tests parsing of terminals with single wildcard
		/// </summary>
		[Test]
        public void Test_Terminals_Wildcard_SpecialCharacter_1234()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->.; A->'a';} rules { e->A X A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a\u1234a", "e(A X='\u1234' A)");
        }
        
        /// <summary>
		/// Tests parsing of terminals with single wildcard
		/// </summary>
		[Test]
        public void Test_Terminals_Wildcard_SpecialCharacter_5678()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->.; A->'a';} rules { e->A X A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a\u5678a", "e(A X='\u5678' A)");
        }
        
        /// <summary>
		/// Tests parsing of terminals with single wildcard
		/// </summary>
		[Test]
        public void Test_Terminals_Wildcard_NewLine()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->.; A->'a';} rules { e->A X A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a\na", "e(A X='\n' A)");
        }
        
        /// <summary>
		/// Tests parsing of terminals with single wildcard
		/// </summary>
		[Test]
        public void Test_Terminals_Wildcard_CarriageReturn()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->.; A->'a';} rules { e->A X A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a\ra", "e(A X='\r' A)");
        }
        
        /// <summary>
		/// Tests parsing of terminals with simple character class
		/// </summary>
		[Test]
        public void Test_Terminals_Class_SimpleCharacter()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->[xyz]; A->'a';} rules { e->A X A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "aya", "e(A X='y' A)");
        }
        
        /// <summary>
		/// Tests parsing of terminals with ranged charater class
		/// </summary>
		[Test]
        public void Test_Terminals_Class_Range()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->[x-z]; A->'a';} rules { e->A X A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "aya", "e(A X='y' A)");
        }
        
        /// <summary>
		/// Tests parsing of terminals with ranged charater class
		/// </summary>
		[Test]
        public void Test_Terminals_Class_RangeLeftBorder()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->[x-z]; A->'a';} rules { e->A X A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "axa", "e(A X='x' A)");
        }
        
        /// <summary>
		/// Tests parsing of terminals with ranged charater class
		/// </summary>
		[Test]
        public void Test_Terminals_Class_RangeRightBorder()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->[x-z]; A->'a';} rules { e->A X A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "aza", "e(A X='z' A)");
        }
        
        /// <summary>
		/// Tests parsing of terminals with negative ranged charater class
		/// </summary>
		[Test]
        public void Test_Terminals_Class_NegativeRange_Excluded()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->[^x-z]; A->'a';} rules { e->A X A; } }";
			ParsingFails(grammar, "Test", ParsingMethod.LALR1, "aya");
        }
        
        /// <summary>
		/// Tests parsing of terminals with negative ranged charater class
		/// </summary>
		[Test]
        public void Test_Terminals_Class_NegativeRange_ExcludedLeftBorder()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->[^x-z]; A->'a';} rules { e->A X A; } }";
			ParsingFails(grammar, "Test", ParsingMethod.LALR1, "axa");
        }
        
        /// <summary>
		/// Tests parsing of terminals with negative ranged charater class
		/// </summary>
		[Test]
        public void Test_Terminals_Class_NegativeRange_ExcludedRightBorder()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->[^x-z]; A->'a';} rules { e->A X A; } }";
			ParsingFails(grammar, "Test", ParsingMethod.LALR1, "aza");
        }
        
        /// <summary>
		/// Tests parsing of terminals with negative ranged charater class
		/// </summary>
		[Test]
        public void Test_Terminals_Class_NegativeRange_Included()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->[^x-z]; A->'a';} rules { e->A X A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "aba", "e(A X='b' A)");
        }
        
        /// <summary>
		/// Tests parsing of terminals with ranged charater class (multiple ranges)
		/// </summary>
		[Test]
        public void Test_Terminals_Class_MultiRange_First()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->[x-z0-9]; A->'a';} rules { e->A X A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "aya", "e(A X='y' A)");
        }
        
        /// <summary>
		/// Tests parsing of terminals with ranged charater class (multiple ranges)
		/// </summary>
		[Test]
        public void Test_Terminals_Class_MultiRange_Second()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->[x-z0-9]; A->'a';} rules { e->A X A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a5a", "e(A X='5' A)");
        }
        
        /// <summary>
		/// Tests parsing of terminals with negative ranged charater class (multiple ranges)
		/// </summary>
		[Test]
        public void Test_Terminals_Class_NegativeMultiRange_First()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->[^x-z0-9]; A->'a';} rules { e->A X A; } }";
			ParsingFails(grammar, "Test", ParsingMethod.LALR1, "aya");
        }
        
        /// <summary>
		/// Tests parsing of terminals with negative ranged charater class (multiple ranges)
		/// </summary>
		[Test]
        public void Test_Terminals_Class_NegativeMultiRange_Second()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->[^x-z0-9]; A->'a';} rules { e->A X A; } }";
			ParsingFails(grammar, "Test", ParsingMethod.LALR1, "a5a");
        }

		/// <summary>
		/// Tests the simple nesting of terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_NestingRules_Simple()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->'x'; Y->'y' X 'y'; A->'a';} rules { e->A Y A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "ayxya", "e(A Y='yxy' A)");
        }

		/// <summary>
		/// Tests the nesting of rule as a prefix
		/// </summary>
		[Test]
        public void Test_Terminals_NestingRules_Prefix_Strict()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->'x'; Y->X 'y'; A->'a';} rules { e->A Y A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "axya", "e(A Y='xy' A)");
        }

		/// <summary>
		/// Tests the nesting of rule as a prefix
		/// </summary>
		[Test]
        public void Test_Terminals_NestingRules_Prefix_Optional_1()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->'x'; Y->X 'y'?; A->'a';} rules { e->A Y A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "axya", "e(A Y='xy' A)");
        }

		/// <summary>
		/// Tests the nesting of rule as a prefix
		/// </summary>
		[Test]
        public void Test_Terminals_NestingRules_Prefix_Optional_2()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->'x'; Y->X 'y'?; A->'a';} rules { e->A Y A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "axa", "e(A Y='x' A)");
        }

		/// <summary>
		/// Tests the star operator in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_OperatorStar_0()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->'a' 'x'* 'a';} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "aa", "e(X='aa')");
        }

		/// <summary>
		/// Tests the star operator in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_OperatorStar_1()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->'a' 'x'* 'a';} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "axa", "e(X='axa')");
        }

		/// <summary>
		/// Tests the star operator in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_OperatorStar_More()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->'a' 'x'* 'a';} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "axxxa", "e(X='axxxa')");
        }

		/// <summary>
		/// Tests the plus operator in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_OperatorPlus_0()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->'a' 'x'+ 'a';} rules { e->X; } }";
			ParsingFails(grammar, "Test", ParsingMethod.LALR1, "aa");
        }

		/// <summary>
		/// Tests the plus operator in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_OperatorPlus_1()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->'a' 'x'+ 'a';} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "axa", "e(X='axa')");
        }

		/// <summary>
		/// Tests the plus operator in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_OperatorPlus_More()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->'a' 'x'+ 'a';} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "axxxa", "e(X='axxxa')");
        }

		/// <summary>
		/// Tests the optional operator in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_OperatorOptional_0()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->'a' 'x'? 'a';} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "aa", "e(X='aa')");
        }

		/// <summary>
		/// Tests the optional operator in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_OperatorOptional_1()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->'a' 'x'? 'a';} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "axa", "e(X='axa')");
        }

		/// <summary>
		/// Tests the optional operator in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_OperatorOptional_More()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->'a' 'x'? 'a';} rules { e->X; } }";
			ParsingFails(grammar, "Test", ParsingMethod.LALR1, "axxxa");
        }

		/// <summary>
		/// Tests the cardinality operator in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_OperatorCardinality_Strict_Exact()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->'a' 'x'{4} 'a';} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "axxxxa", "e(X='axxxxa')");
        }

		/// <summary>
		/// Tests the cardinality operator in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_OperatorCardinality_Strict_Less()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->'a' 'x'{4} 'a';} rules { e->X; } }";
			ParsingFails(grammar, "Test", ParsingMethod.LALR1, "axxxa");
        }

		/// <summary>
		/// Tests the cardinality operator in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_OperatorCardinality_Strict_More()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->'a' 'x'{4} 'a';} rules { e->X; } }";
			ParsingFails(grammar, "Test", ParsingMethod.LALR1, "axxxxxa");
        }

		/// <summary>
		/// Tests the cardinality operator in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_OperatorCardinality_Range_LeftBound()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->'a' 'x'{2,4} 'a';} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "axxa", "e(X='axxa')");
        }

		/// <summary>
		/// Tests the cardinality operator in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_OperatorCardinality_Range_RightBound()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->'a' 'x'{2,4} 'a';} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "axxxxa", "e(X='axxxxa')");
        }

		/// <summary>
		/// Tests the cardinality operator in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_OperatorCardinality_Range_Within()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->'a' 'x'{2,4} 'a';} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "axxxa", "e(X='axxxa')");
        }

		/// <summary>
		/// Tests the cardinality operator in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_OperatorCardinality_Range_Less()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->'a' 'x'{2,4} 'a';} rules { e->X; } }";
			ParsingFails(grammar, "Test", ParsingMethod.LALR1, "axa");
        }

		/// <summary>
		/// Tests the cardinality operator in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_OperatorCardinality_Range_More()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->'a' 'x'{2,4} 'a';} rules { e->X; } }";
			ParsingFails(grammar, "Test", ParsingMethod.LALR1, "axxxxxa");
        }

		/// <summary>
		/// Tests the concatenation operator in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_OperatorConcatenation_Simple()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A-> 'a' 'b';} rules { e->A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "ab", "e(A='ab')");
        }

		/// <summary>
		/// Tests the concatenation operator in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_OperatorConcatenation_Chained()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A-> 'a' 'b' 'c';} rules { e->A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "abc", "e(A='abc')");
        }

		/// <summary>
		/// Tests the union operator in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_OperatorUnion_Simple_Left()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A-> 'a' | 'b';} rules { e->A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a", "e(A='a')");
        }

		/// <summary>
		/// Tests the union operator in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_OperatorUnion_Simple_Right()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A-> 'a' | 'b';} rules { e->A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "b", "e(A='b')");
        }

		/// <summary>
		/// Tests the union operator in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_OperatorUnion_Chained_1()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A-> 'a' | 'b' | 'c';} rules { e->A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "a", "e(A='a')");
        }

		/// <summary>
		/// Tests the union operator in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_OperatorUnion_Chained_2()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A-> 'a' | 'b' | 'c';} rules { e->A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "b", "e(A='b')");
        }

		/// <summary>
		/// Tests the union operator in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_OperatorUnion_Chained_3()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A-> 'a' | 'b' | 'c';} rules { e->A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "c", "e(A='c')");
        }

		/// <summary>
		/// Tests the difference operator in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_OperatorDifference_InNominalOnly()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->[ab]+ - [bc]+;} rules { e->A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "aa", "e(A='aa')");
        }

		/// <summary>
		/// Tests the difference operator in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_OperatorDifference_InIntersection()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->[ab]+ - [bc]+;} rules { e->A; } }";
			ParsingFails(grammar, "Test", ParsingMethod.LALR1, "bb");
        }

		/// <summary>
		/// Tests the difference operator in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_OperatorDifference_InSubstractedOnly()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->[ab]+ - [bc]+;} rules { e->A; } }";
			ParsingFails(grammar, "Test", ParsingMethod.LALR1, "cc");
        }

		/// <summary>
		/// Tests the operator precedence in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_Precedence_StarConcat_NoGrouping_OK()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a' 'b'*;} rules { e->A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "abbbbb", "e(A='abbbbb')");
        }

		/// <summary>
		/// Tests the operator precedence in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_Precedence_StarConcat_NoGrouping_NOK()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a' 'b'*;} rules { e->A; } }";
			ParsingFails(grammar, "Test", ParsingMethod.LALR1, "abab");
        }

		/// <summary>
		/// Tests the operator precedence in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_Precedence_StarConcat_Grouping_OK()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->('a' 'b')*;} rules { e->A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "abab", "e(A='abab')");
        }

		/// <summary>
		/// Tests the operator precedence in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_Precedence_StarConcat_Grouping_NOK()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->('a' 'b')*;} rules { e->A; } }";
			ParsingFails(grammar, "Test", ParsingMethod.LALR1, "abbbbb");
        }

		/// <summary>
		/// Tests the operator precedence in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_Precedence_PlusConcat_NoGrouping_OK()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a' 'b'+;} rules { e->A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "abbbbb", "e(A='abbbbb')");
        }

		/// <summary>
		/// Tests the operator precedence in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_Precedence_PlusConcat_NoGrouping_NOK()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a' 'b'+;} rules { e->A; } }";
			ParsingFails(grammar, "Test", ParsingMethod.LALR1, "abab");
        }

		/// <summary>
		/// Tests the operator precedence in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_Precedence_PlusConcat_Grouping_OK()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->('a' 'b')+;} rules { e->A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "abab", "e(A='abab')");
        }

		/// <summary>
		/// Tests the operator precedence in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_Precedence_PlusConcat_Grouping_NOK()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->('a' 'b')+;} rules { e->A; } }";
			ParsingFails(grammar, "Test", ParsingMethod.LALR1, "abbbbb");
        }

		/// <summary>
		/// Tests the operator precedence in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_Precedence_OptionalConcat_NoGrouping_OK()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a' 'b'? 'c';} rules { e->A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "ac", "e(A='ac')");
        }

		/// <summary>
		/// Tests the operator precedence in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_Precedence_OptionalConcat_NoGrouping_NOK()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a' 'b'? 'c';} rules { e->A; } }";
			ParsingFails(grammar, "Test", ParsingMethod.LALR1, "c");
        }

		/// <summary>
		/// Tests the operator precedence in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_Precedence_OptionalConcat_Grouping_OK()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->('a' 'b')? 'c';} rules { e->A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "c", "e(A='c')");
        }

		/// <summary>
		/// Tests the operator precedence in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_Precedence_OptionalConcat_Grouping_NOK()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->('a' 'b')? 'c';} rules { e->A; } }";
			ParsingFails(grammar, "Test", ParsingMethod.LALR1, "ac");
        }

		/// <summary>
		/// Tests the operator precedence in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_Precedence_CardinalityConcat_NoGrouping_OK()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a' 'b'{4};} rules { e->A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "abbbb", "e(A='abbbb')");
        }

		/// <summary>
		/// Tests the operator precedence in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_Precedence_CardinalityConcat_NoGrouping_NOK()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->'a' 'b'{4};} rules { e->A; } }";
			ParsingFails(grammar, "Test", ParsingMethod.LALR1, "abababab");
        }

		/// <summary>
		/// Tests the operator precedence in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_Precedence_CardinalityConcat_Grouping_OK()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->('a' 'b'){4};} rules { e->A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "abababab", "e(A='abababab')");
        }

		/// <summary>
		/// Tests the operator precedence in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_Precedence_CardinalityConcat_Grouping_NOK()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A->('a' 'b'){4};} rules { e->A; } }";
			ParsingFails(grammar, "Test", ParsingMethod.LALR1, "abbbb");
        }

		/// <summary>
		/// Tests the operator precedence in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_Precedence_ConcatUnion_NoGrouping_OK()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A-> 'a' 'b' | 'c';} rules { e->A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "c", "e(A='c')");
        }

		/// <summary>
		/// Tests the operator precedence in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_Precedence_ConcatUnion_NoGrouping_NOK()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A-> 'a' 'b' | 'c';} rules { e->A; } }";
			ParsingFails(grammar, "Test", ParsingMethod.LALR1, "ac");
        }

		/// <summary>
		/// Tests the operator precedence in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_Precedence_ConcatUnion_Grouping_OK()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A-> 'a' ('b' | 'c');} rules { e->A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "ac", "e(A='ac')");
        }

		/// <summary>
		/// Tests the operator precedence in terminal rules
		/// </summary>
		[Test]
        public void Test_Terminals_Precedence_ConcatUnion_Grouping_NOK()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {A-> 'a' ('b' | 'c');} rules { e->A; } }";
			ParsingFails(grammar, "Test", ParsingMethod.LALR1, "c");
        }

		/// <summary>
		/// Tests the overriding of a terminal rule by another (more general)
		/// </summary>
		[Test]
        public void Test_Terminals_RuleOverriding()
        {
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->'x'; Y->[a-z]; A->'a';} rules { e->A Y A; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "axa", "e(A Y='x' A)");
        }
	}
}