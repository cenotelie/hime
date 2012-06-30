/*
 * @author Charles Hymans
 * */

using Hime.Demo.Tasks;

namespace Hime.Demo
{
    public class Program
    {
        static void Main()
        {
            /*string content = System.IO.File.ReadAllText("Languages\\CSharp4.gram");
            for (int i = 0; i != 300; i++)
                System.IO.File.AppendAllText("Perf.gram", content);*/

            /*Generated.CD.FileCentralDogmaLexer lexer = new Generated.CD.FileCentralDogmaLexer(new System.IO.StreamReader("Perf.gram"));
            int count = 1;
            System.DateTime before = System.DateTime.Now;
            Hime.Redist.Parsers.SymbolToken token = lexer.GetNextToken();
            while (token.SymbolID != 1)
            {
                token = lexer.GetNextToken();
                count++;
            }
            System.DateTime after = System.DateTime.Now;
            System.TimeSpan span = after.Subtract(before);
            System.Console.Out.WriteLine("Scanned " + count + " in " + (int)span.TotalMilliseconds + "ms");
            System.Console.In.ReadLine();*/

            Generated.CD.FileCentralDogmaLexer lexer = new Generated.CD.FileCentralDogmaLexer(new System.IO.StreamReader("Perf.gram"));
            Generated.CD.FileCentralDogmaParser parser = new Generated.CD.FileCentralDogmaParser(lexer);
            System.DateTime before = System.DateTime.Now;
            parser.Parse();
            System.DateTime after = System.DateTime.Now;
            System.TimeSpan span = after.Subtract(before);
            System.Console.Out.WriteLine((int)span.TotalMilliseconds + "ms");
            System.Console.In.ReadLine();

            //IExecutable executable = new CompileCentralDogma();
            //executable.Execute();
        }
    }
}
