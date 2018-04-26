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
using System.Security;
using PlatformAgileFramework.Serializing.Attributes;
using PlatformAgileFramework.Serializing.ECMAReplacements;

namespace PlatformAgileFramework.ErrorAndException
{
	/// <summary>
	/// <para>
	///	The base Generic exception from which all of our PAF custom exceptions derive. Sealed
	/// version of <see cref="PAFAbstractStandardExceptionBase{T}"/>.
	/// </para>
	/// </summary>
	/// <typeparam name="T">
	/// <see cref="PAFAbstractStandardExceptionBase{T}"/>. In this class, <typeparamref name="T"/>
	/// is constrained to implement <see cref="IPAFStandardExceptionData"/>.
	/// </typeparam>
	/// <remarks>
	/// </remarks>
	/// <history>
	/// <author> BMC </author>
	/// <date> 02jul2011 </date>
	/// <contribution>
	/// <para>
	/// Refactored from our "standard" exception which uses the <see cref="IPAFStandardExceptionData"/>
	/// interface for the Generic.
	/// </para>
	/// </contribution>
	/// </history>
	[PAFSerializable]
	public sealed class PAFStandardException<T> :
		PAFAbstractStandardExceptionBase<T>,
		IPAFStandardException<T> where T : IPAFStandardExceptionData
	{
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="PAFAbstractStandardExceptionBase{T}"/> class.
		/// </summary>
		/// <param name="data">
		/// <see cref="PAFAbstractStandardExceptionBase{T}"/>.
		/// </param>
		/// <param name="message"><see cref="Exception"/></param>
		/// <param name="innerException"><see cref="Exception"/></param>
		public PAFStandardException(T data, string message = null, Exception innerException = null
			): base(data, message, innerException)
		{
		}

		/// <summary>
		/// <see cref="PAFAbstractStandardExceptionBase{T}"/>.
		/// </summary>
		/// <param name="info">See <see cref="IPAFSerializationInfoCLS"/>.
		///</param>
		/// <param name="context">
		/// Not used in this method.
		/// </param>
		[SecurityCritical]
// ReSharper disable UnusedParameter.Local
		public PAFStandardException(IPAFSerializationInfoCLS info, PAFSerializationContext context)
// ReSharper restore UnusedParameter.Local
			: this(context, info){}
		/// <summary>
		/// <see cref="PAFAbstractStandardExceptionBase{T}"/>.
		/// </summary>
		/// <param name="context">
		/// Not used in this method.
		/// </param>
		/// <param name="info">See <see cref="IPAFSerializationInfoCLS"/>.
		///</param>
		internal PAFStandardException(PAFSerializationContext context, IPAFSerializationInfoCLS info)
			: base(context, info)
		{
			info.TryGetValue(DATA_TAG, out m_Data);
		}
		#endregion // Constructors
		#region Obligatory Patch for Equals and Hash Code
		/// <summary>
		/// Determines whether the specified <see cref="Object"/> is equal to the
		/// current <see cref="Object"/>.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if the specified <see cref="Object"/> is equal to the current
		/// <see cref="Object"/>; otherwise, false.
		/// </returns>
		/// <param name="obj">
		/// The <see cref="Object"/> to compare with the current <see cref="Object"/>.
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
	}
}