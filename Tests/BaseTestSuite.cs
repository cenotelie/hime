/**********************************************************************
* Copyright (c) 2013 Laurent Wouters and others
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as
* published by the Free Software Foundation, either version 3
* of the License, or (at your option) any later version.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
* 
* You should have received a copy of the GNU Lesser General
* Public License along with this program.
* If not, see <http://www.gnu.org/licenses/>.
* 
* Contributors:
*     Laurent Wouters - lwouters@xowl.org
**********************************************************************/

using System;
using System.IO;
using System.Reflection;
using Hime.CentralDogma;

namespace Hime.Tests
{
    public abstract class BaseTestSuite
    {
        private const string logName = "Log.txt";
        private const string output = "output";

        private string log;
        private ResourceAccessor accessor;
        private string directory;

        protected BaseTestSuite()
        {
            log = Path.Combine(Environment.CurrentDirectory, logName);
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
                Log(ex.Message);
            }
        }

        protected void Log(string message)
        {
            File.AppendAllText(log, message + Environment.NewLine);
            Console.WriteLine(message);
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

        protected bool CheckFile(string file)
        {
            if (!System.IO.File.Exists(file))
                return false;
            System.IO.FileInfo fi = new FileInfo(file);
            return (fi.Length > 0);
        }

        protected Assembly CompileResource(string resource, ParsingMethod method)
        {
            string gram = accessor.GetAllTextFor(resource + ".gram");
            CompilationTask task = new CompilationTask();
            task.AddInputRaw(gram);
            task.GrammarName = resource;
            task.CodeAccess = AccessModifier.Public;
            task.Method = method;
            task.Mode = CompilationMode.Assembly;
            task.Namespace = "Hime.Tests.Generated";
            task.Execute();
            return Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, resource + ".dll"));
        }
    }
}