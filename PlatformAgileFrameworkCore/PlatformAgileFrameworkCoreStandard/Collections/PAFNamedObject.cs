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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security;

#endregion

namespace PlatformAgileFramework.Collections
{
	/// <summary>
	/// An immutable class for objects identified by Type and/or name.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> DAV </author>
	/// <date> 22jun2012 </date>
	/// <description>
	/// Built for 4.0 security. This is a new type that was factored for
	/// flexibility in use.
	/// </description>
	/// </contribution>
	/// </history>
	public class PAFNamedObject : IPAFNamedObjectInternal
	{
		#region Class Fields and Autoproperties.
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal bool m_IsDefaultObject;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal string m_ObjectName;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal object m_ObjectValue;
		#endregion // Class Fields and Autoproperties.
		/// <summary>
		/// Just builds with the props.
		/// </summary>
		/// <param name="objectName">Name. <see langword="null"/> gets blank.</param>
		/// <param name="obj">
		/// The object. <see langword="null"/> is OK.
		/// </param>
		/// <param name="isDefaultObject">
		/// Sets <see cref="IsDefaultObject"/>.
		/// </param>
		public PAFNamedObject(string objectName, object obj = null,
			bool isDefaultObject = false)
		{
			m_ObjectName = "";
			if(objectName != null) m_ObjectName = objectName;
			m_ObjectValue = obj;
			m_IsDefaultObject = isDefaultObject;
		}
		#region Methods
		/// <summary>
		/// See <see cref="IPAFNamedAndTypedObjectInternal"/>
		/// </summary>
		public string ObjectName
		{
			get { return m_ObjectName; }
			[SecurityCritical]
			set { m_ObjectName = value; }
		}
		/// <summary>
		/// See <see cref="IPAFNamedObject"/>
		/// </summary>
		public bool IsDefaultObject
		{
			get { return m_IsDefaultObject; }
			[SecurityCritical] set { m_IsDefaultObject = value; }
		}
		/// <summary>
		/// See <see cref="IPAFNamedObject"/>
		/// </summary>
		public object ObjectValue 
		{ get { return m_ObjectValue; } [SecurityCritical] set { m_ObjectValue = value; } }
		#region IPAFNamedAndTypedObjectInternal Implementation
		/// <summary>
		/// See <see cref="IPAFNamedObjectInternal"/>
		/// </summary>
		/// <param name="isDefaultObject">
		/// See <see cref="IPAFNamedObjectInternal"/>
		/// </param>
		void IPAFNamedObjectInternal.SetIsDefault(bool isDefaultObject)
		{
			m_IsDefaultObject = isDefaultObject;
		}
		/// <summary>
		/// See <see cref="IPAFNamedObjectInternal"/>
		/// </summary>
		/// <param name="objectName">
		/// See <see cref="IPAFNamedObjectInternal"/>
		/// </param>
		void IPAFNamedObjectInternal.SetObjectName(string objectName)
		{
			m_ObjectName = objectName;
		}
		/// <summary>
		/// See <see cref="IPAFNamedObjectInternal"/>
		/// </summary>
		/// <param name="objectValue">
		/// See <see cref="IPAFNamedObjectInternal"/>
		/// </param>
		void IPAFNamedObjectInternal.SetObjectValue(object objectValue)
		{
			m_ObjectValue = objectValue;
		}
		#endregion // IPAFNamedAndTypedObjectInternal Implementation
		#region Static Helpers
		/// <summary>
		/// Separates objects out of a collection.
		/// </summary>
		/// <param name="natos">Incoming named objects.</param>
		/// <returns>object enumeration or <see langword="null"/>.</returns>
		public static IEnumerable<object> GetObjects(IEnumerable<IPAFNamedObject> natos)
		{
			if (natos == null) return null;
			var coll = new Collection<object>();
			foreach (var nato in natos) {
				coll.Add(nato.ObjectValue);
			}
			return coll;
		}
		#endregion // Static Helpers
		#endregion // Methods
	}
}
