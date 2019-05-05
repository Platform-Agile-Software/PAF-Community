
//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2019 Icucom Corporation
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
using System.Globalization;
using System.Security;
using Newtonsoft.Json;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.Logging.Exceptions;
using PlatformAgileFramework.Platform;

// Exception shorthand
using IPAFELED = PlatformAgileFramework.Logging.Exceptions.IPAFEmergencyLoggerExceptionData;
using PAFELED = PlatformAgileFramework.Logging.Exceptions.PAFEmergencyLoggerExceptionData;

namespace PlatformAgileFramework.Logging
{
	/// <summary>
	/// Utilities for logging.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 04feb2019 </date>
	/// <description>
	/// Added JSONFormatForClose.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 02jan2019 </date>
	/// <description>
	/// Added <see cref="MessageOnlyFormatterDelegate"/> for Golea
	/// and others that want no format.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 08jul2018 </date>
	/// <description>
	/// Moved one method from ECMA into Netstandard.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// safe.
	/// </threadsafety>
	// ReSharper disable once PartialTypeWithSinglePart
	public static partial class PAFLoggingUtils
	{
		/// <summary>
		/// This is normally set from the outside from an ECMA assembly. It will
		/// be <see langword="null"/> if not set.
		/// </summary>
		public static IPAFTraceLoggerFactory TraceLoggerFactory { get; [SecurityCritical] set; }
		/// <summary>
		/// Our default formatter that is typically used if none plugged.
		/// </summary>
		/// <param name="message"><see cref="LogFormatterDelegate"/>.</param>
		/// <param name="logLevel"><see cref="LogFormatterDelegate"/></param>
		/// <param name="exception"><see cref="LogFormatterDelegate"/></param>
		/// <param name="header"><see cref="LogFormatterDelegate"/></param>
		/// <param name="enableTimeStamp"><see cref="LogFormatterDelegate"/></param>
		/// <returns>
		/// Formatted log entry string.
		/// </returns>
	    public static string DefaultFormatterDelegate(object message, PAFLoggingLevel logLevel,
		    Exception exception, string header, bool enableTimeStamp)
	    {
		    var lineOut = "";
		    if (header != null) lineOut = header + PlatformUtils.LTRMN;
		    var logTag = "[" + logLevel;
		    if (enableTimeStamp) logTag += " - " + DateTime.Now.ToString(CultureInfo.InvariantCulture);
		    logTag += "]" + PlatformUtils.LTRMN;
		    lineOut += logTag;
		    var output = message.ToString();
		    if (string.IsNullOrEmpty(output))
		    {
			    output = "";
		    }
		    lineOut += output + PlatformUtils.LTRMN;
		    if (exception != null) lineOut += ("Exception: " + exception.Message + PlatformUtils.LTRMN);
		    return lineOut;
	    }

