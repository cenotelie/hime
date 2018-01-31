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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Hime.Redist;
using Hime.Redist.Lexer;
using Hime.Redist.Parsers;
using Hime.Redist.Utils;

namespace Hime.SDK.Reflection
{
	/// <summary>
	/// Helper class for handling generated assembly containing lexers and parsers
	/// </summary>
	public class AssemblyReflection
	{
		/// <summary>
		/// The inspected assembly
		/// </summary>
		private readonly Assembly assembly;
		/// <summary>
		/// The lexers in the current assembly
		/// </summary>
		private readonly List<Type> lexerTypes;
		/// <summary>
		/// The parsers in the current assembly
		/// </summary>
		private readonly List<Type> parserTypes;

		/// <summary>
		/// Gets the lexers in this assembly
		/// </summary>
		public ROList<Type> Lexers { get { return new ROList<Type>(lexerTypes); } }

		/// <summary>
		/// Gets the parsers in this assembly
		/// </summary>
		public ROList<Type> Parsers { get { return new ROList<Type>(parserTypes); } }

		/// <summary>
		/// Initializes this inspector for the given assembly
		/// </summary>
		/// <param name="assembly">The assembly to inspect</param>
		public AssemblyReflection(Assembly assembly)
		{
			this.assembly = assembly;
			lexerTypes = new List<Type>();
			parserTypes = new List<Type>();
			Type baseLexer = typeof(BaseLexer);
			Type baseParser = typeof(BaseLRParser);
			foreach (Type t in assembly.GetTypes())
			{
				if (t.IsClass)
				{
					if (baseLexer.IsAssignableFrom(t))
						lexerTypes.Add(t);
					else if (baseParser.IsAssignableFrom(t))
						parserTypes.Add(t);
				}
			}
		}

		/// <summary>
		/// Initializes this inspector for the assembly in the given file
		/// </summary>
		/// <param name="file">The file containing the assembly to inspect</param>
		public AssemblyReflection(string file) : this(Assembly.LoadFile(Path.GetFullPath(file)))
		{
		}

		/// <summary>
		/// Gets the type in this assembly with the specified fully-qualified name
		/// </summary>
		/// <param name="name">The fully qualified name of a type</param>
		/// <returns>The type with the specified name</returns>
		public Type GetType(string name)
		{
			return assembly.GetType(name);
		}

		/// <summary>
		/// Gets an instance of the default lexer
		/// </summary>
		/// <param name="input">The input for the lexer</param>
		/// <returns>The lexe</returns>
		public BaseLexer getLexer<T>(T input)
		{
			return lexerTypes.Count == 0 ? null : getLexer(lexerTypes[0], input);
		}

		/// <summary>
		/// Gets an instance of the specified lexer
		/// </summary>
		/// <param name="lexerType">The lexer's type</param>
		/// <param name="input">The input for the lexer</param>
		/// <returns>The lexe</returns>
		public BaseLexer getLexer<T>(Type lexerType, T input)
		{
			ConstructorInfo lexerCtor = lexerType.GetConstructor(new [] { typeof(T) });
			object lexer = lexerCtor.Invoke(new object[] { input });
			return (BaseLexer)lexer;
		}

		/// <summary>
		/// Gets an instance of the default parser
		/// </summary>
		/// <param name="input">The input for the associated lexer</param>
		/// <returns>The parser</returns>
		public BaseLRParser GetParser<T>(T input)
		{
			return parserTypes.Count == 0 ? null : GetParser(parserTypes[0], input, null);
		}

		/// <summary>
		/// Gets an instance of the default parser
		/// </summary>
		/// <param name="input">The input for the associated lexer</param>
		/// <param name="actions">The semantic actions for the parser</param>
		/// <returns>The parser</returns>
		public BaseLRParser GetParser<T>(T input, Dictionary<string, SemanticAction> actions)
		{
			return parserTypes.Count == 0 ? null : GetParser(parserTypes[0], input, actions);
		}

		/// <summary>
		/// Gets an instance of the specified parser
		/// </summary>
		/// <param name="name">The parser's fully qualified name</param>
		/// <param name="input">The input for the associated lexer</param>
		/// <returns>The parser</returns>
		public BaseLRParser GetParser<T>(string name, T input)
		{
			return GetParser(assembly.GetType(name), input, null);
		}

		/// <summary>
		/// Gets an instance of the specified parser
		/// </summary>
		/// <param name="name">The parser's fully qualified name</param>
		/// <param name="input">The input for the associated lexer</param>
		/// <param name="actions">The semantic actions for the parser</param>
		/// <returns>The parser</returns>
		public BaseLRParser GetParser<T>(string name, T input, Dictionary<string, SemanticAction> actions)
		{
			return GetParser(assembly.GetType(name), input, actions);
		}

		/// <summary>
		/// Gets an instance of the specified parser
		/// </summary>
		/// <param name="parserType">The parser's type</param>
		/// <param name="input">The input for the associated lexer</param>
		/// <returns>The parser</returns>
		public BaseLRParser GetParser<T>(Type parserType, T input)
		{
			return GetParser(parserType, input, null);
		}

		/// <summary>
		/// Gets an instance of the specified parser
		/// </summary>
		/// <param name="parserType">The parser's type</param>
		/// <param name="input">The input for the associated lexer</param>
		/// <param name="actions">The semantic actions for the parser</param>
		/// <returns>The parser</returns>
		public BaseLRParser GetParser<T>(Type parserType, T input, Dictionary<string, SemanticAction> actions)
		{
			ConstructorInfo[] ctors = parserType.GetConstructors();
			ConstructorInfo parserCtor = null;
			Type lexerType = null;
			ConstructorInfo lexerCtor = null;
			for (int i = 0; i != ctors.Length; i++)
			{
				ParameterInfo[] parameters = ctors[i].GetParameters();
				if (actions == null && parameters.Length == 1)
				{
					lexerType = parameters[0].ParameterType;
					parserCtor = ctors[i];
					break;
				}
				if (actions != null && parameters.Length == 2 && parameters[1].ParameterType == (typeof(Dictionary<string, SemanticAction>)))
				{
					lexerType = parameters[0].ParameterType;
					parserCtor = ctors[i];
					break;
				}
			}
			lexerCtor = lexerType.GetConstructor(new [] { typeof(T) });
			object lexer = lexerCtor.Invoke(new object[] { input });
			object parser = actions == null ? parserCtor.Invoke(new [] { lexer }) : parserCtor.Invoke(new object[] { lexer, actions });
			return (BaseLRParser)parser;
		}
	}
}