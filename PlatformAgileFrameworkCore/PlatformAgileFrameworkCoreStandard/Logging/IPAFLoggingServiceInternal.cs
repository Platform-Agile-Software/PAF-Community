// Copied from Golea and namespaces modified.
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
using System.Collections.Generic;
using System;

#endregion // Using Directives

namespace PlatformAgileFramework.Logging
{
	/// <summary>
	///	Internal Interface for our logging service. This is made internal so as
	/// to try to indicate that it is not to to be used capriciously and so it's
	/// exposure is only to trusted assemblies. Extenders can expose this as public,
	/// we can't.
	/// </summary>
	/// <remarks>
	/// Used to allow access to the set of writers.
	/// </remarks>
	/// <threadsafety>
	/// Implementation-dependent.
	/// </threadsafety>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 22jan2011 </date>
	/// <description>
	/// Added history and better documentation.
	/// </description>
	/// </contribution>
	/// </history>
	// ReSharper disable once InconsistentNaming
	internal interface IPAFLoggingServiceInternal : IPAFLoggingService
	{
		#region Properties
		/// <summary>
		///	Gets the collection of writers.
		/// </summary>
		ICollection<Action<string>> LogWriters { get; }
		#endregion // Properties

		#region Methods
		/// <summary>
		/// Our reader that reads the log file.
		/// </summary>
		string ReadLogFile(string logFile);
		/// <summary>
		/// Sets the the header.
		/// </summary>
		void SetHeader(string header);
		/// <summary>
		/// This is the delegate we use if set.
		/// </summary>
		void SetFormattingDelegatee(LogFormatterDelegate logFormattingDelegate);
		/// <summary>
		/// This is the file to write to.
		/// </summary>
		void SetLogFile(string logFile);
		/// <summary>
		/// This is the current logging level.
		/// </summary>
		void SetLoggingLevel(PAFLoggingLevel loggingLevel);
		/// <summary>
		/// Enables timestamp.
		/// </summary>
		void SetTimeStampEnabled(bool enableTimeStamp);
		#endregion // Methods
	}
}