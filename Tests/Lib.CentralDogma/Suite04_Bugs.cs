using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using Hime.CentralDogma;
using Hime.CentralDogma.Reporting;
using Hime.Redist.AST;

namespace Hime.Tests.CentralDogma
{
    [TestFixture]
    public class Suite04_Bugs : BaseTestSuite
    {
        [Test]
        public void Test001_Bug_InlineEndLine()
        {
            string dir = GetTestDirectory();
            string grammar = "cf grammar Test { options{ Axiom=\"test\"; } terminals{} rules{ test->'\\n'; }  }";
            Assert.IsFalse(CompileRaw(grammar, ParsingMethod.LALR1).HasErrors);
            Assert.IsNotNull(Build());
        }

        [Test]
        public void Test001_Bug_CaseSensitiveInTerminals()
        {
            string dir = GetTestDirectory();
            string grammar = "cf grammar Test { options{ Axiom=\"test\"; } terminals{ A->'a'; } rules{ test->A; }  }";
            Assert.IsFalse(CompileRaw(grammar, ParsingMethod.LALR1).HasErrors);
            Assembly assembly = Build();
            Assert.IsNotNull(assembly);
            
            bool errors = false;
            ASTNode ast = Parse(assembly, "a", out errors);
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
            Assert.IsFalse(CompileRaw(grammar, ParsingMethod.LALR1).HasErrors);
            Assembly assembly = Build();
            Assert.IsNotNull(assembly);

            bool errors = false;
            ASTNode ast = Parse(assembly, "a", out errors);
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
            Assert.IsFalse(CompileRaw(grammar, ParsingMethod.LALR1).HasErrors);
            Assembly assembly = Build();
            Assert.IsNotNull(assembly);

            bool errors = false;
            ASTNode ast = Parse(assembly, "a", out errors);
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
            Assert.IsFalse(CompileRaw(grammar, ParsingMethod.LALR1).HasErrors);
            Assembly assembly = Build();
            Assert.IsNotNull(assembly);

            bool errors = false;
            ASTNode ast = Parse(assembly, "a", out errors);
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
            Assert.IsFalse(CompileRaw(grammar, ParsingMethod.LALR1).HasErrors);
            Assembly assembly = Build();
            Assert.IsNotNull(assembly);

            bool errors = false;
            ASTNode ast = Parse(assembly, "a", out errors);
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
            Assert.IsFalse(CompileRaw(grammar, ParsingMethod.LALR1).HasErrors);
            Assembly assembly = Build();
            Assert.IsNotNull(assembly);

            bool errors = false;
            ASTNode ast = Parse(assembly, "a", out errors);
            Assert.IsNotNull(ast);
            Assert.IsFalse(errors);
            ast = Parse(assembly, "A", out errors);
            Assert.IsNotNull(ast);
            Assert.IsFalse(errors);
        }
    }
}
