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
using System.Reflection;
#endregion // Using Directives

namespace PlatformAgileFramework.TypeHandling.ParameterHelpers
{
	/// <summary>
	/// This struct contains the parameter attributes we care about when doing
	/// method co and contra-variance checks. <see cref="ParameterInfo"/> is
	/// not constructable, so we use a surrogate type to hold the attributes.
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
	public class ParameterCharacteristics
	{
		#region Type fields and autoprops
		/// <summary>
		/// <see langword="true"/> for either an "in" parameter or a "ref"
		/// parameter.
		/// </summary>
		public bool IsIn { get; protected set; }
		/// <summary>
		/// <see langword="true"/> for optinal params at the end.
		/// parameter.
		/// </summary>
		public bool IsOptional { get; protected set; }
		/// <summary>
		/// <see langword="true"/> for either an "out" parameter or a "ref"
		/// parameter.
		/// </summary>
		public bool IsOut { get; protected set; }
		/// <summary>
		/// The underlying type of the parameter.
		/// </summary>
		public Type ParameterType { get; protected set; }
		#endregion // Type fields and autoprops
		#region Constructors

		/// <summary>
		/// Just sets the props.
		/// </summary>
		/// <param name="parameterType">Sets <see cref="ParameterType"/>.</param>
		/// <param name="isOptional">Sets <see cref="IsOptional"/>.</param>
		/// <param name="isIn">Sets <see cref="IsIn"/>.</param>
		/// <param name="isOut">Sets <see cref="IsOut"/>.</param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">"parameterType".</exception>
		/// </exceptions>
		public ParameterCharacteristics(Type parameterType, bool isOptional = true,
			bool isIn = true, bool isOut = false)
		{
			if(parameterType == null) throw new ArgumentNullException("parameterType");
			ParameterType = parameterType;
			IsIn = isIn;
			IsOptional = isOptional;
			IsOut = isOut;
		}
		/// <summary>
		/// Sets the props from a <see cref="ParameterInfo"/>.
		/// </summary>
		/// <param name="parameterInfo">Incoming <see cref="ParameterInfo"/></param>
		public ParameterCharacteristics(ParameterInfo parameterInfo)
			: this(parameterInfo.GetType(), parameterInfo.IsOptional,
				parameterInfo.IsIn, parameterInfo.IsOut)
		{
		}
		#endregion // Constructors
		#region Properties
		/// <summary>
		/// This returns <see langword="true"/> if both an in parameter and
		/// an out parameter.
		/// </summary>
		public bool IsRef { get { return IsIn && IsOut; } }
		#endregion // Properties
	}
}
