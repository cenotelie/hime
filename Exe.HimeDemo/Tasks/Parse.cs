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
        class Interpreter : Generated.MathExp.MathExpParser.Actions
        {
            private Stack<float> stack;

            public float Value { get { return stack.Peek(); } }

            public Interpreter() { stack = new Stack<float>(); }

            public void OnNumber(object head, object[] body, int length)
            {
                TextToken token = body[0] as TextToken;
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

		public ParseMathExp() { }

        public void Execute()
        {
            Interpreter interpreter = new Interpreter();
            Generated.MathExp.MathExpLexer lexer = new Generated.MathExp.MathExpLexer("3+5");
            Generated.MathExp.MathExpParser parser = new Generated.MathExp.MathExpParser(lexer, interpreter);

            bool result = parser.Recognize();
            foreach (ParserError error in parser.Errors)
                Console.WriteLine(error.ToString());
            if (result)
                Console.WriteLine("Result = " + interpreter.Value);
        }
	}
}

