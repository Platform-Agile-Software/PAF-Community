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
	/// Sealed version of <see cref="PAFMemberHolderBase"/>.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// <history>
	/// <author> DAP </author>
	/// <date> 10nov2011 </date>
	/// <contribution>
	/// <para>
	/// New. Made to able to carry <see cref="MethodInfo"/> in its base class.
	/// </para>
	/// </contribution>
	/// </history>
	[PAFSerializable(PAFSerializationType.PAFSurrogate)]
	public sealed class PAFMethodHolder: PAFMemberHolderBase
	{
		#region Constructors
		/// <summary>
		/// Constructor sets props.
		/// </summary>
		/// <param name="methodInfo">
		/// The actual method info, if known.
		/// </param>
		/// <param name="methodName">
		/// String name of the method - we don't support overloads in this
		/// simple model.
		/// </param>
		/// <param name="hostType">
		/// Declaring type.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> is thrown if 
		/// <paramref name="methodInfo"/> is <see langword="null"/> and either the
		/// <paramref name="hostType"/> is <see langword="null"/> or the
		/// <paramref name="methodName"/> is <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		public PAFMethodHolder(MethodInfo methodInfo = null, string methodName = null,
			PAFTypeHolderBase hostType = null): base(methodInfo, methodName, hostType) { }
		/// <summary>
		/// Copy constructor. Makes deep copy.
		/// </summary>
		/// <param name="methodHolder">
		/// See base.
		/// </param>
		/// <exceptions>
		/// See base.
		/// </exceptions>
		public PAFMethodHolder(PAFMethodHolder methodHolder):base(methodHolder)
		{
			if (methodHolder == null) throw new ArgumentNullException("methodHolder");
		}
		#endregion // Constructors
		#region Methods
		/// <summary>
		/// Just converts to a <see cref="MethodInfo"/> - the only thing we
		/// can hold.
		/// </summary>
		public MethodInfo mthdInfo
		{ get { return (MethodInfo)MmbrInfo; } set { MmbrInfo = value; } }
		#region Conversion Operators
		/// <summary>
		/// Calls <c>PAFMethodHolder(info)</c>.
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
		public static implicit operator PAFMethodHolder(MethodInfo info)
		{
			if (info == null)
				throw new ArgumentNullException("info");
			return new PAFMethodHolder(info);
		}
		#endregion // Conversion Operators
		#endregion // Methods
	}
}