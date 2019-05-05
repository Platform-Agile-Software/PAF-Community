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
#endregion // Using Directives

namespace PlatformAgileFramework.ErrorAndException
{
	/// <summary>
	///	Carries a list of exceptions. For security purposes, this data should
	/// be contained in a "sealable" list. If the list is sealed, no more
	/// exceptions may be added.
	/// </summary>
	/// <remarks>
	/// Implementations should normally be at least "[PAFSerializable]".
	/// </remarks>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 11Oct2014 </date>
	/// <description>
	/// Documented.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Generally should be locked after trusted code finishes adding
	/// exceptions.
	/// </threadsafety>
	public interface IPAFAggregateExceptionData : IPAFStandardExceptionData
	{
		#region Properties
		/// <summary>
		/// Gets the exceptions.
		/// </summary>
		IEnumerable<Exception> Exceptions{get; }
		#endregion // Properties
		#region  Methods
		/// <summary>
		/// This method adds an exception to the list of exceptions if the list is not
		/// sealed. If an exception passed to this method is <see langword="null"/>,
		/// this seals the list. The <see langword="null"/> exception is not added
		/// to the list.
		/// </summary>
		/// <param name="exception">Exception to be added.</param>
		/// <exceptions>
		/// <exception> <see cref="InvalidOperationException"/> is thrown if
		/// the list has been sealed by adding a <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		void AddException(Exception exception);
		#endregion // Methods
	}
}