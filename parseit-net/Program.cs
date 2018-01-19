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
using System.IO;
using System.Reflection;
using System.Text;
using Hime.Redist;
using Hime.Redist.Parsers;

namespace Hime.Parseit
{
	/// <summary>
	/// Entry class for the parseit program
	/// </summary>
	public sealed class Program
	{
		/// <summary>
		/// Executes the parseit program
		/// </summary>
		/// <param name="args">The command line arguments</param>
		/// <returns>The error code, or 0 if none</returns>
		public static int Main(string[] args)
		{
			// If no argument is given, print the help screen and return OK
			if (args == null || args.Length != 2)
			{
				PrintHelp();
				return 0;
			}

			TextReader input = new StreamReader(Console.OpenStandardInput(), Encoding.UTF8, false);
			Assembly assembly = Assembly.LoadFile(Path.GetFullPath(args[0]));
			Type parserType = assembly.GetType(args[1]);
			ConstructorInfo parserCtor = parserType.GetConstructors()[0];
			ParameterInfo[] parameters = parserCtor.GetParameters();
			Type lexerType = parameters[0].ParameterType;
			ConstructorInfo lexerCtor = lexerType.GetConstructor(new [] { typeof(TextReader) });
			object lexer = lexerCtor.Invoke(new object[] { input });
			BaseLRParser parser = (BaseLRParser) parserCtor.Invoke(new [] { lexer });

			ParseResult result = parser.Parse();
			StringBuilder builder = new StringBuilder();
			Serialize(builder, result);
			Console.WriteLine(builder.ToString());
			return 0;
		}

		/// <summary>
		/// Prints the help screen for this program
		/// </summary>
		private static void PrintHelp()
		{
			Console.WriteLine("parseit " + Assembly.GetExecutingAssembly().GetName().Version + " (LGPL 3)");
			Console.WriteLine("Command line to parse a piece of input using a packaged parser");
			Console.WriteLine();
			Console.WriteLine("usage: parseit <parserAssembly> <parserName>");
		}

		/// <summary>
		/// Serializes a parse result
		/// </summary>
		private static void Serialize(StringBuilder builder, ParseResult result) {
			builder.Append("{\"errors\": [");
			for (int i = 0; i != result.Errors.Count; i++)
			{
				if (i != 0)
					builder.Append(", ");
				Serialize(builder, result.Errors[i]);
			}
			builder.Append("]");
			if (result.IsSuccess)
			{
				builder.Append(", \"root\": ");
				Serialize(builder, result.Root);
			}
			builder.Append("}");
		}

		/// <summary>
		/// Serializes a parse error
		/// </summary>
		private static void Serialize(StringBuilder builder, ParseError error) {
			builder.Append("{\"type\": \"");
			builder.Append(error.Type.ToString());
			builder.Append("\", \"position\": ");
			Serialize(builder, error.Position);
			builder.Append(", \"length\": ");
			builder.Append(error.Length.ToString());
			builder.Append(", \"message\": \"");
			builder.Append(Escape(error.Message));
			builder.Append("\"}");
		}

		/// <summary>
		/// Serializes an AST node
		/// </summary>
		private static void Serialize(StringBuilder builder, ASTNode node) {
			builder.Append("{\"symbol\": ");
			Serialize(builder, node.Symbol);
			string value = node.Value;
			if (value != null) {
				builder.Append(", \"value\": \"");
				builder.Append(Escape(value));
				builder.Append("\", \"position\": ");
				Serialize(builder, node.Position);
				builder.Append(", \"span\": ");
				Serialize(builder, node.Span);
			}
			builder.Append(", \"children\": [");
			for (int i = 0; i != node.Children.Count; i++)
			{
				if (i != 0)
					builder.Append(", ");
				Serialize(builder, node.Children[i]);
			}
			builder.Append("]}");
		}

		/// <summary>
		/// Serializes a symbol
		/// </summary>
		private static void Serialize(StringBuilder builder, Symbol symbol) {
			builder.Append("{\"id\": ");
			builder.Append(symbol.ID.ToString());
			builder.Append(", \"name\": \"");
			builder.Append(Escape(symbol.Name));
			builder.Append("\"}");
		}

		/// <summary>
		/// Serializes a text position
		/// </summary>
		private static void Serialize(StringBuilder builder, TextPosition position) {
			builder.Append("{\"line\": ");
			builder.Append(position.Line.ToString());
			builder.Append(", \"column\": ");
			builder.Append(position.Column.ToString());
			builder.Append("}");
		}

		/// <summary>
		/// Serializes a text span
		/// </summary>
		private static void Serialize(StringBuilder builder, TextSpan span) {
			builder.Append("{\"index\": ");
			builder.Append(span.Index.ToString());
			builder.Append(", \"length\": ");
			builder.Append(span.Length.ToString());
			builder.Append("}");
		}

		/// <summary>
		/// Escapes the input string for serialization in JSON
		/// </summary>
		private static string Escape(string value)
		{
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i != value.Length; i++)
			{
				char c = value[i];
				if (c == '"')
					builder.Append("\\\"");
				else if (c == '\\')
					builder.Append("\\\\");
				else if (c == '\u0000')
					builder.Append("\\0");
				else if (c == '\u0007')
					builder.Append("\\a");
				else if (c == '\t')
					builder.Append("\\t");
				else if (c == '\r')
					builder.Append("\\r");
				else if (c == '\n')
					builder.Append("\\n");
				else if (c == '\b')
					builder.Append("\\b");
				else if (c == '\f')
					builder.Append("\\f");
				else
					builder.Append(c);
			}
			return builder.ToString();
		}
	}
}
