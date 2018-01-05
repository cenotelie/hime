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
	/// Represents an emitter of lexer and parser for a given grammar on the Rust language
	/// </summary>
	public class EmitterForRust : EmitterBase
	{
		/// <summary>
		/// The path to a local Rust target runtime
		/// </summary>
		private string runtime;

		/// <summary>
		/// Gets the suffix for the emitted lexer code files
		/// </summary>
		public override string SuffixLexerCode { get { return ".rs"; } }

		/// <summary>
		/// Gets suffix for the emitted parser code files
		/// </summary>
		public override string SuffixParserCode { get { return ".rs"; } }

		/// <summary>
		/// Gets suffix for the emitted assemblies
		/// </summary>
		public override string SuffixAssembly { get { return ".crate"; } }

		/// <summary>
		/// Initializes this emitter
		/// </summary>
		/// <param name="units">The units to emit data for</param>
		/// <param name="runtime">The path to a local Rust target runtime</param>
		public EmitterForRust(List<Unit> units, string runtime) : base(new Reporter(), units)
		{
			this.runtime = runtime;
		}

		/// <summary>
		/// Initializes this emitter
		/// </summary>
		/// <param name="unit">The unit to emit data for</param>
		/// <param name="runtime">The path to a local Rust target runtime</param>
		public EmitterForRust(Unit unit, string runtime) : base(new Reporter(), unit)
		{
			this.runtime = runtime;
		}

		/// <summary>
		/// Initializes this emitter
		/// </summary>
		/// <param name="reporter">The reporter to use</param>
		/// <param name="units">The units to emit data for</param>
		/// <param name="runtime">The path to a local Rust target runtime</param>
		public EmitterForRust(Reporter reporter, List<Unit> units, string runtime) : base(reporter, units)
		{
			this.runtime = runtime;
		}

		/// <summary>
		/// Initializes this emitter
		/// </summary>
		/// <param name="reporter">The reporter to use</param>
		/// <param name="unit">The unit to emit data for</param>
		/// <param name="runtime">The path to a local Rust target runtime</param>
		public EmitterForRust(Reporter reporter, Unit unit, string runtime) : base(reporter, unit)
		{
			this.runtime = runtime;
		}

		/// <summary>
		/// Gets the runtime-specific generator of lexer code
		/// </summary>
		/// <param name="unit">The unit to generate a lexer for</param>
		/// <returns>The runtime-specific generator of lexer code</returns>
		protected override Generator GetLexerCodeGenerator(Unit unit)
		{
			return new LexerRustCodeGenerator(unit, unit.Name + SUFFIX_LEXER_DATA);
		}

		/// <summary>
		/// Gets the runtime-specific generator of parser code
		/// </summary>
		/// <param name="unit">The unit to generate a parser for</param>
		/// <returns>The runtime-specific generator of parser code</returns>
		protected override Generator GetParserCodeGenerator(Unit unit)
		{
			return new ParserRustCodeGenerator(unit, GetModuleName(unit), unit.Name + SUFFIX_PARSER_DATA);
		}

		/// <summary>
		/// Emits the assembly for the generated lexer and parser
		/// </summary>
		/// <returns><c>true</c> if the operation succeed</returns>
		protected override bool EmitAssembly()
		{
			reporter.Info("Building assembly " + GetArtifactAssembly() + " ...");
			// setup the cargo project
			string target = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			reporter.Info("Assembling into " + target);
			CreateCargoProject(target);
			// compile
			bool success = ExecuteCommandCargo("cargo", "build --manifest-path " + Path.Combine(target, "Cargo.toml"));
			if (!success)
			{
				// build failed for some reason ...
				return false;
			}
			// package without building
			success = ExecuteCommandCargo("cargo", "package --no-verify --manifest-path " + Path.Combine(target, "Cargo.toml"));
			// extract the result
			if (success)
			{
				if (File.Exists(GetArtifactAssembly()))
					File.Delete(GetArtifactAssembly());
				string[] results = Directory.GetFiles(Path.Combine(Path.Combine(target, "target"), "package"), "hime_generated-*.crate");
				File.Move(results[0], GetArtifactAssembly());
			}
			// cleanup the mess ...
			Directory.Delete(target, true);
			return success;
		}

		/// <summary>
		/// Gets the name of the Rust module for the specified unit
		/// </summary>
		/// <param name="unit">The unit</param>
		/// <returns>The name of the Rust module</returns>
		private static string GetModuleName(Unit unit)
		{
			if (unit.Namespace == null)
				return Helper.GetNamespacePartForRust(unit.Grammar.Name);
			string result = Helper.GetNamespaceForRust(unit.Namespace);
			if (result.Length == 0)
				return Helper.GetNamespacePartForRust(unit.Grammar.Name);
			else
				return result + "::" + Helper.GetNamespacePartForRust(unit.Grammar.Name);
		}

		/// <summary>
		/// The data about a cargo module
		/// </summary>
		private struct Module
		{
			/// <summary>
			/// The path to the module file
			/// </summary>
			public readonly string path;
			/// <summary>
			/// The name of the module file
			/// </summary>
			public readonly string file;

			/// <summary>
			/// Initializes this data
			/// </summary>
			/// <param name="path">The path to the module file</param>
			/// <param name="file">The name of the module file</param>
			public Module(string path, string file)
			{
				this.path = path;
				this.file = file;
			}
		}

		/// <summary>
		/// Creates the physical folder for the specified unit
		/// </summary>
		/// <param name="origin">The directory to start from</param>
		/// <param name="unit">The unit</param>
		/// <returns>The resulting target file</returns>
		private static Module CreateModuleFor(string origin, Unit unit)
		{
			string current = origin;
			string[] parts = GetModuleName(unit).Split(new[] { "::" }, System.StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i != parts.Length - 1; i++)
			{
				string target = Path.Combine(current, parts[i]);
				if (!Directory.Exists(target))
				{
					// Creates the directory for the module
					Directory.CreateDirectory(target);
					// Register the module
					string file = i == 0 ? "lib.rs" : "mod.rs";
					File.AppendAllText(Path.Combine(target, file), "pub mod " + parts[i] + ";\n");
					// Creates the module file
					File.Create(Path.Combine(target, "mod.rs")).Close();
				}
				current = target;
			}
			// for the last part
			{
				// Register the module
				string file = parts.Length == 1 ? "lib.rs" : "mod.rs";
				File.AppendAllText(Path.Combine(current, file), "pub mod " + parts[parts.Length - 1] + ";\n");
			}
			return new Module(current, parts[parts.Length - 1] + ".rs");
		}

		/// <summary>
		/// Creates the maven project to compile
		/// </summary>
		/// <param name="path">The path to the target directory</param>
		private void CreateCargoProject(string path)
		{
			// setup the src folder
			Directory.CreateDirectory(path);
			string src = Path.Combine(path, "src");
			Directory.CreateDirectory(src);
			FileStream lib = File.Create(Path.Combine(src, "lib.rs"));
			StreamWriter writer = new StreamWriter(lib);
			writer.WriteLine("//! Generated parsers");
			writer.WriteLine("extern crate hime_redist;");
			writer.Flush();
			lib.Close();
			foreach (Unit unit in units)
			{
				Module module = CreateModuleFor(src, unit);
				File.Copy(GetArtifactLexerCode(unit), Path.Combine(module.path, module.file), true);
				File.Copy(GetArtifactLexerData(unit), Path.Combine(module.path, unit.Name + SUFFIX_LEXER_DATA), true);
				File.Copy(GetArtifactParserData(unit), Path.Combine(module.path, unit.Name + SUFFIX_PARSER_DATA), true);
			}
			// export the toml
			ExportResource("Rust.Cargo.toml", Path.Combine(path, "Cargo.toml"));
			if (runtime != null)
			{
				File.AppendAllText(Path.Combine(path, "Cargo.toml"), "\n\n[patch.crates-io]\nhime_redist = { path = \"" + runtime + "\" }\n");
			}
		}

		/// <summary>
		/// Executes the specified command (usually a maven command)
		/// </summary>
		/// <param name="verb">The program to execute</param>
		/// <param name="arguments">The arguments</param>
		/// <returns><c>true</c> if the command succeeded</returns>
		private bool ExecuteCommandCargo(string verb, string arguments)
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
				if (line.StartsWith("error"))
					errors = true;
				reporter.Info(line);
			}
			process.WaitForExit();
			process.Close();
			return !errors;
		}
	}
}
