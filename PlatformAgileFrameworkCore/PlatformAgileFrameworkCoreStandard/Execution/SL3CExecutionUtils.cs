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

using System;
using System.Security;
using PlatformAgileFramework.TypeHandling.PartialClassSupport;

namespace PlatformAgileFramework.Execution
{
	/// <summary>
	/// Utilities helping to identify the execution environment.
	/// </summary>
	// Silverlight
// ReSharper disable PartialTypeWithSinglePart
	public partial class ExecutionUtils
// ReSharper restore PartialTypeWithSinglePart
	{
		/// <summary>
		/// Indicates trust mode.
		/// </summary>
		private static bool s_IsElevatedTrust = false;
		/// <summary>
		/// Determines whether we are in low-trust (e.g. Silverlight) or
		/// elevated trust (e.g. trusted ECMA assembly).
		/// </summary>
		/// <returns>
		/// <see langword="true"/> for elevated trust.
		/// </returns>
		public static bool IsElevatedTrust()
		{
			return s_IsElevatedTrust;
		}
		/// <summary>
		/// Allows setting of trust level.
		/// </summary>
		/// <param name="isElevatedTrust">
		/// Sets trust level.
		/// </param>
		[SecurityCritical]
		public static void SetElevatedTrust(bool isElevatedTrust)
		{
			s_IsElevatedTrust = isElevatedTrust;
		}
	}
}
