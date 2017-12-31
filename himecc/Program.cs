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
using Hime.CLI;
using Hime.Redist;
using Hime.SDK;

namespace Hime.HimeCC
{
	/// <summary>
	/// Entry class for the himecc program
	/// </summary>
	public sealed class Program
	{
		private const string ArgHelpShort = "-h";
		private const string ArgHelpLong = "--help";
		private const string ArgRegenerateShort = "-r";
		private const string ArgRegenerateLong = "--regenerate";
		private const string ArgOutputAssembly = "-o:assembly";
		private const string ArgOutputNoSources = "-o:nosources";
		private const string ArgOutputDebug = "-o:debug";
		private const string ArgTargetJava = "-t:java";
		private const string ArgTargetRust = "-t:rust";
		private const string ArgGrammar = "-g";
		private const string ArgPath = "-p";
		private const string ArgMethodRNGLR = "-m:rnglr";
		private const string ArgNamespace = "-n";
		private const string ArgAccessPublic = "-a:public";
		private const string ErrorParsingArgs = "Error while parsing the arguments.";
		private const string ErrorBadArgs = "Incorrect arguments.";
		private const string ErrorPointHelp = "Run without arguments for help.";

		/// <summary>
		/// The program ran without errors
		/// </summary>
		public const int ResultOK = 0;
		/// <summary>
		/// Error while parsing the arguments
		/// </summary>
		public const int ResultErrorParsingArgs = 1;
		/// <summary>
		/// Incorrect arguments
		/// </summary>
		public const int ResultErrorBadArgs = 2;
		/// <summary>
		/// Error while compiling the grammar
		/// </summary>
		public const int ResultErrorCompiling = 3;



		/// <summary>
		/// Executes the himecc program
		/// </summary>
		/// <param name="args">The command line arguments</param>
		/// <returns>The error code, or 0 if none</returns>
		public static int Main(string[] args)
		{
			// If no argument is given, print the help screen and return OK
			if (args == null || args.Length == 0)
			{
				PrintHelp();
				return ResultOK;
			}

			// Parse the arguments
			ParseResult result = CommandLine.ParseArguments(args);
			foreach (ParseError error in result.Errors)
				Console.WriteLine(error.Message);
			if (!result.IsSuccess || result.Errors.Count > 0)
			{
				Console.WriteLine(ErrorParsingArgs);
				Console.WriteLine(ErrorPointHelp);
				return ResultErrorParsingArgs;
			}

			// Check for special switches
			string special = GetSpecialCommand(result.Root);
			if (special == ArgHelpShort || special == ArgHelpLong)
			{
				PrintHelp();
				return ResultOK;
			}
			else if (special == ArgRegenerateShort || special == ArgRegenerateLong)
			{
				GenerateCLParser();
				GenerateCDParser();
				UnicodeHelper.GenerateBlocksDB();
				UnicodeHelper.GenerateCategoriesDB();
				UnicodeHelper.GenerateBlocksTests();
				return ResultOK;
			}

			// Build the compilation task
			CompilationTask task = BuildTask(result.Root);
			if (task == null)
			{
				Console.WriteLine(ErrorBadArgs);
				Console.WriteLine(ErrorPointHelp);
				return ResultErrorBadArgs;
			}

			// Execute the task
			Report report = task.Execute();
			return report.Errors.Count != 0 ? ResultErrorCompiling : ResultOK;
		}

		/// <summary>
		/// Generates the parser for the command line
		/// </summary>
		/// <returns>The number of errors (should be 0)</returns>
		private static int GenerateCLParser()
		{
			System.IO.Stream stream = typeof(CompilationTask).Assembly.GetManifestResourceStream("Hime.CLI.CommandLine.gram");
			CompilationTask task = new CompilationTask();
			task.Mode = Hime.SDK.Output.Mode.Source;
			task.AddInputRaw(stream);
			task.Namespace = "Hime.CLI";
			task.CodeAccess = Hime.SDK.Output.Modifier.Internal;
			task.Method = ParsingMethod.LALR1;
			Report report = task.Execute();
			return report.Errors.Count;
		}

		/// <summary>
		/// Generates the parser for the input files of this compiler (.gram files)
		/// </summary>
		/// <returns>The number of errors (should be 0)</returns>
		private static int GenerateCDParser()
		{
			System.IO.Stream stream = typeof(CompilationTask).Assembly.GetManifestResourceStream("Hime.SDK.Sources.Input.HimeGrammar.gram");
			CompilationTask task = new CompilationTask();
			task.Mode = Hime.SDK.Output.Mode.Source;
			task.AddInputRaw(stream);
			task.GrammarName = "HimeGrammar";
			task.Namespace = "Hime.SDK.Input";
			task.CodeAccess = Hime.SDK.Output.Modifier.Internal;
			task.Method = ParsingMethod.LALR1;
			Report report = task.Execute();
			return report.Errors.Count;
		}

