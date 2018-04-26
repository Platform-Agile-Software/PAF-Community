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
#endregion // Using Directives

namespace PlatformAgileFramework.Logging
{
	/// <summary>
	///	Indicates the log level for the logging event.
	/// </summary>
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
	public enum PAFLoggingLevel
	{
		/// <summary>Used to indicate no level CHANGE.</summary>
		Unset = -1,
		/// <summary>
		/// Logging is off. Normally useful for unit tests so the logger time can be
		/// eliminated, with the test framework doing the logging.
		/// </summary>
		Off = 0,
		/// <summary>Only fatal events are logged.</summary>
		Fatal = 1,
		/// <summary>Only fatal events and error events are logged.</summary>
		Error = 2,
		/// <summary>Fatal, error, warning events are logged.</summary>
		Warn = 3,
		/// <summary>Informative, fatal, error, warning events are logged.</summary>
		Info = 4,
		/// <summary>Everything is logged. Used for debugging.</summary>
		Verbose = 5,
		/// <summary>Uses the default level for the logging service.</summary>
		Default = 6,
	}
}