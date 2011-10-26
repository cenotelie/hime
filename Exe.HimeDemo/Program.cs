/*
 * @author Charles Hymans
 * */

using Hime.Demo.Tasks;

namespace Hime.Demo
{
    public class Program
    {        
        static void Main(string[] args)
        {
            IExecutable executable = new Compile();
            executable.Execute();
        }
    }
}
