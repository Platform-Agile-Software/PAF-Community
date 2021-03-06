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
using PlatformAgileFramework.TypeHandling.BasicValueTypes;

namespace PlatformAgileFramework.TypeHandling.TypeComparison.Comparators
{
	/// <summary>
	/// Comparator for complex doubles with an "epsilon". This implementation determines
	/// if both the real and imaginary components are within an "epsilon of each  other.
	/// Epsilons CAN be different for the two components. Many other implementations can
	/// be found in the signal processing library. 
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
	public class PAFComplexDoubleEquatable : PAFEquatable<PAFComplexDoubleNumber>, IPAFComplexDoubleEquatable
	{
		#region Fields and Autoproperties
		/// <summary>
		/// This will carry the allowed roundoff error and is initialized to our
		/// internal default. Good for typical FFT roundoff and the like.
		/// </summary>
		protected internal double m_LocalEpsilon = PAFMath.s_TinyPositiveDouble;
		/// <summary>
		/// This will be delegated to if present (along with the imaginary version) to
		/// make the comparison.
		/// </summary>
		protected internal IPAFDoubleEquatable m_RealDoubleEquatable;
		/// <summary>
		/// This will be delegated to if present (along with the real version) to
		/// make the comparison.
		/// </summary>
		protected internal IPAFDoubleEquatable m_ImaginaryDoubleEquatable;
		#endregion // Fields and Autoproperties
		#region Constructors

		/// <summary>
		/// Default constructor causes the default epsilon to be used.
		/// </summary>
		public PAFComplexDoubleEquatable() { }
		/// <summary>
		/// Constructor allows a custom epsilon to be used.
		/// </summary>
		/// <param name="epsilon">The custom epsilon.</param>
		public PAFComplexDoubleEquatable(double epsilon) : this()
		{
			m_LocalEpsilon = epsilon;
		}

		/// <summary>
		/// Constructor allows a custom epsilon to be used and
		/// component comparators.
		/// </summary>
		/// <param name="realEquatable">Not <see langword="null"/>.</param>
		/// <param name="imaginaryEquatable">Not <see langword="null"/>.</param>
		/// <param name="epsilon">The custom epsilon.</param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">"realEquatable"</exception>
		/// <exception cref="ArgumentNullException">"imaginaryEquatable"</exception>
		/// </exceptions>
		public PAFComplexDoubleEquatable(IPAFDoubleEquatable realEquatable, IPAFDoubleEquatable imaginaryEquatable, double epsilon)
			: this(epsilon)
		{
			if (realEquatable == null)
				throw new ArgumentNullException("realEquatable");
			if (imaginaryEquatable == null)
				throw new ArgumentNullException("imaginaryEquatable");
			m_RealDoubleEquatable = realEquatable;
			m_ImaginaryDoubleEquatable = imaginaryEquatable;
		}
		#endregion // Constructors
		#region Methods
		/// <summary>
		/// Checks if two <see cref="PAFComplexDoubleNumber"/> values are different.
		/// It does so by thresholding each component individually against the
		/// internal epsilon. It uses the plugin component comparators if they are
		/// there.
		/// </summary>
		/// <param name="value">
		/// <see cref="IPAFEquatable{T}"/>.
		/// </param>
		/// <param name="otherValue">
		/// <see cref="IPAFEquatable{T}"/>.
		/// </param>
		/// <returns>
		/// True if the values are within <see cref="m_LocalEpsilon"/> of each other.
		/// </returns>
		/// <threadsafety>
		/// Safe.
		/// </threadsafety>
		public override bool AreEqual(PAFComplexDoubleNumber value, PAFComplexDoubleNumber otherValue)
		{
			if (m_RealDoubleEquatable != null)
			{
				if (!m_RealDoubleEquatable.AreEqual(value.RealPart, otherValue.RealPart))
					return false;
				if (!m_ImaginaryDoubleEquatable.AreEqual(value.ImaginaryPart, otherValue.ImaginaryPart))
					return false;
				return true;
			}

			return AreEqual(value, otherValue, m_LocalEpsilon);
		}

		/// <summary>
		/// <see cref="IPAFComplexDoubleEquatable"/>. This implementation returns
		/// <see langword="false"/> if either the real components or the imaginary
		/// components are not within <paramref name="epsilon"/> of each other. 
		/// </summary>
		/// <param name="value">
		/// <see cref="IPAFComplexDoubleEquatable"/>.
		/// </param>
		/// <param name="otherValue">
		/// <see cref="IPAFComplexDoubleEquatable"/>.
		/// </param>
		/// <param name="epsilon">
		/// <see cref="IPAFComplexDoubleEquatable"/>.
		/// </param>
		/// <returns>
		/// <see cref="IPAFComplexDoubleEquatable"/>.
		/// </returns>
		/// <threadsafety>
		/// Safe.
		/// </threadsafety>
		public virtual bool AreEqual(PAFComplexDoubleNumber value, PAFComplexDoubleNumber otherValue, double epsilon)
		{
			if (Math.Abs(value.RealPart - otherValue.RealPart) > epsilon)
				return false;
			if (Math.Abs(value.ImaginaryPart - otherValue.ImaginaryPart) > epsilon)
				return false;
			return true;
		}
		#endregion // Methods
	}
}
