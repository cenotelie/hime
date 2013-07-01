using Hime.CentralDogma;

namespace Hime.Demo.Tasks
{
    class Compile : IExecutable
    {
        public void Execute()
        {
            CompilationTask task = new CompilationTask();
            task.AddInputFile("Languages\\ECMAScript.gram");
            task.Method = ParsingMethod.RNGLALR1;
            task.Namespace = "Hime.Demo.Generated";
            task.OutputLog = true;
            task.OutputDocumentation = true;
            task.Execute();
        }
    }
}
