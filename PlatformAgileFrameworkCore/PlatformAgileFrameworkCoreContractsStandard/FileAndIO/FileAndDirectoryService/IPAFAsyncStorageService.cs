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
using System.Collections.Generic;
using System.Security;
using System.Threading.Tasks;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.ErrorAndException.CoreCustomExceptions;
//using PlatformAgileFramework.ErrorAndException.CoreCustomExceptions;
using PlatformAgileFramework.FrameworkServices;

#region Exception Shorthand

//using IPAFFIOED = PlatformAgileFramework.ErrorAndException.CoreCustomExceptions.IPAFFileAndDirectoryIOExceptionData;
//using PAFFIOED = PlatformAgileFramework.ErrorAndException.CoreCustomExceptions.PAFFileAndDirectoryIOExceptionData;
#endregion // Exception Shorthand

#endregion // Using Directives
namespace PlatformAgileFramework.FileAndIO.FileAndDirectoryService
{
	/// <summary>
	///	Interface for our async file service. Just a few methods from our sync interface exposed as
	/// async.
	/// See <see cref="IPAFStorageService"/>
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> Bello </author>
	/// <date> 12mar2016 </date>
	/// <description>
	/// Redid this when TPL got fully functional. Put in documentation.
	/// Added timeout protection for Golea.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Implementations should always be made thread-safe.
	/// </threadsafety>
	/// ReSharper disable once PartialTypeWithSinglePart
	public partial interface IPAFAsyncStorageService: IPAFStorageService
	{
		//Task DeleteFileAsync(string path, int timeoutInMilliseconds = int.MaxValue);

		//Task<bool> FileExistsAsync(string path, int timeoutInMilliseconds = int.MaxValue);

		Task<IPAFStorageStream> PAFOpenFileAsync(string path);
		Task<IPAFStorageStream> PAFOpenFileAsync(string path, int timeoutInMilliseconds);

		//Task SaveFileAsync(string path, IPAFStorageStream fileContents, int timeoutInMilliseconds = int.MaxValue);

	}
} 