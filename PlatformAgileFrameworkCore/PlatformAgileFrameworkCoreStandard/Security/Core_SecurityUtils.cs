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

namespace PlatformAgileFramework.Security
{
	#region Class SecurityUtils
	/// <summary>
	/// A utility class for file, network and general security.
	/// </summary>
// ReSharper disable once PartialTypeWithSinglePart
	// Core part
	public partial class SecurityUtils
	{
	}
	/// <summary>
	/// A utility class for file, network and general security.
	/// </summary>
	internal class SecurityUtilsInternal
	{
		/// <summary>
		/// This is an internal enum that is used to communicate information about
		/// the status of attempted file operations or proposed file operations
		/// that can involve security issues. We want to hide the nature of the
		/// information from unpriviledged clients.
		/// </summary>
		[Flags]
		internal enum FileSecurityProbeStatus
		{
			// Return this to clients needing an Enum = -1 for a generic failure flag.
			GeneralSecurityFailure = -1,
			// Requester has permissions to receive requested data about a file.
			Success = 0,
			// This indicates that the requester (usually not the immediate caller)
			// does not have the required permissions based on a stack walk or
			// other test. For RemoteServicer, this is usually something like
			// the impersonated user.
			UserSecurityAccessFailure = 1,
			// This indicates that the client (usually not the immediate caller)
			// does not have the required permissions.
			ClientSecurityAccessFailure = 2,
			// This indicates that the caller (usually the immediate caller)
			// does not have the required permissions.
			CallerSecurityAccessFailure = 4,
			// This indicates that the framework's AppDomain that called an access
			// method does not have the required permissions.
			FrameworkSecurityAccessFailure = 8
		}
	}
	#endregion
}
