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
using Hime.Redist.Utils;

namespace Hime.SDK.Output
{
	/// <summary>
	/// Helpers for the emitters
	/// </summary>
	public class Helper
	{
		/// <summary>
		/// Sanitizes the name of a symbol for output in C# code
		/// </summary>
		/// <param name="symbol">A symbol</param>
		/// <returns>The sanitized name of the symbol</returns>
		public static string SanitizeNameCS(Grammars.Symbol symbol)
		{
			string name = RemoveSpecials(symbol.Name);
			if (keywordsCS.Contains(name))
				return "_" + name;
			return name;
		}

		/// <summary>
		/// Sanitizes the name of a symbol for output in Java code
		/// </summary>
		/// <param name="symbol">A symbol</param>
		/// <returns>The sanitized name of the symbol</returns>
		public static string SanitizeNameJava(Grammars.Symbol symbol)
		{
			string name = RemoveSpecials(symbol.Name);
			if (keywordsJava.Contains(name))
				return "_" + name;
			return name;
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
		private static readonly ROList<string> keywordsCS = new ROList<string>(new List<string>(new string[] { "abstract",
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
			"while" }));

		/// <summary>
		/// The reserved Java keywords
		/// </summary>
		private static readonly ROList<string> keywordsJava = new ROList<string>(new List<string>(new string[] { "abstract",
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
			"while"}));
	}
}