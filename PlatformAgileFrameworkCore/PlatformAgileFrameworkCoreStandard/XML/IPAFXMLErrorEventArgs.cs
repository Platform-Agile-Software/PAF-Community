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

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.XML
{
	/// <summary>
	///  Returns detailed information related to the XmlErrorEvent.
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
	public partial interface IPAFXMLErrorEventArgs
	{
		/// <summary>
		/// Gets the Exception associated with the error event.
		/// </summary>
		Exception Exception { get; }
		/// <summary>
		/// Gets the text description corresponding to the error event.
		/// </summary>
		string Message { get; }
		/// <summary>
		/// <see langword="true"/> for a warning as opposed to an error.
		/// </summary>
		bool Warning { get; }
	}
}
