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
using System.Collections.Generic;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.ErrorAndException.CoreCustomExceptions;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.Logging.Exceptions
{
	/// <summary>
	///	Exceptions that occur using the emergency logger. Some applications
	/// (and clients) that we support have a hard requirement for an
	/// "emergency logger". This class of exceptions supports this requirement.
	/// Since the emergency logger is the last line of defense for logging
	/// exceptions, these exceptions are normally allowed to bubble up to
	/// some application layer where the application is shut down. An application
	/// which needs an emergency logger should normally test the logger in
	/// the "bootstrap" phase of the application, so the error is resolutely
	/// caught early on.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 23dec2018 </date>
	/// <description>
	/// New. Added explicit support for the emergency logger for back port
	/// to Golea project.
	/// </description>
	/// </contribution>
	/// </history>
	[PAFSerializable]
	public interface IPAFEmergencyLoggerExceptionData: IPAFStandardExceptionData
	{
		#region Properties
		/// <summary>
		/// The exception which occurred when we were attempting to
		/// log with the emergency logger. May be <see langword="null"/>.
		/// </summary>
		Exception EmergencyLoggingException { get; }
		/// <summary>
		/// The original exception, which we were attempting to log with
		/// the emergency logger. May be <see langword="null"/>.
		/// </summary>
		Exception OriginalException { get; }
		#endregion // Properties
	}

	/// <summary>
	/// Set of tags with an enumerator for exception messages. These are the dictionary keys
	/// for extended.
	/// </summary>
	public class PAFEmergencyLoggerExceptionMessageTags
		: PAFExceptionMessageTagsBase<IPAFEmergencyLoggerExceptionData>
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Issued when there is an exception writing with the emergency logger.
		/// </summary>
		public const string ERROR_WRITING_WITH_EMERGENCY_LOGGER = "Error writing with emergency logger";
		/// <summary>
		/// Issued when there is no installed emergency logger or the application can't find it.
		/// </summary>
		public const string CANNOT_LOAD_EMERGENCY_LOGGER = "Cannot load emergency logger";
		/// <summary>
		/// Issued when the main logger can't be created in the startup protocol.
		/// </summary>
		public const string CANNOT_CREATE_MAIN_LOGGER = "Cannot create main logger";

		#endregion // Fields and Autoproperties
		/// <summary>
		/// Just puts the tags in a list to hand out.
		/// </summary>
		static PAFEmergencyLoggerExceptionMessageTags()
		{
			if ((s_Tags != null) && (s_Tags.Count > 0)) return;
			s_Tags = new List<string>
				{
					ERROR_WRITING_WITH_EMERGENCY_LOGGER,
					CANNOT_LOAD_EMERGENCY_LOGGER,
					CANNOT_CREATE_MAIN_LOGGER
				};
			PAFAbstractStandardExceptionDataBase.RegisterNamedExceptionTagsInternal(s_Tags, typeof(IPAFEmergencyLoggerExceptionData));
		}

	}
}