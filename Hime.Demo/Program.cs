namespace Hime.Demo
{
    public class Program
    {        
        static void Main(string[] args)
        {
            IExecutable executable = new Tasks.Compile();
            executable.Execute();
        }
    }
}
