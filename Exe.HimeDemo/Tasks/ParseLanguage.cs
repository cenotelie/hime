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
using Hime.Redist;
using Hime.Redist.Parsers;
using Hime.Redist.Lexer;

namespace Hime.Demo.Tasks
{
    class ParseLanguage : IExecutable
    {
        private string path;
        private string input;

        public ParseLanguage(string path, string input)
        {
            this.path = path;
            this.input = input;
        }

        public void Execute()
        {
            // Build parser assembly
            CompilationTask task = new CompilationTask();
            task.Mode = CompilationMode.Assembly;
            task.AddInputFile(path + ".gram");
            task.Namespace = "Hime.Demo.Generated";
            task.CodeAccess = AccessModifier.Public;
            task.Method = ParsingMethod.LALR1;
            task.Execute();
            Assembly assembly = Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, Path.GetFileName(path) + ".dll"));

            TextReader reader = null;
            if (input != null)
                reader = new StringReader(input);
            else
                reader = new StreamReader(path + ".sample");
            BaseLRParser parser = GetParser(assembly, reader, Path.GetFileName(path));
            ParseResult result = parser.Parse();
            reader.Close();

            foreach (Error error in result.Errors)
                Console.WriteLine(error.ToString());
            if (!result.IsSucess)
                return;
            WinTreeView win = new WinTreeView(result.Root);
            win.ShowDialog();
        }

        private Lexer GetLexer(Assembly assembly, TextReader reader, string name)
        {
            Type lexerType = assembly.GetType("Hime.Demo.Generated." + name + "Lexer");
            ConstructorInfo lexerConstructor = lexerType.GetConstructor(new Type[] { typeof(TextReader) });
            object lexer = lexerConstructor.Invoke(new object[] { reader });
            return lexer as Lexer;
        }

        private BaseLRParser GetParser(Assembly assembly, TextReader reader, string name)
        {
            Lexer lexer = GetLexer(assembly, reader, name);
            Type lexerType = assembly.GetType("Hime.Demo.Generated." + name + "Lexer");
            Type parserType = assembly.GetType("Hime.Demo.Generated." + name + "Parser");
            ConstructorInfo parserConstructor = parserType.GetConstructor(new Type[] { lexerType });
            object parser = parserConstructor.Invoke(new object[] { lexer });
            return parser as BaseLRParser;
        }
    }
}

