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

using System.Collections.Generic;
using Hime.Redist.Utils;

namespace Hime.SDK.Output
{
	/// <summary>
	/// Helpers for the emitters
	/// </summary>
	public static class Helper
	{
		/// <summary>
		/// The prefixing string for reserved C# keywords
		/// </summary>
		private const string CS_KEYWORD_PREFIXING = "_";

		/// <summary>
		/// The prefixing string for reserved Java keywords
		/// </summary>
		private const string JAVA_KEYWORD_PREFIXING = "_";

		/// <summary>
		/// The suffix for virtual symbols that have the same name as a variable
		/// </summary>
		public const string VIRTUAL_SUFFIX = "_virtual";

		/// <summary>
		/// Sanitizes the name of a symbol for output in C# code
		/// </summary>
		/// <param name="name">A name to sanitize</param>
		/// <returns>The sanitized name of the symbol</returns>
		public static string SanitizeNameCS(string name)
		{
			string result = RemoveSpecials(name);
			if (keywordsCS.Contains(result))
				return CS_KEYWORD_PREFIXING + result;
			return result;
		}

		/// <summary>
		/// Sanitizes the name of a symbol for output in Java code
		/// </summary>
		/// <param name="name">A name to sanitize</param>
		/// <returns>The sanitized name of the symbol</returns>
		public static string SanitizeNameJava(string name)
		{
			string result = RemoveSpecials(name);
			if (keywordsJava.Contains(result))
				return JAVA_KEYWORD_PREFIXING + result;
			return result;
		}

		/// <summary>
		/// Removes the specials characters that can arise a the specified symbol name
		/// </summary>
		/// <param name="name">A symbol name</param>
		/// <returns>The cleaned-up name</returns>
		private static string RemoveSpecials(string name)
		{
			System.Text.StringBuilder builder = new System.Text.StringBuilder();
			foreach (char c in name)
			{
				if (c == '_' || (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
					builder.Append(c);
				else if (c >= '0' && c <= '9')
					builder.Append(c);
				else if (c == '<' || c == '>' || c == '-')
					builder.Append('_');
				// drop anything else
			}
			return builder.ToString();
		}

		/// <summary>
		/// The reserved C# keywords
		/// </summary>
		private static readonly ROList<string> keywordsCS = new ROList<string>(new List<string>(new [] { "abstract",
			"as",
			"base",
			"bool",
			"break",
			"byte",
			"case",
			"catch",
			"char",
			"checked",
			"class",
			"const",
			"continue",
			"decimal",
			"default",
			"delegate",
			"do",
			"double",
			"else",
			"enum",
			"event",
			"explicit",
			"extern",
			"false",
			"finally",
			"fixed",
			"float",
			"for",
			"foreach",
			"goto",
			"if",
			"implicit",
			"in",
			"int",
			"interface",
			"internal",
			"is",
			"lock",
			"long",
			"namespace",
			"new",
			"null",
			"object",
			"operator",
			"out",
			"override",
			"params",
			"private",
			"protected",
			"public",
			"readonly",
			"ref",
			"return",
			"sbyte",
			"sealed",
			"short",
			"sizeof",
			"stackalloc",
			"static",
			"string",
			"struct",
			"switch",
			"this",
			"throw",
			"true",
			"try",
			"typeof",
			"uint",
			"ulong",
			"unchecked",
			"unsafe",
			"ushort",
			"using",
			"virtual",
			"void",
			"volatile",
			"while"
		}));

		/// <summary>
		/// The reserved Java keywords
		/// </summary>
		private static readonly ROList<string> keywordsJava = new ROList<string>(new List<string>(new [] { "abstract",
			"continue",
			"for",
			"new",
			"switch",
			"assert",
			"default",
			"if",
			"package",
			"synchronized",
			"boolean",
			"do",
			"goto",
			"private",
			"this",
			"break",
			"double",
			"implements",
			"protected",
			"throw",
			"byte",
			"else",
			"import",
			"public",
			"throws",
			"case",
			"enum",
			"instanceof",
			"return",
			"transient",
			"catch",
			"extends",
			"int",
			"short",
			"try",
			"char",
			"final",
			"interface",
			"static",
			"void",
			"class",
			"finally",
			"long",
			"strictfp",
			"volatile",
			"const",
			"float",
			"native",
			"super",
			"while"
		}));
	}
}