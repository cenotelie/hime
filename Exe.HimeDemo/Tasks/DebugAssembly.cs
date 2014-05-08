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
			Hime.CentralDogma.SDK.AssemblyReflection assembly = new Hime.CentralDogma.SDK.AssemblyReflection("Demo.dll");
			Hime.CentralDogma.SDK.LexerReflection lexer = new Hime.CentralDogma.SDK.LexerReflection(assembly.GetLexerType("Hime.Demo.Generated.DemoLexer"));
			Hime.CentralDogma.SDK.ParserReflection parser = new Hime.CentralDogma.SDK.ParserReflection(assembly.GetParserType("Hime.Demo.Generated.DemoParser"));
			// Export the automata
			Hime.CentralDogma.SDK.Serializers.ExportDOT(lexer.DFA, "Demo.Lexer.dot");
			Hime.CentralDogma.SDK.Serializers.ExportDOT(parser.Automaton, "Demo.Parser.dot");
		}

		/// <summary>
		/// Builds the assembly to be inspected
		/// </summary>
		private void BuildAssembly()
		{
			// Build parser assembly
			string grammar = "grammar Demo { options {Axiom=\"e\";} terminals {K->ub{Katakana}; H->ub{Hiragana};} rules { e->K H; } }";
			Hime.CentralDogma.CompilationTask task = new Hime.CentralDogma.CompilationTask();
			task.Mode = Hime.CentralDogma.Output.Mode.Assembly;
			task.AddInputRaw(grammar);
			task.Namespace = "Hime.Demo.Generated";
			task.GrammarName = "Demo";
			task.CodeAccess = Hime.CentralDogma.Output.Modifier.Public;
			task.Method = Hime.CentralDogma.ParsingMethod.LALR1;
			task.Execute();
		}
	}
}