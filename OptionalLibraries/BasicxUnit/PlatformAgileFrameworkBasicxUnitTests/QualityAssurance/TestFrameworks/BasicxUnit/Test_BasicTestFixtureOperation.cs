//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 Icucom Corporation
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System;
using System.Collections.Generic;
using PlatformAgileFrameworkCore.ErrorAndException;
using PlatformAgileFrameworkCore.QualityAssurance.TestFrameworks.PAFUnit.Attributes;

// ReSharper disable CheckNamespace
namespace PlatformAgileFrameworkCore.QualityAssurance.TestFrameworks.BasicxUnitEmulator
// ReSharper restore CheckNamespace
{
	// Couple of aliases for readability.
	using TD = Test_BasicTestFixtureOperation_TestData;

	#region Static Test Data Class
	/// <summary>
	/// Container for common test data.
	/// </summary>
	public static class Test_BasicTestFixtureOperation_TestData
	{
		#region Class Fields
		#region Data for tests
		/// <summary>
		/// This stores exceptions for later perusal.
		/// </summary>
		public static List<PAFAbstractExceptionBase> ConcurrencyExceptions { get; set; }
		/// <remarks/>
		public static Int32 TimesTestFixtureSetUpWasCalled { get; set; }
		/// <remarks/>
		public static Int32 TimesTestSetUpWasCalled { get; set; }
		/// <remarks/>
		public static Int32 TimesTestMethod1WasCalled { get; set; }
		/// <remarks/>
		public static Int32 TimesTestMethod2WasCalled { get; set; }
		/// <remarks/>
		public static Int32 TimesTestTearDownWasCalled { get; set; }
		/// <remarks/>
		public static  Int32 TimesTestFixtureTearDownWasCalled { get; set; }
		#endregion // Data for tests
		#endregion // Class Fields
		#region Methods
		/// <summary>
		/// Stuffs some basic data into the collections. Also resets static data.
		/// </summary>
		public static void SetupForBasic()
		{
			/////
			if (ConcurrencyExceptions == null)
				ConcurrencyExceptions = new List<PAFAbstractExceptionBase>();
			ConcurrencyExceptions.Clear();

			TimesTestFixtureSetUpWasCalled = 0;
			TimesTestSetUpWasCalled = 0;
			TimesTestMethod1WasCalled = 0;
			TimesTestMethod2WasCalled = 0;
			TimesTestTearDownWasCalled = 0;
			TimesTestFixtureTearDownWasCalled = 0;
		}
		#endregion // Methods
	}
	#endregion // Static Test Data Class

	#region Tests
	/// <summary>
	/// Tests basic operation of setup/tear down and test mthod runs. Alzo.
	/// </summary>
	[PAFTestFixture("This is a test fixture of the test fixture wrapper")]
	public class Tests_BasicTestFixtureOperation
	{
		///////////////////////////////////////////////////////////////////////
		// This section contains tests that verify basic setup of the classes.
		///////////////////////////////////////////////////////////////////////
		/// <summary>
		/// This test checks operation of TestFixtureWrapper. The goal is to
		/// verify the proper calling of setup/teardown methods. We load with the
		/// Nunit framework parameters and verify that the setup and teardown
		/// methods all get called the proper number of times.
		/// </summary>
		[PAFTest("This is a test that tests the test fixture construction")]
		// ReSharper disable InconsistentNaming
		public void Test_TestFixtureConstruction()
			// ReSharper restore InconsistentNaming
		{
			// Don't forget to set up static data.
			TD.SetupForBasic();
			// Set up the parameters for the wrapper. It is simple, since most things
			// are defaulted for us. With these settings, the PAF test runner will
			// simply run the test fixture as though it was a normal NUnit run.
			var testFixtureInfo = new PAFTestFixtureInfo(typeof (Test_CallLoggingTargetTest));

			// Now create the wrapper and initialize it.
			var wrapper = new PAFTestFixtureWrapper(testFixtureInfo);

			// This initializes the fixture without killing the existing instance if there
			// is one. In our case here, true/false does not matter since we only create one
			// wrapper in this test and reference it locally.
			wrapper.Initialize(false);
			var instance = wrapper.FixtureInfo.FixtureInstance;
			// Fixture has to be here first.
			var success = (instance != null);
			QAUtils.xUnitAssertionService(null, success, "Test Fixture creation.", true);
			wrapper.RunTestMethods(null);
			success =
				((TD.TimesTestFixtureSetUpWasCalled == 1)
				 && (TD.TimesTestFixtureTearDownWasCalled == 1)
				 && (TD.TimesTestSetUpWasCalled == 2)
				 && (TD.TimesTestTearDownWasCalled == 2)
				 && (TD.TimesTestMethod1WasCalled == 1)
				 && (TD.TimesTestMethod2WasCalled == 1)
				);
			// Now see if the setups/teardowns were run right.
			QAUtils.xUnitAssertionService(null, success, "Test and Fixture setup/teardown run counts.", true);
		}
	}
	#endregion // Tests
	#region TargetTests
	/// <summary>
	/// <para>
	/// This is a unit test used to test the unit test framework. It simply logs the fact
	/// that all its methods are called. Noted that it is marked as explicit so it won't get called
	/// by the normal unit test procedure.
	/// </para>
	/// </summary>>
	[PAFTestFixture]
	[PAFIgnore(TestFrameworkData.TEST_TARGET_TEST_IGNORED_DESCRIPTION)]
	// ReSharper disable InconsistentNaming
	public class Test_CallLoggingTargetTest
	// ReSharper restore InconsistentNaming
	{
		/// <summary>
		/// FixtureSetUp Method reports when it is called.
		/// </summary>
		[PAFTestFixtureSetUp]
		public virtual void FixtureSetUpMethod()
		{
			TD.TimesTestFixtureSetUpWasCalled++;
		}
		/// <summary>
		/// TestSetup Method reports when it is called.
		/// </summary>
		[PAFSetUp]
		public virtual void TestSetUpMethod()
		{
			TD.TimesTestSetUpWasCalled++;
		}
		/// <summary>
		/// Test Method reports when it is called.
		/// </summary>
		[PAFTest("Test Method 1")]
		// ReSharper disable InconsistentNaming
		public virtual void TestMethod1()
		// ReSharper restore InconsistentNaming
		{
			TD.TimesTestMethod1WasCalled++;
		}
		/// <summary>
		/// Test Method reports when it is called.
		/// </summary>
		[PAFTest("Test Method 2")]
		public virtual void TestMethod2()
		{
			TD.TimesTestMethod2WasCalled++;
		}
		/// <summary>
		/// TestTearDown Method reports when it is called.
		/// </summary>
		[PAFTearDown]
		public virtual void TestTearDownMethod()
		{
			TD.TimesTestTearDownWasCalled++;
		}
		/// <summary>
		/// FixtureTearDown Method reports whether it is called.
		/// </summary>
		[PAFTestFixtureTearDown]
		public virtual void FixtureTearDownMethod()
		{
			TD.TimesTestFixtureTearDownWasCalled++;
		}
	}
	#endregion // TargetTests
}
