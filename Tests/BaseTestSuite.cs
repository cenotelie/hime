using System;
using System.IO;
using System.Reflection;
using Hime.CentralDogma;

namespace Hime.Tests
{
    public abstract class BaseTestSuite
    {
        protected const string log = "Log.txt";
        protected const string output = "output";

        protected ResourceAccessor accessor;
        protected string directory;

        protected BaseTestSuite()
        {
            accessor = new ResourceAccessor(Assembly.GetExecutingAssembly(), "Resources");
            directory = "Data_" + this.GetType().Name;
            try
            {
                if (Directory.Exists(directory))
                    Directory.Delete(directory, true);
                Directory.CreateDirectory(directory);
            }
            catch (IOException ex)
            {
                File.AppendAllText(log, ex.Message + Environment.NewLine);
            }
        }

        protected void SetTestDirectory() {
            System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace();
            System.Diagnostics.StackFrame caller = trace.GetFrame(1);
            string dir = Path.Combine(directory, caller.GetMethod().Name);
            if (Directory.Exists(dir))
                Directory.Delete(dir, true);
            Directory.CreateDirectory(dir);
            Environment.CurrentDirectory = dir;
        }

        protected void ExportResource(string name, string file) { accessor.Export(name, file); }
    }
}