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

using System.Collections.Generic;
using Hime.CentralDogma.Automata;

namespace Hime.CentralDogma
{
	/// <summary>
	/// Represents a Unicode block of characters
	/// </summary>
    public class UnicodeBlock
    {
		/// <summary>
		/// The block's name
		/// </summary>
        private string name;

		/// <summary>
		/// The block's character span
		/// </summary>
		private UnicodeSpan span;

		/// <summary>
		/// Get this block's name
		/// </summary>
        public string Name { get { return name; } }

		/// <summary>
        /// Gets the span of this block
        /// </summary>
        public UnicodeSpan Span { get { return span; } }

		/// <summary>
		/// Initializes this Unicode block
		/// </summary>
		/// <param name="name">Block's name</param>
		/// <param name="begin">Beginning character (included)</param>
		/// <param name="end">End character (included)</param>
        private UnicodeBlock(string name, int begin, int end)
		{
            this.name = name;
			this.span = new UnicodeSpan(begin, end);
        }

		/// <summary>
        /// Gets the string representation of this block
        /// </summary>
        /// <returns>The string representation</returns>
        public override string ToString()
        {
            return name + " " + span.ToString();
        }

		/// <summary>
		/// Cache for the standard Unicode blocks (plane 0)
		/// </summary>
		private static Dictionary<string, UnicodeBlock> blocks = null;

		/// <summary>
		/// Gets the supported Unicode blocks
		/// </summary>
		/// <returns>The supported Unicode blocks</returns>
		public static ICollection<UnicodeBlock> GetBlocks()
		{
			if (blocks == null)
				BuildBlocks();
			return blocks.Values;
		}

		/// <summary>
		/// Gets the block with the given name
		/// </summary>
		/// <param name="name">A Unicode block name</param>
		/// <returns>The corresponding block, or null if it does not exists</returns>
		public static UnicodeBlock GetBlock(string name)
		{
			if (blocks == null)
				BuildBlocks();
			if (blocks.ContainsKey(name))
				return blocks[name];
			return null;
		}

