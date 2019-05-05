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
using PlatformAgileFramework.Collections.Enumerators.BasicEnumerableProviders;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.TestEnumerableProviders
{
	/// <summary>
	/// Default implementation of <see cref="ITestElementInfoItemResettableEnumerableProvider{T}"/>
	/// </summary>
	/// <typeparam name="T">A <see cref="IPAFTestElementInfo"/>.</typeparam>
	/// <threadsafety>
	/// Safe
	/// </threadsafety>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 13dec2017 </date>
	/// <description>
	/// Built this container so we can just delegate to it.
	/// </description>
	/// </contribution>
	/// </history>
	public class ResettableTestElementInfoEnumerableProvider<T>
		: PAFResettableEnumerableProviderBase<T>,
		ITestElementInfoItemResettableEnumerableProvider<T>
		where T: IPAFTestElementInfo
	{
		#region Constructors
		/// <summary>
		/// See base.
		/// </summary>
		/// <param name="enumerable">
		/// See base.
		/// </param>
		/// <param name="haltCurrentEnumeratorOnReset">
		/// See base.
		/// </param>
		/// <param name="cloner">
		/// See base.
		/// </param>
		public ResettableTestElementInfoEnumerableProvider(IEnumerable<T> enumerable = null,
			bool haltCurrentEnumeratorOnReset = true, TypeHandlingUtils.TypeCloner<T> cloner = null)
			:base(enumerable, haltCurrentEnumeratorOnReset, cloner){}
		/// <summary>
		/// See base.
		/// </summary>
		/// <param name="oldEnumerable">
		/// See base.
		/// </param>
		public ResettableTestElementInfoEnumerableProvider(PAFResettableEnumerableProviderBase<T> oldEnumerable)
			:base(oldEnumerable){}
		#endregion // Constructors

		/// <summary>
		/// <see cref="ITestElementInfoItemResettableEnumerableProvider{T}"/>
		/// </summary>
		/// <param name="enumerable">
		/// <see cref="ITestElementInfoItemResettableEnumerableProvider{T}"/>
		/// </param>
		public void SetEnumerable(IEnumerable<T> enumerable)
		{
			m_InnerEnumerable = enumerable;
		}
	}
}