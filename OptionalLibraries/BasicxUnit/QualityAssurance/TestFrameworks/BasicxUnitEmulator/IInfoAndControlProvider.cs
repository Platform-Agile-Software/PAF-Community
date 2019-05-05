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
using PlatformAgileFramework.MultiProcessing.AsyncControl;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator
{
	/// <summary>
	/// Just provides an accessor for a <see cref="IPAFTestFixtureWrapper"/> and 
	/// <see cref="IAsyncControlObject"/>to aid in aggregation.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 20dec2017 </date>
	/// <description>
	/// Bit the bullet and made this an accessor for
	/// <see cref="IPAFTestFixtureWrapper"/>
	/// insteadof <see cref="IPAFTestFixtureInfo"/>, which is
	/// the way it should have been. The old one has been moved to Goshaloma,
	/// since that's the only place it's used.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> BMC </author>
	/// <date> 22jul2012 </date>
	/// <description>
	/// Separated from the fixture tree for BasicxUnit.
	/// </description>
	/// </contribution>
	/// </history>
	public interface IInfoAndControlProvider
		: IPAFTestFixtureWrapperProvider, IAsyncControlObjectProvider
	{
	}
}
