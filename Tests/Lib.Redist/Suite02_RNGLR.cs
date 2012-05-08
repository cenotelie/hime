/*
 * Author: Charles Hymans
 * */
using System;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
using NUnit.Framework;
using Hime.Parsers;
using Hime.Utils.Reporting;
using Hime.Redist.Parsers;
using System.IO;
using System.CodeDom.Compiler;

namespace Hime.Tests.Redist
{
    [TestFixture]
    public class Suite02_RNGLR : BaseTestSuite
    {
        private Assembly assemblyECMA;

        private void CompileECMA()
        {
            string lexer = Path.Combine(directory, "ECMALexer.cs");
            string parser = Path.Combine(directory, "ECMAParser.cs");
            Report report = CompileResource("Lib.Redist.ECMAScript.gram", ParsingMethod.RNGLALR1, lexer, parser);
            if (report.HasErrors)
                return;
            assemblyECMA = Build(lexer, parser);
        }

        public Suite02_RNGLR()
        {
            try { CompileECMA(); }
            catch (Exception ex) { File.AppendAllText("Log.txt", ex.Message); }
        }

		[Test]
        public void Test000_EmptyInput()
        {
            string input = "";
            bool errors = false;
            Assert.IsNotNull(assemblyECMA, "ECMA parser is not built");
            SyntaxTreeNode root = Parse(assemblyECMA, input, out errors);
            Assert.IsFalse(errors, "Errors while parsing");
            Assert.IsNotNull(root, "No AST");
        }

        [Test]
        public void Test001_SimpleVar()
        {
            string input = "var a = 0;";
            bool errors = false;
            Assert.IsNotNull(assemblyECMA, "ECMA parser is not built");
            SyntaxTreeNode root = Parse(assemblyECMA, input, out errors);
            Assert.IsFalse(errors, "Errors while parsing");
            Assert.IsNotNull(root, "No AST");
        }
    }
}