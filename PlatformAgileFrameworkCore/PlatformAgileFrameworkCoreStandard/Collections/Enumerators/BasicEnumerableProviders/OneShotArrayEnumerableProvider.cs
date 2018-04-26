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
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
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
	/// Implements <see cref="IPAFResettableEnumerableProvider{T}"/> by
	/// just handing out items in array from first to last. This is a FINITE
	/// enumeration.
	/// </summary>
	/// <typeparam name="T">Type that is to be enumerated.</typeparam>
	/// <threadsafety>
	/// This class is NOT necessarily thread-safe. The incoming array is copied before being
	/// stored as the basic collection for iteration. However, attempts to make modifications
	/// to individual stored items which are not themselves thread-safe can result in
	/// concurrency errors. In the scenario where the user of the class constructs it and
	/// does not make further modifications to the elements in the original array, thread safety
	/// is guaranteed. This class does not use locks on the internals, so a different thread
	/// cannot be resetting the enumeration.
	/// </threadsafety>
	public class OneShotArrayEnumerableProvider<T>: PAFResettableEnumerableProviderBase<T>
	{
		/// <summary>
		/// Constructor loads the array.
		/// </summary>
		/// <param name="dataCollection">
		/// The incoming collection of data. In this constructor, it is
		/// forced to be a finite <see cref="ICollection{T}"/> to underscore
		/// that this enumerable is designed to give a one-time enumeration
		/// of a finite list.
		/// </param>
		/// <param name="haltCurrentEnumeratorOnReset">
		/// See base.
		/// </param>
		/// <param name="cloner">
		/// See base.
		/// </param>
// ReSharper disable ParameterTypeCanBeEnumerable.Local
// Collection guarantees finite-ness.
		public OneShotArrayEnumerableProvider(ICollection<T> dataCollection = null,
			bool haltCurrentEnumeratorOnReset = true, TypeHandlingUtils.TypeCloner<T> cloner = null)
			: base(dataCollection, haltCurrentEnumeratorOnReset, cloner)
		{}
		/// <summary>
		/// <see cref="IPAFResettableEnumerableProvider{T}"/>.
		/// </summary>
		/// <param name="dataArray">
		/// <see cref="IPAFResettableEnumerableProvider{T}"/>. <see langword="null"/>
		/// exits without doing anything. In this method, it is forced to
		/// be a finite enumeration. An <see cref="ICollection{T}"/> or a
		/// <see cref="IPAFResettableEnumerable{T}"/> with finite content
		/// will work.
		/// </param>
		/// <threadsafety>
		/// This method is not synchronized. It lets the base implementation
		/// reset the actual data, so there is no need.
		/// </threadsafety>
		/// <exceptions>
		/// <exception>
		/// <see cref="ArgumentException"/> is thrown if the incoming
		/// <paramref name="dataArray"/> is not finite.
		/// "dataArray must be finite".
		/// </exception>
		/// </exceptions>
		public override void SetData(IEnumerable<T> dataArray)
		{
			if (dataArray == null) return;
// ReSharper disable PossibleMultipleEnumeration
			if (!dataArray.CheckEnumerationIsFinite())
// ReSharper restore PossibleMultipleEnumeration
				throw new ArgumentException("dataArray must be finite");
// ReSharper disable PossibleMultipleEnumeration
			var result = dataArray.ReloadFiniteEnumerable(InnerEnumerable);
// ReSharper restore PossibleMultipleEnumeration
			base.SetData(result);
		}
	}
}