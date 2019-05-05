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

namespace PlatformAgileFramework.MultiProcessing.Threading.Attributes
{
	/// <summary>
	/// Attribute placed on types/members to signal the type of serialization in use.
	/// Note that the type of serialization in use by a class may be overridden on a
	/// member-by-member basis.
	/// </summary>
	/// <remarks>
	/// Noted that the attribute is not inherited so it must be repeated on any subclasses.
	/// Multiples are allowed, since partial class parts may have different synchronization.
	/// </remarks>
	[PAFSynchronized(PAFSynchronizedVisibilityType.All)]
	[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class | AttributeTargets.Interface
		| AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	public sealed class PAFSynchronizedAttribute : PAFSynchronizedAttributeBase
	{
		/// <summary>
		/// See base class.
		/// </summary>
		/// <param name="synchronizedVisibilityType">
		/// See base class.
		/// </param>
		public PAFSynchronizedAttribute(PAFSynchronizedVisibilityType synchronizedVisibilityType)
			:base(synchronizedVisibilityType)
		{
		}

	}
}
