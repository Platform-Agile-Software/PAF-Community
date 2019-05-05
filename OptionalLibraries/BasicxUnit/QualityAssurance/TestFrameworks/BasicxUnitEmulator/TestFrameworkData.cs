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
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System;
using System.Collections.Generic;

// Exception shorthand.
using PAED = PlatformAgileFramework.ErrorAndException.PAFAggregateExceptionData;


namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator
{
	/// <summary>
	/// <para>
	/// Utilities to support reflection-based test runners. The utility mehods contained
	/// herein employ stringful properties to access various third-party test framework
	/// machinery so the test frameworks do not have to be explicitly loaded into the
    /// current <c>AppDomain</c>. This level of isolation is needed, since not all
	/// test frameworks will be available on all platforms.
	/// </para>
	/// <para>
	/// The attribute data is exposed as public so framework builders can introduce
	/// their own framework definitions. PAF xUnit definitions generally follow NUnit with
	/// "PAF" tacked on the front, since this is what many programmers are accustomed
	/// to. Goshaloma provides many extensions and additions to these attributes.
	/// </para>
	/// </summary>
	/// <threadsafety>
	/// Nothing in here is synchronized. Framework extenders will load extensions
	/// once, at setup time.
	/// </threadsafety>
    /// <history>
    /// <contribution>
    /// <author> KRM </author>
    /// <date> 01aug2012 </date>
    /// <description>
    /// New.
    /// </description>
    /// </contribution>
    /// </history>
// ReSharper disable PartialTypeWithSinglePart
	public partial class TestFrameworkData
// ReSharper restore PartialTypeWithSinglePart
	{
		#region Class Fields and Autoproperties.
		#region Names of Pre-loaded Test Frameworks
		/// <summary>
		/// FrameworkName.
		/// </summary>
		// TODO - KRM. Put this in as an extension example so MS doesn't sue us.
		public const string MSTestFramework = "MSTest";
		/// <summary>
		/// FrameworkName.
		/// </summary>
		public const string NUnitFramework = "NUnit";
		/// <summary>
		/// FrameworkName.
		/// </summary>
		public const string PAFUnitFramework = "PAFUnit";
		#endregion // Names of Pre-loaded Test Frameworks
		/// <summary>
		/// This is a signal value for the description in the "ignore" test attribute.
		/// It is needed because some test fixtures have the sole purpose of being
		/// a target for test infrastructure tests. We do not want these called in
		/// any automated run of this assembly. This particular string is recognized
		/// by the PAF test runners and causes the ignored test or fixture not to be
		/// reported in the logs.
		/// </summary>
		public const string TEST_TARGET_TEST_IGNORED_DESCRIPTION = "Test target test ignored";
		#region Test Framework Specific Definitions
		/// <summary>
		/// The name of the <see cref="Type"/> of the exception that is thrown when an assertion failure
		/// is detected.
		/// </summary>
		public static readonly IList<string> s_AssertionFailureExceptionNames
			= new List<string>(new[] { "PAFAssertionException", "AssertionException", "AssertFailedException" });
		/// <summary>
		/// The name of the attribute indicating a test method or fixture requires
		/// elevated trust.
		/// </summary>
		public static readonly IList<string> s_ElevatedTrustAttributeNames
			= new List<string>(new[] { "PAFElevatedTrustAttribute", "", "" });
		/// <summary>
		/// The name of the attribute indicating an expected <see cref="Exception"/> to be thrown
		/// during a given test method call.
		/// </summary>
		public static readonly IList<string> s_ExpectedExceptionAttributeNames
			= new List<string>(new[] { "PAFExpectedExceptionAttribute", "ExpectedExceptionAttribute", "ExpectedExceptionAttribute" });
		/// <summary>
		/// The name of the attribute indicating a test or fixture that is to be run
		/// only if explicitly specified. MS doesn't have one.
		/// </summary>
		public static readonly IList<string> s_ExplicitAttributeNames
			= new List<string>(new[] { "PAFExplicitAttribute", "ExplicitAttribute", "" });
		/// <summary>
		/// The name of the attribute indicating an ignored test for the particular
		/// testing system in use. If a method is decorated with this attribute, it is not
		/// run.
		/// </summary>
		public static readonly IList<string> s_IgnoreTestAttributeNames
			= new List<string>(new[] { "PAFIgnoreAttribute", "IgnoreAttribute", "IgnoreAttribute" });
		/// <summary>
		/// The name of the property describing a reason for a test being ignored for the particular
		/// testing system in use.
		/// </summary>
		/// <remarks>
		/// Nunit uses properties on the IgnoreAttribute class, while MS uses
		/// a separate attribute.
		/// </remarks>
		public static readonly IList<string> s_IgnoreTestReasonNames
			= new List<string>(new[] { "Reason", "Reason", "" });
		/// <summary>
		/// The name of the attribute indicating a test class for the particular
		/// testing system in use. In C# and other object-oriented languages,
		/// a "fixture" is a class.
		/// </summary>
		public static readonly IList<string> s_TestClassAttributeNames
			= new List<string>(new[] { "PAFTestFixtureAttribute", "TestFixtureAttribute", "TestClassAttribute" });
		/// <summary>
		/// The name of the property describing a test method for the particular
		/// testing system in use.
		/// </summary>
		/// <remarks>
		/// Nunit uses properties on the TestFixtureAttribute class, while MS uses
		/// a separate attribute.
		/// </remarks>
		public static readonly IList<string> s_TestDescriptionPropertyNames
			= new List<string>(new[] { "Description", "Description", "" });
		/// <summary>
		/// The name of the attribute describing a test method for the particular
		/// testing system in use.
		/// </summary>
		/// <remarks>
		/// Nunit uses properties on the TestFixtureAttribute class, while MS uses
		/// a separate attribute.
		/// </remarks>
		public static readonly IList<string> s_TestDescriptionAttributeNames
			= new List<string>(new[] { "", "", "DescriptionAttribute" });
		/// <summary>
		/// The name of the attribute indicating a test method for the particular
		/// testing system in use.
		/// </summary>
		public static readonly IList<string> s_TestMethodAttributeNames
			= new List<string>(new[] { "PAFTestAttribute", "TestAttribute", "TestMethodAttributeName" });
		/// <summary>
		/// The name of the attribute indicating a test setup method for the particular
		/// testing system in use. This is the method that, if present, is called before
		/// each test method in the test class is called.
		/// </summary>
		public static readonly IList<string> s_TestSetUpMethodAttributeNames
			= new List<string>(new[] { "PAFSetUpAttribute", "SetUpAttribute", "TestInitializeAttribute" });
		/// <summary>
		/// The name of the attribute indicating a test teardown method for the particular
		/// testing system in use. This is the method that, if present, is called after
		/// each test method in the test class is called.
		/// </summary>
		public static readonly IList<string> s_TestTearDownMethodAttributeNames
			= new List<string>(new[] { "PAFTearDownAttribute", "TearDownAttribute", "TestCleanupAttribute" });
		/// <summary>
		/// The name of the attribute indicating a test setup method for the particular
		/// testing system in use. This is the method that, if present, is called ONCE
		/// before the set of test methods in the test class is called.
		/// </summary>
		public static readonly IList<string> s_TestClassSetUpMethodAttributeNames
			= new List<string>(new[] { "PAFTestFixtureSetUpAttribute", "TestFixtureSetUpAttribute", "ClassInitializeAttribute" });
		/// <summary>
		/// The name of the attribute indicating a test teardown method for the particular
		/// testing system in use. This is the method that, if present, is called ONCE
		/// after the set of test methods in the test class is called.
		/// </summary>
		public static readonly IList<string> s_TestClassTearDownMethodAttributeNames
			= new List<string>(new[] { "PAFTestFixtureTearDownAttribute", "TestFixtureTearDownAttribute", "ClassCleanupAttribute" });
		/// <summary>
		/// The name of the attribute indicating a assembly setup method for the particular
		/// testing system in use. This is the method that, if present, is called ONCE
		/// when the assembly is loaded.
		/// </summary>
		public static readonly IList<string> s_TestAssemblySetUpMethodAttributeNames
			= new List<string>(new []{ "PAFHarnessSetUpAttribute", "", "AssemblyInitializeAttribute" });
		/// <summary>
		/// The name of the attribute indicating a assembly teardown method for the particular
		/// testing system in use. This is the method that, if present, is called ONCE
		/// after all test methods in the test assembly is called.
		/// </summary>
		public static readonly IList<string> s_TestAssemblyCleanupMethodAttributeNames
			= new List<string>(new []{ "PAFHarnessTearDownAttribute", "", "AssemblyCleanupAttribute" });
		#endregion // Test Framework Specific Definitions
		/// <summary>
		/// This is the list of framework names that are preloaded. Add your framework
		/// to the end of the list for extending the xUnit emulator.
		/// </summary>
		public static readonly IList<string> s_xUnitNameList
			= new List<string>(new []{PAFUnitFramework, NUnitFramework, MSTestFramework});
		#endregion // Class Fields and Autoproperties.
		#region Methods

		/// <summary>
		/// Returns a list of available test framework names that have been loaded.
		/// </summary>
		/// <returns></returns>
		public static IList<string> GetAvailableTestFrameworks()
		{
			return s_xUnitNameList;
		}

		#region Test Framework Specific Properties
		/// <summary>
		/// The name of the <see cref="Type"/> of the exception that is thrown when an assertion failure
		/// is detected.
		/// </summary>
		/// <param name="testFrameworkName">The name of the framework.</param>
		/// <returns> The name. </returns>
		public static string AssertionFailureExceptionName(string testFrameworkName)
		{
			if(string.IsNullOrEmpty(testFrameworkName)) return null;
			return s_AssertionFailureExceptionNames[FrameworkIndex(testFrameworkName)];
		}
		/// <summary>
		/// The name of the attribute indicating an elevated trust environment is
		/// needed to run a test or fixture
		/// </summary>
		/// <param name="testFrameworkName">The name of the framework.</param>
		/// <returns> The name. </returns>
		public static string ElevatedTrustAttributeName(string testFrameworkName)
		{
			if (string.IsNullOrEmpty(testFrameworkName)) return null;
			return s_ElevatedTrustAttributeNames[FrameworkIndex(testFrameworkName)];
		}
		/// <summary>
		/// The name of the attribute indicating an expected <see cref="Exception"/> to be thrown
		/// during a given test method call.
		/// </summary>
		/// <param name="testFrameworkName">The name of the framework.</param>
		/// <returns> The name. </returns>
		public static string ExpectedExceptionAttributeName(string testFrameworkName)
		{
			if (string.IsNullOrEmpty(testFrameworkName)) return null;
			return s_ExpectedExceptionAttributeNames[FrameworkIndex(testFrameworkName)];
		}
		/// <summary>
		/// The name of the attribute indicating a test or fixture that is to be run
		/// only if explicitly specified. MS doesn't have one.
		/// </summary>
		/// <param name="testFrameworkName">The name of the framework.</param>
		/// <returns> The name. </returns>
		public static string ExplicitAttributeName(string testFrameworkName)
		{
			if (string.IsNullOrEmpty(testFrameworkName)) return null;
			return s_ExplicitAttributeNames[FrameworkIndex(testFrameworkName)];
		}
		/// <summary>
		/// The name of the attribute indicating an ignored test for the particular
		/// testing system in use. If a method is decorated with this attribute, it is not
		/// run.
		/// </summary>
		/// <param name="testFrameworkName">The name of the framework.</param>
		/// <returns> The name. </returns>
		public static string IgnoreTestAttributeName(string testFrameworkName)
		{
			if (string.IsNullOrEmpty(testFrameworkName)) return null;
			return s_IgnoreTestAttributeNames[FrameworkIndex(testFrameworkName)];
		}
		/// <summary>
		/// The name of the attribute indicating a assembly teardown method for the particular
		/// testing system in use. This is the method that, if present, is called ONCE
		/// after all test methods in the test assembly have been called.
		/// </summary>
		/// <param name="testFrameworkName">The name of the framework.</param>
		/// <returns> The name. Blank string for Nunit. </returns>
		/// <remarks>
		/// Nunit does not (in 2.4.8) have an assembly cleanup method attribute.
		/// </remarks>
		public static string TestAssemblyCleanupMethodAttributeName(string testFrameworkName)
		{
			if (string.IsNullOrEmpty(testFrameworkName)) return null;
			return s_TestAssemblyCleanupMethodAttributeNames[FrameworkIndex(testFrameworkName)];
		}
		/// <summary>
		/// The name of the attribute indicating a assembly setup method for the particular
		/// testing system in use. This is the method that, if present, is called ONCE
		/// when the assembly is loaded.
		/// </summary>
		/// <returns> The name. Blank string for Nunit. </returns>
		/// <remarks>
		/// Nunit does not (in 2.4.8) have an assembly setup method attribute.
		/// </remarks>
		public static string TestAssemblySetUpMethodAttributeName(string testFrameworkName)
		{
			if (string.IsNullOrEmpty(testFrameworkName)) return null;
			return s_TestAssemblySetUpMethodAttributeNames[FrameworkIndex(testFrameworkName)];
		}
		/// <summary>
		/// The name of the attribute indicating a test class for the particular
		/// testing system in use. In C# and other object-oriented languages,
		/// a "fixture" is a class.
		/// </summary>
		/// <param name="testFrameworkName">The name of the framework.</param>
		/// <returns> The name. </returns>
		public static string TestClassAttributeName(string testFrameworkName)
		{
			if (string.IsNullOrEmpty(testFrameworkName)) return null;
			return s_TestClassAttributeNames[FrameworkIndex(testFrameworkName)];
		}
		/// <summary>
		/// The name of the attribute indicating a test setup method for the particular
		/// testing system in use. This is the method that, if present, is called ONCE
		/// before the set of test methods in the test class is called.
		/// </summary>
		/// <param name="testFrameworkName">The name of the framework.</param>
		/// <returns> The name. </returns>
		public static string TestClassSetUpMethodAttributeName(string testFrameworkName)
		{
			if (string.IsNullOrEmpty(testFrameworkName)) return null;
			return s_TestClassSetUpMethodAttributeNames[FrameworkIndex(testFrameworkName)];
		}
		/// <summary>
		/// The name of the attribute indicating a test teardown method for the particular
		/// testing system in use. This is the method that, if present, is called ONCE
		/// after the set of test methods in the test class is called.
		/// </summary>
		/// <param name="testFrameworkName">The name of the framework.</param>
		/// <returns> The name. </returns>
		public static string TestClassTearDownMethodAttributeName(string testFrameworkName)
		{
			if (string.IsNullOrEmpty(testFrameworkName)) return null;
			return s_TestClassTearDownMethodAttributeNames[FrameworkIndex(testFrameworkName)];
		}
		/// <summary>
		/// The name of the property describing a test method for the particular
		/// testing system in use.
		/// </summary>
		/// <param name="testFrameworkName">The name of the framework.</param>
		/// <returns> The name. Blank string for MS. </returns>
		/// <remarks>
		/// Nunit uses properties on the TestFixtureAttribute class, while MS uses
		/// a separate attribute.
		/// </remarks>
		public static string TestDescriptionPropertyName(string testFrameworkName)
		{
			if (string.IsNullOrEmpty(testFrameworkName)) return null;
			return s_TestDescriptionPropertyNames[FrameworkIndex(testFrameworkName)];
		}
		/// <summary>
		/// The name of the attribute describing a test method for the particular
		/// testing system in use.
		/// </summary>
		/// <param name="testFrameworkName">The name of the framework.</param>
		/// <returns> The name. Blank string for Nunit. </returns>
		/// <remarks>
		/// Nunit uses properties on the TestFixtureAttribute class, while MS uses
		/// a separate attribute.
		/// </remarks>
		public static string TestDescriptionAttributeName(string testFrameworkName)
		{
			if (string.IsNullOrEmpty(testFrameworkName)) return null;
			return s_TestDescriptionAttributeNames[FrameworkIndex(testFrameworkName)];
		}
		/// <summary>
		/// The name of the attribute indicating a test method for the particular
		/// testing system in use.
		/// </summary>
		/// <param name="testFrameworkName">The name of the framework.</param>
		/// <returns> The name. </returns>
		public static string TestMethodAttributeName(string testFrameworkName)
		{
			if (string.IsNullOrEmpty(testFrameworkName)) return null;
			return s_TestMethodAttributeNames[FrameworkIndex(testFrameworkName)];
		}
		/// <summary>
		/// The name of the attribute indicating a test setup method for the particular
		/// testing system in use. This is the method that, if present, is called before
		/// each test method in the test class is called.
		/// </summary>
		/// <param name="testFrameworkName">The name of the framework.</param>
		/// <returns> The name. </returns>
		public static string TestSetUpMethodAttributeName(string testFrameworkName)
		{
			if (string.IsNullOrEmpty(testFrameworkName)) return null;
			return s_TestSetUpMethodAttributeNames[FrameworkIndex(testFrameworkName)];
		}
		/// <summary>
		/// The name of the attribute indicating a test teardown method for the particular
		/// testing system in use. This is the method that, if present, is called after
		/// each test method in the test class is called.
		/// </summary>
		/// <param name="testFrameworkName">The name of the framework.</param>
		/// <returns> The name. </returns>
		public static string TestTearDownMethodAttributeName(string testFrameworkName)
		{
			if (string.IsNullOrEmpty(testFrameworkName)) return null;
			return s_TestTearDownMethodAttributeNames[FrameworkIndex(testFrameworkName)];
		}
		#endregion // Test Framework Specific Properties
		/// <summary>
		/// This property returns the index into the array of attributes for the various
		/// possible framework types.
		/// </summary>
		/// <param name="testFrameworkName">The name of the framework.</param>
		/// <returns>The index or -1 if not found.</returns>
		public static int FrameworkIndex(string testFrameworkName)
		{
			return s_xUnitNameList.IndexOf(testFrameworkName);
		}
		#endregion // Methods
	}
}
