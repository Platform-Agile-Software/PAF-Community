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
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.MultiProcessing.Threading.Attributes
{
	/// <summary>
	/// Attribute placed on types/members to signal whether they are synchronized.
	/// See <see cref="IPAFSynchronizedAttribute"/>.
	/// </summary>
	/// <threadsafety>
	/// Thread-safe.
	/// </threadsafety>
// ReSharper disable PartialTypeWithSinglePart
	public partial class PAFSynchronizedAttributeBase : Attribute, IPAFSynchronizedAttribute
// ReSharper restore PartialTypeWithSinglePart
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Backing for prop/method.
		/// </summary>
		/// <remarks>
		/// Private visibility since we don't expect the use of a surrogate.
		/// </remarks>
		private PAFSynchronizedVisibilityType m_SynchronizedVisibilityType;
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Default constructor for interitance and framework support.
		/// </summary>
		protected internal PAFSynchronizedAttributeBase() { }
		/// <summary>
		/// Main constructor for the attribute.
		/// </summary>
		/// <param name="synchronizedVisibilityType">
		/// See <see cref="IPAFSerializableAttribute"/>.
		/// </param>
		public PAFSynchronizedAttributeBase(PAFSynchronizedVisibilityType synchronizedVisibilityType)
		{
			// TODO KRM bad type exception for enum = none.
			m_SynchronizedVisibilityType = synchronizedVisibilityType;
		}
		#endregion // Constructors
		#region Properties
		/// <summary>
		/// See <see cref="IPAFSynchronizedAttribute"/>.
		/// </summary>
// ReSharper disable ConvertToAutoProperty
// ReSharper error - shouldn't flag this when we need non-virtual access through the field.
		public virtual PAFSynchronizedVisibilityType SynchronizedVisibilityType
// ReSharper restore ConvertToAutoProperty
		{ get { return m_SynchronizedVisibilityType; } }
		#endregion // Properties
	}
}
