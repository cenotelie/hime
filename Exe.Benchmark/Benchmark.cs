using System;
using System.Reflection;
using Hime.CentralDogma;
using System.IO;

namespace Hime.Benchmark
{
    class Benchmark
    {
        private ParsingMethod method;
        private string language;
        private string input;
        private string output;
        private int size;
        private int sampleSize;
        private bool doLexer;
        private bool doParser;
        private int tokenCount;
        private Assembly assembly;
        
        public Benchmark(ParsingMethod method, bool lexer, bool parser)
        {
            this.method = method;
            this.language = "CSharp4.gram";
            this.input = "Perf.gram";
            this.output = "result.txt";
            this.size = 600;
            this.sampleSize = 20;
            this.doLexer = lexer;
            this.doParser = parser;
        }

        public void Run()
        {
            Setup();
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
            // Build parser assembly
            System.IO.Stream stream = typeof(CompilationTask).Assembly.GetManifestResourceStream("Hime.CentralDogma.Sources.Input.FileCentralDogma.gram");
            CompilationTask task = new CompilationTask();
            task.Mode = CompilationMode.Assembly;
            task.AddInputRaw(stream);
            task.Namespace = "Hime.Benchmark.Generated";
            task.GrammarName = "FileCentralDogma";
            task.CodeAccess = AccessModifier.Public;
            task.Method = method;
            task.Execute();
            assembly = Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, "FileCentralDogma.dll"));

            if (System.IO.File.Exists(output))
                System.IO.File.Delete(output);

            // Build input
            stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Hime.Benchmark.Languages." + language);
            System.IO.StreamReader reader = new System.IO.StreamReader(stream);
            string content = reader.ReadToEnd();
            reader.Close();
            if (System.IO.File.Exists(input))
                System.IO.File.Delete(input);
            for (int i = 0; i != size; i++)
                System.IO.File.AppendAllText(input, content);
            
            // Get input's statistics
            reader = new System.IO.StreamReader(input);
            Hime.Redist.Lexer.TextLexer lexer = GetLexer(reader);
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

        private Hime.Redist.Lexer.TextLexer GetLexer(System.IO.StreamReader reader)
        {
            Type lexerType = assembly.GetType("Hime.Benchmark.Generated.FileCentralDogmaLexer");
            ConstructorInfo lexerConstructor = lexerType.GetConstructor(new Type[] { typeof(System.IO.TextReader) });
            object lexer = lexerConstructor.Invoke(new object[] { reader });
            return lexer as Hime.Redist.Lexer.TextLexer;
        }

        private Hime.Redist.Parsers.BaseLRParser GetParser(System.IO.StreamReader reader)
        {
            Hime.Redist.Lexer.TextLexer lexer = GetLexer(reader);
            Type lexerType = assembly.GetType("Hime.Benchmark.Generated.FileCentralDogmaLexer");
            Type parserType = assembly.GetType("Hime.Benchmark.Generated.FileCentralDogmaParser");
            ConstructorInfo parserConstructor = parserType.GetConstructor(new Type[] { lexerType });
            object parser = parserConstructor.Invoke(new object[] { lexer });
            return parser as Hime.Redist.Parsers.BaseLRParser;
        }

        private void BenchmarkLexer(int index)
        {
            System.IO.StreamReader reader = new System.IO.StreamReader(input);
            Hime.Redist.Lexer.TextLexer lexer = GetLexer(reader);
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
            Hime.Redist.Parsers.BaseLRParser parser = GetParser(reader);
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
