/**********************************************************************
* Copyright (c) 2014 Laurent Wouters and others
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
using Hime.CentralDogma.SDK;
using Hime.Redist;
using Hime.Redist.Parsers;

namespace Hime.Demo.Tasks
{
	/// <summary>
	/// This tasks tests the parsing of C# source files
	/// </summary>
	class ParseCSharp : IExecutable
	{
		/// <summary>
		/// Execute this instance.
		/// </summary>
		public void Execute()
		{
			string rootDir = Program.GetRepoRoot();

			// Build parser assembly
			Stream stream = new FileStream(rootDir + Path.DirectorySeparatorChar + "extras" + Path.DirectorySeparatorChar + "Grammars" + Path.DirectorySeparatorChar + "ECMA_CSharp.gram", FileMode.Open);
			CompilationTask task = new CompilationTask();
			task.Mode = Hime.CentralDogma.Output.Mode.Assembly;
			task.AddInputRaw("ECMA_CSharp.gram", stream);
			task.Namespace = "Hime.Demo.Generated";
			task.GrammarName = "CSharp";
			task.CodeAccess = Hime.CentralDogma.Output.Modifier.Public;
			task.Method = ParsingMethod.RNGLALR1;
			Report report = task.Execute();
			stream.Close();
			if (report.Errors.Count > 0)
				return;

			// Load the generated assembly
			AssemblyReflection assembly = new AssemblyReflection(Path.Combine(Environment.CurrentDirectory, "CSharp.dll"));

			ParseIn(assembly, rootDir + Path.DirectorySeparatorChar + "runtimes" + Path.DirectorySeparatorChar + "net" + Path.DirectorySeparatorChar + "Sources");
			ParseIn(assembly, rootDir + Path.DirectorySeparatorChar + "core" + Path.DirectorySeparatorChar + "Sources");
		}

		/// <summary>
		/// Recusively parses the C# sources in the specified folder and its sub-folders
		/// </summary>
		/// <param name="assembly">The compiled assembly containing the C# parser</param>
		/// <param name="folder">The folder to parse C# sources in</param>
		private void ParseIn(AssemblyReflection assembly, string folder)
		{
			string[] files = Directory.GetFiles(folder);
			foreach (string file in files)
			{
				if (file.EndsWith(".cs"))
				{
					Console.WriteLine("== Parsing " + Path.Combine(folder, file));
					StreamReader input = new StreamReader(new FileStream(Path.Combine(folder, file), FileMode.Open));
					IParser parser = assembly.GetParser(input);
					ParseResult result = parser.Parse();
					input.Close();
					foreach (ParseError error in result.Errors)
					{
						Console.WriteLine("[ERROR] " + error.Message);
						Context context = result.Input.GetContext(error.Position);
						Console.Write("\t");
						Console.WriteLine(context.Content);
						Console.Write("\t");
						Console.WriteLine(context.Pointer);
					}
				}
			}
			string[] subs = Directory.GetDirectories(folder);
			foreach (string sub in subs)
				ParseIn(assembly, Path.Combine(folder, sub));
		}
	}
}