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

namespace Hime.Benchmark
{
	/// <summary>
	/// Represents a task
	/// </summary>
	public interface Task
	{
		/// <summary>
		/// Executes this task
		/// </summary>
		/// <param name="assembly">Path to the assembly containing the component to benchmark</param>
		/// <param name="input">Path to the input file</param>
		/// <param name="useStream"><code>true</code> if the input shall be used a stream (instead of a pre-read string)</param>
		void Execute(string assembly, string input, bool useStream);
	}
}
