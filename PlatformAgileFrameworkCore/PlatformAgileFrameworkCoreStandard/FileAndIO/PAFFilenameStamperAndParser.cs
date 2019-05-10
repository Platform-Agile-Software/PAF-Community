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
using System.Threading.Tasks;
using PlatformAgileFramework.Annotations;
using PlatformAgileFramework.StringParsing;
#endregion // Using Directives

namespace PlatformAgileFramework.FileAndIO
{
	/// <summary>
	/// Commonly-used implementation of <see cref="IPAFFilenameStamperAndParser"/>.
	/// This one is designed for the file format <c>BASEFILENAME_yyyy-MM-dd_HH-mm-ss_(n).ext</c>
	/// where n is the version number. Now also deals with <c>BASEFILENAME_yyyy-MM-dd_HH-mm-ss-zzz_(n).ext</c>,
	/// optionally. Milliseconds need not be present, however. USE MILLISECONDS IN MULTI-THREADED SCENARIOS
	/// TO AVOID FILENAME COLLISIONS!
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 03may2019 </date>
	/// <description>
	/// Had to put in the delay to avoid filename collisions in multi-threaded scenarios where
	/// file names may be generated simultaneously on separate threads. This should actually be
	/// handled in the app, but it probably won't be, so we put in the protection here.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> Brian T. </author>
	/// <date> 07dec2018 </date>
	/// <description>
	/// New. Built for Golea, but we are using this because it's as good as anything else
	/// so it's now our default.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Safe.
	/// </threadsafety>
	/// <remarks>
	/// This is a client-specific implementation, but is a typical date/time/version format.
	/// The date/time and version stamp were best handled jointly.
	/// </remarks>
	public class PAFFilenameStamperAndParser : IPAFFilenameStamperAndParser
	{
		/// <summary>
		/// This is a monitor lock object that is used to provide a two-millisecond delay in
		/// generating a date/time stamp. This is needed in the case of concurrent callers
		/// which may hit this class simultaneously.
		/// </summary>
		private object m_LockObject = new object();
		/// <summary>
		/// Constructor loads properties.
		/// </summary>
		/// <param name="fileBaseName"> Loads <see cref="FileBaseName"/></param>
		/// <param name="dateTimeFormatString">
		/// Loads <see cref="DateTimeFormatString"/>.
		/// Default is <see cref="PAFFileUtils.DEFAULT_FILE_DATE_TIME_FORMAT_STRING"/>.
		/// </param>
		/// <param name="addMilliseconds">Loads <see cref="AddMilliseconds"/></param>
		public PAFFilenameStamperAndParser(string fileBaseName, string dateTimeFormatString = null,
			bool addMilliseconds = true)
		{
			FileBaseName = fileBaseName;
			DateTimeFormatString = dateTimeFormatString;
			if (DateTimeFormatString == null)
				DateTimeFormatString = PAFFileUtils.DEFAULT_FILE_DATE_TIME_FORMAT_STRING;
			AddMilliseconds = addMilliseconds;
		}
		/// <summary>
		/// <see cref="IPAFFilenameStamperAndParser"/>
		/// </summary>
		public string DateTimeFormatString { get; protected internal set; }
		/// <summary>
		/// <see cref="IPAFFilenameStamperAndParser"/>
		/// </summary>
		public string FileBaseName { get; protected internal set; }
		/// <summary>
		/// <see cref="IPAFFilenameStamperAndParser"/>. This has a setter
		/// that is NOT part of the interface for security reasons. It allows the
		/// versioning to be controlled entirely externally, simplifying the file
		/// writers. Default = <see langword="null"/>.
		/// </summary>
		public int? FileVersion { get; set; } = null;
		/// <summary>
		/// <see cref="IPAFFilenameStamperAndParser"/>
		/// </summary>
		public bool AddMilliseconds { get; protected internal set; }
		public virtual DateTimeAndVersion ParseFilename([NotNull] string fullFilePathWithoutExtension)
		{
			var dtvHolder = new DateTimeAndVersion();
			dtvHolder.FileDateTime = PAFFileUtils.DefaultGetDateTimeFromFilename
				(fullFilePathWithoutExtension, FileBaseName);

			// See if we have a version on the end.
			var expectedClosingParenPosition = fullFilePathWithoutExtension.Length - 1;
			if (fullFilePathWithoutExtension[expectedClosingParenPosition] != ')')
				return dtvHolder;

			var numberString
				= StringParsingUtils.GetNumberInSpanBackwards(
					fullFilePathWithoutExtension.Substring(0, expectedClosingParenPosition));

			if (string.IsNullOrEmpty(numberString))
			{
				// Got nothing...
				dtvHolder.FileVersion = -1;
				return dtvHolder;
			}

			// parse the valid number string and install.
			dtvHolder.FileVersion = int.Parse(numberString);
			return dtvHolder;
		}
		/// <param name="fullFilePathWithoutExtension">
		/// <see cref="IPAFFilenameStamperAndParser"/>
		/// </param>
		/// <param name="datetimeToStamp">
		/// <see cref="IPAFFilenameStamperAndParser"/>
		/// </param>
		/// <param name="fileVersion">
		/// Any non-negative number causes a _(n) to be added to
		/// the end of the filename before the dot and extension. n can be any integer. Version
		/// was placed in the method since there was a use case to change it dynamically during
		/// a run of the file writer. This versioning format and procedure are specific to this
		/// base class. Subclass if something different is needed. This value overrides
		/// <see cref="FileVersion"/> if non-negative. This means that versioning can be
		/// turned  on but not off.
		/// </param>
		public virtual string StampFilename(string fullFilePathWithoutExtension,
			DateTime datetimeToStamp = default(DateTime), int fileVersion = -1)
		{
			DateTime dateTime;
			lock (m_LockObject)
			{
				dateTime = DateTime.Now;
				Task.Delay(2).Wait();
			}

			if (datetimeToStamp != default(DateTime))
				dateTime = datetimeToStamp;

			/////////////////////////////////////////////////////////////////////
			//  Paste on the date/time if we need to.
			var areWeDateTimeStamping = datetimeToStamp != DateTime.MaxValue;
			if (areWeDateTimeStamping)
			{
				var dateTimeString = string.Format(DateTimeFormatString, dateTime);
				fullFilePathWithoutExtension += dateTimeString;
				if (AddMilliseconds)
				{
					var milliseconds = dateTime.Millisecond;
					var millisecondString = milliseconds.ToString();
					millisecondString = StringParsingUtils.LeftPadToLength(millisecondString, 3);
					fullFilePathWithoutExtension += "-" + millisecondString;
				}
			}

			/////////////////////////////////////////////////////////////////////
			var versionNumber = FileVersion;
			//  Paste on the version if we need to override the prop.
			if (fileVersion >= 0)
				versionNumber = fileVersion;

			// Add it to the file if we got something.
			if(versionNumber.HasValue)
				fullFilePathWithoutExtension += "_(" + versionNumber.Value + ")";

			return fullFilePathWithoutExtension;
		}
	}
}
