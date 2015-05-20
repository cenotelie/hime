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
using Hime.SDK;
using Hime.SDK.Reflection;
using Hime.Redist;
using Hime.Redist.Parsers;

namespace Hime.Demo.Tasks
{
	/// <summary>
	/// This tasks regenerates the parser for the SDK inputs and re-parses the input grammar with the generated parser
	/// </summary>
	class ParseGrammar : IExecutable
	{
		/// <summary>
		/// Execute this instance.
		/// </summary>
		public void Execute()
		{
			// Build parser assembly
			Stream stream = typeof(CompilationTask).Assembly.GetManifestResourceStream("Hime.SDK.Sources.Input.HimeGrammar.gram");
			CompilationTask task = new CompilationTask();
			task.Mode = Hime.SDK.Output.Mode.Assembly;
			task.AddInputRaw(stream);
			task.Namespace = "Hime.Demo.Generated";
			task.GrammarName = "HimeGrammar";
			task.CodeAccess = Hime.SDK.Output.Modifier.Public;
			task.Method = ParsingMethod.LALR1;
			task.Execute();
			stream.Close();

			// Load the generated assembly
			AssemblyReflection assembly = new AssemblyReflection(Path.Combine(Environment.CurrentDirectory, "HimeGrammar.dll"));

			// Re-parse the input grammar with the generated parser
			StreamReader input = new StreamReader(typeof(CompilationTask).Assembly.GetManifestResourceStream("Hime.SDK.Sources.Input.HimeGrammar.gram"));
			BaseLRParser parser = assembly.GetParser(input);
			ParseResult result = parser.Parse();
			input.Close();

			// Display the errors if any
			foreach (ParseError error in result.Errors)
				Console.WriteLine(error);
			if (!result.IsSuccess)
				return;
			WinTreeView win = new WinTreeView(result.Root);
			win.ShowDialog();
		}
	}
}

