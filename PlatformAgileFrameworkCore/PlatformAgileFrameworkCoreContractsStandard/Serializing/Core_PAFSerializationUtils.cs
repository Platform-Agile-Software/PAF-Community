//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2014 Icucom Corporation
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

#pragma warning disable 1587
///<file>
/// This file contains serialiazation utilities that support 
/// general custom serialization on the ECMA CLI framework.
/// This is the part that runs under Silverlight.
/// <file/>
#pragma warning restore 1587
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
// note KRM 14aug2017
//using System.Runtime.Serialization;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.Collections.ExtensionMethods;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.Serializing
{
	/// <summary>
	/// Helper methods for serialization/deserialization. This part supplies core
	/// framework functionality for low-priviledge environments. Design of the class
	/// allows for extension by inheritance or through partial class augmentation
	/// or both.
	/// </summary>
	/// <history>
	/// <author> KRM </author>
	/// <date> 14aug2017 </date>
	/// <contribution>
	/// <description>
	/// Took out datacontract attribute and reference to serialization assy, since .Net standard
	/// choked when I converted it.
	/// </description>
	/// </contribution>
	/// <author> KRM </author>
	/// <date> 04jun2015 </date>
	/// <contribution>
	/// <description>
	/// Changed to work with attribute types instead of attributes to be consistent
	/// with 4.5 reflection. Killed loading of collections except statically. I didn't
	/// see the use case.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> B. McCoy </author>
	/// <date> 01sep2011 </date>
	/// <description>
	/// <para>
	/// Added history.
	/// </para>
	/// <para>
	/// Changed the reflection-based checks for serializability into attribute array
	/// by using partial method to load "SerializableAttribute". Second SL refactor.
	/// </para>
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Thread-safe.
	/// </threadsafety>
	// ReSharper disable PartialTypeWithSinglePart
	public partial class PAFSerializationUtils
// ReSharper restore PartialTypeWithSinglePart
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Backing for prop.
		/// </summary>
		private static readonly IList<Type> s_SerializationEnablingAttributeTypes;
		/// <summary>
		/// Backing for prop.
		/// </summary>
		private static readonly IList<Type> s_SerializationEnablingInterfaces;
		#endregion Class Fields and Autoproperties

		#region Constructors
		/// <summary>
		/// Initializes collections. Adds <see cref="PAFSerializableAttribute"/>
		/// and <see cref="DataContractAttribute"/>.
		/// </summary>
		static PAFSerializationUtils()
		{
			/////////////////////////////////////////////////////////////////
			// Add the attributes.
			s_SerializationEnablingAttributeTypes = new Collection<Type>();
			// ReSharper disable InvocationIsSkipped
			LoadSerializationAttributesPartial(ref s_SerializationEnablingAttributeTypes);
			// ReSharper restore InvocationIsSkipped
			// Exclude dupes in case either of these was added by the method.
			// Note KRM 14aug2017
//			s_SerializationEnablingAttributeTypes.AddRangeNoDupes(
//			new[] { typeof(PAFSerializableAttribute), typeof(DataContractAttribute) });
			s_SerializationEnablingAttributeTypes.AddRangeNoDupes(
				new[] { typeof(PAFSerializableAttribute) });

			/////////////////////////////////////////////////////////////////
			s_SerializationEnablingInterfaces = new Collection<Type>();
			// ReSharper disable InvocationIsSkipped
			LoadSerializationInterfaces(ref s_SerializationEnablingInterfaces);
			// ReSharper restore InvocationIsSkipped
		}
		#endregion Constructors
		#region Methods
		#region Partial Methods
		/// <summary>
		/// This partial method is designed to load the "SerializableAttribute" in
		/// ECMA/CLR environments and/or load other attribute types for other serialization
		/// methodologies.
		/// </summary>
		/// <param name="attributeTypes">Attribute type collection that the method adds to.</param>
		// ReSharper disable PartialMethodWithSinglePart
		static partial void LoadSerializationAttributesPartial(ref IList<Type> attributeTypes);
		// ReSharper restore PartialMethodWithSinglePart
		/// <summary>
		/// This partial method is designed to load interface types that enable other serialization
		/// methodologies.
		/// </summary>
		/// <param name="types">Type collection that the method adds to.</param>
		// ReSharper disable PartialMethodWithSinglePart
		static partial void LoadSerializationInterfaces(ref IList<Type> types);
		// ReSharper restore PartialMethodWithSinglePart
		#endregion // Partial Methods
		/// <summary>
		/// Returns the various attribute types that types can be decorated with to
		/// afford serialization.
		/// </summary>
		public static IEnumerable<Type> GetSerializationEnablingAttributeTypes()
		{
			return s_SerializationEnablingAttributeTypes.BuildCollection();
		}
		/// <summary>
		/// Returns the various interfaces that types can implement to
		/// afford serialization.
		/// </summary>
		public static IEnumerable<Type> GetSerializationEnablingInterfaces()
		{
			return s_SerializationEnablingInterfaces.BuildCollection();
		}
		#endregion // Methods
	}
}
