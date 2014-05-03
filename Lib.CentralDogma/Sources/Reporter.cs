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
using System.IO;
using log4net;

namespace Hime.CentralDogma
{
	/// <summary>
	/// Represents a logger for the CentralDogma assembly
	/// </summary>
	public sealed class Reporter
	{
		private Report report;
		private ILog log;

		/// <summary>
		/// Gets the current report
		/// </summary>
		public Report Result { get { return report; } }

		private static bool configured = false;

		private static void Configure()
		{
			if (configured)
				return;
			log4net.Layout.PatternLayout layout = new log4net.Layout.PatternLayout("%-5p: %m%n");
			log4net.Appender.ConsoleAppender appender = new log4net.Appender.ConsoleAppender();
			appender.Layout = layout;
			log4net.Config.BasicConfigurator.Configure(appender);
			configured = true;
		}

		/// <summary>
		/// Initializes the reporter for the given type
		/// </summary>
		/// <param name="type">The reporting component's type</param>
		public Reporter(System.Type type)
		{
			Configure();
			log = log4net.LogManager.GetLogger(type);
			report = new Report();
		}

		/// <summary>
		/// Adds a new info entry to the log
		/// </summary>
		/// <param name="message">The info message</param>
		public void Info(object message)
		{
			report.Infos.Add(message);
			log.Info(message.ToString());
		}
		/// <summary>
		/// Adds a new warning entry in the log
		/// </summary>
		/// <param name="message">The info message</param>
		public void Warn(object message)
		{
			report.Warnings.Add(message);
			log.Warn(message.ToString());
		}
		/// <summary>
		/// Adds a new error entry in the log
		/// </summary>
		/// <param name="message">The info message</param>
		public void Error(object message)
		{
			report.Errors.Add(message);
			log.Error(message.ToString());
		}
	}
}
