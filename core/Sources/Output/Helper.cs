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

namespace Hime.CentralDogma.Output
{
	/// <summary>
	/// Helpers for the emitters
	/// </summary>
	public class Helper
	{
		/// <summary>
		/// Sanitizes the name of a symbol for output in code
		/// </summary>
		/// <param name="symbol">A symbol</param>
		/// <returns>The sanitized name of the symbol</returns>
		public static string SanitizeName(Grammars.Symbol symbol)
		{
			string original = symbol.Name;
			System.Text.StringBuilder builder = new System.Text.StringBuilder();
			foreach (char c in original)
			{
				if (c == '_' || (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z'))
					builder.Append(c);
				else if (c >= '0' && c <= '9')
					builder.Append(c);
				else if (c == '<' || c == '>' || c == '-')
					builder.Append('_');
			}
			original = builder.ToString();
			if (keywords.Contains(original))
				return "@" + original;
			return original;
		}

		private static readonly ROList<string> keywords = new ROList<string>(new List<string>(new string[] { "abstract",
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
	}
}