		/// <summary>
		/// Builds the standard Unicode blocks
		/// </summary>
		private static void BuildBlocks()
		{
			blocks = new Dictionary<string, UnicodeBlock>();
			blocks.Add("BasicLatin", new UnicodeBlock("BasicLatin", 0x0000, 0x007F));
			blocks.Add("Latin-1Supplement", new UnicodeBlock("Latin-1Supplement", 0x0080, 0x00FF));
			blocks.Add("LatinExtended-A", new UnicodeBlock("LatinExtended-A", 0x0100, 0x017F));
			blocks.Add("LatinExtended-B", new UnicodeBlock("LatinExtended-B", 0x0180, 0x024F));
			blocks.Add("IPAExtensions", new UnicodeBlock("IPAExtensions", 0x0250, 0x02AF));
			blocks.Add("SpacingModifierLetters", new UnicodeBlock("SpacingModifierLetters", 0x02B0, 0x02FF));
			blocks.Add("CombiningDiacriticalMarks", new UnicodeBlock("CombiningDiacriticalMarks", 0x0300, 0x036F));
			blocks.Add("GreekandCoptic", new UnicodeBlock("GreekandCoptic", 0x0370, 0x03FF));
			blocks.Add("Cyrillic", new UnicodeBlock("Cyrillic", 0x0400, 0x04FF));
			blocks.Add("CyrillicSupplement", new UnicodeBlock("CyrillicSupplement", 0x0500, 0x052F));
			blocks.Add("Armenian", new UnicodeBlock("Armenian", 0x0530, 0x058F));
			blocks.Add("Hebrew", new UnicodeBlock("Hebrew", 0x0590, 0x05FF));
			blocks.Add("Arabic", new UnicodeBlock("Arabic", 0x0600, 0x06FF));
			blocks.Add("Syriac", new UnicodeBlock("Syriac", 0x0700, 0x074F));
			blocks.Add("ArabicSupplement", new UnicodeBlock("ArabicSupplement", 0x0750, 0x077F));
			blocks.Add("Thaana", new UnicodeBlock("Thaana", 0x0780, 0x07BF));
			blocks.Add("NKo", new UnicodeBlock("NKo", 0x07C0, 0x07FF));
			blocks.Add("Samaritan", new UnicodeBlock("Samaritan", 0x0800, 0x083F));
			blocks.Add("Mandaic", new UnicodeBlock("Mandaic", 0x0840, 0x085F));
			blocks.Add("ArabicExtended-A", new UnicodeBlock("ArabicExtended-A", 0x08A0, 0x08FF));
			blocks.Add("Devanagari", new UnicodeBlock("Devanagari", 0x0900, 0x097F));
			blocks.Add("Bengali", new UnicodeBlock("Bengali", 0x0980, 0x09FF));
			blocks.Add("Gurmukhi", new UnicodeBlock("Gurmukhi", 0x0A00, 0x0A7F));
			blocks.Add("Gujarati", new UnicodeBlock("Gujarati", 0x0A80, 0x0AFF));
			blocks.Add("Oriya", new UnicodeBlock("Oriya", 0x0B00, 0x0B7F));
			blocks.Add("Tamil", new UnicodeBlock("Tamil", 0x0B80, 0x0BFF));
			blocks.Add("Telugu", new UnicodeBlock("Telugu", 0x0C00, 0x0C7F));
			blocks.Add("Kannada", new UnicodeBlock("Kannada", 0x0C80, 0x0CFF));
			blocks.Add("Malayalam", new UnicodeBlock("Malayalam", 0x0D00, 0x0D7F));
			blocks.Add("Sinhala", new UnicodeBlock("Sinhala", 0x0D80, 0x0DFF));
			blocks.Add("Thai", new UnicodeBlock("Thai", 0x0E00, 0x0E7F));
			blocks.Add("Lao", new UnicodeBlock("Lao", 0x0E80, 0x0EFF));
			blocks.Add("Tibetan", new UnicodeBlock("Tibetan", 0x0F00, 0x0FFF));
			blocks.Add("Myanmar", new UnicodeBlock("Myanmar", 0x1000, 0x109F));
			blocks.Add("Georgian", new UnicodeBlock("Georgian", 0x10A0, 0x10FF));
			blocks.Add("HangulJamo", new UnicodeBlock("HangulJamo", 0x1100, 0x11FF));
			blocks.Add("Ethiopic", new UnicodeBlock("Ethiopic", 0x1200, 0x137F));
			blocks.Add("EthiopicSupplement", new UnicodeBlock("EthiopicSupplement", 0x1380, 0x139F));
			blocks.Add("Cherokee", new UnicodeBlock("Cherokee", 0x13A0, 0x13FF));
			blocks.Add("UnifiedCanadianAboriginalSyllabics", new UnicodeBlock("UnifiedCanadianAboriginalSyllabics", 0x1400, 0x167F));
			blocks.Add("Ogham", new UnicodeBlock("Ogham", 0x1680, 0x169F));
			blocks.Add("Runic", new UnicodeBlock("Runic", 0x16A0, 0x16FF));
			blocks.Add("Tagalog", new UnicodeBlock("Tagalog", 0x1700, 0x171F));
			blocks.Add("Hanunoo", new UnicodeBlock("Hanunoo", 0x1720, 0x173F));
			blocks.Add("Buhid", new UnicodeBlock("Buhid", 0x1740, 0x175F));
			blocks.Add("Tagbanwa", new UnicodeBlock("Tagbanwa", 0x1760, 0x177F));
			blocks.Add("Khmer", new UnicodeBlock("Khmer", 0x1780, 0x17FF));
			blocks.Add("Mongolian", new UnicodeBlock("Mongolian", 0x1800, 0x18AF));
			blocks.Add("UnifiedCanadianAboriginalSyllabicsExtended", new UnicodeBlock("UnifiedCanadianAboriginalSyllabicsExtended", 0x18B0, 0x18FF));
			blocks.Add("Limbu", new UnicodeBlock("Limbu", 0x1900, 0x194F));
			blocks.Add("TaiLe", new UnicodeBlock("TaiLe", 0x1950, 0x197F));
			blocks.Add("NewTaiLue", new UnicodeBlock("NewTaiLue", 0x1980, 0x19DF));
			blocks.Add("KhmerSymbols", new UnicodeBlock("KhmerSymbols", 0x19E0, 0x19FF));
			blocks.Add("Buginese", new UnicodeBlock("Buginese", 0x1A00, 0x1A1F));
			blocks.Add("TaiTham", new UnicodeBlock("TaiTham", 0x1A20, 0x1AAF));
			blocks.Add("Balinese", new UnicodeBlock("Balinese", 0x1B00, 0x1B7F));
			blocks.Add("Sundanese", new UnicodeBlock("Sundanese", 0x1B80, 0x1BBF));
			blocks.Add("Batak", new UnicodeBlock("Batak", 0x1BC0, 0x1BFF));
			blocks.Add("Lepcha", new UnicodeBlock("Lepcha", 0x1C00, 0x1C4F));
			blocks.Add("OlChiki", new UnicodeBlock("OlChiki", 0x1C50, 0x1C7F));
			blocks.Add("SundaneseSupplement", new UnicodeBlock("SundaneseSupplement", 0x1CC0, 0x1CCF));
			blocks.Add("VedicExtensions", new UnicodeBlock("VedicExtensions", 0x1CD0, 0x1CFF));
			blocks.Add("PhoneticExtensions", new UnicodeBlock("PhoneticExtensions", 0x1D00, 0x1D7F));
			blocks.Add("PhoneticExtensionsSupplement", new UnicodeBlock("PhoneticExtensionsSupplement", 0x1D80, 0x1DBF));
			blocks.Add("CombiningDiacriticalMarksSupplement", new UnicodeBlock("CombiningDiacriticalMarksSupplement", 0x1DC0, 0x1DFF));
			blocks.Add("LatinExtendedAdditional", new UnicodeBlock("LatinExtendedAdditional", 0x1E00, 0x1EFF));
			blocks.Add("GreekExtended", new UnicodeBlock("GreekExtended", 0x1F00, 0x1FFF));
			blocks.Add("GeneralPunctuation", new UnicodeBlock("GeneralPunctuation", 0x2000, 0x206F));
			blocks.Add("SuperscriptsAndSubscripts", new UnicodeBlock("SuperscriptsAndSubscripts", 0x2070, 0x209F));
			blocks.Add("CurrencySymbols", new UnicodeBlock("CurrencySymbols", 0x20A0, 0x20CF));
			blocks.Add("CombiningDiacriticalMarksforSymbols", new UnicodeBlock("CombiningDiacriticalMarksforSymbols", 0x20D0, 0x20FF));
			blocks.Add("LetterlikeSymbols", new UnicodeBlock("LetterlikeSymbols", 0x2100, 0x214F));
			blocks.Add("NumberForms", new UnicodeBlock("NumberForms", 0x2150, 0x218F));
			blocks.Add("Arrows", new UnicodeBlock("Arrows", 0x2190, 0x21FF));
			blocks.Add("MathematicalOperators", new UnicodeBlock("MathematicalOperators", 0x2200, 0x22FF));
			blocks.Add("MiscellaneousTechnical", new UnicodeBlock("MiscellaneousTechnical", 0x2300, 0x23FF));
			blocks.Add("ControlPictures", new UnicodeBlock("ControlPictures", 0x2400, 0x243F));
			blocks.Add("OpticalCharacterRecognition", new UnicodeBlock("OpticalCharacterRecognition", 0x2440, 0x245F));
			blocks.Add("EnclosedAlphanumerics", new UnicodeBlock("EnclosedAlphanumerics", 0x2460, 0x24FF));
			blocks.Add("BoxDrawing", new UnicodeBlock("BoxDrawing", 0x2500, 0x257F));
			blocks.Add("BlockElements", new UnicodeBlock("BlockElements", 0x2580, 0x259F));
			blocks.Add("GeometricShapes", new UnicodeBlock("GeometricShapes", 0x25A0, 0x25FF));
			blocks.Add("MiscellaneousSymbols", new UnicodeBlock("MiscellaneousSymbols", 0x2600, 0x26FF));
			blocks.Add("Dingbats", new UnicodeBlock("Dingbats", 0x2700, 0x27BF));
			blocks.Add("MiscellaneousMathematicalSymbols-A", new UnicodeBlock("MiscellaneousMathematicalSymbols-A", 0x27C0, 0x27EF));
			blocks.Add("SupplementalArrows-A", new UnicodeBlock("SupplementalArrows-A", 0x27F0, 0x27FF));
			blocks.Add("BraillePatterns", new UnicodeBlock("BraillePatterns", 0x2800, 0x28FF));
			blocks.Add("SupplementalArrows-B", new UnicodeBlock("SupplementalArrows-B", 0x2900, 0x297F));
			blocks.Add("MiscellaneousMathematicalSymbols-B", new UnicodeBlock("MiscellaneousMathematicalSymbols-B", 0x2980, 0x29FF));
			blocks.Add("SupplementalMathematicalOperators", new UnicodeBlock("SupplementalMathematicalOperators", 0x2A00, 0x2AFF));
			blocks.Add("MiscellaneousSymbolsandArrows", new UnicodeBlock("MiscellaneousSymbolsandArrows", 0x2B00, 0x2BFF));
			blocks.Add("Glagolitic", new UnicodeBlock("Glagolitic", 0x2C00, 0x2C5F));
			blocks.Add("LatinExtended-C", new UnicodeBlock("LatinExtended-C", 0x2C60, 0x2C7F));
			blocks.Add("Coptic", new UnicodeBlock("Coptic", 0x2C80, 0x2CFF));
			blocks.Add("GeorgianSupplement", new UnicodeBlock("GeorgianSupplement", 0x2D00, 0x2D2F));
			blocks.Add("Tifinagh", new UnicodeBlock("Tifinagh", 0x2D30, 0x2D7F));
			blocks.Add("EthiopicExtended", new UnicodeBlock("EthiopicExtended", 0x2D80, 0x2DDF));
			blocks.Add("CyrillicExtended-A", new UnicodeBlock("CyrillicExtended-A", 0x2DE0, 0x2DFF));
			blocks.Add("SupplementalPunctuation", new UnicodeBlock("SupplementalPunctuation", 0x2E00, 0x2E7F));
			blocks.Add("CJKRadicalsSupplement", new UnicodeBlock("CJKRadicalsSupplement", 0x2E80, 0x2EFF));
			blocks.Add("KangxiRadicals", new UnicodeBlock("KangxiRadicals", 0x2F00, 0x2FDF));
			blocks.Add("IdeographicDescriptionCharacters", new UnicodeBlock("IdeographicDescriptionCharacters", 0x2FF0, 0x2FFF));
			blocks.Add("CJKSymbolsandPunctuation", new UnicodeBlock("CJKSymbolsandPunctuation", 0x3000, 0x303F));
			blocks.Add("Hiragana", new UnicodeBlock("Hiragana", 0x3040, 0x309F));
			blocks.Add("Katakana", new UnicodeBlock("Katakana", 0x30A0, 0x30FF));
			blocks.Add("Bopomofo", new UnicodeBlock("Bopomofo", 0x3100, 0x312F));
			blocks.Add("HangulCompatibilityJamo", new UnicodeBlock("HangulCompatibilityJamo", 0x3130, 0x318F));
			blocks.Add("Kanbun", new UnicodeBlock("Kanbun", 0x3190, 0x319F));
			blocks.Add("BopomofoExtended", new UnicodeBlock("BopomofoExtended", 0x31A0, 0x31BF));
			blocks.Add("CJKStrokes", new UnicodeBlock("CJKStrokes", 0x31C0, 0x31EF));
			blocks.Add("KatakanaPhoneticExtensions", new UnicodeBlock("KatakanaPhoneticExtensions", 0x31F0, 0x31FF));
			blocks.Add("EnclosedCJKLettersandMonths", new UnicodeBlock("EnclosedCJKLettersandMonths", 0x3200, 0x32FF));
			blocks.Add("CJKCompatibility", new UnicodeBlock("CJKCompatibility", 0x3300, 0x33FF));
			blocks.Add("CJKUnifiedIdeographsExtensionA", new UnicodeBlock("CJKUnifiedIdeographsExtensionA", 0x3400, 0x4DBF));
			blocks.Add("YijingHexagramSymbols", new UnicodeBlock("YijingHexagramSymbols", 0x4DC0, 0x4DFF));
			blocks.Add("CJKUnifiedIdeographs", new UnicodeBlock("CJKUnifiedIdeographs", 0x4E00, 0x9FFF));
			blocks.Add("YiSyllables", new UnicodeBlock("YiSyllables", 0xA000, 0xA48F));
			blocks.Add("YiRadicals", new UnicodeBlock("YiRadicals", 0xA490, 0xA4CF));
			blocks.Add("Lisu", new UnicodeBlock("Lisu", 0xA4D0, 0xA4FF));
			blocks.Add("Vai", new UnicodeBlock("Vai", 0xA500, 0xA63F));
			blocks.Add("CyrillicExtended-B", new UnicodeBlock("CyrillicExtended-B", 0xA640, 0xA69F));
			blocks.Add("Bamum", new UnicodeBlock("Bamum", 0xA6A0, 0xA6FF));
			blocks.Add("ModifierToneLetters", new UnicodeBlock("ModifierToneLetters", 0xA700, 0xA71F));
			blocks.Add("LatinExtended-D", new UnicodeBlock("LatinExtended-D", 0xA720, 0xA7FF));
			blocks.Add("SylotiNagri", new UnicodeBlock("SylotiNagri", 0xA800, 0xA82F));
			blocks.Add("CommonIndicNumberForms", new UnicodeBlock("CommonIndicNumberForms", 0xA830, 0xA83F));
			blocks.Add("Phags-pa", new UnicodeBlock("Phags-pa", 0xA840, 0xA87F));
			blocks.Add("Saurashtra", new UnicodeBlock("Saurashtra", 0xA880, 0xA8DF));
			blocks.Add("DevanagariExtended", new UnicodeBlock("DevanagariExtended", 0xA8E0, 0xA8FF));
			blocks.Add("KayahLi", new UnicodeBlock("KayahLi", 0xA900, 0xA92F));
			blocks.Add("Rejang", new UnicodeBlock("Rejang", 0xA930, 0xA95F));
			blocks.Add("HangulJamoExtended-A", new UnicodeBlock("HangulJamoExtended-A", 0xA960, 0xA97F));
			blocks.Add("Javanese", new UnicodeBlock("Javanese", 0xA980, 0xA9DF));
			blocks.Add("Cham", new UnicodeBlock("Cham", 0xAA00, 0xAA5F));
			blocks.Add("MyanmarExtended-A", new UnicodeBlock("MyanmarExtended-A", 0xAA60, 0xAA7F));
			blocks.Add("TaiViet", new UnicodeBlock("TaiViet", 0xAA80, 0xAADF));
			blocks.Add("MeeteiMayekExtensions", new UnicodeBlock("MeeteiMayekExtensions", 0xAAE0, 0xAAFF));
			blocks.Add("EthiopicExtended-A", new UnicodeBlock("EthiopicExtended-A", 0xAB00, 0xAB2F));
			blocks.Add("MeeteiMayek", new UnicodeBlock("MeeteiMayek", 0xABC0, 0xABFF));
			blocks.Add("HangulSyllables", new UnicodeBlock("HangulSyllables", 0xAC00, 0xD7AF));
			blocks.Add("HangulJamoExtended-B", new UnicodeBlock("HangulJamoExtended-B", 0xD7B0, 0xD7FF));
			//blocks.Add("HighSurrogates", new UnicodeBlock("HighSurrogates", 0xD800, 0xDB7F));
			//blocks.Add("HighPrivateUseSurrogates", new UnicodeBlock("HighPrivateUseSurrogates", 0xDB80, 0xDBFF));
			//blocks.Add("LowSurrogates", new UnicodeBlock("LowSurrogates", 0xDC00, 0xDFFF));
			blocks.Add("PrivateUseArea", new UnicodeBlock("PrivateUseArea", 0xE000, 0xF8FF));
			blocks.Add("CJKCompatibilityIdeographs", new UnicodeBlock("CJKCompatibilityIdeographs", 0xF900, 0xFAFF));
			blocks.Add("AlphabeticPresentationForms", new UnicodeBlock("AlphabeticPresentationForms", 0xFB00, 0xFB4F));
			blocks.Add("ArabicPresentationForms-A", new UnicodeBlock("ArabicPresentationForms-A", 0xFB50, 0xFDFF));
			blocks.Add("VariationSelectors", new UnicodeBlock("VariationSelectors", 0xFE00, 0xFE0F));
			blocks.Add("VerticalForms", new UnicodeBlock("VerticalForms", 0xFE10, 0xFE1F));
			blocks.Add("CombiningHalfMarks", new UnicodeBlock("CombiningHalfMarks", 0xFE20, 0xFE2F));
			blocks.Add("CJKCompatibilityForms", new UnicodeBlock("CJKCompatibilityForms", 0xFE30, 0xFE4F));
			blocks.Add("SmallFormVariants", new UnicodeBlock("SmallFormVariants", 0xFE50, 0xFE6F));
			blocks.Add("ArabicPresentationForms-B", new UnicodeBlock("ArabicPresentationForms-B", 0xFE70, 0xFEFF));
			blocks.Add("HalfwidthAndFullwidthForms", new UnicodeBlock("HalfwidthAndFullwidthForms", 0xFF00, 0xFFEF));
			blocks.Add("Specials", new UnicodeBlock("Specials", 0xFFF0, 0xFFFF));
		}
    }
}