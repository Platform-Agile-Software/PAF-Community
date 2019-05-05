//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2017Icucom Corporation
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

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator
{
	/// <summary>
	/// Holds static and dynamic information (e.g. the instance) for each harness.
	/// </summary>
	/// <threadsafety>
	/// Implementations are NOT necessarily expected to be thread-safe.
	/// </threadsafety>
 	/// <history>
    /// <contribution>
	/// <author> KRM </author>
	/// <date> 01aug2012 </date>
    /// <description>
	/// Separated from the fixture tree for BasicxUnit. Derived
	/// directly from <see cref="IPAFTestElementInfo"/> 
    /// </description>
	/// </contribution>
	/// </history>
	// ReSharper disable once PartialTypeWithSinglePart
	// Remainder in Goshaloma.
	public partial interface IPAFTestHarnessInfo: IPAFTestElementInfo
    {
	}
}
