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

namespace PlatformAgileFramework.MultiProcessing.Threading.Attributes
{
	#region Enums
	/// <summary>
	/// Specifies what members are synchronized in terms of their visibility.
	/// </summary>
	/// <history>
	/// <author> DAP </author>
	/// <date> 09sep2011 </date>
	/// <contribution>
	/// <para>
	/// Added history.
	/// </para>
	/// <para>
	/// Took explicit interface visibility stuff out and put into Multiprocessing.
	/// Alternatively stated, refactored and split up the model with regular
	/// visibility stuff here. For reflection-based access, private members are
	/// now always expected to be unsynchronized unless attributed. The model still
	/// includes internal methods.
	/// </para>
	/// </contribution>
	/// </history>
	[Flags]
	public enum PAFSynchronizedVisibilityType
	{
		/// <summary>
		/// Indicates that this type is not synchronized overall, but individual
		/// members may be. Valid when applied to class, struct or interface.
		/// </summary>
		None = 0,
		/// <summary>
		/// Indicates that public members are synchronized.
		/// </summary>
		Public = 1,
		/// <summary>
		/// Indicates that protected members are synchronized.
		/// </summary>
		Protected = 2,
		/// <summary>
		/// Indicates that internal members are synchronized.
		/// </summary>
		Internal = 4,
		/// <summary>
		/// Indicates all members are synchronized.
		/// </summary>
		All = 7
	}
	#endregion // Enums
	/// <summary>
	/// Interface for accessing PAFAttributes.
	/// </summary>
	public interface IPAFSynchronizedAttribute
	{
		#region Properties
		/// <summary>
		/// Specifies the members that are synchronized on the type/member.
		/// </summary>
		PAFSynchronizedVisibilityType SynchronizedVisibilityType { get; }
		#endregion // Fields and Autoproperties
	}
}
