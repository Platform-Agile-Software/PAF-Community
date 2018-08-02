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
using System.Security;

#endregion

namespace PlatformAgileFramework.Collections
{
	/// <summary>
	/// This one is an interface to manipulate the named objects in collections. This
	/// adds the notion of a "default" object in a collection.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> DAP </author>
	/// <date> 04jan2012 </date>
	/// <description>
	/// Documented.
	/// Changed to a public interface. 4.0 security allows us to expose it
	/// and simply leave the methods security critical. We don't need an
	/// internal interface, since the implementations are used only in
	/// framework libs. If somebody complains, we'll add the internal stuff.
	/// </description>
	/// </contribution>
	/// </history>
	public interface IPAFNamedObject
	{
		/// <summary>
		/// Manipulates the name of the object.
		/// </summary>
		/// <remarks>
		/// Typically name can be blank, indicating a default object for
		/// the type. The name should never be <see langword="null"/>.
		/// </remarks>
		string ObjectName
		{ get;
			//[SecurityCritical]
			set; }
		/// <summary>
		/// Manipulates the value of the object.
		/// </summary>
		object ObjectValue
		{ get;
			//[SecurityCritical]
			set; }
		/// <summary>
		/// Manipulates the default flag. This identifies the "default" object
		/// in a collection. Overrides the "blank" item in a collection being the default.
		/// </summary>
		bool IsDefaultObject
		{ get;
			//[SecurityCritical]
			set; }
	}
}
