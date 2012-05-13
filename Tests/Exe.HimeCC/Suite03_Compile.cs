/*
 * @author Charles Hymans
 * */

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Hime.HimeCC;
using Hime.Utils.Reporting;
using Hime.Parsers;
using NUnit.Framework;

namespace Hime.Tests.HimeCC
{
    [TestFixture]
    public class Suite03_Compile: BaseTestSuite
    {
        private static string defaultGrammar = "MathExp.gram";
        private static string defaultLexer = "lexer.cs";
        private static string defaultParser = "parser.cs";

        private string[] BuildDefaultCommand(string grammar)
        {
            ExportResource("Exe.HimeCC." + grammar, grammar);
            string[] command = new String[] { grammar, "--lexer", defaultLexer, "--parser", defaultParser };
            return command;
        }
        
		[Test]
        public void Test001_DefaultNamespace_GeneratedLexer()
        {
            string dir = GetTestDirectory();
            Program.Main(BuildDefaultCommand(defaultGrammar));
            File.ReadAllText(defaultLexer);
        }

        [Test]
        public void Test002_DefaultNamespace_GeneratedParser()
        {
            string dir = GetTestDirectory();
            Program.Main(BuildDefaultCommand(defaultGrammar));
            File.ReadAllText(defaultParser);
        }

        [Test]
        public void Test003_DefaultNamespace_ShouldBuild()
        {
            string dir = GetTestDirectory();
            Program.Main(BuildDefaultCommand(defaultGrammar));
            Assembly assembly = Build(defaultLexer, defaultParser);
            Assert.IsNotNull(assembly);
        }

		[Test]
        public void Test004_DefaultNamespace_LexerExists()
        {
            string dir = GetTestDirectory();
            Program.Main(BuildDefaultCommand(defaultGrammar));
            Assembly assembly = Build(defaultLexer, defaultParser);
            System.Type lexer = assembly.GetType("MathExp.MathExpLexer");
            Assert.IsNotNull(lexer);
        }

        [Test]
        public void Test005_DefaultNamespace_ParserExists()
        {
            string dir = GetTestDirectory();
            Program.Main(BuildDefaultCommand(defaultGrammar));
            Assembly assembly = Build(defaultLexer, defaultParser);
            System.Type lexer = assembly.GetType("MathExp.MathExpParser");
            Assert.IsNotNull(lexer);
        }

        [Test]
        public void Test007_DontCrashOnEmptyFile()
        {
            string dir = GetTestDirectory();
            Program.Main(BuildDefaultCommand("Empty.gram"));
        }

		[Test]
        public void Test008_CanCompileCentralDogma()
        {
            string dir = GetTestDirectory();
            string source = "FileCentralDogma.gram";
            ExportResource("Exe.HimeCC.FileCentralDogma.gram", source);
            string[] command = new String[] { source, "-g", "FileCentralDogma", "--lexer", defaultLexer, "--parser", defaultParser, "-m", "LALR1" };
            int result = Program.Main(command);
            Assert.AreEqual(0, result);
            Assembly assembly = Build(defaultLexer, defaultParser);
            Assert.IsNotNull(assembly);
        }
	}
}
