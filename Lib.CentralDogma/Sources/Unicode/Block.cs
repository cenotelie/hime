/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:22
 * 
 */
using System.Collections.Generic;

namespace Hime.CentralDogma.Unicode
{
    class Block : Span
    {
        private string name;
        public string Name { get { return name; } }

        private Block(ushort begin, ushort end, string name)
            : base(begin, end)
        {
            this.name = name;
        }

        private static Dictionary<string, Block> categories = null;
        private static void BuildCategories()
        {
            categories = new Dictionary<string, Block>();
            BuildCategories_Cat(new Block(0x0000, 0x007F, "IsBasicLatin"));
            BuildCategories_Cat(new Block(0x0080, 0x00FF, "IsLatin-1Supplement"));
            BuildCategories_Cat(new Block(0x0100, 0x017F, "IsLatinExtended-A"));
            BuildCategories_Cat(new Block(0x0180, 0x024F, "IsLatinExtended-B"));
            BuildCategories_Cat(new Block(0x0250, 0x02AF, "IsIPAExtensions"));
            BuildCategories_Cat(new Block(0x02B0, 0x02FF, "IsSpacingModifierLetters"));
            BuildCategories_Cat(new Block(0x0300, 0x036F, "IsCombiningDiacriticalMarks"));
            BuildCategories_Cat(new Block(0x0370, 0x03FF, "IsGreek"));
            BuildCategories_Cat(new Block(0x0400, 0x04FF, "IsCyrillic"));
            BuildCategories_Cat(new Block(0x0500, 0x052F, "IsCyrillicSupplement"));
            BuildCategories_Cat(new Block(0x0530, 0x058F, "IsArmenian"));
            BuildCategories_Cat(new Block(0x0590, 0x05FF, "IsHebrew"));
            BuildCategories_Cat(new Block(0x0600, 0x06FF, "IsArabic"));
            BuildCategories_Cat(new Block(0x0700, 0x074F, "IsSyriac"));
            BuildCategories_Cat(new Block(0x0780, 0x07BF, "IsThaana"));
            BuildCategories_Cat(new Block(0x0900, 0x097F, "IsDevanagari"));
            BuildCategories_Cat(new Block(0x0980, 0x09FF, "IsBengali"));
            BuildCategories_Cat(new Block(0x0A00, 0x0A7F, "IsGurmukhi"));
            BuildCategories_Cat(new Block(0x0A80, 0x0AFF, "IsGujarati"));
            BuildCategories_Cat(new Block(0x0B00, 0x0B7F, "IsOriya"));
            BuildCategories_Cat(new Block(0x0B80, 0x0BFF, "IsTamil"));
            BuildCategories_Cat(new Block(0x0C00, 0x0C7F, "IsTelugu"));
            BuildCategories_Cat(new Block(0x0C80, 0x0CFF, "IsKannada"));
            BuildCategories_Cat(new Block(0x0D00, 0x0D7F, "IsMalayalam"));
            BuildCategories_Cat(new Block(0x0D80, 0x0DFF, "IsSinhala"));
            BuildCategories_Cat(new Block(0x0E00, 0x0E7F, "IsThai"));
            BuildCategories_Cat(new Block(0x0E80, 0x0EFF, "IsLao"));
            BuildCategories_Cat(new Block(0x0F00, 0x0FFF, "IsTibetan"));
            BuildCategories_Cat(new Block(0x1000, 0x109F, "IsMyanmar"));
            BuildCategories_Cat(new Block(0x10A0, 0x10FF, "IsGeorgian"));
            BuildCategories_Cat(new Block(0x1100, 0x11FF, "IsHangulJamo"));
            BuildCategories_Cat(new Block(0x1200, 0x137F, "IsEthiopic"));
            BuildCategories_Cat(new Block(0x13A0, 0x13FF, "IsCherokee"));
            BuildCategories_Cat(new Block(0x1400, 0x167F, "IsUnifiedCanadianAboriginalSyllabics"));
            BuildCategories_Cat(new Block(0x1680, 0x169F, "IsOgham"));
            BuildCategories_Cat(new Block(0x16A0, 0x16FF, "IsRunic"));
            BuildCategories_Cat(new Block(0x1700, 0x171F, "IsTagalog"));
            BuildCategories_Cat(new Block(0x1720, 0x173F, "IsHanunoo"));
            BuildCategories_Cat(new Block(0x1740, 0x175F, "IsBuhid"));
            BuildCategories_Cat(new Block(0x1760, 0x177F, "IsTagbanwa"));
            BuildCategories_Cat(new Block(0x1780, 0x17FF, "IsKhmer"));
            BuildCategories_Cat(new Block(0x1800, 0x18AF, "IsMongolian"));
            BuildCategories_Cat(new Block(0x1900, 0x194F, "IsLimbu"));
            BuildCategories_Cat(new Block(0x1950, 0x197F, "IsTaiLe"));
            BuildCategories_Cat(new Block(0x19E0, 0x19FF, "IsKhmerSymbols"));
            BuildCategories_Cat(new Block(0x1D00, 0x1D7F, "IsPhoneticExtensions"));
            BuildCategories_Cat(new Block(0x1E00, 0x1EFF, "IsLatinExtendedAdditional"));
            BuildCategories_Cat(new Block(0x1F00, 0x1FFF, "IsGreekExtended"));
            BuildCategories_Cat(new Block(0x2000, 0x206F, "IsGeneralPunctuation"));
            BuildCategories_Cat(new Block(0x2070, 0x209F, "IsSuperscriptsandSubscripts"));
            BuildCategories_Cat(new Block(0x20A0, 0x20CF, "IsCurrencySymbols"));
            BuildCategories_Cat(new Block(0x20D0, 0x20FF, "IsCombiningDiacriticalMarksforSymbols"));
            BuildCategories_Cat(new Block(0x2100, 0x214F, "IsLetterlikeSymbols"));
            BuildCategories_Cat(new Block(0x2150, 0x218F, "IsNumberForms"));
            BuildCategories_Cat(new Block(0x2190, 0x21FF, "IsArrows"));
            BuildCategories_Cat(new Block(0x2200, 0x22FF, "IsMathematicalOperators"));
            BuildCategories_Cat(new Block(0x2300, 0x23FF, "IsMiscellaneousTechnical"));
            BuildCategories_Cat(new Block(0x2400, 0x243F, "IsControlPictures"));
            BuildCategories_Cat(new Block(0x2440, 0x245F, "IsOpticalCharacterRecognition"));
            BuildCategories_Cat(new Block(0x2460, 0x24FF, "IsEnclosedAlphanumerics"));
            BuildCategories_Cat(new Block(0x2500, 0x257F, "IsBoxDrawing"));
            BuildCategories_Cat(new Block(0x2580, 0x259F, "IsBlockElements"));
            BuildCategories_Cat(new Block(0x25A0, 0x25FF, "IsGeometricShapes"));
            BuildCategories_Cat(new Block(0x2600, 0x26FF, "IsMiscellaneousSymbols"));
            BuildCategories_Cat(new Block(0x2700, 0x27BF, "IsDingbats"));
            BuildCategories_Cat(new Block(0x27C0, 0x27EF, "IsMiscellaneousMathematicalSymbols-A"));
            BuildCategories_Cat(new Block(0x27F0, 0x27FF, "IsSupplementalArrows-A"));
            BuildCategories_Cat(new Block(0x2800, 0x28FF, "IsBraillePatterns"));
            BuildCategories_Cat(new Block(0x2900, 0x297F, "IsSupplementalArrows-B"));
            BuildCategories_Cat(new Block(0x2980, 0x29FF, "IsMiscellaneousMathematicalSymbols-B"));
            BuildCategories_Cat(new Block(0x2A00, 0x2AFF, "IsSupplementalMathematicalOperators"));
            BuildCategories_Cat(new Block(0x2B00, 0x2BFF, "IsMiscellaneousSymbolsandArrows"));
            BuildCategories_Cat(new Block(0x2E80, 0x2EFF, "IsCJKRadicalsSupplement"));
            BuildCategories_Cat(new Block(0x2F00, 0x2FDF, "IsKangxiRadicals"));
            BuildCategories_Cat(new Block(0x2FF0, 0x2FFF, "IsIdeographicDescriptionCharacters"));
            BuildCategories_Cat(new Block(0x3000, 0x303F, "IsCJKSymbolsandPunctuation"));
            BuildCategories_Cat(new Block(0x3040, 0x309F, "IsHiragana"));
            BuildCategories_Cat(new Block(0x30A0, 0x30FF, "IsKatakana"));
            BuildCategories_Cat(new Block(0x3100, 0x312F, "IsBopomofo"));
            BuildCategories_Cat(new Block(0x3130, 0x318F, "IsHangulCompatibilityJamo"));
            BuildCategories_Cat(new Block(0x3190, 0x319F, "IsKanbun"));
            BuildCategories_Cat(new Block(0x31A0, 0x31BF, "IsBopomofoExtended"));
            BuildCategories_Cat(new Block(0x31F0, 0x31FF, "IsKatakanaPhoneticExtensions"));
            BuildCategories_Cat(new Block(0x3200, 0x32FF, "IsEnclosedCJKLettersandMonths"));
            BuildCategories_Cat(new Block(0x3300, 0x33FF, "IsCJKCompatibility"));
            BuildCategories_Cat(new Block(0x3400, 0x4DBF, "IsCJKUnifiedIdeographsExtensionA"));
            BuildCategories_Cat(new Block(0x4DC0, 0x4DFF, "IsYijingHexagramSymbols"));
            BuildCategories_Cat(new Block(0x4E00, 0x9FFF, "IsCJKUnifiedIdeographs"));
            BuildCategories_Cat(new Block(0xA000, 0xA48F, "IsYiSyllables"));
            BuildCategories_Cat(new Block(0xA490, 0xA4CF, "IsYiRadicals"));
            BuildCategories_Cat(new Block(0xAC00, 0xD7AF, "IsHangulSyllables"));
            BuildCategories_Cat(new Block(0xD800, 0xDB7F, "IsHighSurrogates"));
            BuildCategories_Cat(new Block(0xDB80, 0xDBFF, "IsHighPrivateUseSurrogates"));
            BuildCategories_Cat(new Block(0xDC00, 0xDFFF, "IsLowSurrogates"));
            BuildCategories_Cat(new Block(0xE000, 0xF8FF, "IsPrivateUse"));
            BuildCategories_Cat(new Block(0xF900, 0xFAFF, "IsCJKCompatibilityIdeographs"));
            BuildCategories_Cat(new Block(0xFB00, 0xFB4F, "IsAlphabeticPresentationForms"));
            BuildCategories_Cat(new Block(0xFB50, 0xFDFF, "IsArabicPresentationForms-A"));
            BuildCategories_Cat(new Block(0xFE00, 0xFE0F, "IsVariationSelectors"));
            BuildCategories_Cat(new Block(0xFE20, 0xFE2F, "IsCombiningHalfMarks"));
            BuildCategories_Cat(new Block(0xFE30, 0xFE4F, "IsCJKCompatibilityForms"));
            BuildCategories_Cat(new Block(0xFE50, 0xFE6F, "IsSmallFormVariants"));
            BuildCategories_Cat(new Block(0xFE70, 0xFEFF, "IsArabicPresentationForms-B"));
            BuildCategories_Cat(new Block(0xFF00, 0xFFEF, "IsHalfwidthandFullwidthForms"));
            BuildCategories_Cat(new Block(0xFFF0, 0xFFFF, "IsSpecials"));
        }
        private static void BuildCategories_Cat(Block cat) { categories.Add(cat.name, cat); }
        public static Dictionary<string, Block> Categories
        {
            get
            {
                if (categories == null)
                    BuildCategories();
                return categories;
            }
        }
    }
}