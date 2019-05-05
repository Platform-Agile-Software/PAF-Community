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
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System;
using System.Collections.Generic;

namespace PlatformAgileFramework.Serializing.Interfaces
{
	/// <summary>
	/// <para>
	/// This interface exposes members that allow populating and enumerating
	/// a collection.
	/// </para>
	/// <para>
	/// This interface is designed for an implementation of a collection
	/// containing elements that are serializable. Since serializability
	/// is often denoted by attributes, there is no generic constraint on
	/// this base interface, which must allow implementation by all collections
	/// of serializable elements.
	/// </para>
	/// </summary>
	/// <typeparam name="T">
	/// The type of the item in the collection.
	/// </typeparam>
	/// <threadsafety>
	/// The methods described by this interface are not meant to be used in
	/// a multi-threaded environment.
	/// </threadsafety>
	public interface IPAFSerializableCollection<T>: IEnumerable<T>
	{
		#region Methods
		/// <summary>
		/// Installs a <typeparamref name="T"/> which is assumed to be serializable. This
		/// should will throw an exception if the incoming object is not serializable.
		/// This provides a bit of up-front protection that is better than letting an
		/// exception be thrown deep in the serialization system.
		/// </summary>
		/// <param name="item">The serializable item.</param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> is thrown if the argument
		/// is <see langword="null"/>.
		/// </exception>
		/// <exception cref="ArgumentException"> is thrown if <paramref name="item"/>
		/// is not serializable.
		/// </exception>
		/// </exceptions>
		void Add(T item);
		#endregion // Methods
	}
}
