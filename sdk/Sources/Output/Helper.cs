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
using System.Text;
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
		public static string GetCSConstantName(string name)
		{
			string result = RemoveSpecials(name);
			if (keywordsCS.Contains(result))
				return CS_KEYWORD_PREFIXING + result;
			return result;
		}

		/// <summary>
		/// Sanitizes the name of a symbol into the name of a function in Java
		/// </summary>
		/// <param name="name">A name to sanitize</param>
		/// <returns>The sanitized name of the symbol</returns>
		public static string GetCSFunctionName(string name)
		{
			return GetNamespacePartForCS(name);
		}

		/// <summary>
		/// Sanitizes the name of a symbol for output in Java code
		/// </summary>
		/// <param name="name">A name to sanitize</param>
		/// <returns>The sanitized name of the symbol</returns>
		public static string GetJavaConstantName(string name)
		{
			string result = RemoveSpecials(name);
			if (keywordsJava.Contains(result))
				return JAVA_KEYWORD_PREFIXING + result;
			return result;
		}

		/// <summary>
		/// Sanitizes the name of a symbol into the name of a function in Java
		/// </summary>
		/// <param name="name">A name to sanitize</param>
		/// <returns>The sanitized name of the symbol</returns>
		public static string GetJavaFunctionName(string name)
		{
			return GetNamespacePartForJava(name);
		}

		/// <summary>
		/// Sanitizes the name of a symbol into the name of a constant in Rust
		/// </summary>
		/// <param name="name">A name to sanitize</param>
		/// <returns>The sanitized name of the symbol</returns>
		public static string GetRustConstantName(string name)
		{
			return RemoveSpecials(name.ToUpperInvariant());
		}

		/// <summary>
		/// Sanitizes the name of a symbol into the name of a function in Rust
		/// </summary>
		/// <param name="name">A name to sanitize</param>
		/// <returns>The sanitized name of the symbol</returns>
		public static string GetRustFunctionName(string name)
		{
			return GetNamespacePartForRust(name);
		}

		/// <summary>
		/// Gets the C# compatible name for the specified namespace
		/// </summary>
		/// <param name="input">The original namespace</param>
		/// <returns>The resulting namespace</returns>
		public static string GetNamespaceForCS(string input)
		{
			if (input.Contains("::"))
				input = input.Replace("::", ".");
			string[] parts = input.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i != parts.Length; i++)
			{
				if (i != 0)
					builder.Append(".");
				builder.Append(GetNamespacePartForCS(parts[i]));
			}
			return builder.ToString();
		}

		/// <summary>
		/// Gets the C# compatible name for a part of a namespace
		/// </summary>
		/// <param name="input">The original name</param>
		/// <returns>The resulting name</returns>
		public static string GetNamespacePartForCS(string input)
		{
			input = RemoveSpecials(input);
			bool forceUpper = true;
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i != input.Length; i++)
			{
				char c = input[i];
				if (i == 0)
				{
					if (c == '_')
					{
						builder.Append(c);
					}
					else if (c >= 'A' && c <= 'Z')
					{
						builder.Append(c);
						forceUpper = false;
					}
					else if (c >= 'a' && c <= 'z')
					{
						builder.Append(char.ToUpperInvariant(c));
						forceUpper = false;
					}
					else if (c >= '0' && c <= '9')
					{
						builder.Append("_");
						builder.Append(c);
					}
				}
				else
				{
					if (c >= 'A' && c <= 'Z')
					{
						builder.Append(c);
						forceUpper = false;
					}
					else if (c >= 'a' && c <= 'z')
					{
						builder.Append(forceUpper ? char.ToUpperInvariant(c) : c);
						forceUpper = false;
					}
					else if (c >= '0' && c <= '9')
					{
						builder.Append(c);
						forceUpper = true;
					}
					else
					{
						forceUpper = true;
					}
				}
			}
			return builder.ToString();
		}

		/// <summary>
		/// Gets the Java compatible name for the specified namespace
		/// </summary>
		/// <param name="input">The original namespace</param>
		/// <returns>The resulting namespace</returns>
		public static string GetNamespaceForJava(string input)
		{
			if (input.Contains("::"))
				input = input.Replace("::", ".");
			string[] parts = input.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i != parts.Length; i++)
			{
				if (i != 0)
					builder.Append(".");
				builder.Append(GetNamespacePartForJava(parts[i]));
			}
			return builder.ToString();
		}

		/// <summary>
		/// Gets the Java compatible name for a part of a namespace
		/// </summary>
		/// <param name="input">The original name</param>
		/// <returns>The resulting name</returns>
		public static string GetNamespacePartForJava(string input)
		{
			input = RemoveSpecials(input);
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i != input.Length; i++)
			{
				char c = input[i];
				if (i == 0)
				{
					if (c >= 'A' && c <= 'Z')
						builder.Append(char.ToLowerInvariant(c));
					else if (c >= 'a' && c <= 'z')
						builder.Append(c);
					else if (c >= '0' && c <= '9')
					{
						builder.Append("_");
						builder.Append(c);
					}
					else
						builder.Append('_');
				}
				else
				{
					if (c >= 'A' && c <= 'Z')
						builder.Append(char.ToLowerInvariant(c));
					else if (c >= 'a' && c <= 'z')
						builder.Append(c);
					else if (c >= '0' && c <= '9')
						builder.Append(c);
					else
						builder.Append('_');
				}
			}
			return builder.ToString();
		}

		/// <summary>
		/// Gets the Rust compatible name for the specified namespace
		/// </summary>
		/// <param name="input">The original namespace</param>
		/// <returns>The resulting namespace</returns>
		public static string GetNamespaceForRust(string input)
		{
			if (input.Contains("."))
				input = input.Replace(".", "::");
			string[] parts = input.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i != parts.Length; i++)
			{
				if (i != 0)
					builder.Append("::");
				builder.Append(GetNamespacePartForRust(parts[i]));
			}
			return builder.ToString();
		}

		/// <summary>
		/// Gets the Rust compatible name for a part of a namespace
		/// </summary>
		/// <param name="input">The original name</param>
		/// <returns>The resulting name</returns>
		public static string GetNamespacePartForRust(string input)
		{
			input = RemoveSpecials(input);
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i != input.Length; i++)
			{
				char c = input[i];
				if (i == 0)
				{
					if (c >= 'A' && c <= 'Z')
						builder.Append(char.ToLowerInvariant(c));
					else if (c >= 'a' && c <= 'z')
						builder.Append(c);
					else if (c >= '0' && c <= '9')
					{
						builder.Append("_");
						builder.Append(c);
					}
					else
						builder.Append('_');
				}
				else
				{
					if (c >= 'A' && c <= 'Z')
						builder.Append(char.ToLowerInvariant(c));
					else if (c >= 'a' && c <= 'z')
						builder.Append(c);
					else if (c >= '0' && c <= '9')
						builder.Append(c);
					else
						builder.Append('_');
				}
			}
			return builder.ToString();
		}

		/// <summary>
		/// Removes the specials characters that can arise a the specified symbol name
		/// </summary>
		/// <param name="name">A symbol name</param>
		/// <returns>The cleaned-up name</returns>
		private static string RemoveSpecials(string name)
		{
			StringBuilder builder = new StringBuilder();
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
		private static readonly ROList<string> keywordsCS = new ROList<string>(new List<string>(new[] { "abstract",
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
		private static readonly ROList<string> keywordsJava = new ROList<string>(new List<string>(new[] { "abstract",
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