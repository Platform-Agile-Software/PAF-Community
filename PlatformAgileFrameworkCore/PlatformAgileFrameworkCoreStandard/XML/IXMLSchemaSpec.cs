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
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.XML
{
	/// <summary>
	/// Delegate to process schema or other xml reading errors.
	/// </summary>
	/// <param name="sender">The usual object.</param>
	/// <param name="e">Our error event arguments.</param>
	/// <remarks>
	/// This method must not throw or pass exceptions.
	/// </remarks>
	public delegate void PAFXMLReaderErrorEventHandler(object sender,
		IPAFXMLErrorEventArgs e);
	/// <summary>
	/// Delegate to validate an XML file.
	/// </summary>
	/// <param name="xmlExaminer">
	/// Set of params with an active stream with any position. Stream will be rewound.
	/// Stream can be left in any position. Not <see langword="null"/>.
	/// </param>
	/// <param name="schema">
	/// Set of schemas to validate against.If <see langword = "null"/> or empty,
	/// typical implementations will validate for correctly-formed XML.
	/// </param>
	/// <param name="errorHandler">
	/// May be <see langword="null"/>.
	/// </param>
	/// <remarks>
	/// This method may throw or pass exceptions. A typical scenario is for this
	/// method is to throw exceptions on fatal errors and to gather and return warnings.
	/// </remarks>
	public delegate void PAFXMLValidationHandler(IXMLExaminer xmlExaminer,
		IEnumerable<IXMLSchemaSpec> schema, PAFXMLReaderErrorEventHandler errorHandler);
	/// <summary>
	/// A little container to hold filenames and ns's for schemas.
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
	public interface IXMLSchemaSpec
	{
		#region Properties
		/// <summary>
		/// Full path to xsd file. e.g. c:\myXsd.xsd
		/// </summary>
		string FileName { get;}
		/// <summary>
		/// Target namespace. e.g. urn:content-type.
		/// </summary>
		string NameSpace { get;}
		#endregion //Properties
	}
}
