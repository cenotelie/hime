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
using System.IO;
using System.Reflection;
using Hime.CentralDogma;
using Hime.Redist.Parsers;

namespace Hime.Demo.Tasks
{
    class ParseGrammar : IExecutable
    {
        public void Execute()
        {
            // Build parser assembly
            System.IO.Stream stream = typeof(CompilationTask).Assembly.GetManifestResourceStream("Hime.CentralDogma.Sources.Input.FileCentralDogma.gram");
            CompilationTask task = new CompilationTask();
            task.Mode = CompilationMode.Assembly;
            task.AddInputRaw(stream);
            task.Namespace = "Hime.Benchmark.Generated";
            task.GrammarName = "FileCentralDogma";
            task.CodeAccess = AccessModifier.Public;
            task.Method = ParsingMethod.RNGLALR1;
            task.Execute();
            Assembly assembly = Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, "FileCentralDogma.dll"));

            System.IO.StreamReader reader = new System.IO.StreamReader("Languages\\FileCentralDogma.gram");
            Hime.Redist.Parsers.BaseLRParser parser = GetParser(assembly, reader);
            Redist.AST.ASTNode root = parser.Parse();
            reader.Close();
            
            foreach (ParserError error in parser.Errors)
                Console.WriteLine(error.ToString());
            if (root == null)
                return;
            WinTreeView win = new WinTreeView(root);
            win.ShowDialog();
        }

        private Hime.Redist.Lexer.TextLexer GetLexer(Assembly assembly, System.IO.StreamReader reader)
        {
            Type lexerType = assembly.GetType("Hime.Benchmark.Generated.FileCentralDogmaLexer");
            ConstructorInfo lexerConstructor = lexerType.GetConstructor(new Type[] { typeof(System.IO.TextReader) });
            object lexer = lexerConstructor.Invoke(new object[] { reader });
            return lexer as Hime.Redist.Lexer.TextLexer;
        }

        private Hime.Redist.Parsers.BaseLRParser GetParser(Assembly assembly, System.IO.StreamReader reader)
        {
            Hime.Redist.Lexer.TextLexer lexer = GetLexer(assembly, reader);
            Type lexerType = assembly.GetType("Hime.Benchmark.Generated.FileCentralDogmaLexer");
            Type parserType = assembly.GetType("Hime.Benchmark.Generated.FileCentralDogmaParser");
            ConstructorInfo parserConstructor = parserType.GetConstructor(new Type[] { lexerType });
            object parser = parserConstructor.Invoke(new object[] { lexer });
            return parser as Hime.Redist.Parsers.BaseLRParser;
        }
    }
}

