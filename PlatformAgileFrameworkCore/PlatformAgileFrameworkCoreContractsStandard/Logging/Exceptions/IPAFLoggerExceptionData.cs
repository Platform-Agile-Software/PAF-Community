//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2019 Icucom Corporation
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
	///	General logging exception built when rolling logger was put into production.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 09jan2019 </date>
	/// <description>
	/// New.
	/// </description>
	/// </contribution>
	/// </history>
	[PAFSerializable]
	public interface IPAFLoggerExceptionData: IPAFStandardExceptionData
	{
		#region Properties
		/// <summary>
		/// The file or destination which we are having trouble writing
		/// to.
		/// </summary>
		string ProblematicLogFile { get; }
		#endregion // Properties
	}

	/// <summary>
	/// Set of tags with an enumerator for exception messages. These are the dictionary keys
	/// for extended.
	/// </summary>
	public class PAFLoggerExceptionMessageTags
		: PAFExceptionMessageTagsBase<IPAFEmergencyLoggerExceptionData>
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Issued when the message is too large.
		/// </summary>
		public const string LOG_STORAGE_OVERFLOW = "Log storage overflow";

		#endregion // Fields and Autoproperties
		/// <summary>
		/// Just puts the tags in a list to hand out.
		/// </summary>
		static PAFLoggerExceptionMessageTags()
		{
			if ((s_Tags != null) && (s_Tags.Count > 0)) return;
			s_Tags = new List<string>
				{
					LOG_STORAGE_OVERFLOW
				};
			PAFAbstractStandardExceptionDataBase.RegisterNamedExceptionTagsInternal(s_Tags, typeof(IPAFLoggerExceptionData));
		}

	}
}