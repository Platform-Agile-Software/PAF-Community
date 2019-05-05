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
using System.Reflection;

namespace PlatformAgileFramework.TypeHandling
{
	/// <summary>
	/// Class that wraps a constructor info and optional FIXED parameters
	/// for use as a copyer.
	/// </summary>
	/// <typeparam name="T">
	/// Type of the object to wrap.
	/// </typeparam>
	public class ConstructorCopyer<T>
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Construction params.
		/// </summary>
		protected readonly List<object> m_Parameters = new List<object>();
		/// <summary>
		/// Info.
		/// </summary>
		protected ConstructorInfo m_ConstructorInfo;
		#endregion // Class Fields and Autoproperties
		/// <summary>
		/// Allows a constructor along with additional FIXED parameters to be specified.
		/// The class always takes the object of type <typeparamref name="T"/> in its
		/// copy method, but will also utilize the additional parameters, if specified.
		/// </summary>
		/// <param name="constructorInfo">
		/// Constructor which must take a number of parameters equal to the number of
		/// incoming parameters plus one.
		/// </param>
		/// <param name="parameters">
		/// Constructor parameters which are input to the constructor after the
		/// first parameter, which is the object of type <typeparamref name="T"/>.
		/// Can be <see langword="null"/>, in which case a single argument copy constructor
		/// is expected.
		/// </param>
		public ConstructorCopyer(ConstructorInfo constructorInfo,
			IEnumerable<object> parameters = null)
		{
			m_ConstructorInfo = constructorInfo;

			if(parameters != null)
				m_Parameters.AddRange(parameters);

			// Make a hole for the item to copy.
			m_Parameters.Insert(0, null);
		}
		/// <summary>
		/// Provides a copy through a copy constructor.
		/// </summary>
		/// <returns>
		/// A copy of <typeparamref name="T"/> or "default(T)" if the input
		/// is "default(T)".
		/// </returns>
		/// <exceptions>
		/// <see cref="NotSupportedException"/> if <typeparamref name="T"/> does not
		/// have a copy constructor.
		/// </exceptions>
		public T ConstructedCopy(T itemToCopy)
		{
			if (itemToCopy.Equals(default(T)))
				return default(T);
			// Always stick the item to copy into the first position.
			m_Parameters[0] = itemToCopy;
			return (T) m_ConstructorInfo.Invoke(m_Parameters.ToArray());
		}
	}
}