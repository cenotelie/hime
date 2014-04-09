/**********************************************************************
* Copyright (c) 2013 Laurent Wouters and others
* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU Lesser General Public License as
* published by the Free Software Foundation, either version 3
* of the License, or (at your option) any later version.
* 
* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
* GNU Lesser General Public License for more details.
* 
* You should have received a copy of the GNU Lesser General
* Public License along with this program.
* If not, see <http://www.gnu.org/licenses/>.
* 
* Contributors:
*     Laurent Wouters - lwouters@xowl.org
**********************************************************************/

using NUnit.Framework;

namespace Hime.Tests
{
	/// <summary>
	/// Test suite for the HimeCC options
	/// </summary>
    [TestFixture]
    public class HimeCCOptions : BaseTestSuite
    {
    	/// <summary>
    	/// Tests the return value of himecc on -h
    	/// </summary>
        [Test]
        public void Test_HelpLong() { Test_Help("--help"); }
        
        /// <summary>
    	/// Tests the return value of himecc on -h
    	/// </summary>
        [Test]
        public void Test_HelpShort() { Test_Help("-h"); }
        
        /// <summary>
    	/// Tests the return value of himecc on help arguments
    	/// </summary>
        /// <param name="param">The command line parameter</param>
        private void Test_Help(string param)
        {
            SetTestDirectory();
            int result = Hime.HimeCC.Program.Main(new string[] { param });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result, "himecc " + param + " did not return OK (0), returned " + result);
        }

        /// <summary>
        /// Tests the regeneration of command line and central dogma parsers with himecc --regenerate
        /// </summary>
        [Test]
        public void Test_RegenerateLong() { Test_Regeneration("--regenerate"); }

        /// <summary>
        /// Tests the regeneration of command line and central dogma parsers with himecc -r
        /// </summary>
        [Test]
        public void Test_RegenerateShort() { Test_Regeneration("-r"); }

