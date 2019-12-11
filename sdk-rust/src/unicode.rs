/*******************************************************************************
 * Copyright (c) 2019 Association Cénotélie (cenotelie.fr)
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
 ******************************************************************************/

//! Unicode support

use std::collections::HashMap;

/// Represents a Unicode code point
#[derive(Debug, Copy, Clone, Eq, PartialEq, PartialOrd, Ord, Hash)]
pub struct CodePoint(u32);

impl CodePoint {
    /// Initializes the code point
    pub fn new(value: u32) -> CodePoint {
        if (value >= 0xD800 && value <= 0xDFFF) || value >= 0x0011_0000 {
            panic!("The value is not a valid Unicode character code point");
        }
        CodePoint(value)
    }

    /// Gets the code point value
    pub fn value(self) -> u32 {
        self.0
    }

    /// Gets a value indicating whether this codepoint is in Unicode plane 0
    pub fn is_plane0(self) -> bool {
        self.0 <= 0xFFFF
    }

    /// Gets the UTF-16 encoding of this code point
    pub fn get_utf16(self) -> [u16; 2] {
        if self.0 <= 0xFFFF {
            // plane 0
            return [self.0 as u16, 0];
        }
        let temp = self.0 - 0x10000;
        let lead = (temp >> 10) + 0xD800;
        let trail = (temp & 0x03FF) + 0xDC00;
        [lead as u16, trail as u16]
    }
}

/// Represents a range of Unicode characters
#[derive(Debug, Copy, Clone, Eq, PartialEq, Hash)]
pub struct Span {
    /// Beginning of the range (included)
    pub begin: CodePoint,
    /// End of the range (included)
    pub end: CodePoint
}

impl Span {
    /// Initializes this character span
    pub fn new(begin: u32, end: u32) -> Span {
        Span {
            begin: CodePoint::new(begin),
            end: CodePoint::new(end)
        }
    }

    /// Gets the range's length in number of characters
    pub fn len(self) -> u32 {
        self.end.0 - self.begin.0 + 1
    }

    /// Gets whether the range is empty
    pub fn is_empty(self) -> bool {
        self.end.0 < self.begin.0
    }

    /// Gets a value indicating whether this codepoint is in Unicode plane 0
    pub fn is_plane0(self) -> bool {
        self.begin.is_plane0() && self.end.is_plane0()
    }
}

/// Represents a Unicode block of characters
#[derive(Debug, Clone)]
pub struct Block {
    /// The block's name
    pub name: &'static str,
    /// The block's character span
    pub span: Span
}

impl Block {
    /// Initializes this Unicode block
    pub fn new(name: &'static str, begin: u32, end: u32) -> Block {
        Block {
            name,
            span: Span::new(begin, end)
        }
    }
}

/// Represents a Unicode category
#[derive(Debug, Clone)]
pub struct Category {
    /// The category's name
    pub name: &'static str,
    /// The list of character spans contained in this category
    pub spans: Vec<Span>
}

impl Category {
    /// Represents a Unicode category
    pub fn new(name: &'static str) -> Category {
        Category {
            name,
            spans: Vec::new()
        }
    }

    /// Adds a span to this category
    pub fn add_span(&mut self, begin: u32, end: u32) {
        self.spans.push(Span::new(begin, end));
    }

    /// Aggregate the specified category into this one
    pub fn aggregate(&mut self, category: &Category) {
        self.spans.copy_from_slice(&category.spans);
    }
}

