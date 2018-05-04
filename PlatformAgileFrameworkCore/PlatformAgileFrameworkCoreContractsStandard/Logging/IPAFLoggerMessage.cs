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
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-
// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.Logging
{
	/// <summary>
	/// This is a paremeter set to be passed as the "magic" object in the
	/// logger to customize it for a given call.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> Brian T. </author>
	/// <date> 02jun2015 </date>
	/// <description>
	/// Copied from Golea.
	/// </description>
	/// </contribution>
	/// </history>
	/// <remarks>
	/// This is one of many trapdoors that could be constructed.
	/// </remarks>
	// ReSharper disable once InconsistentNaming
	public interface IPAFLoggerMessage
	{
		#region Properties
		/// <summary>
		/// This gets the parameters for the logger. For a client to override
		/// behavior locally.
		/// </summary>
		IPAFLoggerParameters LoggerParameters { get; }
		/// <summary>
		/// This is the message to be logged.
		/// </summary>
		object LogMessage { get; }
		#endregion // Properties
	}
}