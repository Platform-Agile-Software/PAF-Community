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

namespace PlatformAgileFramework.XML.Namespace
{
	/// <summary>
	/// Interface that allows manipulating a type with its XML
	/// namespace. This is the internal version that allows
	/// setting things and allows access to the ECMA namespace.
	/// </summary>
	/// <remarks>
	/// The interface can be worn by different types on different sides of
	/// a remote boundary and the whole thing is useful for translating to
	/// proxy types, deferred types, etc.
	/// </remarks>
	internal interface IXMLNamedTypeInternal : IXMLNamedType
	{
		#region Properties
		/// <summary>
		/// The ECMA namespace, which CAN be be <see langword="null"/> or blank. Note that
		/// the ECMA namespace can contain other data, including assembly info.
		/// </summary>
		string ECMANamespace { get; set; }
		#endregion // Properties

		#region Methods
		/// <summary>
		/// Sets the name, which can never be <see langword="null"/> or blank.
		/// </summary>
		/// <param name="name">
		/// The name of the serialized/deserialized type.</param>
		void SetName(string name);
		/// <summary>
		/// Sets the XML name space, which CAN be <see langword="null"/> or blank.
		/// </summary>
		/// <param name="xMLNamespace">
		/// The XML name of the serialized/deserialized type.</param>
		void SetXMLNamespace(string xMLNamespace);
		#endregion // Methods
	}
}
