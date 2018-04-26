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

using System.Collections.Generic;
using System.Collections;

namespace PlatformAgileFramework.TypeHandling
{
	/// <summary>
	/// Class that wraps a single instance of a type into an <see cref="IEnumerable"/>
	/// so it can be displayed in GUI lists, etc.
	/// </summary>
	/// <typeparam name="T">
	/// Type of the object to wrap.
	/// </typeparam>
	public class EnumeratedSingleton<T> : IEnumerable<T>
	{
		readonly List<T> m_List = new List<T>();
		/// <summary>
		/// Just puts the typed object into a list.
		/// </summary>
		/// <param name="singleton">
		/// The type that we want to wrap.
		/// </param>
		public EnumeratedSingleton(T singleton)
		{
			m_List.Add(singleton);
		}
		/// <summary>
		/// See <see cref="IEnumerator{T}"/>.
		/// </summary>
		/// <returns>
		/// See <see cref="IEnumerator{T}"/>.
		/// </returns>
		public IEnumerator<T> GetEnumerator()
		{
			return m_List.GetEnumerator();
		}
		/// <summary>
		/// See <see cref="IEnumerator"/>.
		/// </summary>
		/// <returns>
		/// See <see cref="IEnumerator"/>.
		/// </returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return m_List.GetEnumerator();
		}
	}
}