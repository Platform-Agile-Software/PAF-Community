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

namespace PlatformAgileFramework.TypeHandling
{
	/// <summary>
	/// <para>
	///	Simple container interface to hold just certain props for a "PAFTypeHolder".
	/// </para>
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 30nov2012 </date>
	/// <description>
	/// New.
	/// Needed just a small container to export in the contracts assy. This gives
	/// enough info for remote services, but doesn't reveal anything about our
	/// internals.
	/// </description>
	/// </contribution>
	/// </history>
	public interface IPAFTypeProps
	{
		#region Properties
		/// <summary>
		/// Holds the full type name.
		/// </summary>
		string AssemblyQualifiedTypeName { get; }
		/// <summary>
		/// Holds the type, if available. Will return <see langword="null"/> if the type
		/// is not local or has not been resolved. This property is never serialized.
		/// </summary>
		Type TypeType { get; }
		#endregion // Properties
	}
}