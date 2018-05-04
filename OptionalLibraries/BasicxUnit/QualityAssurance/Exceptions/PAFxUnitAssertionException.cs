//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 Icucom Corporation
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
using PlatformAgileFramework.ErrorAndException;

namespace PlatformAgileFramework.QualityAssurance.Exceptions
{
	/// <summary>
	/// This class represents a marker or signal exception that indicates
	/// an assertion has failed. It is similar to the assertion exceptions
	/// in NUnit and the MS test frameworks.
	/// </summary>
	// ReSharper disable once InconsistentNaming
	public class PAFxUnitAssertionException : PAFAbstractExceptionBase
	{
		#region Constructors
		/// <summary>
		/// Standard constructor.
		/// </summary>
		/// <summary>
		/// Initializes a new instance of the <see cref="PAFxUnitAssertionException"/> class.
		/// </summary>
		/// <param name="message"><see cref="Exception"/></param>
		/// <param name="innerException"><see cref="Exception"/></param>
		/// <remarks>
		/// Constructor also sets our versions of the message and inner exception.
		/// </remarks>
		public PAFxUnitAssertionException(string message = null, Exception innerException = null)
			: base(message, innerException)
		{
		}

        /// <summary>
        /// Gets the exception data.
        /// </summary>
        /// <returns><see langword="null"/></returns>
        public override object GetExceptionData()
        {
            return null;
        }
        #endregion // Constructors
    }

}

