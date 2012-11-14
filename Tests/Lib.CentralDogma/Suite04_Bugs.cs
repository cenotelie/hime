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

        [Test]
        public void Test001_Bug_CaseSensitiveInTerminals()
        {
            string dir = GetTestDirectory();
            string grammar = "cf grammar Test { options{ Axiom=\"test\"; } terminals{ A->'a'; } rules{ test->A; }  }";
            Assert.IsFalse(DoCompile(grammar, ParsingMethod.LALR1, dir).HasErrors);
            Assembly assembly = Build("lexer.cs", "parser.cs");
            Assert.IsNotNull(assembly);
            
            bool errors = false;
            SyntaxTreeNode ast = Parse(assembly, "a", out errors);
            Assert.IsNotNull(ast);
            Assert.IsFalse(errors);
            ast = Parse(assembly, "A", out errors);
            Assert.IsTrue(errors);
        }

        [Test]
        public void Test001_Bug_CaseInsensitiveInTerminals()
        {
            string dir = GetTestDirectory();
            string grammar = "cf grammar Test { options{ Axiom=\"test\"; } terminals{ A->~'a'; } rules{ test->A; }  }";
            Assert.IsFalse(DoCompile(grammar, ParsingMethod.LALR1, dir).HasErrors);
            Assembly assembly = Build("lexer.cs", "parser.cs");
            Assert.IsNotNull(assembly);

            bool errors = false;
            SyntaxTreeNode ast = Parse(assembly, "a", out errors);
            Assert.IsNotNull(ast);
            Assert.IsFalse(errors);
            ast = Parse(assembly, "A", out errors);
            Assert.IsNotNull(ast);
            Assert.IsFalse(errors);
        }

        [Test]
        public void Test001_Bug_CaseSensitiveInRules()
        {
            string dir = GetTestDirectory();
            string grammar = "cf grammar Test { options{ Axiom=\"test\"; } terminals{} rules{ test->'a'; }  }";
            Assert.IsFalse(DoCompile(grammar, ParsingMethod.LALR1, dir).HasErrors);
            Assembly assembly = Build("lexer.cs", "parser.cs");
            Assert.IsNotNull(assembly);

            bool errors = false;
            SyntaxTreeNode ast = Parse(assembly, "a", out errors);
            Assert.IsNotNull(ast);
            Assert.IsFalse(errors);
            ast = Parse(assembly, "A", out errors);
            Assert.IsTrue(errors);
        }

        [Test]
        public void Test001_Bug_CaseInsensitiveInRules()
        {
            string dir = GetTestDirectory();
            string grammar = "cf grammar Test { options{ Axiom=\"test\"; } terminals{} rules{ test->~'a'; }  }";
            Assert.IsFalse(DoCompile(grammar, ParsingMethod.LALR1, dir).HasErrors);
            Assembly assembly = Build("lexer.cs", "parser.cs");
            Assert.IsNotNull(assembly);

            bool errors = false;
            SyntaxTreeNode ast = Parse(assembly, "a", out errors);
            Assert.IsNotNull(ast);
            Assert.IsFalse(errors);
            ast = Parse(assembly, "A", out errors);
            Assert.IsNotNull(ast);
            Assert.IsFalse(errors);
        }

        [Test]
        public void Test001_Bug_GlobalCaseInsensitive_Terminals()
        {
            string dir = GetTestDirectory();
            string grammar = "cf grammar Test { options{ Axiom=\"test\"; CaseSensitive=\"False\"; } terminals{} rules{ test->'a'; }  }";
            Assert.IsFalse(DoCompile(grammar, ParsingMethod.LALR1, dir).HasErrors);
            Assembly assembly = Build("lexer.cs", "parser.cs");
            Assert.IsNotNull(assembly);

            bool errors = false;
            SyntaxTreeNode ast = Parse(assembly, "a", out errors);
            Assert.IsNotNull(ast);
            Assert.IsFalse(errors);
            ast = Parse(assembly, "A", out errors);
            Assert.IsNotNull(ast);
            Assert.IsFalse(errors);
        }

        [Test]
        public void Test001_Bug_GlobalCaseInsensitive_Rules()
        {
            string dir = GetTestDirectory();
            string grammar = "cf grammar Test { options{ Axiom=\"test\"; CaseSensitive=\"False\"; } terminals{ A->'a'; } rules{ test->A; }  }";
            Assert.IsFalse(DoCompile(grammar, ParsingMethod.LALR1, dir).HasErrors);
            Assembly assembly = Build("lexer.cs", "parser.cs");
            Assert.IsNotNull(assembly);

            bool errors = false;
            SyntaxTreeNode ast = Parse(assembly, "a", out errors);
            Assert.IsNotNull(ast);
            Assert.IsFalse(errors);
            ast = Parse(assembly, "A", out errors);
            Assert.IsNotNull(ast);
            Assert.IsFalse(errors);
        }
    }
}
