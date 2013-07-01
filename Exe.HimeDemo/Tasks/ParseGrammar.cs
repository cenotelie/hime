using System;
using System.IO;
using System.Reflection;
using Hime.CentralDogma;
using Hime.Redist;

namespace Hime.Demo.Tasks
{
    class ParseGrammar : IExecutable
    {
        public void Execute()
        {
            // Build parser assembly
            System.IO.Stream stream = typeof(CompilationTask).Assembly.GetManifestResourceStream("Hime.CentralDogma.Sources.Input.FileCentralDogma.gram");
            CompilationTask task = new CompilationTask();
            task.Mode = CompilationMode.Assembly;
            task.AddInputRaw(stream);
            task.Namespace = "Hime.Benchmark.Generated";
            task.GrammarName = "FileCentralDogma";
            task.CodeAccess = AccessModifier.Public;
            task.Method = ParsingMethod.LALR1;
            task.Execute();
            Assembly assembly = Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, "FileCentralDogma.dll"));

            System.IO.StreamReader reader = new System.IO.StreamReader("Languages\\FileCentralDogma.gram");
            Hime.Redist.Parsers.BaseLRParser parser = GetParser(assembly, reader);
            ParseTree ast = parser.Parse();
            reader.Close();
            
            foreach (Error error in parser.Errors)
                Console.WriteLine(error.ToString());
            if (ast == null)
                return;
            WinTreeView win = new WinTreeView(ast);
            win.ShowDialog();
        }

        private Hime.Redist.Lexer.TextLexer GetLexer(Assembly assembly, System.IO.StreamReader reader)
        {
            Type lexerType = assembly.GetType("Hime.Benchmark.Generated.FileCentralDogmaLexer");
            ConstructorInfo lexerConstructor = lexerType.GetConstructor(new Type[] { typeof(System.IO.TextReader) });
            object lexer = lexerConstructor.Invoke(new object[] { reader });
            return lexer as Hime.Redist.Lexer.TextLexer;
        }

        private Hime.Redist.Parsers.BaseLRParser GetParser(Assembly assembly, System.IO.StreamReader reader)
        {
            Hime.Redist.Lexer.TextLexer lexer = GetLexer(assembly, reader);
            Type lexerType = assembly.GetType("Hime.Benchmark.Generated.FileCentralDogmaLexer");
            Type parserType = assembly.GetType("Hime.Benchmark.Generated.FileCentralDogmaParser");
            ConstructorInfo parserConstructor = parserType.GetConstructor(new Type[] { lexerType });
            object parser = parserConstructor.Invoke(new object[] { lexer });
            return parser as Hime.Redist.Parsers.BaseLRParser;
        }
    }
}

