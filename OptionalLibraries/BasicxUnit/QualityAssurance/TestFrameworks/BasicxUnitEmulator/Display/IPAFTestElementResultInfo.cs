//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 -2017 Icucom Corporation
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

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Display
{
	/// <summary>
	/// Holds test result information for each test item(harness, suite, assy, test).
	/// </summary>
	/// <threadsafety>
	/// Implementations are NOT necessarily expected to be thread-safe.
	/// </threadsafety>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 22nov2017 </date>
	/// <description>
	/// Refactored to be produced "on-demand" by a test framework.
	/// </description>
	/// </contribution>
	/// </history>
	public interface IPAFTestElementResultInfo
	{
        #region Properties
		/// <summary>
		/// This is the 0 - based element number in a set of children. -1 is a signal
		/// value for a number NOT to be displayed in the output. This is used by GUIs
		/// to NOT label the root node in a display section.
		/// </summary>
		int ChildElementNumber { get; set; }
		/// <summary>
		/// Custom gatherer for printable children that can be installed. Intention is that a
		/// default will be used if <see langword="null"/>.
		/// </summary>
		/// <exceptions>
		/// <exception cref="ArgumentNullException()">"ChildGatherer"</exception>
		/// </exceptions>
		Func<IPAFTestElementResultInfo, IList<IPAFTestElementResultInfo>>
			DisplayChildResultGatherer { get; set; }
		/// <summary>
		/// Custom printer that can be installed. Intention is that a
		/// default will be used if <see langword="null"/>.
		/// </summary>
		/// <exceptions>
		/// <exception cref="ArgumentNullException()">"CustomPrinter"</exception>
		/// </exceptions>
		Func<IPAFTestElementResultInfo, bool, int, bool, string> CustomPrinter { get; set; }
		/// <summary>
		/// Wrapped element info for us to get details
		/// if we need them.
		/// </summary>
		IPAFTestElementInfo ElementInfo { get; set; }
		/// <summary>
		/// Method, Fixture, Assembly, AssemblySet, Harness.
		/// </summary>
		string ElementTypeTag { get; }
		/// <summary>
		/// Should we display the node at all? Set this to turn off display
		/// unconditionally - otherwise it depends on failures.
		/// </summary>
		bool? ShouldDisplay { get; set; }
		/// <summary>
		/// Should we display failing nodes.
		/// </summary>
		bool ShouldDisplayFailures { get; set; }
		/// <summary>
		/// Should we display inactive/excluded nodes.
		/// </summary>
		bool ShouldDisplayInactive { get; set; }
		/// <summary>
		/// Should we display passing nodes.
		/// </summary>
		bool ShouldDisplaySuccesses { get; set; }
		#endregion Properties
	}
}
