// Copied from Golea and namespaces modified.
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
using PlatformAgileFramework.Logging;
#endregion // Using Directives

namespace PlatformAgileFramework.FileAndIO
{
	/// <summary>
	///	Internal Interface for our file writer. This is made internal so as
	/// to try to indicate that it is not to to be used capriciously and so it's
	/// exposure is only to trusted assemblies. Extenders can expose this as public,
	/// we can't.
	/// </summary>
	/// <threadsafety>
	/// Implementation-dependent.
	/// </threadsafety>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 04feb2019 </date>
	/// <description>
	/// Factored out of the logger for Golea.
	/// </description>
	/// </contribution>
	/// </history>
	// ReSharper disable once InconsistentNaming
	internal interface IPAFFileWriterInternal : IPAFFileWriter
	{
		#region Properties
		/// <summary>
		///	Gets the collection of writers.
		/// </summary>
		ICollection<Action<string>> Writers { get; }
		#endregion // Properties

		#region Methods
		/// <summary>
		/// Our reader that reads the output file.
		/// </summary>
		/// <param name="outputFile">
		/// If null or blank, reads the "current" file.
		/// </param>
		string ReadOutputFile(string outputFile);
		/// <summary>
		/// This is the file to write to. This can contain a directory specification
		/// or be just a directory specification, including symbolic directories.
		/// File and/or directory must be set at once, to allow easy locking of
		/// writes. Vacuous string does nothing.
		/// </summary>
		void SetOutputFile(string outputFilename);
		#endregion // Methods
	}
}