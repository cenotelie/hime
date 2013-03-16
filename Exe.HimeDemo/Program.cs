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
            IExecutable executable = new ParseTest();
            executable.Execute();
        }
    }
}
