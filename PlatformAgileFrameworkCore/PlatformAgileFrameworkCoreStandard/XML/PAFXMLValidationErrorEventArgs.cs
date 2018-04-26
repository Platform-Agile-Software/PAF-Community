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

namespace PlatformAgileFramework.XML
{
	/// <summary>
	/// Returns detailed information related to the XmlErrorEvent.
	/// </summary>
	/// <history>
	/// <author> DAP </author>
	/// <date> 05nov2012 </date>
	/// <contribution>
	/// <para>
	/// Created. Support for allowing PAF legacy CLR stuff to run in core.
	/// </para>
	/// </contribution>
	/// </history>
	// ReSharper disable once PartialTypeWithSinglePart
	public partial class PAFXMLValidationErrorEventArgs : EventArgs, IPAFXMLErrorEventArgs
	{
		/// <summary>
		/// Support for <see cref="IPAFXMLErrorEventArgs"/>
		/// </summary>
		protected readonly Exception m_Exception;
		/// <summary>
		/// Support for <see cref="IPAFXMLErrorEventArgs"/>
		/// </summary>
		protected readonly string m_Message;
		/// <summary>
		/// Support for <see cref="IPAFXMLErrorEventArgs"/>
		/// </summary>
		protected readonly bool m_Warning;

		/// <summary>
		/// Constructor just loads the fields.
		/// </summary>
		/// <param name="exception">
		/// The exception property. non-<see langword="null"/>.
		/// </param>
		/// <param name="message">
		/// The message property.
		/// </param>
		/// <param name="warning">
		/// The warning property.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">
		/// Thrown if <paramref name="exception"/> is <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		public PAFXMLValidationErrorEventArgs(Exception exception, string message = "", bool warning = false)
		{
			if (exception == null) throw new ArgumentNullException("exception");
			m_Exception = exception;
			m_Message = message;
			m_Warning = warning;
		}
		/// <summary>
		/// <see cref="IPAFXMLErrorEventArgs"/>
		/// </summary>
		public Exception Exception { get { return m_Exception; } }
		/// <summary>
		/// <see cref="IPAFXMLErrorEventArgs"/>
		/// </summary>
		public string Message { get { return m_Message; } }
		/// <summary>
		/// <see cref="IPAFXMLErrorEventArgs"/>
		/// </summary>
		public bool Warning { get { return m_Warning; } }
	}

}
