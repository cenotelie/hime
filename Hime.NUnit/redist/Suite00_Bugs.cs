/*
 * Author: Charles Hymans
 * Date: 18/07/2011
 * Time: 20:59
 * 
 */
using System;
using NUnit.Framework;
using Hime.Parsers;
using Hime.Kernel.Reporting;
using Hime.Redist.Parsers;
using System.Reflection;

namespace Hime.NUnit.Redist
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
