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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

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
			return new LexerNetCodeGenerator(unit, unit.Name + SUFFIX_LEXER_DATA);
		}

		/// <summary>
		/// Gets the runtime-specific generator of parser code
		/// </summary>
		/// <param name="unit">The unit to generate a parser for</param>
		/// <returns>The runtime-specific generator of parser code</returns>
		protected override Generator GetParserCodeGenerator(Unit unit)
		{
			return new ParserNetCodeGenerator(unit, unit.Name + SUFFIX_PARSER_DATA);
		}

		/// <summary>
		/// Emits the assembly for the generated lexer and parser
		/// </summary>
		/// <returns><c>true</c> if the operation succeed</returns>
		protected override bool EmitAssembly()
		{
			// .Net Core => use the SDK
			if (IsRunninOnNetCore())
				return EmitAssemblyNetCoreSDK();
			// fallback to the framework compiler, if available
			return EmitAssemblyFxCompiler();
		}

		/// <summary>
		/// Get whether the current process is being run on the .Net Core framework
		/// </summary>
		/// <returns><c>true</c> if the current framework is .Net Core</returns>
		private static bool IsRunninOnNetCore()
		{
#if NETSTANDARD2_0
			return (RuntimeInformation.FrameworkDescription.StartsWith(".NET Core"));
#else
			return Process.GetCurrentProcess().MainModule.ModuleName == "dotnet";
#endif
		}

		/// <summary>
		/// Emits the assembly for the generated lexer and parser using the runtime's compiler
		/// </summary>
		/// <returns><c>true</c> if the operation succeed</returns>
		private bool EmitAssemblyFxCompiler()
		{
			reporter.Info("Building assembly " + GetArtifactAssembly() + " ...");
			string fileRedist = System.Reflection.Assembly.GetAssembly(typeof(Hime.Redist.ParseResult)).Location;
			string fileNetstandard = Path.Combine(Path.GetDirectoryName(fileRedist), "netstandard.dll");
			bool hasError = false;
			string output = OutputPath + GetUniqueID() + SuffixAssembly;
			using (System.CodeDom.Compiler.CodeDomProvider compiler = System.CodeDom.Compiler.CodeDomProvider.CreateProvider("C#"))
			{
				System.CodeDom.Compiler.CompilerParameters compilerparams = new System.CodeDom.Compiler.CompilerParameters();
				compilerparams.GenerateExecutable = false;
				compilerparams.GenerateInMemory = false;
				compilerparams.ReferencedAssemblies.Add(fileRedist);
				compilerparams.ReferencedAssemblies.Add(fileNetstandard);
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

		/// <summary>
		/// Emits the assembly for the generated lexer and parser using .Net Core SDK
		/// </summary>
		/// <returns><c>true</c> if the operation succeed</returns>
		private bool EmitAssemblyNetCoreSDK()
		{
			reporter.Info("Building assembly " + GetArtifactAssembly() + " ...");
			// setup the .Net Core project
			string projectFolder = CreateNetCoreProject();
			// compile
			System.PlatformID platform = System.Environment.OSVersion.Platform;
			bool success = ExecuteCommandDotnet("dotnet", projectFolder, "restore");
			if (!success)
				return false;
			success = ExecuteCommandDotnet("dotnet", projectFolder, "build -c Release");
			if (!success)
				return false;
			// extract the result
			if (File.Exists(GetArtifactAssembly()))
				File.Delete(GetArtifactAssembly());
			File.Move(Path.Combine(Path.Combine(Path.Combine(Path.Combine(projectFolder, "bin"), "Release"), "netstandard2.0"), "Hime.Generated.dll"), GetArtifactAssembly());
			// cleanup the mess ...
			Directory.Delete(projectFolder, true);
			return success;
		}

		/// <summary>
		/// Creates the .Net Core project to compile
		/// </summary>
		/// <returns>The path to the project</returns>
		private string CreateNetCoreProject()
		{
			// setup the Sources folder
			string folder = Path.Combine(OutputPath, GetUniqueID());
			if (!Directory.Exists(folder))
				Directory.CreateDirectory(folder);
			foreach (Unit unit in units)
			{
				File.Copy(GetArtifactLexerCode(unit), Path.Combine(folder, unit.Name + SuffixLexerCode), true);
				File.Copy(GetArtifactParserCode(unit), Path.Combine(folder, unit.Name + SuffixParserCode), true);
				File.Copy(GetArtifactLexerData(unit), Path.Combine(folder, unit.Name + SUFFIX_LEXER_DATA), true);
				File.Copy(GetArtifactParserData(unit), Path.Combine(folder, unit.Name + SUFFIX_PARSER_DATA), true);
			}
			// export the csproj file
			ExportResource("NetCore.parser.csproj", Path.Combine(folder, "parser.csproj"));
			return folder;
		}

		/// <summary>
		/// Executes the specified command (usually a dotnet command)
		/// </summary>
		/// <param name="verb">The program to execute</param>
		/// <param name="projectFolder">The path to the project folder</param>
		/// <param name="arguments">The arguments</param>
		/// <returns><c>true</c> if the command succeeded</returns>
		private bool ExecuteCommandDotnet(string verb, string projectFolder, string arguments)
		{
			reporter.Info("Executing command " + verb + " " + arguments);
			System.Diagnostics.Process process = new System.Diagnostics.Process();
			process.StartInfo.FileName = verb;
			process.StartInfo.Arguments = arguments;
			process.StartInfo.RedirectStandardOutput = true;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.WorkingDirectory = projectFolder;
			process.Start();
			bool errors = false;
			while (true)
			{
				string line = process.StandardOutput.ReadLine();
				if (line == null)
					break;
				if (line.Contains("FAILED"))
					errors = true;
				reporter.Info(line);
			}
			process.WaitForExit();
			process.Close();
			return !errors;
		}
	}
}