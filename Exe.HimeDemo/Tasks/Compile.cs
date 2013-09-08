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

using Hime.CentralDogma;

namespace Hime.Demo.Tasks
{
    class Compile : IExecutable
    {
        public void Execute()
        {
            CompilationTask task = new CompilationTask();
            task.AddInputFile("Languages\\ECMAScript.gram");
            task.Method = ParsingMethod.RNGLALR1;
            task.Namespace = "Hime.Demo.Generated";
            task.OutputLog = true;
            task.OutputDocumentation = true;
            task.Execute();
        }
    }
}
