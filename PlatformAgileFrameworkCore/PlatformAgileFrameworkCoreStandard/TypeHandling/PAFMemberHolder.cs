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
using System.Reflection;
using PlatformAgileFramework.Serializing;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.TypeHandling
{
	/// <summary>
	/// Sealed version of <see cref="PAFMemberHolderBase"/>.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// <history>
	/// <author> DAP </author>
	/// <date> 07nov2011 </date>
	/// <contribution>
	/// <para>
	/// New sealed version.
	/// </para>
	/// </contribution>
	/// </history>
	[PAFSerializable(PAFSerializationType.PAFSurrogate)]
	public sealed class PAFMemberHolder: PAFMemberHolderBase
	{
		#region Constructors
		/// <summary>
		/// Constructor sets props -just calls base.
		/// </summary>
		/// <param name="memberInfo">
		/// See base.
		/// </param>
		/// <param name="memberName">
		/// See base.
		/// </param>
		/// <param name="hostType">
		/// See base.
		/// </param>
		/// <exceptions>
		/// See base.
		/// </exceptions>
		public PAFMemberHolder(MemberInfo memberInfo, string memberName = null,
			PAFTypeHolderBase hostType = null): base(memberInfo, memberName, hostType){}
		/// <summary>
		/// Copy constructor. Makes deep copy.
		/// </summary>
		/// <param name="memberHolder">
		/// See base.
		/// </param>
		/// <exceptions>
		/// See base.
		/// </exceptions>
		public PAFMemberHolder(PAFMemberHolderBase memberHolder):base(memberHolder)
		{
			if (memberHolder == null) throw new ArgumentNullException("memberHolder");
		}
		#endregion // Constructors
		#region Methods
		#region Conversion Operators
		/// <summary>
		/// Calls <c>PAFMemberHolder(null, null, info)</c>.
		/// </summary>
		/// <param name="info">
		/// The info to be wrapped. Not <see langword="null"/>.
		/// </param>
		/// <returns>
		/// One of us.
		/// </returns>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> is thrown if <paramref name="info"/>
		/// is <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		public static implicit operator PAFMemberHolder(MemberInfo info)
		{
			if (info == null)
				throw new ArgumentNullException("info");
			return new PAFMemberHolder(info);
		}
		#endregion // Conversion Operators
		#endregion // Methods
	}
}