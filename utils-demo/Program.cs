/*******************************************************************************
 * Copyright (c) 2017 Association Cénotélie (cenotelie.fr)
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
 ******************************************************************************/

using System;
using System.IO;
using Hime.Demo.Tasks;
using Hime.Redist;

namespace Hime.Demo
{
	/// <summary>
	/// Main program
	/// </summary>
	public class Program
	{
		/// <summary>
		/// The entry point of the program, where the program control starts and ends.
		/// </summary>
		static void Main()
		{
			IExecutable executable = new ParseTest();
			executable.Execute();
		}

		/// <summary>
		/// Gets the path to the root of the repository
		/// </summary>
		/// <returns>The path to the root of the repository</returns>
		public static string GetRepoRoot()
		{
			DirectoryInfo current = new DirectoryInfo(Environment.CurrentDirectory);
			DirectoryInfo[] subs = current.GetDirectories("core");
			while (subs == null || subs.Length == 0)
			{
				current = current.Parent;
				subs = current.GetDirectories("core");
			}
			return current.FullName;
		}

		/// <summary>
		/// Prints the errors in the specified parse result, if any
		/// </summary>
		/// <param name="result">The result of a parsing task</param>
		public static void PrintErrors(ParseResult result)
		{
			foreach (ParseError error in result.Errors)
			{
				Console.WriteLine("[ERROR] " + error.Message);
				TextContext context = result.Input.GetContext(error.Position, error.Length);
				Console.Write("\t");
				Console.WriteLine(context.Content);
				Console.Write("\t");
				Console.WriteLine(context.Pointer);
			}
		}
	}
}
