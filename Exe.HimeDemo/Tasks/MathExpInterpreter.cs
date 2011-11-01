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

        internal MathExpInterpreter() 
		{ 
			this.stack = new System.Collections.Generic.Stack<float>(); 
		}

        public void OnNumber(SyntaxTreeNode SubRoot)
        {
        	SyntaxTreeNode node = SubRoot.Children[0];
            SymbolTokenText token = (SymbolTokenText)node.Symbol;
            float value = Convert.ToSingle(token.ValueText);
            stack.Push(value);
        }

        public void OnMult(SyntaxTreeNode SubRoot)
        {
        	float right = stack.Pop();
            float left = stack.Pop();
            stack.Push(left * right);
        }

        public void OnDiv(SyntaxTreeNode SubRoot)
        {
            float right = stack.Pop();
            float left = stack.Pop();
            stack.Push(left / right);
        }

        public void OnPlus(SyntaxTreeNode SubRoot)
        {
            float right = stack.Pop();
            float left = stack.Pop();
            stack.Push(left + right);
        }

        public void OnMinus(SyntaxTreeNode SubRoot)
        {
            float right = stack.Pop();
            float left = stack.Pop();
            stack.Push(left - right);
        }
	}
}
