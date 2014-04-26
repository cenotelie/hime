/**********************************************************************
* Copyright (c) 2014 Laurent Wouters and others
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
using System.IO;
using System.Reflection;
using Hime.CentralDogma;
using Hime.CentralDogma.SDK;
using Hime.Redist;

namespace Hime.Demo.Tasks
{
    class ParseGrammar : IExecutable
    {
        public void Execute()
        {
            // Build parser assembly
            Stream stream = typeof(CompilationTask).Assembly.GetManifestResourceStream("Hime.CentralDogma.Sources.Input.FileCentralDogma.gram");
            CompilationTask task = new CompilationTask();
            task.Mode = CompilationMode.Assembly;
            task.AddInputRaw(stream);
            task.Namespace = "Hime.Benchmark.Generated";
            task.GrammarName = "FileCentralDogma";
            task.CodeAccess = AccessModifier.Public;
            task.Method = ParsingMethod.LALR1;
            task.Execute();
            stream.Close();

			// Load the generated assembly
			AssemblyReflection assembly = new AssemblyReflection(Path.Combine(Environment.CurrentDirectory, "FileCentralDogma.dll"));

			// Re-parse the input grammar with the generated parser
            StreamReader input = new StreamReader(typeof(CompilationTask).Assembly.GetManifestResourceStream("Hime.CentralDogma.Sources.Input.FileCentralDogma.gram"));
			BaseLRParser parser = assembly.GetDefaultParser(input);
            ParseResult result = parser.Parse();
            input.Close();
            
			// Display the errors if any
            foreach (ParserError error in parser.Errors)
                Console.WriteLine(error.ToString());
            if (!result.IsSuccess)
                return;
            WinTreeView win = new WinTreeView(result.Root);
            win.ShowDialog();
        }

        private Hime.Redist.Lexer.Lexer GetLexer(Assembly assembly, System.IO.StreamReader reader)
        {
            Type lexerType = assembly.GetType("Hime.CentralDogma.Input.FileCentralDogmaLexer");
            ConstructorInfo lexerConstructor = lexerType.GetConstructor(new Type[] { typeof(System.IO.TextReader) });
            object lexer = lexerConstructor.Invoke(new object[] { reader });
            return lexer as Hime.Redist.Lexer.Lexer;
        }

        private Hime.Redist.Parsers.BaseLRParser GetParser(Assembly assembly, System.IO.StreamReader reader)
        {
            Hime.Redist.Lexer.Lexer lexer = GetLexer(assembly, reader);
            Type lexerType = assembly.GetType("Hime.CentralDogma.Input.FileCentralDogmaLexer");
            Type parserType = assembly.GetType("Hime.CentralDogma.Input.FileCentralDogmaParser");
            ConstructorInfo parserConstructor = parserType.GetConstructor(new Type[] { lexerType });
            object parser = parserConstructor.Invoke(new object[] { lexer });
            return parser as Hime.Redist.Parsers.BaseLRParser;
        }
    }
}

