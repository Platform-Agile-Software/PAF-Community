
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
using Newtonsoft.Json;

namespace PlatformAgileFramework.Logging
{
    /// <summary>
    /// Utilities for logging.
    /// </summary>
    /// <history>
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
     public class PAFLoggingUtils
    {
        /// <summary>
        /// This one is processed by Newtonsoft JSON. Very simply converts the DTO into
        /// a string.
        /// </summary>
        /// <param name="message">
        /// A JSON DTO.
        /// </param>
        /// <param name="logLevel">Unused.</param>
        /// <param name="exception">Unused.</param>
        /// <param name="header">Unused.</param>
        /// <param name="enableTimeStamp">Unused.</param>
        /// <returns></returns>
        public static string NewtonsoftFormatLog(object message, PAFLoggingLevel logLevel,
            Exception exception, string header, bool enableTimeStamp)
        {
            var output = JsonConvert.SerializeObject(message);
            return output;
        }
    }
}
