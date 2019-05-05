//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2017 Icucom Corporation
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

using System.Collections.Generic;
using PlatformAgileFramework.Collections.Enumerators;
using PlatformAgileFramework.Collections.Enumerators.BasicEnumerableProviders;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.TestEnumerableProviders
{
	/// <summary>
	/// Default implementation of <see cref="ITestElementInfoItemEnumerableProviderProvider{T}"/>.
	/// This interface extends its base by adding a set method.
	/// </summary>
	/// <typeparam name="T">
	/// Constrained to be a <see cref="IPAFTestElementInfo"/> for our testing model.
	/// </typeparam>
	/// <threadsafety>
	/// This class is thread-safe if used as a immutable type (i.e. don't use the set method).
	/// </threadsafety>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 12dec2017 </date>
	/// <description>
	/// Factored this out of Goshaloma, since we need this in BasicxUnit.
	/// </description>
	/// </contribution>
	/// </history>
	public class TestElementInfoItemEnumerableProvider<T>
		: PAFEnumerableProvider<T> where T:IPAFTestElementInfo
	{
		#region Constructors
		/// <summary>
		/// Constructor just accepts an enumerable.
		/// </summary>
		/// <param name="enumerable">
		/// The incoming enumerable to be wrapped.
		/// </param>
		public TestElementInfoItemEnumerableProvider(IEnumerable<T> enumerable): base(enumerable)
		{
		}
		#endregion // Constructors
		#region Methods
		/// <summary>
		/// <see cref="ITestElementInfoItemResettableEnumerableProvider{T}"/>
		/// </summary>
		public void SetProvider(IEnumerable<T> enumerable)
		{
			m_Enumerable = enumerable;
		}
		#endregion // Methods
	}
}