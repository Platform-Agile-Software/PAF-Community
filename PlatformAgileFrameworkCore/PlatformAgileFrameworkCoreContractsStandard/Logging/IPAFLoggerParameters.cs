// copied from Golea
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
// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.Logging
{
	/// <summary>
	///	Interface for the "magic" object in the PAF logger to customize it for a given call. 
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 16apr2016 </date>
	/// <description>
	/// Added custom logger file because I needed it to debug the updated
	/// framework. Talk about a cool use case!
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 22jan2011 </date>
	/// <description>
	/// Added history and documentation.
	/// </description>
	/// </contribution>
	/// </history>
	// ReSharper disable once InconsistentNaming
	public interface IPAFLoggerParameters
	{
		#region Properties
		/// <summary>
		/// This is a log file that is used to write output. May be <see langword="null"/>.
		/// </summary>
		string OutputFileName { get; }
		/// <summary>
		/// Enable date stamp under header if there is one. For an actual general logger
		/// implementation, anything but <see langword="false"/> would enable time stamps.
		/// </summary>
		bool? EnableTimeStamp { get; }
		/// <summary>
		/// This is a header that is placed above every log output. It goes before the
		/// timestamp, if there is one. May be <see langword="null"/>.
		/// </summary>
		string Header { get; }
		/// <summary>
		/// This is the logging level to be used.
		/// </summary>
		PAFLoggingLevel LoggingLevel { get; }
		#endregion // Properties
	}
}