        /// <summary>
        /// Tests the regeneration of command line and central dogma parsers with himecc
        /// </summary>
        /// <param name="param">The command line parameter</param>
        private void Test_Regeneration(string param)
        {
            SetTestDirectory();
            int result = Hime.HimeCC.Program.Main(new string[] { param });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result, "himecc " + param + " did not return OK (0), returned " + result);
            Assert.IsTrue(CheckFileExists("CommandLineLexer.cs"), "himecc " + param + " failed to produce CommandLineLexer.cs");
            Assert.IsTrue(CheckFileExists("CommandLineLexer.bin"), "himecc " + param + " failed to produce CommandLineLexer.bin");
            Assert.IsTrue(CheckFileExists("CommandLineParser.cs"), "himecc " + param + " failed to produce CommandLineParser.cs");
            Assert.IsTrue(CheckFileExists("CommandLineParser.bin"), "himecc " + param + " failed to produce CommandLineParser.bin");
            Assert.IsTrue(CheckFileExists("FileCentralDogmaLexer.cs"), "himecc " + param + " failed to produce FileCentralDogmaLexer.cs");
            Assert.IsTrue(CheckFileExists("FileCentralDogmaLexer.bin"), "himecc " + param + " failed to produce FileCentralDogmaLexer.bin");
            Assert.IsTrue(CheckFileExists("FileCentralDogmaParser.cs"), "himecc " + param + " failed to produce FileCentralDogmaParser.cs");
            Assert.IsTrue(CheckFileExists("FileCentralDogmaParser.bin"), "himecc " + param + " failed to produce FileCentralDogmaParser.bin");
        }

        /// <summary>
        /// Tests the default output of himecc on normal compilation
        /// </summary>
        [Test]
        public void Test_OutputModeDefault()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result, "himecc MathExp.gram did not return OK (0), returned " + result);
            Assert.IsTrue(CheckFileExists("MathExpLexer.cs"), "himecc MathExp.gram failed to produce MathExpLexer.cs");
            Assert.IsTrue(CheckFileExists("MathExpLexer.bin"), "himecc MathExp.gram failed to produce MathExpLexer.bin");
            Assert.IsTrue(CheckFileExists("MathExpParser.cs"), "himecc MathExp.gram failed to produce MathExpParser.cs");
            Assert.IsTrue(CheckFileExists("MathExpParser.bin"), "himecc MathExp.gram failed to produce MathExpParser.bin");
            Assert.IsFalse(CheckFileExists("MathExp.dll"), "himecc MathExp.gram produced MathExp.dll, shouldn't have");
        }

        /// <summary>
        /// Tests the output of himecc when requesting the assembly
        /// </summary>
        [Test]
        public void Test_OutputModeAssembly()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram -o:assembly" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result, "himecc MathExp.gram -o:assembly did not return OK (0), returned " + result);
            Assert.IsTrue(CheckFileExists("MathExpLexer.cs"), "himecc MathExp.gram -o:assembly failed to produce MathExpLexer.cs");
            Assert.IsTrue(CheckFileExists("MathExpLexer.bin"), "himecc MathExp.gram -o:assembly failed to produce MathExpLexer.bin");
            Assert.IsTrue(CheckFileExists("MathExpParser.cs"), "himecc MathExp.gram -o:assembly failed to produce MathExpParser.cs");
            Assert.IsTrue(CheckFileExists("MathExpParser.bin"), "himecc MathExp.gram -o:assembly failed to produce MathExpParser.bin");
            Assert.IsTrue(CheckFileExists("MathExp.dll"), "himecc MathExp.gram -o:assembly failed to produce MathExp.dll");
        }

        /// <summary>
        /// Tests the output of himecc when requesting no sources
        /// </summary>
        [Test]
        public void Test_OutputModeNoSources()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram -o:nosources" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result, "himecc MathExp.gram -o:nosources did not return OK (0), returned " + result);
            Assert.IsFalse(CheckFileExists("MathExpLexer.cs"), "himecc MathExp.gram -o:nosources produced MathExpLexer.cs, shouldn't have");
            Assert.IsFalse(CheckFileExists("MathExpLexer.bin"), "himecc MathExp.gram -o:nosources produced MathExpLexer.bin, shouldn't have");
            Assert.IsFalse(CheckFileExists("MathExpParser.cs"), "himecc MathExp.gram -o:nosources produced MathExpParser.cs, shouldn't have");
            Assert.IsFalse(CheckFileExists("MathExpParser.bin"), "himecc MathExp.gram -o:nosources produced MathExpParser.bin, shouldn't have");
            Assert.IsTrue(CheckFileExists("MathExp.dll"), "himecc MathExp.gram -o:nosources failed to produce MathExp.dll");
        }

        /// <summary>
        /// Tests the output of himecc when selecting a specific grammar among all loaded ones
        /// The grammar to compile is in the first input
        /// </summary>
        [Test]
        public void Test_GrammarName_Order()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            ExportResource("ParseTree.gram", "ParseTree.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram ParseTree.gram -g MathExp" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result, "himecc did not return OK (0), returned " + result);
            Assert.IsTrue(CheckFileExists("MathExpLexer.cs"), "himecc failed to produce MathExpLexer.cs");
            Assert.IsTrue(CheckFileExists("MathExpLexer.bin"), "himecc failed to produce MathExpLexer.bin");
            Assert.IsTrue(CheckFileExists("MathExpParser.cs"), "himecc failed to produce MathExpParser.cs");
            Assert.IsTrue(CheckFileExists("MathExpParser.bin"), "himecc failed to produce MathExpParser.bin");
        }

        /// <summary>
        /// Tests the output of himecc when selecting a specific grammar among all loaded ones
        /// The grammar to compile is NOT in the first input
        /// </summary>
        [Test]
        public void Test_GrammarName_Unordered()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            ExportResource("ParseTree.gram", "ParseTree.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "ParseTree.gram MathExp.gram -g MathExp" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result, "himecc did not return OK (0), returned " + result);
            Assert.IsTrue(CheckFileExists("MathExpLexer.cs"), "himecc failed to produce MathExpLexer.cs");
            Assert.IsTrue(CheckFileExists("MathExpLexer.bin"), "himecc failed to produce MathExpLexer.bin");
            Assert.IsTrue(CheckFileExists("MathExpParser.cs"), "himecc failed to produce MathExpParser.cs");
            Assert.IsTrue(CheckFileExists("MathExpParser.bin"), "himecc failed to produce MathExpParser.bin");
        }

        /// <summary>
        /// Tests the output of himecc when selecting a simple prefix for the output
        /// </summary>
        [Test]
        public void Test_Prefix_FileName()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram -p XXX" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result, "himecc did not return OK (0), returned " + result);
            Assert.IsTrue(CheckFileExists("XXXLexer.cs"), "himecc failed to produce XXXLexer.cs");
            Assert.IsTrue(CheckFileExists("XXXLexer.bin"), "himecc failed to produce XXXLexer.bin");
            Assert.IsTrue(CheckFileExists("XXXParser.cs"), "himecc failed to produce XXXParser.cs");
            Assert.IsTrue(CheckFileExists("XXXParser.bin"), "himecc failed to produce XXXParser.bin");
        }

        /// <summary>
        /// Tests the output of himecc when selecting a complex prefix (directory) for the output
        /// </summary>
        [Test]
        public void Test_Prefix_Directory()
        {
            SetTestDirectory();
            System.IO.Directory.CreateDirectory("YYY");
            string prefix = System.IO.Path.Combine("YYY", "XXX");
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram -p " + prefix });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result, "himecc did not return OK (0), returned " + result);
            Assert.IsTrue(CheckFileExists(prefix + "Lexer.cs"), "himecc failed to produce " + prefix + "Lexer.cs");
            Assert.IsTrue(CheckFileExists(prefix + "Lexer.bin"), "himecc failed to produce " + prefix + "Lexer.bin");
            Assert.IsTrue(CheckFileExists(prefix + "Parser.cs"), "himecc failed to produce " + prefix + "Parser.cs");
            Assert.IsTrue(CheckFileExists(prefix + "Parser.bin"), "himecc failed to produce " + prefix + "Parser.bin");
        }

        /// <summary>
        /// Tests the default namespace of the generated code produced by himecc
        /// </summary>
        [Test]
        public void Test_NamespaceDefault()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result, "himecc did not return OK (0), returned " + result);
			Assert.IsTrue(CheckFileExists("MathExpParser.cs"), "himecc failed to produce MathExpParser.cs");
			Assert.IsTrue(CheckFileContains("MathExpParser.cs", "namespace MathExp"), "Expected default namespace to be the grammar name, was not");
        }
        
        /// <summary>
        /// Tests the user-defined namespace of the generated code produced by himecc
        /// </summary>
        [Test]
        public void Test_NamespaceUserDefined()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram -n Hime.Tests.Generated" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result, "himecc did not return OK (0), returned " + result);
			Assert.IsTrue(CheckFileExists("MathExpParser.cs"), "himecc failed to produce MathExpParser.cs");
			Assert.IsTrue(CheckFileContains("MathExpParser.cs", "namespace Hime.Tests.Generated"), "Failed to generated code in the user-defined namespace");
        }
        
        /// <summary>
        /// Tests the output assembly produced by himecc on the default parsing method
        /// </summary>
        [Test]
        public void Test_MethodDefault()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result, "himecc did not return OK (0), returned " + result);
			Assert.IsTrue(CheckFileExists("MathExpParser.cs"), "himecc failed to produce MathExpParser.cs");
			Assert.IsTrue(CheckFileContains("MathExpParser.cs", "class MathExpParser : LRkParser"), "Generated parser is not an LR(k) parser by default");
        }

        /// <summary>
        /// Tests the output assembly produced by himecc on the RNGLR parsing method
        /// </summary>
        [Test]
        public void Test_MethodRNGLR()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram -m:rnglr" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result, "himecc did not return OK (0), returned " + result);
			Assert.IsTrue(CheckFileExists("MathExpParser.cs"), "himecc failed to produce MathExpParser.cs");
			Assert.IsTrue(CheckFileContains("MathExpParser.cs", "class MathExpParser : RNGLRParser"), "Generated parser is not a RNGLR parser");
        }

        /// <summary>
        /// Tests the default visibility of the code in the assembly produced by himecc
        /// </summary>
        [Test]
        public void Test_AccessDefault()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result, "himecc did not return OK (0), returned " + result);
			Assert.IsTrue(CheckFileExists("MathExpParser.cs"), "himecc failed to produce MathExpParser.cs");
			Assert.IsTrue(CheckFileContains("MathExpParser.cs", "internal class MathExpParser"), "Generated parser is not internal by default");
        }

        /// <summary>
        /// Tests the public visibility of the code in the assembly produced by himecc
        /// </summary>
        [Test]
        public void Test_AccessPublic()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram -a:public" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result, "himecc did not return OK (0), returned " + result);
            Assert.IsTrue(CheckFileExists("MathExpParser.cs"), "himecc failed to produce MathExpParser.cs");
			Assert.IsTrue(CheckFileContains("MathExpParser.cs", "public class MathExpParser"), "Generated parser is not public");
        }

        /// <summary>
        /// Tests default log production of himecc
        /// </summary>
        [Test]
        public void Test_LogDefault()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result, "himecc did not return OK (0), returned " + result);
            Assert.IsFalse(CheckFileExists("MathExpLog.mht"), "himecc produced a log file, shouldn't have");
        }

        /// <summary>
        /// Tests the activation of the log output of himecc
        /// </summary>
        [Test]
        public void Test_LogEnabled()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram -l" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result, "himecc did not return OK (0), returned " + result);
            Assert.IsTrue(CheckFileExists("MathExpLog.mht"), "himecc did not produce a log file, MathExpLog.mht was expected");
        }

        /// <summary>
        /// Tests the default documentation production of himecc
        /// </summary>
        [Test]
        public void Test_DocDefault()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result, "himecc did not return OK (0), returned " + result);
            Assert.IsFalse(System.IO.Directory.Exists("MathExpDoc"), "himecc produced some documentation, shouldn't have");
        }

        /// <summary>
        /// Tests the activation of the documentation production of himecc
        /// </summary>
        [Test]
        public void Test_DocEnabled()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram -d" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result, "himecc did not return OK (0), returned " + result);
            Assert.IsTrue(System.IO.Directory.Exists("MathExpDoc"), "himecc did not produce the documentation, MathExpDoc directory was expected");
        }
    }
}
