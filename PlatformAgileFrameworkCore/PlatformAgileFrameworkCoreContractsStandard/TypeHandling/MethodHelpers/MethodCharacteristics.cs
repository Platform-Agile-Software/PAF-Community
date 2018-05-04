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
using System.Reflection;
using PlatformAgileFramework.TypeHandling.ParameterHelpers;
#endregion // Using Directives

namespace PlatformAgileFramework.TypeHandling.MethodHelpers
{
	/// <summary>
	/// This class extends <see cref="MethodParameters"/> by adding the host type.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 04jun2015 </date>
	/// <description>
	/// New.
	/// Clean up of the delegate verification stuff in our reflection classes.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Unsafe. Not intended for multi-threaded environmnts.
	/// </threadsafety>
	public class MethodCharacteristics : MethodParameters
	{
		#region Type fields and autoprops
		/// <summary>
		/// Host type for instance methods - <see langword="null"/> statics.
		/// </summary>
		public Type HostType { get; protected set; }
		#endregion // Type fields and autoprops

		#region Constructors
		/// <summary>
		/// Just constructs our representation from a set of <see cref="ParameterInfo"/>'s
		/// and a host type.
		/// </summary>
		/// <param name="parameters">
		/// <see langword="null"/> for a method with no parames and no return value.
		/// </param>
		/// <param name="hostType">
		/// Type of the instance the method lives on. <see langword="null"/> for static method.
		/// </param>
		public MethodCharacteristics(IEnumerable<ParameterInfo> parameters = null, Type hostType = null)
			: base(parameters)
		{
			HostType = hostType;
		}

		/// <summary>
		/// Just constructs our representation from a set of <see cref="ParameterCharacteristics"/>'s
		/// and a host type.
		/// </summary>
		/// <param name="parameters">
		/// <see langword="null"/> for a method with no params.
		/// </param>
		/// <param name="returnValue">
		/// Characteristics for the return value - <see langword="null"/> for a void method.
		/// </param>
		/// <param name="hostType">
		/// Type of the instance the method lives on. <see langword="null"/> for static method.
		/// </param>
		public MethodCharacteristics(IEnumerable<ParameterCharacteristics> parameters = null,
			ParameterCharacteristics returnValue = null, Type hostType = null)
			: base(parameters, returnValue)
		{
			HostType = hostType;
		}
		#endregion // Constructors	
	}
}
