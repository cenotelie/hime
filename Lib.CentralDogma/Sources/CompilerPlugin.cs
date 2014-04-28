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
using Hime.Redist;

namespace Hime.CentralDogma
{
	/// <summary>
	/// Represents a plugin for a compilation task
	/// </summary>
	/// <remarks>
	/// Compiler plugins are used to interpret data that have already been parsed
	/// </remarks>
	interface CompilerPlugin
	{
		/// <summary>
		/// Gets the data loader
		/// </summary>
		/// <param name="resName">The name of the data to load</param>
		/// <param name="node">The root of the AST containing the data to load</param>
		/// <param name="log">The reporter</param>
		Grammars.GrammarLoader GetLoader(string resName, ASTNode node, Reporting.Reporter log);
	}
}
