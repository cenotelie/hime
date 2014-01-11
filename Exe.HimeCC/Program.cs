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
using System.Reflection;
using System.Text;
using Hime.CentralDogma;
using Hime.CentralDogma.Reporting;
using Hime.HimeCC.CL;
using Hime.Redist;
using Hime.Redist.Symbols;

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
        private const string ArgGrammar = "-g";
        private const string ArgPrefix = "-p";
        private const string ArgMethodRNGLR = "-m:rnglr";
        private const string ArgNamespace = "-n";
        private const string ArgAccessPublic = "-a:public";
        private const string ArgLog = "-l";
        private const string ArgDoc = "-d";

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
        /// <returns>The number of errors</returns>
        public static int Main(string[] args)
        {
            Program program = new Program();
            return program.Run(args);
        }

        private int Run(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                PrintHelp();
                return ResultOK;
            }

            // Parse the arguments
            ParseResult line = ParseArguments(args);
            if (!line.IsSucess)
            {
                Console.WriteLine(ErrorParsingArgs);
                Console.WriteLine(ErrorPointHelp);
                return ResultErrorParsingArgs;
            }
            
            // Check for special switches
            string special = GetSpecialCommand(line.Root);
            if (special == ArgHelpShort || special == ArgHelpLong)
            {
                PrintHelp();
                return ResultOK;
            }
            else if (special == ArgRegenerateShort || special == ArgRegenerateLong)
            {
                GenerateCLParser();
                GenerateCDParser();
                return ResultOK;
            }

            // Build the compilation task
            CompilationTask task = BuildTask(line.Root);
            if (task == null)
            {
                Console.WriteLine(ErrorBadArgs);
                Console.WriteLine(ErrorPointHelp);
                return ResultErrorBadArgs;
            }

            // Execute the task
            Report report = task.Execute();
            if (report.HasErrors)
                return ResultErrorCompiling;
            return ResultOK;
        }

        private int GenerateCLParser()
        {
            System.IO.Stream stream = typeof(Program).Assembly.GetManifestResourceStream("himecc.CommandLine.gram");
            CompilationTask task = new CompilationTask();
            task.Mode = CompilationMode.Source;
            task.AddInputRaw(stream);
            task.Namespace = "Hime.HimeCC.CL";
            task.CodeAccess = AccessModifier.Internal;
            task.Method = ParsingMethod.LALR1;
            Report report = task.Execute();
            return report.ErrorCount;
        }

        private int GenerateCDParser()
        {
            System.IO.Stream stream = typeof(CompilationTask).Assembly.GetManifestResourceStream("Hime.CentralDogma.Sources.Input.FileCentralDogma.gram");
            CompilationTask task = new CompilationTask();
            task.Mode = CompilationMode.Source;
            task.AddInputRaw(stream);
            task.GrammarName = "FileCentralDogma";
            task.Namespace = "Hime.CentralDogma.Input";
            task.CodeAccess = AccessModifier.Internal;
            task.Method = ParsingMethod.LALR1;
            Report report = task.Execute();
            return report.ErrorCount;
        }

        private ParseResult ParseArguments(string[] args)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string arg in args)
            {
                builder.Append(" ");
                builder.Append(arg);
            }
            CommandLineLexer lexer = new CommandLineLexer(builder.ToString());
            CommandLineParser parser = new CommandLineParser(lexer);
            ParseResult result = parser.Parse();
            foreach (Error error in result.Errors)
                Console.WriteLine(error.Message);
            return result;
        }

        private string GetSpecialCommand(ASTNode line)
        {
            if (line.Children[0].Children.Count == 0 && line.Children[1].Children.Count == 1)
                return (line.Children[1].Children[0].Symbol as TextToken).Value;
            return null;
        }

        private CompilationTask BuildTask(ASTNode line)
        {
            CompilationTask task = new CompilationTask();
            foreach (ASTNode value in line.Children[0].Children)
                AddInput(task, value);
            foreach (ASTNode arg in line.Children[1].Children)
            {
                switch ((arg.Symbol as TextToken).Value)
                {
                    case ArgOutputAssembly:
                        if (task.Mode == CompilationMode.Source)
                            task.Mode = CompilationMode.SourceAndAssembly;
                        break;
                    case ArgOutputNoSources:
                        task.Mode = CompilationMode.Assembly;
                        break;
                    case ArgGrammar:
                        if (arg.Children.Count != 1)
                            return null;
                        task.GrammarName = GetValue(arg);
                        break;
                    case ArgPrefix:
                        if (arg.Children.Count != 1)
                            return null;
                        task.OutputPrefix = GetValue(arg);
                        break;
                    case ArgMethodRNGLR:
                        task.Method = ParsingMethod.RNGLALR1;
                        break;
                    case ArgNamespace:
                        if (arg.Children.Count != 1)
                            return null;
                        task.Namespace = GetValue(arg);
                        break;
                    case ArgAccessPublic:
                        task.CodeAccess = AccessModifier.Public;
                        break;
                    case ArgLog:
                        task.OutputLog = true;
                        break;
                    case ArgDoc:
                        task.OutputDocumentation = true;
                        break;
                    default:
                        Console.WriteLine("Unknown argument " + (arg.Symbol as TextToken).Value);
                        return null;
                }
            }
            return task;
        }

        private void AddInput(CompilationTask task, ASTNode node)
        {
            string value = (node.Symbol as TextToken).Value;
            if (value == null)
                return;
            if (value.StartsWith("\""))
                value = value.Substring(1, value.Length - 2);
            task.AddInputFile(value);
        }

        private string GetValue(ASTNode argument)
        {
            if (argument.Children.Count == 0)
                return null;
            string value = (argument.Children[0].Symbol as TextToken).Value;
            if (value.StartsWith("\""))
                return value.Substring(1, value.Length - 2);
            return value;
        }

        private void PrintHelp()
        {
            Console.WriteLine("himecc " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " (LGPL 3)");
            Console.WriteLine("Hime parser generator, generates lexers and parsers in C# 2.0.");
            Console.WriteLine();
            Console.WriteLine("usage: himecc <files> [options]");
            Console.WriteLine("options:");
            Console.WriteLine(ArgHelpShort + ", "+ArgHelpLong + "\tShow this text");
            Console.WriteLine();
            Console.WriteLine(ArgOutputAssembly + "\tCompile the generated parser code in an assembly");
            Console.WriteLine();
            Console.WriteLine(ArgOutputNoSources + "\tOnly generate the assembly, do not keep the sources");
            Console.WriteLine();
            Console.WriteLine(ArgGrammar + " <grammar>\tSelect the top grammar to compile if more than one are given");
            Console.WriteLine();
            Console.WriteLine(ArgPrefix + " <prefix>\tSet the path for the outputs, default is the current directory");
            Console.WriteLine();
            Console.WriteLine(ArgMethodRNGLR + "\tUse the RNGLR parsing algorithm, default is LALR");
            Console.WriteLine();
            Console.WriteLine(ArgNamespace + " <namespace>\tNamespace for the generated code, default is the grammar's name");
            Console.WriteLine();
            Console.WriteLine(ArgAccessPublic + "\tPublic modifier for the generated code, default is internal");
            Console.WriteLine();
            Console.WriteLine(ArgLog + "\t\tOutput the log as a MHTML file");
            Console.WriteLine();
            Console.WriteLine(ArgDoc + "\t\tOutput the documentation for the compiled grammar");
        }
    }
}
