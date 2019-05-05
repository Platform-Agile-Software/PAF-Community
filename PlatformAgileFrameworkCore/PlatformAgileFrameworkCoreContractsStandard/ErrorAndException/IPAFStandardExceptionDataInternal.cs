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
using PlatformAgileFramework.Logging;

namespace PlatformAgileFramework.ErrorAndException
{
	/// <summary>
	///	The base interface which must be implemented by all Standard PAF exceptions
	/// if members are to be settable by the framework. Required for PAF surrogate
	/// serialization.
	/// </summary>
	/// <history>
	/// <author> BMC </author>
	/// <date> 22apr2011 </date>
	/// <contribution>
	/// Refactored this out of the monolithic program so Framework builders could have
	/// an extensibility capability.
	/// </contribution>
	/// </history>
	internal interface IPAFStandardExceptionDataInternal : IPAFStandardExceptionData
	{
		#region Methods
		/// <summary>
		///	Sets the extension data.
		/// </summary>
		void SetExtensionData_Internal(object extensionData);
		/// <summary>
		///	Sets a value indicating that this exception is fatal. Fatal
		/// exceptions halt application execution. Defaults to <see langword="null"/>,
		/// which indicates no override of default behavior.
		/// </summary>
		void SetIsFatal_Internal(bool? isFatal);
		/// <summary>
		///	Sets the log level. This indicates if and when this exception is logged. 
		///	Defaults to <see langword="null"/>, indicating no override of default logging level.
		/// </summary>
		void SetLoggingLevel_Internal(PAFLoggingLevel? pafLoggingLevel);
		#endregion Methods
	}
}