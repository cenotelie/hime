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
            //IExecutable executable = new Compile("Languages\\Test2.gram", "Hime.Demo.Generated", ParsingMethod.RNGLALR1);
            //executable.Execute();
            IExecutable executable = new ParseRNGLR();
            executable.Execute();
        }
    }
}
