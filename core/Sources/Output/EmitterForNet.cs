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
using System.Collections.Generic;
using System.IO;

namespace Hime.SDK.Output
{
	/// <summary>
	/// Represents an emitter of lexer and parser for a given grammar on the .Net platform
	/// </summary>
	public class EmitterForNet : EmitterBase
	{
		/// <summary>
		/// The global random source
		/// </summary>
		private static Random rand = new Random();

		/// <summary>
		/// Gets a unique identifier for generated assemblies
		/// </summary>
		/// <returns>A unique identifier</returns>
		private static string GetUniqueID()
		{
			int i1 = rand.Next();
			int i2 = rand.Next();
			int i3 = rand.Next();
			int i4 = rand.Next();
			return i1.ToString("X8") + "_" + i2.ToString("X8") + "_" + i3.ToString("X8") + "_" + i4.ToString("X8");
		}

		/// <summary>
		/// Gets the suffix for the emitted lexer code files
		/// </summary>
		public override string SuffixLexerCode { get { return "Lexer.cs"; } }
		/// <summary>
		/// Gets suffix for the emitted parser code files
		/// </summary>
		public override string SuffixParserCode { get { return "Parser.cs"; } }
		/// <summary>
		/// Gets suffix for the emitted assemblies
		/// </summary>
		public override string SuffixAssembly { get { return ".dll"; } }

		/// <summary>
		/// Initializes this emitter
		/// </summary>
		/// <param name="units">The units to emit data for</param>
		public EmitterForNet(List<Unit> units) : base(new Reporter(), units)
		{
		}

		/// <summary>
		/// Initializes this emitter
		/// </summary>
		/// <param name="unit">The unit to emit data for</param>
		public EmitterForNet(Unit unit) : base(new Reporter(), unit)
		{
		}

		/// <summary>
		/// Initializes this emitter
		/// </summary>
		/// <param name="reporter">The reporter to use</param>
		/// <param name="units">The units to emit data for</param>
		public EmitterForNet(Reporter reporter, List<Unit> units) : base(reporter, units)
		{
		}

		/// <summary>
		/// Initializes this emitter
		/// </summary>
		/// <param name="reporter">The reporter to use</param>
		/// <param name="unit">The unit to emit data for</param>
		public EmitterForNet(Reporter reporter, Unit unit) : base(reporter, unit)
		{
		}
		
		/// <summary>
		/// Gets the runtime-specific generator of lexer code
		/// </summary>
		/// <param name="unit">The unit to generate a lexer for</param>
		/// <returns>The runtime-specific generator of lexer code</returns>
		protected override Generator GetLexerCodeGenerator(Unit unit)
		{
			return new LexerNetCodeGenerator(unit, unit.Name + suffixLexerData);
		}

		/// <summary>
		/// Gets the runtime-specific generator of parser code
		/// </summary>
		/// <param name="unit">The unit to generate a parser for</param>
		/// <returns>The runtime-specific generator of parser code</returns>
		protected override Generator GetParserCodeGenerator(Unit unit)
		{
			return new ParserNetCodeGenerator(unit, unit.Name + suffixParserData);
		}

		/// <summary>
		/// Emits the assembly for the generated lexer and parser
		/// </summary>
		/// <returns><c>true</c> if the operation succeed</returns>
		protected override bool EmitAssembly()
		{
			reporter.Info("Building assembly " + GetArtifactAssembly() + " ...");
			string redist = System.Reflection.Assembly.GetAssembly(typeof(Hime.Redist.ParseResult)).Location;
			bool hasError = false;
			string output = path + GetUniqueID() + SuffixAssembly;
			using (System.CodeDom.Compiler.CodeDomProvider compiler = System.CodeDom.Compiler.CodeDomProvider.CreateProvider("C#"))
			{
				System.CodeDom.Compiler.CompilerParameters compilerparams = new System.CodeDom.Compiler.CompilerParameters();
				compilerparams.GenerateExecutable = false;
				compilerparams.GenerateInMemory = false;
				compilerparams.ReferencedAssemblies.Add("mscorlib.dll");
				compilerparams.ReferencedAssemblies.Add("System.dll");
				compilerparams.ReferencedAssemblies.Add(redist);
				foreach (Unit unit in units)
				{
					compilerparams.EmbeddedResources.Add(GetArtifactLexerData(unit));
					compilerparams.EmbeddedResources.Add(GetArtifactParserData(unit));
				}
				compilerparams.OutputAssembly = output;
				List<string> files = new List<string>();
				foreach (Unit unit in units)
				{
					files.Add(GetArtifactLexerCode(unit));
					files.Add(GetArtifactParserCode(unit));
				}
				System.CodeDom.Compiler.CompilerResults results = compiler.CompileAssemblyFromFile(compilerparams, files.ToArray());
				foreach (System.CodeDom.Compiler.CompilerError error in results.Errors)
				{
					reporter.Error(error.ToString());
					hasError = true;
				}
			}
			if (hasError)
				return false;
			// stage the output
			if (File.Exists(GetArtifactAssembly()))
				File.Delete(GetArtifactAssembly());
			File.Move(output, GetArtifactAssembly());
			return true;
		}
	}
}