using System.Collections.Generic;

namespace Hime.CentralDogma.Automata
{
    struct CharSpan
    {
        private char spanBegin;
        private char spanEnd;

        public static CharSpan Null = new CharSpan(System.Convert.ToChar(1), System.Convert.ToChar(0));

        public char Begin { get { return spanBegin; } }
        public char End { get { return spanEnd; } }
        public int Length { get { return spanEnd - spanBegin + 1; } }

        public CharSpan(char Begin, char End)
        {
            spanBegin = Begin;
            spanEnd = End;
        }

        public static CharSpan Intersect(CharSpan Left, CharSpan Right)
        {
            if (Left.spanBegin < Right.spanBegin)
            {
                if (Left.spanEnd < Right.spanBegin)
                    return Null;
                if (Left.spanEnd < Right.spanEnd)
                    return new CharSpan(Right.spanBegin, Left.spanEnd);
                return new CharSpan(Right.spanBegin, Right.spanEnd);
            }
            else
            {
                if (Right.spanEnd < Left.spanBegin)
                    return Null;
                if (Right.spanEnd < Left.spanEnd)
                    return new CharSpan(Left.spanBegin, Right.spanEnd);
                return new CharSpan(Left.spanBegin, Left.spanEnd);
            }
        }

        public static CharSpan Split(CharSpan Original, CharSpan Splitter, out CharSpan Rest)
        {
            if (Original.spanBegin == Splitter.spanBegin)
            {
                Rest = Null;
                if (Original.spanEnd == Splitter.spanEnd) return Null;
                return new CharSpan(System.Convert.ToChar(Splitter.spanEnd + 1), Original.spanEnd);
            }
            if (Original.spanEnd == Splitter.spanEnd)
            {
                Rest = Null;
                return new CharSpan(Original.spanBegin, System.Convert.ToChar(Splitter.spanBegin - 1));
            }
            Rest = new CharSpan(System.Convert.ToChar(Splitter.spanEnd + 1), Original.spanEnd);
            return new CharSpan(Original.spanBegin, System.Convert.ToChar(Splitter.spanBegin - 1));
        }

        public static int Compare(CharSpan left, CharSpan right) { return left.spanBegin.CompareTo(right.spanBegin); }
        public static int CompareReverse(CharSpan left, CharSpan right) { return right.spanBegin.CompareTo(left.spanBegin); }

        public override string ToString()
        {
            if (spanBegin > spanEnd)
                return string.Empty;
            if (spanBegin == spanEnd)
                return CharToString(spanBegin);
            return "[" + CharToString(spanBegin) + "-" + CharToString(spanEnd) + "]";
        }

        private string CharToString(char c)
        {
            System.Globalization.UnicodeCategory cat = char.GetUnicodeCategory(c);
            switch (cat)
            {
                case System.Globalization.UnicodeCategory.ModifierLetter:
                case System.Globalization.UnicodeCategory.NonSpacingMark:
                case System.Globalization.UnicodeCategory.SpacingCombiningMark:
                case System.Globalization.UnicodeCategory.EnclosingMark:
                case System.Globalization.UnicodeCategory.SpaceSeparator:
                case System.Globalization.UnicodeCategory.LineSeparator:
                case System.Globalization.UnicodeCategory.ParagraphSeparator:
                case System.Globalization.UnicodeCategory.Control:
                case System.Globalization.UnicodeCategory.Format:
                case System.Globalization.UnicodeCategory.Surrogate:
                case System.Globalization.UnicodeCategory.PrivateUse:
                case System.Globalization.UnicodeCategory.OtherNotAssigned:
                    return CharToString_NonPrintable(c);
                default:
                    return c.ToString();
            }
        }
        private string CharToString_NonPrintable(char c)
        {
            string result = "U+" + System.Convert.ToUInt16(c).ToString("X");
            return result;
        }

        public override bool Equals(object obj)
        {
            if (obj is CharSpan)
            {
                CharSpan Span = (CharSpan)obj;
                return ((spanBegin == Span.spanBegin) && (spanEnd == Span.spanEnd));
            }
            return false;
        }
        public override int GetHashCode() { return base.GetHashCode(); }
    }
}