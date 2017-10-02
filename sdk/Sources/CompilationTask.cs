/*******************************************************************************
 * Copyright (c) 2017 Association Cénotélie (cenotelie.fr)
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
 ******************************************************************************/

using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;

namespace Hime.SDK
{
	/// <summary>
	/// Represents a compilation task for the generation of lexers and parsers from grammars
	/// </summary>
	public class CompilationTask
	{
		/// <summary>
		/// Gets the compiler's version
		/// </summary>
		public static string Version
		{
			get
			{
				return typeof(CompilationTask).Assembly.GetName().Version.ToString();
			}
		}

		/// <summary>
		/// The name of the grammar to compile in the case where several grammars are loaded.
		/// </summary>
		private string grammarName;
		/// <summary>
		/// The compiler's output mode
		/// </summary>
		private Output.Mode? outputMode;
		/// <summary>
		/// The target runtime
		/// </summary>
		private Output.Runtime? outputTarget;
		/// <summary>
		/// The path for the compiler's output
		/// </summary>
		private string outputPath;
		/// <summary>
		/// The namespace for the generated code
		/// </summary>
		private string outputNamespace;
		/// <summary>
		/// The access modifier for the generated code
		/// </summary>
		private Output.Modifier? outputAccess;
		/// <summary>
		/// The parsing method use
		/// </summary>
		private ParsingMethod? method;
		/// <summary>
		/// The reporter
		/// </summary>
		protected Reporter reporter;
		/// <summary>
		/// The loader for this task
		/// </summary>
		protected Input.Loader loader;

		/// <summary>
		/// Gets or sets the name of the grammar to compile.
		/// When only one grammar is loaded, it will be automatically selected.
		/// </summary>
		public string GrammarName
		{
			get { return grammarName; }
			set { grammarName = value; }
		}

		/// <summary>
		/// Gets ot sets the compiler's mode
		/// </summary>
		public Output.Mode Mode
		{
			get { return outputMode.HasValue ? outputMode.Value : Output.Mode.Source; }
			set { outputMode = value; }
		}

		/// <summary>
		/// Gets or sets the target runtime
		/// </summary>
		public Output.Runtime Target
		{
			get { return outputTarget.HasValue ? outputTarget.Value : Output.Runtime.Net; }
			set { outputTarget = value; }
		}

		/// <summary>
		/// Gets ot sets the compiler's output files' path.
		/// If this property is not set, the path will be the current directory.
		/// </summary>
		public string OutputPath
		{
			get { return outputPath; }
			set { outputPath = value; }
		}

		/// <summary>
		/// Gets or sets the namespace in which the generated Lexer and Parser classes will be put.
		/// If this property is not set, the namespace will be the name of the grammar.
		/// </summary>
		public string Namespace
		{
			get { return outputNamespace; }
			set { outputNamespace = value; }
		}

		/// <summary>
		/// Gets or sets the access modifiers for the generated Lexer and Parser classes.
		/// The default value is Internal.
		/// </summary>
		public Output.Modifier CodeAccess
		{
			get { return outputAccess.HasValue ? outputAccess.Value : Output.Modifier.Internal; }
			set { outputAccess = value; }
		}

		/// <summary>
		/// Gets or sets the parsing method to use.
		/// The default value is LALR1.
		/// </summary>
		public ParsingMethod Method
		{
			get { return method.HasValue ? method.Value : ParsingMethod.LALR1; }
			set { method = value; }
		}

		/// <summary>
		/// Initializes a new compilation task
		/// </summary>
		public CompilationTask()
			: this(new Reporter())
		{
		}

		/// <summary>
		/// Initializes a new compilation task
		/// </summary>
		/// <param name="reporter">The reported to use</param>
		public CompilationTask(Reporter reporter)
		{
			this.grammarName = null;
			this.outputMode = null;
			this.outputTarget = null;
			this.outputPath = null;
			this.outputNamespace = null;
			this.outputAccess = null;
			this.method = null;
			this.reporter = reporter;
			this.loader = new Input.Loader(this.reporter);
		}

		/// <summary>
		/// Adds a new file as input
		/// </summary>
		/// <param name="file">The input file</param>
		public void AddInputFile(string file)
		{
			loader.AddInputFile(file);
		}

		/// <summary>
		/// Adds a new data string as input
		/// </summary>
		/// <param name="data">The data string</param>
		public void AddInputRaw(string data)
		{
			loader.AddInputRaw(data);
		}

		/// <summary>
		/// Adds a new named data string as input
		/// </summary>
		/// <param name="name">The input's name</param>
		/// <param name="data">The data string</param>
		public void AddInputRaw(string name, string data)
		{
			loader.AddInputRaw(name, data);
		}

