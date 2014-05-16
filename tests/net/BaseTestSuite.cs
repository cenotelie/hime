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
		/// <summary>
		/// The current directory for the current test
		/// </summary>
		protected string directory;

		/// <summary>
		/// Initializes a new instance of the <see cref="Hime.Tests.BaseTestSuite"/> class.
		/// </summary>
		protected BaseTestSuite()
		{
			directory = "Data_" + this.GetType().Name;
			try
			{
				if (Directory.Exists(directory))
					Directory.Delete(directory, true);
				Directory.CreateDirectory(directory);
			}
			catch (IOException)
			{
			}
		}

		/// <summary>
		/// Setups the current directory for the test to come
		/// </summary>
		protected void SetTestDirectory()
		{
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
		protected void ExportResource(string name, string file)
		{
			Stream stream = typeof(BaseTestSuite).Assembly.GetManifestResourceStream("Hime.Tests.Resources." + name);
			StreamReader reader = new StreamReader(stream);
			string content = reader.ReadToEnd();
			reader.Close();
			File.WriteAllText(file, content);
		}

		/// <summary>
		/// Gets the content of the specified resource
		/// </summary>
		/// <param name="name">The name of the resource</param>
		/// <returns>The content of the specified resource</returns>
		protected string GetResource(string name)
		{
			Stream stream = typeof(BaseTestSuite).Assembly.GetManifestResourceStream("Hime.Tests.Resources." + name);
			StreamReader reader = new StreamReader(stream);
			string content = reader.ReadToEnd();
			reader.Close();
			return content;
		}

		/// <summary>
		/// Checks whether the given file exists and is not empty
		/// </summary>
		/// <param name="file">A file name</param>
		/// <returns>True if the file exists and is not empty</returns>
		protected bool CheckFileExists(string file)
		{
			if (!System.IO.File.Exists(file))
				return false;
			System.IO.FileInfo fi = new FileInfo(file);
			return (fi.Length > 0);
		}

		/// <summary>
		/// Checks that the given file contains the given string
		/// </summary>
		/// <param name="file">The file to check</param>
		/// <param name="data">The data to check for in the file</param>
		protected bool CheckFileContains(string file, string data)
		{
			if (!CheckFileExists(file))
				return false;
			string content = System.IO.File.ReadAllText(file);
			return content.Contains(data);
		}
	}
}