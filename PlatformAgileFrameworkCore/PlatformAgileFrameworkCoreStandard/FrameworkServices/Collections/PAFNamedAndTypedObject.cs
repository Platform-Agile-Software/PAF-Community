//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2019 Icucom Corporation
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Security;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.TypeHandling;
using PlatformAgileFramework.TypeHandling.Exceptions;
#endregion

namespace PlatformAgileFramework.Collections
{
	/// <summary>
	/// A class for objects identified by Type and/or name.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 21jan2019 </date>
	/// <description>
	/// Brought constructor for late type resolution back in.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> DAP </author>
	/// <date> 22jan2012 </date>
	/// <description>
	/// New. Refactored from the old class to derive from <see cref="PAFNamedObject"/>.
	/// Took out constructors for late type resolution and moved them to extended.
	/// </description>
	/// </contribution>
	/// </history>
	// ReSharper disable once PartialTypeWithSinglePart
	// Core part
	public partial class PAFNamedAndTypedObject : PAFNamedObject, IPAFNamedAndTypedObjectInternal
	{
		#region Class Fields and Autoproperties.
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal string m_AssemblyQualifiedObjectTypeName;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal Type m_ObjectType;
		#endregion // Class Fields and Autoproperties.
		#region Constructors
		/// <summary>
		/// Just builds with the props.
		/// </summary>
		/// <param name="objectType">
		/// Sets <see cref="ObjectType"/>.
		/// </param>
		/// <param name="objectName">
		/// See base class.
		/// </param>
		/// <param name="obj">
		/// See base class.
		/// </param>
		/// <param name="isDefaultObject">
		/// See base class.
		/// </param>
		/// <exceptions>
		/// <exception cref="PAFStandardException{T}"> with
		/// <see cref="PAFTypeMismatchExceptionMessageTags.FIRST_TYPE_NOT_CASTABLE_TO_SECOND_TYPE"/>
		/// message if <paramref name="obj"/> is loaded and it does not inherit from
		/// <paramref name="objectType"/>.
		/// </exception>
		/// </exceptions>
		public PAFNamedAndTypedObject(Type objectType,
			string objectName = "", object obj = null,
			bool isDefaultObject = false)
			: base(objectName, obj, isDefaultObject)
		{
			if ((obj != null) && (objectType == null))
			{
				objectType = obj.GetType();
			}

			m_ObjectType = typeof(object);
			if (objectType != null) m_ObjectType = objectType;

			m_AssemblyQualifiedObjectTypeName = m_ObjectType.AssemblyQualifiedName;
			if (obj == null) return;

			var exception = TypeHandlingUtils.ObjectNotInheritedException(
				obj, m_ObjectType);
			if (exception != null)
			{
				throw exception;
			}
		}

		/// <summary>
		/// Just builds with the props. The ability to specify the
		/// type string was re-added (this constructor) to re-enable
		/// late-bound operation, which was needed for the add-in model
		/// and Golea. This was always problematic due to mono's need
		/// for a slightly different format for the type name. Apparently
		/// this has been resolved in .Net Standard.
		/// </summary>
		/// <param name="assemblyQualifiedObjectTypeName">
		/// This is the textual name of the type. This is overwritten on
		/// construction if type or object arguments are specified.
		/// </param>
		/// <param name="objectType">
		/// Sets <see cref="ObjectType"/>.
		/// </param>
		/// <param name="objectName">
		/// See base class.
		/// </param>
		/// <param name="obj">
		/// See base class.
		/// </param>
		/// <param name="isDefaultObject">
		/// See base class.
		/// </param>
		/// <exceptions>
		/// <exception cref="PAFStandardException{T}"> with
		/// <see cref="PAFTypeMismatchExceptionMessageTags.FIRST_TYPE_NOT_CASTABLE_TO_SECOND_TYPE"/>
		/// message if <paramref name="obj"/> is loaded and it does not inherit from
		/// <paramref name="objectType"/>.
		/// </exception>
		/// </exceptions>
		public PAFNamedAndTypedObject(
			string assemblyQualifiedObjectTypeName,
			Type objectType = null,
			string objectName = "", object obj = null,
			bool isDefaultObject = false)
			: this(objectType, objectName, obj, isDefaultObject)
		{
			m_AssemblyQualifiedObjectTypeName = assemblyQualifiedObjectTypeName;
		}
		#endregion // Constructors
		#region Properties
		/// <summary>
		/// See <see cref="IPAFNamedAndTypedObject"/>
		/// </summary>
		public string AssemblyQualifiedObjectType
		{
			get { return m_AssemblyQualifiedObjectTypeName; }
			[SecurityCritical]
			set
			{
				if (m_AssemblyQualifiedObjectTypeName != null)
					throw new InvalidOperationException("Type string is already set");
				m_AssemblyQualifiedObjectTypeName = value;
			}
		}
		/// <summary>
		/// See <see cref="IPAFNamedAndTypedObject"/>
		/// </summary>
		public Type ObjectType
		{
			get { return m_ObjectType; }
			[SecurityCritical]
			set { m_ObjectType = value; }
		}
		#endregion // Properties
		#region IPAFNamedAndTypedObjectInternal Implementation
		/// <summary>
		/// See <see cref="IPAFNamedAndTypedObjectInternal"/>
		/// </summary>
		/// <param name="assemblyQualifiedObjectTypeName">
		/// See <see cref="IPAFNamedAndTypedObjectInternal"/>
		/// </param>
		void IPAFNamedAndTypedObjectInternal.SetAssemblyQualifiedObjectTypeName(
			string assemblyQualifiedObjectTypeName)
		{
			if (m_ObjectType != null)
				throw new InvalidOperationException("Type is already resolved");
			m_AssemblyQualifiedObjectTypeName = assemblyQualifiedObjectTypeName;
		}
		/// <summary>
		/// See <see cref="IPAFNamedAndTypedObjectInternal"/>
		/// </summary>
		/// <param name="objectType">
		/// See <see cref="IPAFNamedAndTypedObjectInternal"/>
		/// </param>
		void IPAFNamedAndTypedObjectInternal.SetObjectType(Type objectType)
		{
			m_ObjectType = objectType;
		}
		#endregion // IPAFNamedAndTypedObjectInternal Implementation
		#region Static Helpers
		/// <summary>
		/// Separates types out of a collection.
		/// </summary>
		/// <param name="natos">Incoming named and typed objects.</param>
		/// <returns>
		/// type enumeration. May have nulls due to late binding.
		/// </returns>
		public static IEnumerable<Type> GetTypes(IEnumerable<IPAFNamedAndTypedObject> natos)
		{
			if (natos == null) return null;
			var coll = new Collection<Type>();
			foreach (var nato in natos)
			{
				coll.Add(nato.ObjectType);
			}
			return coll;
		}
		#endregion // Static Helpers
	}
}
