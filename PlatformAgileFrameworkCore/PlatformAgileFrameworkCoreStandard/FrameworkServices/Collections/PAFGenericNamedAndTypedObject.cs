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

#region Using directives
using System;
using System.Security;

#endregion

namespace PlatformAgileFramework.Collections
{
	/// <summary>
	/// A class for objects identified by Type and/or name. Generic version that is
	/// basically just a type-safe wrapper.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> DAP </author>
	/// <date> 22jan2012 </date>
	/// <description>
	/// New.
	/// </description>
	/// </contribution>
	/// </history>
	// ReSharper disable once PartialTypeWithSinglePart
	// Core part
	public partial class PAFNamedAndTypedObject<T> : PAFNamedAndTypedObject,
		IPAFNamedAndTypedObjectInternal<T>
	{
		#region Constructors
		/// <summary>
		/// Just builds with the props.
		/// </summary>
		/// <param name="objectType">
		/// Type of the object - <see langword="null"/> causes type to be
		/// inferred from the object, if there is one. Else this becomes
		/// just a wrapper for an <see cref="object"/>.
		/// </param>
		/// <param name="objectName">
		/// Name. <see langword="null"/> gets blank.
		/// </param>
		/// <param name="item">
		/// The generic item. <see langword="null"/> is OK.
		/// </param>
		/// <param name="isDefaultObject">
		/// </param>
		/// <exceptions>
		/// Base class throws exceptions for type mismatch.
		/// See <see cref="PAFNamedAndTypedObject(Type, string, object, bool)"/>
		/// </exceptions>
		public PAFNamedAndTypedObject(Type objectType, string objectName = null,
			T item = default(T), bool isDefaultObject = false)
			:base(objectType, objectName, item, isDefaultObject)
		{
		}
		#endregion // Constructors
		#region Properties
		/// <summary>
		/// See <see cref="IPAFNamedAndTypedObject{T}"/>.
		/// </summary>
		public virtual T ItemValue
		{
			get { return (T)m_ObjectValue; }
			[SecurityCritical] set { m_ObjectValue = value; }
		}
		#endregion // Properties
		#region Methods
		void IPAFNamedAndTypedObjectInternal<T>.SetItemValue(T item)
		{
			m_ObjectValue = item;
		}
		#endregion // Methods
	}
}
