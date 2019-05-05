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

using PlatformAgileFramework.ErrorAndException;


namespace PlatformAgileFramework.Logging
{
	/// <summary>
	/// Test logger version of <see cref="EmergencyLoggingServiceBase"/>.
	/// </summary>
	/// <history>
	/// <author> DAP </author>
	/// <date> 01jun2012 </date>
	/// <contribution>
	/// New.
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// See base class.
	/// </threadsafety>
	public sealed class PlainTextEmergencyLoggingService : EmergencyLoggingServiceBase
	{
		#region Constructors
		/// <summary>
		/// See <see cref="EmergencyLoggingServiceBase"/>. This is just a
		/// constructor for this sealed class that sets up props on the base
		/// to use it as a permanent logger. This logger is normally used for testing.
		/// </summary>
		/// <param name="truncateFileOnStart">
		/// Sets <see cref="EmergencyLoggingServiceBase.TruncateFileOnStart"/>.
		/// </param>
		/// <param name="emergencyLogFilePath">
		/// See <see cref="EmergencyLoggingServiceBase"/>.
		/// </param>
		/// <param name="headerPrefixText">
		/// Sets <see cref="EmergencyLoggingServiceBase.m_HeaderPrefixText"/>.
		/// Default = "Log Entry".
		/// </param>
		/// <exception> <see cref="PAFStandardException{PAFSED}"/> is thrown if
		///  an instance of the emergency logger is already constructed.
		///  <see>
		///      <cref>PAFServiceExceptionData.SERVICE_ALREADY_CREATED</cref>
		///  </see>
		///      .
		///  </exception>
		internal PlainTextEmergencyLoggingService(bool truncateFileOnStart = false,
			string emergencyLogFilePath = null, string headerPrefixText = null)
			: base(null, emergencyLogFilePath, truncateFileOnStart, headerPrefixText)
		{
			TruncateFileOnStart = truncateFileOnStart;

			// Set up to use ourselves as main.
			MainService = this;

			ObjectName = "PlainTextInRoot";
		}

		#endregion // Constructors
	}
}