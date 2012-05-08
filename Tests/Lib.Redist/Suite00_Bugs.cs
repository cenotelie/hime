/*
 * Author: Laurent Wouters
 * Date: 21/10/2011
 * Time: 19:00
 * 
 */
using System;
using System.IO;
using NUnit.Framework;
using Hime.Parsers;
using Hime.Utils.Reporting;
using Hime.Redist.Parsers;
using System.Reflection;

namespace Hime.Tests.Redist
{
    [TestFixture]
    public class Suite00_Bugs : BaseTestSuite
    {
        [Test]
        public void Bug494()
        {
            string dir = GetTestDirectory();
            string lexer = Path.Combine(dir, "lexer.cs");
            string parser = Path.Combine(dir, "parser.cs");
            string grammar = GetResourceContent("Lib.Redist.Bug494.gram");
            Assert.IsFalse(CompileRaw(grammar, ParsingMethod.LALR1, lexer, parser).HasErrors, "Grammar compilation failed!");
            Assembly assembly = Build(lexer, parser);
            bool errors = false;
            SyntaxTreeNode node = Parse(assembly, "aa", out errors);
            Assert.AreEqual(node.Children.Count, 2);
        }
    }
}