		/// <summary>
		/// Adds a new data stream as input
		/// </summary>
		/// <param name="stream">The input stream</param>
		public void AddInputRaw(Stream stream)
		{
			loader.AddInputRaw(stream);
		}

		/// <summary>
		/// Adds a new named data stream as input
		/// </summary>
		/// <param name="name">The input's name</param>
		/// <param name="stream">The input stream</param>
		public void AddInputRaw(string name, Stream stream)
		{
			loader.AddInputRaw(name, stream);
		}

		/// <summary>
		/// Adds a new data reader as input
		/// </summary>
		/// <param name="reader">The input reader</param>
		public void AddInputRaw(TextReader reader)
		{
			loader.AddInputRaw(reader);
		}

		/// <summary>
		/// Adds a new named data reader as input
		/// </summary>
		/// <param name="name">The input's name</param>
		/// <param name="reader">The input reader</param>
		public void AddInputRaw(string name, TextReader reader)
		{
			loader.AddInputRaw(name, reader);
		}

		/// <summary>
		/// Adds the specified pre-parsed grammar to the inputs
		/// </summary>
		/// <param name="node">The parse tree of a grammar</param>
		/// <param name="input">The input that contains the grammar</param>
		public void AddInput(Hime.Redist.ASTNode node, Hime.Redist.Text input)
		{
			loader.AddInput(node, input);
		}

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
			reporter.Info("Hime.SDK " + Version);
			// Load data
			List<Grammars.Grammar> inputs = loader.Load();
			if (inputs == null)
				return;
			// resolve the target grammars to compile
			if (inputs.Count == 0)
			{
				reporter.Error("No grammar in inputs");
				return;
			}
            List<Output.Unit> units = ExecuteDoPrepareUnits(inputs);
            if (units.Count == 0)
                return;
            ExecuteDoEmit(units);
		}

        /// <summary>
        /// Prepares the compilation units given grammar inputs
        /// </summary>
        /// <param name="inputs">The loaded grammars</param>
        /// <returns>The compilation units</returns>
        protected List<Output.Unit> ExecuteDoPrepareUnits(List<Grammars.Grammar> inputs)
        {
            List<Output.Unit> units = new List<Output.Unit>();
            if (grammarName != null)
            {
                foreach (Grammars.Grammar potential in inputs)
                {
                    if (potential.Name == GrammarName)
                    {
                        units.Add(new Output.Unit(
                            potential,
                            GetOutputPathFor(potential),
                            GetCompilationModeFor(potential),
                            GetParsingMethodFor(potential),
                            GetNamespaceFor(potential),
                            GetAccessModifierFor(potential)));
                        break;
                    }
                }
                if (units.Count == 0)
                {
                    reporter.Error("Grammar " + grammarName + " cannot be found");
                    return units;
                }
            }
            else
            {
                foreach (Grammars.Grammar potential in inputs)
                {
                    units.Add(new Output.Unit(
                        potential,
                        GetOutputPathFor(potential),
                        GetCompilationModeFor(potential),
                        GetParsingMethodFor(potential),
                        GetNamespaceFor(potential),
                        GetAccessModifierFor(potential)));
                }
            }
            return units;
        }

        /// <summary>
        /// Emits the artifacts for multiple compilation units
        /// </summary>
        /// <param name="units">The compilation units</param>
        protected void ExecuteDoEmit(List<Output.Unit> units)
        {
            if (units.Count == 1)
            {
                // only one target
                ExecuteDoEmitSingleUnit(units[0]);
            }
            else if (outputMode.HasValue && outputTarget.HasValue)
            {
                // same mode and same target for all units
                // (may generate a single assembly for all units, if requested)
                ExecuteDoEmitSameModeSameTarget(units);
            }
            else
            {
                // treat all units as separate
                foreach (Output.Unit unit in units)
                    ExecuteDoEmitSingleUnit(unit);
            }
        }

        /// <summary>
        /// Emits the artifacts for a single compilation unit
        /// </summary>
        /// <param name="unit">The compilation unit</param>
        protected void ExecuteDoEmitSingleUnit(Output.Unit unit)
        {
            Output.EmitterBase emitter = null;
            switch (GetTargetRuntimeFor(unit.Grammar))
            {
                case Output.Runtime.Net:
                    emitter = new Output.EmitterForNet(reporter, unit);
                    break;
                case Output.Runtime.Java:
                    emitter = new Output.EmitterForJava(reporter, unit);
                    break;
            }
            bool success = emitter.Emit();
            if (!success)
                reporter.Error("Failed to emit some output");
        }

