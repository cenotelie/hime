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
		public override string SuffixAssembly { get { return "-1.0.0.jar"; } }

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
		/// Gets the radical for the names of the embedded resources
		/// </summary>
		/// <returns>The radical for the names of the embedded resources</returns>
		protected string GetResourceName()
		{
			return "/" + nmspace.Replace(".", "/") + "/" + grammar.Name;
		}

		/// <summary>
		/// Gets the runtime-specific generator of lexer code
		/// </summary>
		/// <param name="separator">The separator terminal</param>
		/// <returns>The runtime-specific generator of lexer code</returns>
		protected override Generator GetLexerCodeGenerator(Grammars.Terminal separator)
		{
			return new LexerJavaCodeGenerator(nmspace, modifier, grammar.Name, GetResourceName() + suffixLexerData, expected, separator);
		}

		/// <summary>
		/// Gets the runtime-specific generator of parser code
		/// </summary>
		/// <param name="parserType">The type of parser to generate</param>
		/// <returns>The runtime-specific generator of parser code</returns>
		protected override Generator GetParserCodeGenerator(string parserType)
		{
			return new ParserJavaCodeGenerator(nmspace, modifier, grammar.Name, GetResourceName() + suffixParserData, grammar, parserType);
		}

		/// <summary>
		/// Emits the assembly for the generated lexer and parser
		/// </summary>
		/// <returns><c>true</c> if the operation succeed</returns>
		protected override bool EmitAssembly()
		{
			reporter.Info("Building assembly " + ArtifactAssembly + " ...");
			// setup the maven project
			CreateMavenProject();
			// compile
			bool success = ExecuteCommand("mvn", "package");
			// extract the result
			if (success)
				File.Move(Path.Combine(Path.Combine(prefix, "target"), grammar.Name + "-1.0.0.jar"), ArtifactAssembly);
			// cleanup the mess ...
			Directory.Delete(Path.Combine(prefix, "src"), true);
			Directory.Delete(Path.Combine(prefix, "res"), true);
			Directory.Delete(Path.Combine(prefix, "target"), true);
			File.Delete(Path.Combine(prefix, "pom.xml"));
			return success;
		}

		/// <summary>
		/// Creates the maven project to compile
		/// </summary>
		private void CreateMavenProject()
		{
			// setup the sources folder
			string bottom = Path.Combine(prefix, "src");
			Directory.CreateDirectory(bottom);
			string[] parts = nmspace.Split(new char[] { '.' }, System.StringSplitOptions.RemoveEmptyEntries);
			for (int i=0; i!=parts.Length; i++)
			{
				bottom = Path.Combine(bottom, parts[i]);
				Directory.CreateDirectory(bottom);
			}
			File.Copy(ArtifactLexerCode, Path.Combine(bottom, grammar.Name + SuffixLexerCode));
			File.Copy(ArtifactParserCode, Path.Combine(bottom, grammar.Name + SuffixParserCode));

			// setup the resources folder
			bottom = Path.Combine(prefix, "res");
			Directory.CreateDirectory(bottom);
			for (int i=0; i!=parts.Length; i++)
			{
				bottom = Path.Combine(bottom, parts[i]);
				Directory.CreateDirectory(bottom);
			}
			File.Copy(ArtifactLexerData, Path.Combine(bottom, grammar.Name + suffixLexerData));
			File.Copy(ArtifactParserData, Path.Combine(bottom, grammar.Name + suffixParserData));

			// export the pom
			ExportResource("Java.pom.xml", Path.Combine(prefix, "pom.xml"));

			// setup the pom
			string content = File.ReadAllText(Path.Combine(prefix, "pom.xml"));
			content = content.Replace("${gen-name}", grammar.Name);
			File.WriteAllText(Path.Combine(prefix, "pom.xml"), content, System.Text.Encoding.UTF8);
		}

		/// <summary>
		/// Exports the resource to the specified file
		/// </summary>
		/// <param name="name">The name of the resource to export</param>
		/// <param name="file">The file to export to</param>
		private void ExportResource(string name, string file)
		{
			System.Reflection.Assembly assembly = (typeof(EmitterForJava)).Assembly;
			BinaryReader reader = new BinaryReader(assembly.GetManifestResourceStream("Hime.CentralDogma.Resources." + name));
			BinaryWriter writer = new BinaryWriter(new FileStream(file, FileMode.Create));
			while (true)
			{
				byte[] buffer = reader.ReadBytes(2048);
				if (buffer == null || buffer.Length == 0)
					break;
				writer.Write(buffer);
			}
			writer.Close();
			reader.Close();
		}

		/// <summary>
		/// Executes the specified command (usually a maven command)
		/// </summary>
		/// <param name="verb">The program to execute</param>
		/// <param name="arguments">The arguments</param>
		/// <returns><c>true</c> if the command succeeded</returns>
		private bool ExecuteCommand(string verb, string arguments)
		{
			reporter.Info("Executing command " + verb + " " + arguments);
			System.Diagnostics.Process process = new System.Diagnostics.Process();
			process.StartInfo.FileName = verb;
			process.StartInfo.Arguments = arguments;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.UseShellExecute = false;
			process.Start();
			bool errors = false;
			while (true)
			{
				string line = process.StandardOutput.ReadLine();
				if (line == null || line.Length == 0)
					break;
				if (line.StartsWith("[ERROR]"))
				{
					reporter.Error(line.Substring(8));
					errors = true;
				}
				else if (line.StartsWith("[WARNING]"))
					reporter.Warn(line.Substring(10));
				else if (line.StartsWith("[INFO]"))
					reporter.Info(line.Substring(7));
				else
					reporter.Info(line);
			}
			process.WaitForExit();
			return !errors;
		}
	}
}