	    /// <summary>
	    /// Formatter that accepts a list of serialized JSON objects, applies
	    /// proper separators and encloses the list in <c>"[]"</c>. This formatting
	    /// is needed before de-serializing the JSON-serialized text file. This
	    /// method is often called on a JSON file that has had multiple,
	    /// independent objects written to it.
	    /// </summary>
	    /// <param name="jsonObjectList">
	    /// Concatenated JSON-serialized strings <c>{}{}{} .. {}</c>.
	    /// </param>
	    /// <returns>
	    /// Formatted string in the form <c>[{},{},{}, ... {}]</c>
	    /// </returns>
	    /// <exceptions>
	    /// <exception cref="ArgumentNullException"><paramref name="jsonObjectList"/>.</exception>
	    /// </exceptions>
	    public static string JSONObjectListFormatForClose(
		    string jsonObjectList)
	    {
		    if (string.IsNullOrEmpty(jsonObjectList))
		    {
			    throw new ArgumentNullException(nameof(jsonObjectList));
		    }

		    var separatedObjects = jsonObjectList.Replace("}{", "},{");

		    var enclosedList = $"[{separatedObjects}]";

		    return enclosedList;
	    }
	    /// <summary>
		/// Formatter that outputs just the message as a <see cref="string"/>.
		/// </summary>
		/// <param name="message"><see cref="LogFormatterDelegate"/>.</param>
		/// <param name="logLevel">
		/// <see cref="LogFormatterDelegate"/>. Not used.
		/// </param>
		/// <param name="exception">
		/// <see cref="LogFormatterDelegate"/>. Not used.
		/// </param>
		/// <param name="header">
		/// <see cref="LogFormatterDelegate"/>. Not used.
		/// </param>
		/// <param name="enableTimeStamp">
		/// <see cref="LogFormatterDelegate"/>. Not used.
		/// </param>
		/// <returns>
		/// Formatted log entry string.
		/// </returns>
		public static string MessageOnlyFormatterDelegate(object message, PAFLoggingLevel logLevel,
		    Exception exception, string header, bool enableTimeStamp)
	    {
		    var output = message.ToString();
		    if (string.IsNullOrEmpty(output))
		    {
			    output = "";
		    }
		    return output;
	    }
	    /// <summary>
	    /// This one is processed by Newtonsoft JSON. Very simply converts the DTO into
	    /// a string.
	    /// </summary>
	    /// <param name="message">
	    /// A JSON DTO.
	    /// </param>
	    /// <param name="logLevel">Unused.</param>
	    /// <param name="exception">Unused.</param>
	    /// <param name="header">
	    /// This is text that is prepended to the log output string if not
	    /// <see langword="null"/>. It is normally a comma in JSON
	    /// logs, since this is a needed separator between JSON objects
	    /// in a composite JSON object.
	    /// </param>
	    /// <param name="enableTimeStamp">Unused.</param>
	    /// <returns>
	    /// The formatted output string.
	    /// </returns>
	    public static string NewtonsoftFormatLog(object message, PAFLoggingLevel logLevel,
		    Exception exception, string header, bool enableTimeStamp)
	    {
		    var output = "";
		    if (!string.IsNullOrEmpty(header))
			    output += header;
		    output += JsonConvert.SerializeObject(message);
		    return output;
	    }
	    /// <summary>
	    /// Logs a logger's disablement to the emergency logger. Unified one-offs
	    /// of this procedure in one place. The disablement is assumed to be caused
	    /// by a non-recoverable error.
	    /// </summary>
	    /// <param name="exception">
	    /// Exception that was generated by the logging attempt. May be <see langword="null"/>
	    /// if the shutdown was not triggered by an exception.
	    /// </param>
	    /// <param name="isDisabled">
	    /// This parameter is provided here to emphasize correct usage of this method. Since
	    /// this method should be called ONLY once by any logger, we put the disablement
	    /// mechanism explicitly is this method.
	    /// </param>
	    /// <exceptions>
	    /// <exception cref="PAFStandardException{T}">
	    /// <see cref="PAFEmergencyLoggerExceptionMessageTags.ERROR_WRITING_WITH_EMERGENCY_LOGGER"/>
	    /// if emergency logger doesn't even work to log main logger failure.
	    /// </exception>
	    /// </exceptions>
	    public static void ReportDisabledLogger(Exception exception, out bool isDisabled)
	    {
		    isDisabled = true;
		    Exception emergencyLoggerLoggingException = null;
		    try
		    {
			    var provider = PAFServiceManagerContainer.ServiceManager
				    .GetTypedService<IPAFEmergencyServiceProvider<IPAFLoggingService>>(false, null);
			    provider.EmergencyService.LogEntry("LoggerFatalFailure",
				    PAFLoggingLevel.Error, exception);
		    }
		    catch (Exception ex)
		    {
			    emergencyLoggerLoggingException = ex;
		    }

		    if (emergencyLoggerLoggingException == null) return;
		    // If the emergency logger is broke, we MUST fail the app.
		    var exceptionData
			    = new PAFELED(emergencyLoggerLoggingException, null);
		    throw new PAFStandardException<IPAFELED>(exceptionData, PAFEmergencyLoggerExceptionMessageTags.ERROR_WRITING_WITH_EMERGENCY_LOGGER);
	    }
	}
}
