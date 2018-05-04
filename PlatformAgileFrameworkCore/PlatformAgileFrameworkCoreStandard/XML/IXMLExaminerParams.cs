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
using System.Collections.Generic;
using PlatformAgileFramework.FileAndIO.FileAndDirectoryService;

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.XML
{
	/// <summary>
	/// This interface contains the settable parameters for the
	/// XML examiner.
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
	public partial interface IXMLExaminerParams : IDisposable
	{
		/// <summary>
		/// Specifies the name of elements that are to be scanned for
		/// "key/value" pairs in the initialization phase to populate
		/// the <see cref="IXMLExaminer.KeyValueDictionary"/>.
		/// </summary>
		/// <remarks>
		/// Needed for legacy support. If you don't know what this is,
		/// you don't want to know.
		/// </remarks>
		[Obsolete("Legacy CLR PAF")]
		string KeyValueElementName { get; set; }
		/// <summary>
		/// Set the maximum number of errors that will be tolerated before
		/// quitting, either from an installed validation event handler or
		/// from caught exceptions. If this number is exceeded, the read is
		/// stopped. Default is 100.
		/// </summary>
		int MaxErrors { get; set; }
		/// <summary>
		/// Set the maximum number of warnings that will be tolerated before
		/// quitting, from an installed validation event handler. If this
		/// number is exceeded, the read is stopped. Default is 100.
		/// </summary>
		int MaxWarnings { get; set; }
		/// <summary>
		/// Installs a validation event handler.  If <see langword="true"/>, the
		/// internal textual-based event handler is used if <see cref="Validator"/>
		/// is <see langword="true"/>.
		/// </summary>
		/// <remarks>
		/// In the old CLR version of this class, this used to be a standard
		/// XSD validation event. This member has been generalized to support an
		/// abstract notion of validation, which can be delegated to the XSD-style
		/// validation if desired as is done in the CLR extension.
		/// </remarks>
		PAFXMLReaderErrorEventHandler ValidationEventHandler { get; set; }
		/// <summary>
		/// This allows plugging of an arbitrary validation mechanism. This, coupled
		/// with the <see cref=" ValidationEventHandler"/> allows augmentation
		/// of the examiner with external components. In contrast to the old CLR version,
		/// there is no default validation mechanism. If this property is not set, no
		/// validation is performed.
		/// </summary>
		PAFXMLValidationHandler Validator { get; set; }
		/// <summary>
		/// Filename to open. May be a symbolic filename. Ignored if a stream is passed.
		/// </summary>
		string XMLInputFilePath { get; set; }
		/// <summary>
		/// This stream can be passed in or read internally from a supplied
		/// <see cref="XMLInputFilePath"/>. It is exposed so that the caller
		/// may rewind the stream or perform arbitrary operations on it from
		/// the outside.
		/// </summary>
		IPAFStorageStream XMLInputStream { get; set; }
		/// <summary>
		/// Default schema directory is used if this is not set. Used to search for schemas
		/// specified by filenames only. If set, directory is prepended to each file
		/// name. The directory spec must have a trailing directory separator character.
		/// If not specified, raw filenames are used and thus must be full paths.
		/// </summary>
		string XMLSchemaDirectory { get; set; }
		/// <summary>
		/// Get/set schema file list. Used only when validating. This list must be set
		/// for validating to be performed. Otherwise validation delegate is ignored.
		/// </summary>
		IList<IXMLSchemaSpec> XMLSchemas{get; set; }
		/// <summary>
		/// Get/set the validation flags. These flags are not used if validation is
		/// not in effect. Default is errors reported only.
		/// </summary>
		PAFXMLSchemaValidationFlags XMLValidationFlags{get; set; }
	}
}
