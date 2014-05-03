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
using System.Xml;

namespace Hime.CentralDogma
{
	/// <summary>
	/// Represents a compilation report
	/// </summary>
	public sealed class Report
	{
		private List<object> infos;
		private List<object> warnings;
		private List<object> errors;

		/// <summary>
		/// Gets whether the report contains errors
		/// </summary>
		public bool HasErrors { get { return (errors.Count > 0); } }

		/// <summary>
		/// Gets the informational entries in this report
		/// </summary>
		public List<object> Infos { get { return infos; } }
		/// <summary>
		/// Gets the informational entries in this report
		/// </summary>
		public List<object> Warnings { get { return warnings; } }
		/// <summary>
		/// Gets the informational entries in this report
		/// </summary>
		public List<object> Errors { get { return errors; } }

		/// <summary>
		/// Initializes a new report
		/// </summary>
		public Report()
		{
			this.infos = new List<object>();
			this.warnings = new List<object>();
			this.errors = new List<object>();
		}
	}
}