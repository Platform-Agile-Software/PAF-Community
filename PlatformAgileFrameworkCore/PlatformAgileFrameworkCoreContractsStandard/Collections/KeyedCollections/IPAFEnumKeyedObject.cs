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

namespace PlatformAgileFramework.Collections.KeyedCollections
{
	/// <summary>
	/// An interface for objects identified by an <see cref="Enum"/>.
	/// </summary>
	/// <remarks>
	/// Note that <see cref="Enum"/> - indexed objects can be sorted either
	/// by integer value or by corresponding textual name. Use the Enum
	/// helpers to manipulate enums to get names. This can't be a constrained
	/// Generic interface because CLI doesn't allow constraint to be an
	/// <see cref="Enum"/>.
	/// </remarks>
	public interface IPAFEnumKeyedObject
	{
		/// <summary>
		/// This method returns a key of the item.
		/// </summary>
		/// <returns>
		/// An <see cref="Enum"/> value.
		/// </returns>
		Enum GetItemEnumKey();
	}
}
