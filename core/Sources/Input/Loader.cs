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
using Hime.Redist;

namespace Hime.CentralDogma.Input
{
	/// <summary>
	/// Represents a loader of inputs that produces grammars
	/// </summary>
	/// <remarks>
	/// This class will close all the streams it is passed after loading the data from them.
	/// </remarks>
	public class Loader
	{
		/// <summary>
		/// Next unique identifier for raw (anonymous) inputs
		/// </summary>
		private int nextRawID;
		/// <summary>
		/// Repositories of inputs
		/// </summary>
		private List<KeyValuePair<string, TextReader>> inputs;
		/// <summary>
		/// Repositories of pre-parsed inputs
		/// </summary>
		private List<KeyValuePair<ASTNode, Text>> preReadInputs;
		/// <summary>
		/// Repositories of inner loaders
		/// </summary>
		private Dictionary<string, Grammars.Loader> inners;
		/// <summary>
		/// The reporter
		/// </summary>
		private Reporter reporter;

		/// <summary>
		/// Initializes this loader
		/// </summary>
		public Loader() : this(new Reporter())
		{
		}
		/// <summary>
		/// Initializes this loader
		/// </summary>
		/// <param name="reporter">The reporter to use</param>
		public Loader(Reporter reporter)
		{
			this.nextRawID = 0;
			this.inputs = new List<KeyValuePair<string, TextReader>>();
			this.preReadInputs = new List<KeyValuePair<ASTNode, Text>>();
			this.inners = new Dictionary<string, Grammars.Loader>();
			this.reporter = reporter;
		}

		/// <summary>
		/// Gets a unique identifier for a raw (anonymous) input
		/// </summary>
		/// <returns>A unique identifier</returns>
		private string GetRawInputID()
		{
			return "raw" + (nextRawID++);
		}

		/// <summary>
		/// Adds a new file as input
		/// </summary>
		/// <param name="file">The input file</param>
		public void AddInputFile(string file)
		{
			inputs.Add(new KeyValuePair<string, TextReader>(file, new StreamReader(file)));
		}
		/// <summary>
		/// Adds a new data string as input
		/// </summary>
		/// <param name="data">The data string</param>
		public void AddInputRaw(string data)
		{
			inputs.Add(new KeyValuePair<string, TextReader>(GetRawInputID(), new StringReader(data)));
		}
		/// <summary>
		/// Adds a new named data string as input
		/// </summary>
		/// <param name="name">The input's name</param>
		/// <param name="data">The data string</param>
		public void AddInputRaw(string name, string data)
		{
			inputs.Add(new KeyValuePair<string, TextReader>(name, new StringReader(data)));
		}
		/// <summary>
		/// Adds a new data stream as input
		/// </summary>
		/// <param name="stream">The input stream</param>
		public void AddInputRaw(Stream stream)
		{
			inputs.Add(new KeyValuePair<string, TextReader>(GetRawInputID(), new StreamReader(stream)));
		}
		/// <summary>
		/// Adds a new named data stream as input
		/// </summary>
		/// <param name="name">The input's name</param>
		/// <param name="stream">The input stream</param>
		public void AddInputRaw(string name, Stream stream)
		{
			inputs.Add(new KeyValuePair<string, TextReader>(name, new StreamReader(stream)));
		}
		/// <summary>
		/// Adds a new data reader as input
		/// </summary>
		/// <param name="reader">The input reader</param>
		public void AddInputRaw(TextReader reader)
		{
			inputs.Add(new KeyValuePair<string, TextReader>(GetRawInputID(), reader));
		}
		/// <summary>
		/// Adds a new named data reader as input
		/// </summary>
		/// <param name="name">The input's name</param>
		/// <param name="reader">The input reader</param>
		public void AddInputRaw(string name, TextReader reader)
		{
			inputs.Add(new KeyValuePair<string, TextReader>(name, reader));
		}
		/// <summary>
		/// Adds the specified pre-parsed grammar to the inputs
		/// </summary>
		/// <param name="node">The parse tree of a grammar</param>
		/// <param name="input">The input that contains the grammar</param>
		public void AddInput(ASTNode node, Text input)
		{
			preReadInputs.Add(new KeyValuePair<ASTNode, Text>(node, input));
		}

		/// <summary>
		/// Parses the inputs
		/// </summary>
		/// <returns>The loaded grammars</returns>
		public List<Grammars.Grammar> Load()
		{
			List<Grammars.Grammar> result = new List<Grammars.Grammar>();
			foreach (KeyValuePair<string, TextReader> pair in inputs)
				if (!LoadInput(pair.Key, pair.Value))
					return result;
			foreach (KeyValuePair<ASTNode, Text> pair in preReadInputs)
				if (!LoadInput(pair.Key, pair.Value))
					return result;
			if (!SolveDependencies())
				return result;
			foreach (Grammars.Loader inner in inners.Values)
				result.Add(inner.Grammar);
			return result;
		}

		/// <summary>
		/// Loads the specified input
		/// </summary>
		/// <param name="node">The parse tree of a grammar</param>
		/// <param name="input">The input that contains the grammar</param>
		/// <returns><c>true</c> if the operation succeed</returns>
		private bool LoadInput(ASTNode node, Text input)
		{
			Grammars.Loader loader = new Grammars.Loader(node.Children[0].Symbol.Value, input, node, reporter);
			inners.Add(loader.Grammar.Name, loader);
			return true;
		}

		/// <summary>
		/// Parses the input with the given identifier
		/// </summary>
		/// <param name="name">The input's name</param>
		/// <param name="reader">The input's reader</param>
		/// <returns><c>true</c> if the operation succeed</returns>
		private bool LoadInput(string name, TextReader reader)
		{
			reporter.Info("Reading input " + name + " ...");
			bool hasErrors = false;
			Input.HimeGrammarLexer lexer = new Input.HimeGrammarLexer(reader);
			Input.HimeGrammarParser parser = new Input.HimeGrammarParser(lexer);
			ParseResult result = null;
			try
			{
				result = parser.Parse();
			}
			catch (System.Exception ex)
			{
				reporter.Error("Fatal error in " + name);
				reporter.Error(ex);
				hasErrors = true;
			}
			foreach (ParseError error in result.Errors)
			{
				reporter.Error(name + error.Message, result.Input, error.Position);
				hasErrors = true;
			}
			if (result.IsSuccess)
			{
				foreach (ASTNode gnode in result.Root.Children)
				{
					Grammars.Loader loader = new Grammars.Loader(name, result.Input, gnode, reporter);
					inners.Add(loader.Grammar.Name, loader);
				}
			}
			reader.Close();
			return !hasErrors;
		}

		/// <summary>
		/// Solves the dependencies between the inputs and interprets the parsed inputs
		/// </summary>
		/// <returns><c>true</c> if all dependencies were solved</returns>
		private bool SolveDependencies()
		{
			int unsolved = 1;
			while (unsolved != 0)
			{
				unsolved = 0;
				int solved = 0;
				foreach (Grammars.Loader loader in inners.Values)
				{
					if (loader.IsSolved)
						continue;
					loader.Load(inners);
					if (loader.IsSolved)
						solved++;
					else
						unsolved++;
				}
				if (unsolved != 0 && solved == 0)
				{
					foreach (string name in inners.Keys)
					{
						Grammars.Loader loader = inners[name];
						foreach (string dep in loader.Dependencies)
							reporter.Error(string.Format("Failed to solve dependency of {0} on {1}", name, dep));
					}
					return false;
				}
			}
			return true;
		}
	}
}