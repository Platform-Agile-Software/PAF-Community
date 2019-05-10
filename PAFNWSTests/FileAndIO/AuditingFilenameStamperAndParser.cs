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

#region Using Directives
using System;
using System.Collections.Generic;
#endregion // Using Directives

namespace PlatformAgileFramework.FileAndIO
{
	/// <summary>
	/// Test version of the S&P that audits what we do.
	/// </summary>
	public class AuditingFilenameStamperAndParser : PAFFilenameStamperAndParser
	{
		/// <summary>
		/// Collects all the date time and versions we have parsed.
		/// </summary>
		// ReSharper disable once InconsistentNaming
		public static List<DateTimeAndVersion> s_DTAV = new List<DateTimeAndVersion>();
		/// <summary>
		/// Collects all the filenames we have produced.
		/// </summary>
		public static List<string> s_FileNames = new List<string>();
		/// <summary>
		/// Constructor loads properties.
		/// </summary>
		/// <param name="fileBaseName"> See base class.</param>
		/// <param name="dateTimeFormatString">
		/// See base class.
		/// </param>
		/// <param name="addMilliseconds">See base class.</param>
		public AuditingFilenameStamperAndParser(string fileBaseName, string dateTimeFormatString = null,
			bool addMilliseconds = true)
		:base(fileBaseName, dateTimeFormatString, addMilliseconds)
		{
		}
		public override DateTimeAndVersion ParseFilename( string fullFilePathWithoutExtension)
		{
			var dtav = base.ParseFilename(fullFilePathWithoutExtension);
			s_DTAV.Add(dtav);
			return dtav;
		}
		public override string StampFilename(string fullFilePathWithoutExtension,
			DateTime datetimeToStamp = default(DateTime), int fileVersion = -1)
		{
			var fileName = base.StampFilename(fullFilePathWithoutExtension,
			datetimeToStamp,fileVersion);

			s_FileNames.Add(fileName);
			return fileName;
		}
	}
}
