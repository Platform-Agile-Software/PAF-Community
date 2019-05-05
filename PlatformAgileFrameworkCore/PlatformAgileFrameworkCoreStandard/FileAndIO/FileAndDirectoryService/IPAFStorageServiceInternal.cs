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

#region Using Directives

using System;

#region Exception Shorthand

using IPAFFIOED = PlatformAgileFramework.ErrorAndException.CoreCustomExceptions.IPAFFileAndDirectoryIOExceptionData;
using PAFFIOED = PlatformAgileFramework.ErrorAndException.CoreCustomExceptions.PAFFileAndDirectoryIOExceptionData;
#endregion // Exception Shorthand

#endregion // Using Directives
namespace PlatformAgileFramework.FileAndIO.FileAndDirectoryService
{
	/// <summary>
	///	Internal part of split interface. See <see cref="IPAFStorageService"/>.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 17jan2012 </date>
	/// <description>
	/// New.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 22jun2014 </date>
	/// <description>
	/// Exposed symbolic directory translation stuff, since XML classes need to be supplied
	/// with filenames.
	/// </description>
	/// </contribution>
	/// </history>
	public interface IPAFStorageServiceInternal : IPAFStorageService
	{
		#region Methods
		/// <summary>
		/// Translates a symbolic directory symbol to a real directory spec on the native machine
		/// or environment.
		/// </summary>
		/// <param name="symbolicDirectorySymbol">
		/// The stringful symbol.
		/// </param>
		/// <returns><see langword="null"/> if symbol not in dictionary.</returns>
		string GetMappedDirectorySymbolInternal(string symbolicDirectorySymbol);

		/// <summary>
		/// Translates a file specification symbol to a real file path on the native machine
		/// or environment.
		/// </summary>
		/// <param name="fileSpec">
		/// The stringful symbol.
		/// </param>
		/// <returns><see langword="null"/> if symbol not in dictionary.</returns>
		string GetConvertedFileSpecInternal(string fileSpec);

		/// <summary>
		/// Sets application root in cases where it needs to be forced.
		/// </summary>
		/// <param name="rootDirSpec">
		/// The stringful root spec with no trailing dirsep.
		/// </param>
		void SetApplicationRootInternal(string rootDirSpec);

		#endregion // Methods

	}
} 