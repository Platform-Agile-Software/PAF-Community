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

using PlatformAgileFramework.Collections.Enumerators;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator
{
	/// <summary>
	/// Allows enumeration of test elements by providing a provider of enumerables.
	/// This interface constrains its base and extends its base by adding a set method.
	/// </summary>
	/// <threadsafety>
	/// Since this is used in a concurrent testing facility (if used with Goshaloma),
	/// it should be used in a thread safe manner, if run under conditions of
	/// concurrency. We only need the set method, in our own work, in Goshaloma,
	/// but it's here if others have the same use case.
	/// </threadsafety>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 12dec2017 </date>
	/// <description>
	/// Factored this out of Goshaloma, since we need this in BasicxUnit.
	/// Trying to introduce type-safety for ongoing development as well as not
	/// break legacy usages - wish me luck...
	/// </description>
	/// </contribution>
	/// </history>
	public interface ITestElementInfoItemEnumerableProviderProvider<T>
		: IPAFEnumerableProviderProvider<T>
		where T: IPAFTestElementInfo
	{
        #region Properties
		/// <summary>
		/// Provides a thread-safe enumerator for elements. Set should not be used
		/// except at startup. It really should not be there, but there are some
		/// scenarios that require it.
		/// </summary>
		void SetProvider(IPAFEnumerableProvider<T> provider);
		#endregion Properties
	}
}
