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
	/// Test suite for the return values of the HimeCC program
	/// </summary>
    [TestFixture]
    public class HimeCCReturn : BaseTestSuite
    {
    	/// <summary>
    	/// Tests the return value of himecc without arguments
    	/// </summary>
        [Test]
        public void Test_NoArgument()
        {
            int result = Hime.HimeCC.Program.Main(null);
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result, "HimeCC without argument did not return OK (0), returned " + result);
            result = Hime.HimeCC.Program.Main(new string[0]);
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result, "HimeCC with empty arguments did not return OK (0), returned " + result);
        }

        /// <summary>
        /// Tests the return value of himecc when failing to parse arguments
        /// </summary>
        [Test]
        public void Test_GibberishCommandLine()
        {
            int result = Hime.HimeCC.Program.Main(new string[] { "'\"ç" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultErrorParsingArgs, result, "HimeCC with unparsable arguments did not return parse failure (1), returned " + result);
        }

        /// <summary>
        /// Tests the return value of himecc on unknown arguments
        /// </summary>
        [Test]
        public void Test_UnknownArgument()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram -x aa" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultErrorBadArgs, result, "HimeCC with unknown arguments did not return bad argument (2), returned " + result);
            Assert.IsFalse(CheckFileExists("MathExpLexer.cs"), "HimeCC with unknown arguments produced unexpected output (MathExpLexer.cs)");
            Assert.IsFalse(CheckFileExists("MathExpLexer.bin"), "HimeCC with unknown arguments produced unexpected output (MathExpLexer.bin)");
            Assert.IsFalse(CheckFileExists("MathExpParser.cs"), "HimeCC with unknown arguments produced unexpected output (MathExpParser.cs)");
            Assert.IsFalse(CheckFileExists("MathExpParser.bin"), "HimeCC with unknown arguments produced unexpected output (MathExpParser.bin)");
        }

        /// <summary>
        /// Tests the return value of himecc on compilation failure
        /// </summary>
        [Test]
        public void Test_CompilationError()
        {
            SetTestDirectory();
            ExportResource("Error.gram", "Error.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "Error.gram" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultErrorCompiling, result, "HimeCC on compilation failure did not report failure (3), returned " + result);
            Assert.IsFalse(CheckFileExists("ErrorLexer.cs"), "HimeCC on compilation failure produced unexpected output (ErrorLexer.cs)");
            Assert.IsFalse(CheckFileExists("ErrorLexer.bin"), "HimeCC on compilation failure produced unexpected output (ErrorLexer.bin)");
            Assert.IsFalse(CheckFileExists("ErrorParser.cs"), "HimeCC on compilation failure produced unexpected output (ErrorParser.cs)");
            Assert.IsFalse(CheckFileExists("ErrorParser.bin"), "HimeCC on compilation failure produced unexpected output (ErrorParser.bin)");
        }

        /// <summary>
        /// Tests the return value of himecc on normal compilation
        /// </summary>
        [Test]
        public void Test_NominalCompilation()
        {
            SetTestDirectory();
            ExportResource("MathExp.gram", "MathExp.gram");
            int result = Hime.HimeCC.Program.Main(new string[] { "MathExp.gram" });
            Assert.AreEqual(Hime.HimeCC.Program.ResultOK, result, "HimeCC on normal compilation did not report success (0), returned " + result);
            Assert.IsTrue(CheckFileExists("MathExpLexer.cs"), "HimeCC on normal compilation did not produced expected output (MathExpLexer.cs)");
            Assert.IsTrue(CheckFileExists("MathExpLexer.bin"), "HimeCC on normal compilation did not produced expected output (MathExpLexer.bin)");
            Assert.IsTrue(CheckFileExists("MathExpParser.cs"), "HimeCC on normal compilation did not produced expected output (MathExpParser.cs)");
            Assert.IsTrue(CheckFileExists("MathExpParser.bin"), "HimeCC on normal compilation did not produced expected output (MathExpParser.bin)");
        }
    }
}
