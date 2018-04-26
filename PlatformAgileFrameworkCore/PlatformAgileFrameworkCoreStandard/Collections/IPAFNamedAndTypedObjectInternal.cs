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
	/// An interface for objects identified by Type and/or name. This interface is
	/// internal, since we normally would like to have immutable implementations
	/// of the public interface. There are cases requiring modification of the
	/// members after construction, however.
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
	internal interface IPAFNamedAndTypedObjectInternal : IPAFNamedAndTypedObject,
		IPAFNamedObjectInternal
	{
		/// <summary>
		/// Sets the type after the implementation is constructed.
		/// </summary>
		/// <param name="objectType">
		/// The type to be set.
		/// </param>
		/// <remarks>
		/// Obsolete - see <see cref="IPAFNamedAndTypedObject"/>.
		/// </remarks>
		[Obsolete("setting after construction is not a good idea")]
		void SetObjectType(Type objectType);
		/// <summary>
		/// Sets the type name after the implementation is constructed.
		/// </summary>
		/// <param name="assemblyQualifiedObjectTypeName">
		/// The type name to be set.
		/// </param>
		/// <exceptions>
		/// <exception cref="InvalidOperationException">
		/// "Type is already resolved" if <see cref="IPAFNamedAndTypedObject.ObjectType"/>
		/// is not <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		void SetAssemblyQualifiedObjectTypeName(string assemblyQualifiedObjectTypeName);
	}
}
