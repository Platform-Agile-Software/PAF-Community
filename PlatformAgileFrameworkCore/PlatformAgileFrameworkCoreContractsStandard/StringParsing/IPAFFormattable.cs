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

namespace PlatformAgileFramework.StringParsing
{
	/// <summary>
	/// This interface provides methods for formatting objects into string format.
	/// </summary>
	/// <history>
	/// <author> BMC </author>
	/// <date> 22apr2011 </date>
	/// <contribution>
	/// Added history when moved to Core.
	/// Added additional methods to enable use of defaults.
	/// </contribution>
	/// </history>
	public interface IPAFFormattable
	{
		/// <summary>
		/// Formats the value of the current implementing instance instance using
		/// the specified format.
		/// </summary>
		/// <param name="format">
		/// The <see cref="String"/> specifying the format to use. Can be 
		/// <see langword="null"/>. If so, the implementing instance will
		/// normally provide a default format.
		/// </param>
		/// <param name="formatProvider">
		/// The <see cref="IPAFFormatProvider"/> to use to format the value.
		/// Can be <see langword="null"/>. If so, the implementing instance will
		/// normally provide a default provider.
		/// </param>
		/// <returns>
		/// A <see cref="String"/> containing a string representation of the
		/// implementation instance in the specified format. Never <see langword="null"/>.
		/// Implementors should return <see cref="Object.ToString()"/> if nothing
		/// else can be provided.
		/// </returns>
		/// <remarks>
		/// Not implemented with default params so implementors may utilize
		/// explicit inteface implementation.
		/// </remarks>
		string FormatToString(string format, IPAFFormatProvider formatProvider);
		/// <remarks>
		/// See main method.
		/// </remarks>
		string FormatToString(string format);
		/// <remarks>
		/// See main method.
		/// </remarks>
		string FormatToString();
	}
}
