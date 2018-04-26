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
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System;
using System.Collections.Generic;

namespace PlatformAgileFramework.TypeHandling.TypeComparison.Comparators
{
	/// <summary>
	/// Comparator for types. This class is designed to be used, by default
	/// as a singleton. However, in our work, we need to construct a comparator
	/// with parameters sometimes, so the constructor is not private.
	/// </summary>
	/// <history>
	/// <author> BMC </author>
	/// <date> 02jun2017 </date>
	/// <contribution>
	/// <description>
	/// New. Rework/reformat of the <see cref="IComparable{T}"/> helpers.
	/// Microsoft cr*p <see cref="EqualityComparer{T}"/> has virtual
	/// internals, so we can't specialize/extend that. 
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Safe.
	/// </threadsafety>
	public class PAFEquatable<T> : PAFEquatable,  IPAFEquatable<T>
	{
		#region Fields and Autoprops
		/// <summary>
		/// This a thread-safe wrapper for constructing the singleton.
		/// </summary>
		/// <remarks>
		/// Lazy class is thread-safe by default.
		/// </remarks>
		private static readonly Lazy<PAFEquatable<T>> s_Singleton =
			new Lazy<PAFEquatable<T>>(ConstructPAFEquatable);

		#endregion // Fields and Autoprops
		#region Constructors
		/// <summary>
		/// For the singleton.
		/// </summary>
		protected PAFEquatable()
		{
		}

		#endregion // Constructors
		#region Properties
		/// <summary>
		/// Get the singleton instance of the class.
		/// </summary>
		/// <returns>The singleton.</returns>
		public static PAFEquatable<T> Instance
		{
			get { return s_Singleton.Value; }
		}

		#endregion // Properties
		#region Methods
		/// <summary>
		/// Not quite a constructor - a factory for the lazy construction.
		/// </summary>
		private static PAFEquatable<T> ConstructPAFEquatable()
		{
			return new PAFEquatable<T>();
		}
		/// <summary>
		/// Checks if two values of an arbitrary Generic are different.
		/// </summary>
		/// <typeparam name="T">Type of the value.</typeparam>
		/// <param name="value">
		/// First value to be compared.
		/// </param>
		/// <param name="otherValue">
		/// Other value to be compared.
		/// </param>
		/// <returns>
		/// True if the values are equal, ACCORDING TO THE SPECIFIC IMPLEMENTATION.
		/// </returns>
		/// <threadsafety>
		/// Safe.
		/// </threadsafety>
		public virtual bool AreEqual(T value, T otherValue)
		{
			if (EqualityComparer<T>.Default.Equals(value, otherValue)) return true;
			return false;
		}
		#endregion // Methods

	}
}
