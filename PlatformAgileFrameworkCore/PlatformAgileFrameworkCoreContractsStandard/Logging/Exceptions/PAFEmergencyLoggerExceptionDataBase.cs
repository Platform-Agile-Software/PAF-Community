//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2018 Icucom Corporation
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
using PlatformAgileFramework.ErrorAndException;

namespace PlatformAgileFramework.Logging.Exceptions
{
	/// <summary>
	/// Base class for "emergency logger" exceptions.
	/// See <see cref="IPAFEmergencyLoggerExceptionData"/>.
	/// </summary>
	public abstract class PAFEmergencyLoggerExceptionDataBase :
		PAFAbstractStandardExceptionDataBase, IPAFEmergencyLoggerExceptionData
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Backing for the prop.
		/// </summary>
		internal Exception m_EmergencyLoggingException;
		/// <summary>
		/// Backing for the prop.
		/// </summary>
		internal Exception m_OriginalException;
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Constructor builds with the standard arguments plus the
		/// <see cref="IPAFEmergencyLoggerExceptionData.EmergencyLoggingException"/> and
		/// <see cref="IPAFEmergencyLoggerExceptionData.OriginalException"/>.
		/// </summary>
		/// <param name="emergencyLoggingException">
		/// See <see cref="IPAFEmergencyLoggerExceptionData"/>.
		/// </param>
		/// <param name="originalException">
		/// See <see cref="IPAFEmergencyLoggerExceptionData"/>.
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
		protected PAFEmergencyLoggerExceptionDataBase(Exception emergencyLoggingException,
			Exception originalException, object extensionData = null,
			PAFLoggingLevel? pafLoggingLevel = null, bool?
			isFatal = null)
			: base(extensionData, pafLoggingLevel, isFatal)
		{
			m_EmergencyLoggingException = emergencyLoggingException;
			m_OriginalException = originalException;
		}
		#endregion Constructors
		#region Properties
		/// <summary>
		/// See <see cref="IPAFEmergencyLoggerExceptionData"/>.
		/// </summary>
		public Exception EmergencyLoggingException
		{
			get { return m_EmergencyLoggingException; }
			protected internal set { m_EmergencyLoggingException = value; }
		}
		/// <summary>
		/// See <see cref="IPAFEmergencyLoggerExceptionData"/>.
		/// </summary>
		public Exception OriginalException
		{
			get { return m_OriginalException; }
			protected internal set { m_OriginalException = value; }
		}
		#endregion // Properties
	}
}
