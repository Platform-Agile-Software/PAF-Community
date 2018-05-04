//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2016 Icucom Corporation
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

namespace PlatformAgileFramework.Collections.Enumerators
{	/// <summary>
	/// <para>
	/// Please see <see cref="IPAFResettableEnumerableProvider{T}"/> for the rationale
	/// of the resettable enumeration interfaces. This interface describes reset
	/// functionality that is common to both resettanle enumerators and resettable
	/// enumerables.
	/// </para>
	/// <para>
	/// This interface allows an enumeration to be reset, so it can be handed
	/// out over and over again after reset, possibly with different data. It is useful
	/// in cases where enumerations are costly to initially produce. It can be used to
	/// extend either an <see cref="IEnumerable{T}"/> or a <see cref="IEnumerator{T}"/>,
	/// among other things.
	/// </para>
	/// </summary>
	/// <typeparam name="T">Type that is to be enumerated.</typeparam>
	/// <threadsafety>
	/// Implementations need not necessarily be thread-safe.
	/// </threadsafety>
	/// <history>
	/// <author> KRM </author>
	/// <date> 15oct2012 </date>
	/// <contribution>
	/// <para>
	/// Build this so I could refactor "PAFResettableEnumerableBase{T}".
	/// </para>
	/// </contribution>
	/// </history>
	public interface IPAFResettableEnumeration<in T>
	{
		#region Properties
		/// <summary>
		/// Tells whether this enumeration is finite. Note that even if the
		/// input enumerable in the <see cref="SetData"/> method, the output
		/// enumeration may be infinite. This would be the case if the enumerator
		/// picked elements out of a finite list at random, forever,
		/// until the foreach loop exited (and dispose was called). On the
		/// other hand, if the input enumerator was infinite, the
		/// enumerator would not have to be, if only a finite number of values
		/// were used.
		/// </summary>
		/// <remarks>
		/// The nullable object is needed, since implementations may be loaded
		/// from unknown sources, which may be indeterminate as to finite-ness.
		/// </remarks>
		bool? IsEnumerationFinite { get; }
		#endregion // Properties
		#region Methods
		/// <summary>
		/// Resettable enumerator/enumerable's <see cref="IDisposable.Dispose"/> method
		/// should not NECESSARILY dispose the resources of the type, since it may simply be reset
		/// and used again. It's up to the implementation to determine when the type is
		/// actually destroyed. When it is time, this method should be called.
		/// </summary>
		void DisposeEnumeration();
		/// <summary>
		/// Sets the data. Provides the capability to reset and refresh the internal data.
		/// Implementations can use the incoming data or not. It's provided in case the
		/// implementor wishes to reset the state of the enumerable with external data.
		/// </summary>
		/// <param name="dataEnumerable">
		/// <para>
		/// The incoming data. Can be <see langword="null"/>. Depending on the implementation, the
		/// incoming enumerable may not be finite, so it should never be loaded into
		/// a container of any kind, unless only a finite number of elements are selected.
		/// Neither should extension methods assuming a finite enumeration be employed.
		/// </para>
		/// <para>
		/// Note this very same parameter is utilized on <see cref="IPAFResettableEnumerableProvider{T}"/>
		/// and <see cref="IPAFResettableEnumerable{T}"/>.
		/// An implementation of <see cref="IPAFResettableEnumerableProvider{T}"/> can inspect
		/// its inner enumerable to determine if it is an <see cref="IPAFResettableEnumerator{T}"/>
		/// and call our reset method directly.
		/// </para>
		/// </param>
		void SetData(IEnumerable<T> dataEnumerable);
		#endregion Methods
	}
}