//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2014 Icucom Corporation
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
using PlatformAgileFramework.TypeHandling.BasicValueTypes;

namespace PlatformAgileFramework.TypeHandling.TypeExtensionMethods.NumericExtensions
{
	/// <summary>
	/// <para>
	/// This class has a set of extension methods for some simple numeric types,
	/// mostly to serve signal processing needs.
	/// </para>
	/// </summary>
	/// <history>
	/// <author> BMC </author>
	/// <date> 02jun2017 </date>
	/// <contribution>
	/// <description>
	/// New. Built along the way for the rework/reformat of the
	/// <see cref="IComparable{T}"/> helpers.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Safe.
	/// </threadsafety>
	public static class SimpleNumericTypeExtensions
	{
		/// <summary>
		/// Finds the magnitude of a <see cref="PAFComplexDoubleNumber"/>.
		/// </summary>
		/// <param name="complexDoubleNumber">
		/// Number whose magnitude is to be found.
		/// </param>
		/// <returns>Square root of squares of components.</returns>
		public static double Magnitude(this PAFComplexDoubleNumber complexDoubleNumber)
		{
			var square = complexDoubleNumber.RealPart * complexDoubleNumber.RealPart;
			square += complexDoubleNumber.ImaginaryPart * complexDoubleNumber.ImaginaryPart;
			return Math.Sqrt(square);
		}

		/// <summary>
		/// Finds the phase of a <see cref="PAFComplexDoubleNumber"/>.
		/// </summary>
		/// <param name="complexDoubleNumber">
		/// Number whose phase is to be found.
		/// </param>
		/// <returns>ATan of the components if they are both non-zero, 0.0 otherwise.</returns>
		public static double Phase(this PAFComplexDoubleNumber complexDoubleNumber)
		{
			//// We know what we're doing, ReSharper...
			// ReSharper disable CompareOfFloatsByEqualityOperator
			if (complexDoubleNumber.RealPart == 0 || complexDoubleNumber.ImaginaryPart == 0)
				// ReSharper restore CompareOfFloatsByEqualityOperator
				return 0.0;
			var phase = Math.Atan2(complexDoubleNumber.RealPart, complexDoubleNumber.ImaginaryPart);
			return phase;
		}
		/// <summary>
		/// Finds the magnitude of a <see cref="PAFComplexFloatNumber"/>.
		/// </summary>
		/// <param name="complexFloatNumber">
		/// Number whose magnitude is to be found.
		/// </param>
		/// <returns>Square root of squares of components.</returns>
		/// <remarks>
		/// We reconvert a double result to a float so we are consistently
		/// doing floating arithmetic.
		/// </remarks>
		public static float Magnitude(this PAFComplexFloatNumber complexFloatNumber)
		{
			var square = complexFloatNumber.RealPart * complexFloatNumber.RealPart;
			square += complexFloatNumber.ImaginaryPart * complexFloatNumber.ImaginaryPart;
			return (float) Math.Sqrt(square);
		}

		/// <summary>
		/// Finds the phase of a <see cref="PAFComplexFloatNumber"/>.
		/// </summary>
		/// <param name="complexFloatNumber">
		/// Number whose phase is to be found.
		/// </param>
		/// <returns>ATan of the components if they are both non-zero, 0.0 otherwise.</returns>
		/// <remarks>
		/// We reconvert a double result to a float so we are consistently
		/// doing floating arithmetic.
		/// </remarks>
		public static float Phase(this PAFComplexFloatNumber complexFloatNumber)
		{
			//// We know what we're doing, ReSharper...
			// ReSharper disable CompareOfFloatsByEqualityOperator
			if (complexFloatNumber.RealPart == 0 || complexFloatNumber.ImaginaryPart == 0)
				// ReSharper restore CompareOfFloatsByEqualityOperator
				return 0.0F;
			var phase = Math.Atan2(complexFloatNumber.RealPart, complexFloatNumber.ImaginaryPart);
			return (float) phase;
		}
	}
}
