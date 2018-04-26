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

#region Using directives

using System;

#endregion

namespace PlatformAgileFramework.Collections
{
	/// <summary>
	/// An interface for objects identified by name. Internal version.
	/// See <see cref="IPAFNamedObject"/>.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> DAP </author>
	/// <date> 22jan2012 </date>
	/// <description>
	/// Built because somebody said they needed it.
	/// </description>
	/// </contribution>
	/// </history>
	internal interface IPAFNamedObjectInternal : IPAFNamedObject
	{
		/// <summary>
		/// Sets the default flag after the type is constructed.
		/// </summary>
		/// <param name="isDefaultObject">
		/// The setting to be applied to the flag.
		/// </param>
		void SetIsDefault(bool isDefaultObject);
		/// <summary>
		/// Sets the name of the object.
		/// </summary>
		void SetObjectName(string name);
		/// <summary>
		/// Sets the value of the object.
		/// </summary>
		void SetObjectValue(object objectValue);
	}
}
