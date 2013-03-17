/*
 * Author: Laurent WOUTERS
 * */
using System;
using System.IO;
using System.Reflection;
using Hime.CentralDogma;
using Hime.Redist.Parsers;

namespace Hime.Demo.Tasks
{
    class ParseTest : IExecutable
    {
        public void Execute()
        {
            // Build parser assembly
            CompilationTask task = new CompilationTask();
            task.Mode = CompilationMode.Assembly;
            task.AddInputFile("Languages\\Test2.gram");
            task.Namespace = "Hime.Demo.Generated";
            task.GrammarName = "Test2";
            task.CodeAccess = AccessModifier.Public;
            task.Method = ParsingMethod.RNGLALR1;
            task.OutputDocumentation = true;
            task.OutputLog = true;
            task.Execute();
            Assembly assembly = Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, "Test2.dll"));

            System.IO.StreamReader reader = new System.IO.StreamReader("Languages\\FileCentralDogma.gram");
            Hime.Redist.Parsers.BaseLRParser parser = GetParser(assembly, new StringReader("aa"));
            Redist.AST.ASTNode root = parser.Parse();
            reader.Close();
            
            foreach (ParserError error in parser.Errors)
                Console.WriteLine(error.ToString());
            if (root == null)
                return;
            WinTreeView win = new WinTreeView(root);
            win.ShowDialog();
        }

        private Hime.Redist.Lexer.TextLexer GetLexer(Assembly assembly, System.IO.TextReader reader)
        {
            Type lexerType = assembly.GetType("Hime.Demo.Generated.Test2Lexer");
            ConstructorInfo lexerConstructor = lexerType.GetConstructor(new Type[] { typeof(System.IO.TextReader) });
            object lexer = lexerConstructor.Invoke(new object[] { reader });
            return lexer as Hime.Redist.Lexer.TextLexer;
        }

        private Hime.Redist.Parsers.BaseLRParser GetParser(Assembly assembly, System.IO.TextReader reader)
        {
            Hime.Redist.Lexer.TextLexer lexer = GetLexer(assembly, reader);
            Type lexerType = assembly.GetType("Hime.Demo.Generated.Test2Lexer");
            Type parserType = assembly.GetType("Hime.Demo.Generated.Test2Parser");
            ConstructorInfo parserConstructor = parserType.GetConstructor(new Type[] { lexerType });
            object parser = parserConstructor.Invoke(new object[] { lexer });
            return parser as Hime.Redist.Parsers.BaseLRParser;
        }
    }
}

