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

using System;
using System.Collections.Generic;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.Collections.Enumerators.BasicEnumerableProviders
{
	/// <summary>
	/// Sealed version of <see cref="PAFResettableEnumerableProviderBase{T}"/>
	/// </summary>
	/// <typeparam name="T">See base.</typeparam>
	/// <threadsafety>
	/// See base.
	/// </threadsafety>
	public sealed class PAFResettableEnumerableProvider<T>
		: PAFResettableEnumerableProviderBase<T>
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
		public PAFResettableEnumerableProvider(IEnumerable<T> enumerable = null,
			bool haltCurrentEnumeratorOnReset = true, TypeHandlingUtils.TypeCloner<T> cloner = null)
			:base(enumerable, haltCurrentEnumeratorOnReset, cloner){}
		/// <summary>
		/// See base.
		/// </summary>
		/// <param name="oldEnumerable">
		/// See base.
		/// </param>
		public PAFResettableEnumerableProvider(PAFResettableEnumerableProviderBase<T> oldEnumerable)
			:base(oldEnumerable){}
		#endregion // Constructors
	}
}