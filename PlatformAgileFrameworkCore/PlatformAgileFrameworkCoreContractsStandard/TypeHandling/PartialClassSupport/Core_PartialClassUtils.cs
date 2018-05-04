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

namespace PlatformAgileFramework.TypeHandling.PartialClassSupport
{
	/// <summary>
	/// Helper classes/methods for overall application support.
	/// This part is Silverlight compatible. Just delegates for now.
	/// The delegates return outputs through "ref" variables, as is
	/// required for partial method usage.
	/// </summary>
	/// <history>
	/// <author> KRM </author>
	/// <date> 02sep2012 </date>
	/// <contribution>
	/// New.
	/// </contribution>
	/// </history>
// ReSharper disable PartialTypeWithSinglePart
	public partial class PartialClassUtils
// ReSharper restore PartialTypeWithSinglePart
	{
		#region Delegates
		/// <summary>
		/// Delegate is conveniently used as a partial method signature for
		/// an OPTIONAL method that can definatively decide whether something is
		/// T/F or make no decision at all. It is useful when used within
		/// a method that calls the partial method to determine whether
		/// the partial method was linked and had a decision about whether
		/// the attribute was T/F in the current environment.
		/// </summary>
		/// <param name="trueFalseOrIndifferent">
		/// Users of this delegate can check whether this value has been unpdated
		/// from a <see langword="null"/> initial value. This is one way to employ this
		/// delegate.
		/// </param>
		public delegate void TrueFalseOrIndifferent(ref bool? trueFalseOrIndifferent);
		#endregion // Delegates
	}
}
