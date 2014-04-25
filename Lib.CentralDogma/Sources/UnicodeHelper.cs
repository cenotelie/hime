/**********************************************************************
* Copyright (c) 2014 Laurent Wouters and others
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

using System.Net;

namespace Hime.CentralDogma
{
	/// <summary>
	/// Contains a set of helper methods for the support of Unicode
	/// </summary>
	/// <remarks>
	/// The current supported Unicode version is 6.3.0
	/// </remarks>
	public class UnicodeHelper
	{
		/// <summary>
		/// The URL of the latest specification of Unicode blocks
		/// </summary>
		private const string urlUnicodeBlocks = "http://www.unicode.org/Public/UCD/latest/ucd/Blocks.txt";
		/// <summary>
		/// The URL of the latest specification of Unicode code points
		/// </summary>
		private const string urlUnicodeData = "http://www.unicode.org/Public/UCD/latest/ucd/UnicodeData.txt";

		public static void UpdateBlocks()
		{
			WebClient client = new WebClient();
			client.DownloadFile(urlUnicodeBlocks, "Blocks.txt");
		}
	}
}