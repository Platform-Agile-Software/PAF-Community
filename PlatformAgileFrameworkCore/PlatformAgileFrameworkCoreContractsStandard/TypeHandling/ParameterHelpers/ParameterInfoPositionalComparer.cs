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

#region Using Directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.Collections.ExtensionMethods;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;

#endregion // Using Directives

namespace PlatformAgileFramework.TypeHandling.ParameterHelpers
{
	/// <summary>
	/// Just a little sorting attachment for use in internal setup.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> JAW(P) </author>
	/// <date> 06jul2015 </date>
	/// <description>
	/// New.
	/// Clean up of the delegate verification stuff in our reflection classes.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Unsafe. Not intended for multi-threaded environmnts.
	/// </threadsafety>

	public class ParameterInfoPositionalComparer : IComparer<ParameterInfo>
	{
		/// <remarks>
		/// See <see cref="IComparer{ParameterInfo}"/>.
		/// </remarks>
		public int Compare(ParameterInfo firstParam, ParameterInfo secondParam)
		{
			return (firstParam.Position - secondParam.Position);
		}
	}
}
