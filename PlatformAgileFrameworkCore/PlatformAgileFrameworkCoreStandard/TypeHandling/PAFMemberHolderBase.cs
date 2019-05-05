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
using System.Reflection;
using PlatformAgileFramework.Serializing;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.TypeHandling
{
	/// <summary>
	/// <para>
	///	Contains the name of a member and possibly the actual <see cref="MemberInfo"/>.
	/// This class is designed to carry member information locally or across
	/// "AppDomain" boundaries or whenever a serializable representation
	/// of the member is needed.
	/// </para>
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// <history>
	/// <author> DAP </author>
	/// <date> 06nov2011 </date>
	/// <contribution>
	/// <para>
	/// Added history, changed name space, added more DOCs.
	/// </para>
	/// </contribution>
	/// </history>
	[PAFSerializable(PAFSerializationType.PAFSurrogate)]
	public class PAFMemberHolderBase
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// The host type, which is serialized.
		/// </summary>
		protected internal PAFTypeHolderBase m_HostType;
		/// <summary>
		/// Member info. Never serialized.
		/// </summary>
		protected MemberInfo m_MmbrInfo;
		/// <summary>
		/// Stringful representation of the method, which is serialized.
		/// </summary>
		protected internal string m_MemberName;
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Constructor sets props.
		/// </summary>
		/// <param name="memberInfo">
		/// The actual <see cref="MemberInfo"/>, if available.
		/// </param>
		/// <param name="memberName">
		/// Not blank or <see langword="null"/>, for late binding. If
		/// <paramref name="memberInfo"/> is non-null, it will be loaded
		/// from that with the <see cref="MemberInfo.Name"/>.
		/// </param>
		/// <param name="hostType">
		/// Not blank or <see langword="null"/>, for late binding. If
		/// <paramref name="memberInfo"/> is non-null, it will be loaded
		/// from that with the <see cref="MemberInfo.DeclaringType"/>.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> is thrown if 
		/// <paramref name="memberName"/> is <see langword="null"/> and the
		/// <paramref name="memberInfo"/> is <see langword="null"/>.
		/// </exception>
		/// <exception cref="ArgumentNullException"> is thrown if 
		/// <paramref name="hostType"/> is <see langword="null"/> and the
		/// <paramref name="memberInfo"/> is <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		protected PAFMemberHolderBase(MemberInfo memberInfo, string memberName = null,
			PAFTypeHolderBase hostType = null)
		{
			// Early bound solves everything!
			if (memberInfo != null) {
				memberName = memberInfo.Name;
				hostType = memberInfo.DeclaringType;
			}

			if (string.IsNullOrEmpty(memberName)) throw new ArgumentNullException("memberName");
			if (hostType == null) throw new ArgumentNullException("hostType");

			// Everything is OK - load 'em up.
			m_MemberName = memberName;
			m_HostType = hostType;
			m_MmbrInfo = memberInfo;
		}
		/// <summary>
		/// Copy constructor. Makes deep copy.
		/// </summary>
		/// <param name="memberHolder">
		/// One of us.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> is thrown if 
		/// <paramref name="memberHolder"/> is <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		protected PAFMemberHolderBase(PAFMemberHolderBase memberHolder)
		{
			if (memberHolder == null) throw new ArgumentNullException("memberHolder");
			// Everything is OK - load 'em up.
			m_MemberName = memberHolder.MemberName;
			m_HostType = memberHolder.HostType;
			m_MmbrInfo = memberHolder.MmbrInfo;
		}
		#endregion // Constructors
		#region Properties
		/// <summary>
		/// The host type. Can be late-bound. Never <see langword="null"/>.
		/// </summary>
		public virtual PAFTypeHolderBase HostType
		{ get { return m_HostType; } protected set { m_HostType = value; } }
		/// <summary>
		/// The actual <see cref="MemberInfo"/>. This may be <see langword="null"/> in late-bound
		/// scenarios.
		/// </summary>
		public virtual MemberInfo MmbrInfo
		{ get { return m_MmbrInfo; } set { m_MmbrInfo = value; } }
		/// <summary>
		/// Stringful representation of the member - never <see langword="null"/> or
		/// blank.
		/// </summary>
		public virtual string MemberName
		{ get { return m_MemberName; } protected set { m_MemberName = value; } }
		#endregion // Properties
		#region Methods
		#region Conversion Operators
		/// <summary>
		/// Calls <c>PAFMemberHolderBase(info)</c>.
		/// </summary>
		/// <param name="info">
		/// The info to be wrapped. Not <see langword="null"/>.
		/// </param>
		/// <returns>
		/// One of us.
		/// </returns>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> is thrown if <paramref name="info"/>.
		/// is <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		public static implicit operator PAFMemberHolderBase(MemberInfo info)
		{
			if (info == null)
				throw new ArgumentNullException("info");
			return new PAFMemberHolderBase(info);
		}
		#endregion // Conversion Operators
		#region Obligatory Patch for Equals and Hash Code
		/// <summary>
		/// Determines whether the specified <see cref="object"/> is equal to the
		/// current <see cref="object"/>.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if the specified <see cref="object"/> is equal to the current
		/// <see cref="object"/>; otherwise, false.
		/// </returns>
		/// <param name="obj">
		/// The <see cref="object"/> to compare with the current <see cref="object"/>.
		/// </param>
		/// <remarks>
		/// Patch for Microsoft's mistake.
		/// </remarks>
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			// ReSharper disable BaseObjectEqualsIsObjectEquals
			return GetType() == obj.GetType() && base.Equals(obj);
			// ReSharper restore BaseObjectEqualsIsObjectEquals
		}
		/// <summary>
		/// We are a reference type so just call base to shut up the compiler/tools.
		/// </summary>
		/// <returns>
		/// The original hash code.
		/// </returns>
		public override int GetHashCode()
		{
			// ReSharper disable BaseObjectGetHashCodeCallInGetHashCode
			return base.GetHashCode();
			// ReSharper restore BaseObjectGetHashCodeCallInGetHashCode
		}
		#endregion // Obligatory Patch for Equals and Hash Code
		#endregion // Methods
	}
}