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
#region Using directives

using System;

#endregion

namespace PlatformAgileFramework.ErrorAndException
{
	/// <summary>
	/// <para>
	/// Just a standard PAF exception that signals a "special" circumstance. It
	/// is used to exit try blocks that may contain complicated logic inside to get
	/// down to a catch block so the catch block can filter the exception and
	/// identify it as a certain exception.
	/// </para>
	/// <para>
	/// This exception should not be serialized, should never go anywhere, should
	/// never be wrapped in another, but is just used as a "signalling" exception
	/// to navigate around try/catch blocks. It's probably not a "best practice", but
	/// it has been necessary in some cases where it's not possible to determine
	/// what exceptions third-party methods will throw.
	/// </para>
	/// </summary>
	public class MarkerException : PAFAbstractExceptionBase
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// GUID identifying the exception.
		/// </summary>
		private readonly Guid m_exceptionGUID;
		#endregion// Class Fields and Autoproperties
		#region Properties
		/// <summary>
		/// This defines the read-only GUID given to the exception at construction
		/// time.
		/// </summary>
		public Guid ExceptionGUID
		{ get { return m_exceptionGUID; } }
		#endregion // Properties
		#region Constructors
		#endregion // Constructors
		/// <summary>
		/// Default constructor that randomly generates the internal GUID.
		/// </summary>
		public MarkerException()
		{
			m_exceptionGUID = Guid.NewGuid();
		}
		/// <summary>
		/// Constructor that sets the internal GUID.
		/// </summary>
		/// <param name="exceptionGUID">
		/// The GUID that wiil be returned by the <see cref="ExceptionGUID"/> property.
		/// </param>
		public MarkerException(Guid exceptionGUID)
		{
			m_exceptionGUID = exceptionGUID;
		}

        /// <summary>
        /// Gets the exception data. Vacuous in this case.
        /// </summary>
        /// <returns><see langword="null"/></returns>
        public override object GetExceptionData()
        {
            return null;
        }
    }
}
