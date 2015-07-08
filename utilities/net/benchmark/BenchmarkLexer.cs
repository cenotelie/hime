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
using System.Diagnostics;
using System.IO;
using Hime.SDK.Reflection;
using Hime.Redist.Lexer;

namespace Hime.Benchmark
{
	/// <summary>
	/// Represents a benchmark for a lexer
	/// </summary>
	public class BenchmarkLexer : Task
	{
		/// <summary>
		/// Executes this benchmark
		/// </summary>
		/// <param name="assembly">Path to the assembly containing the component to benchmark</param>
		/// <param name="input">Path to the input file</param>
		/// <param name="useStream"><code>true</code> if the input shall be used a stream (instead of a pre-read string)</param>
		public void Execute(string assembly, string input, bool useStream)
		{
			AssemblyReflection asm = new AssemblyReflection(assembly);
			BaseLexer lexer = useStream ? asm.getLexer(new StreamReader(new FileStream(input, FileMode.Open))) : asm.getLexer(File.ReadAllText(input));

			Stopwatch watch = new Stopwatch();
			watch.Start();
			Hime.Redist.Token token = lexer.GetNextToken();
			while (token.Symbol.ID != Hime.Redist.Symbol.SID_DOLLAR)
				token = lexer.GetNextToken();
			watch.Stop();
			Console.WriteLine(watch.ElapsedMilliseconds);
		}
	}
}