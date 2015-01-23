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

namespace Hime.Demo.Tasks
{
	/// <summary>
	/// This task demonstrates how to use the SDK API to debug a generated assembly containing a lexer and a parser
	/// </summary>
	class DebugAssembly : IExecutable
	{
		/// <summary>
		///  Execute this instance.
		/// </summary>
		public void Execute()
		{
			// build an assembly to be inspected
			BuildAssembly();
			// load the generated assembly and retrieve the lexer and parser
			Hime.SDK.Reflection.AssemblyReflection assembly = new Hime.SDK.Reflection.AssemblyReflection("Demo.dll");
			Hime.SDK.Reflection.LexerReflection lexer = new Hime.SDK.Reflection.LexerReflection(assembly.GetType("Hime.Demo.Generated.DemoLexer"));
			Hime.SDK.Reflection.ParserReflection parser = new Hime.SDK.Reflection.ParserReflection(assembly.GetType("Hime.Demo.Generated.DemoParser"));
			// Export the automata
			Hime.SDK.Reflection.Serializers.ExportDOT(lexer.DFA, "Demo.Lexer.dot");
			Hime.SDK.Reflection.Serializers.ExportDOT(parser.Automaton, "Demo.Parser.dot");
		}

		/// <summary>
		/// Builds the assembly to be inspected
		/// </summary>
		private void BuildAssembly()
		{
			// Build parser assembly
			string grammar = "grammar Demo { options {Axiom=\"e\";} terminals {K->ub{Katakana}; H->ub{Hiragana};} rules { e->K H; } }";
			Hime.SDK.CompilationTask task = new Hime.SDK.CompilationTask();
			task.Mode = Hime.SDK.Output.Mode.Assembly;
			task.AddInputRaw(grammar);
			task.Namespace = "Hime.Demo.Generated";
			task.GrammarName = "Demo";
			task.CodeAccess = Hime.SDK.Output.Modifier.Public;
			task.Method = Hime.SDK.ParsingMethod.LALR1;
			task.Execute();
		}
	}
}