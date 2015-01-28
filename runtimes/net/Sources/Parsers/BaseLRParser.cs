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
using Hime.Redist.Utils;

namespace Hime.Redist.Parsers
{
	/// <summary>
	/// Represents a base LR parser
	/// </summary>
	public abstract class BaseLRParser : IParser
	{
		/// <summary>
		/// Maximum number of errors
		/// </summary>
		protected const int MAX_ERROR_COUNT = 100;
		/// <summary>
		/// The default value of the recover mode
		/// </summary>
		protected const bool DEFAULT_MODE_RECOVER = true;
		/// <summary>
		/// The default value of the debug mode
		/// </summary>
		protected const bool DEFAULT_MODE_DEBUG = false;


		/// <summary>
		/// Determines whether the parser will try to recover from errors
		/// </summary>
		protected bool modeRecover;
		/// <summary>
		/// Value indicating whether this parser is in debug mode
		/// </summary>
		protected bool modeDebug;
		/// <summary>
		/// Parser's variable symbols
		/// </summary>
		protected ROList<Symbol> symVariables;
		/// <summary>
		/// Parser's virtual symbols
		/// </summary>
		protected ROList<Symbol> symVirtuals;
		/// <summary>
		/// Parser's action symbols
		/// </summary>
		protected ROList<SemanticAction> symActions;
		/// <summary>
		/// List of the encountered syntaxic errors
		/// </summary>
		protected List<ParseError> allErrors;
		/// <summary>
		/// Lexer associated to this parser
		/// </summary>
		protected Lexer.BaseLexer lexer;


		/// <summary>
		/// Gets the variable symbols used by this parser
		/// </summary>
		public ROList<Symbol> SymbolVariables { get { return symVariables; } }
		/// <summary>
		/// Gets the virtual symbols used by this parser
		/// </summary>
		public ROList<Symbol> SymbolVirtuals { get { return symVirtuals; } }
		/// <summary>
		/// Gets the action symbols used by this parser
		/// </summary>
		public ROList<SemanticAction> SymbolActions { get { return symActions; } }

		/// <summary>
		/// Gets or sets whether the paser should try to recover from errors
		/// </summary>
		public bool ModeRecoverErrors
		{
			get { return modeRecover; }
			set { modeRecover = value; }
		}
		/// <summary>
		/// Gets or sets a value indicating whether this parser is in debug mode
		/// </summary>
		public bool ModeDebug
		{
			get { return modeDebug; }
			set { modeDebug = value; }
		}

		/// <summary>
		/// Initializes a new instance of the LRkParser class with the given lexer
		/// </summary>
		/// <param name="variables">The parser's variables</param>
		/// <param name="virtuals">The parser's virtuals</param>
		/// <param name="actions">The parser's actions</param>
		/// <param name="lexer">The input lexer</param>
		protected BaseLRParser(Symbol[] variables, Symbol[] virtuals, SemanticAction[] actions, Lexer.BaseLexer lexer)
		{
			this.modeRecover = DEFAULT_MODE_RECOVER;
			this.modeDebug = DEFAULT_MODE_DEBUG;
			this.symVariables = new ROList<Symbol>(new List<Symbol>(variables));
			this.symVirtuals = new ROList<Symbol>(new List<Symbol>(virtuals));
			this.symActions = new ROList<SemanticAction>(new List<SemanticAction>(actions));
			this.modeRecover = true;
			this.allErrors = new List<ParseError>();
			this.lexer = lexer;
			this.lexer.OnError += OnLexicalError;
		}

		/// <summary>
		/// Adds the given lexical error emanating from the lexer to the list of errors
		/// </summary>
		/// <param name="error">Lexical error</param>
		protected void OnLexicalError(ParseError error)
		{
			allErrors.Add(error);
		}

		/// <summary>
		/// Parses the input and returns the result
		/// </summary>
		/// <returns>A ParseResult object containing the data about the result</returns>
		public abstract ParseResult Parse();
	}
}
