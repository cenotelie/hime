using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using NUnit.Framework;
using Hime.Parsers;
using Hime.Utils.Reporting;
using Hime.Redist.Parsers;

namespace Hime.Tests.CentralDogma
{
    [TestFixture]
    public class Suite04_Bugs : BaseTestSuite
    {
        private Report DoCompile(string grammar, ParsingMethod method, string dir)
        {
            string lexer = "lexer.cs";
            string parser = "parser.cs";
            return CompileRaw(grammar, method, lexer, parser);
        }

        [Test]
        public void Test001_Bug_InlineEndLine()
        {
            string dir = GetTestDirectory();
            string grammar = "cf grammar Test { options{ Axiom=\"test\"; } terminals{} rules{ test->'\\n'; }  }";
            Assert.IsFalse(DoCompile(grammar, ParsingMethod.LALR1, dir).HasErrors);
            Assert.IsNotNull(Build("lexer.cs", "parser.cs"));
        }
    }
}
