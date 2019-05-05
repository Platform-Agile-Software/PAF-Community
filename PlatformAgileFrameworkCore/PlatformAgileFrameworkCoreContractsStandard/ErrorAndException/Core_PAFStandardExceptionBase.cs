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
using System.Security;
using PlatformAgileFramework.Serializing.Attributes;
using PlatformAgileFramework.Serializing.ECMAReplacements;

namespace PlatformAgileFramework.ErrorAndException
{
	/// <summary>
	/// <para>
	///	The base Generic exception from which all of our custom exceptions derive. The exception
	/// is designed to be used in both low and elevated trust environments with serialization
	/// performed by the PAFSerializer.
	/// </para>
	/// </summary>
	/// <typeparam name="T">
	/// This type is constrained to implement <see cref="IPAFStandardExceptionData"/> and
	/// must be serializable
	/// </typeparam>
	/// <remarks>
	/// <para>
	/// Partial class for extension in CLR and extended library.
	/// </para>
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
// ReSharper disable PartialTypeWithSinglePart
	public abstract partial class PAFAbstractStandardExceptionBase<T>
		: PAFAbstractExceptionBase<T> where T: IPAFStandardExceptionData
// ReSharper restore PartialTypeWithSinglePart
	{
		#region Constructors
		/// <summary>
		/// <see cref="PAFAbstractExceptionBase{T}"/>.
		/// </summary>
		/// <param name="data">
		/// <see cref="PAFAbstractExceptionBase{T}"/>.
		/// </param>
		/// <param name="message"><see cref="Exception"/></param>
		/// <param name="innerException"><see cref="Exception"/></param>
		protected PAFAbstractStandardExceptionBase(T data, string message = null,
			Exception innerException = null)
			: base(data, message, innerException)
		{
		}

		/// <summary>
		/// <see cref="PAFAbstractExceptionBase{T}"/>.
		/// </summary>
		/// <param name="info">See <see cref="IPAFSerializationInfoCLS"/>.
		/// </param>
		/// <param name="context">
		/// Not used in this method.
		/// </param>
		[SecurityCritical]
// ReSharper disable UnusedParameter.Local
		protected internal PAFAbstractStandardExceptionBase(IPAFSerializationInfoCLS info,
			PAFSerializationContext context)
// ReSharper restore UnusedParameter.Local
			: this(context, info){}
		/// <summary>
		/// <see cref="PAFAbstractExceptionBase{T}"/>.
		/// </summary>
		/// <param name="context">
		/// Not used in this method.
		/// </param>
		/// <param name="info">See <see cref="IPAFSerializationInfoCLS"/>.
		/// </param>
		/// <remarks>
		/// Switch args so we don't have to define a dummy arg to build another constructor.
		/// </remarks>
		internal PAFAbstractStandardExceptionBase(PAFSerializationContext context, IPAFSerializationInfoCLS info)
			: base(context, info)
		{
			info.TryGetValue(DATA_TAG, out m_Data);
		}
		#endregion // Constructors
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
	}
}