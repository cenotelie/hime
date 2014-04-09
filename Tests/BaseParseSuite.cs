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
using Hime.Redist.AST;
using Hime.Redist.Symbols;
using NUnit.Framework;

namespace Hime.Tests
{
	/// <summary>
	/// Represents a base test suite for integration parsing test
	/// </summary>
	public abstract class BaseParseSuite : BaseTestSuite
	{
		/// <summary>
		/// Constructor for lexers of parse trees
		/// </summary>
		private ConstructorInfo parseTreeLexer;
		/// <summary>
		/// Constructor for parsers of parse trees
		/// </summary>
        private ConstructorInfo parseTreeParser;
        
        /// <summary>
        /// Initializes this test suite
        /// </summary>
        protected BaseParseSuite()
        {
        	Assembly assembly = CompileResource("ParseTree", ParsingMethod.LALR1);
            Type tl = assembly.GetType("Hime.Tests.Generated.ParseTreeLexer");
            Type tp = assembly.GetType("Hime.Tests.Generated.ParseTreeParser");
            parseTreeLexer = tl.GetConstructor(new Type[] { typeof(string) });
            parseTreeParser = tp.GetConstructor(new Type[] { tl });
        }
        
        /// <summary>
        /// Parses the string representation of the given parse tree
        /// </summary>
        /// <param name="data">A string representation of a parse tree</param>
        /// <returns>The parse tree's AST</returns>
        protected ASTNode ParseTree(string data)
        {
        	Hime.Redist.Lexer.ILexer lexer = parseTreeLexer.Invoke(new object[] { data }) as Hime.Redist.Lexer.ILexer;
            Hime.Redist.Parsers.IParser parser = parseTreeParser.Invoke(new object[] { lexer }) as Hime.Redist.Parsers.IParser;
            return parser.Parse();
        }
        
        /// <summary>
        /// Compare two parse trees
        /// </summary>
        /// <param name="expected">The expected sub tree</param>
        /// <param name="node">The sub tree to compare</param>
        /// <returns>True if the two trees match</returns>
        protected bool Compare(ASTNode expected, ASTNode node)
        {
            if (node.Symbol.Name != (expected.Symbol as Token).Value)
                return false;
            if (expected.Children[0].Children.Count != 0)
            {
                string vRef = (expected.Children[0].Children[0].Symbol as Token).Value;
                vRef = vRef.Substring(1, vRef.Length - 2);
                string vReal = (node.Symbol as Token).Value;
                if (vReal != vRef)
                    return false;
            }
            if (node.Children.Count != expected.Children[1].Children.Count)
                return false;
            for (int i = 0; i != node.Children.Count; i++)
                if (!Compare(expected.Children[1].Children[i], node.Children[i]))
                    return false;
            return true;
        }
        
        /// <summary>
        /// Tests whether the given grammar parses the input as the expected AST
        /// </summary>
        /// <param name="grammar">The grammar to use</param>
        /// <param name="method">The parsing method</param>
        /// <param name="input">The input</param>
        /// <param name="expected">The expected AST</param>
        protected void TestMatch(string grammar, ParsingMethod method, string input, string expected)
        {
        	CompilationTask task = new CompilationTask();
            task.AddInputRaw("anon", grammar);
            task.CodeAccess = AccessModifier.Public;
            task.Method = method;
            task.Mode = CompilationMode.Assembly;
            task.Namespace = "Hime.Tests.Generated";
            Hime.CentralDogma.Reporting.Report report = task.Execute();
            Assert.AreEqual(0, report.ErrorCount, "Failed to compile the grammar");
            Assert.IsTrue(CheckFile("anon.dll"), "Failed to produce the assembly");
            return Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, "anon.dll"));
        	
        	
        	
        	Assembly assembly = CompileResource("ParseTree", ParsingMethod.LALR1);
            Type tl = assembly.GetType("Hime.Tests.Generated.ParseTreeLexer");
            Type tp = assembly.GetType("Hime.Tests.Generated.ParseTreeParser");
            parseTreeLexer = tl.GetConstructor(new Type[] { typeof(string) });
            parseTreeParser = tp.GetConstructor(new Type[] { tl });
        }
	}
}