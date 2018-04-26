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
using PlatformAgileFramework.Collections.Comparers;

namespace PlatformAgileFramework.Collections.Dictionaries
{
	/// <summary>
	/// See <see cref="IPAFNamedAndTypedObjectDictionary{T}"/>
	/// </summary>
	/// <threadsafety>
	/// This class subclasses an ordinary dictionary and is thus not thread-safe.
	/// </threadsafety>
	public class PAFNamedAndTypedObjectDictionary<T> : Dictionary<IPAFNamedAndTypedObject, T>,
		IPAFNamedAndTypedObjectDictionary<T>
	{
		#region Constructors
		/// <summary>
		/// This constructor builds the dictionary with a case-insensitive name
		/// comparison.
		/// </summary>
		/// <param name="ignoreCase">
		/// This parameter tells whether or not to ignore case in name comparisons.
		/// Default is <see langword="false"/>. This parameter is only used when the default
		/// comparer is used (<paramref name="comparer"/> is <see langword="null"/>).
		/// </param>
		/// <param name="comparer">
		/// This parameter allows a <see cref="IEqualityComparer{IPAFNamedAndTypedObject}"/>
		/// to be installed. Default is to use <see cref="PAFNameAndTypeComparer(Boolean)"/>,
		/// where the argument is loaded from <paramref name="ignoreCase"/>.
		/// </param>
		public PAFNamedAndTypedObjectDictionary(bool ignoreCase = false,
			IEqualityComparer<IPAFNamedAndTypedObject> comparer = null)
			: base(comparer ?? new PAFNameAndTypeComparer(ignoreCase))
		{}
		#endregion // Constructors
	}
}
