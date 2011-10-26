/*
 * Author: Charles Hymans
 * 
 */
using System;
using NUnit.Framework;
using Hime.Kernel.Reporting;

namespace Hime.Tests.Project0_CentralDogma
{
	[TestFixture]
	public class Suite02_Report
	{
		[Test]
		public void Test000_ErrorCount_ShouldReturnTheNumberOfErrors()
		{
			Report report = new Report();
			Assert.AreEqual(0, report.ErrorCount);
		}
		
		// TODO: I find the system of section not easy to use for report!!!
		// would be better to have a linear collection of errors
	}
}

