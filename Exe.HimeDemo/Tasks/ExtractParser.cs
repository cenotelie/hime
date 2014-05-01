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
using Hime.CentralDogma.SDK;

namespace Hime.Demo.Tasks
{
	/// <summary>
	/// This task generates a parser and uses the SDK API to outputs the generated lexer and parser
	/// </summary>
	class ExtractParser : IExecutable
	{
		/// <summary>
		///  Execute this instance.
		/// </summary>
		public void Execute()
		{
			// Build parser assembly
			string grammar = "cf grammar Demo { options {Axiom=\"e\";} terminals {K->ub{Katakana}; H->ub{Hiragana};} rules { e->K H; } }";
			CompilationTask task = new CompilationTask();
			task.Mode = CompilationMode.Assembly;
			task.AddInputRaw(grammar);
			task.Namespace = "Hime.Demo.Generated";
			task.GrammarName = "Demo";
			task.CodeAccess = AccessModifier.Public;
			task.Method = ParsingMethod.LALR1;
			task.Execute();

			// Load the generated assembly and retrieve the lexer
			AssemblyReflection assembly = new AssemblyReflection("Demo.dll");
			LexerReflection lexer = new LexerReflection(assembly.GetLexerType("Hime.Demo.Generated.DemoLexer"));
			ParserReflection parser = new ParserReflection(assembly.GetParserType("Hime.Demo.Generated.DemoParser"));
			// Export the automata
			GraphSerializer.ExportDOT(lexer.DFA, "Demo.Lexer.dot");
			GraphSerializer.ExportDOT(parser.Automaton, "Demo.Parser.dot");
		}
	}
}