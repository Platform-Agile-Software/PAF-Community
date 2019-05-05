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
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.FrameworkServices.Exceptions
{
	/// <summary>
	/// Base class for service exceptions.
	/// </summary>
// ReSharper disable PartialTypeWithSinglePart
	public abstract partial class PAFServiceTypeMismatchExceptionDataBase :
// ReSharper restore PartialTypeWithSinglePart
		PAFServiceExceptionDataBase, IPAFServiceTypeMismatchExceptionData
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Backing for the prop.
		/// </summary>
		internal IPAFTypeHolder m_RequiredType;
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Constructor builds with the standard arguments plus the
		/// <see cref="IPAFServiceExceptionData.ProblematicService"/>
		/// and the
		/// <see cref="IPAFServiceTypeMismatchExceptionData.RequiredType"/>.
		/// </summary>
		/// <param name="problematicService">
		/// See <see cref="IPAFServiceExceptionData"/>.
		/// </param>
		/// <param name="requiredType">
		/// See <see cref="IPAFServiceTypeMismatchExceptionData"/>.
		/// </param>
		/// <param name="extensionData">
		/// See <see cref="PAFAbstractStandardExceptionDataBase"/>
		/// </param>
		/// <param name="pafLoggingLevel">
		/// See <see cref="PAFAbstractStandardExceptionDataBase"/>
		/// </param>
		/// <param name="isFatal">
		/// See <see cref="PAFAbstractStandardExceptionDataBase"/>
		/// </param>
		protected PAFServiceTypeMismatchExceptionDataBase(
			IPAFServiceDescription problematicService,
			IPAFTypeHolder requiredType, object extensionData = null,
			PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
			: base(problematicService, extensionData, pafLoggingLevel, isFatal)
		{
			m_RequiredType = requiredType;
		}
		#endregion Constructors
		#region Properties
		/// <summary>
		/// See <see cref="IPAFServiceTypeMismatchExceptionData"/>.
		/// </summary>
		public IPAFTypeHolder RequiredType
		{
			get { return m_RequiredType; }
			protected internal set { m_RequiredType = value; }
		}
		#endregion // Properties
	}
}
