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
using System.Collections.Generic;
using Hime.Redist.Symbols;
using Hime.Redist.Parsers;

namespace Hime.Demo.Tasks
{
    class ParseMathExp : IExecutable
    {
        private Stack<float> stack;

        public void OnNumber(object head, object[] body, int length)
        {
            TextToken token = body[0] as TextToken;
            stack.Push(Single.Parse(token.Value));
        }
        public void OnMult(object head, object[] body, int length)
        {
            float right = stack.Pop();
            float left = stack.Pop();
            stack.Push(left * right);
        }
        public void OnDiv(object head, object[] body, int length)
        {
            float right = stack.Pop();
            float left = stack.Pop();
            stack.Push(left / right);
        }
        public void OnPlus(object head, object[] body, int length)
        {
            float right = stack.Pop();
            float left = stack.Pop();
            stack.Push(left + right);
        }
        public void OnMinus(object head, object[] body, int length)
        {
            float right = stack.Pop();
            float left = stack.Pop();
            stack.Push(left - right);
        }

        public void Execute()
        {
            /*stack = new Stack<float>();
            Generated.MathExpParser.Actions actions = new Generated.MathExpParser.Actions();
            actions.OnNumber = new SemanticAction(OnNumber);
            actions.OnMult = new SemanticAction(OnMult);
            actions.OnDiv = new SemanticAction(OnDiv);
            actions.OnPlus = new SemanticAction(OnPlus);
            actions.OnMinus = new SemanticAction(OnMinus);
            Generated.MathExpLexer lexer = new Generated.MathExpLexer("3+5");
            Generated.MathExpParser parser = new Generated.MathExpParser(lexer, actions);

            Redist.AST.CSTNode root = parser.Parse();
            foreach (ParserError error in parser.Errors)
                Console.WriteLine(error.ToString());
            if (root == null)
                return;
            else
                Console.WriteLine("Result = " + stack.Peek());
            WinTreeView win = new WinTreeView(root);
            win.ShowDialog();*/
        }
    }
}

