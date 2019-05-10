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
#endregion // Using Directives

namespace PlatformAgileFramework.FileAndIO
{
	/// <summary>
	///	Protocol for stamping a file with a date.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 28feb2019 </date>
	/// <description>
	/// New. Initially built to make date stamping and versioning more general with a plugin.
	/// </description>
	/// </contribution>
	/// </history>
	public interface IPAFFilenameStamperAndParser
	{
		#region Properties
		/// <summary>
		/// The string that is used to format the date/time.
		/// </summary>
		string DateTimeFormatString { get; }
		/// <summary>
		/// The base filename. This is the "current" filename that is set by the writer for the
		/// current run of the file writer. This should not include the directory or extension.
		/// This is the name that date/time and version information is incorporated into.
		/// </summary>
		string FileBaseName { get; }
		/// <summary>
		/// The version number to be embedded in the file somehow, if non-null.
		/// </summary>
		int? FileVersion { get; set; }

		/// <summary>
		/// If <see langword="true"/> a zero-padded three digit representation
		/// of milliseconds will be added to the time stamp. <c>(-nnn)</c>.
		/// </summary>
		bool AddMilliseconds { get; }
		#endregion // Properties
		#region Methods
		/// <summary>
		/// Extracts a date/time and version somehow from a filename.
		/// </summary>
		/// <param name="fullFilePathWithExtension">
		/// Full filename to parse.
		/// </param>
		/// <returns>
		/// Extracted date/time and/or version.
		/// </returns>
		DateTimeAndVersion ParseFilename(string fullFilePathWithExtension);
		/// <summary>
		/// Incorporates a date/time nd version somehow into a filename.
		/// </summary>
		/// <param name="fullFilePathWithoutExtension">
		/// Everything but the extension and no dot, please.
		/// </param>
		/// <param name="datetimeToStamp">
		/// <see langword="default(DateTime)"/> normally defaults to <see cref="DateTime.Now"/>.
		/// <see cref="DateTime.MaxValue"/> turns off date stamping.
		/// </param>
		/// <param name="fileVersion">
		/// Default of -1 causes no versioning. Any positive number causes versioning of some variety. Version
		/// was placed in the method since there was a use case to change it dynamically during
		/// a run of the file writer.
		/// </param>
		/// <returns>
		/// Augmented file path.
		/// </returns>
		string StampFilename(string fullFilePathWithoutExtension, DateTime datetimeToStamp = default(DateTime),
			int fileVersion = -1);
		#endregion // Methods
	}

	/// <summary>
	/// Just a little struct to send out combination of date/time and version.
	/// </summary>
	public struct DateTimeAndVersion
	{
		/// <summary>
		/// Returns null if no date found in filename.
		/// </summary>
		public DateTime? FileDateTime { get; set; }
		/// <summary>
		/// Returns -1 for no version found in file name.
		/// </summary>
		public int FileVersion { get; set; }
	}
}
