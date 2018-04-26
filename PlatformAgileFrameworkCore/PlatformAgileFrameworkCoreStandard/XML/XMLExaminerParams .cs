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

namespace PlatformAgileFramework.XML
{
	/// <summary>
	/// Default implementation of the interface.
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
	public partial class XMLExaminerParams : IXMLExaminerParams
	{
		#region IXMLExaminerParams Members
		/// <summary>
		/// <see cref="IXMLExaminerParams"/>
		/// </summary>
		public string KeyValueElementName { get; set; }
		/// <summary>
		/// <see cref="IXMLExaminerParams"/>
		/// </summary>
		public int MaxErrors { get; set; }
		/// <summary>
		/// <see cref="IXMLExaminerParams"/>
		/// </summary>
		public int MaxWarnings { get; set; }
		/// <summary>
		/// <see cref="IXMLExaminerParams"/>
		/// </summary>
		public PAFXMLReaderErrorEventHandler ValidationEventHandler { get; set; }
		/// <summary>
		/// <see cref="IXMLExaminerParams"/>
		/// </summary>
		public PAFXMLValidationEventHandlerDelegate ValidationEventHandlerDelegate { get; set; }
		/// <summary>
		/// <see cref="IXMLExaminerParams"/>
		/// </summary>
		public PAFXMLValidationHandler Validator { get; set; }
		/// <summary>
		/// <see cref="IXMLExaminerParams"/>
		/// </summary>
		public string XMLInputFilePath { get; set; }
		/// <summary>
		/// <see cref="IXMLExaminerParams"/>
		/// </summary>
		public IPAFStorageStream XMLInputStream { get; set; }
		/// <summary>
		/// <see cref="IXMLExaminerParams"/>
		/// </summary>
		public string XMLSchemaDirectory { get; set; }
		/// <summary>
		/// <see cref="IXMLExaminerParams"/>
		/// </summary>
		public IList<IXMLSchemaSpec> XMLSchemas { get; set; }
		/// <summary>
		/// <see cref="IXMLExaminerParams"/>
		/// </summary>
		public PAFXMLSchemaValidationFlags XMLValidationFlags { get; set; }
		#endregion //IXMLExaminerParams Members

		#region IDisposable Members
		/// <summary>
		/// Simple dispose for this one - only one item.
		/// </summary>
		public virtual void Dispose()
		{
			if (XMLInputStream == null) return;
			try
			{
				XMLInputStream.Dispose();
			}
			catch
			{

				XMLInputStream = null;
			}
		}
		#endregion
	}

}
