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
	class ParseTest : IExecutable
	{
		/// <summary>
		/// Execute this instance.
		/// </summary>
		public void Execute()
		{
			// Build parser assembly
			CompilationTask task = new CompilationTask();
			task.Mode = Hime.SDK.Output.Mode.Assembly;
			task.AddInputRaw("grammar Test {\n\t\toptions {Axiom=\"e\";}\n\t\tterminals {\n\t\t\tX0 -> 'x';\n\t\t\tcontext inner1 { X1 -> 'x'; }\n\t\t\tcontext inner2 { X2 -> 'x'; }\n\t\t}\n\t\trules {\n\t\t\tsub1 -> '('! inner1 ')'!;\n\t\t\tinner1 -> (X1 | sub1 | sub2)* ;\n\t\t\tsub2 -> '['! inner2 ']'!;\n\t\t\tinner2 -> (X2 | sub1 | sub2)*;\n\t\t\te -> (X0 | sub1 | sub2)* ;\n\t\t}\n\t}");
			task.Namespace = "Hime.Demo.Generated";
			task.CodeAccess = Hime.SDK.Output.Modifier.Public;
			task.Method = ParsingMethod.RNGLALR1;
			task.Execute();

			// Load the generated assembly
			AssemblyReflection assembly = new AssemblyReflection(Path.Combine(Environment.CurrentDirectory, "Test.dll"));

			// Re-parse the input grammar with the generated parser
			BaseLRParser parser = assembly.GetParser("x[x(x)]");
			ParseResult result = parser.Parse();

			// Display the errors if any
			Program.PrintErrors(result);
			if (!result.IsSuccess)
				return;
			WinTreeView win = new WinTreeView(result.Root);
			win.ShowDialog();
		}
	}
}
