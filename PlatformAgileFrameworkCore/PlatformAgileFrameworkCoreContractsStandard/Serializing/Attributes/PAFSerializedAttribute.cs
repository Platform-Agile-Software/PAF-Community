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
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.Serializing.Attributes
{
	/// <summary>
	/// Attribute placed on types/members to signal that they are/not to be serialized.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public sealed class PAFSerializedAttribute : Attribute
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Indicates whether the member is to be serialized. Defaults to <see langword="null"/>.
		/// Meaning of this value is dependent on the serializer in use.
		/// </summary>
		private bool? m_Serialize;
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Sets defaults for the class.
		/// </summary>
		public PAFSerializedAttribute()
		{
		}
		/// <summary>
		/// Sets a definitive value for <see cref="Serialize"/>.
		/// </summary>
		public PAFSerializedAttribute(bool serialize)
		{
			Serialize = serialize;
		}

		#endregion
		#region Properties
		/// <summary>
		/// Indicates whether the member is to be serialized. Can be set first time
		/// by low-priviledge callers.
		/// </summary>
		public bool Serialize
		{
			get
			{
				return m_Serialize.GetValueOrDefault();
			}
			set
			{
				if (m_Serialize.HasValue) CoreContract_TypeHandlingUtils.CriticalSet(value);
				else m_Serialize = value;
			}
		}
		#endregion // Properties
	}
}
