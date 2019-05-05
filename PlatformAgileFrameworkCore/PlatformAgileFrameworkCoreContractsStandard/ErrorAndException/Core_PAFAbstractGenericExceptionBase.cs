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

using System;
using System.Security;
using PlatformAgileFramework.Serializing.Attributes;
using PlatformAgileFramework.Serializing.ECMAReplacements;

namespace PlatformAgileFramework.ErrorAndException
{
	/// <summary>
	/// Default implementation of <see cref="IPAFExceptionBase{T}"/>.
	/// </summary>
	/// <typeparam name="T">
	/// <see cref="IPAFExceptionBase{T}"/>.
	/// </typeparam>
	/// <remarks>
	/// <para>
	/// Partial class for extension in CLR and extended library.
	/// </para>
	/// <para>
	/// Noted that it's best to close the class at compile time, not dynamically at runtime,
	/// since tools can be used to report on serializability of <typeparamref name="T"/>
	/// at compile time. The alternative is the throwing of an exception during the
	/// creation of an exception, which is problematic.
	/// </para>
	/// <para>
	/// Noted: If we are never sending these exceptions out of an AppDomain, we don't
	/// care about serializability, necessarily. Use <see cref="PAFAbstractSerializableExceptionBase{T}"/>
	/// if you need a serializability check.
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
	public abstract partial class PAFAbstractExceptionBase<T>
		: PAFAbstractExceptionBase, IPAFExceptionBase<T>
	// ReSharper restore PartialTypeWithSinglePart
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Internal for the surrogate.
		/// </summary>
		protected internal T m_Data;
		/// <summary>
		/// Indexer for the data.
		/// </summary>
		protected internal const string DATA_TAG = "DATA_TAG"; 
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="PAFAbstractExceptionBase"/> class.
		/// </summary>
		/// <param name="data">
		/// This is the Generic data that must be serializable, either inherently through
		/// serializability attributes or interfaces or having a surrogate installed.
		/// </param>
		/// <param name="message"><see cref="Exception"/>.</param>
		/// <param name="innerException"><see cref="Exception"/>.</param>
		protected PAFAbstractExceptionBase(T data, string message = null, Exception innerException = null): base(message, innerException)
		{
			m_Data = data;
		}

		/// <summary>
		/// Protected for subclasses. Security critical for elevated priviledge environments.
		/// Calls internal constructor. This is the standard constructor that is used in the
		/// legacy .Net serializer.
		/// </summary>
		/// <param name="info">See <see cref="IPAFSerializationInfoCLS"/>.
		///</param>
		/// <param name="context">
		/// Not used in this method.
		/// </param>
// ReSharper disable UnusedParameter.Local
		protected internal PAFAbstractExceptionBase(IPAFSerializationInfoCLS info,
			PAFSerializationContext context)
// ReSharper restore UnusedParameter.Local
			: this(context, info){}
		/// <summary>
		/// Internal for the surrogate. Builds basic exception. We don't check for
		/// the content of <typeparamref name="T"/>. It can be "default(T)".
		/// </summary>
		/// <param name="context">
		/// Not used in this method.
		/// </param>
		/// <param name="info">See <see cref="IPAFSerializationInfoCLS"/>.
		///</param>
		/// <remarks>
		/// Switch args so we don't have to define a dummy arg to build another constructor.
		/// </remarks>
		internal PAFAbstractExceptionBase(PAFSerializationContext context, IPAFSerializationInfoCLS info)
			: base(context, info)
		{
			info.TryGetValue(DATA_TAG, out m_Data);
		}
		#endregion // Constructors
		#region Methods
        /// <summary>
        /// This method returns the generic exception data that the client has constructed
        /// the exception with.
        /// </summary>
        /// <returns>
        /// The Generic data item.
        /// </returns>
        public virtual T GetExceptionDataItem()
        {
            return m_Data;
        }
        /// <summary>
        /// This method returns the non-Generic exception data that the client has constructed
        /// the exception with.
        /// </summary>
        /// <returns>
        /// The object data item.
        /// </returns>
        public override object GetExceptionData()
        {
            return GetExceptionDataItem();
        }
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