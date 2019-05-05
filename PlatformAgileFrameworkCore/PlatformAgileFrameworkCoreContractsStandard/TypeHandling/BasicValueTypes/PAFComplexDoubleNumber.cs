//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2018 Icucom Corporation
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

#region Using Directives

#endregion

using System.Runtime.CompilerServices;

namespace PlatformAgileFramework.TypeHandling.BasicValueTypes
{
	/// <summary>
	/// This struct models a complex number with real and imaginary parts.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> BMC </author>
	/// <date> 05jul2012 </date>
	/// <description>
	/// New.
	/// Pulled from my signal processing stuff - there is a need for this in core.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Immutable type.
	/// </threadsafety>
	public struct PAFComplexDoubleNumber
	{
		#region Fields and Autoproperties
		/// <summary>
		/// This is real part of the complex number (the "general" number in Steinmetz-speak).
		/// </summary>
		internal double m_RealPart;
		/// <summary>
		/// This is imaginary part of the complex number (the "general" number in Steinmetz-speak).
		/// </summary>
		internal double m_ImaginaryPart;
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Immutable constructor.
		/// </summary>
		/// <param name="realPart">Sets real part of the number.</param>
		/// <param name="imaginaryPart">Sets imaginary part of the number.</param>
		public PAFComplexDoubleNumber(double realPart, double imaginaryPart)
		:this()
		{
			m_RealPart = realPart;
			m_ImaginaryPart = imaginaryPart;
		}
		#endregion Constructors

		#region Properties
		/// <summary>
		/// Returns the real part.
		/// </summary>
		public double RealPart
		{
			get { return m_RealPart; }
		}
		/// <summary>
		/// Returns the imaginary part.
		/// </summary>
		public double ImaginaryPart
		{
			get { return m_ImaginaryPart; }
		}
		#endregion // Properties
	}
}
