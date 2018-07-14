
//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2018 Icucom Corporation
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

namespace PlatformAgileFramework.Logging
{
    /// <summary>
    /// Pre-canned version of JSON logger that just takes a DTO and writes it out.
    /// </summary>
    /// <history>
    /// <contribution>
    /// <author> KRM </author>
    /// <date> 09jun2018 </date>
    /// <description>
    /// Built a simple JSON logger for client use.
    /// </description>
    /// </contribution>
    /// </history>
    /// <threadsafety>
    /// See base class
    /// </threadsafety>
    public class PAFJSONDTOLoggingService : PAFLoggingService
    {
        #region Constructors

        /// <summary>
        /// We plug in the JSONDTO formatter.
        /// </summary>
         public PAFJSONDTOLoggingService() : base(PAFLoggingLevel.Error)
        {
            FormatterDelegate = PAFLoggingUtils.NewtonsoftFormatLog;
        }

        /// <summary>
        /// Constructor allows setting of time stamp and header, etc.
        /// </summary>
        /// <param name="loggingLevel">
        /// See base.
        /// </param>
        /// <param name="enableTimeStamp">
        /// See base.
        /// </param>
        /// <param name="header">
        /// See base.
        /// </param>
        /// <param name="logFile">
        /// See base.
        /// </param>
        public PAFJSONDTOLoggingService
        (PAFLoggingLevel loggingLevel = PAFLoggingLevel.Error,
            bool enableTimeStamp = true, string header = null, string logFile = null)
            : base(loggingLevel, enableTimeStamp, header, logFile)
        {
            FormatterDelegate = PAFLoggingUtils.NewtonsoftFormatLog;
        }
        #endregion // Constructors
    }
}
