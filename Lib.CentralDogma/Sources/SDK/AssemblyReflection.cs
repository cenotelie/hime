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

namespace Hime.CentralDogma.SDK
{
	/// <summary>
	/// Helper class for handling generated assembly containing lexers and parsers
	/// </summary>
	public class AssemblyReflection
	{
		/// <summary>
		/// The inspected assembly
		/// </summary>
		protected Assembly assembly;

		/// <summary>
		/// Initializes this inspector for the given assembly
		/// </summary>
		/// <param name="assembly">The assembly to inspect</param>
		public AssemblyReflection(Assembly assembly)
		{
			this.assembly = assembly;
		}

		/// <summary>
		/// Initializes this inspector for the assembly in the given file
		/// </summary>
		/// <param name="file">The file containing the assembly to inspect</param>
		public AssemblyReflection(string file)
		{
			this.assembly = Assembly.LoadFile(file);
		}

		/// <summary>
		/// Gets the lexer inside this assembly that has the given (fully-qualified) name
		/// </summary>
		/// <param name="name">A fully-qualified name</param>
		/// <returns>The lexer type</returns>
		public Type GetLexerType(string name)
		{
			return assembly.GetType(name);
		}

		/// <summary>
		/// Gets all the lexers types inside this assembly
		/// </summary>
		/// <returns>The lexers</returns>
		public ICollection<Type> GetLexersType()
		{
			Type baseLexer = typeof(Hime.Redist.Lexer.ILexer);
			List<Type> result = new List<Type>();
			foreach (Type t in assembly.GetTypes())
				if (t.IsClass && baseLexer.IsAssignableFrom(t))
					result.Add(t);
			return result;
		}

		/// <summary>
		/// Gets the parser inside this assembly that has the given (fully-qualified) name
		/// </summary>
		/// <param name="name">A fully-qualified name</param>
		/// <returns>The parser type</returns>
		public Type GetParserType(string name)
		{
			return assembly.GetType(name);
		}

		/// <summary>
		/// Gets all the parsers types inside this assembly
		/// </summary>
		/// <returns>The parsers</returns>
		public List<Type> GetParsersType()
		{
			Type baseParser = typeof(Hime.Redist.Parsers.BaseLRParser);
			List<Type> result = new List<Type>();
			foreach (Type t in assembly.GetTypes())
				if (t.IsClass && baseParser.IsAssignableFrom(t))
					result.Add(t);
			return result;
		}

		/// <summary>
		/// Gets an instance of the parser with the given (fully-qualified) name
		/// </summary>
		/// <param name="name">The fully qualified name of the parser's type</param>
		/// <param name="input">The input for the associated lexer</param>
		/// <returns>The parser</returns>
		public Hime.Redist.Parsers.IParser GetParser(string name, TextReader input)
		{
			Type parserType = GetParserType(name);
			ConstructorInfo[] ctors = parserType.GetConstructors();
			ConstructorInfo parserCtor = null;
			Type lexerType = null;
			ConstructorInfo lexerCtor = null;
			for (int i=0; i!=ctors.Length; i++)
			{
				ParameterInfo[] parameters = ctors[i].GetParameters();
				if (parameters.Length == 1)
				{
					parserCtor = ctors[i];
					lexerType = parameters[0].ParameterType;
					break;
				}
			}
			lexerCtor = lexerType.GetConstructor(new Type[] { typeof(TextReader) });
			object lexer = lexerCtor.Invoke(new object[] { input });
			object parser = parserCtor.Invoke(new object[] { lexer });
			return (Hime.Redist.Parsers.IParser)parser;
		}

		/// <summary>
		/// Gets an instance of the default parser in this assembly
		/// </summary>
		/// <param name="input">The input for the associated lexer</param>
		/// <returns>The parser</returns>
		public Hime.Redist.Parsers.IParser GetDefaultParser(TextReader input)
		{
			List<Type> parsers = GetParsersType();
			if (parsers.Count == 0)
				return null;
			return GetParser(parsers[0].FullName, input);
		}
	}
}