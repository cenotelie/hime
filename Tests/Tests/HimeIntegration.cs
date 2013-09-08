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

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Hime.CentralDogma;
using Hime.Redist.AST;
using Hime.Redist.Lexer;
using Hime.Redist.Parsers;
using Hime.Redist.Symbols;
using NUnit.Framework;

namespace Hime.Tests
{
    [TestFixture]
    public class HimeIntegration : BaseTestSuite
    {
        private ConstructorInfo scriptLexer;
        private ConstructorInfo scriptParser;
        private Dictionary<string, ConstructorInfo> resLexer;
        private Dictionary<string, ConstructorInfo> resParser;

        public HimeIntegration()
        {
            GenerateTestScriptParser();
            resLexer = new Dictionary<string, ConstructorInfo>();
            resParser = new Dictionary<string, ConstructorInfo>();
        }

        private void GenerateTestScriptParser()
        {
            Assembly assembly = CompileResource("TestScript", ParsingMethod.LALR1);
            Type tl = assembly.GetType("Hime.Tests.Generated.TestScriptLexer");
            Type tp = assembly.GetType("Hime.Tests.Generated.TestScriptParser");
            scriptLexer = tl.GetConstructor(new Type[] { typeof(string) });
            scriptParser = tp.GetConstructor(new Type[] { tl });
        }

        private void ResolveParserFor(string resource, ParsingMethod method)
        {
            if (resParser.ContainsKey(resource))
                return;
            Assembly assembly = CompileResource(resource, method);
            System.Type tl = assembly.GetType("Hime.Tests.Generated." + resource + "Lexer");
            System.Type tp = assembly.GetType("Hime.Tests.Generated." + resource + "Parser");
            System.Reflection.ConstructorInfo cl = tl.GetConstructor(new Type[] { typeof(string) });
            System.Reflection.ConstructorInfo cp = tp.GetConstructor(new Type[] { tl });
            resLexer.Add(resource, cl);
            resParser.Add(resource, cp);
        }

        private bool Compare(ASTNode node, ASTNode check)
        {
            if (node.Symbol.Name != (check.Symbol as Token).Value)
                return false;
            if (check.Children[0].Children.Count != 0)
            {
                string vRef = (check.Children[0].Children[0].Symbol as Token).Value;
                vRef = vRef.Substring(1, vRef.Length - 2);
                string vReal = (node.Symbol as Token).Value;
                if (vReal != vRef)
                    return false;
            }
            if (node.Children.Count != check.Children[1].Children.Count)
                return false;
            for (int i = 0; i != node.Children.Count; i++)
                if (!Compare(node.Children[i], check.Children[1].Children[i]))
                    return false;
            return true;
        }

        private bool ExecuteTest(ASTNode test, ParsingMethod method)
        {
            string name = (test.Symbol as Token).Value;
            string resource = (test.Children[0].Symbol as Token).Value;
            string input = (test.Children[1].Symbol as Token).Value;
            input = input.Substring(1, input.Length - 2);
            ASTNode check = test.Children[2];
            ResolveParserFor(resource, method);
            ILexer lc = resLexer[resource].Invoke(new object[] { input }) as ILexer;
            IParser pc = resParser[resource].Invoke(new object[] { lc }) as IParser;
            bool result = Compare(pc.Parse(), check);
            if (!result)
                Log("Test " + name + " failed");
            return result;
        }

        private bool ExecuteScript(string script, ParsingMethod method)
        {
            Log("Executing test suite " + script);
            ExportResource(script, script);
            ILexer lc = scriptLexer.Invoke(new object[] { File.ReadAllText(script) }) as ILexer;
            IParser pc = scriptParser.Invoke(new object[] { lc }) as IParser;
            ASTNode root = pc.Parse();
            int succeeded = 0;
            int failed = 0;
            foreach (ASTNode test in root.Children)
                if (ExecuteTest(test, method))
                    succeeded++;
                else
                    failed++;
            Log(succeeded.ToString() + " passed, " + failed.ToString() + " failed");
            return (failed == 0);
        }

        [Test]
        public void Test_Integration()
        {
            SetTestDirectory();
            Assert.IsTrue(ExecuteScript("Integration.tests", ParsingMethod.LALR1));
        }
    }
}
