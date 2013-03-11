using System;

namespace Hime.Benchmark
{
    class Benchmark
    {
        private string language;
        private string input;
        private string output;
        private int size;
        private int sampleSize;
        private bool doLexer;
        private bool doParser;
        private int tokenCount;

        public Benchmark()
        {
            language = "CSharp4.gram";
            input = "Perf.gram";
            output = "result.txt";
            size = 600;
            sampleSize = 20;
            doLexer = false;
            doParser = true;
        }

        public void Run()
        {
            //Setup();
            if (doLexer)
            {
                Console.WriteLine("-- lexer");
                for (int i = 0; i != sampleSize; i++)
                    BenchmarkLexer(i);
            }
            if (doParser)
            {
                Console.WriteLine("-- parser");
                for (int i = 0; i != sampleSize; i++)
                    BenchmarkParser(i);
            }
        }

        private void Setup()
        {
            if (System.IO.File.Exists(input))
                System.IO.File.Delete(input);
            if (System.IO.File.Exists(output))
                System.IO.File.Delete(output);

            string content = System.IO.File.ReadAllText("Languages\\" + language);
            for (int i = 0; i != size; i++)
                System.IO.File.AppendAllText(input, content);
            System.IO.StreamReader reader = new System.IO.StreamReader(input);
            Generated.CD.FileCentralDogmaLexer lexer = new Generated.CD.FileCentralDogmaLexer(reader);
            Hime.Redist.Symbols.Token token = lexer.GetNextToken();
            while (token.SymbolID != 1)
            {
                token = lexer.GetNextToken();
                tokenCount++;
            }
            reader.Close();
            System.IO.File.AppendAllText(output, "-- tokens: " + tokenCount + "\n");
            Console.WriteLine("-- completed setup: " + tokenCount + " tokens");
        }

        private void BenchmarkLexer(int index)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(input);
            Generated.CD.FileCentralDogmaLexer lexer = new Generated.CD.FileCentralDogmaLexer(reader);
            System.DateTime before = System.DateTime.Now;
            Hime.Redist.Symbols.Token token = lexer.GetNextToken();
            while (token.SymbolID != 1)
            {
                token = lexer.GetNextToken();
                tokenCount++;
            }
            System.DateTime after = System.DateTime.Now;
            System.TimeSpan span = after.Subtract(before);
            reader.Close();
            System.IO.File.AppendAllText(output, (int)span.TotalMilliseconds + "\n");
            Console.WriteLine("completed lexer[" + index + "]: " + (int)span.TotalMilliseconds + " ms");
        }

        private void BenchmarkParser(int index)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(input);
            Generated.CD.FileCentralDogmaLexer lexer = new Generated.CD.FileCentralDogmaLexer(reader);
            Generated.CD.FileCentralDogmaParser parser = new Generated.CD.FileCentralDogmaParser(lexer);
            System.DateTime before = System.DateTime.Now;
            parser.Parse();
            System.DateTime after = System.DateTime.Now;
            System.TimeSpan span = after.Subtract(before);
            reader.Close();
            System.IO.File.AppendAllText(output, (int)span.TotalMilliseconds + "\n");
            Console.WriteLine("completed parser[" + index + "]: " + (int)span.TotalMilliseconds + " ms");
        }
    }
}
