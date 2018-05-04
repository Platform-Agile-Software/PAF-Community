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
using PlatformAgileFramework.TypeHandling;

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.Serializing.Attributes
{
	/// <summary>
	/// Attribute placed on types/members to signal the type of serialization in use.
	/// Note that the type of serialization in use by a class may be overridden on a
	/// member-by-member basis.
	/// </summary>
	[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class | AttributeTargets.Interface
		| AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
	public sealed class PAFSerializableAttribute : PAFSerializableAttributeBase
	{
		#region Constructors
		/// <summary>
		/// Default constructor corresponds to "unspecified" serialization type.
		/// </summary>
		public PAFSerializableAttribute()
		:base(PAFSerializationType.Unspecified)
		{ }
		/// <summary>
		/// See <see cref=" PAFSerializableAttributeBase"/>.
		/// </summary>
		/// <param name="serializationTypeInUse">
		/// See <see cref=" PAFSerializableAttributeBase"/>.
		/// </param>
		public PAFSerializableAttribute(PAFSerializationType serializationTypeInUse)
			: base(serializationTypeInUse) { }
		/// <summary>
		/// See <see cref=" PAFSerializableAttributeBase"/>.
		/// </summary>
		/// <param name="serializationTypeInUse">
		/// See <see cref=" PAFSerializableAttributeBase"/>.
		/// </param>
		/// <param name="customSerializer">
		/// See <see cref=" PAFSerializableAttributeBase"/>.
		/// </param>
		public PAFSerializableAttribute(PAFSerializationType serializationTypeInUse, IPAFTypeProps customSerializer)
			: base(serializationTypeInUse, customSerializer) { }
		#endregion // Constructors
	}
}