		/// <summary>
		/// Gets the name of the first argument if there is one and it is not preceded by any value
		/// </summary>
		/// <param name="line">The AST representation of the command line</param>
		/// <returns>The name of the first argument, or null if none</returns>
		private static string GetSpecialCommand(ASTNode line)
		{
			if (line.Children[0].Children.Count == 0 && line.Children[1].Children.Count == 1)
				return line.Children[1].Children[0].Value;
			return null;
		}

		/// <summary>
		/// Builds the compilation task corresponding to the given command line
		/// </summary>
		/// <param name="line">The parsed command line as an AST</param>
		/// <returns>The corresponding compilation task, or null if there is any error</returns>
		private static CompilationTask BuildTask(ASTNode line)
		{
			CompilationTask task = new CompilationTask();
			// All single values before the arguments shall be inputs
			foreach (ASTNode value in line.Children[0].Children)
				AddInput(task, value);
			// Inspect each passed argument
			foreach (ASTNode arg in line.Children[1].Children)
			{
				switch (arg.Value)
				{
					case ArgOutputAssembly:
						if (task.Mode == Hime.SDK.Output.Mode.Source)
							task.Mode = Hime.SDK.Output.Mode.SourceAndAssembly;
						break;
					case ArgOutputNoSources:
						task.Mode = Hime.SDK.Output.Mode.Assembly;
						break;
					case ArgOutputDebug:
						task.Mode = Hime.SDK.Output.Mode.Debug;
						break;
					case ArgTargetJava:
						task.Target = Hime.SDK.Output.Runtime.Java;
						break;
					case ArgTargetRust:
						task.Target = Hime.SDK.Output.Runtime.Rust;
						break;
					case ArgGrammar:
						if (arg.Children.Count != 1)
							return null;
						task.GrammarName = CommandLine.GetValue(arg);
						break;
					case ArgPath:
						if (arg.Children.Count != 1)
							return null;
						task.OutputPath = CommandLine.GetValue(arg);
						break;
					case ArgMethodRNGLR:
						task.Method = ParsingMethod.RNGLALR1;
						break;
					case ArgNamespace:
						if (arg.Children.Count != 1)
							return null;
						task.Namespace = CommandLine.GetValue(arg);
						break;
					case ArgAccessPublic:
						task.CodeAccess = Hime.SDK.Output.Modifier.Public;
						break;
					default:
						Console.WriteLine("Unknown argument " + arg.Value);
						return null;
				}
			}
			return task;
		}

		/// <summary>
		/// Adds an input to a compilation task
		/// </summary>
		/// <param name="task">The compilation task</param>
		/// <param name="node">The input as a parsed data in the command line</param>
		private static void AddInput(CompilationTask task, ASTNode node)
		{
			string value = node.Value;
			if (value == null)
				return;
			if (value.StartsWith("\""))
				value = value.Substring(1, value.Length - 2);
			task.AddInputFile(value);
		}

		/// <summary>
		/// Prints the help screen for this program
		/// </summary>
		private static void PrintHelp()
		{
			Console.WriteLine("himecc " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " (LGPL 3)");
			Console.WriteLine("Hime parser generator, generates lexers and parsers in C# 2.0.");
			Console.WriteLine();
			Console.WriteLine("usage: himecc <files> [options]");
			Console.WriteLine("options:");
			Console.WriteLine(ArgHelpShort + ", " + ArgHelpLong + "\tShow this text");
			Console.WriteLine();
			Console.WriteLine(ArgOutputAssembly + "\tCompile the generated parser code in an assembly");
			Console.WriteLine();
			Console.WriteLine(ArgOutputNoSources + "\tOnly generate the assembly, do not keep the sources");
			Console.WriteLine();
			Console.WriteLine(ArgOutputDebug + "\tGenerate debug artifacts in addition to the sources");
			Console.WriteLine();
			Console.WriteLine(ArgTargetJava + "\tTarget the Java runtime instead of .Net");
			Console.WriteLine();
			Console.WriteLine(ArgTargetRust + "\tTarget the Rust language instead of .Net");
			Console.WriteLine();
			Console.WriteLine(ArgGrammar + " <grammar>\tSelect the top grammar to compile if more than one are given");
			Console.WriteLine();
			Console.WriteLine(ArgPath + " <prefix>\tSet the path for the outputs, default is the current directory");
			Console.WriteLine();
			Console.WriteLine(ArgMethodRNGLR + "\tUse the RNGLR parsing algorithm, default is LALR");
			Console.WriteLine();
			Console.WriteLine(ArgNamespace + " <namespace>\tNamespace for the generated code, default is the grammar's name");
			Console.WriteLine();
			Console.WriteLine(ArgAccessPublic + "\tPublic modifier for the generated code, default is internal");
		}
	}
}
