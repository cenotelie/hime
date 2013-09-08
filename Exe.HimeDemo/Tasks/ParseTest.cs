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
using Hime.Redist.Lexer;

namespace Hime.Demo.Tasks
{
    class ParseTest : IExecutable
    {
        private string name;

        public ParseTest(string name)
        {
            this.name = name;
        }

        public void Execute()
        {
            // Build parser assembly
            CompilationTask task = new CompilationTask();
            task.Mode = CompilationMode.Assembly;
            task.AddInputFile("D:\\Dev\\VisualStudioProjects\\Hime\\Extras\\Grammars\\" + name + ".gram");
            task.Namespace = "Hime.Demo.Generated";
            task.CodeAccess = AccessModifier.Public;
            task.Method = ParsingMethod.LALR1;
            task.Execute();
            Assembly assembly = Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, name + ".dll"));

            TextReader reader = new StreamReader("D:\\Dev\\VisualStudioProjects\\Hime\\Extras\\Grammars\\" + name + ".sample");
            BaseLRParser parser = GetParser(assembly, reader, name);
            Redist.AST.ASTNode root = parser.Parse();
            reader.Close();
            
            foreach (ParserError error in parser.Errors)
                Console.WriteLine(error.ToString());
            if (root == null)
                return;
            WinTreeView win = new WinTreeView(root);
            win.ShowDialog();
        }

        private TextLexer GetLexer(Assembly assembly, TextReader reader, string name)
        {
            Type lexerType = assembly.GetType("Hime.Demo.Generated." + name + "Lexer");
            ConstructorInfo lexerConstructor = lexerType.GetConstructor(new Type[] { typeof(TextReader) });
            object lexer = lexerConstructor.Invoke(new object[] { reader });
            return lexer as TextLexer;
        }

        private BaseLRParser GetParser(Assembly assembly, TextReader reader, string name)
        {
            TextLexer lexer = GetLexer(assembly, reader, name);
            Type lexerType = assembly.GetType("Hime.Demo.Generated." + name + "Lexer");
            Type parserType = assembly.GetType("Hime.Demo.Generated." + name + "Parser");
            ConstructorInfo parserConstructor = parserType.GetConstructor(new Type[] { lexerType });
            object parser = parserConstructor.Invoke(new object[] { lexer });
            return parser as BaseLRParser;
        }
    }
}

