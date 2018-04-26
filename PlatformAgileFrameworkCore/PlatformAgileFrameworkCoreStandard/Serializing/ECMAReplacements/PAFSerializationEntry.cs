//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 Icucom Corporation
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

#region Using directives

using System;
using PlatformAgileFramework.Collections;

#endregion

namespace PlatformAgileFramework.Serializing.ECMAReplacements
{
	/// <summary>
	/// An immutable class for objects identified by Type and/or name.
	/// Implements <see cref="IPAFNamedObject"/> for use in a dictionary.
	/// Designed as a replacement for the classical
	/// "System.Runtime.Serialization.SerializationEntry".
	/// </summary>
	public sealed class PAFSerializationEntry: PAFNamedAndTypedObject, IPAFSerializationEntry
	{
		/// <summary>
		/// See <see cref="PAFNamedAndTypedObject(string, Type, Object)"/>.
		/// </summary>
		/// <param name="objectName">
		/// See <see cref="PAFNamedAndTypedObject(string, Type, Object)"/>.
		/// This argument may not be <see langword="null"/> or empty in this class.
		/// </param>
		/// <param name="objectType">
		/// See <see cref="PAFNamedAndTypedObject(string, Type, Object)"/>.
		/// </param>
		/// <param name="objectValue">
		/// See <see cref="PAFNamedAndTypedObject(string, Type, Object)"/>.
		/// </param>
		/// <remarks>
		/// Noted args are switched to match "System.Runtime.Serialization.SerializationEntry"
		/// constructor.
		/// </remarks>
		/// <exception>
		/// <see cref="ArgumentNullException"/> is thrown if <paramref name="objectName"/>
		/// is <see langword="null"/> or empty.
		/// "objectName"
		/// </exception>
		public PAFSerializationEntry(String objectName, Object objectValue, Type objectType )
			:base(objectName, objectType, objectValue)
		{
			if (String.IsNullOrEmpty(objectName)) {
				throw new ArgumentNullException("objectName");
			}
		}
	}
}
