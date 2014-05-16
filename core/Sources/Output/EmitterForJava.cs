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
using System.Collections.Generic;
using System.IO;

namespace Hime.CentralDogma.Output
{
	/// <summary>
	/// Represents an emitter of lexer and parser for a given grammar on the Java platform
	/// </summary>
	public class EmitterForJava : EmitterBase
	{
		/// <summary>
		/// Gets the suffix for the emitted lexer code files
		/// </summary>
		public override string SuffixLexerCode { get { return "Lexer.java"; } }
		/// <summary>
		/// Gets suffix for the emitted parser code files
		/// </summary>
		public override string SuffixParserCode { get { return "Parser.java"; } }
		/// <summary>
		/// Gets suffix for the emitted assemblies
		/// </summary>
		public override string SuffixAssembly { get { return ".jar"; } }

		/// <summary>
		/// Initializes this emitter
		/// </summary>
		/// <param name="grammar">The grammar to emit data for</param>
		public EmitterForJava(Grammars.Grammar grammar) : base(grammar) { }
		/// <summary>
		/// Initializes this emitter
		/// </summary>
		/// <param name="reporter">The reporter to use</param>
		/// <param name="grammar">The grammar to emit data for</param>
		public EmitterForJava(Reporter reporter, Grammars.Grammar grammar) : base(reporter, grammar) { }


		/// <summary>
		/// Gets the runtime-specific generator of lexer code
		/// </summary>
		/// <param name="separator">The separator terminal</param>
		/// <returns>The runtime-specific generator of lexer code</returns>
		protected override Generator GetLexerCodeGenerator(Grammars.Terminal separator)
		{
			return new LexerJavaCodeGenerator(nmspace, modifier, grammar.Name, prefix + suffixLexerData, expected, separator);
		}

		/// <summary>
		/// Gets the runtime-specific generator of parser code
		/// </summary>
		/// <param name="parserType">The type of parser to generate</param>
		/// <returns>The runtime-specific generator of parser code</returns>
		protected override Generator GetParserCodeGenerator(string parserType)
		{
			return new ParserJavaCodeGenerator(nmspace, modifier, grammar.Name, prefix + suffixParserData, grammar, parserType);
		}

		/// <summary>
		/// Emits the assembly for the generated lexer and parser
		/// </summary>
		/// <returns><c>true</c> if the operation succeed</returns>
		protected override bool EmitAssembly()
		{
			return false;
		}
	}
}