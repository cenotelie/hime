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
	/// Tests the basic grammar features
	/// </summary>
	[TestFixture]
	public class UnicodeBlocks : BaseParseSuite
	{
		/// <summary>
		/// Tests the parsing of character block BasicLatin
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_BasicLatin_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{BasicLatin};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0000", "e(X='\u0000')");
		}

		/// <summary>
		/// Tests the parsing of character block BasicLatin
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_BasicLatin_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{BasicLatin};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u007F", "e(X='\u007F')");
		}

		/// <summary>
		/// Tests the parsing of character block Latin-1Supplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Latin_1Supplement_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Latin-1Supplement};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0080", "e(X='\u0080')");
		}

		/// <summary>
		/// Tests the parsing of character block Latin-1Supplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Latin_1Supplement_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Latin-1Supplement};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u00FF", "e(X='\u00FF')");
		}

		/// <summary>
		/// Tests the parsing of character block LatinExtended-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_LatinExtended_A_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{LatinExtended-A};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0100", "e(X='\u0100')");
		}

		/// <summary>
		/// Tests the parsing of character block LatinExtended-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_LatinExtended_A_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{LatinExtended-A};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u017F", "e(X='\u017F')");
		}

		/// <summary>
		/// Tests the parsing of character block LatinExtended-B
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_LatinExtended_B_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{LatinExtended-B};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0180", "e(X='\u0180')");
		}

		/// <summary>
		/// Tests the parsing of character block LatinExtended-B
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_LatinExtended_B_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{LatinExtended-B};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u024F", "e(X='\u024F')");
		}

		/// <summary>
		/// Tests the parsing of character block IPAExtensions
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_IPAExtensions_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{IPAExtensions};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0250", "e(X='\u0250')");
		}

		/// <summary>
		/// Tests the parsing of character block IPAExtensions
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_IPAExtensions_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{IPAExtensions};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u02AF", "e(X='\u02AF')");
		}

		/// <summary>
		/// Tests the parsing of character block SpacingModifierLetters
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SpacingModifierLetters_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{SpacingModifierLetters};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u02B0", "e(X='\u02B0')");
		}

		/// <summary>
		/// Tests the parsing of character block SpacingModifierLetters
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SpacingModifierLetters_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{SpacingModifierLetters};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u02FF", "e(X='\u02FF')");
		}

		/// <summary>
		/// Tests the parsing of character block CombiningDiacriticalMarks
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CombiningDiacriticalMarks_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CombiningDiacriticalMarks};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0300", "e(X='\u0300')");
		}

		/// <summary>
		/// Tests the parsing of character block CombiningDiacriticalMarks
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CombiningDiacriticalMarks_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CombiningDiacriticalMarks};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u036F", "e(X='\u036F')");
		}

		/// <summary>
		/// Tests the parsing of character block GreekandCoptic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_GreekandCoptic_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{GreekandCoptic};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0370", "e(X='\u0370')");
		}

		/// <summary>
		/// Tests the parsing of character block GreekandCoptic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_GreekandCoptic_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{GreekandCoptic};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u03FF", "e(X='\u03FF')");
		}

		/// <summary>
		/// Tests the parsing of character block Cyrillic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Cyrillic_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Cyrillic};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0400", "e(X='\u0400')");
		}

		/// <summary>
		/// Tests the parsing of character block Cyrillic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Cyrillic_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Cyrillic};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u04FF", "e(X='\u04FF')");
		}

		/// <summary>
		/// Tests the parsing of character block CyrillicSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CyrillicSupplement_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CyrillicSupplement};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0500", "e(X='\u0500')");
		}

		/// <summary>
		/// Tests the parsing of character block CyrillicSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CyrillicSupplement_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CyrillicSupplement};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u052F", "e(X='\u052F')");
		}

		/// <summary>
		/// Tests the parsing of character block Armenian
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Armenian_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Armenian};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0530", "e(X='\u0530')");
		}

		/// <summary>
		/// Tests the parsing of character block Armenian
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Armenian_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Armenian};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u058F", "e(X='\u058F')");
		}

		/// <summary>
		/// Tests the parsing of character block Hebrew
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Hebrew_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Hebrew};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0590", "e(X='\u0590')");
		}

		/// <summary>
		/// Tests the parsing of character block Hebrew
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Hebrew_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Hebrew};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u05FF", "e(X='\u05FF')");
		}

		/// <summary>
		/// Tests the parsing of character block Arabic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Arabic_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Arabic};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0600", "e(X='\u0600')");
		}

		/// <summary>
		/// Tests the parsing of character block Arabic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Arabic_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Arabic};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u06FF", "e(X='\u06FF')");
		}

		/// <summary>
		/// Tests the parsing of character block Syriac
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Syriac_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Syriac};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0700", "e(X='\u0700')");
		}

		/// <summary>
		/// Tests the parsing of character block Syriac
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Syriac_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Syriac};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u074F", "e(X='\u074F')");
		}

		/// <summary>
		/// Tests the parsing of character block ArabicSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_ArabicSupplement_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{ArabicSupplement};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0750", "e(X='\u0750')");
		}

		/// <summary>
		/// Tests the parsing of character block ArabicSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_ArabicSupplement_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{ArabicSupplement};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u077F", "e(X='\u077F')");
		}

		/// <summary>
		/// Tests the parsing of character block Thaana
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Thaana_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Thaana};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0780", "e(X='\u0780')");
		}

		/// <summary>
		/// Tests the parsing of character block Thaana
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Thaana_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Thaana};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u07BF", "e(X='\u07BF')");
		}

		/// <summary>
		/// Tests the parsing of character block NKo
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_NKo_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{NKo};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u07C0", "e(X='\u07C0')");
		}

		/// <summary>
		/// Tests the parsing of character block NKo
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_NKo_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{NKo};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u07FF", "e(X='\u07FF')");
		}

		/// <summary>
		/// Tests the parsing of character block Samaritan
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Samaritan_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Samaritan};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0800", "e(X='\u0800')");
		}

		/// <summary>
		/// Tests the parsing of character block Samaritan
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Samaritan_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Samaritan};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u083F", "e(X='\u083F')");
		}

		/// <summary>
		/// Tests the parsing of character block Mandaic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Mandaic_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Mandaic};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0840", "e(X='\u0840')");
		}

		/// <summary>
		/// Tests the parsing of character block Mandaic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Mandaic_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Mandaic};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u085F", "e(X='\u085F')");
		}

		/// <summary>
		/// Tests the parsing of character block ArabicExtended-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_ArabicExtended_A_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{ArabicExtended-A};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u08A0", "e(X='\u08A0')");
		}

		/// <summary>
		/// Tests the parsing of character block ArabicExtended-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_ArabicExtended_A_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{ArabicExtended-A};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u08FF", "e(X='\u08FF')");
		}

		/// <summary>
		/// Tests the parsing of character block Devanagari
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Devanagari_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Devanagari};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0900", "e(X='\u0900')");
		}

		/// <summary>
		/// Tests the parsing of character block Devanagari
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Devanagari_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Devanagari};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u097F", "e(X='\u097F')");
		}

		/// <summary>
		/// Tests the parsing of character block Bengali
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Bengali_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Bengali};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0980", "e(X='\u0980')");
		}

		/// <summary>
		/// Tests the parsing of character block Bengali
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Bengali_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Bengali};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u09FF", "e(X='\u09FF')");
		}

		/// <summary>
		/// Tests the parsing of character block Gurmukhi
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Gurmukhi_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Gurmukhi};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0A00", "e(X='\u0A00')");
		}

		/// <summary>
		/// Tests the parsing of character block Gurmukhi
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Gurmukhi_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Gurmukhi};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0A7F", "e(X='\u0A7F')");
		}

		/// <summary>
		/// Tests the parsing of character block Gujarati
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Gujarati_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Gujarati};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0A80", "e(X='\u0A80')");
		}

		/// <summary>
		/// Tests the parsing of character block Gujarati
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Gujarati_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Gujarati};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0AFF", "e(X='\u0AFF')");
		}

		/// <summary>
		/// Tests the parsing of character block Oriya
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Oriya_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Oriya};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0B00", "e(X='\u0B00')");
		}

		/// <summary>
		/// Tests the parsing of character block Oriya
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Oriya_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Oriya};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0B7F", "e(X='\u0B7F')");
		}

		/// <summary>
		/// Tests the parsing of character block Tamil
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Tamil_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Tamil};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0B80", "e(X='\u0B80')");
		}

		/// <summary>
		/// Tests the parsing of character block Tamil
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Tamil_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Tamil};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0BFF", "e(X='\u0BFF')");
		}

		/// <summary>
		/// Tests the parsing of character block Telugu
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Telugu_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Telugu};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0C00", "e(X='\u0C00')");
		}

		/// <summary>
		/// Tests the parsing of character block Telugu
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Telugu_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Telugu};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0C7F", "e(X='\u0C7F')");
		}

		/// <summary>
		/// Tests the parsing of character block Kannada
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Kannada_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Kannada};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0C80", "e(X='\u0C80')");
		}

		/// <summary>
		/// Tests the parsing of character block Kannada
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Kannada_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Kannada};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0CFF", "e(X='\u0CFF')");
		}

		/// <summary>
		/// Tests the parsing of character block Malayalam
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Malayalam_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Malayalam};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0D00", "e(X='\u0D00')");
		}

		/// <summary>
		/// Tests the parsing of character block Malayalam
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Malayalam_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Malayalam};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0D7F", "e(X='\u0D7F')");
		}

		/// <summary>
		/// Tests the parsing of character block Sinhala
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Sinhala_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Sinhala};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0D80", "e(X='\u0D80')");
		}

		/// <summary>
		/// Tests the parsing of character block Sinhala
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Sinhala_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Sinhala};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0DFF", "e(X='\u0DFF')");
		}

		/// <summary>
		/// Tests the parsing of character block Thai
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Thai_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Thai};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0E00", "e(X='\u0E00')");
		}

		/// <summary>
		/// Tests the parsing of character block Thai
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Thai_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Thai};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0E7F", "e(X='\u0E7F')");
		}

		/// <summary>
		/// Tests the parsing of character block Lao
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Lao_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Lao};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0E80", "e(X='\u0E80')");
		}

		/// <summary>
		/// Tests the parsing of character block Lao
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Lao_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Lao};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0EFF", "e(X='\u0EFF')");
		}

		/// <summary>
		/// Tests the parsing of character block Tibetan
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Tibetan_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Tibetan};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0F00", "e(X='\u0F00')");
		}

		/// <summary>
		/// Tests the parsing of character block Tibetan
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Tibetan_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Tibetan};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u0FFF", "e(X='\u0FFF')");
		}

		/// <summary>
		/// Tests the parsing of character block Myanmar
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Myanmar_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Myanmar};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1000", "e(X='\u1000')");
		}

		/// <summary>
		/// Tests the parsing of character block Myanmar
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Myanmar_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Myanmar};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u109F", "e(X='\u109F')");
		}

		/// <summary>
		/// Tests the parsing of character block Georgian
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Georgian_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Georgian};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u10A0", "e(X='\u10A0')");
		}

		/// <summary>
		/// Tests the parsing of character block Georgian
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Georgian_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Georgian};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u10FF", "e(X='\u10FF')");
		}

		/// <summary>
		/// Tests the parsing of character block HangulJamo
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_HangulJamo_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{HangulJamo};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1100", "e(X='\u1100')");
		}

		/// <summary>
		/// Tests the parsing of character block HangulJamo
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_HangulJamo_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{HangulJamo};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u11FF", "e(X='\u11FF')");
		}

		/// <summary>
		/// Tests the parsing of character block Ethiopic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Ethiopic_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Ethiopic};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1200", "e(X='\u1200')");
		}

		/// <summary>
		/// Tests the parsing of character block Ethiopic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Ethiopic_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Ethiopic};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u137F", "e(X='\u137F')");
		}

		/// <summary>
		/// Tests the parsing of character block EthiopicSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_EthiopicSupplement_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{EthiopicSupplement};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1380", "e(X='\u1380')");
		}

		/// <summary>
		/// Tests the parsing of character block EthiopicSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_EthiopicSupplement_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{EthiopicSupplement};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u139F", "e(X='\u139F')");
		}

		/// <summary>
		/// Tests the parsing of character block Cherokee
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Cherokee_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Cherokee};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u13A0", "e(X='\u13A0')");
		}

		/// <summary>
		/// Tests the parsing of character block Cherokee
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Cherokee_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Cherokee};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u13FF", "e(X='\u13FF')");
		}

		/// <summary>
		/// Tests the parsing of character block UnifiedCanadianAboriginalSyllabics
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_UnifiedCanadianAboriginalSyllabics_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{UnifiedCanadianAboriginalSyllabics};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1400", "e(X='\u1400')");
		}

		/// <summary>
		/// Tests the parsing of character block UnifiedCanadianAboriginalSyllabics
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_UnifiedCanadianAboriginalSyllabics_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{UnifiedCanadianAboriginalSyllabics};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u167F", "e(X='\u167F')");
		}

		/// <summary>
		/// Tests the parsing of character block Ogham
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Ogham_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Ogham};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1680", "e(X='\u1680')");
		}

		/// <summary>
		/// Tests the parsing of character block Ogham
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Ogham_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Ogham};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u169F", "e(X='\u169F')");
		}

		/// <summary>
		/// Tests the parsing of character block Runic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Runic_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Runic};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u16A0", "e(X='\u16A0')");
		}

		/// <summary>
		/// Tests the parsing of character block Runic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Runic_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Runic};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u16FF", "e(X='\u16FF')");
		}

		/// <summary>
		/// Tests the parsing of character block Tagalog
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Tagalog_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Tagalog};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1700", "e(X='\u1700')");
		}

		/// <summary>
		/// Tests the parsing of character block Tagalog
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Tagalog_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Tagalog};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u171F", "e(X='\u171F')");
		}

		/// <summary>
		/// Tests the parsing of character block Hanunoo
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Hanunoo_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Hanunoo};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1720", "e(X='\u1720')");
		}

		/// <summary>
		/// Tests the parsing of character block Hanunoo
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Hanunoo_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Hanunoo};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u173F", "e(X='\u173F')");
		}

		/// <summary>
		/// Tests the parsing of character block Buhid
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Buhid_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Buhid};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1740", "e(X='\u1740')");
		}

		/// <summary>
		/// Tests the parsing of character block Buhid
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Buhid_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Buhid};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u175F", "e(X='\u175F')");
		}

		/// <summary>
		/// Tests the parsing of character block Tagbanwa
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Tagbanwa_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Tagbanwa};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1760", "e(X='\u1760')");
		}

		/// <summary>
		/// Tests the parsing of character block Tagbanwa
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Tagbanwa_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Tagbanwa};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u177F", "e(X='\u177F')");
		}

		/// <summary>
		/// Tests the parsing of character block Khmer
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Khmer_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Khmer};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1780", "e(X='\u1780')");
		}

		/// <summary>
		/// Tests the parsing of character block Khmer
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Khmer_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Khmer};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u17FF", "e(X='\u17FF')");
		}

		/// <summary>
		/// Tests the parsing of character block Mongolian
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Mongolian_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Mongolian};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1800", "e(X='\u1800')");
		}

		/// <summary>
		/// Tests the parsing of character block Mongolian
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Mongolian_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Mongolian};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u18AF", "e(X='\u18AF')");
		}

		/// <summary>
		/// Tests the parsing of character block UnifiedCanadianAboriginalSyllabicsExtended
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_UnifiedCanadianAboriginalSyllabicsExtended_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{UnifiedCanadianAboriginalSyllabicsExtended};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u18B0", "e(X='\u18B0')");
		}

		/// <summary>
		/// Tests the parsing of character block UnifiedCanadianAboriginalSyllabicsExtended
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_UnifiedCanadianAboriginalSyllabicsExtended_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{UnifiedCanadianAboriginalSyllabicsExtended};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u18FF", "e(X='\u18FF')");
		}

		/// <summary>
		/// Tests the parsing of character block Limbu
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Limbu_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Limbu};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1900", "e(X='\u1900')");
		}

		/// <summary>
		/// Tests the parsing of character block Limbu
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Limbu_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Limbu};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u194F", "e(X='\u194F')");
		}

		/// <summary>
		/// Tests the parsing of character block TaiLe
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_TaiLe_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{TaiLe};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1950", "e(X='\u1950')");
		}

		/// <summary>
		/// Tests the parsing of character block TaiLe
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_TaiLe_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{TaiLe};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u197F", "e(X='\u197F')");
		}

		/// <summary>
		/// Tests the parsing of character block NewTaiLue
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_NewTaiLue_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{NewTaiLue};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1980", "e(X='\u1980')");
		}

		/// <summary>
		/// Tests the parsing of character block NewTaiLue
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_NewTaiLue_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{NewTaiLue};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u19DF", "e(X='\u19DF')");
		}

		/// <summary>
		/// Tests the parsing of character block KhmerSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_KhmerSymbols_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{KhmerSymbols};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u19E0", "e(X='\u19E0')");
		}

		/// <summary>
		/// Tests the parsing of character block KhmerSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_KhmerSymbols_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{KhmerSymbols};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u19FF", "e(X='\u19FF')");
		}

		/// <summary>
		/// Tests the parsing of character block Buginese
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Buginese_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Buginese};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1A00", "e(X='\u1A00')");
		}

		/// <summary>
		/// Tests the parsing of character block Buginese
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Buginese_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Buginese};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1A1F", "e(X='\u1A1F')");
		}

		/// <summary>
		/// Tests the parsing of character block TaiTham
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_TaiTham_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{TaiTham};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1A20", "e(X='\u1A20')");
		}

		/// <summary>
		/// Tests the parsing of character block TaiTham
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_TaiTham_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{TaiTham};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1AAF", "e(X='\u1AAF')");
		}

		/// <summary>
		/// Tests the parsing of character block Balinese
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Balinese_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Balinese};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1B00", "e(X='\u1B00')");
		}

		/// <summary>
		/// Tests the parsing of character block Balinese
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Balinese_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Balinese};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1B7F", "e(X='\u1B7F')");
		}

		/// <summary>
		/// Tests the parsing of character block Sundanese
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Sundanese_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Sundanese};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1B80", "e(X='\u1B80')");
		}

		/// <summary>
		/// Tests the parsing of character block Sundanese
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Sundanese_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Sundanese};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1BBF", "e(X='\u1BBF')");
		}

		/// <summary>
		/// Tests the parsing of character block Batak
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Batak_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Batak};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1BC0", "e(X='\u1BC0')");
		}

		/// <summary>
		/// Tests the parsing of character block Batak
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Batak_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Batak};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1BFF", "e(X='\u1BFF')");
		}

		/// <summary>
		/// Tests the parsing of character block Lepcha
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Lepcha_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Lepcha};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1C00", "e(X='\u1C00')");
		}

		/// <summary>
		/// Tests the parsing of character block Lepcha
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Lepcha_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Lepcha};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1C4F", "e(X='\u1C4F')");
		}

		/// <summary>
		/// Tests the parsing of character block OlChiki
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_OlChiki_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{OlChiki};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1C50", "e(X='\u1C50')");
		}

		/// <summary>
		/// Tests the parsing of character block OlChiki
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_OlChiki_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{OlChiki};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1C7F", "e(X='\u1C7F')");
		}

		/// <summary>
		/// Tests the parsing of character block SundaneseSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SundaneseSupplement_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{SundaneseSupplement};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1CC0", "e(X='\u1CC0')");
		}

		/// <summary>
		/// Tests the parsing of character block SundaneseSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SundaneseSupplement_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{SundaneseSupplement};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1CCF", "e(X='\u1CCF')");
		}

		/// <summary>
		/// Tests the parsing of character block VedicExtensions
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_VedicExtensions_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{VedicExtensions};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1CD0", "e(X='\u1CD0')");
		}

		/// <summary>
		/// Tests the parsing of character block VedicExtensions
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_VedicExtensions_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{VedicExtensions};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1CFF", "e(X='\u1CFF')");
		}

		/// <summary>
		/// Tests the parsing of character block PhoneticExtensions
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_PhoneticExtensions_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{PhoneticExtensions};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1D00", "e(X='\u1D00')");
		}

		/// <summary>
		/// Tests the parsing of character block PhoneticExtensions
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_PhoneticExtensions_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{PhoneticExtensions};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1D7F", "e(X='\u1D7F')");
		}

		/// <summary>
		/// Tests the parsing of character block PhoneticExtensionsSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_PhoneticExtensionsSupplement_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{PhoneticExtensionsSupplement};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1D80", "e(X='\u1D80')");
		}

		/// <summary>
		/// Tests the parsing of character block PhoneticExtensionsSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_PhoneticExtensionsSupplement_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{PhoneticExtensionsSupplement};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1DBF", "e(X='\u1DBF')");
		}

		/// <summary>
		/// Tests the parsing of character block CombiningDiacriticalMarksSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CombiningDiacriticalMarksSupplement_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CombiningDiacriticalMarksSupplement};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1DC0", "e(X='\u1DC0')");
		}

		/// <summary>
		/// Tests the parsing of character block CombiningDiacriticalMarksSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CombiningDiacriticalMarksSupplement_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CombiningDiacriticalMarksSupplement};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1DFF", "e(X='\u1DFF')");
		}

		/// <summary>
		/// Tests the parsing of character block LatinExtendedAdditional
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_LatinExtendedAdditional_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{LatinExtendedAdditional};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1E00", "e(X='\u1E00')");
		}

		/// <summary>
		/// Tests the parsing of character block LatinExtendedAdditional
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_LatinExtendedAdditional_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{LatinExtendedAdditional};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1EFF", "e(X='\u1EFF')");
		}

		/// <summary>
		/// Tests the parsing of character block GreekExtended
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_GreekExtended_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{GreekExtended};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1F00", "e(X='\u1F00')");
		}

		/// <summary>
		/// Tests the parsing of character block GreekExtended
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_GreekExtended_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{GreekExtended};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u1FFF", "e(X='\u1FFF')");
		}

		/// <summary>
		/// Tests the parsing of character block GeneralPunctuation
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_GeneralPunctuation_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{GeneralPunctuation};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2000", "e(X='\u2000')");
		}

		/// <summary>
		/// Tests the parsing of character block GeneralPunctuation
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_GeneralPunctuation_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{GeneralPunctuation};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u206F", "e(X='\u206F')");
		}

		/// <summary>
		/// Tests the parsing of character block SuperscriptsAndSubscripts
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SuperscriptsAndSubscripts_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{SuperscriptsAndSubscripts};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2070", "e(X='\u2070')");
		}

		/// <summary>
		/// Tests the parsing of character block SuperscriptsAndSubscripts
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SuperscriptsAndSubscripts_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{SuperscriptsAndSubscripts};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u209F", "e(X='\u209F')");
		}

		/// <summary>
		/// Tests the parsing of character block CurrencySymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CurrencySymbols_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CurrencySymbols};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u20A0", "e(X='\u20A0')");
		}

		/// <summary>
		/// Tests the parsing of character block CurrencySymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CurrencySymbols_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CurrencySymbols};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u20CF", "e(X='\u20CF')");
		}

		/// <summary>
		/// Tests the parsing of character block CombiningDiacriticalMarksforSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CombiningDiacriticalMarksforSymbols_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CombiningDiacriticalMarksforSymbols};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u20D0", "e(X='\u20D0')");
		}

		/// <summary>
		/// Tests the parsing of character block CombiningDiacriticalMarksforSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CombiningDiacriticalMarksforSymbols_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CombiningDiacriticalMarksforSymbols};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u20FF", "e(X='\u20FF')");
		}

		/// <summary>
		/// Tests the parsing of character block LetterlikeSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_LetterlikeSymbols_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{LetterlikeSymbols};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2100", "e(X='\u2100')");
		}

		/// <summary>
		/// Tests the parsing of character block LetterlikeSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_LetterlikeSymbols_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{LetterlikeSymbols};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u214F", "e(X='\u214F')");
		}

		/// <summary>
		/// Tests the parsing of character block NumberForms
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_NumberForms_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{NumberForms};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2150", "e(X='\u2150')");
		}

		/// <summary>
		/// Tests the parsing of character block NumberForms
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_NumberForms_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{NumberForms};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u218F", "e(X='\u218F')");
		}

		/// <summary>
		/// Tests the parsing of character block Arrows
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Arrows_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Arrows};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2190", "e(X='\u2190')");
		}

		/// <summary>
		/// Tests the parsing of character block Arrows
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Arrows_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Arrows};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u21FF", "e(X='\u21FF')");
		}

		/// <summary>
		/// Tests the parsing of character block MathematicalOperators
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MathematicalOperators_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{MathematicalOperators};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2200", "e(X='\u2200')");
		}

		/// <summary>
		/// Tests the parsing of character block MathematicalOperators
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MathematicalOperators_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{MathematicalOperators};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u22FF", "e(X='\u22FF')");
		}

		/// <summary>
		/// Tests the parsing of character block MiscellaneousTechnical
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MiscellaneousTechnical_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{MiscellaneousTechnical};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2300", "e(X='\u2300')");
		}

		/// <summary>
		/// Tests the parsing of character block MiscellaneousTechnical
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MiscellaneousTechnical_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{MiscellaneousTechnical};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u23FF", "e(X='\u23FF')");
		}

		/// <summary>
		/// Tests the parsing of character block ControlPictures
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_ControlPictures_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{ControlPictures};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2400", "e(X='\u2400')");
		}

		/// <summary>
		/// Tests the parsing of character block ControlPictures
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_ControlPictures_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{ControlPictures};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u243F", "e(X='\u243F')");
		}

		/// <summary>
		/// Tests the parsing of character block OpticalCharacterRecognition
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_OpticalCharacterRecognition_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{OpticalCharacterRecognition};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2440", "e(X='\u2440')");
		}

		/// <summary>
		/// Tests the parsing of character block OpticalCharacterRecognition
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_OpticalCharacterRecognition_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{OpticalCharacterRecognition};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u245F", "e(X='\u245F')");
		}

		/// <summary>
		/// Tests the parsing of character block EnclosedAlphanumerics
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_EnclosedAlphanumerics_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{EnclosedAlphanumerics};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2460", "e(X='\u2460')");
		}

		/// <summary>
		/// Tests the parsing of character block EnclosedAlphanumerics
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_EnclosedAlphanumerics_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{EnclosedAlphanumerics};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u24FF", "e(X='\u24FF')");
		}

		/// <summary>
		/// Tests the parsing of character block BoxDrawing
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_BoxDrawing_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{BoxDrawing};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2500", "e(X='\u2500')");
		}

		/// <summary>
		/// Tests the parsing of character block BoxDrawing
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_BoxDrawing_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{BoxDrawing};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u257F", "e(X='\u257F')");
		}

		/// <summary>
		/// Tests the parsing of character block BlockElements
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_BlockElements_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{BlockElements};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2580", "e(X='\u2580')");
		}

		/// <summary>
		/// Tests the parsing of character block BlockElements
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_BlockElements_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{BlockElements};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u259F", "e(X='\u259F')");
		}

		/// <summary>
		/// Tests the parsing of character block GeometricShapes
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_GeometricShapes_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{GeometricShapes};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u25A0", "e(X='\u25A0')");
		}

		/// <summary>
		/// Tests the parsing of character block GeometricShapes
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_GeometricShapes_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{GeometricShapes};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u25FF", "e(X='\u25FF')");
		}

		/// <summary>
		/// Tests the parsing of character block MiscellaneousSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MiscellaneousSymbols_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{MiscellaneousSymbols};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2600", "e(X='\u2600')");
		}

		/// <summary>
		/// Tests the parsing of character block MiscellaneousSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MiscellaneousSymbols_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{MiscellaneousSymbols};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u26FF", "e(X='\u26FF')");
		}

		/// <summary>
		/// Tests the parsing of character block Dingbats
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Dingbats_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Dingbats};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2700", "e(X='\u2700')");
		}

		/// <summary>
		/// Tests the parsing of character block Dingbats
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Dingbats_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Dingbats};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u27BF", "e(X='\u27BF')");
		}

		/// <summary>
		/// Tests the parsing of character block MiscellaneousMathematicalSymbols-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MiscellaneousMathematicalSymbols_A_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{MiscellaneousMathematicalSymbols-A};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u27C0", "e(X='\u27C0')");
		}

		/// <summary>
		/// Tests the parsing of character block MiscellaneousMathematicalSymbols-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MiscellaneousMathematicalSymbols_A_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{MiscellaneousMathematicalSymbols-A};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u27EF", "e(X='\u27EF')");
		}

		/// <summary>
		/// Tests the parsing of character block SupplementalArrows-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SupplementalArrows_A_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{SupplementalArrows-A};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u27F0", "e(X='\u27F0')");
		}

		/// <summary>
		/// Tests the parsing of character block SupplementalArrows-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SupplementalArrows_A_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{SupplementalArrows-A};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u27FF", "e(X='\u27FF')");
		}

		/// <summary>
		/// Tests the parsing of character block BraillePatterns
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_BraillePatterns_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{BraillePatterns};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2800", "e(X='\u2800')");
		}

		/// <summary>
		/// Tests the parsing of character block BraillePatterns
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_BraillePatterns_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{BraillePatterns};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u28FF", "e(X='\u28FF')");
		}

		/// <summary>
		/// Tests the parsing of character block SupplementalArrows-B
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SupplementalArrows_B_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{SupplementalArrows-B};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2900", "e(X='\u2900')");
		}

		/// <summary>
		/// Tests the parsing of character block SupplementalArrows-B
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SupplementalArrows_B_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{SupplementalArrows-B};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u297F", "e(X='\u297F')");
		}

		/// <summary>
		/// Tests the parsing of character block MiscellaneousMathematicalSymbols-B
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MiscellaneousMathematicalSymbols_B_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{MiscellaneousMathematicalSymbols-B};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2980", "e(X='\u2980')");
		}

		/// <summary>
		/// Tests the parsing of character block MiscellaneousMathematicalSymbols-B
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MiscellaneousMathematicalSymbols_B_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{MiscellaneousMathematicalSymbols-B};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u29FF", "e(X='\u29FF')");
		}

		/// <summary>
		/// Tests the parsing of character block SupplementalMathematicalOperators
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SupplementalMathematicalOperators_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{SupplementalMathematicalOperators};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2A00", "e(X='\u2A00')");
		}

		/// <summary>
		/// Tests the parsing of character block SupplementalMathematicalOperators
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SupplementalMathematicalOperators_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{SupplementalMathematicalOperators};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2AFF", "e(X='\u2AFF')");
		}

		/// <summary>
		/// Tests the parsing of character block MiscellaneousSymbolsandArrows
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MiscellaneousSymbolsandArrows_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{MiscellaneousSymbolsandArrows};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2B00", "e(X='\u2B00')");
		}

		/// <summary>
		/// Tests the parsing of character block MiscellaneousSymbolsandArrows
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MiscellaneousSymbolsandArrows_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{MiscellaneousSymbolsandArrows};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2BFF", "e(X='\u2BFF')");
		}

		/// <summary>
		/// Tests the parsing of character block Glagolitic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Glagolitic_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Glagolitic};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2C00", "e(X='\u2C00')");
		}

		/// <summary>
		/// Tests the parsing of character block Glagolitic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Glagolitic_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Glagolitic};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2C5F", "e(X='\u2C5F')");
		}

		/// <summary>
		/// Tests the parsing of character block LatinExtended-C
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_LatinExtended_C_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{LatinExtended-C};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2C60", "e(X='\u2C60')");
		}

		/// <summary>
		/// Tests the parsing of character block LatinExtended-C
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_LatinExtended_C_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{LatinExtended-C};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2C7F", "e(X='\u2C7F')");
		}

		/// <summary>
		/// Tests the parsing of character block Coptic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Coptic_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Coptic};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2C80", "e(X='\u2C80')");
		}

		/// <summary>
		/// Tests the parsing of character block Coptic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Coptic_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Coptic};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2CFF", "e(X='\u2CFF')");
		}

		/// <summary>
		/// Tests the parsing of character block GeorgianSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_GeorgianSupplement_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{GeorgianSupplement};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2D00", "e(X='\u2D00')");
		}

		/// <summary>
		/// Tests the parsing of character block GeorgianSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_GeorgianSupplement_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{GeorgianSupplement};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2D2F", "e(X='\u2D2F')");
		}

		/// <summary>
		/// Tests the parsing of character block Tifinagh
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Tifinagh_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Tifinagh};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2D30", "e(X='\u2D30')");
		}

		/// <summary>
		/// Tests the parsing of character block Tifinagh
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Tifinagh_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Tifinagh};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2D7F", "e(X='\u2D7F')");
		}

		/// <summary>
		/// Tests the parsing of character block EthiopicExtended
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_EthiopicExtended_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{EthiopicExtended};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2D80", "e(X='\u2D80')");
		}

		/// <summary>
		/// Tests the parsing of character block EthiopicExtended
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_EthiopicExtended_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{EthiopicExtended};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2DDF", "e(X='\u2DDF')");
		}

		/// <summary>
		/// Tests the parsing of character block CyrillicExtended-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CyrillicExtended_A_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CyrillicExtended-A};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2DE0", "e(X='\u2DE0')");
		}

		/// <summary>
		/// Tests the parsing of character block CyrillicExtended-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CyrillicExtended_A_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CyrillicExtended-A};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2DFF", "e(X='\u2DFF')");
		}

		/// <summary>
		/// Tests the parsing of character block SupplementalPunctuation
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SupplementalPunctuation_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{SupplementalPunctuation};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2E00", "e(X='\u2E00')");
		}

		/// <summary>
		/// Tests the parsing of character block SupplementalPunctuation
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SupplementalPunctuation_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{SupplementalPunctuation};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2E7F", "e(X='\u2E7F')");
		}

		/// <summary>
		/// Tests the parsing of character block CJKRadicalsSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKRadicalsSupplement_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CJKRadicalsSupplement};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2E80", "e(X='\u2E80')");
		}

		/// <summary>
		/// Tests the parsing of character block CJKRadicalsSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKRadicalsSupplement_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CJKRadicalsSupplement};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2EFF", "e(X='\u2EFF')");
		}

		/// <summary>
		/// Tests the parsing of character block KangxiRadicals
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_KangxiRadicals_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{KangxiRadicals};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2F00", "e(X='\u2F00')");
		}

		/// <summary>
		/// Tests the parsing of character block KangxiRadicals
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_KangxiRadicals_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{KangxiRadicals};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2FDF", "e(X='\u2FDF')");
		}

		/// <summary>
		/// Tests the parsing of character block IdeographicDescriptionCharacters
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_IdeographicDescriptionCharacters_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{IdeographicDescriptionCharacters};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2FF0", "e(X='\u2FF0')");
		}

		/// <summary>
		/// Tests the parsing of character block IdeographicDescriptionCharacters
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_IdeographicDescriptionCharacters_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{IdeographicDescriptionCharacters};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u2FFF", "e(X='\u2FFF')");
		}

		/// <summary>
		/// Tests the parsing of character block CJKSymbolsandPunctuation
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKSymbolsandPunctuation_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CJKSymbolsandPunctuation};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u3000", "e(X='\u3000')");
		}

		/// <summary>
		/// Tests the parsing of character block CJKSymbolsandPunctuation
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKSymbolsandPunctuation_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CJKSymbolsandPunctuation};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u303F", "e(X='\u303F')");
		}

		/// <summary>
		/// Tests the parsing of character block Hiragana
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Hiragana_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Hiragana};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u3040", "e(X='\u3040')");
		}

		/// <summary>
		/// Tests the parsing of character block Hiragana
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Hiragana_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Hiragana};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u309F", "e(X='\u309F')");
		}

		/// <summary>
		/// Tests the parsing of character block Katakana
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Katakana_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Katakana};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u30A0", "e(X='\u30A0')");
		}

		/// <summary>
		/// Tests the parsing of character block Katakana
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Katakana_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Katakana};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u30FF", "e(X='\u30FF')");
		}

		/// <summary>
		/// Tests the parsing of character block Bopomofo
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Bopomofo_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Bopomofo};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u3100", "e(X='\u3100')");
		}

		/// <summary>
		/// Tests the parsing of character block Bopomofo
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Bopomofo_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Bopomofo};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u312F", "e(X='\u312F')");
		}

		/// <summary>
		/// Tests the parsing of character block HangulCompatibilityJamo
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_HangulCompatibilityJamo_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{HangulCompatibilityJamo};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u3130", "e(X='\u3130')");
		}

		/// <summary>
		/// Tests the parsing of character block HangulCompatibilityJamo
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_HangulCompatibilityJamo_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{HangulCompatibilityJamo};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u318F", "e(X='\u318F')");
		}

		/// <summary>
		/// Tests the parsing of character block Kanbun
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Kanbun_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Kanbun};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u3190", "e(X='\u3190')");
		}

		/// <summary>
		/// Tests the parsing of character block Kanbun
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Kanbun_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Kanbun};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u319F", "e(X='\u319F')");
		}

		/// <summary>
		/// Tests the parsing of character block BopomofoExtended
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_BopomofoExtended_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{BopomofoExtended};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u31A0", "e(X='\u31A0')");
		}

		/// <summary>
		/// Tests the parsing of character block BopomofoExtended
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_BopomofoExtended_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{BopomofoExtended};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u31BF", "e(X='\u31BF')");
		}

		/// <summary>
		/// Tests the parsing of character block CJKStrokes
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKStrokes_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CJKStrokes};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u31C0", "e(X='\u31C0')");
		}

		/// <summary>
		/// Tests the parsing of character block CJKStrokes
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKStrokes_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CJKStrokes};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u31EF", "e(X='\u31EF')");
		}

		/// <summary>
		/// Tests the parsing of character block KatakanaPhoneticExtensions
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_KatakanaPhoneticExtensions_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{KatakanaPhoneticExtensions};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u31F0", "e(X='\u31F0')");
		}

		/// <summary>
		/// Tests the parsing of character block KatakanaPhoneticExtensions
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_KatakanaPhoneticExtensions_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{KatakanaPhoneticExtensions};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u31FF", "e(X='\u31FF')");
		}

		/// <summary>
		/// Tests the parsing of character block EnclosedCJKLettersandMonths
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_EnclosedCJKLettersandMonths_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{EnclosedCJKLettersandMonths};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u3200", "e(X='\u3200')");
		}

		/// <summary>
		/// Tests the parsing of character block EnclosedCJKLettersandMonths
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_EnclosedCJKLettersandMonths_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{EnclosedCJKLettersandMonths};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u32FF", "e(X='\u32FF')");
		}

		/// <summary>
		/// Tests the parsing of character block CJKCompatibility
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKCompatibility_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CJKCompatibility};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u3300", "e(X='\u3300')");
		}

		/// <summary>
		/// Tests the parsing of character block CJKCompatibility
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKCompatibility_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CJKCompatibility};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u33FF", "e(X='\u33FF')");
		}

		/// <summary>
		/// Tests the parsing of character block CJKUnifiedIdeographsExtensionA
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKUnifiedIdeographsExtensionA_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CJKUnifiedIdeographsExtensionA};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u3400", "e(X='\u3400')");
		}

		/// <summary>
		/// Tests the parsing of character block CJKUnifiedIdeographsExtensionA
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKUnifiedIdeographsExtensionA_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CJKUnifiedIdeographsExtensionA};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u4DBF", "e(X='\u4DBF')");
		}

		/// <summary>
		/// Tests the parsing of character block YijingHexagramSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_YijingHexagramSymbols_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{YijingHexagramSymbols};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u4DC0", "e(X='\u4DC0')");
		}

		/// <summary>
		/// Tests the parsing of character block YijingHexagramSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_YijingHexagramSymbols_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{YijingHexagramSymbols};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u4DFF", "e(X='\u4DFF')");
		}

		/// <summary>
		/// Tests the parsing of character block CJKUnifiedIdeographs
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKUnifiedIdeographs_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CJKUnifiedIdeographs};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u4E00", "e(X='\u4E00')");
		}

		/// <summary>
		/// Tests the parsing of character block CJKUnifiedIdeographs
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKUnifiedIdeographs_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CJKUnifiedIdeographs};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\u9FFF", "e(X='\u9FFF')");
		}

		/// <summary>
		/// Tests the parsing of character block YiSyllables
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_YiSyllables_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{YiSyllables};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA000", "e(X='\uA000')");
		}

		/// <summary>
		/// Tests the parsing of character block YiSyllables
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_YiSyllables_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{YiSyllables};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA48F", "e(X='\uA48F')");
		}

		/// <summary>
		/// Tests the parsing of character block YiRadicals
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_YiRadicals_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{YiRadicals};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA490", "e(X='\uA490')");
		}

		/// <summary>
		/// Tests the parsing of character block YiRadicals
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_YiRadicals_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{YiRadicals};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA4CF", "e(X='\uA4CF')");
		}

		/// <summary>
		/// Tests the parsing of character block Lisu
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Lisu_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Lisu};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA4D0", "e(X='\uA4D0')");
		}

		/// <summary>
		/// Tests the parsing of character block Lisu
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Lisu_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Lisu};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA4FF", "e(X='\uA4FF')");
		}

		/// <summary>
		/// Tests the parsing of character block Vai
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Vai_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Vai};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA500", "e(X='\uA500')");
		}

		/// <summary>
		/// Tests the parsing of character block Vai
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Vai_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Vai};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA63F", "e(X='\uA63F')");
		}

		/// <summary>
		/// Tests the parsing of character block CyrillicExtended-B
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CyrillicExtended_B_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CyrillicExtended-B};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA640", "e(X='\uA640')");
		}

		/// <summary>
		/// Tests the parsing of character block CyrillicExtended-B
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CyrillicExtended_B_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CyrillicExtended-B};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA69F", "e(X='\uA69F')");
		}

		/// <summary>
		/// Tests the parsing of character block Bamum
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Bamum_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Bamum};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA6A0", "e(X='\uA6A0')");
		}

		/// <summary>
		/// Tests the parsing of character block Bamum
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Bamum_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Bamum};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA6FF", "e(X='\uA6FF')");
		}

		/// <summary>
		/// Tests the parsing of character block ModifierToneLetters
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_ModifierToneLetters_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{ModifierToneLetters};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA700", "e(X='\uA700')");
		}

		/// <summary>
		/// Tests the parsing of character block ModifierToneLetters
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_ModifierToneLetters_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{ModifierToneLetters};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA71F", "e(X='\uA71F')");
		}

		/// <summary>
		/// Tests the parsing of character block LatinExtended-D
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_LatinExtended_D_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{LatinExtended-D};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA720", "e(X='\uA720')");
		}

		/// <summary>
		/// Tests the parsing of character block LatinExtended-D
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_LatinExtended_D_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{LatinExtended-D};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA7FF", "e(X='\uA7FF')");
		}

		/// <summary>
		/// Tests the parsing of character block SylotiNagri
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SylotiNagri_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{SylotiNagri};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA800", "e(X='\uA800')");
		}

		/// <summary>
		/// Tests the parsing of character block SylotiNagri
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SylotiNagri_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{SylotiNagri};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA82F", "e(X='\uA82F')");
		}

		/// <summary>
		/// Tests the parsing of character block CommonIndicNumberForms
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CommonIndicNumberForms_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CommonIndicNumberForms};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA830", "e(X='\uA830')");
		}

		/// <summary>
		/// Tests the parsing of character block CommonIndicNumberForms
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CommonIndicNumberForms_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CommonIndicNumberForms};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA83F", "e(X='\uA83F')");
		}

		/// <summary>
		/// Tests the parsing of character block Phags-pa
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Phags_pa_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Phags-pa};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA840", "e(X='\uA840')");
		}

		/// <summary>
		/// Tests the parsing of character block Phags-pa
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Phags_pa_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Phags-pa};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA87F", "e(X='\uA87F')");
		}

		/// <summary>
		/// Tests the parsing of character block Saurashtra
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Saurashtra_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Saurashtra};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA880", "e(X='\uA880')");
		}

		/// <summary>
		/// Tests the parsing of character block Saurashtra
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Saurashtra_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Saurashtra};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA8DF", "e(X='\uA8DF')");
		}

		/// <summary>
		/// Tests the parsing of character block DevanagariExtended
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_DevanagariExtended_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{DevanagariExtended};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA8E0", "e(X='\uA8E0')");
		}

		/// <summary>
		/// Tests the parsing of character block DevanagariExtended
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_DevanagariExtended_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{DevanagariExtended};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA8FF", "e(X='\uA8FF')");
		}

		/// <summary>
		/// Tests the parsing of character block KayahLi
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_KayahLi_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{KayahLi};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA900", "e(X='\uA900')");
		}

		/// <summary>
		/// Tests the parsing of character block KayahLi
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_KayahLi_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{KayahLi};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA92F", "e(X='\uA92F')");
		}

		/// <summary>
		/// Tests the parsing of character block Rejang
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Rejang_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Rejang};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA930", "e(X='\uA930')");
		}

		/// <summary>
		/// Tests the parsing of character block Rejang
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Rejang_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Rejang};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA95F", "e(X='\uA95F')");
		}

		/// <summary>
		/// Tests the parsing of character block HangulJamoExtended-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_HangulJamoExtended_A_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{HangulJamoExtended-A};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA960", "e(X='\uA960')");
		}

		/// <summary>
		/// Tests the parsing of character block HangulJamoExtended-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_HangulJamoExtended_A_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{HangulJamoExtended-A};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA97F", "e(X='\uA97F')");
		}

		/// <summary>
		/// Tests the parsing of character block Javanese
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Javanese_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Javanese};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA980", "e(X='\uA980')");
		}

		/// <summary>
		/// Tests the parsing of character block Javanese
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Javanese_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Javanese};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uA9DF", "e(X='\uA9DF')");
		}

		/// <summary>
		/// Tests the parsing of character block Cham
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Cham_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Cham};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uAA00", "e(X='\uAA00')");
		}

		/// <summary>
		/// Tests the parsing of character block Cham
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Cham_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Cham};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uAA5F", "e(X='\uAA5F')");
		}

		/// <summary>
		/// Tests the parsing of character block MyanmarExtended-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MyanmarExtended_A_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{MyanmarExtended-A};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uAA60", "e(X='\uAA60')");
		}

		/// <summary>
		/// Tests the parsing of character block MyanmarExtended-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MyanmarExtended_A_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{MyanmarExtended-A};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uAA7F", "e(X='\uAA7F')");
		}

		/// <summary>
		/// Tests the parsing of character block TaiViet
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_TaiViet_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{TaiViet};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uAA80", "e(X='\uAA80')");
		}

		/// <summary>
		/// Tests the parsing of character block TaiViet
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_TaiViet_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{TaiViet};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uAADF", "e(X='\uAADF')");
		}

		/// <summary>
		/// Tests the parsing of character block MeeteiMayekExtensions
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MeeteiMayekExtensions_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{MeeteiMayekExtensions};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uAAE0", "e(X='\uAAE0')");
		}

		/// <summary>
		/// Tests the parsing of character block MeeteiMayekExtensions
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MeeteiMayekExtensions_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{MeeteiMayekExtensions};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uAAFF", "e(X='\uAAFF')");
		}

		/// <summary>
		/// Tests the parsing of character block EthiopicExtended-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_EthiopicExtended_A_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{EthiopicExtended-A};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uAB00", "e(X='\uAB00')");
		}

		/// <summary>
		/// Tests the parsing of character block EthiopicExtended-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_EthiopicExtended_A_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{EthiopicExtended-A};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uAB2F", "e(X='\uAB2F')");
		}

		/// <summary>
		/// Tests the parsing of character block MeeteiMayek
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MeeteiMayek_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{MeeteiMayek};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uABC0", "e(X='\uABC0')");
		}

		/// <summary>
		/// Tests the parsing of character block MeeteiMayek
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MeeteiMayek_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{MeeteiMayek};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uABFF", "e(X='\uABFF')");
		}

		/// <summary>
		/// Tests the parsing of character block HangulSyllables
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_HangulSyllables_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{HangulSyllables};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uAC00", "e(X='\uAC00')");
		}

		/// <summary>
		/// Tests the parsing of character block HangulSyllables
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_HangulSyllables_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{HangulSyllables};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uD7AF", "e(X='\uD7AF')");
		}

		/// <summary>
		/// Tests the parsing of character block HangulJamoExtended-B
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_HangulJamoExtended_B_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{HangulJamoExtended-B};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uD7B0", "e(X='\uD7B0')");
		}

		/// <summary>
		/// Tests the parsing of character block HangulJamoExtended-B
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_HangulJamoExtended_B_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{HangulJamoExtended-B};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uD7FF", "e(X='\uD7FF')");
		}

		/// <summary>
		/// Tests the parsing of character block HighSurrogates
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_HighSurrogates_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{HighSurrogates};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uD800", "e(X='\uD800')");
		}

		/// <summary>
		/// Tests the parsing of character block HighSurrogates
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_HighSurrogates_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{HighSurrogates};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uDB7F", "e(X='\uDB7F')");
		}

		/// <summary>
		/// Tests the parsing of character block HighPrivateUseSurrogates
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_HighPrivateUseSurrogates_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{HighPrivateUseSurrogates};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uDB80", "e(X='\uDB80')");
		}

		/// <summary>
		/// Tests the parsing of character block HighPrivateUseSurrogates
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_HighPrivateUseSurrogates_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{HighPrivateUseSurrogates};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uDBFF", "e(X='\uDBFF')");
		}

		/// <summary>
		/// Tests the parsing of character block LowSurrogates
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_LowSurrogates_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{LowSurrogates};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uDC00", "e(X='\uDC00')");
		}

		/// <summary>
		/// Tests the parsing of character block LowSurrogates
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_LowSurrogates_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{LowSurrogates};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uDFFF", "e(X='\uDFFF')");
		}

		/// <summary>
		/// Tests the parsing of character block PrivateUseArea
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_PrivateUseArea_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{PrivateUseArea};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uE000", "e(X='\uE000')");
		}

		/// <summary>
		/// Tests the parsing of character block PrivateUseArea
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_PrivateUseArea_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{PrivateUseArea};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uF8FF", "e(X='\uF8FF')");
		}

		/// <summary>
		/// Tests the parsing of character block CJKCompatibilityIdeographs
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKCompatibilityIdeographs_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CJKCompatibilityIdeographs};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uF900", "e(X='\uF900')");
		}

		/// <summary>
		/// Tests the parsing of character block CJKCompatibilityIdeographs
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKCompatibilityIdeographs_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CJKCompatibilityIdeographs};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uFAFF", "e(X='\uFAFF')");
		}

		/// <summary>
		/// Tests the parsing of character block AlphabeticPresentationForms
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_AlphabeticPresentationForms_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{AlphabeticPresentationForms};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uFB00", "e(X='\uFB00')");
		}

		/// <summary>
		/// Tests the parsing of character block AlphabeticPresentationForms
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_AlphabeticPresentationForms_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{AlphabeticPresentationForms};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uFB4F", "e(X='\uFB4F')");
		}

		/// <summary>
		/// Tests the parsing of character block ArabicPresentationForms-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_ArabicPresentationForms_A_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{ArabicPresentationForms-A};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uFB50", "e(X='\uFB50')");
		}

		/// <summary>
		/// Tests the parsing of character block ArabicPresentationForms-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_ArabicPresentationForms_A_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{ArabicPresentationForms-A};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uFDFF", "e(X='\uFDFF')");
		}

		/// <summary>
		/// Tests the parsing of character block VariationSelectors
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_VariationSelectors_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{VariationSelectors};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uFE00", "e(X='\uFE00')");
		}

		/// <summary>
		/// Tests the parsing of character block VariationSelectors
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_VariationSelectors_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{VariationSelectors};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uFE0F", "e(X='\uFE0F')");
		}

		/// <summary>
		/// Tests the parsing of character block VerticalForms
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_VerticalForms_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{VerticalForms};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uFE10", "e(X='\uFE10')");
		}

		/// <summary>
		/// Tests the parsing of character block VerticalForms
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_VerticalForms_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{VerticalForms};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uFE1F", "e(X='\uFE1F')");
		}

		/// <summary>
		/// Tests the parsing of character block CombiningHalfMarks
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CombiningHalfMarks_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CombiningHalfMarks};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uFE20", "e(X='\uFE20')");
		}

		/// <summary>
		/// Tests the parsing of character block CombiningHalfMarks
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CombiningHalfMarks_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CombiningHalfMarks};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uFE2F", "e(X='\uFE2F')");
		}

		/// <summary>
		/// Tests the parsing of character block CJKCompatibilityForms
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKCompatibilityForms_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CJKCompatibilityForms};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uFE30", "e(X='\uFE30')");
		}

		/// <summary>
		/// Tests the parsing of character block CJKCompatibilityForms
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKCompatibilityForms_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{CJKCompatibilityForms};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uFE4F", "e(X='\uFE4F')");
		}

		/// <summary>
		/// Tests the parsing of character block SmallFormVariants
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SmallFormVariants_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{SmallFormVariants};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uFE50", "e(X='\uFE50')");
		}

		/// <summary>
		/// Tests the parsing of character block SmallFormVariants
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SmallFormVariants_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{SmallFormVariants};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uFE6F", "e(X='\uFE6F')");
		}

		/// <summary>
		/// Tests the parsing of character block ArabicPresentationForms-B
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_ArabicPresentationForms_B_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{ArabicPresentationForms-B};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uFE70", "e(X='\uFE70')");
		}

		/// <summary>
		/// Tests the parsing of character block ArabicPresentationForms-B
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_ArabicPresentationForms_B_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{ArabicPresentationForms-B};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uFEFF", "e(X='\uFEFF')");
		}

		/// <summary>
		/// Tests the parsing of character block HalfwidthAndFullwidthForms
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_HalfwidthAndFullwidthForms_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{HalfwidthAndFullwidthForms};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uFF00", "e(X='\uFF00')");
		}

		/// <summary>
		/// Tests the parsing of character block HalfwidthAndFullwidthForms
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_HalfwidthAndFullwidthForms_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{HalfwidthAndFullwidthForms};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uFFEF", "e(X='\uFFEF')");
		}

		/// <summary>
		/// Tests the parsing of character block Specials
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Specials_LeftBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Specials};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uFFF0", "e(X='\uFFF0')");
		}

		/// <summary>
		/// Tests the parsing of character block Specials
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Specials_RightBound()
		{
			SetTestDirectory();
			string grammar = "cf grammar Test { options {Axiom=\"e\";} terminals {X->ub{Specials};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, "\uFFFF", "e(X='\uFFFF')");
		}
	}
}