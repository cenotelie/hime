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

        private string[] BuildDefaultCommand(string dir)
        {
            string source = Path.Combine(dir, defaultGrammar);
            string lexer = Path.Combine(dir, defaultLexer);
            string parser = Path.Combine(dir, defaultParser);
            ExportResource("Exe.HimeCC." + defaultGrammar, source);
            string[] command = new String[] { source, "--lexer", lexer, "--parser", parser };
            return command;
        }
        
		[Test]
        public void Test001_DefaultNamespace_GeneratedLexer()
        {
            string dir = GetTestDirectory();
            Program.Main(BuildDefaultCommand(dir));
            File.ReadAllText(Path.Combine(dir, defaultLexer));
        }

        [Test]
        public void Test002_DefaultNamespace_GeneratedParser()
        {
            string dir = GetTestDirectory();
            Program.Main(BuildDefaultCommand(dir));
            File.ReadAllText(Path.Combine(dir, defaultParser));
        }

        [Test]
        public void Test003_DefaultNamespace_ShouldBuild()
        {
            string dir = GetTestDirectory();
            Program.Main(BuildDefaultCommand(dir));
            Assembly assembly = Build(Path.Combine(dir, defaultLexer), Path.Combine(dir, defaultParser));
            Assert.IsNotNull(assembly);
        }

		[Test]
        public void Test004_DefaultNamespace_LexerExists()
        {
            string dir = GetTestDirectory();
            Program.Main(BuildDefaultCommand(dir));
            Assembly assembly = Build(Path.Combine(dir, defaultLexer), Path.Combine(dir, defaultParser));
            System.Type lexer = assembly.GetType("MathExp.MathExpLexer");
            Assert.IsNotNull(lexer);
        }

        [Test]
        public void Test005_DefaultNamespace_ParserExists()
        {
            string dir = GetTestDirectory();
            Program.Main(BuildDefaultCommand(dir));
            Assembly assembly = Build(Path.Combine(dir, defaultLexer), Path.Combine(dir, defaultParser));
            System.Type lexer = assembly.GetType("MathExp.MathExpParser");
            Assert.IsNotNull(lexer);
        }

        [Test]
        public void Test007_DontCrashOnEmptyFile()
        {
            string dir = GetTestDirectory();
            string source = Path.Combine(dir, "Empty.gram");
            string lexer = Path.Combine(dir, defaultLexer);
            string parser = Path.Combine(dir, defaultParser);
            ExportResource("Exe.HimeCC.Empty.gram", source);
            string[] command = new String[] { source, "--lexer", lexer, "--parser", parser };
            Program.Main(command);
        }

		[Test]
        public void Test008_CanCompileCentralDogma()
        {
            string dir = GetTestDirectory();
            string source = Path.Combine(dir, "FileCentralDogma.gram");
            string lexer = Path.Combine(dir, defaultLexer);
            string parser = Path.Combine(dir, defaultParser);
            ExportResource("Exe.HimeCC.FileCentralDogma.gram", source);
            string[] command = new String[] { source, "-g", "FileCentralDogma", "--lexer", lexer, "--parser", parser, "-m", "LALR1" };
            int result = Program.Main(command);
            Assert.AreEqual(0, result);
            Assembly assembly = Build(Path.Combine(dir, defaultLexer), Path.Combine(dir, defaultParser));
            Assert.IsNotNull(assembly);
        }
	}
}
