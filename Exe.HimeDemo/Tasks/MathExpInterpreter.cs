using System;
using System.Collections.Generic;
using System.Text;

namespace Hime.Demo.Tasks
{
    class MathExpInterpreter : Analyser.MathExp_Parser.Actions
        {
            private System.Collections.Generic.Stack<float> stack;

            public float Value { get { return stack.Peek(); } }

            public MathExpInterpreter() { stack = new System.Collections.Generic.Stack<float>(); }

            public void OnNumber(Hime.Redist.Parsers.SyntaxTreeNode SubRoot)
            {
                Hime.Redist.Parsers.SyntaxTreeNode node = SubRoot.Children[0];
                Hime.Redist.Parsers.SymbolTokenText token = (Hime.Redist.Parsers.SymbolTokenText)node.Symbol;
                float value = System.Convert.ToSingle(token.ValueText);
                stack.Push(value);
            }

            public void OnMult(Hime.Redist.Parsers.SyntaxTreeNode SubRoot)
            {
                float right = stack.Pop();
                float left = stack.Pop();
                stack.Push(left * right);
            }

            public void OnDiv(Hime.Redist.Parsers.SyntaxTreeNode SubRoot)
            {
                float right = stack.Pop();
                float left = stack.Pop();
                stack.Push(left / right);
            }

            public void OnPlus(Hime.Redist.Parsers.SyntaxTreeNode SubRoot)
            {
                float right = stack.Pop();
                float left = stack.Pop();
                stack.Push(left + right);
            }

            public void OnMinus(Hime.Redist.Parsers.SyntaxTreeNode SubRoot)
            {
                float right = stack.Pop();
                float left = stack.Pop();
                stack.Push(left - right);
            }
        }
}
