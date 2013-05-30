using System;
using System.CodeDom.Compiler;
using System.Reflection;
using Hime.CentralDogma;
using Hime.Redist.Lexer;

namespace Hime.Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            Benchmark bench = new Benchmark();
            bench.Run();
        }
    }
}
