/*
 * Author: Charles Hymans
 * */
using System;
using Hime.Redist.Parsers;
using System.Collections.Generic;

namespace Hime.Demo.Tasks
{
	class Parse : IExecutable
	{
        class Interpreter : Generated.MathExp.MathExpParser.Actions
        {
            private Stack<float> stack;

            public float Value { get { return stack.Peek(); } }

            public Interpreter() { stack = new Stack<float>(); }

            public void OnNumber(object head, object[] body, int length)
            {
                SymbolTokenText token = body[0] as SymbolTokenText;
                stack.Push(Single.Parse(token.ValueText));
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
        }

		public Parse() { }

        public void Execute()
        {
            Interpreter interpreter = new Interpreter();
            Generated.MathExp.MathExpLexer lexer = new Generated.MathExp.MathExpLexer("3+5");
            Generated.MathExp.MathExpParser parser = new Generated.MathExp.MathExpParser(lexer, interpreter);

            bool result = parser.Recognize();

            foreach (ParserError error in parser.Errors)
			{
                Console.WriteLine(error.ToString());
			}
			
            /*if (root == null) return;
            using (LangTest.WinTreeView window = new LangTest.WinTreeView(root))
			{
                window.ShowDialog();
            }*/
        }
	}
}

