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

namespace PlatformAgileFramework.StringParsing
{
	/// <summary>
	/// Provides a mechanism for retrieving an object to control formatting.
	/// </summary>
	/// <history>
	/// <author> BMC </author>
	/// <date> 22apr2011 </date>
	/// <contribution>
	/// Added comments when moved to Core.
	/// </contribution>
	/// </history>
	// TODO - KRM - why didn't we move the generic over here? This doesn't do us
	// TODO much good in most cases.
	public interface IPAFFormatProvider
	{
		/// <summary>
		/// Returns an object that provides formatting services for the specified type.
		/// </summary> 
		/// <param name="formatType">
		/// A <see cref="Type"/> that specifies the type of format object to return.
		/// The use of this parameter is entirely implementation-dependent. It may
		/// be the type of the object that requires formatting or something else
		/// entirely.
		/// </param>
		/// <returns>
		/// An instance of the object corresponding to <paramref name="formatType"/>,
		/// if the <see cref="IPAFFormatProvider"/> implementation can supply that
		/// type of object; otherwise, null.
		/// </returns>
		object GetFormatService(Type formatType);
		/// <summary>
		/// Returns an object that provides formatting services for the specified instance.
		/// </summary> 
		/// <param name="formatSelector">
		/// An <see cref="object"/> that specifies the type of format object to return.
		/// The use of this parameter is entirely implementation-dependent. It may
		/// be an instance of the type of the object that requires formatting or something
		/// else entirely.
		/// </param>
		/// <returns>
		/// An instance of the object corresponding to <paramref name="formatSelector"/>,
		/// if the <see cref="IPAFFormatProvider"/> implementation can supply that
		/// type of object; otherwise, null.
		/// </returns>
		object GetFormatService(object formatSelector);
	}
}