/// Gets all blocks
pub fn get_blocks() -> HashMap<&'static str, Block> {
    let mut db = HashMap::new();
    db.insert("BasicLatin", Block::new("BasicLatin", 0x0, 0x7F));
    db.insert(
        "Latin-1Supplement",
        Block::new("Latin-1Supplement", 0x80, 0xFF)
    );
    db.insert(
        "LatinExtended-A",
        Block::new("LatinExtended-A", 0x100, 0x17F)
    );
    db.insert(
        "LatinExtended-B",
        Block::new("LatinExtended-B", 0x180, 0x24F)
    );
    db.insert("IPAExtensions", Block::new("IPAExtensions", 0x250, 0x2AF));
    db.insert(
        "SpacingModifierLetters",
        Block::new("SpacingModifierLetters", 0x2B0, 0x2FF)
    );
    db.insert(
        "CombiningDiacriticalMarks",
        Block::new("CombiningDiacriticalMarks", 0x300, 0x36F)
    );
    db.insert("GreekandCoptic", Block::new("GreekandCoptic", 0x370, 0x3FF));
    db.insert("Cyrillic", Block::new("Cyrillic", 0x400, 0x4FF));
    db.insert(
        "CyrillicSupplement",
        Block::new("CyrillicSupplement", 0x500, 0x52F)
    );
    db.insert("Armenian", Block::new("Armenian", 0x530, 0x58F));
    db.insert("Hebrew", Block::new("Hebrew", 0x590, 0x5FF));
    db.insert("Arabic", Block::new("Arabic", 0x600, 0x6FF));
    db.insert("Syriac", Block::new("Syriac", 0x700, 0x74F));
    db.insert(
        "ArabicSupplement",
        Block::new("ArabicSupplement", 0x750, 0x77F)
    );
    db.insert("Thaana", Block::new("Thaana", 0x780, 0x7BF));
    db.insert("NKo", Block::new("NKo", 0x7C0, 0x7FF));
    db.insert("Samaritan", Block::new("Samaritan", 0x800, 0x83F));
    db.insert("Mandaic", Block::new("Mandaic", 0x840, 0x85F));
    db.insert(
        "SyriacSupplement",
        Block::new("SyriacSupplement", 0x860, 0x86F)
    );
    db.insert(
        "ArabicExtended-A",
        Block::new("ArabicExtended-A", 0x8A0, 0x8FF)
    );
    db.insert("Devanagari", Block::new("Devanagari", 0x900, 0x97F));
    db.insert("Bengali", Block::new("Bengali", 0x980, 0x9FF));
    db.insert("Gurmukhi", Block::new("Gurmukhi", 0xA00, 0xA7F));
    db.insert("Gujarati", Block::new("Gujarati", 0xA80, 0xAFF));
    db.insert("Oriya", Block::new("Oriya", 0xB00, 0xB7F));
    db.insert("Tamil", Block::new("Tamil", 0xB80, 0xBFF));
    db.insert("Telugu", Block::new("Telugu", 0xC00, 0xC7F));
    db.insert("Kannada", Block::new("Kannada", 0xC80, 0xCFF));
    db.insert("Malayalam", Block::new("Malayalam", 0xD00, 0xD7F));
    db.insert("Sinhala", Block::new("Sinhala", 0xD80, 0xDFF));
    db.insert("Thai", Block::new("Thai", 0xE00, 0xE7F));
    db.insert("Lao", Block::new("Lao", 0xE80, 0xEFF));
    db.insert("Tibetan", Block::new("Tibetan", 0xF00, 0xFFF));
    db.insert("Myanmar", Block::new("Myanmar", 0x1000, 0x109F));
    db.insert("Georgian", Block::new("Georgian", 0x10A0, 0x10FF));
    db.insert("HangulJamo", Block::new("HangulJamo", 0x1100, 0x11FF));
    db.insert("Ethiopic", Block::new("Ethiopic", 0x1200, 0x137F));
    db.insert(
        "EthiopicSupplement",
        Block::new("EthiopicSupplement", 0x1380, 0x139F)
    );
    db.insert("Cherokee", Block::new("Cherokee", 0x13A0, 0x13FF));
    db.insert(
        "UnifiedCanadianAboriginalSyllabics",
        Block::new("UnifiedCanadianAboriginalSyllabics", 0x1400, 0x167F)
    );
    db.insert("Ogham", Block::new("Ogham", 0x1680, 0x169F));
    db.insert("Runic", Block::new("Runic", 0x16A0, 0x16FF));
    db.insert("Tagalog", Block::new("Tagalog", 0x1700, 0x171F));
    db.insert("Hanunoo", Block::new("Hanunoo", 0x1720, 0x173F));
    db.insert("Buhid", Block::new("Buhid", 0x1740, 0x175F));
    db.insert("Tagbanwa", Block::new("Tagbanwa", 0x1760, 0x177F));
    db.insert("Khmer", Block::new("Khmer", 0x1780, 0x17FF));
    db.insert("Mongolian", Block::new("Mongolian", 0x1800, 0x18AF));
    db.insert(
        "UnifiedCanadianAboriginalSyllabicsExtended",
        Block::new("UnifiedCanadianAboriginalSyllabicsExtended", 0x18B0, 0x18FF)
    );
    db.insert("Limbu", Block::new("Limbu", 0x1900, 0x194F));
    db.insert("TaiLe", Block::new("TaiLe", 0x1950, 0x197F));
    db.insert("NewTaiLue", Block::new("NewTaiLue", 0x1980, 0x19DF));
    db.insert("KhmerSymbols", Block::new("KhmerSymbols", 0x19E0, 0x19FF));
    db.insert("Buginese", Block::new("Buginese", 0x1A00, 0x1A1F));
    db.insert("TaiTham", Block::new("TaiTham", 0x1A20, 0x1AAF));
    db.insert(
        "CombiningDiacriticalMarksExtended",
        Block::new("CombiningDiacriticalMarksExtended", 0x1AB0, 0x1AFF)
    );
    db.insert("Balinese", Block::new("Balinese", 0x1B00, 0x1B7F));
    db.insert("Sundanese", Block::new("Sundanese", 0x1B80, 0x1BBF));
    db.insert("Batak", Block::new("Batak", 0x1BC0, 0x1BFF));
    db.insert("Lepcha", Block::new("Lepcha", 0x1C00, 0x1C4F));
    db.insert("OlChiki", Block::new("OlChiki", 0x1C50, 0x1C7F));
    db.insert(
        "CyrillicExtended-C",
        Block::new("CyrillicExtended-C", 0x1C80, 0x1C8F)
    );
    db.insert(
        "SundaneseSupplement",
        Block::new("SundaneseSupplement", 0x1CC0, 0x1CCF)
    );
    db.insert(
        "VedicExtensions",
        Block::new("VedicExtensions", 0x1CD0, 0x1CFF)
    );
    db.insert(
        "PhoneticExtensions",
        Block::new("PhoneticExtensions", 0x1D00, 0x1D7F)
    );
    db.insert(
        "PhoneticExtensionsSupplement",
        Block::new("PhoneticExtensionsSupplement", 0x1D80, 0x1DBF)
    );
    db.insert(
        "CombiningDiacriticalMarksSupplement",
        Block::new("CombiningDiacriticalMarksSupplement", 0x1DC0, 0x1DFF)
    );
    db.insert(
        "LatinExtendedAdditional",
        Block::new("LatinExtendedAdditional", 0x1E00, 0x1EFF)
    );
    db.insert("GreekExtended", Block::new("GreekExtended", 0x1F00, 0x1FFF));
    db.insert(
        "GeneralPunctuation",
        Block::new("GeneralPunctuation", 0x2000, 0x206F)
    );
    db.insert(
        "SuperscriptsandSubscripts",
        Block::new("SuperscriptsandSubscripts", 0x2070, 0x209F)
    );
    db.insert(
        "CurrencySymbols",
        Block::new("CurrencySymbols", 0x20A0, 0x20CF)
    );
    db.insert(
        "CombiningDiacriticalMarksforSymbols",
        Block::new("CombiningDiacriticalMarksforSymbols", 0x20D0, 0x20FF)
    );
    db.insert(
        "LetterlikeSymbols",
        Block::new("LetterlikeSymbols", 0x2100, 0x214F)
    );
    db.insert("NumberForms", Block::new("NumberForms", 0x2150, 0x218F));
    db.insert("Arrows", Block::new("Arrows", 0x2190, 0x21FF));
    db.insert(
        "MathematicalOperators",
        Block::new("MathematicalOperators", 0x2200, 0x22FF)
    );
    db.insert(
        "MiscellaneousTechnical",
        Block::new("MiscellaneousTechnical", 0x2300, 0x23FF)
    );
    db.insert(
        "ControlPictures",
        Block::new("ControlPictures", 0x2400, 0x243F)
    );
    db.insert(
        "OpticalCharacterRecognition",
        Block::new("OpticalCharacterRecognition", 0x2440, 0x245F)
    );
    db.insert(
        "EnclosedAlphanumerics",
        Block::new("EnclosedAlphanumerics", 0x2460, 0x24FF)
    );
    db.insert("BoxDrawing", Block::new("BoxDrawing", 0x2500, 0x257F));
    db.insert("BlockElements", Block::new("BlockElements", 0x2580, 0x259F));
    db.insert(
        "GeometricShapes",
        Block::new("GeometricShapes", 0x25A0, 0x25FF)
    );
    db.insert(
        "MiscellaneousSymbols",
        Block::new("MiscellaneousSymbols", 0x2600, 0x26FF)
    );
    db.insert("Dingbats", Block::new("Dingbats", 0x2700, 0x27BF));
    db.insert(
        "MiscellaneousMathematicalSymbols-A",
        Block::new("MiscellaneousMathematicalSymbols-A", 0x27C0, 0x27EF)
    );
    db.insert(
        "SupplementalArrows-A",
        Block::new("SupplementalArrows-A", 0x27F0, 0x27FF)
    );
    db.insert(
        "BraillePatterns",
        Block::new("BraillePatterns", 0x2800, 0x28FF)
    );
    db.insert(
        "SupplementalArrows-B",
        Block::new("SupplementalArrows-B", 0x2900, 0x297F)
    );
    db.insert(
        "MiscellaneousMathematicalSymbols-B",
        Block::new("MiscellaneousMathematicalSymbols-B", 0x2980, 0x29FF)
    );
    db.insert(
        "SupplementalMathematicalOperators",
        Block::new("SupplementalMathematicalOperators", 0x2A00, 0x2AFF)
    );
    db.insert(
        "MiscellaneousSymbolsandArrows",
        Block::new("MiscellaneousSymbolsandArrows", 0x2B00, 0x2BFF)
    );
    db.insert("Glagolitic", Block::new("Glagolitic", 0x2C00, 0x2C5F));
    db.insert(
        "LatinExtended-C",
        Block::new("LatinExtended-C", 0x2C60, 0x2C7F)
    );
    db.insert("Coptic", Block::new("Coptic", 0x2C80, 0x2CFF));
    db.insert(
        "GeorgianSupplement",
        Block::new("GeorgianSupplement", 0x2D00, 0x2D2F)
    );
    db.insert("Tifinagh", Block::new("Tifinagh", 0x2D30, 0x2D7F));
    db.insert(
        "EthiopicExtended",
        Block::new("EthiopicExtended", 0x2D80, 0x2DDF)
    );
    db.insert(
        "CyrillicExtended-A",
        Block::new("CyrillicExtended-A", 0x2DE0, 0x2DFF)
    );
    db.insert(
        "SupplementalPunctuation",
        Block::new("SupplementalPunctuation", 0x2E00, 0x2E7F)
    );
    db.insert(
        "CJKRadicalsSupplement",
        Block::new("CJKRadicalsSupplement", 0x2E80, 0x2EFF)
    );
    db.insert(
        "KangxiRadicals",
        Block::new("KangxiRadicals", 0x2F00, 0x2FDF)
    );
    db.insert(
        "IdeographicDescriptionCharacters",
        Block::new("IdeographicDescriptionCharacters", 0x2FF0, 0x2FFF)
    );
    db.insert(
        "CJKSymbolsandPunctuation",
        Block::new("CJKSymbolsandPunctuation", 0x3000, 0x303F)
    );
    db.insert("Hiragana", Block::new("Hiragana", 0x3040, 0x309F));
    db.insert("Katakana", Block::new("Katakana", 0x30A0, 0x30FF));
    db.insert("Bopomofo", Block::new("Bopomofo", 0x3100, 0x312F));
    db.insert(
        "HangulCompatibilityJamo",
        Block::new("HangulCompatibilityJamo", 0x3130, 0x318F)
    );
    db.insert("Kanbun", Block::new("Kanbun", 0x3190, 0x319F));
    db.insert(
        "BopomofoExtended",
        Block::new("BopomofoExtended", 0x31A0, 0x31BF)
    );
    db.insert("CJKStrokes", Block::new("CJKStrokes", 0x31C0, 0x31EF));
    db.insert(
        "KatakanaPhoneticExtensions",
        Block::new("KatakanaPhoneticExtensions", 0x31F0, 0x31FF)
    );
    db.insert(
        "EnclosedCJKLettersandMonths",
        Block::new("EnclosedCJKLettersandMonths", 0x3200, 0x32FF)
    );
    db.insert(
        "CJKCompatibility",
        Block::new("CJKCompatibility", 0x3300, 0x33FF)
    );
    db.insert(
        "CJKUnifiedIdeographsExtensionA",
        Block::new("CJKUnifiedIdeographsExtensionA", 0x3400, 0x4DBF)
    );
    db.insert(
        "YijingHexagramSymbols",
        Block::new("YijingHexagramSymbols", 0x4DC0, 0x4DFF)
    );
    db.insert(
        "CJKUnifiedIdeographs",
        Block::new("CJKUnifiedIdeographs", 0x4E00, 0x9FFF)
    );
    db.insert("YiSyllables", Block::new("YiSyllables", 0xA000, 0xA48F));
    db.insert("YiRadicals", Block::new("YiRadicals", 0xA490, 0xA4CF));
    db.insert("Lisu", Block::new("Lisu", 0xA4D0, 0xA4FF));
    db.insert("Vai", Block::new("Vai", 0xA500, 0xA63F));
    db.insert(
        "CyrillicExtended-B",
        Block::new("CyrillicExtended-B", 0xA640, 0xA69F)
    );
    db.insert("Bamum", Block::new("Bamum", 0xA6A0, 0xA6FF));
    db.insert(
        "ModifierToneLetters",
        Block::new("ModifierToneLetters", 0xA700, 0xA71F)
    );
    db.insert(
        "LatinExtended-D",
        Block::new("LatinExtended-D", 0xA720, 0xA7FF)
    );
    db.insert("SylotiNagri", Block::new("SylotiNagri", 0xA800, 0xA82F));
    db.insert(
        "CommonIndicNumberForms",
        Block::new("CommonIndicNumberForms", 0xA830, 0xA83F)
    );
    db.insert("Phags-pa", Block::new("Phags-pa", 0xA840, 0xA87F));
    db.insert("Saurashtra", Block::new("Saurashtra", 0xA880, 0xA8DF));
    db.insert(
        "DevanagariExtended",
        Block::new("DevanagariExtended", 0xA8E0, 0xA8FF)
    );
    db.insert("KayahLi", Block::new("KayahLi", 0xA900, 0xA92F));
    db.insert("Rejang", Block::new("Rejang", 0xA930, 0xA95F));
    db.insert(
        "HangulJamoExtended-A",
        Block::new("HangulJamoExtended-A", 0xA960, 0xA97F)
    );
    db.insert("Javanese", Block::new("Javanese", 0xA980, 0xA9DF));
    db.insert(
        "MyanmarExtended-B",
        Block::new("MyanmarExtended-B", 0xA9E0, 0xA9FF)
    );
    db.insert("Cham", Block::new("Cham", 0xAA00, 0xAA5F));
    db.insert(
        "MyanmarExtended-A",
        Block::new("MyanmarExtended-A", 0xAA60, 0xAA7F)
    );
    db.insert("TaiViet", Block::new("TaiViet", 0xAA80, 0xAADF));
    db.insert(
        "MeeteiMayekExtensions",
        Block::new("MeeteiMayekExtensions", 0xAAE0, 0xAAFF)
    );
    db.insert(
        "EthiopicExtended-A",
        Block::new("EthiopicExtended-A", 0xAB00, 0xAB2F)
    );
    db.insert(
        "LatinExtended-E",
        Block::new("LatinExtended-E", 0xAB30, 0xAB6F)
    );
    db.insert(
        "CherokeeSupplement",
        Block::new("CherokeeSupplement", 0xAB70, 0xABBF)
    );
    db.insert("MeeteiMayek", Block::new("MeeteiMayek", 0xABC0, 0xABFF));
    db.insert(
        "HangulSyllables",
        Block::new("HangulSyllables", 0xAC00, 0xD7AF)
    );
    db.insert(
        "HangulJamoExtended-B",
        Block::new("HangulJamoExtended-B", 0xD7B0, 0xD7FF)
    );
    db.insert(
        "PrivateUseArea",
        Block::new("PrivateUseArea", 0xE000, 0xF8FF)
    );
    db.insert(
        "CJKCompatibilityIdeographs",
        Block::new("CJKCompatibilityIdeographs", 0xF900, 0xFAFF)
    );
    db.insert(
        "AlphabeticPresentationForms",
        Block::new("AlphabeticPresentationForms", 0xFB00, 0xFB4F)
    );
    db.insert(
        "ArabicPresentationForms-A",
        Block::new("ArabicPresentationForms-A", 0xFB50, 0xFDFF)
    );
    db.insert(
        "VariationSelectors",
        Block::new("VariationSelectors", 0xFE00, 0xFE0F)
    );
    db.insert("VerticalForms", Block::new("VerticalForms", 0xFE10, 0xFE1F));
    db.insert(
        "CombiningHalfMarks",
        Block::new("CombiningHalfMarks", 0xFE20, 0xFE2F)
    );
    db.insert(
        "CJKCompatibilityForms",
        Block::new("CJKCompatibilityForms", 0xFE30, 0xFE4F)
    );
    db.insert(
        "SmallFormVariants",
        Block::new("SmallFormVariants", 0xFE50, 0xFE6F)
    );
    db.insert(
        "ArabicPresentationForms-B",
        Block::new("ArabicPresentationForms-B", 0xFE70, 0xFEFF)
    );
    db.insert(
        "HalfwidthandFullwidthForms",
        Block::new("HalfwidthandFullwidthForms", 0xFF00, 0xFFEF)
    );
    db.insert("Specials", Block::new("Specials", 0xFFF0, 0xFFFF));
    db.insert(
        "LinearBSyllabary",
        Block::new("LinearBSyllabary", 0x10000, 0x1007F)
    );
    db.insert(
        "LinearBIdeograms",
        Block::new("LinearBIdeograms", 0x10080, 0x100FF)
    );
    db.insert(
        "AegeanNumbers",
        Block::new("AegeanNumbers", 0x10100, 0x1013F)
    );
    db.insert(
        "AncientGreekNumbers",
        Block::new("AncientGreekNumbers", 0x10140, 0x1018F)
    );
    db.insert(
        "AncientSymbols",
        Block::new("AncientSymbols", 0x10190, 0x101CF)
    );
    db.insert("PhaistosDisc", Block::new("PhaistosDisc", 0x101D0, 0x101FF));
    db.insert("Lycian", Block::new("Lycian", 0x10280, 0x1029F));
    db.insert("Carian", Block::new("Carian", 0x102A0, 0x102DF));
    db.insert(
        "CopticEpactNumbers",
        Block::new("CopticEpactNumbers", 0x102E0, 0x102FF)
    );
    db.insert("OldItalic", Block::new("OldItalic", 0x10300, 0x1032F));
    db.insert("Gothic", Block::new("Gothic", 0x10330, 0x1034F));
    db.insert("OldPermic", Block::new("OldPermic", 0x10350, 0x1037F));
    db.insert("Ugaritic", Block::new("Ugaritic", 0x10380, 0x1039F));
    db.insert("OldPersian", Block::new("OldPersian", 0x103A0, 0x103DF));
    db.insert("Deseret", Block::new("Deseret", 0x10400, 0x1044F));
    db.insert("Shavian", Block::new("Shavian", 0x10450, 0x1047F));
    db.insert("Osmanya", Block::new("Osmanya", 0x10480, 0x104AF));
    db.insert("Osage", Block::new("Osage", 0x104B0, 0x104FF));
    db.insert("Elbasan", Block::new("Elbasan", 0x10500, 0x1052F));
    db.insert(
        "CaucasianAlbanian",
        Block::new("CaucasianAlbanian", 0x10530, 0x1056F)
    );
    db.insert("LinearA", Block::new("LinearA", 0x10600, 0x1077F));
    db.insert(
        "CypriotSyllabary",
        Block::new("CypriotSyllabary", 0x10800, 0x1083F)
    );
    db.insert(
        "ImperialAramaic",
        Block::new("ImperialAramaic", 0x10840, 0x1085F)
    );
    db.insert("Palmyrene", Block::new("Palmyrene", 0x10860, 0x1087F));
    db.insert("Nabataean", Block::new("Nabataean", 0x10880, 0x108AF));
    db.insert("Hatran", Block::new("Hatran", 0x108E0, 0x108FF));
    db.insert("Phoenician", Block::new("Phoenician", 0x10900, 0x1091F));
    db.insert("Lydian", Block::new("Lydian", 0x10920, 0x1093F));
    db.insert(
        "MeroiticHieroglyphs",
        Block::new("MeroiticHieroglyphs", 0x10980, 0x1099F)
    );
    db.insert(
        "MeroiticCursive",
        Block::new("MeroiticCursive", 0x109A0, 0x109FF)
    );
    db.insert("Kharoshthi", Block::new("Kharoshthi", 0x10A00, 0x10A5F));
    db.insert(
        "OldSouthArabian",
        Block::new("OldSouthArabian", 0x10A60, 0x10A7F)
    );
    db.insert(
        "OldNorthArabian",
        Block::new("OldNorthArabian", 0x10A80, 0x10A9F)
    );
    db.insert("Manichaean", Block::new("Manichaean", 0x10AC0, 0x10AFF));
    db.insert("Avestan", Block::new("Avestan", 0x10B00, 0x10B3F));
    db.insert(
        "InscriptionalParthian",
        Block::new("InscriptionalParthian", 0x10B40, 0x10B5F)
    );
    db.insert(
        "InscriptionalPahlavi",
        Block::new("InscriptionalPahlavi", 0x10B60, 0x10B7F)
    );
    db.insert(
        "PsalterPahlavi",
        Block::new("PsalterPahlavi", 0x10B80, 0x10BAF)
    );
    db.insert("OldTurkic", Block::new("OldTurkic", 0x10C00, 0x10C4F));
    db.insert("OldHungarian", Block::new("OldHungarian", 0x10C80, 0x10CFF));
    db.insert(
        "RumiNumeralSymbols",
        Block::new("RumiNumeralSymbols", 0x10E60, 0x10E7F)
    );
    db.insert("Brahmi", Block::new("Brahmi", 0x11000, 0x1107F));
    db.insert("Kaithi", Block::new("Kaithi", 0x11080, 0x110CF));
    db.insert("SoraSompeng", Block::new("SoraSompeng", 0x110D0, 0x110FF));
    db.insert("Chakma", Block::new("Chakma", 0x11100, 0x1114F));
    db.insert("Mahajani", Block::new("Mahajani", 0x11150, 0x1117F));
    db.insert("Sharada", Block::new("Sharada", 0x11180, 0x111DF));
    db.insert(
        "SinhalaArchaicNumbers",
        Block::new("SinhalaArchaicNumbers", 0x111E0, 0x111FF)
    );
    db.insert("Khojki", Block::new("Khojki", 0x11200, 0x1124F));
    db.insert("Multani", Block::new("Multani", 0x11280, 0x112AF));
    db.insert("Khudawadi", Block::new("Khudawadi", 0x112B0, 0x112FF));
    db.insert("Grantha", Block::new("Grantha", 0x11300, 0x1137F));
    db.insert("Newa", Block::new("Newa", 0x11400, 0x1147F));
    db.insert("Tirhuta", Block::new("Tirhuta", 0x11480, 0x114DF));
    db.insert("Siddham", Block::new("Siddham", 0x11580, 0x115FF));
    db.insert("Modi", Block::new("Modi", 0x11600, 0x1165F));
    db.insert(
        "MongolianSupplement",
        Block::new("MongolianSupplement", 0x11660, 0x1167F)
    );
    db.insert("Takri", Block::new("Takri", 0x11680, 0x116CF));
    db.insert("Ahom", Block::new("Ahom", 0x11700, 0x1173F));
    db.insert("WarangCiti", Block::new("WarangCiti", 0x118A0, 0x118FF));
    db.insert(
        "ZanabazarSquare",
        Block::new("ZanabazarSquare", 0x11A00, 0x11A4F)
    );
    db.insert("Soyombo", Block::new("Soyombo", 0x11A50, 0x11AAF));
    db.insert("PauCinHau", Block::new("PauCinHau", 0x11AC0, 0x11AFF));
    db.insert("Bhaiksuki", Block::new("Bhaiksuki", 0x11C00, 0x11C6F));
    db.insert("Marchen", Block::new("Marchen", 0x11C70, 0x11CBF));
    db.insert("MasaramGondi", Block::new("MasaramGondi", 0x11D00, 0x11D5F));
    db.insert("Cuneiform", Block::new("Cuneiform", 0x12000, 0x123FF));
    db.insert(
        "CuneiformNumbersandPunctuation",
        Block::new("CuneiformNumbersandPunctuation", 0x12400, 0x1247F)
    );
    db.insert(
        "EarlyDynasticCuneiform",
        Block::new("EarlyDynasticCuneiform", 0x12480, 0x1254F)
    );
    db.insert(
        "EgyptianHieroglyphs",
        Block::new("EgyptianHieroglyphs", 0x13000, 0x1342F)
    );
    db.insert(
        "AnatolianHieroglyphs",
        Block::new("AnatolianHieroglyphs", 0x14400, 0x1467F)
    );
    db.insert(
        "BamumSupplement",
        Block::new("BamumSupplement", 0x16800, 0x16A3F)
    );
    db.insert("Mro", Block::new("Mro", 0x16A40, 0x16A6F));
    db.insert("BassaVah", Block::new("BassaVah", 0x16AD0, 0x16AFF));
    db.insert("PahawhHmong", Block::new("PahawhHmong", 0x16B00, 0x16B8F));
    db.insert("Miao", Block::new("Miao", 0x16F00, 0x16F9F));
    db.insert(
        "IdeographicSymbolsandPunctuation",
        Block::new("IdeographicSymbolsandPunctuation", 0x16FE0, 0x16FFF)
    );
    db.insert("Tangut", Block::new("Tangut", 0x17000, 0x187FF));
    db.insert(
        "TangutComponents",
        Block::new("TangutComponents", 0x18800, 0x18AFF)
    );
    db.insert(
        "KanaSupplement",
        Block::new("KanaSupplement", 0x1B000, 0x1B0FF)
    );
    db.insert(
        "KanaExtended-A",
        Block::new("KanaExtended-A", 0x1B100, 0x1B12F)
    );
    db.insert("Nushu", Block::new("Nushu", 0x1B170, 0x1B2FF));
    db.insert("Duployan", Block::new("Duployan", 0x1BC00, 0x1BC9F));
    db.insert(
        "ShorthandFormatControls",
        Block::new("ShorthandFormatControls", 0x1BCA0, 0x1BCAF)
    );
    db.insert(
        "ByzantineMusicalSymbols",
        Block::new("ByzantineMusicalSymbols", 0x1D000, 0x1D0FF)
    );
    db.insert(
        "MusicalSymbols",
        Block::new("MusicalSymbols", 0x1D100, 0x1D1FF)
    );
    db.insert(
        "AncientGreekMusicalNotation",
        Block::new("AncientGreekMusicalNotation", 0x1D200, 0x1D24F)
    );
    db.insert(
        "TaiXuanJingSymbols",
        Block::new("TaiXuanJingSymbols", 0x1D300, 0x1D35F)
    );
    db.insert(
        "CountingRodNumerals",
        Block::new("CountingRodNumerals", 0x1D360, 0x1D37F)
    );
    db.insert(
        "MathematicalAlphanumericSymbols",
        Block::new("MathematicalAlphanumericSymbols", 0x1D400, 0x1D7FF)
    );
    db.insert(
        "SuttonSignWriting",
        Block::new("SuttonSignWriting", 0x1D800, 0x1DAAF)
    );
    db.insert(
        "GlagoliticSupplement",
        Block::new("GlagoliticSupplement", 0x1E000, 0x1E02F)
    );
    db.insert("MendeKikakui", Block::new("MendeKikakui", 0x1E800, 0x1E8DF));
    db.insert("Adlam", Block::new("Adlam", 0x1E900, 0x1E95F));
    db.insert(
        "ArabicMathematicalAlphabeticSymbols",
        Block::new("ArabicMathematicalAlphabeticSymbols", 0x1EE00, 0x1EEFF)
    );
    db.insert("MahjongTiles", Block::new("MahjongTiles", 0x1F000, 0x1F02F));
    db.insert("DominoTiles", Block::new("DominoTiles", 0x1F030, 0x1F09F));
    db.insert("PlayingCards", Block::new("PlayingCards", 0x1F0A0, 0x1F0FF));
    db.insert(
        "EnclosedAlphanumericSupplement",
        Block::new("EnclosedAlphanumericSupplement", 0x1F100, 0x1F1FF)
    );
    db.insert(
        "EnclosedIdeographicSupplement",
        Block::new("EnclosedIdeographicSupplement", 0x1F200, 0x1F2FF)
    );
    db.insert(
        "MiscellaneousSymbolsandPictographs",
        Block::new("MiscellaneousSymbolsandPictographs", 0x1F300, 0x1F5FF)
    );
    db.insert("Emoticons", Block::new("Emoticons", 0x1F600, 0x1F64F));
    db.insert(
        "OrnamentalDingbats",
        Block::new("OrnamentalDingbats", 0x1F650, 0x1F67F)
    );
    db.insert(
        "TransportandMapSymbols",
        Block::new("TransportandMapSymbols", 0x1F680, 0x1F6FF)
    );
    db.insert(
        "AlchemicalSymbols",
        Block::new("AlchemicalSymbols", 0x1F700, 0x1F77F)
    );
    db.insert(
        "GeometricShapesExtended",
        Block::new("GeometricShapesExtended", 0x1F780, 0x1F7FF)
    );
    db.insert(
        "SupplementalArrows-C",
        Block::new("SupplementalArrows-C", 0x1F800, 0x1F8FF)
    );
    db.insert(
        "SupplementalSymbolsandPictographs",
        Block::new("SupplementalSymbolsandPictographs", 0x1F900, 0x1F9FF)
    );
    db.insert(
        "CJKUnifiedIdeographsExtensionB",
        Block::new("CJKUnifiedIdeographsExtensionB", 0x20000, 0x2A6DF)
    );
    db.insert(
        "CJKUnifiedIdeographsExtensionC",
        Block::new("CJKUnifiedIdeographsExtensionC", 0x2A700, 0x2B73F)
    );
    db.insert(
        "CJKUnifiedIdeographsExtensionD",
        Block::new("CJKUnifiedIdeographsExtensionD", 0x2B740, 0x2B81F)
    );
    db.insert(
        "CJKUnifiedIdeographsExtensionE",
        Block::new("CJKUnifiedIdeographsExtensionE", 0x2B820, 0x2CEAF)
    );
    db.insert(
        "CJKUnifiedIdeographsExtensionF",
        Block::new("CJKUnifiedIdeographsExtensionF", 0x2CEB0, 0x2EBEF)
    );
    db.insert(
        "CJKCompatibilityIdeographsSupplement",
        Block::new("CJKCompatibilityIdeographsSupplement", 0x2F800, 0x2FA1F)
    );
    db.insert("Tags", Block::new("Tags", 0xE0000, 0xE007F));
    db.insert(
        "VariationSelectorsSupplement",
        Block::new("VariationSelectorsSupplement", 0xE0100, 0xE01EF)
    );
    db.insert(
        "SupplementaryPrivateUseArea-A",
        Block::new("SupplementaryPrivateUseArea-A", 0xF0000, 0xFFFFF)
    );
    db.insert(
        "SupplementaryPrivateUseArea-B",
        Block::new("SupplementaryPrivateUseArea-B", 0x0010_0000, 0x0010_FFFF)
    );
    db
}

/// Gets all categories
pub fn get_categories() -> HashMap<&'static str, Category> {
    HashMap::new()
}
