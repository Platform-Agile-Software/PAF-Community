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
	/// Attribute placed on types/members to signal the type of serialization in use.
	/// Note that the type of serialization in use by a class may be overridden on a
	/// member-by-member basis.
	/// </summary>
	/// <threadsafety>
	/// Not thread-safe. Make a deep clone to hand out copies to clients for inspection.
	/// </threadsafety>
	/// <remarks>
	/// <para>
	/// Noted that the attribute is not inherited so it must be repeated on any subclasses it
	/// is applied to.
	/// </para>
	/// <para>
	/// The use of the attribute on the interface means that the implementation must
	/// support AT LEAST the serialization capabilities associated with the interface.
	/// Implementations should call the appropriate serialization utility to do the
	/// check in their static constructor.
	/// </para>
	/// <para>
	/// We must allow multiple decorations for partial classes.
	/// </para>
	/// </remarks>
	[AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class | AttributeTargets.Interface
		| AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = true)]
// ReSharper disable PartialTypeWithSinglePart
	public partial class PAFSerializableAttributeBase : Attribute,
		IPAFSerializableAttributeInternal
// ReSharper restore PartialTypeWithSinglePart
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Backing for prop/method.
		/// </summary>
		/// <remarks>
		/// Private visibility since we don't expect the use of a surrogate.
		/// </remarks>
		private PAFSerializationType m_SerializationTypeInUse;
		/// <summary>
		/// Backing for prop/method.
		/// </summary>
		/// <remarks>
		/// Private visibility since we don't expect the use of a surrogate.
		/// </remarks>
		private IPAFTypeProps m_CustomSerializer;
		/// <summary>
		/// Backing for prop/method.
		/// </summary>
		/// <remarks>
		/// Private visibility since we don't expect the use of a surrogate.
		/// </remarks>
		private object m_SerializationState;
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Default constructor for interitance and framework support.
		/// </summary>
		protected internal PAFSerializableAttributeBase() { }
		/// <summary>
		/// Main constructor for the attribute.
		/// </summary>
		/// <param name="serializationTypeInUse">
		/// See <see cref="IPAFSerializableAttribute"/>.
		/// </param>
		/// <param name="customSerializer">
		/// See <see cref="IPAFSerializableAttribute"/>. Default = <see langword="null"/>.
		/// </param>
		public PAFSerializableAttributeBase(PAFSerializationType serializationTypeInUse,
			IPAFTypeProps customSerializer = null)
		{
			// TODO KRM bad type exception for enum = none.
			m_SerializationTypeInUse = serializationTypeInUse;
			m_CustomSerializer = customSerializer;
		}
		#endregion // Constructors
		#region Properties
		/// <summary>
		/// Backing for the explicit implementation.
		/// </summary>
// ReSharper disable ConvertToAutoProperty
// ReSharper error - shouldn't flag this when we need non-virtual access through the field.
		internal virtual object SerializationState
// ReSharper restore ConvertToAutoProperty
		{ get { return m_SerializationState; } set { m_SerializationState = value; } }
		#endregion // Properties
		#region Methods
		/// <summary>
		/// Backing for the explicit implementation.
		/// </summary>
		internal virtual void SetSerializationTypeInUse(PAFSerializationType serializationTypeInUse)
		{
			m_SerializationTypeInUse = serializationTypeInUse;
		}
		/// <summary>
		/// Backing for the explicit implementation.
		/// </summary>
		internal virtual void SetCustomSerializerID(IPAFTypeProps customSerializer)
		{
			m_CustomSerializer = customSerializer;
		}
		#endregion // Methods
		#region IPAFSerializableAttributeInternal Implementation
		#region IPAFSerializableAttribute Implementation
		/// <summary>
		/// See <see cref="IPAFSerializableAttribute"/>.
		/// </summary>
		public PAFSerializationType SerializationTypesEnabled
		{ get { return m_SerializationTypeInUse; } }
		/// <summary>
		/// See <see cref="IPAFSerializableAttribute"/>.
		/// </summary>
		public IPAFTypeProps CustomSerializer
		{ get { return m_CustomSerializer; } }
		#endregion
		/// <summary>
		/// See <see cref="IPAFSerializableAttributeInternal"/>.
		/// </summary>
		void IPAFSerializableAttributeInternal.SetSerializationTypeInUse(PAFSerializationType serializationTypeInUse)
		{
			SetSerializationTypeInUse(serializationTypeInUse);
		}
		/// <summary>
		/// See <see cref="IPAFSerializableAttributeInternal"/>.
		/// </summary>
		void IPAFSerializableAttributeInternal.SetCustomSerializer(IPAFTypeProps customSerializer)
		{
			SetCustomSerializerID(customSerializer);
		}
		/// <summary>
		/// See <see cref="IPAFSerializableAttributeInternal"/>.
		/// </summary>
		object IPAFSerializableAttributeInternal.SerializationState
		{ get { return SerializationState; } set { SerializationState = value; } }
		#endregion // IPAFSerializableAttributeInternal Implementation
	}
}
