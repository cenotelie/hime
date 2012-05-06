/*
 * Author: Charles Hymans
 * 
 */
using System;
using NUnit.Framework;
using Hime.Utils.Reporting;

namespace Hime.Tests.Project0_CentralDogma
{
	[TestFixture]
	public class Suite02_Report
	{
		[Test]
		public void Test000_Constructor_ShouldNotFail()
		{
			new Report();
		}
		
		[Test]
		public void Test001_ErrorCount_ShouldReturnTheNumberOfErrors()
		{
			Report report = new Report();
			Assert.AreEqual(0, report.ErrorCount);
		}
		
		[Test]
		public void Test002_Errors_ShouldEnumerateAllErrors()
		{
			Report report = new Report();
			Section section = report.AddSection("Parser");
			section.AddEntry(new Entry(ELevel.Error, "", ""));
			int count = 0;
			foreach (Entry error in report.Errors)
			{
				count++;
			}
			Assert.AreEqual(1, count);
		}
		
		// TODO: I find the system of section not easy to use for report!!!
		// would be better to have a linear collection of errors
	}
}

