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

#region Using directives

using System;
using System.Security;

#endregion

namespace PlatformAgileFramework.Collections
{
	/// <summary>
	/// An interface for objects identified by Type and/or name.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> DAP </author>
	/// <date> 02jan2012 </date>
	/// <description>
	/// New.
	/// </description>
	/// </contribution>
	/// </history>
	public interface IPAFNamedAndTypedObject : IPAFNamedObject
	{
		/// <summary>
		/// Manipulates the type of the object. String version for remoting.
		/// </summary>
		/// <exceptions>
		/// <exception cref="InvalidOperationException">
		/// "Type is already resolved" if <see cref="ObjectType"/> is not
		/// <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		string AssemblyQualifiedObjectType
		{
			get;
			[SecurityCritical]
			set;
		}
		/// <summary>
		/// Gets the type of the object. Any object should usually match the
		/// type. There are some extreme cases where the type can be reset to
		/// a type that the object inherits from or implements. This is not
		/// a good idea and the setter is left here for legacy code support.
		/// </summary>
		Type ObjectType
		{
			get;
			[SecurityCritical]
			set;
		}
	}
}
