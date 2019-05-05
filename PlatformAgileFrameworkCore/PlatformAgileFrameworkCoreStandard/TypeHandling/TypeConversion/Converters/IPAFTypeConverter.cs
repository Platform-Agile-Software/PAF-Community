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

namespace PlatformAgileFramework.TypeHandling.TypeConversion.Converters
{
	/// <summary>
	/// Interface that replaces basic "TypeConverter" functionality in core.
	/// </summary>
	public interface IPAFTypeConverter
	{
		#region Properties
		/// <summary>
		/// Access to the format provider for string conversions.
		/// </summary>
		IFormatProvider FormatProvider {get; set;}
		#endregion // Properties
		#region Methods
		/// <summary>
		/// Determines if the value can be converted from another type.
		/// </summary>
		/// <param name="sourceType">Type of the source object.</param>
		/// <returns>
		/// Returns <see langword = "false"/> if the source is not convertable to the
		/// type desired.
		/// </returns>
		bool CanConvertFrom(Type sourceType);

		/// <summary>
		/// Determines if the type can be converted to a specified type.
		/// </summary>
		/// <param name="destinationType">The type to convert to.</param>
		/// <returns>
		/// Returns <see langword = "false"/> if we can't convert to the destination type.
		/// </returns>
		bool CanConvertTo(Type destinationType);

		/// <summary>
		/// This method converts from an incoming type to our type.
		/// </summary>
		/// <param name="value">Value to be converted.</param>
		/// <returns>
		/// The <see cref="object"/> if conversion is successful, <see langword="null"/>
		/// otherwise. No exceptions are generated.
		/// </returns>
		object ConvertFrom(object value);

		/// <summary>
		/// This method converts from an incoming string to our type.
		/// </summary>
		/// <param name="stringValue">String value to be converted.</param>
		/// <returns>
		/// The <see cref="object"/> if conversion is successful, <see langword="null"/>
		/// otherwise. No exceptions are generated.
		/// </returns>
		object ConvertFromString(string stringValue);

		/// <summary>
		/// This version attempts to convert the <see cref="value"/> to a value of our type.
		/// </summary>
		/// <param name="value"> Value to be converted.
		/// </param>
		/// <param name="destinationType">
		/// Type to convert to.
		/// </param>
		/// <returns>
		/// <see langword="null"/> for creation failure. No exceptions are thrown.
		/// </returns>
		object ConvertTo(object value, Type destinationType);
		#endregion // Methods
	}
}
