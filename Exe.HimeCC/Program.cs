/*
 * Author: Laurent Wouters
 * Date: 14/09/2011
 * Time: 17:26
 * 
 */
using System;
using System.Reflection;
using System.Text;
using Hime.CentralDogma;
using Hime.CentralDogma.Reporting;
using Hime.HimeCC.CL;
using Hime.Redist.AST;
using Hime.Redist.Parsers;
using Hime.Redist.Symbols;

namespace Hime.Compiler
{
    /// <summary>
    /// Entry class for the himecc program
    /// </summary>
    public sealed class Program
    {
        private const string ArgOutputShort = "-o";
        private const string ArgOutputLong = "--output";
        private const string ArgGrammarShort = "-g";
        private const string ArgGrammarLong = "--grammar";
        private const string ArgPrefixShort = "-p";
        private const string ArgPrefixLong = "--prefix";
        private const string ArgMethodShort = "-m";
        private const string ArgMethodLong = "--method";
        private const string ArgNamespaceShort = "-n";
        private const string ArgNamespaceLong = "--namespace";
        private const string ArgAccessShort = "-a";
        private const string ArgAccessLong = "--access";
        private const string ArgLogShort = "-l";
        private const string ArgLogLong = "--log";
        private const string ArgDocShort = "-d";
        private const string ArgDocLong = "--doc";

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
                return 0;
            }
            ASTNode line = ParseArguments(args);
            if (line == null)
            {
                PrintHelp();
                return 0;
            }
            CompilationTask task = BuildTask(line);
            Report report = task.Execute();
            return report.ErrorCount;
        }

        private ASTNode ParseArguments(string[] args)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string arg in args)
            {
                builder.Append(" ");
                builder.Append(arg);
            }
            CommandLineLexer lexer = new CommandLineLexer(builder.ToString());
            CommandLineParser parser = new CommandLineParser(lexer);
            ASTNode root = parser.Parse();
            foreach (ParserError error in parser.Errors)
                Console.WriteLine(error.Message);
            if (parser.Errors.Count > 0)
                return null;
            return root;
        }

        private CompilationTask BuildTask(ASTNode line)
        {
            CompilationTask task = new CompilationTask();
            foreach (ASTNode value in line.Children[0].Children)
                AddInput(task, value);
            foreach (ASTNode arg in line.Children[1].Children)
            {
                switch ((arg.Symbol as TextToken).ValueText)
                {
                    case ArgOutputShort:
                    case ArgOutputLong:
                        SelectArgOutput(task, arg);
                        break;
                    case ArgGrammarShort:
                    case ArgGrammarLong:
                        SelectArgGrammar(task, arg);
                        break;
                    case ArgPrefixShort:
                    case ArgPrefixLong:
                        SelectArgPrefix(task, arg);
                        break;
                    case ArgMethodShort:
                    case ArgMethodLong:
                        SelectArgMethod(task, arg);
                        break;
                    case ArgNamespaceShort:
                    case ArgNamespaceLong:
                        SelectArgNamespace(task, arg);
                        break;
                    case ArgAccessShort:
                    case ArgAccessLong:
                        SelectArgAccess(task, arg);
                        break;
                    case ArgLogShort:
                    case ArgLogLong:
                        SelectArgLog(task, arg);
                        break;
                    case ArgDocShort:
                    case ArgDocLong:
                        SelectArgDoc(task, arg);
                        break;
                }
            }
            return task;
        }

        private void AddInput(CompilationTask task, ASTNode node)
        {
            string value = (node.Symbol as TextToken).ValueText;
            if (value == null)
                return;
            if (value.StartsWith("\""))
                value = value.Substring(1, value.Length - 2);
            task.AddInputFile(value);
        }

        private void SelectArgOutput(CompilationTask task, ASTNode node)
        {
            string value = GetValue(node);
            if (value == null)
                return;
            switch (value)
            {
                case "Assembly": task.Mode = CompilationMode.Assembly; break;
                case "Sources": task.Mode = CompilationMode.Source; break;
                case "All": task.Mode = CompilationMode.SourceAndAssembly; break;
            }
        }

        private void SelectArgGrammar(CompilationTask task, ASTNode node)
        {
            string value = GetValue(node);
            if (value == null)
                return;
            task.GrammarName = value;
        }

        private void SelectArgPrefix(CompilationTask task, ASTNode node) {
            string value = GetValue(node);
            if (value == null)
                return;
            task.OutputPrefix = value;
        }

        private void SelectArgMethod(CompilationTask task, ASTNode node)
        {
            string value = GetValue(node);
            if (value == null)
                return;
            switch (value)
            {
                case "LALR": task.Method = ParsingMethod.LALR1; break;
                case "RNGLR": task.Method = ParsingMethod.RNGLALR1; break;
            }
        }

        private void SelectArgNamespace(CompilationTask task, ASTNode node)
        {
            string value = GetValue(node);
            if (value == null)
                return;
            task.Namespace = value;
        }

        private void SelectArgAccess(CompilationTask task, ASTNode node)
        {
            string value = GetValue(node);
            if (value == null)
                return;
            switch (value)
            {
                case "Public": task.CodeAccess = AccessModifier.Public; break;
                case "Internal": task.CodeAccess = AccessModifier.Internal; break;
            }
        }

        private void SelectArgLog(CompilationTask task, ASTNode node) { task.OutputLog = true; }

        private void SelectArgDoc(CompilationTask task, ASTNode node) { task.OutputDocumentation = true; }

        private string GetValue(ASTNode argument)
        {
            if (argument.Children.Count == 0)
                return null;
            string value = (argument.Children[0].Symbol as TextToken).ValueText;
            if (value.StartsWith("\""))
                return value.Substring(1, value.Length - 2);
            return value;
        }

        private void PrintHelp()
        {
            Console.WriteLine(Assembly.GetExecutingAssembly().FullName);
            Console.WriteLine("This is free software. You may redistribute copies of it under the terms of");
            Console.WriteLine("the LGPL License <http://www.gnu.org/licenses/lgpl.html>.");
            Console.WriteLine("");
            Console.WriteLine("Default usage:");
            Console.WriteLine(" > himecc MyGram.gram");
            Console.WriteLine("Use the RNGLR algorithm:");
            Console.WriteLine(" > himecc MyGram.gram -p RNGLALR1");
            Console.WriteLine("");
            Console.WriteLine("Parameters:");
            Console.WriteLine(ArgOutputShort + "\tSelect the output mode: Assembly, Sources, All");
            Console.WriteLine(ArgOutputLong + "\tdefault: Sources");
            Console.WriteLine("");
            Console.WriteLine(ArgGrammarShort + "\tSelect the top grammar to compile");
            Console.WriteLine(ArgGrammarLong + "\tdefault: <empty>");
            Console.WriteLine("");
            Console.WriteLine(ArgPrefixShort + "\tPath for the outputs");
            Console.WriteLine(ArgPrefixLong + "\tdefault: current directory");
            Console.WriteLine("");
            Console.WriteLine(ArgMethodShort + "\tSelect the parsing method: LALR, RNGLR");
            Console.WriteLine(ArgMethodLong + "\tdefault: LALR");
            Console.WriteLine("");
            Console.WriteLine(ArgNamespaceShort + "\tNamespace for the generated code");
            Console.WriteLine(ArgNamespaceLong + "\tdefault: compiled grammar's name");
            Console.WriteLine("");
            Console.WriteLine(ArgAccessShort + "\tAccess modifier for the generated code: Public, Internal");
            Console.WriteLine(ArgAccessLong + "\tdefault: Internal");
            Console.WriteLine("");
            Console.WriteLine(ArgLogShort + "\tOutput the log as a MHTML file");
            Console.WriteLine(ArgLogLong);
            Console.WriteLine("");
            Console.WriteLine(ArgDocShort + "\tOutput the documentation for the compiled grammar");
            Console.WriteLine(ArgDocLong);
        }
    }
}
