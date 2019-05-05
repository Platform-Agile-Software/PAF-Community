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

namespace PlatformAgileFramework.TypeHandling
{
	/// <summary>
	/// Helper classes/methods for TypeHandling. This is the superclass that
	/// lives in the contract assembly. It exposes only those items that need
	/// to be revealed to the outside world.
	/// </summary>
	// ReSharper disable PartialTypeWithSinglePart
	public partial class CoreContract_TypeHandlingUtils
	// ReSharper restore PartialTypeWithSinglePart
	{
		////KRM todo - this is useful, but redocument it.
		#region Methods
		/// <summary>
		/// This method ensures that any reset of the value is done by
		/// elevated priviledge callers.
		/// </summary>
		/// <param name="int32Value">The value to set on the output.</param>
		[SecurityCritical]
		public static int? CriticalSet(int? int32Value)
		{
			return int32Value;
		}
		/// <summary>
		/// This method ensures that any reset of the value is done by
		/// elevated priviledge callers.
		/// </summary>
		/// <param name="boolValue">The value to set on the output.</param>
		[SecurityCritical]
		public static bool? CriticalSet(bool? boolValue)
		{
			return boolValue;
		}
		#endregion // Methods
	}
}