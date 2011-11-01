/*
 * Author: Charles Hymans
 * */
using System;
using System.Collections.Generic;
using System.Text;
using Hime.Redist.Parsers;

namespace Hime.Demo.Tasks
{
    internal class MathExpInterpreter : Analyser.MathExp_Parser.Actions
    {
		private Stack<float> stack;

        internal float Value { get { return stack.Peek(); } }

        internal MathExpInterpreter() { stack = new System.Collections.Generic.Stack<float>(); }

        internal void OnNumber(SyntaxTreeNode SubRoot)
        {
        	SyntaxTreeNode node = SubRoot.Children[0];
            SymbolTokenText token = (SymbolTokenText)node.Symbol;
            float value = Convert.ToSingle(token.ValueText);
            stack.Push(value);
        }

        internal void OnMult(SyntaxTreeNode SubRoot)
        {
        	float right = stack.Pop();
            float left = stack.Pop();
            stack.Push(left * right);
        }

        internal void OnDiv(SyntaxTreeNode SubRoot)
        {
            float right = stack.Pop();
            float left = stack.Pop();
            stack.Push(left / right);
        }

        internal void OnPlus(SyntaxTreeNode SubRoot)
        {
            float right = stack.Pop();
            float left = stack.Pop();
            stack.Push(left + right);
        }

        internal void OnMinus(SyntaxTreeNode SubRoot)
        {
            float right = stack.Pop();
            float left = stack.Pop();
            stack.Push(left - right);
        }
	}
}
