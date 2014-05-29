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
using System.Reflection;
using Hime.CentralDogma;
using System.IO;

namespace Hime.Benchmark
{
	class Benchmark
	{
		private const string dirExtras = "Extras";
		private const string dirGrammars = "Grammars";
		private string language;
		private string input;
		private string output;
		private int sampleFactor;
		private int expCount;
		private bool rebuildInput;
		private bool rebuildParsers;
		private bool doStats;
		private bool doLexer;
		private bool doParserLALR;
		private bool doParserRNGLR;

		public Benchmark()
		{
			this.language = "CSharp4";
			this.input = "Perf.gram";
			this.output = "result.txt";
			this.sampleFactor = 600;
			this.expCount = 20;
			this.rebuildInput = true;
			this.rebuildParsers = true;
			this.doStats = false;
			this.doLexer = true;
			this.doParserLALR = false;
			this.doParserRNGLR = false;
		}

		public void Run()
		{
			if (System.IO.File.Exists(output))
				System.IO.File.Delete(output);
            
			if (rebuildInput)
				BuildInput();
            
			Assembly asmLALR = null;
			Assembly asmGLR = null;
			if (rebuildParsers)
			{
				asmLALR = Compile(ParsingMethod.LALR1);
				asmGLR = Compile(ParsingMethod.RNGLALR1);
			} else
			{
				asmLALR = Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, "gen_LALR1.dll"));
				asmGLR = Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, "gen_RNGLALR1.dll"));
			}

			if (doStats)
				OutputInputStats(asmLALR);

			if (doLexer)
			{
				System.IO.File.AppendAllText(output, "-- lexer\n");
				Console.WriteLine("-- lexer");
				for (int i = 0; i != expCount; i++)
					BenchmarkLexer(asmLALR, i);
			}

			if (doParserLALR)
			{
				System.IO.File.AppendAllText(output, "-- parser LALR\n");
				Console.WriteLine("-- parser LALR");
				for (int i = 0; i != expCount; i++)
					BenchmarkParser(asmLALR, i);
			}

			if (doParserRNGLR)
			{
				System.IO.File.AppendAllText(output, "-- parser GLR\n");
				Console.WriteLine("-- parser GLR");
				for (int i = 0; i != expCount; i++)
					BenchmarkParser(asmGLR, i);
			}
		}

		private void BuildInput()
		{
			DirectoryInfo current = new DirectoryInfo(Environment.CurrentDirectory);
			DirectoryInfo[] subs = current.GetDirectories(dirExtras);
			while (subs == null || subs.Length == 0)
			{
				current = current.Parent;
				subs = current.GetDirectories(dirExtras);
			}
			DirectoryInfo extras = subs[0];
			DirectoryInfo grammars = extras.GetDirectories(dirGrammars)[0];

			System.IO.StreamReader reader = new System.IO.StreamReader(Path.Combine(grammars.FullName, language + ".gram"));
			string content = reader.ReadToEnd();
			reader.Close();
			if (System.IO.File.Exists(input))
				System.IO.File.Delete(input);
			for (int i = 0; i != sampleFactor; i++)
				System.IO.File.AppendAllText(input, content);
		}

		private Assembly Compile(ParsingMethod method)
		{
			System.IO.Stream stream = typeof(CompilationTask).Assembly.GetManifestResourceStream("Hime.CentralDogma.Sources.Input.HimeGrammar.gram");
			CompilationTask task = new CompilationTask();
			task.Mode = Hime.CentralDogma.Output.Mode.Assembly;
			task.AddInputRaw(stream);
			task.Namespace = "Hime.Benchmark.Generated";
			task.GrammarName = "HimeGrammar";
			task.CodeAccess = Hime.CentralDogma.Output.Modifier.Public;
			task.Method = method;
			task.OutputPath = "gen_" + method.ToString();
			task.Execute();
			return Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, "gen_" + method.ToString() + ".dll"));
		}

		private void OutputInputStats(Assembly assembly)
		{
			System.IO.StreamReader reader = new System.IO.StreamReader(input);
			Hime.Redist.Lexer.ILexer lexer = GetLexer(assembly, reader);
			Hime.Redist.Token token = lexer.GetNextToken();
			int count = 0;
			while (token.SymbolID != 1)
			{
				token = lexer.GetNextToken();
				count++;
			}
			reader.Close();
			System.GC.Collect();
			System.IO.File.AppendAllText(output, "-- tokens: " + count + "\n");
			Console.WriteLine("-- tokens: " + count);
		}

		private Hime.Redist.Lexer.ILexer GetLexer(Assembly assembly, System.IO.StreamReader reader)
		{
			Type lexerType = assembly.GetType("Hime.Benchmark.Generated.HimeGrammarLexer");
			ConstructorInfo lexerConstructor = lexerType.GetConstructor(new Type[] { typeof(System.IO.TextReader) });
			object lexer = lexerConstructor.Invoke(new object[] { reader });
			return lexer as Hime.Redist.Lexer.ILexer;
		}

		private Hime.Redist.Parsers.BaseLRParser GetParser(Assembly assembly, System.IO.StreamReader reader)
		{
			Hime.Redist.Lexer.ILexer lexer = GetLexer(assembly, reader);
			Type lexerType = assembly.GetType("Hime.Benchmark.Generated.HimeGrammarLexer");
			Type parserType = assembly.GetType("Hime.Benchmark.Generated.HimeGrammarParser");
			ConstructorInfo parserConstructor = parserType.GetConstructor(new Type[] { lexerType });
			object parser = parserConstructor.Invoke(new object[] { lexer });
			return parser as Hime.Redist.Parsers.BaseLRParser;
		}

		private void BenchmarkLexer(Assembly assembly, int index)
		{
			System.IO.StreamReader reader = new System.IO.StreamReader(input);
			Hime.Redist.Lexer.ILexer lexer = GetLexer(assembly, reader);
			System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
			watch.Start();
			Hime.Redist.Token token = lexer.GetNextToken();
			while (token.SymbolID != 1)
				token = lexer.GetNextToken();
			watch.Stop();
			reader.Close();
			System.GC.Collect();
			System.IO.File.AppendAllText(output, watch.ElapsedMilliseconds + "\n");
			Console.WriteLine(watch.ElapsedMilliseconds);
		}

		private void BenchmarkParser(Assembly assembly, int index)
		{
			System.IO.StreamReader reader = new System.IO.StreamReader(input);
			Hime.Redist.Parsers.BaseLRParser parser = GetParser(assembly, reader);
			System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
			watch.Start();
			parser.Parse();
			watch.Stop();
			reader.Close();
			System.GC.Collect();
			System.IO.File.AppendAllText(output, watch.ElapsedMilliseconds + "\n");
			Console.WriteLine(watch.ElapsedMilliseconds);
		}
	}
}
