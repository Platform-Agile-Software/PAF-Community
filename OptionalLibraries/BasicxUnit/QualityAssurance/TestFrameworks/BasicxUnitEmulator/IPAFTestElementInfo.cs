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
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System;
using System.Collections.Generic;
using PlatformAgileFramework.Collections.Enumerators;
using PlatformAgileFramework.Execution.Pipeline;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Display;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator
{
	/// <summary>
	/// Status of a test node. 0 - 2 used in core.
	/// </summary>
	public enum TestElementRunnabilityStatus
	{
		/// <summary>
		/// Test(s) will run.
		/// </summary>
		Active = 0,
		/// <summary>
		/// Test(s) is excluded due to excessive errors.
		/// </summary>
		ExcludedByErrors = 1,
		/// <summary>
		/// Element is excluded by a static attribute.
		/// </summary>
		ExcludedByAttribute = 2,
		/// <summary>
		/// Not on inclusion list when in in inclusion mode.
		/// </summary>
		NotIncludedByList = 3,
		/// <summary>
		/// Specifically excluded.
		/// </summary>
		ExcludedByList = 4,
		/// <summary>
		/// Dynamically culled by the testbench.
		/// </summary>
		ExcludedDynamically = 5,
		/// <summary>
		/// Element is an observation node.
		/// </summary>
		ObservationNode = 6
	}
    /// <summary>
    /// Holds information for each test item(harness, suite, assy, test).
    /// This interface provides the <see cref="IPAFBaseExePipelineInitialize{T}"/>
    /// so that constructors can be designed to just set properties and do other
    /// non-risky (in terms of throwing exceptions) things. The real heavy construction
    /// is done in the initialization method. This is done so we can have a correctly
    /// constructed type to hold on to and put in our tree, even though it may be
    /// rendered invalid by later initailization errors. These classes utilize the
    /// base (short) pipeline in core. Initialization will build the tree and
    /// validate most things. At this point, exposed properties are available for
    /// inspection or binding to a GUI. The "uninitialize" phase is generally
    /// intended to reset and clear things in preparation for another run. Properties
    /// are generally still bindable until dispose is called.
    /// </summary>
    /// <threadsafety>
    /// Implementations are NOT necessarily expected to be thread-safe.
    /// </threadsafety>
    /// <history>
    /// <contribution>
    /// <author> KRM </author>
    /// <date> 12nov2017 </date>
    /// <description>
    /// <para>
    /// Refactored to use a provided services model consistently, so put it
    /// on this interface. Updated for enhanced (broken) reflection library
    /// in .Net standard. Combined Golea style fixtures with xUnit wrapper
    /// style.
    /// </para>
    /// </description>
    /// </contribution>
    /// <contribution>
    /// <author> KRM </author>
    /// <date> 07aug2012 </date>
    /// <description>
    /// <para>
    /// Redesigned so we can keep the variegated <see cref="IPAFTestElementInfo"/>'s,
    /// but provide a simple type-safe interface so we don't have a technical
    /// support disaster when we release this. Existing users need the variegated
    /// collections......
    /// </para>
    /// </description>
    /// </contribution>
    /// <contribution>
    /// <author> KRM </author>
    /// <date> 04aug2012 </date>
    /// <description>
    /// <para>
    /// Added history. Added elevated trust stuff.
    /// </para>
    /// </description>
    /// </contribution>
    /// </history>
    /// <remarks>
    /// Please use the equivalent Generic members in <see cref="IPAFTestElementInfo{T}"/>
    /// if possible, when they exist.
    /// </remarks>
    public interface IPAFTestElementInfo :
        IPAFBaseExePipeline<IPAFServiceManager<IPAFService>>,
        IPAFClassProviderPattern<IPAFServiceManager<IPAFService>>,
        IPAFProviderPattern<IPAFPipelineParams<IPAFServiceManager<IPAFService>>>,
        IDisposable
	{
		#region Properties
		/// <summary>
		/// All children of the node.
		/// </summary>
		IEnumerable<IPAFTestElementInfo> AllChildren { get; }
		/// <summary>
		/// All children of the node that should be displayed in results.
		/// It is the responsibility of the element to determine this.
		/// This must never be <see langword="null"/>, a default must always
		/// be provided. Typically, <see cref="AllChildren"/> is used for
		/// the default.
		/// </summary>
		Func<IPAFTestElementInfo, IList<IPAFTestElementInfo>> GetDisplayChildElements { get; set; }
		/// <summary>
		/// Indicates the various states an element can be in.
		/// </summary>
		TestElementRunnabilityStatus TestElementStatus { get; set; }
		/// <summary>
		/// Exceptions associated with the element.
		/// </summary>
		/// <remarks>
		/// Be careful not to modify elements, since they are NOT thread-safe.
		/// The enumerable must be safe, but it's elements are not.
		/// </remarks>
		IEnumerable<Exception> Exceptions { get; }
		/// <summary>
		/// Reason for excluding test or fixture or assy. <see langword="null"/> for not excluded, blank for
		/// no reason given.
		/// </summary>
		string ExcludedReason { get; set; }
		/// <summary>
		/// Reason for ignoring test or fixture or assy. <see langword="null"/> for not ignored, blank for
		/// no reason given.
		/// </summary>
		string IgnoredReason { get; set; }
	    /// <summary>
	    /// Tells if item needs elevated trust to run.
	    /// </summary>
	    bool IsElevatedTrust { get; set; }
	    /// <summary>
	    /// Tells if the element has threads or tasks running inside any
	    /// of its components. If an assembly, if any of its fixtures
	    /// are running or are scheduled to run, this should be
	    /// <see langword="true"/>. This can be set hierarchically.
	    /// </summary>
	    bool IsRunning { get; set; }
        /// <summary>
        /// If non- <see langword="null"/> the manager attached to this node will be
        /// used for tests on this node.
        /// </summary>
	    IPAFServiceManager<IPAFService> LocalServiceManager { get; }
        /// <summary>
        /// Name of item - assy, class or method name.
        /// </summary>
        string TestElementName { get; }
        /// <summary>
        /// Did element pass? <see langword="null"/> if not run yet or ignored. Generally
        /// this is <see langword="false"/> if any children are false.
        /// </summary>
        bool? Passed { get; set; }
		/// <summary>
		/// Get to my parent. Returns <see langword="null"/> if root.
		/// </summary>
		IPAFTestElementInfo Parent { get; }
		/// <summary>
		/// Custom Enumerator for element info for the test items. This may be <see langword="null"/>
		/// if not provided. Noted: Do not convert this enumerator to a list, since the
		/// enumeration may be infinite! This enumerates locally-held <see cref="IPAFTestElementInfo"/>'s.
		/// If this property is <see langword="null"/>, the enumeration will be once through
		/// in the original enumeration.
		/// </summary>
		IPAFEnumerableProvider<IPAFTestElementInfo> TestElementEnumerableProvider { get; set; }
		/// <summary>
		/// This generates display data for us, on demand.
		/// </summary>
		IPAFTestElementResultInfo TestElementResultInfo { get; }
		#endregion Properties
		#region Methods
		/// <summary>
		/// Adds an element to the node.
		/// </summary>
		/// <param name="testElementInfo">
		/// Element info to add.
		/// </param>
		/// <remarks>
		/// Please use the Generic version if possible.
		/// </remarks>
		void AddTestElement(IPAFTestElementInfo testElementInfo);
		/// <summary>
		/// Adds an exception to the node.
		/// </summary>
		/// <param name="exception">
		/// Exception to add.
		/// </param>
		void AddTestException(Exception exception);
		/// <summary>
		/// This one has to be directly on the interface so it can be overridden
		/// in the Generic.
		/// </summary>
		/// <returns>The elements that the implementation chooses to display.</returns>
		IList<IPAFTestElementInfo> GetElementsToDisplay();
		#endregion Methods
	}
}
