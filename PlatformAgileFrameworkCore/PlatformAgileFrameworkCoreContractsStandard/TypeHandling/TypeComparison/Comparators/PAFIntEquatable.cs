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

namespace PlatformAgileFramework.TypeHandling.TypeComparison.Comparators
{
	/// <summary>
	/// Comparator for ints with an "epsilon".
	/// </summary>
	/// <history>
	/// <author> BMC </author>
	/// <date> 02jun2017 </date>
	/// <contribution>
	/// <description>
	/// New. Rework/reformat of the <see cref="IComparable{T}"/> helpers.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Safe.
	/// </threadsafety>
	public class PAFIntEquatable : PAFEquatable<int>, IPAFIntEquatable
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Sensible thing is a zero epsilon.
		/// </summary>
		protected internal int m_LocalEpsilon;
		#endregion // Fields and Autoproperties
		#region Constructors

		/// <summary>
		/// Default constructor causes the default epsilon to be used.
		/// </summary>
		public PAFIntEquatable() { }

		/// <summary>
		/// Constructor allows a custom epsilon to be used.
		/// </summary>
		/// <param name="epsilon">The custom epsilon.</param>
		public PAFIntEquatable(int epsilon) : this()
		{
			m_LocalEpsilon = epsilon;
		}
		#endregion // Constructors
		#region Methods
		/// <summary>
		/// Checks if two <see cref="int"/> values are different.
		/// </summary>
		/// <param name="value">
		/// First value to be compared.
		/// </param>
		/// <param name="otherValue">
		/// Other value to be compared.
		/// </param>
		/// <returns>
		/// True if the values are within <see cref="m_LocalEpsilon"/> of each other.
		/// </returns>
		/// <threadsafety>
		/// Safe.
		/// </threadsafety>
		public override bool AreEqual(int value, int otherValue)
		{
			if (Math.Abs(value - otherValue) < m_LocalEpsilon)
				return true;
			return false;
		}

		/// <summary>
		/// <see cref="IPAFIntEquatable"/>.
		/// </summary>
		/// <param name="value">
		/// <see cref="IPAFIntEquatable"/>.
		/// </param>
		/// <param name="otherValue">
		/// <see cref="IPAFIntEquatable"/>.
		/// </param>
		/// <param name="epsilon">
		/// <see cref="IPAFIntEquatable"/>.
		/// </param>
		/// <returns>
		/// <see cref="IPAFIntEquatable"/>.
		/// </returns>
		/// <threadsafety>
		/// Safe.
		/// </threadsafety>
		public virtual bool AreEqual(int value, int otherValue, int epsilon)
		{
			if (Math.Abs(value - otherValue) < epsilon)
				return true;
			return false;
		}
		#endregion // Methods
	}
}
