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
using System.Collections.Generic;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.ErrorAndException.CoreCustomExceptions
{
	/// <summary>
	///	Exceptions that occur during construction, mostly triggered during reflection
	/// operations.
	/// </summary>
	/// <remarks>
	/// Need for aggregation on this exception type, so interface is defined.
	/// </remarks>
	[PAFSerializable]
	public sealed class PAFConstructorExceptionData :
		PAFAbstractStandardExceptionDataBase, IPAFConstructorExceptionData
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Backing for the prop.
		/// </summary>
		internal PAFTypeHolder m_ConstructionFailureType;
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Constructor builds with the standard arguments plus the <see cref="PAFTypeHolder"/>.
		/// </summary>
		/// <param name="constructionFailureType">
		/// Loads <see cref="ConstructionFailureType"/>. May be <see langword="null"/>.
		/// </param>
		/// <param name="extensionData">
		/// Sets <see cref="IPAFStandardExceptionData.ExtensionData"/>.
		/// </param>
		/// <param name="isFatal">
		/// See <see cref="PAFAbstractStandardExceptionDataBase"/>
		/// </param>
		/// <param name="pafLoggingLevel">
		/// See <see cref="PAFAbstractStandardExceptionDataBase"/>
		/// </param>
		public PAFConstructorExceptionData(PAFTypeHolder constructionFailureType = null, object extensionData = null,
			PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
			: base(extensionData, pafLoggingLevel, isFatal)
		{
			m_ConstructionFailureType = constructionFailureType;
		}
		#endregion Constructors
		#region Properties
		/// <summary>
		/// Representation of the type the finalizer was called on.
		/// </summary>
		public PAFTypeHolder ConstructionFailureType
		{
			get { return m_ConstructionFailureType; }
			internal set { m_ConstructionFailureType = value; }
		}
		#endregion // Properties
	}
}