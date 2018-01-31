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

namespace Hime.SDK.Output
{
	/// <summary>
	/// Helpers for the emitters
	/// </summary>
	public static class Helper
	{
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
				builder.Append(ToUpperCamelCase(parts[i]));
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
				builder.Append(ToSnakeCase(parts[i]));
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
				builder.Append(ToSnakeCase(parts[i]));
			}
			return builder.ToString();
		}

		/// <summary>
		/// Converts a name to upper camel case
		/// </summary>
		/// <param name="name">The original name</param>
		/// <returns>The corresponding snake case name</returns>
		public static string ToUpperCamelCase(string name)
		{
			if (name == null || name.Length == 0)
				return name;
			StringBuilder builder = new StringBuilder();
			bool newWord = false;
			char c = name[0];
			if (c >= 'A' && c <= 'Z')
				builder.Append(c);
			else if (c >= 'a' && c <= 'z')
				builder.Append(char.ToUpperInvariant(c));
			else if (c >= '0' && c <= '9')
			{
				builder.Append("_");
				builder.Append(c);
				newWord = true;
			}
			else
			{
				newWord = true;
			}

			for (int i = 1; i != name.Length; i++)
			{
				c = name[i];
				if (c >= 'A' && c <= 'Z')
				{
					builder.Append(c);
					newWord = false;
				}
				else if (c >= 'a' && c <= 'z')
				{
					builder.Append(newWord ? char.ToUpperInvariant(c) : c);
					newWord = false;
				}
				else if (c >= '0' && c <= '9')
				{
					builder.Append(c);
					newWord = true;
				}
				else
				{
					newWord = true;
				}
			}
			return builder.ToString();
		}

		/// <summary>
		/// Converts a name to lower camel case
		/// </summary>
		/// <param name="name">The original name</param>
		/// <returns>The corresponding snake case name</returns>
		public static string ToLowerCamelCase(string name)
		{
			if (name == null || name.Length == 0)
				return name;
			StringBuilder builder = new StringBuilder();
			bool newWord = false;
			char c = name[0];
			if (c >= 'A' && c <= 'Z')
				builder.Append(char.ToLowerInvariant(c));
			else if (c >= 'a' && c <= 'z')
				builder.Append(c);
			else if (c >= '0' && c <= '9')
			{
				builder.Append("_");
				builder.Append(c);
				newWord = true;
			}

			for (int i = 1; i != name.Length; i++)
			{
				c = name[i];
				if (c >= 'A' && c <= 'Z')
				{
					builder.Append(builder.Length == 0 ? char.ToLowerInvariant(c) : c);
					newWord = false;
				}
				else if (c >= 'a' && c <= 'z')
				{
					builder.Append(newWord ? char.ToUpperInvariant(c) : c);
					newWord = false;
				}
				else if (c >= '0' && c <= '9')
				{
					builder.Append(c);
					newWord = true;
				}
				else
				{
					newWord = true;
				}
			}
			return builder.ToString();
		}

		/// <summary>
		/// Converts a name to upper case
		/// </summary>
		/// <param name="name">The original name</param>
		/// <returns>The corresponding snake case name</returns>
		public static string ToUpperCase(string name)
		{
			if (name == null || name.Length == 0)
				return name;
			StringBuilder builder = new StringBuilder();
			char c = name[0];
			if (c >= 'A' && c <= 'Z')
				builder.Append(c);
			else if (c >= 'a' && c <= 'z')
				builder.Append(char.ToUpperInvariant(c));
			else if (c >= '0' && c <= '9')
			{
				builder.Append("_");
				builder.Append(c);
			}
			else
				builder.Append('_');

			for (int i = 1; i != name.Length; i++)
			{
				c = name[i];
				if (c >= 'A' && c <= 'Z')
				{
					if ((name[i - 1] >= 'a' && name[i - 1] <= 'z') || (name[i - 1] >= '0' && name[i - 1] <= '9'))
						// preceded by a lower-case character or a number, this is a new word
						builder.Append("_");
					builder.Append(c);
				}
				else if (c >= 'a' && c <= 'z')
					builder.Append(char.ToUpperInvariant(c));
				else if (c >= '0' && c <= '9')
					builder.Append(c);
				else
					builder.Append('_');
			}
			return builder.ToString();
		}

		/// <summary>
		/// Converts a name to snake case
		/// </summary>
		/// <param name="name">The original name</param>
		/// <returns>The corresponding snake case name</returns>
		public static string ToSnakeCase(string name)
		{
			if (name == null || name.Length == 0)
				return name;
			StringBuilder builder = new StringBuilder();
			char c = name[0];
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

			for (int i = 1; i != name.Length; i++)
			{
				c = name[i];
				if (c >= 'A' && c <= 'Z')
				{
					if ((name[i - 1] >= 'a' && name[i - 1] <= 'z') || (name[i - 1] >= '0' && name[i - 1] <= '9'))
						// preceded by a lower-case character or a number, this is a new word
						builder.Append("_");
					builder.Append(char.ToLowerInvariant(c));
				}
				else if (c >= 'a' && c <= 'z')
					builder.Append(c);
				else if (c >= '0' && c <= '9')
					builder.Append(c);
				else
					builder.Append('_');
			}
			return builder.ToString();
		}
	}
}