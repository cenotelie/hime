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
using Hime.Demo.Tasks;

namespace Hime.Demo
{
    public class Program
    {
        private const string dirExtras = "Extras";
        private const string dirGrammars = "Grammars";

        static void Main()
        {
            string name = "EBNF";
            string input = null;

            DirectoryInfo extras = FindExtras();
            DirectoryInfo grammars = extras.GetDirectories(dirGrammars)[0];
            
            //IExecutable executable = new ParseLanguage(Path.Combine(grammars.FullName, name), input);
            IExecutable executable = new Bootstrap();
            executable.Execute();
        }

        private static DirectoryInfo FindExtras()
        {
            DirectoryInfo current = new DirectoryInfo(Environment.CurrentDirectory);
            DirectoryInfo[] subs = current.GetDirectories(dirExtras);
            while (subs == null || subs.Length == 0)
            {
                current = current.Parent;
                subs = current.GetDirectories(dirExtras);
            }
            return subs[0];
        }
    }
}
