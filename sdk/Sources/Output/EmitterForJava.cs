/*******************************************************************************
 * Copyright (c) 2017 Association Cénotélie (cenotelie.fr)
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
 ******************************************************************************/

using System.Collections.Generic;
using System.IO;

namespace Hime.SDK.Output
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
		/// <param name="units">The units to emit data for</param>
		public EmitterForJava(List<Unit> units) : base(new Reporter(), units)
		{
		}

		/// <summary>
		/// Initializes this emitter
		/// </summary>
		/// <param name="unit">The unit to emit data for</param>
		public EmitterForJava(Unit unit) : base(new Reporter(), unit)
		{
		}

		/// <summary>
		/// Initializes this emitter
		/// </summary>
		/// <param name="reporter">The reporter to use</param>
		/// <param name="units">The units to emit data for</param>
		public EmitterForJava(Reporter reporter, List<Unit> units) : base(reporter, units)
		{
		}

		/// <summary>
		/// Initializes this emitter
		/// </summary>
		/// <param name="reporter">The reporter to use</param>
		/// <param name="unit">The unit to emit data for</param>
		public EmitterForJava(Reporter reporter, Unit unit) : base(reporter, unit)
		{
		}

		/// <summary>
		/// Gets the runtime-specific generator of lexer code
		/// </summary>
		/// <param name="unit">The unit to generate a lexer for</param>
		/// <returns>The runtime-specific generator of lexer code</returns>
		protected override Generator GetLexerCodeGenerator(Unit unit)
		{
			return new LexerJavaCodeGenerator(unit, unit.Name + SUFFIX_LEXER_DATA);
		}

		/// <summary>
		/// Gets the runtime-specific generator of parser code
		/// </summary>
		/// <param name="unit">The unit to generate a parser for</param>
		/// <returns>The runtime-specific generator of parser code</returns>
		protected override Generator GetParserCodeGenerator(Unit unit)
		{
			return new ParserJavaCodeGenerator(unit, unit.Name + SUFFIX_PARSER_DATA);
		}

		/// <summary>
		/// Emits the assembly for the generated lexer and parser
		/// </summary>
		/// <returns><c>true</c> if the operation succeed</returns>
		protected override bool EmitAssembly()
		{
			reporter.Info("Building assembly " + GetArtifactAssembly() + " ...");
			// setup the maven project
			string projectFolder = CreateMavenProject();
			// compile
			System.PlatformID platform = System.Environment.OSVersion.Platform;
			bool success;
			if (platform == System.PlatformID.Unix || platform == System.PlatformID.MacOSX)
				success = ExecuteCommandMvn("mvn", "package -f " + Path.Combine(projectFolder, "pom.xml"));
			else
				success = ExecuteCommandMvn("cmd.exe", "/c mvn.cmd package -f " + Path.Combine(projectFolder, "pom.xml"));
			// extract the result
			if (success)
			{
				if (File.Exists(GetArtifactAssembly()))
					File.Delete(GetArtifactAssembly());
				string[] results = Directory.GetFiles(Path.Combine(projectFolder, "target"), "hime-generated-*.jar");
				File.Move(results[0], GetArtifactAssembly());
			}
			// cleanup the mess ...
			Directory.Delete(projectFolder, true);
			return success;
		}

		/// <summary>
		/// Creates the physical folder for the specified unit
		/// </summary>
		/// <param name="origin">The directory to start from</param>
		/// <param name="unit">The unit</param>
		/// <returns>The resulting folder</returns>
		private static string CreateFolderFor(string origin, Unit unit)
		{
			string current = origin;
			string[] parts = Helper.GetNamespaceForJava(unit.Namespace == null ? unit.Grammar.Name : unit.Namespace).Split(new[] { '.' }, System.StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i != parts.Length; i++)
			{
				current = Path.Combine(current, parts[i]);
				if (!Directory.Exists(current))
					Directory.CreateDirectory(current);
			}
			return current;
		}

		/// <summary>
		/// Creates the maven project to compile
		/// </summary>
		/// <returns>The resulting folder</returns>
		private string CreateMavenProject()
		{
			string target = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			reporter.Info("Assembling into " + target);
			// setup the src folder
			string src = Path.Combine(target, "src");
			foreach (Unit unit in units)
			{
				string folder = CreateFolderFor(src, unit);
				File.Copy(GetArtifactLexerCode(unit), Path.Combine(folder, unit.Name + SuffixLexerCode), true);
				File.Copy(GetArtifactParserCode(unit), Path.Combine(folder, unit.Name + SuffixParserCode), true);
			}

			// setup the res folder
			string res = Path.Combine(target, "res");
			foreach (Unit unit in units)
			{
				string folder = CreateFolderFor(res, unit);
				File.Copy(GetArtifactLexerData(unit), Path.Combine(folder, unit.Name + SUFFIX_LEXER_DATA), true);
				File.Copy(GetArtifactParserData(unit), Path.Combine(folder, unit.Name + SUFFIX_PARSER_DATA), true);
			}

			// export the pom
			ExportResource("Java.pom.xml", Path.Combine(target, "pom.xml"));
			return target;
		}

		/// <summary>
		/// Executes the specified command (usually a maven command)
		/// </summary>
		/// <param name="verb">The program to execute</param>
		/// <param name="arguments">The arguments</param>
		/// <returns><c>true</c> if the command succeeded</returns>
		private bool ExecuteCommandMvn(string verb, string arguments)
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
				if (string.IsNullOrEmpty(line))
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
			process.Close();
			return !errors;
		}
	}
}
