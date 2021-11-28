/*******************************************************************************
 * Copyright (c) 2021 Association Cénotélie (cenotelie.fr)
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

//! The results of tests

use std::io::Write;
use std::iter::Sum;
use std::ops::Add;
use std::time::{Duration, Instant};

use hime_sdk::errors::Error;
use hime_sdk::Runtime;

/// The statistics for test results
#[derive(Debug, Default, Clone, Copy)]
pub struct Statistics {
    /// The number of tests in success
    pub success: usize,
    /// The number of tests in failure
    pub failure: usize,
    /// The number of tests in error
    pub error: usize
}

impl Add<Statistics> for Statistics {
    type Output = Statistics;
    fn add(self, rhs: Statistics) -> Self::Output {
        Statistics {
            success: self.success + rhs.success,
            failure: self.failure + rhs.failure,
            error: self.error + rhs.error
        }
    }
}

impl Sum for Statistics {
    fn sum<I: Iterator<Item = Self>>(iter: I) -> Self {
        let mut success = 0;
        let mut failure = 0;
        let mut error = 0;
        for s in iter {
            success += s.success;
            failure += s.failure;
            error += s.error;
        }
        Statistics {
            success,
            failure,
            error
        }
    }
}

impl Statistics {
    /// Gets the total number of executed tests
    pub fn total(&self) -> usize {
        self.success + self.failure + self.error
    }
}

/// The results of the execution of tests
#[derive(Debug, Default, Clone)]
pub struct ExecutionResults(pub Vec<FixtureResults>);

impl ExecutionResults {
    /// Gets the statistics for this execution
    pub fn get_stats(&self) -> Statistics {
        self.0.iter().map(|t| t.get_stats()).sum()
    }
}

/// The results for the tests on a fixture
#[derive(Debug, Clone)]
pub struct FixtureResults {
    /// The fixture's name
    pub name: String,
    /// The tests results
    pub tests: Vec<TestResult>
}

impl FixtureResults {
    /// Gets the statistics for this fixture
    pub fn get_stats(&self) -> Statistics {
        self.tests.iter().map(|t| t.get_stats()).sum()
    }
}

/// The result of a single test for different runtimes
#[derive(Debug, Clone)]
pub struct TestResult {
    /// The test name
    pub name: String,
    /// The result of the test on the .Net runtime
    pub dot_net: Option<TestResultOnRuntime>,
    /// The result of the test on the Java runtime
    pub java: Option<TestResultOnRuntime>,
    /// The result of the test on the Rust runtime
    pub rust: Option<TestResultOnRuntime>
}

impl TestResult {
    /// Gets the statistics for this test
    pub fn get_stats(&self) -> Statistics {
        self.dot_net
            .as_ref()
            .map(|r| r.get_stats())
            .unwrap_or_default()
            + self
                .java
                .as_ref()
                .map(|r| r.get_stats())
                .unwrap_or_default()
            + self
                .rust
                .as_ref()
                .map(|r| r.get_stats())
                .unwrap_or_default()
    }
}

/// The status of a test result
#[derive(Debug, Clone, Copy, PartialEq, Eq)]
pub enum TestResultStatus {
    /// The test was a success
    Success,
    /// The test failed on the test assertion
    Failure,
    /// The test is on error
    Error
}

/// The result of a single test on a specific runtime
#[derive(Debug, Clone)]
pub struct TestResultOnRuntime {
    /// The runtime for test
    pub runtime: Runtime,
    /// The start time for the test
    pub start_time: Instant,
    /// The time spent in the test
    pub spent_time: Duration,
    /// The test status in the end
    pub status: TestResultStatus,
    /// The console output for the test
    pub output: String
}

impl TestResultOnRuntime {
    /// Gets the statistics for this result
    pub fn get_stats(&self) -> Statistics {
        match self.status {
            TestResultStatus::Success => Statistics {
                success: 1,
                failure: 0,
                error: 0
            },
            TestResultStatus::Failure => Statistics {
                success: 0,
                failure: 1,
                error: 0
            },
            TestResultStatus::Error => Statistics {
                success: 0,
                failure: 0,
                error: 1
            }
        }
    }

    /// Writes the test result as XML
    pub fn write_xml<W: Write>(&self, writer: &mut W, test_name: &str) -> Result<(), Error> {
        write!(writer, "<testcase ")?;

        writeln!(writer, ">")?;

        writeln!(writer, "</testcase>")?;
        Ok(())
    }
}
