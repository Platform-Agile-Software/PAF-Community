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

#region Using Directives
using System;
using System.Security;
using PlatformAgileFramework.FrameworkServices;
#endregion // Using Directives

namespace PlatformAgileFramework.Logging
{
	#region Delegates
	/// <summary>
	/// Supports a pluggable formatting delegate.
	/// </summary>
	/// <param name="message">See <see cref="IPAFLoggingService.LogEntry"/>.</param>
	/// <param name="logLevel">See <see cref="IPAFLoggingService.LogEntry"/>.</param>
	/// <param name="exception">See <see cref="IPAFLoggingService.LogEntry"/>.</param>
	/// <param name="header">Optional header.</param>
	/// <param name="enableTimeStamp">Puts a time stamp under the header.</param>
	public delegate string LogFormatterDelegate(object message, PAFLoggingLevel logLevel,
		Exception exception, string header, bool enableTimeStamp);
	#endregion // Delegates
	/// <summary>
	///	Interface for our logging service. Extremely narrow interface for ease in
	/// mapping to other platforms and logging protocols. Framework extenders should
	/// feel free to extend this interface to encompass logging methods needed by
	/// specific applications. This interface, however, is designed to be the single
	/// "touch point" across platforms. Extension methods for this interface are the
	/// best way to add more methods here, if the user likes that sort of thing. 
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 22jan2011 </date>
	/// <description>
	/// Added history and documentation.
	/// </description>
	/// </contribution>
	/// </history>
	public interface IPAFLoggingService: IPAFService
	{
		#region Methods
		/// <summary>
		///	Logs the exception with a message.
		/// </summary>
		/// <param name="message">
		/// The message to log. <see cref="Object.ToString"/> gets the message
		/// in standard scenarios. If <see langword="null"/> or blank, message is pulled
		/// from the exception, if the exception is not <see langword="null"/>.
		/// </param>
		/// <param name="logLevel">
		/// The minimum level that must be enabled in this logger for the exception
		/// or message to be logged. Default = <see cref="PAFLoggingLevel.Default"/>.
		/// </param>
		/// <param name="exception">
		/// The exception. May be <c>null.</c>. Default = <see langword="null"/>.
		/// </param>
		/// <exception cref="ArgumentException">
		/// Thrown if <paramref name="message"/> and <see paramref="exception"/> are both
		/// <see langword="null"/> or blank.
		/// </exception>
		void LogEntry(object message, PAFLoggingLevel logLevel = PAFLoggingLevel.Default,
			Exception exception = null);
		#endregion Methods
	}
}