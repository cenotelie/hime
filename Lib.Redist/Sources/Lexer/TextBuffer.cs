namespace Hime.Redist.Lexer
{
    struct TextBuffer
    {
        private char[] data;
        private int start;
        private int end;

        public char this[int index] { get { return data[index]; } }
        public int Start { get { return start; } }
        public int End { get { return end; } }
        public bool IsEmpty { get { return (end <= start); } }

        public TextBuffer(char[] data, int start, int end)
        {
            this.data = data;
            this.start = start;
            this.end = end;
        }
    }
}
