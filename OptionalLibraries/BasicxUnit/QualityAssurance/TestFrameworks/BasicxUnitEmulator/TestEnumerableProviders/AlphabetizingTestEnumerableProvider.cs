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
using System.Reflection;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.Collections.Enumerators;
using PlatformAgileFramework.Collections.Enumerators.BasicEnumerableProviders;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.TestEnumerableProviders
{
	/// <summary>
	/// Implements <see cref="IPAFResettableEnumerableProvider{MethodInfo}"/> by
	/// just handing out items in array from first to last.
	/// </summary>
	/// <remarks>
	/// This class is NOT thread-safe. To make it so, the array should be copied
	/// before enumeration.
	/// </remarks>
	public class AlphabetizingTestEnumerableProvider<T> : OneShotArrayEnumerableProvider<T>,
		ITestElementInfoItemResettableEnumerableProvider<T>
		where T:IPAFTestElementInfo
	{
		#region Constructors
		/// <summary>
		/// Constructor loads the array.
		/// </summary>
		/// <param name="dataCollection">
		/// The incoming collection of <see cref="IPAFTestElementInfo"/>s.
		/// </param>
		/// <param name="haltCurrentEnumeratorOnReset">
		/// See base.
		/// </param>
		/// <param name="cloner">
		/// See base.
		/// </param>
// ReSharper disable ParameterTypeCanBeEnumerable.Local
// Collection guarantees finite-ness.
		public AlphabetizingTestEnumerableProvider(ICollection<T> dataCollection = null,
			bool haltCurrentEnumeratorOnReset = true, TypeHandlingUtils.TypeCloner<T> cloner = null)
			: base(dataCollection, haltCurrentEnumeratorOnReset, cloner)
		{}
		#endregion // Constructors
		#region Methods
		/// <summary>
		/// <see cref="IPAFResettableEnumerableProvider{T}"/>.
		/// the data. This method sorts data locally, then calls base to load it.
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
				// TODO KRM build exception.
				throw new ArgumentException("dataArray must be finite");

			// Get out of the enumerble and into a finite collection for our work.
			// ReSharper disable once PossibleMultipleEnumeration
			//// Resharper is confused......
			var col = dataArray.IntoCollection();
			
			// Sort it.
			col = col.AlphabetizeMethodInfos();

			// Call base to load it safely.
			base.SetData(col);
		}
		#endregion Methods

		/// <summary>
		/// To complete implementation of ITestElementInfoItemResettableEnumerableProvider{T}.
		/// </summary>
		/// <param name="enumerable">enumerable to be set.</param>
		public virtual void SetEnumerable(IEnumerable<T> enumerable)
		{
			SetData(enumerable);
		}
	}
}