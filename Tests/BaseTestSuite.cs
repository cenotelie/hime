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
using NUnit.Framework;

namespace Hime.Tests
{
	/// <summary>
	/// Represents a base test suite with helper methods
	/// </summary>
    public abstract class BaseTestSuite
    {
        private const string logName = "Log.txt";
        private const string output = "output";

        /// <summary>
        /// The log file for the tests
        /// </summary>
        private string log;
        /// <summary>
        /// The resource accessor for this test bundle
        /// </summary>
        private ResourceAccessor accessor;
        /// <summary>
        /// The current directory for the current test
        /// </summary>
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

        /// <summary>
        /// Logs the given message
        /// </summary>
        /// <param name="message">A message</param>
        protected void Log(string message)
        {
            File.AppendAllText(log, message + Environment.NewLine);
            Console.WriteLine(message);
        }

        /// <summary>
        /// Setups the current directory for the test to come
        /// </summary>
        protected void SetTestDirectory() {
            System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace();
            System.Diagnostics.StackFrame caller = trace.GetFrame(1);
            string dir = Path.Combine(directory, caller.GetMethod().Name);
            if (Directory.Exists(dir))
                Directory.Delete(dir, true);
            Directory.CreateDirectory(dir);
            Environment.CurrentDirectory = dir;
        }

        /// <summary>
        /// Exports the given resource to the given file
        /// </summary>
        /// <param name="name">The name of the resource to export</param>
        /// <param name="file">The file to export to</param>
        protected void ExportResource(string name, string file) { accessor.Export(name, file); }

        /// <summary>
        /// Checks whether the given file exists and is not empty
        /// </summary>
        /// <param name="file">A file name</param>
        /// <returns>True if the file exists and is not empty</returns>
        protected bool CheckFile(string file)
        {
            if (!System.IO.File.Exists(file))
                return false;
            System.IO.FileInfo fi = new FileInfo(file);
            return (fi.Length > 0);
        }

        /// <summary>
        /// Compiles the given resource grammar for the given parsing method, generates an assembly and loads it
        /// </summary>
        /// <param name="resource">The resource to compile</param>
        /// <param name="method">The parsing method to generate</param>
        /// <returns>The resulting loaded assembly</returns>
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
            Hime.CentralDogma.Reporting.Report report = task.Execute();
            Assert.AreEqual(0, report.ErrorCount, "himecc failed to compile the resource");
            Assert.IsTrue(CheckFile(resource + ".dll"), "himecc failed to produce " + resource + ".dll");
            return Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, resource + ".dll"));
        }
    }
}