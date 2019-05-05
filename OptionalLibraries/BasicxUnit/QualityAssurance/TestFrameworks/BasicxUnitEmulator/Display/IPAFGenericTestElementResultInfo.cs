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
    /// Generic version of <see cref="IPAFTestElementResultInfo"/>
    /// </summary>
    public interface IPAFTestElementResultInfo<T>: IPAFTestElementResultInfo
        where T: IPAFTestElementInfo
	{
		#region Properties
		///// <summary>
		///// Custom gatherer for printable children that can be installed. Intention is that a
		///// default will be used if <see langword="null"/>. This is in the Generic interface
		///// so we can have specific gatherers for derived types.
		///// </summary>
		//Func<IPAFTestElementResultInfo<T>,IList<IPAFTestElementResultInfo>>
		//	DisplayedChildResultItemGatherer { get; set; }
		/// <summary>
		/// Custom printer that can be installed. This must never be <see langword="null"/>.
		/// a default must always be installed.
		/// </summary>
		Func<IPAFTestElementResultInfo<T>, bool, int, bool, string> CustomItemPrinter { get; set; }
        /// <summary>
		/// Wrapped element info for us to get details
		/// if we need them. Generic version.
		/// </summary>
		T ElementInfoItem { get; }
		#endregion Properties
	}
}
