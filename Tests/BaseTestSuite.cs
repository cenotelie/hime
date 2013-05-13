using System;
using System.IO;
using System.Reflection;
using Hime.CentralDogma;
using Hime.Redist.AST;
using Hime.Redist.Symbols;
using Hime.Redist.Lexer;
using Hime.Redist.Parsers;

namespace Hime.Tests
{
    public abstract class BaseTestSuite
    {
        private const string log = "Log.txt";
        private const string output = "output";

        private static ConstructorInfo astCheckLexer;
        private static ConstructorInfo astCheckParser;

        private ResourceAccessor accessor;
        private string directory;

        protected BaseTestSuite()
        {
            accessor = new ResourceAccessor(Assembly.GetExecutingAssembly(), "Resources");
            directory = "Data_" + this.GetType().Name;
            try
            {
                if (Directory.Exists(directory))
                    Directory.Delete(directory, true);
                Directory.CreateDirectory(directory);
            }
            catch (IOException ex)
            {
                File.AppendAllText(log, ex.Message + Environment.NewLine);
            }
        }

        private bool Compare(ASTNode node, ASTNode check)
        {
            if (node.Symbol.Name != (check.Symbol as Token).Value)
                return false;
            if (check.Children[0].Children.Count != 0)
            {
                string vRef = (check.Children[0].Children[0].Symbol as Token).Value;
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

        private void BuildASTCheckParser()
        {
            string gram = accessor.GetAllTextFor("ASTCheck.gram");
            CompilationTask task = new CompilationTask();
            task.AddInputRaw(gram);
            task.CodeAccess = AccessModifier.Public;
            task.Method = ParsingMethod.LALR1;
            task.Mode = CompilationMode.Assembly;
            task.Namespace = "Hime.Tests.Generated";
            task.Execute();
            Assembly assembly = Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, "ASTCheck.dll"));
            Type tl = assembly.GetType("Hime.Tests.Generated.ASTCheckLexer");
            Type tp = assembly.GetType("Hime.Tests.Generated.ASTCheckParser");
            astCheckLexer = tl.GetConstructor(new Type[] { typeof(string) });
            astCheckParser = tp.GetConstructor(new Type[] { tl });
        }

        protected void SetTestDirectory() {
            System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace();
            System.Diagnostics.StackFrame caller = trace.GetFrame(1);
            string dir = Path.Combine(directory, caller.GetMethod().Name);
            if (Directory.Exists(dir))
                Directory.Delete(dir, true);
            Directory.CreateDirectory(dir);
            Environment.CurrentDirectory = dir;
        }

        protected void ExportResource(string name, string file) { accessor.Export(name, file); }

        protected bool TestInput(string resource, string input, string check)
        {
            if (astCheckLexer == null)
                BuildASTCheckParser();
            
            ExportResource(resource + ".gram", resource + ".gram");
            int result = Hime.HimeCC.Program.Main(new string[] { resource + ".gram -o:nosources -a:public -n Hime.Tests.Generated" });
            System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, resource + ".dll"));
            System.Type tl = assembly.GetType("Hime.Tests.Generated." + resource + "Lexer");
            System.Type tp = assembly.GetType("Hime.Tests.Generated." + resource + "Parser");
            System.Reflection.ConstructorInfo cl = tl.GetConstructor(new Type[] { typeof(string) });
            System.Reflection.ConstructorInfo cp = tp.GetConstructor(new Type[] { tl });
            ILexer li = cl.Invoke(new object[] { input }) as ILexer;
            IParser pi = cp.Invoke(new object[] { li }) as IParser;
            ILexer lc =  astCheckLexer.Invoke(new object[] { check }) as ILexer;
            IParser pc = astCheckParser.Invoke(new object[] { lc }) as IParser;
            return Compare(pi.Parse(), pc.Parse());
        }
    }
}