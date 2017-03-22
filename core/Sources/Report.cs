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

namespace Hime.SDK
{
	/// <summary>
	/// Represents a compilation report
	/// </summary>
	public class Report
	{
		/// <summary>
		/// The list of info messages in this report
		/// </summary>
		private readonly List<object> infos;
		/// <summary>
		/// The list of warnings in this report
		/// </summary>
		private readonly List<object> warnings;
		/// <summary>
		/// The list of errors in this report
		/// </summary>
		private readonly List<object> errors;

		/// <summary>
		/// Gets the informational entries in this report
		/// </summary>
		public ROList<object> Infos { get { return new ROList<object>(infos); } }

		/// <summary>
		/// Gets the informational entries in this report
		/// </summary>
		public ROList<object> Warnings { get { return new ROList<object>(warnings); } }

		/// <summary>
		/// Gets the informational entries in this report
		/// </summary>
		public ROList<object> Errors { get { return new ROList<object>(errors); } }

		/// <summary>
		/// Initializes a new report
		/// </summary>
		public Report()
		{
			infos = new List<object>();
			warnings = new List<object>();
			errors = new List<object>();
		}

		/// <summary>
		/// Adds a new info entry
		/// </summary>
		/// <param name="message">The info message</param>
		public void AddInfo(object message)
		{
			infos.Add(message);
		}

		/// <summary>
		/// Adds a new warning entry
		/// </summary>
		/// <param name="message">The warning message</param>
		public void AddWarning(object message)
		{
			warnings.Add(message);
		}

		/// <summary>
		/// Adds a new error entry
		/// </summary>
		/// <param name="message">The error message</param>
		public void AddError(object message)
		{
			errors.Add(message);
		}
	}
}