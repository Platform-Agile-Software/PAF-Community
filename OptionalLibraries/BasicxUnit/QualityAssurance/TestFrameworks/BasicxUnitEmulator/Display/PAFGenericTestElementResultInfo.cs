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
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Display
{
    /// <summary>
    /// Default implementation of <see cref="IPAFTestElementResultInfo{T}"/>.
    /// See <see cref="IPAFTestElementResultInfo{T}"/>.
    /// </summary>
    /// <threadsafety>
    /// NOT thread-safe.
    /// </threadsafety>
    public class PAFTestElementResultInfo<T>: PAFTestElementResultInfo,  IPAFTestElementResultInfo<T>
        where T: IPAFTestElementInfo
	{
		#region Fields and Autoproperties
		#region Statics
		/// <remarks>
		/// Provided as a default printer for <typeparamref name="T"/>s
		/// </remarks>
		public static Func<IPAFTestElementResultInfo<T>, bool, int, bool, string> DefaultCustomItemPrinter { get; set; }
			= (testElementResultInfo, displayChildNumber, detailLevel, printHierarchy)
				=> testElementResultInfo.PrintResultAtNode(displayChildNumber, detailLevel, printHierarchy);
		#endregion // Statics
		/// <remarks>
		/// <see cref="IPAFTestElementResultInfo{T}"/>.
		/// </remarks>
		public Func<IPAFTestElementResultInfo<T>, bool, int, bool, string> CustomItemPrinter { get; set; } = DefaultCustomItemPrinter;
		/// <remarks>
		/// <see cref="IPAFTestElementResultInfo{T}"/>.
		/// </remarks>
		public virtual T ElementInfoItem
		{
			get { return (T)m_ElementInfo; }
		}
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Default for construct and set style.
		/// </summary>
		public PAFTestElementResultInfo()
		{
		}
		/// <summary>
		/// Just wraps the info.
		/// </summary>
		/// <param name="elementInfo">Wrapped info.</param>
		public PAFTestElementResultInfo(T elementInfo)
			: base(elementInfo)
		{
		}
		#endregion // Constructors
	}
}
