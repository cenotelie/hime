/*
 * Author: Charles Hymans
 * */
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

