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
	/// Tests the support of unicode blocks
	/// </summary>
	[TestFixture]
	public class UnicodeBlocks : BaseParseSuite
	{
		/// <summary>
		/// Tests that the given codepoint can be parsed with the given block
		/// </summary>
		/// <param name="name">A Unicode block name</param>
		/// <param name="codepoint">A Unicode code point</param>
		private void Test_WithinBlock(string name, int codepoint)
		{
			UnicodeCodePoint cp = new UnicodeCodePoint(codepoint);
			string values = new string(cp.GetUTF16());
			string grammar = "grammar Test { options {Axiom=\"e\";} terminals {X->ub{" + name + "};} rules { e->X; } }";
			ParsingMatches(grammar, "Test", ParsingMethod.LALR1, values, "e(X='" + values + "')");
		}

		/// <summary>
		/// Tests the parsing of character block BasicLatin
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_BasicLatin_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("BasicLatin", 0x0);
		}

		/// <summary>
		/// Tests the parsing of character block BasicLatin
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_BasicLatin_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("BasicLatin", 0x7F);
		}

		/// <summary>
		/// Tests the parsing of character block Latin-1Supplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Latin1Supplement_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Latin-1Supplement", 0x80);
		}

		/// <summary>
		/// Tests the parsing of character block Latin-1Supplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Latin1Supplement_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Latin-1Supplement", 0xFF);
		}

		/// <summary>
		/// Tests the parsing of character block LatinExtended-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_LatinExtendedA_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("LatinExtended-A", 0x100);
		}

		/// <summary>
		/// Tests the parsing of character block LatinExtended-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_LatinExtendedA_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("LatinExtended-A", 0x17F);
		}

		/// <summary>
		/// Tests the parsing of character block LatinExtended-B
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_LatinExtendedB_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("LatinExtended-B", 0x180);
		}

		/// <summary>
		/// Tests the parsing of character block LatinExtended-B
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_LatinExtendedB_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("LatinExtended-B", 0x24F);
		}

		/// <summary>
		/// Tests the parsing of character block IPAExtensions
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_IPAExtensions_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("IPAExtensions", 0x250);
		}

		/// <summary>
		/// Tests the parsing of character block IPAExtensions
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_IPAExtensions_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("IPAExtensions", 0x2AF);
		}

		/// <summary>
		/// Tests the parsing of character block SpacingModifierLetters
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SpacingModifierLetters_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("SpacingModifierLetters", 0x2B0);
		}

		/// <summary>
		/// Tests the parsing of character block SpacingModifierLetters
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SpacingModifierLetters_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("SpacingModifierLetters", 0x2FF);
		}

		/// <summary>
		/// Tests the parsing of character block CombiningDiacriticalMarks
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CombiningDiacriticalMarks_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CombiningDiacriticalMarks", 0x300);
		}

		/// <summary>
		/// Tests the parsing of character block CombiningDiacriticalMarks
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CombiningDiacriticalMarks_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CombiningDiacriticalMarks", 0x36F);
		}

		/// <summary>
		/// Tests the parsing of character block GreekandCoptic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_GreekandCoptic_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("GreekandCoptic", 0x370);
		}

		/// <summary>
		/// Tests the parsing of character block GreekandCoptic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_GreekandCoptic_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("GreekandCoptic", 0x3FF);
		}

		/// <summary>
		/// Tests the parsing of character block Cyrillic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Cyrillic_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Cyrillic", 0x400);
		}

		/// <summary>
		/// Tests the parsing of character block Cyrillic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Cyrillic_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Cyrillic", 0x4FF);
		}

		/// <summary>
		/// Tests the parsing of character block CyrillicSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CyrillicSupplement_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CyrillicSupplement", 0x500);
		}

		/// <summary>
		/// Tests the parsing of character block CyrillicSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CyrillicSupplement_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CyrillicSupplement", 0x52F);
		}

		/// <summary>
		/// Tests the parsing of character block Armenian
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Armenian_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Armenian", 0x530);
		}

		/// <summary>
		/// Tests the parsing of character block Armenian
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Armenian_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Armenian", 0x58F);
		}

		/// <summary>
		/// Tests the parsing of character block Hebrew
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Hebrew_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Hebrew", 0x590);
		}

		/// <summary>
		/// Tests the parsing of character block Hebrew
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Hebrew_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Hebrew", 0x5FF);
		}

		/// <summary>
		/// Tests the parsing of character block Arabic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Arabic_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Arabic", 0x600);
		}

		/// <summary>
		/// Tests the parsing of character block Arabic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Arabic_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Arabic", 0x6FF);
		}

		/// <summary>
		/// Tests the parsing of character block Syriac
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Syriac_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Syriac", 0x700);
		}

		/// <summary>
		/// Tests the parsing of character block Syriac
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Syriac_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Syriac", 0x74F);
		}

		/// <summary>
		/// Tests the parsing of character block ArabicSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_ArabicSupplement_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("ArabicSupplement", 0x750);
		}

		/// <summary>
		/// Tests the parsing of character block ArabicSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_ArabicSupplement_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("ArabicSupplement", 0x77F);
		}

		/// <summary>
		/// Tests the parsing of character block Thaana
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Thaana_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Thaana", 0x780);
		}

		/// <summary>
		/// Tests the parsing of character block Thaana
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Thaana_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Thaana", 0x7BF);
		}

		/// <summary>
		/// Tests the parsing of character block NKo
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_NKo_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("NKo", 0x7C0);
		}

		/// <summary>
		/// Tests the parsing of character block NKo
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_NKo_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("NKo", 0x7FF);
		}

		/// <summary>
		/// Tests the parsing of character block Samaritan
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Samaritan_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Samaritan", 0x800);
		}

		/// <summary>
		/// Tests the parsing of character block Samaritan
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Samaritan_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Samaritan", 0x83F);
		}

		/// <summary>
		/// Tests the parsing of character block Mandaic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Mandaic_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Mandaic", 0x840);
		}

		/// <summary>
		/// Tests the parsing of character block Mandaic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Mandaic_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Mandaic", 0x85F);
		}

		/// <summary>
		/// Tests the parsing of character block ArabicExtended-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_ArabicExtendedA_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("ArabicExtended-A", 0x8A0);
		}

		/// <summary>
		/// Tests the parsing of character block ArabicExtended-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_ArabicExtendedA_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("ArabicExtended-A", 0x8FF);
		}

		/// <summary>
		/// Tests the parsing of character block Devanagari
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Devanagari_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Devanagari", 0x900);
		}

		/// <summary>
		/// Tests the parsing of character block Devanagari
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Devanagari_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Devanagari", 0x97F);
		}

		/// <summary>
		/// Tests the parsing of character block Bengali
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Bengali_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Bengali", 0x980);
		}

		/// <summary>
		/// Tests the parsing of character block Bengali
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Bengali_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Bengali", 0x9FF);
		}

		/// <summary>
		/// Tests the parsing of character block Gurmukhi
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Gurmukhi_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Gurmukhi", 0xA00);
		}

		/// <summary>
		/// Tests the parsing of character block Gurmukhi
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Gurmukhi_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Gurmukhi", 0xA7F);
		}

		/// <summary>
		/// Tests the parsing of character block Gujarati
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Gujarati_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Gujarati", 0xA80);
		}

		/// <summary>
		/// Tests the parsing of character block Gujarati
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Gujarati_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Gujarati", 0xAFF);
		}

		/// <summary>
		/// Tests the parsing of character block Oriya
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Oriya_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Oriya", 0xB00);
		}

		/// <summary>
		/// Tests the parsing of character block Oriya
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Oriya_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Oriya", 0xB7F);
		}

		/// <summary>
		/// Tests the parsing of character block Tamil
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Tamil_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Tamil", 0xB80);
		}

		/// <summary>
		/// Tests the parsing of character block Tamil
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Tamil_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Tamil", 0xBFF);
		}

		/// <summary>
		/// Tests the parsing of character block Telugu
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Telugu_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Telugu", 0xC00);
		}

		/// <summary>
		/// Tests the parsing of character block Telugu
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Telugu_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Telugu", 0xC7F);
		}

		/// <summary>
		/// Tests the parsing of character block Kannada
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Kannada_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Kannada", 0xC80);
		}

		/// <summary>
		/// Tests the parsing of character block Kannada
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Kannada_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Kannada", 0xCFF);
		}

		/// <summary>
		/// Tests the parsing of character block Malayalam
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Malayalam_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Malayalam", 0xD00);
		}

		/// <summary>
		/// Tests the parsing of character block Malayalam
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Malayalam_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Malayalam", 0xD7F);
		}

		/// <summary>
		/// Tests the parsing of character block Sinhala
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Sinhala_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Sinhala", 0xD80);
		}

		/// <summary>
		/// Tests the parsing of character block Sinhala
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Sinhala_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Sinhala", 0xDFF);
		}

		/// <summary>
		/// Tests the parsing of character block Thai
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Thai_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Thai", 0xE00);
		}

		/// <summary>
		/// Tests the parsing of character block Thai
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Thai_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Thai", 0xE7F);
		}

		/// <summary>
		/// Tests the parsing of character block Lao
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Lao_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Lao", 0xE80);
		}

		/// <summary>
		/// Tests the parsing of character block Lao
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Lao_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Lao", 0xEFF);
		}

		/// <summary>
		/// Tests the parsing of character block Tibetan
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Tibetan_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Tibetan", 0xF00);
		}

		/// <summary>
		/// Tests the parsing of character block Tibetan
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Tibetan_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Tibetan", 0xFFF);
		}

		/// <summary>
		/// Tests the parsing of character block Myanmar
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Myanmar_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Myanmar", 0x1000);
		}

		/// <summary>
		/// Tests the parsing of character block Myanmar
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Myanmar_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Myanmar", 0x109F);
		}

		/// <summary>
		/// Tests the parsing of character block Georgian
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Georgian_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Georgian", 0x10A0);
		}

		/// <summary>
		/// Tests the parsing of character block Georgian
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Georgian_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Georgian", 0x10FF);
		}

		/// <summary>
		/// Tests the parsing of character block HangulJamo
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_HangulJamo_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("HangulJamo", 0x1100);
		}

		/// <summary>
		/// Tests the parsing of character block HangulJamo
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_HangulJamo_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("HangulJamo", 0x11FF);
		}

		/// <summary>
		/// Tests the parsing of character block Ethiopic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Ethiopic_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Ethiopic", 0x1200);
		}

		/// <summary>
		/// Tests the parsing of character block Ethiopic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Ethiopic_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Ethiopic", 0x137F);
		}

		/// <summary>
		/// Tests the parsing of character block EthiopicSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_EthiopicSupplement_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("EthiopicSupplement", 0x1380);
		}

		/// <summary>
		/// Tests the parsing of character block EthiopicSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_EthiopicSupplement_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("EthiopicSupplement", 0x139F);
		}

		/// <summary>
		/// Tests the parsing of character block Cherokee
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Cherokee_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Cherokee", 0x13A0);
		}

		/// <summary>
		/// Tests the parsing of character block Cherokee
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Cherokee_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Cherokee", 0x13FF);
		}

		/// <summary>
		/// Tests the parsing of character block UnifiedCanadianAboriginalSyllabics
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_UnifiedCanadianAboriginalSyllabics_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("UnifiedCanadianAboriginalSyllabics", 0x1400);
		}

		/// <summary>
		/// Tests the parsing of character block UnifiedCanadianAboriginalSyllabics
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_UnifiedCanadianAboriginalSyllabics_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("UnifiedCanadianAboriginalSyllabics", 0x167F);
		}

		/// <summary>
		/// Tests the parsing of character block Ogham
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Ogham_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Ogham", 0x1680);
		}

		/// <summary>
		/// Tests the parsing of character block Ogham
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Ogham_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Ogham", 0x169F);
		}

		/// <summary>
		/// Tests the parsing of character block Runic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Runic_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Runic", 0x16A0);
		}

		/// <summary>
		/// Tests the parsing of character block Runic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Runic_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Runic", 0x16FF);
		}

		/// <summary>
		/// Tests the parsing of character block Tagalog
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Tagalog_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Tagalog", 0x1700);
		}

		/// <summary>
		/// Tests the parsing of character block Tagalog
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Tagalog_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Tagalog", 0x171F);
		}

		/// <summary>
		/// Tests the parsing of character block Hanunoo
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Hanunoo_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Hanunoo", 0x1720);
		}

		/// <summary>
		/// Tests the parsing of character block Hanunoo
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Hanunoo_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Hanunoo", 0x173F);
		}

		/// <summary>
		/// Tests the parsing of character block Buhid
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Buhid_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Buhid", 0x1740);
		}

		/// <summary>
		/// Tests the parsing of character block Buhid
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Buhid_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Buhid", 0x175F);
		}

		/// <summary>
		/// Tests the parsing of character block Tagbanwa
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Tagbanwa_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Tagbanwa", 0x1760);
		}

		/// <summary>
		/// Tests the parsing of character block Tagbanwa
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Tagbanwa_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Tagbanwa", 0x177F);
		}

		/// <summary>
		/// Tests the parsing of character block Khmer
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Khmer_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Khmer", 0x1780);
		}

		/// <summary>
		/// Tests the parsing of character block Khmer
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Khmer_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Khmer", 0x17FF);
		}

		/// <summary>
		/// Tests the parsing of character block Mongolian
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Mongolian_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Mongolian", 0x1800);
		}

		/// <summary>
		/// Tests the parsing of character block Mongolian
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Mongolian_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Mongolian", 0x18AF);
		}

		/// <summary>
		/// Tests the parsing of character block UnifiedCanadianAboriginalSyllabicsExtended
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_UnifiedCanadianAboriginalSyllabicsExtended_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("UnifiedCanadianAboriginalSyllabicsExtended", 0x18B0);
		}

		/// <summary>
		/// Tests the parsing of character block UnifiedCanadianAboriginalSyllabicsExtended
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_UnifiedCanadianAboriginalSyllabicsExtended_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("UnifiedCanadianAboriginalSyllabicsExtended", 0x18FF);
		}

		/// <summary>
		/// Tests the parsing of character block Limbu
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Limbu_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Limbu", 0x1900);
		}

		/// <summary>
		/// Tests the parsing of character block Limbu
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Limbu_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Limbu", 0x194F);
		}

		/// <summary>
		/// Tests the parsing of character block TaiLe
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_TaiLe_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("TaiLe", 0x1950);
		}

		/// <summary>
		/// Tests the parsing of character block TaiLe
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_TaiLe_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("TaiLe", 0x197F);
		}

		/// <summary>
		/// Tests the parsing of character block NewTaiLue
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_NewTaiLue_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("NewTaiLue", 0x1980);
		}

		/// <summary>
		/// Tests the parsing of character block NewTaiLue
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_NewTaiLue_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("NewTaiLue", 0x19DF);
		}

		/// <summary>
		/// Tests the parsing of character block KhmerSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_KhmerSymbols_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("KhmerSymbols", 0x19E0);
		}

		/// <summary>
		/// Tests the parsing of character block KhmerSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_KhmerSymbols_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("KhmerSymbols", 0x19FF);
		}

		/// <summary>
		/// Tests the parsing of character block Buginese
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Buginese_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Buginese", 0x1A00);
		}

		/// <summary>
		/// Tests the parsing of character block Buginese
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Buginese_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Buginese", 0x1A1F);
		}

		/// <summary>
		/// Tests the parsing of character block TaiTham
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_TaiTham_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("TaiTham", 0x1A20);
		}

		/// <summary>
		/// Tests the parsing of character block TaiTham
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_TaiTham_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("TaiTham", 0x1AAF);
		}

		/// <summary>
		/// Tests the parsing of character block Balinese
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Balinese_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Balinese", 0x1B00);
		}

		/// <summary>
		/// Tests the parsing of character block Balinese
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Balinese_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Balinese", 0x1B7F);
		}

		/// <summary>
		/// Tests the parsing of character block Sundanese
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Sundanese_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Sundanese", 0x1B80);
		}

		/// <summary>
		/// Tests the parsing of character block Sundanese
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Sundanese_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Sundanese", 0x1BBF);
		}

		/// <summary>
		/// Tests the parsing of character block Batak
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Batak_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Batak", 0x1BC0);
		}

		/// <summary>
		/// Tests the parsing of character block Batak
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Batak_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Batak", 0x1BFF);
		}

		/// <summary>
		/// Tests the parsing of character block Lepcha
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Lepcha_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Lepcha", 0x1C00);
		}

		/// <summary>
		/// Tests the parsing of character block Lepcha
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Lepcha_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Lepcha", 0x1C4F);
		}

		/// <summary>
		/// Tests the parsing of character block OlChiki
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_OlChiki_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("OlChiki", 0x1C50);
		}

		/// <summary>
		/// Tests the parsing of character block OlChiki
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_OlChiki_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("OlChiki", 0x1C7F);
		}

		/// <summary>
		/// Tests the parsing of character block SundaneseSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SundaneseSupplement_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("SundaneseSupplement", 0x1CC0);
		}

		/// <summary>
		/// Tests the parsing of character block SundaneseSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SundaneseSupplement_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("SundaneseSupplement", 0x1CCF);
		}

		/// <summary>
		/// Tests the parsing of character block VedicExtensions
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_VedicExtensions_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("VedicExtensions", 0x1CD0);
		}

		/// <summary>
		/// Tests the parsing of character block VedicExtensions
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_VedicExtensions_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("VedicExtensions", 0x1CFF);
		}

		/// <summary>
		/// Tests the parsing of character block PhoneticExtensions
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_PhoneticExtensions_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("PhoneticExtensions", 0x1D00);
		}

		/// <summary>
		/// Tests the parsing of character block PhoneticExtensions
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_PhoneticExtensions_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("PhoneticExtensions", 0x1D7F);
		}

		/// <summary>
		/// Tests the parsing of character block PhoneticExtensionsSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_PhoneticExtensionsSupplement_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("PhoneticExtensionsSupplement", 0x1D80);
		}

		/// <summary>
		/// Tests the parsing of character block PhoneticExtensionsSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_PhoneticExtensionsSupplement_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("PhoneticExtensionsSupplement", 0x1DBF);
		}

		/// <summary>
		/// Tests the parsing of character block CombiningDiacriticalMarksSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CombiningDiacriticalMarksSupplement_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CombiningDiacriticalMarksSupplement", 0x1DC0);
		}

		/// <summary>
		/// Tests the parsing of character block CombiningDiacriticalMarksSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CombiningDiacriticalMarksSupplement_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CombiningDiacriticalMarksSupplement", 0x1DFF);
		}

		/// <summary>
		/// Tests the parsing of character block LatinExtendedAdditional
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_LatinExtendedAdditional_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("LatinExtendedAdditional", 0x1E00);
		}

		/// <summary>
		/// Tests the parsing of character block LatinExtendedAdditional
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_LatinExtendedAdditional_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("LatinExtendedAdditional", 0x1EFF);
		}

		/// <summary>
		/// Tests the parsing of character block GreekExtended
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_GreekExtended_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("GreekExtended", 0x1F00);
		}

		/// <summary>
		/// Tests the parsing of character block GreekExtended
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_GreekExtended_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("GreekExtended", 0x1FFF);
		}

		/// <summary>
		/// Tests the parsing of character block GeneralPunctuation
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_GeneralPunctuation_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("GeneralPunctuation", 0x2000);
		}

		/// <summary>
		/// Tests the parsing of character block GeneralPunctuation
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_GeneralPunctuation_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("GeneralPunctuation", 0x206F);
		}

		/// <summary>
		/// Tests the parsing of character block SuperscriptsandSubscripts
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SuperscriptsandSubscripts_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("SuperscriptsandSubscripts", 0x2070);
		}

		/// <summary>
		/// Tests the parsing of character block SuperscriptsandSubscripts
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SuperscriptsandSubscripts_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("SuperscriptsandSubscripts", 0x209F);
		}

		/// <summary>
		/// Tests the parsing of character block CurrencySymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CurrencySymbols_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CurrencySymbols", 0x20A0);
		}

		/// <summary>
		/// Tests the parsing of character block CurrencySymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CurrencySymbols_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CurrencySymbols", 0x20CF);
		}

		/// <summary>
		/// Tests the parsing of character block CombiningDiacriticalMarksforSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CombiningDiacriticalMarksforSymbols_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CombiningDiacriticalMarksforSymbols", 0x20D0);
		}

		/// <summary>
		/// Tests the parsing of character block CombiningDiacriticalMarksforSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CombiningDiacriticalMarksforSymbols_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CombiningDiacriticalMarksforSymbols", 0x20FF);
		}

		/// <summary>
		/// Tests the parsing of character block LetterlikeSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_LetterlikeSymbols_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("LetterlikeSymbols", 0x2100);
		}

		/// <summary>
		/// Tests the parsing of character block LetterlikeSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_LetterlikeSymbols_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("LetterlikeSymbols", 0x214F);
		}

		/// <summary>
		/// Tests the parsing of character block NumberForms
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_NumberForms_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("NumberForms", 0x2150);
		}

		/// <summary>
		/// Tests the parsing of character block NumberForms
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_NumberForms_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("NumberForms", 0x218F);
		}

		/// <summary>
		/// Tests the parsing of character block Arrows
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Arrows_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Arrows", 0x2190);
		}

		/// <summary>
		/// Tests the parsing of character block Arrows
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Arrows_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Arrows", 0x21FF);
		}

		/// <summary>
		/// Tests the parsing of character block MathematicalOperators
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MathematicalOperators_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("MathematicalOperators", 0x2200);
		}

		/// <summary>
		/// Tests the parsing of character block MathematicalOperators
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MathematicalOperators_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("MathematicalOperators", 0x22FF);
		}

		/// <summary>
		/// Tests the parsing of character block MiscellaneousTechnical
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MiscellaneousTechnical_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("MiscellaneousTechnical", 0x2300);
		}

		/// <summary>
		/// Tests the parsing of character block MiscellaneousTechnical
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MiscellaneousTechnical_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("MiscellaneousTechnical", 0x23FF);
		}

		/// <summary>
		/// Tests the parsing of character block ControlPictures
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_ControlPictures_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("ControlPictures", 0x2400);
		}

		/// <summary>
		/// Tests the parsing of character block ControlPictures
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_ControlPictures_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("ControlPictures", 0x243F);
		}

		/// <summary>
		/// Tests the parsing of character block OpticalCharacterRecognition
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_OpticalCharacterRecognition_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("OpticalCharacterRecognition", 0x2440);
		}

		/// <summary>
		/// Tests the parsing of character block OpticalCharacterRecognition
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_OpticalCharacterRecognition_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("OpticalCharacterRecognition", 0x245F);
		}

		/// <summary>
		/// Tests the parsing of character block EnclosedAlphanumerics
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_EnclosedAlphanumerics_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("EnclosedAlphanumerics", 0x2460);
		}

		/// <summary>
		/// Tests the parsing of character block EnclosedAlphanumerics
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_EnclosedAlphanumerics_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("EnclosedAlphanumerics", 0x24FF);
		}

		/// <summary>
		/// Tests the parsing of character block BoxDrawing
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_BoxDrawing_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("BoxDrawing", 0x2500);
		}

		/// <summary>
		/// Tests the parsing of character block BoxDrawing
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_BoxDrawing_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("BoxDrawing", 0x257F);
		}

		/// <summary>
		/// Tests the parsing of character block BlockElements
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_BlockElements_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("BlockElements", 0x2580);
		}

		/// <summary>
		/// Tests the parsing of character block BlockElements
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_BlockElements_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("BlockElements", 0x259F);
		}

		/// <summary>
		/// Tests the parsing of character block GeometricShapes
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_GeometricShapes_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("GeometricShapes", 0x25A0);
		}

		/// <summary>
		/// Tests the parsing of character block GeometricShapes
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_GeometricShapes_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("GeometricShapes", 0x25FF);
		}

		/// <summary>
		/// Tests the parsing of character block MiscellaneousSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MiscellaneousSymbols_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("MiscellaneousSymbols", 0x2600);
		}

		/// <summary>
		/// Tests the parsing of character block MiscellaneousSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MiscellaneousSymbols_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("MiscellaneousSymbols", 0x26FF);
		}

		/// <summary>
		/// Tests the parsing of character block Dingbats
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Dingbats_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Dingbats", 0x2700);
		}

		/// <summary>
		/// Tests the parsing of character block Dingbats
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Dingbats_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Dingbats", 0x27BF);
		}

		/// <summary>
		/// Tests the parsing of character block MiscellaneousMathematicalSymbols-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MiscellaneousMathematicalSymbolsA_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("MiscellaneousMathematicalSymbols-A", 0x27C0);
		}

		/// <summary>
		/// Tests the parsing of character block MiscellaneousMathematicalSymbols-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MiscellaneousMathematicalSymbolsA_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("MiscellaneousMathematicalSymbols-A", 0x27EF);
		}

		/// <summary>
		/// Tests the parsing of character block SupplementalArrows-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SupplementalArrowsA_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("SupplementalArrows-A", 0x27F0);
		}

		/// <summary>
		/// Tests the parsing of character block SupplementalArrows-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SupplementalArrowsA_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("SupplementalArrows-A", 0x27FF);
		}

		/// <summary>
		/// Tests the parsing of character block BraillePatterns
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_BraillePatterns_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("BraillePatterns", 0x2800);
		}

		/// <summary>
		/// Tests the parsing of character block BraillePatterns
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_BraillePatterns_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("BraillePatterns", 0x28FF);
		}

		/// <summary>
		/// Tests the parsing of character block SupplementalArrows-B
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SupplementalArrowsB_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("SupplementalArrows-B", 0x2900);
		}

		/// <summary>
		/// Tests the parsing of character block SupplementalArrows-B
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SupplementalArrowsB_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("SupplementalArrows-B", 0x297F);
		}

		/// <summary>
		/// Tests the parsing of character block MiscellaneousMathematicalSymbols-B
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MiscellaneousMathematicalSymbolsB_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("MiscellaneousMathematicalSymbols-B", 0x2980);
		}

		/// <summary>
		/// Tests the parsing of character block MiscellaneousMathematicalSymbols-B
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MiscellaneousMathematicalSymbolsB_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("MiscellaneousMathematicalSymbols-B", 0x29FF);
		}

		/// <summary>
		/// Tests the parsing of character block SupplementalMathematicalOperators
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SupplementalMathematicalOperators_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("SupplementalMathematicalOperators", 0x2A00);
		}

		/// <summary>
		/// Tests the parsing of character block SupplementalMathematicalOperators
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SupplementalMathematicalOperators_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("SupplementalMathematicalOperators", 0x2AFF);
		}

		/// <summary>
		/// Tests the parsing of character block MiscellaneousSymbolsandArrows
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MiscellaneousSymbolsandArrows_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("MiscellaneousSymbolsandArrows", 0x2B00);
		}

		/// <summary>
		/// Tests the parsing of character block MiscellaneousSymbolsandArrows
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MiscellaneousSymbolsandArrows_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("MiscellaneousSymbolsandArrows", 0x2BFF);
		}

		/// <summary>
		/// Tests the parsing of character block Glagolitic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Glagolitic_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Glagolitic", 0x2C00);
		}

		/// <summary>
		/// Tests the parsing of character block Glagolitic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Glagolitic_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Glagolitic", 0x2C5F);
		}

		/// <summary>
		/// Tests the parsing of character block LatinExtended-C
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_LatinExtendedC_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("LatinExtended-C", 0x2C60);
		}

		/// <summary>
		/// Tests the parsing of character block LatinExtended-C
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_LatinExtendedC_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("LatinExtended-C", 0x2C7F);
		}

		/// <summary>
		/// Tests the parsing of character block Coptic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Coptic_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Coptic", 0x2C80);
		}

		/// <summary>
		/// Tests the parsing of character block Coptic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Coptic_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Coptic", 0x2CFF);
		}

		/// <summary>
		/// Tests the parsing of character block GeorgianSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_GeorgianSupplement_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("GeorgianSupplement", 0x2D00);
		}

		/// <summary>
		/// Tests the parsing of character block GeorgianSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_GeorgianSupplement_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("GeorgianSupplement", 0x2D2F);
		}

		/// <summary>
		/// Tests the parsing of character block Tifinagh
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Tifinagh_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Tifinagh", 0x2D30);
		}

		/// <summary>
		/// Tests the parsing of character block Tifinagh
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Tifinagh_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Tifinagh", 0x2D7F);
		}

		/// <summary>
		/// Tests the parsing of character block EthiopicExtended
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_EthiopicExtended_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("EthiopicExtended", 0x2D80);
		}

		/// <summary>
		/// Tests the parsing of character block EthiopicExtended
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_EthiopicExtended_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("EthiopicExtended", 0x2DDF);
		}

		/// <summary>
		/// Tests the parsing of character block CyrillicExtended-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CyrillicExtendedA_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CyrillicExtended-A", 0x2DE0);
		}

		/// <summary>
		/// Tests the parsing of character block CyrillicExtended-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CyrillicExtendedA_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CyrillicExtended-A", 0x2DFF);
		}

		/// <summary>
		/// Tests the parsing of character block SupplementalPunctuation
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SupplementalPunctuation_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("SupplementalPunctuation", 0x2E00);
		}

		/// <summary>
		/// Tests the parsing of character block SupplementalPunctuation
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SupplementalPunctuation_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("SupplementalPunctuation", 0x2E7F);
		}

		/// <summary>
		/// Tests the parsing of character block CJKRadicalsSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKRadicalsSupplement_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CJKRadicalsSupplement", 0x2E80);
		}

		/// <summary>
		/// Tests the parsing of character block CJKRadicalsSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKRadicalsSupplement_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CJKRadicalsSupplement", 0x2EFF);
		}

		/// <summary>
		/// Tests the parsing of character block KangxiRadicals
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_KangxiRadicals_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("KangxiRadicals", 0x2F00);
		}

		/// <summary>
		/// Tests the parsing of character block KangxiRadicals
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_KangxiRadicals_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("KangxiRadicals", 0x2FDF);
		}

		/// <summary>
		/// Tests the parsing of character block IdeographicDescriptionCharacters
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_IdeographicDescriptionCharacters_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("IdeographicDescriptionCharacters", 0x2FF0);
		}

		/// <summary>
		/// Tests the parsing of character block IdeographicDescriptionCharacters
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_IdeographicDescriptionCharacters_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("IdeographicDescriptionCharacters", 0x2FFF);
		}

		/// <summary>
		/// Tests the parsing of character block CJKSymbolsandPunctuation
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKSymbolsandPunctuation_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CJKSymbolsandPunctuation", 0x3000);
		}

		/// <summary>
		/// Tests the parsing of character block CJKSymbolsandPunctuation
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKSymbolsandPunctuation_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CJKSymbolsandPunctuation", 0x303F);
		}

		/// <summary>
		/// Tests the parsing of character block Hiragana
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Hiragana_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Hiragana", 0x3040);
		}

		/// <summary>
		/// Tests the parsing of character block Hiragana
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Hiragana_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Hiragana", 0x309F);
		}

		/// <summary>
		/// Tests the parsing of character block Katakana
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Katakana_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Katakana", 0x30A0);
		}

		/// <summary>
		/// Tests the parsing of character block Katakana
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Katakana_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Katakana", 0x30FF);
		}

		/// <summary>
		/// Tests the parsing of character block Bopomofo
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Bopomofo_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Bopomofo", 0x3100);
		}

		/// <summary>
		/// Tests the parsing of character block Bopomofo
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Bopomofo_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Bopomofo", 0x312F);
		}

		/// <summary>
		/// Tests the parsing of character block HangulCompatibilityJamo
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_HangulCompatibilityJamo_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("HangulCompatibilityJamo", 0x3130);
		}

		/// <summary>
		/// Tests the parsing of character block HangulCompatibilityJamo
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_HangulCompatibilityJamo_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("HangulCompatibilityJamo", 0x318F);
		}

		/// <summary>
		/// Tests the parsing of character block Kanbun
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Kanbun_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Kanbun", 0x3190);
		}

		/// <summary>
		/// Tests the parsing of character block Kanbun
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Kanbun_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Kanbun", 0x319F);
		}

		/// <summary>
		/// Tests the parsing of character block BopomofoExtended
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_BopomofoExtended_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("BopomofoExtended", 0x31A0);
		}

		/// <summary>
		/// Tests the parsing of character block BopomofoExtended
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_BopomofoExtended_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("BopomofoExtended", 0x31BF);
		}

		/// <summary>
		/// Tests the parsing of character block CJKStrokes
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKStrokes_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CJKStrokes", 0x31C0);
		}

		/// <summary>
		/// Tests the parsing of character block CJKStrokes
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKStrokes_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CJKStrokes", 0x31EF);
		}

		/// <summary>
		/// Tests the parsing of character block KatakanaPhoneticExtensions
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_KatakanaPhoneticExtensions_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("KatakanaPhoneticExtensions", 0x31F0);
		}

		/// <summary>
		/// Tests the parsing of character block KatakanaPhoneticExtensions
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_KatakanaPhoneticExtensions_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("KatakanaPhoneticExtensions", 0x31FF);
		}

		/// <summary>
		/// Tests the parsing of character block EnclosedCJKLettersandMonths
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_EnclosedCJKLettersandMonths_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("EnclosedCJKLettersandMonths", 0x3200);
		}

		/// <summary>
		/// Tests the parsing of character block EnclosedCJKLettersandMonths
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_EnclosedCJKLettersandMonths_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("EnclosedCJKLettersandMonths", 0x32FF);
		}

		/// <summary>
		/// Tests the parsing of character block CJKCompatibility
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKCompatibility_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CJKCompatibility", 0x3300);
		}

		/// <summary>
		/// Tests the parsing of character block CJKCompatibility
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKCompatibility_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CJKCompatibility", 0x33FF);
		}

		/// <summary>
		/// Tests the parsing of character block CJKUnifiedIdeographsExtensionA
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKUnifiedIdeographsExtensionA_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CJKUnifiedIdeographsExtensionA", 0x3400);
		}

		/// <summary>
		/// Tests the parsing of character block CJKUnifiedIdeographsExtensionA
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKUnifiedIdeographsExtensionA_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CJKUnifiedIdeographsExtensionA", 0x4DBF);
		}

		/// <summary>
		/// Tests the parsing of character block YijingHexagramSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_YijingHexagramSymbols_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("YijingHexagramSymbols", 0x4DC0);
		}

		/// <summary>
		/// Tests the parsing of character block YijingHexagramSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_YijingHexagramSymbols_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("YijingHexagramSymbols", 0x4DFF);
		}

		/// <summary>
		/// Tests the parsing of character block CJKUnifiedIdeographs
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKUnifiedIdeographs_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CJKUnifiedIdeographs", 0x4E00);
		}

		/// <summary>
		/// Tests the parsing of character block CJKUnifiedIdeographs
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKUnifiedIdeographs_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CJKUnifiedIdeographs", 0x9FFF);
		}

		/// <summary>
		/// Tests the parsing of character block YiSyllables
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_YiSyllables_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("YiSyllables", 0xA000);
		}

		/// <summary>
		/// Tests the parsing of character block YiSyllables
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_YiSyllables_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("YiSyllables", 0xA48F);
		}

		/// <summary>
		/// Tests the parsing of character block YiRadicals
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_YiRadicals_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("YiRadicals", 0xA490);
		}

		/// <summary>
		/// Tests the parsing of character block YiRadicals
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_YiRadicals_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("YiRadicals", 0xA4CF);
		}

		/// <summary>
		/// Tests the parsing of character block Lisu
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Lisu_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Lisu", 0xA4D0);
		}

		/// <summary>
		/// Tests the parsing of character block Lisu
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Lisu_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Lisu", 0xA4FF);
		}

		/// <summary>
		/// Tests the parsing of character block Vai
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Vai_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Vai", 0xA500);
		}

		/// <summary>
		/// Tests the parsing of character block Vai
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Vai_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Vai", 0xA63F);
		}

		/// <summary>
		/// Tests the parsing of character block CyrillicExtended-B
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CyrillicExtendedB_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CyrillicExtended-B", 0xA640);
		}

		/// <summary>
		/// Tests the parsing of character block CyrillicExtended-B
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CyrillicExtendedB_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CyrillicExtended-B", 0xA69F);
		}

		/// <summary>
		/// Tests the parsing of character block Bamum
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Bamum_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Bamum", 0xA6A0);
		}

		/// <summary>
		/// Tests the parsing of character block Bamum
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Bamum_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Bamum", 0xA6FF);
		}

		/// <summary>
		/// Tests the parsing of character block ModifierToneLetters
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_ModifierToneLetters_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("ModifierToneLetters", 0xA700);
		}

		/// <summary>
		/// Tests the parsing of character block ModifierToneLetters
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_ModifierToneLetters_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("ModifierToneLetters", 0xA71F);
		}

		/// <summary>
		/// Tests the parsing of character block LatinExtended-D
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_LatinExtendedD_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("LatinExtended-D", 0xA720);
		}

		/// <summary>
		/// Tests the parsing of character block LatinExtended-D
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_LatinExtendedD_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("LatinExtended-D", 0xA7FF);
		}

		/// <summary>
		/// Tests the parsing of character block SylotiNagri
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SylotiNagri_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("SylotiNagri", 0xA800);
		}

		/// <summary>
		/// Tests the parsing of character block SylotiNagri
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SylotiNagri_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("SylotiNagri", 0xA82F);
		}

		/// <summary>
		/// Tests the parsing of character block CommonIndicNumberForms
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CommonIndicNumberForms_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CommonIndicNumberForms", 0xA830);
		}

		/// <summary>
		/// Tests the parsing of character block CommonIndicNumberForms
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CommonIndicNumberForms_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CommonIndicNumberForms", 0xA83F);
		}

		/// <summary>
		/// Tests the parsing of character block Phags-pa
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Phagspa_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Phags-pa", 0xA840);
		}

		/// <summary>
		/// Tests the parsing of character block Phags-pa
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Phagspa_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Phags-pa", 0xA87F);
		}

		/// <summary>
		/// Tests the parsing of character block Saurashtra
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Saurashtra_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Saurashtra", 0xA880);
		}

		/// <summary>
		/// Tests the parsing of character block Saurashtra
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Saurashtra_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Saurashtra", 0xA8DF);
		}

		/// <summary>
		/// Tests the parsing of character block DevanagariExtended
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_DevanagariExtended_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("DevanagariExtended", 0xA8E0);
		}

		/// <summary>
		/// Tests the parsing of character block DevanagariExtended
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_DevanagariExtended_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("DevanagariExtended", 0xA8FF);
		}

		/// <summary>
		/// Tests the parsing of character block KayahLi
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_KayahLi_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("KayahLi", 0xA900);
		}

		/// <summary>
		/// Tests the parsing of character block KayahLi
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_KayahLi_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("KayahLi", 0xA92F);
		}

		/// <summary>
		/// Tests the parsing of character block Rejang
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Rejang_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Rejang", 0xA930);
		}

		/// <summary>
		/// Tests the parsing of character block Rejang
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Rejang_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Rejang", 0xA95F);
		}

		/// <summary>
		/// Tests the parsing of character block HangulJamoExtended-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_HangulJamoExtendedA_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("HangulJamoExtended-A", 0xA960);
		}

		/// <summary>
		/// Tests the parsing of character block HangulJamoExtended-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_HangulJamoExtendedA_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("HangulJamoExtended-A", 0xA97F);
		}

		/// <summary>
		/// Tests the parsing of character block Javanese
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Javanese_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Javanese", 0xA980);
		}

		/// <summary>
		/// Tests the parsing of character block Javanese
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Javanese_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Javanese", 0xA9DF);
		}

		/// <summary>
		/// Tests the parsing of character block Cham
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Cham_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Cham", 0xAA00);
		}

		/// <summary>
		/// Tests the parsing of character block Cham
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Cham_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Cham", 0xAA5F);
		}

		/// <summary>
		/// Tests the parsing of character block MyanmarExtended-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MyanmarExtendedA_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("MyanmarExtended-A", 0xAA60);
		}

		/// <summary>
		/// Tests the parsing of character block MyanmarExtended-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MyanmarExtendedA_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("MyanmarExtended-A", 0xAA7F);
		}

		/// <summary>
		/// Tests the parsing of character block TaiViet
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_TaiViet_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("TaiViet", 0xAA80);
		}

		/// <summary>
		/// Tests the parsing of character block TaiViet
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_TaiViet_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("TaiViet", 0xAADF);
		}

		/// <summary>
		/// Tests the parsing of character block MeeteiMayekExtensions
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MeeteiMayekExtensions_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("MeeteiMayekExtensions", 0xAAE0);
		}

		/// <summary>
		/// Tests the parsing of character block MeeteiMayekExtensions
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MeeteiMayekExtensions_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("MeeteiMayekExtensions", 0xAAFF);
		}

		/// <summary>
		/// Tests the parsing of character block EthiopicExtended-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_EthiopicExtendedA_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("EthiopicExtended-A", 0xAB00);
		}

		/// <summary>
		/// Tests the parsing of character block EthiopicExtended-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_EthiopicExtendedA_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("EthiopicExtended-A", 0xAB2F);
		}

		/// <summary>
		/// Tests the parsing of character block MeeteiMayek
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MeeteiMayek_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("MeeteiMayek", 0xABC0);
		}

		/// <summary>
		/// Tests the parsing of character block MeeteiMayek
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MeeteiMayek_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("MeeteiMayek", 0xABFF);
		}

		/// <summary>
		/// Tests the parsing of character block HangulSyllables
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_HangulSyllables_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("HangulSyllables", 0xAC00);
		}

		/// <summary>
		/// Tests the parsing of character block HangulSyllables
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_HangulSyllables_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("HangulSyllables", 0xD7AF);
		}

		/// <summary>
		/// Tests the parsing of character block HangulJamoExtended-B
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_HangulJamoExtendedB_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("HangulJamoExtended-B", 0xD7B0);
		}

		/// <summary>
		/// Tests the parsing of character block HangulJamoExtended-B
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_HangulJamoExtendedB_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("HangulJamoExtended-B", 0xD7FF);
		}

		/// <summary>
		/// Tests the parsing of character block PrivateUseArea
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_PrivateUseArea_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("PrivateUseArea", 0xE000);
		}

		/// <summary>
		/// Tests the parsing of character block PrivateUseArea
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_PrivateUseArea_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("PrivateUseArea", 0xF8FF);
		}

		/// <summary>
		/// Tests the parsing of character block CJKCompatibilityIdeographs
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKCompatibilityIdeographs_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CJKCompatibilityIdeographs", 0xF900);
		}

		/// <summary>
		/// Tests the parsing of character block CJKCompatibilityIdeographs
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKCompatibilityIdeographs_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CJKCompatibilityIdeographs", 0xFAFF);
		}

		/// <summary>
		/// Tests the parsing of character block AlphabeticPresentationForms
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_AlphabeticPresentationForms_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("AlphabeticPresentationForms", 0xFB00);
		}

		/// <summary>
		/// Tests the parsing of character block AlphabeticPresentationForms
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_AlphabeticPresentationForms_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("AlphabeticPresentationForms", 0xFB4F);
		}

		/// <summary>
		/// Tests the parsing of character block ArabicPresentationForms-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_ArabicPresentationFormsA_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("ArabicPresentationForms-A", 0xFB50);
		}

		/// <summary>
		/// Tests the parsing of character block ArabicPresentationForms-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_ArabicPresentationFormsA_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("ArabicPresentationForms-A", 0xFDFF);
		}

		/// <summary>
		/// Tests the parsing of character block VariationSelectors
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_VariationSelectors_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("VariationSelectors", 0xFE00);
		}

		/// <summary>
		/// Tests the parsing of character block VariationSelectors
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_VariationSelectors_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("VariationSelectors", 0xFE0F);
		}

		/// <summary>
		/// Tests the parsing of character block VerticalForms
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_VerticalForms_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("VerticalForms", 0xFE10);
		}

		/// <summary>
		/// Tests the parsing of character block VerticalForms
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_VerticalForms_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("VerticalForms", 0xFE1F);
		}

		/// <summary>
		/// Tests the parsing of character block CombiningHalfMarks
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CombiningHalfMarks_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CombiningHalfMarks", 0xFE20);
		}

		/// <summary>
		/// Tests the parsing of character block CombiningHalfMarks
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CombiningHalfMarks_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CombiningHalfMarks", 0xFE2F);
		}

		/// <summary>
		/// Tests the parsing of character block CJKCompatibilityForms
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKCompatibilityForms_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CJKCompatibilityForms", 0xFE30);
		}

		/// <summary>
		/// Tests the parsing of character block CJKCompatibilityForms
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKCompatibilityForms_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CJKCompatibilityForms", 0xFE4F);
		}

		/// <summary>
		/// Tests the parsing of character block SmallFormVariants
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SmallFormVariants_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("SmallFormVariants", 0xFE50);
		}

		/// <summary>
		/// Tests the parsing of character block SmallFormVariants
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SmallFormVariants_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("SmallFormVariants", 0xFE6F);
		}

		/// <summary>
		/// Tests the parsing of character block ArabicPresentationForms-B
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_ArabicPresentationFormsB_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("ArabicPresentationForms-B", 0xFE70);
		}

		/// <summary>
		/// Tests the parsing of character block ArabicPresentationForms-B
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_ArabicPresentationFormsB_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("ArabicPresentationForms-B", 0xFEFF);
		}

		/// <summary>
		/// Tests the parsing of character block HalfwidthandFullwidthForms
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_HalfwidthandFullwidthForms_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("HalfwidthandFullwidthForms", 0xFF00);
		}

		/// <summary>
		/// Tests the parsing of character block HalfwidthandFullwidthForms
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_HalfwidthandFullwidthForms_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("HalfwidthandFullwidthForms", 0xFFEF);
		}

		/// <summary>
		/// Tests the parsing of character block Specials
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Specials_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Specials", 0xFFF0);
		}

		/// <summary>
		/// Tests the parsing of character block Specials
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Specials_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Specials", 0xFFFF);
		}

		/// <summary>
		/// Tests the parsing of character block LinearBSyllabary
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_LinearBSyllabary_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("LinearBSyllabary", 0x10000);
		}

		/// <summary>
		/// Tests the parsing of character block LinearBSyllabary
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_LinearBSyllabary_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("LinearBSyllabary", 0x1007F);
		}

		/// <summary>
		/// Tests the parsing of character block LinearBIdeograms
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_LinearBIdeograms_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("LinearBIdeograms", 0x10080);
		}

		/// <summary>
		/// Tests the parsing of character block LinearBIdeograms
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_LinearBIdeograms_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("LinearBIdeograms", 0x100FF);
		}

		/// <summary>
		/// Tests the parsing of character block AegeanNumbers
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_AegeanNumbers_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("AegeanNumbers", 0x10100);
		}

		/// <summary>
		/// Tests the parsing of character block AegeanNumbers
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_AegeanNumbers_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("AegeanNumbers", 0x1013F);
		}

		/// <summary>
		/// Tests the parsing of character block AncientGreekNumbers
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_AncientGreekNumbers_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("AncientGreekNumbers", 0x10140);
		}

		/// <summary>
		/// Tests the parsing of character block AncientGreekNumbers
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_AncientGreekNumbers_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("AncientGreekNumbers", 0x1018F);
		}

		/// <summary>
		/// Tests the parsing of character block AncientSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_AncientSymbols_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("AncientSymbols", 0x10190);
		}

		/// <summary>
		/// Tests the parsing of character block AncientSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_AncientSymbols_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("AncientSymbols", 0x101CF);
		}

		/// <summary>
		/// Tests the parsing of character block PhaistosDisc
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_PhaistosDisc_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("PhaistosDisc", 0x101D0);
		}

		/// <summary>
		/// Tests the parsing of character block PhaistosDisc
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_PhaistosDisc_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("PhaistosDisc", 0x101FF);
		}

		/// <summary>
		/// Tests the parsing of character block Lycian
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Lycian_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Lycian", 0x10280);
		}

		/// <summary>
		/// Tests the parsing of character block Lycian
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Lycian_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Lycian", 0x1029F);
		}

		/// <summary>
		/// Tests the parsing of character block Carian
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Carian_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Carian", 0x102A0);
		}

		/// <summary>
		/// Tests the parsing of character block Carian
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Carian_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Carian", 0x102DF);
		}

		/// <summary>
		/// Tests the parsing of character block OldItalic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_OldItalic_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("OldItalic", 0x10300);
		}

		/// <summary>
		/// Tests the parsing of character block OldItalic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_OldItalic_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("OldItalic", 0x1032F);
		}

		/// <summary>
		/// Tests the parsing of character block Gothic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Gothic_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Gothic", 0x10330);
		}

		/// <summary>
		/// Tests the parsing of character block Gothic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Gothic_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Gothic", 0x1034F);
		}

		/// <summary>
		/// Tests the parsing of character block Ugaritic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Ugaritic_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Ugaritic", 0x10380);
		}

		/// <summary>
		/// Tests the parsing of character block Ugaritic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Ugaritic_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Ugaritic", 0x1039F);
		}

		/// <summary>
		/// Tests the parsing of character block OldPersian
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_OldPersian_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("OldPersian", 0x103A0);
		}

		/// <summary>
		/// Tests the parsing of character block OldPersian
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_OldPersian_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("OldPersian", 0x103DF);
		}

		/// <summary>
		/// Tests the parsing of character block Deseret
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Deseret_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Deseret", 0x10400);
		}

		/// <summary>
		/// Tests the parsing of character block Deseret
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Deseret_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Deseret", 0x1044F);
		}

		/// <summary>
		/// Tests the parsing of character block Shavian
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Shavian_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Shavian", 0x10450);
		}

		/// <summary>
		/// Tests the parsing of character block Shavian
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Shavian_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Shavian", 0x1047F);
		}

		/// <summary>
		/// Tests the parsing of character block Osmanya
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Osmanya_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Osmanya", 0x10480);
		}

		/// <summary>
		/// Tests the parsing of character block Osmanya
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Osmanya_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Osmanya", 0x104AF);
		}

		/// <summary>
		/// Tests the parsing of character block CypriotSyllabary
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CypriotSyllabary_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CypriotSyllabary", 0x10800);
		}

		/// <summary>
		/// Tests the parsing of character block CypriotSyllabary
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CypriotSyllabary_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CypriotSyllabary", 0x1083F);
		}

		/// <summary>
		/// Tests the parsing of character block ImperialAramaic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_ImperialAramaic_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("ImperialAramaic", 0x10840);
		}

		/// <summary>
		/// Tests the parsing of character block ImperialAramaic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_ImperialAramaic_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("ImperialAramaic", 0x1085F);
		}

		/// <summary>
		/// Tests the parsing of character block Phoenician
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Phoenician_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Phoenician", 0x10900);
		}

		/// <summary>
		/// Tests the parsing of character block Phoenician
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Phoenician_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Phoenician", 0x1091F);
		}

		/// <summary>
		/// Tests the parsing of character block Lydian
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Lydian_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Lydian", 0x10920);
		}

		/// <summary>
		/// Tests the parsing of character block Lydian
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Lydian_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Lydian", 0x1093F);
		}

		/// <summary>
		/// Tests the parsing of character block MeroiticHieroglyphs
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MeroiticHieroglyphs_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("MeroiticHieroglyphs", 0x10980);
		}

		/// <summary>
		/// Tests the parsing of character block MeroiticHieroglyphs
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MeroiticHieroglyphs_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("MeroiticHieroglyphs", 0x1099F);
		}

		/// <summary>
		/// Tests the parsing of character block MeroiticCursive
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MeroiticCursive_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("MeroiticCursive", 0x109A0);
		}

		/// <summary>
		/// Tests the parsing of character block MeroiticCursive
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MeroiticCursive_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("MeroiticCursive", 0x109FF);
		}

		/// <summary>
		/// Tests the parsing of character block Kharoshthi
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Kharoshthi_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Kharoshthi", 0x10A00);
		}

		/// <summary>
		/// Tests the parsing of character block Kharoshthi
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Kharoshthi_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Kharoshthi", 0x10A5F);
		}

		/// <summary>
		/// Tests the parsing of character block OldSouthArabian
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_OldSouthArabian_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("OldSouthArabian", 0x10A60);
		}

		/// <summary>
		/// Tests the parsing of character block OldSouthArabian
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_OldSouthArabian_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("OldSouthArabian", 0x10A7F);
		}

		/// <summary>
		/// Tests the parsing of character block Avestan
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Avestan_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Avestan", 0x10B00);
		}

		/// <summary>
		/// Tests the parsing of character block Avestan
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Avestan_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Avestan", 0x10B3F);
		}

		/// <summary>
		/// Tests the parsing of character block InscriptionalParthian
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_InscriptionalParthian_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("InscriptionalParthian", 0x10B40);
		}

		/// <summary>
		/// Tests the parsing of character block InscriptionalParthian
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_InscriptionalParthian_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("InscriptionalParthian", 0x10B5F);
		}

		/// <summary>
		/// Tests the parsing of character block InscriptionalPahlavi
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_InscriptionalPahlavi_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("InscriptionalPahlavi", 0x10B60);
		}

		/// <summary>
		/// Tests the parsing of character block InscriptionalPahlavi
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_InscriptionalPahlavi_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("InscriptionalPahlavi", 0x10B7F);
		}

		/// <summary>
		/// Tests the parsing of character block OldTurkic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_OldTurkic_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("OldTurkic", 0x10C00);
		}

		/// <summary>
		/// Tests the parsing of character block OldTurkic
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_OldTurkic_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("OldTurkic", 0x10C4F);
		}

		/// <summary>
		/// Tests the parsing of character block RumiNumeralSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_RumiNumeralSymbols_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("RumiNumeralSymbols", 0x10E60);
		}

		/// <summary>
		/// Tests the parsing of character block RumiNumeralSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_RumiNumeralSymbols_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("RumiNumeralSymbols", 0x10E7F);
		}

		/// <summary>
		/// Tests the parsing of character block Brahmi
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Brahmi_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Brahmi", 0x11000);
		}

		/// <summary>
		/// Tests the parsing of character block Brahmi
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Brahmi_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Brahmi", 0x1107F);
		}

		/// <summary>
		/// Tests the parsing of character block Kaithi
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Kaithi_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Kaithi", 0x11080);
		}

		/// <summary>
		/// Tests the parsing of character block Kaithi
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Kaithi_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Kaithi", 0x110CF);
		}

		/// <summary>
		/// Tests the parsing of character block SoraSompeng
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SoraSompeng_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("SoraSompeng", 0x110D0);
		}

		/// <summary>
		/// Tests the parsing of character block SoraSompeng
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SoraSompeng_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("SoraSompeng", 0x110FF);
		}

		/// <summary>
		/// Tests the parsing of character block Chakma
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Chakma_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Chakma", 0x11100);
		}

		/// <summary>
		/// Tests the parsing of character block Chakma
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Chakma_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Chakma", 0x1114F);
		}

		/// <summary>
		/// Tests the parsing of character block Sharada
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Sharada_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Sharada", 0x11180);
		}

		/// <summary>
		/// Tests the parsing of character block Sharada
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Sharada_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Sharada", 0x111DF);
		}

		/// <summary>
		/// Tests the parsing of character block Takri
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Takri_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Takri", 0x11680);
		}

		/// <summary>
		/// Tests the parsing of character block Takri
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Takri_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Takri", 0x116CF);
		}

		/// <summary>
		/// Tests the parsing of character block Cuneiform
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Cuneiform_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Cuneiform", 0x12000);
		}

		/// <summary>
		/// Tests the parsing of character block Cuneiform
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Cuneiform_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Cuneiform", 0x123FF);
		}

		/// <summary>
		/// Tests the parsing of character block CuneiformNumbersandPunctuation
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CuneiformNumbersandPunctuation_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CuneiformNumbersandPunctuation", 0x12400);
		}

		/// <summary>
		/// Tests the parsing of character block CuneiformNumbersandPunctuation
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CuneiformNumbersandPunctuation_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CuneiformNumbersandPunctuation", 0x1247F);
		}

		/// <summary>
		/// Tests the parsing of character block EgyptianHieroglyphs
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_EgyptianHieroglyphs_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("EgyptianHieroglyphs", 0x13000);
		}

		/// <summary>
		/// Tests the parsing of character block EgyptianHieroglyphs
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_EgyptianHieroglyphs_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("EgyptianHieroglyphs", 0x1342F);
		}

		/// <summary>
		/// Tests the parsing of character block BamumSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_BamumSupplement_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("BamumSupplement", 0x16800);
		}

		/// <summary>
		/// Tests the parsing of character block BamumSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_BamumSupplement_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("BamumSupplement", 0x16A3F);
		}

		/// <summary>
		/// Tests the parsing of character block Miao
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Miao_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Miao", 0x16F00);
		}

		/// <summary>
		/// Tests the parsing of character block Miao
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Miao_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Miao", 0x16F9F);
		}

		/// <summary>
		/// Tests the parsing of character block KanaSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_KanaSupplement_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("KanaSupplement", 0x1B000);
		}

		/// <summary>
		/// Tests the parsing of character block KanaSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_KanaSupplement_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("KanaSupplement", 0x1B0FF);
		}

		/// <summary>
		/// Tests the parsing of character block ByzantineMusicalSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_ByzantineMusicalSymbols_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("ByzantineMusicalSymbols", 0x1D000);
		}

		/// <summary>
		/// Tests the parsing of character block ByzantineMusicalSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_ByzantineMusicalSymbols_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("ByzantineMusicalSymbols", 0x1D0FF);
		}

		/// <summary>
		/// Tests the parsing of character block MusicalSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MusicalSymbols_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("MusicalSymbols", 0x1D100);
		}

		/// <summary>
		/// Tests the parsing of character block MusicalSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MusicalSymbols_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("MusicalSymbols", 0x1D1FF);
		}

		/// <summary>
		/// Tests the parsing of character block AncientGreekMusicalNotation
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_AncientGreekMusicalNotation_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("AncientGreekMusicalNotation", 0x1D200);
		}

		/// <summary>
		/// Tests the parsing of character block AncientGreekMusicalNotation
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_AncientGreekMusicalNotation_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("AncientGreekMusicalNotation", 0x1D24F);
		}

		/// <summary>
		/// Tests the parsing of character block TaiXuanJingSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_TaiXuanJingSymbols_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("TaiXuanJingSymbols", 0x1D300);
		}

		/// <summary>
		/// Tests the parsing of character block TaiXuanJingSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_TaiXuanJingSymbols_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("TaiXuanJingSymbols", 0x1D35F);
		}

		/// <summary>
		/// Tests the parsing of character block CountingRodNumerals
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CountingRodNumerals_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CountingRodNumerals", 0x1D360);
		}

		/// <summary>
		/// Tests the parsing of character block CountingRodNumerals
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CountingRodNumerals_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CountingRodNumerals", 0x1D37F);
		}

		/// <summary>
		/// Tests the parsing of character block MathematicalAlphanumericSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MathematicalAlphanumericSymbols_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("MathematicalAlphanumericSymbols", 0x1D400);
		}

		/// <summary>
		/// Tests the parsing of character block MathematicalAlphanumericSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MathematicalAlphanumericSymbols_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("MathematicalAlphanumericSymbols", 0x1D7FF);
		}

		/// <summary>
		/// Tests the parsing of character block ArabicMathematicalAlphabeticSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_ArabicMathematicalAlphabeticSymbols_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("ArabicMathematicalAlphabeticSymbols", 0x1EE00);
		}

		/// <summary>
		/// Tests the parsing of character block ArabicMathematicalAlphabeticSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_ArabicMathematicalAlphabeticSymbols_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("ArabicMathematicalAlphabeticSymbols", 0x1EEFF);
		}

		/// <summary>
		/// Tests the parsing of character block MahjongTiles
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MahjongTiles_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("MahjongTiles", 0x1F000);
		}

		/// <summary>
		/// Tests the parsing of character block MahjongTiles
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MahjongTiles_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("MahjongTiles", 0x1F02F);
		}

		/// <summary>
		/// Tests the parsing of character block DominoTiles
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_DominoTiles_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("DominoTiles", 0x1F030);
		}

		/// <summary>
		/// Tests the parsing of character block DominoTiles
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_DominoTiles_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("DominoTiles", 0x1F09F);
		}

		/// <summary>
		/// Tests the parsing of character block PlayingCards
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_PlayingCards_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("PlayingCards", 0x1F0A0);
		}

		/// <summary>
		/// Tests the parsing of character block PlayingCards
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_PlayingCards_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("PlayingCards", 0x1F0FF);
		}

		/// <summary>
		/// Tests the parsing of character block EnclosedAlphanumericSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_EnclosedAlphanumericSupplement_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("EnclosedAlphanumericSupplement", 0x1F100);
		}

		/// <summary>
		/// Tests the parsing of character block EnclosedAlphanumericSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_EnclosedAlphanumericSupplement_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("EnclosedAlphanumericSupplement", 0x1F1FF);
		}

		/// <summary>
		/// Tests the parsing of character block EnclosedIdeographicSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_EnclosedIdeographicSupplement_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("EnclosedIdeographicSupplement", 0x1F200);
		}

		/// <summary>
		/// Tests the parsing of character block EnclosedIdeographicSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_EnclosedIdeographicSupplement_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("EnclosedIdeographicSupplement", 0x1F2FF);
		}

		/// <summary>
		/// Tests the parsing of character block MiscellaneousSymbolsAndPictographs
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MiscellaneousSymbolsAndPictographs_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("MiscellaneousSymbolsAndPictographs", 0x1F300);
		}

		/// <summary>
		/// Tests the parsing of character block MiscellaneousSymbolsAndPictographs
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_MiscellaneousSymbolsAndPictographs_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("MiscellaneousSymbolsAndPictographs", 0x1F5FF);
		}

		/// <summary>
		/// Tests the parsing of character block Emoticons
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Emoticons_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Emoticons", 0x1F600);
		}

		/// <summary>
		/// Tests the parsing of character block Emoticons
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Emoticons_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Emoticons", 0x1F64F);
		}

		/// <summary>
		/// Tests the parsing of character block TransportAndMapSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_TransportAndMapSymbols_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("TransportAndMapSymbols", 0x1F680);
		}

		/// <summary>
		/// Tests the parsing of character block TransportAndMapSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_TransportAndMapSymbols_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("TransportAndMapSymbols", 0x1F6FF);
		}

		/// <summary>
		/// Tests the parsing of character block AlchemicalSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_AlchemicalSymbols_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("AlchemicalSymbols", 0x1F700);
		}

		/// <summary>
		/// Tests the parsing of character block AlchemicalSymbols
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_AlchemicalSymbols_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("AlchemicalSymbols", 0x1F77F);
		}

		/// <summary>
		/// Tests the parsing of character block CJKUnifiedIdeographsExtensionB
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKUnifiedIdeographsExtensionB_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CJKUnifiedIdeographsExtensionB", 0x20000);
		}

		/// <summary>
		/// Tests the parsing of character block CJKUnifiedIdeographsExtensionB
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKUnifiedIdeographsExtensionB_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CJKUnifiedIdeographsExtensionB", 0x2A6DF);
		}

		/// <summary>
		/// Tests the parsing of character block CJKUnifiedIdeographsExtensionC
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKUnifiedIdeographsExtensionC_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CJKUnifiedIdeographsExtensionC", 0x2A700);
		}

		/// <summary>
		/// Tests the parsing of character block CJKUnifiedIdeographsExtensionC
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKUnifiedIdeographsExtensionC_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CJKUnifiedIdeographsExtensionC", 0x2B73F);
		}

		/// <summary>
		/// Tests the parsing of character block CJKUnifiedIdeographsExtensionD
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKUnifiedIdeographsExtensionD_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CJKUnifiedIdeographsExtensionD", 0x2B740);
		}

		/// <summary>
		/// Tests the parsing of character block CJKUnifiedIdeographsExtensionD
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKUnifiedIdeographsExtensionD_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CJKUnifiedIdeographsExtensionD", 0x2B81F);
		}

		/// <summary>
		/// Tests the parsing of character block CJKCompatibilityIdeographsSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKCompatibilityIdeographsSupplement_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CJKCompatibilityIdeographsSupplement", 0x2F800);
		}

		/// <summary>
		/// Tests the parsing of character block CJKCompatibilityIdeographsSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_CJKCompatibilityIdeographsSupplement_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("CJKCompatibilityIdeographsSupplement", 0x2FA1F);
		}

		/// <summary>
		/// Tests the parsing of character block Tags
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Tags_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Tags", 0xE0000);
		}

		/// <summary>
		/// Tests the parsing of character block Tags
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_Tags_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("Tags", 0xE007F);
		}

		/// <summary>
		/// Tests the parsing of character block VariationSelectorsSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_VariationSelectorsSupplement_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("VariationSelectorsSupplement", 0xE0100);
		}

		/// <summary>
		/// Tests the parsing of character block VariationSelectorsSupplement
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_VariationSelectorsSupplement_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("VariationSelectorsSupplement", 0xE01EF);
		}

		/// <summary>
		/// Tests the parsing of character block SupplementaryPrivateUseArea-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SupplementaryPrivateUseAreaA_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("SupplementaryPrivateUseArea-A", 0xF0000);
		}

		/// <summary>
		/// Tests the parsing of character block SupplementaryPrivateUseArea-A
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SupplementaryPrivateUseAreaA_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("SupplementaryPrivateUseArea-A", 0xFFFFF);
		}

		/// <summary>
		/// Tests the parsing of character block SupplementaryPrivateUseArea-B
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SupplementaryPrivateUseAreaB_LeftBound()
		{
			SetTestDirectory();
			Test_WithinBlock("SupplementaryPrivateUseArea-B", 0x100000);
		}

		/// <summary>
		/// Tests the parsing of character block SupplementaryPrivateUseArea-B
		/// </summary>
		[Test]
		public void Test_UnicodeBlock_SupplementaryPrivateUseAreaB_RightBound()
		{
			SetTestDirectory();
			Test_WithinBlock("SupplementaryPrivateUseArea-B", 0x10FFFF);
		}
	}
}