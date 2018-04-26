//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2017 Icucom Corporation
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

using System.Collections.Generic;
using System.Reflection;
using PlatformAgileFramework.Collections.Enumerators;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator
{
	/// <summary>
	/// Holds static and dynamic information (e.g. the instance) for each fixture.
	/// </summary>
	/// <threadsafety>
	/// See individual members. Advice is given based on typical useage.
	/// </threadsafety>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 13dec2017 </date>
	/// <description>
	/// More consolidation and type-safety work. Derive now from the CLOSED Generic
	/// <see cref="IPAFTestElementInfo{IPAFTestFixtureInfo}"/> and
	/// <see cref="IPAFResettableEnumerableProvider{T}"/>
	/// to get a simple, type-safe enumerator for children. We can still
	/// enumerate over anything <see cref="IPAFTestElementInfo"/> - ish through
	/// extension methods.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> BMC </author>
	/// <date> 29sep2012 </date>
	/// <description>
	/// Re-added inclusion, exclusion lists due to angry user.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 01aug2012 </date>
	/// <description>
	/// Separated from the fixture tree for BasicxUnit. Derived
	/// directly from <see cref="IPAFTestElementInfo"/> 
	/// </description>
	/// </contribution>
	/// </history>
	// ReSharper disable once PossibleInterfaceMemberAmbiguity
	//// Just need to implement at least one explicitly, ReSharper.
	public interface IPAFTestFixtureInfo : IPAFTestElementInfo<IPAFTestFixtureInfo>,
		ITestElementInfoItemEnumerableProviderProvider<IPAFTestMethodInfo>
	{
		#region Properties

		/// <summary>
		/// Constructor info.
		/// </summary>
		/// <threadsafety>
		/// Normally loaded in the construction path - needn't be synchronized.
		/// </threadsafety>
		ConstructorInfo FixtureConstructor { get; }

		/// <summary>
		/// Fixture object.
		/// </summary>
		/// <threadsafety>
		/// Normally loaded in the construction path - needn't be synchronized.
		/// </threadsafety>
		object FixtureInstance { get; set; }

		/// <summary>
		/// Method info. for the fixture setup method.
		/// </summary>
		/// <threadsafety>
		/// Normally loaded in the construction path - needn't be synchronized.
		/// </threadsafety>
		IPAFTestMethodInfo FixtureSetUpMethod { get; set; }

		/// <summary>
		/// Method info. for the fixture teardown method.
		/// </summary>
		/// <threadsafety>
		/// Normally loaded in the construction path - needn't be synchronized.
		/// </threadsafety>
		IPAFTestMethodInfo FixtureTearDownMethod { get; }

		/// <summary>
		/// The type of the fixture class.
		/// </summary>
		/// <threadsafety>
		/// Normally loaded in the construction path - needn't be synchronized.
		/// </threadsafety>
		IPAFTypeHolder FixtureType { get; }

		/// <summary>
		/// The framework params for this fixture.
		/// </summary>
		/// <threadsafety>
		/// Normally loaded in the construction path - needn't be synchronized.
		/// </threadsafety>
		IPAFTestFrameworkBehavior FrameworkBehavior { get; set; }

		/// <summary>
		/// This is the list of test methods out of all test methods
		/// on the class that will be called. This must be set before the
		/// fixture is initialized. The reason this is so useful is that
		/// it allows a tester to focus on all but a certain number of
		/// tests in a fixture. This is most useful for the development
		/// of tests. It has also traditionally been used to dynamically
		/// filter out tests that have been identitied as "long-running"
		/// tests for an initial test run.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Set either <see cref="TestMethodExclusionList"/>
		/// or this. Obviously it makes no sense to set both, but some
		/// folks did in the old framework :-( 
		/// </para>
		/// <para>
		/// Since these methods are typically looked up through relection,
		/// there is no guarantee that they will exist. There is no compulsion
		/// for an implementation to guarantee that they are found. No
		/// spelling errors, please.....
		/// </para>
		/// </remarks>
		IList<string> TestMethodInclusionList { get; set; }
		/// <summary>
		/// This is the list of test methods out of all test methods
		/// on the class that will not be called. This must be set before the
		/// fixture is initialized.  The reason this is so useful is that
		/// it allows a tester to focus on a certain number of
		/// tests in a fixture. This is most useful for the development
		/// of tests.
		/// </summary>
		/// <remarks>
		/// <para></para>
		/// Set either <see cref="TestMethodInclusionList"/>
		/// or this. Obviously it makes no sense to set both, but some
		/// folks did in the old framework :-( 
		/// </remarks>
		IList<string> TestMethodExclusionList { get; set; }

		/// <summary>
		/// Has the fixture setup method been called? Ignored for JUnit emulation
		/// or <see langword="null"/> instance.
		/// </summary>
		/// <threadsafety>
		/// Must be synchronized.
		/// </threadsafety>
		bool HasFixtureSetupBeenCalled { get; set; }
		/// <summary>
		/// Has the fixture tear down method been called? Ignored for JUnit emulation
		/// or <see langword="null"/> instance.
		/// </summary>
		/// <threadsafety>
		/// Must be synchronized.
		/// </threadsafety>
		bool HasFixtureTearDownBeenCalled { get; set; }
		/// <summary>
		/// Limits the number of times test methods from a fixture have been called.This
		/// property is needed to limit the tests called for an infinite enumerator.
		/// </summary>
		/// <remarks>
		/// This needs to be here instead of on <see cref="IPAFTestFixtureWrapper"/>, since different
		/// wrappers may be accessing this.
		/// </remarks>
		long MaxTimesAnyTestCalled { get; set; }
		/// <summary>
		/// Keeps track of the number of times a test method from a fixture has been called.
		/// For straight Nunit or MSTest emulation, this number increases from zero to the
		/// total number of active test methods in the fixture.
		/// </summary>
		/// <remarks>
		/// This needs to be here instead of on <see cref="IPAFTestFixtureWrapper"/>, since different
		/// wrappers may be accessing this.
		/// <threadsafety>
		/// Must be synchronized.
		/// </threadsafety>
		/// </remarks>
		long NumTimesAnyTestCalled { get; set; }

		/// <summary>
		/// Used to determine if any wrappers are still active, to make
		/// a detemination if we are still running.
		/// </summary>
		/// <threadsafety>
		/// Normally loaded in the construction path - needn't be synchronized.
		/// </threadsafety>
		IList<IPAFTestFixtureWrapper> TestFixtureWrappers { get; }
		/// <summary>
		/// Method info. for the test setup method.
		/// </summary>
		/// <threadsafety>
		/// Normally loaded in the construction path - needn't be synchronized.
		/// </threadsafety>
		IPAFTestMethodInfo TestSetUpMethod { get; set; }
		/// <summary>
		/// Method info. for the test teardown method.
		/// </summary>
		/// <threadsafety>
		/// Normally loaded in the construction path - needn't be synchronized.
		/// </threadsafety>
		IPAFTestMethodInfo TestTearDownMethod { get; set; }
		#endregion Properties
		#region Methods
		/// <summary>
		/// Returns a collection of the most common contained
		/// objects - test methods. This returns only active methods.
		/// </summary>
		/// <returns>
		/// Should never be <see langword="null"/> if the parent is "active".
		/// </returns>
		/// <threadsafety>
		/// Needs to be synchronized.
		/// </threadsafety>
		ICollection<IPAFTestMethodInfo> GetActiveTestMethods();
		/// <summary>
		/// Returns a collection of the most common contained
		/// objects - test methods. This returns ALL test methods,
		/// whether active or not.
		/// </summary>
		/// <returns>
		/// Should never be <see langword="null"/> if the parent is "active".
		/// </returns>
		/// <threadsafety>
		/// Normally loaded in the construction path - needn't be synchronized.
		/// </threadsafety>
		ICollection<IPAFTestMethodInfo> GetTestMethods();
		/// <summary>
		/// Just exposes the set method for the pipeline is running prop.
		/// </summary>
		/// <param name="isRunning">
		/// Sets fixture to running or not.
		/// </param>
		/// <threadsafety>
		/// TARGET must be synchronized.
		/// </threadsafety>
		void SetTestFixtureRunning(bool isRunning);
		#endregion // Methods
	}
}
