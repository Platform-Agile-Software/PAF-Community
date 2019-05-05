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
using System.Collections.Generic;
using PlatformAgileFramework.Collections.Enumerators;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.ErrorAndException
{
	/// <summary>
	///	Carries a list of exceptions. For security purposes, this data item contains a
	/// "sealable" list. If the list is sealed, no more exceptions may be added.
	/// </summary>
	[PAFSerializable]
	public sealed class PAFAggregateExceptionData : PAFAbstractStandardExceptionDataBase,
		IPAFAggregateExceptionData
	{
		#region Fields and Autoproperties
		/// <summary>
		/// The exceptions.
		/// </summary>
		internal IPAFSealableEnumerable<Exception> m_ExceptionItems;
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Constructor builds with the standard arguments plus exceptions.
		/// </summary>
		/// <param name="exceptions">
		/// Loads <see cref="Exceptions"/>. May be <see langword="null"/>. If this enumeration is not <see langword="null"/>
		/// and terminates with a <see langword="null"/> exception. no more exceptions may be added with the
		/// <see cref="AddException"/> method. The terminating <see langword="null"/> is not added to the list.
		/// </param>
		/// <param name="extensionData">
		/// Sets <see cref="IPAFStandardExceptionData.ExtensionData"/>.
		/// </param>
		/// <param name="pafLoggingLevel">
		/// See <see cref="PAFAbstractStandardExceptionDataBase"/>
		/// </param>
		/// <param name="isFatal">
		/// See <see cref="PAFAbstractStandardExceptionDataBase"/>
		/// </param>
		public PAFAggregateExceptionData(IEnumerable<Exception> exceptions = null, object extensionData = null,
			PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null )
			:base(extensionData, pafLoggingLevel, isFatal)
		{
			m_ExceptionItems = new PAFSealableEnumerable<Exception>(exceptions);
		}

		#endregion Constructors
		#region Properties
		/// <summary>
		/// <see cref="IPAFAggregateExceptionData"/>
		/// </summary>
		public IEnumerable<Exception> Exceptions
		{
			get { return m_ExceptionItems.Items; }
		}
		#endregion // Properties
		#region  Methods
		/// <summary>
		/// <see cref="IPAFAggregateExceptionData"/>
		/// </summary>
		/// <param name="exception">
		/// <see cref="IPAFAggregateExceptionData"/>
		/// </param>
		public void AddException(Exception exception)
		{
			m_ExceptionItems.AddItem(exception);
		}
		#endregion // Methods
	}
}