        /// <summary>
        /// Emits the artifacts for multiple compilation units with the same compilation mode and target runtime
        /// </summary>
        /// <param name="units">The compilation units</param>
        protected void ExecuteDoEmitSameModeSameTarget(List<Output.Unit> units)
        {
            Output.EmitterBase emitter = null;
            switch (GetTargetRuntimeFor(units[0].Grammar))
            {
                case Output.Runtime.Net:
                    emitter = new Output.EmitterForNet(reporter, units);
                    break;
                case Output.Runtime.Java:
                    emitter = new Output.EmitterForJava(reporter, units);
                    break;
            }
            bool success = emitter.Emit();
            if (!success)
                reporter.Error("Failed to emit some output");
        }



		/// <summary>
		/// Gets the output path applicable for a specific grammar
		/// </summary>
		/// <param name="grammar">A grammar</param>
		/// <returns>The output path for the grammar</returns>
		private string GetOutputPathFor(Grammars.Grammar grammar)
		{
			string path = GetOutputPathForBase(grammar);
			if (path.Length > 0 && !path.EndsWith(Path.DirectorySeparatorChar.ToString()))
				path += Path.DirectorySeparatorChar;
			return path;
		}

		/// <summary>
		/// Gets the output path applicable for a specific grammar
		/// </summary>
		/// <param name="grammar">A grammar</param>
		/// <returns>The output path for the grammar</returns>
		private string GetOutputPathForBase(Grammars.Grammar grammar)
		{
			if (outputPath != null)
				return outputPath;
			string value = grammar.GetOption(Grammars.Grammar.OPTION_OUTPUT_PATH);
			if (value != null)
				return value;
			return "";
		}

		/// <summary>
		/// Gets the compilation mode applicable for a specific grammar
		/// </summary>
		/// <param name="grammar">A grammar</param>
		/// <returns>The compilation mode for the grammar</returns>
		private Output.Mode GetCompilationModeFor(Grammars.Grammar grammar)
		{
			if (outputMode.HasValue)
				return outputMode.Value;
			string value = grammar.GetOption(Grammars.Grammar.OPTION_COMPILATION_MODE);
			if (value == null)
				return Output.Mode.Source;
			if (value.Equals("Assembly"))
				return Output.Mode.Assembly;
			if (value.Equals("SourceAndAssembly"))
				return Output.Mode.SourceAndAssembly;
			if (value.Equals("Debug"))
				return Output.Mode.Debug;
			return Output.Mode.Source;
		}

		/// <summary>
		/// Gets the target runtime applicable for a specific grammar
		/// </summary>
		/// <param name="grammar">A grammar</param>
		/// <returns>The target runtime for the grammar</returns>
		private Output.Runtime GetTargetRuntimeFor(Grammars.Grammar grammar)
		{
			if (outputTarget.HasValue)
				return outputTarget.Value;
			string value = grammar.GetOption(Grammars.Grammar.OPTION_RUNTIME);
			if (value == null)
				return Output.Runtime.Net;
			if (value.Equals("Java"))
				return Output.Runtime.Java;
			return Output.Runtime.Net;
		}

		/// <summary>
		/// Gets the parsing method applicable for a specific grammar
		/// </summary>
		/// <param name="grammar">A grammar</param>
		/// <returns>The parsing method for the grammar</returns>
		private ParsingMethod GetParsingMethodFor(Grammars.Grammar grammar)
		{
			if (method.HasValue)
				return method.Value;
			string value = grammar.GetOption(Grammars.Grammar.OPTION_PARSER_TYPE);
			if (value == null)
				return ParsingMethod.LALR1;
			if (value.Equals("RNGLR"))
				return ParsingMethod.RNGLALR1;
			return ParsingMethod.LALR1;
		}

		/// <summary>
		/// Gets the namespace applicable for a specific grammar
		/// </summary>
		/// <param name="grammar">A grammar</param>
		/// <returns>The namespace for the grammar</returns>
		private string GetNamespaceFor(Grammars.Grammar grammar)
		{
			if (outputNamespace != null)
				return outputNamespace;
			string value = grammar.GetOption(Grammars.Grammar.OPTION_NAMESPACE);
			if (value != null)
				return value;
			return grammar.Name;
		}

		/// <summary>
		/// Gets the access modifier applicable for a specific grammar
		/// </summary>
		/// <param name="grammar">A grammar</param>
		/// <returns>The access modifier for the grammar</returns>
		private Output.Modifier GetAccessModifierFor(Grammars.Grammar grammar)
		{
			if (outputAccess.HasValue)
				return outputAccess.Value;
			string value = grammar.GetOption(Grammars.Grammar.OPTION_ACCESS_MODIFIER);
			if (value == null)
				return Output.Modifier.Internal;
			if (value.Equals("Public"))
				return Output.Modifier.Public;
			return Output.Modifier.Internal;
		}
	}
}