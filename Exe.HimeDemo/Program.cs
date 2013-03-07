/*
 * @author Charles Hymans
 * */

using Hime.Demo.Tasks;
using Hime.CentralDogma;

namespace Hime.Demo
{
    public class Program
    {
        static void Main()
        {
            //IExecutable executable = new Compile("Languages\\MathExp.gram", "Hime.Demo.Generated", ParsingMethod.LALR1);
            //executable.Execute();
            IExecutable executable = new ParseMathExp();
            executable.Execute();
        }
    }
}
