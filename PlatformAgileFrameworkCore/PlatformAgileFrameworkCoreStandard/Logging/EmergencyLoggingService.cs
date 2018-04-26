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
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.FrameworkServices;

// Exception shorthand.
using PAFSED = PlatformAgileFramework.FrameworkServices.Exceptions.PAFServiceExceptionData;
using PAFSEDB = PlatformAgileFramework.FrameworkServices.Exceptions.PAFServiceExceptionDataBase;


namespace PlatformAgileFramework.Logging
{
	/// <summary>
	/// Sealed version of <see cref="EmergencyLoggingServiceBase"/> that also
	/// allows <see cref="EmergencyLoggingServiceBase.CanRunWithoutMain"/> to be set.
	/// </summary>
	/// <history>
	/// <author> DAP </author>
	/// <date> 21jun2011 </date>
	/// <contribution>
	/// New.
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// See base class.
	/// </threadsafety>
	public sealed class EmergencyLoggingService : EmergencyLoggingServiceBase
	{
		#region Constructors
		/// <remarks>
		/// Parameterless constructor calls base with defaults.
		/// Security critical, since we don't want underpriveliged callers to construct this.
		/// </remarks>
		[SecurityCritical]
		public EmergencyLoggingService()
			: base(null, null)
		{
			CanRunWithoutMain = false;
		}

		/// <summary>
		/// See <see cref="EmergencyLoggingServiceBase"/>. This constructor allows us to use
		/// the emergency logger forever.
		/// </summary>
		/// <param name="canRunWithoutMain">
		/// Sets <see cref="EmergencyLoggingServiceBase.CanRunWithoutMain"/>. Set this to
		/// <see langword="true"/> to not throw an exception if a "main" logger cannot
		/// be constructed.
		/// </param>
		/// <param name="mainServiceDescription">
		/// See <see cref="EmergencyLoggingServiceBase"/>.
		/// </param>
		/// <param name="emergencyLogFilePath">
		/// See <see cref="EmergencyLoggingServiceBase"/>.
		/// </param>
		///  <exception> <see cref="PAFStandardException{PAFSED}"/> is thrown if
		///  an instance of the emergency logger is already constructed.
		///  <see>
		///      <cref>PAFServiceExceptionData.SERVICE_ALREADY_CREATED</cref>
		///  </see>
		///      .
		///  </exception>
		/// <remarks>
		/// Security critical, since we don't want underpriveliged callers to construct this.
		/// </remarks>
		[SecurityCritical]
		public EmergencyLoggingService(bool canRunWithoutMain,
			IPAFServiceDescription mainServiceDescription = null,
			string emergencyLogFilePath = null)
			: base(mainServiceDescription, emergencyLogFilePath)
		{
			CanRunWithoutMain = canRunWithoutMain;
		}

		/// <summary>
		/// See <see cref="EmergencyLoggingServiceBase"/>. This is just a
		/// constructor for the sealed class - internal version.
		/// </summary>
		/// <param name="mainServiceDescription">
		/// See <see cref="EmergencyLoggingServiceBase"/>.
		/// </param>
		/// <param name="emergencyLogFilePath">
		/// See <see cref="EmergencyLoggingServiceBase"/>.
		/// </param>
		///  <exception> <see cref="PAFStandardException{PAFSED}"/> is thrown if
		///  an instance of the emergency logger is already constructed.
		///  <see>
		///      <cref>PAFServiceExceptionData.SERVICE_ALREADY_CREATED</cref>
		///  </see>
		///      .
		///  </exception>
		internal EmergencyLoggingService(IPAFServiceDescription mainServiceDescription = null,
			string emergencyLogFilePath = null)
			: base(mainServiceDescription, emergencyLogFilePath)
		{
		}

		#endregion // Constructors
	}
}