/*
 * Author: Laurent Wouters
 * Date: 21/10/2011
 * Time: 19:00
 * 
 */
using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using Hime.CentralDogma;
using Hime.Redist.AST;

namespace Hime.Tests.Redist
{
    [TestFixture]
    public class Suite00_Bugs : BaseTestSuite
    {
        [Test]
        public void Bug494()
        {
            string dir = GetTestDirectory();
            Assert.IsFalse(CompileResource("Lib.Redist.Bug494.gram", ParsingMethod.LALR1).HasErrors, "Grammar compilation failed!");
            Assembly assembly = Build();
            bool errors = false;
            ASTNode node = Parse(assembly, "aa", out errors);
            Assert.AreEqual(node.Children.Count, 2);
        }
    }
}
