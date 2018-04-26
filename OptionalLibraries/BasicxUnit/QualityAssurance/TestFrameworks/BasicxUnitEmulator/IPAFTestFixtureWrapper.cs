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

using System;
using PlatformAgileFramework.MultiProcessing.Threading;
using PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Exceptions;
using PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.TestEnumerableProviders;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator
{
	#region Delegates
	/// <summary>
	/// This is the standard delegate for the test runner.
	/// </summary>
	/// <param name="testFixtureWrapperProvider">
	/// Characteristics and state of the fixture.
	/// </param>
	/// <returns>
	/// <see langword="null"/> if all is well.
	/// </returns>
	/// <remarks>
	/// The test runner delegate should not set any exceptions or status
	/// indicators on the <paramref name="testFixtureWrapperProvider"/> in the
	/// case of a general <see cref="PAFTestFixtureMethodExceptionMessageTags.TEST_RUNNER_FAILURE"/>
	/// error. This should be done by the test runner caller.
	/// </remarks>
	public delegate Exception
		PAFTestRunner(IPAFTestFixtureWrapperProvider testFixtureWrapperProvider);
	#endregion // Delegates
	/// <summary>
	/// <para>
	/// This interface defines the methods to call into various tests designed
	/// to run within different unit test frameworks. The interface offers the
	/// ability to install a custom enumerator that allows an arbitrary enumeration
	/// over the test methods.
	/// </para>
	/// <para>
	/// This interface enforces the PAF pipeline pattern. It is not so necessary in
	/// the simple base implementations, but becomes necessary in the concurrent
	/// testing extensions.
	/// </para>
	/// </summary>
	/// <threadsafety>
	/// A wrapper is designed to be called from a single thread.
	/// </threadsafety>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 18dec2017 </date>
	/// <description>
	/// Made a <see cref="IPAFTestElementInfo"/>
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> BMC </author>
	/// <date> 22jul2012 </date>
	/// <description>
	/// Separated from the fixture tree for BasicxUnit. Cleaned up DOCs.  Made
	/// general mods to run in the same appdomain as the caller for SL.
	/// </description>
	/// </contribution>
    /// </history>
	// ReSharper disable once PossibleInterfaceMemberAmbiguity
	//// Have to implement at least one as explicit, ReSharper.
	public interface IPAFTestFixtureWrapper : IParameterizedThreadStartProvider,
	    IPAFTestElementInfo<IPAFTestFixtureWrapper>,
		ITestElementInfoItemResettableEnumerableProviderProvider<IPAFTestMethodInfo>
	{
		#region Properties
		/// <summary>
		/// This is provided so that the state of the wrapped test fixture can be
		/// examined.
		/// </summary>
		IPAFTestFixtureInfo FixtureInfo { get; }

		/// <summary>
		/// This is the SETTABLE test runner that normally will be used as a
		/// <see cref="IParameterizedThreadStartProvider"/> from the days of old.
		/// </summary>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">"TestRunnerDelegate"</exception>
		/// </exceptions>
		/// <remarks>
		/// This is where all the concurrency was moved to. Now folks can use
		/// threads or tasks. Cleanest way to support legacy stuff.
		/// </remarks>
		Action<IPAFTestFixtureWrapper, object> TestRunnerdelegate { get; set; }

		/// <summary>
		/// This is the delegate that runs the methods in the test fixture. This delegate
		/// may be <see langword="null"/>, in which case the wrapper must use a default.
		/// </summary>
		PAFTestRunner TestRunner { get; }
		#endregion // Properties
	}
}
