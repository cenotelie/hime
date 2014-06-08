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
using System.Collections.Generic;
using System.IO;

namespace Hime.CentralDogma
{
	/// <summary>
	/// Represents a compilation task for the generation of lexers and parsers from grammars
	/// </summary>
	public class CompilationTask
	{
		/// <summary>
		/// Gets the compiler's version
		/// </summary>
		public static string Version { get { return typeof(CompilationTask).Assembly.GetName().Version.ToString(); } }

		/// <summary>
		/// Gets or sets the name of the grammar to compile in the case where several grammars are loaded.
		/// When only one grammar is loaded, it will be automatically selected.
		/// </summary>
		public string GrammarName { get; set; }

		/// <summary>
		/// Gets ot sets the compiler's mode
		/// </summary>
		public Output.Mode Mode { get; set; }

		/// <summary>
		/// Gets or sets the target runtime
		/// </summary>
		public Output.Runtime Target { get; set; }

		/// <summary>
		/// Gets ot sets the compiler's output files' path.
		/// If this property is not set, the path will be the current directory.
		/// </summary>
		public string OutputPath { get; set; }

		/// <summary>
		/// Gets or sets the namespace in which the generated Lexer and Parser classes will be put.
		/// If this property is not set, the namespace will be the name of the grammar.
		/// </summary>
		public string Namespace { get; set; }

		/// <summary>
		/// Gets or sets the access modifiers for the generated Lexer and Parser classes.
		/// The default value is Internal.
		/// </summary>
		public Output.Modifier CodeAccess { get; set; }

		/// <summary>
		/// Gets or sets the parsing method to use.
		/// The default value is LALR1.
		/// </summary>
		public ParsingMethod Method { get; set; }

		/// <summary>
		/// The reporter
		/// </summary>
		protected Reporter reporter;
		/// <summary>
		/// The loader for this task
		/// </summary>
		protected Input.Loader loader;

		/// <summary>
		/// Initializes a new compilation task
		/// </summary>
		public CompilationTask() : this(new Reporter()) { }
		/// <summary>
		/// Initializes a new compilation task
		/// </summary>
		/// <param name="reporter">The reported to use</param>
		public CompilationTask(Reporter reporter)
		{
			this.Mode = Output.Mode.Source;
			this.Target = Output.Runtime.Net;
			this.Method = ParsingMethod.LALR1;
			this.CodeAccess = Output.Modifier.Internal;
			this.reporter = reporter;
			this.loader = new Input.Loader(this.reporter);
		}

		/// <summary>
		/// Adds a new file as input
		/// </summary>
		/// <param name="file">The input file</param>
		public void AddInputFile(string file) { loader.AddInputFile(file); }
		/// <summary>
		/// Adds a new data string as input
		/// </summary>
		/// <param name="data">The data string</param>
		public void AddInputRaw(string data) { loader.AddInputRaw(data); }
		/// <summary>
		/// Adds a new named data string as input
		/// </summary>
		/// <param name="name">The input's name</param>
		/// <param name="data">The data string</param>
		public void AddInputRaw(string name, string data) { loader.AddInputRaw(name, data); }
		/// <summary>
		/// Adds a new data stream as input
		/// </summary>
		/// <param name="stream">The input stream</param>
		public void AddInputRaw(Stream stream) { loader.AddInputRaw(stream); }
		/// <summary>
		/// Adds a new named data stream as input
		/// </summary>
		/// <param name="name">The input's name</param>
		/// <param name="stream">The input stream</param>
		public void AddInputRaw(string name, Stream stream) { loader.AddInputRaw(name, stream); }
		/// <summary>
		/// Adds a new data reader as input
		/// </summary>
		/// <param name="reader">The input reader</param>
		public void AddInputRaw(TextReader reader) { loader.AddInputRaw(reader); }
		/// <summary>
		/// Adds a new named data reader as input
		/// </summary>
		/// <param name="name">The input's name</param>
		/// <param name="reader">The input reader</param>
		public void AddInputRaw(string name, TextReader reader) { loader.AddInputRaw(name, reader); }
		/// <summary>
		/// Adds the specified pre-parsed grammar to the inputs
		/// </summary>
		/// <param name="node">The parse tree of a grammar</param>
		/// <param name="input">The input that contains the grammar</param>
		public void AddInput(Hime.Redist.ASTNode node, Hime.Redist.Text input) { loader.AddInput(node, input); }

		/// <summary>
		/// Executes this compilation task
		/// </summary>
		/// <returns>The compilation report</returns>
		public Report Execute()
		{
			try
			{
				ExecuteDo();
			}
			catch (System.Exception ex)
			{
				reporter.Error(ex);
			}
			return reporter.Result;
		}

		/// <summary>
		/// Executes the compilation task
		/// </summary>
		protected void ExecuteDo()
		{
			reporter.Info("Hime.CentralDogma " + Version);

			// Load data
			List<Grammars.Grammar> grammars = loader.Load();
			if (grammars == null)
				return;

			// resolve the target grammar to compile
			Grammars.Grammar target = null;
			if (GrammarName != null)
			{
				foreach (Grammars.Grammar potential in grammars)
				{
					if (potential.Name == GrammarName)
					{
						target = potential;
						break;
					}
				}
				if (target == null)
					reporter.Error("Grammar " + GrammarName + " cannot be found");
			}
			else
			{
				if (grammars.Count > 1)
					reporter.Error("Inputs contain more than one grammar, cannot decide which one to compile");
				else if (grammars.Count == 0)
					reporter.Error("No grammar in inputs");
				else
					target = grammars[0];
			}
			if (target == null)
				return;

			// prepare the target grammar
			reporter.Info("Preparing grammar " + target.Name + " ...");
			string message = target.Prepare();
			if (message != null)
			{
				reporter.Error(message);
				return;
			}

			// Build names
			string nmspace = (Namespace != null) ? Namespace : target.Name;

			// emit the artifacts
			Output.EmitterBase emitter = null;
			switch (Target)
			{
				case Output.Runtime.Net:
					emitter = new Output.EmitterForNet(reporter, target);
					break;
				case Output.Runtime.Java:
					emitter = new Output.EmitterForJava(reporter, target);
					break;
			}
			emitter.Emit((OutputPath != null) ? OutputPath : "", nmspace, CodeAccess, Method, Mode);
		}
	}
}