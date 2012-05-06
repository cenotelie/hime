/*
 * Author: Laurent Wouters
 * Date: 21/10/2011
 * Time: 19:00
 * 
 */
using System;
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
            string grammar = GetAllTextFor("RedistBug494.gram");
            Assert.IsFalse(CompileRaw(grammar, ParsingMethod.LALR1).HasErrors, "Grammar compilation failed!");
            Assembly assembly = Build();
            bool errors = false;
            SyntaxTreeNode node = Parse(assembly, "aa", out errors);
            Assert.AreEqual(node.Children.Count, 2);
        }
    }
}
