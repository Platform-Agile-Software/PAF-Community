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
using System.Xml.Linq;
using PlatformAgileFramework.Execution.Pipeline;

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.XML
{
	#region Delegates
	/// <summary>
	/// We expose a delegate to hook events.
	/// </summary>
	/// <param name="args">
	/// <see cref="IPAFXMLErrorEventArgs"/>.
	/// </param>
	public delegate void PAFXMLValidationEventHandlerDelegate(IPAFXMLErrorEventArgs args);
	#endregion // Delegates
	#region Enums
	/// <summary>
	/// Enum for special codes inside the XML.
	/// </summary>
	// TODO -KRM we absolutely cannot obsolete stuuf which we are still using ourselves!
	// [Obsolete("Legacy CLR PAF")]
	public enum ValueLiteralCode
	{
		/// <summary>
		/// Normal value.
		/// </summary>
		Normal = 0,
		/// <summary>
		/// "DEFAULTED"
		/// </summary>
		Defaulted = 1,
		/// <summary>
		/// "INHERITED"
		/// </summary>
		Inherited = 2
	}

	/// <summary>
	/// Specifies schema validation options used by a
	/// schema validator.
	/// </summary>
	/// This was a MS type that was made internal in Silverlight. We need it for
	/// legacy apps that use XSDs.
	[Flags]
	public enum PAFXMLSchemaValidationFlags
	{
		/// <remark/>
		None = 0,
		/// <remark/>
		ProcessInlineSchema = 1,
		/// <remark/>
		ProcessSchemaLocation = 2,
		/// <remark/>
		ReportValidationWarnings = 4,
		/// <remark/>
		ProcessIdentityConstraints = 8,
		/// <remark/>
		AllowXmlAttributes = 16,
	}
	#endregion // Enums
	/// <summary>
	/// This interface is a model for a type that reads an XML file or
	/// stream with optional validation and with error handling. It also
	/// has a "look-through" capability to read a configuration file
	/// or to selectively read configuration statements from a general
	/// XML file for setup for a second reading pass. The configuration
	/// file that this class reads can have the same specification as
	/// any app.config file, but the Key/Value dictionary
	/// that can be embedded in the file is limited to
	/// <ELEMENT_NAME key="StringKey" value="StringValue"/> constructs.
	/// The behavior of the class is largely dependent on the setting of
	/// various construction parameters. The interface ensures implementation
	/// of <see cref="IDisposable"/> to dispose of any internal streams.
	/// </summary>
	/// <remarks>
	/// The interface follows proper inteface-based design principles by
	/// being mostly technology neutral. The original PAF internal
	/// implementations of this interface were based on 2004-era Microsoft
	/// XML classes with augmentation by helper methods for missing features
	/// or things that either Microsoft or Mono had wrong (mostly Microsoft).
	/// The only thing that's exposed here in core is basic <c>System.Xml.Linq</c>
	/// technology, since that was all that was in Silverlight and now in
	/// most modern PCLs.
	/// </remarks>
	/// <threadsafety>
	/// Implementations are not expected to be thread-safe.
	/// </threadsafety>
	#region History
	/// <history>
	/// <description>
	/// <author> Brain T. </author>
	/// <date> 25apr2016 </date>
	/// <contribution>
	/// Put back basic linq stuff that can run in core that was removed.
	/// </contribution>
	/// </description>
	/// <description>
	/// <author> DAP </author>
	/// <date> 05nov2012 </date>
	/// <contribution>
	/// Created. Support for allowing PAF legacy CLR stuff to run in core.
	/// </contribution>
	/// </description>
	/// </history>
	#endregion History
		// XPATH stuff in extended.
	// ReSharper disable once PartialTypeWithSinglePart 
	public partial interface IXMLExaminer : IPAFBaseExePipelineInitialize<IXMLExaminerParams>,
		IDisposable
	{
		#region Properties
		/// <summary>
		/// Get the list of exceptions generated. Useful when an unknown exception
		/// was triggered. This list is not populated when an error event handler
		/// is installed.
		/// </summary>
		IList<Exception> ExceptionList {get;}
		/// <summary>
		/// Get the internal key/value dictionary, which is recreated every time
		/// readkeyvalues is called with a non-<see langword="null"/> "elementName" value.
		/// </summary>
		/// <remarks>
		/// Needed for legacy support. If you don't know what this is, you don't want to know.
		/// </remarks>
		[Obsolete("Legacy CLR PAF")]
		IDictionary<string, string> KeyValueDictionary { get; }
		/// <summary>
		/// Get the number of errors that have occurred, either from an
		/// installed validation event handler or from caught exceptions.
		/// </summary>
		int TotalErrors{get;}
		/// <summary>
		/// Get the number of warnings that have occurred from an installed
		/// validation event handler.
		/// </summary>
		int TotalWarnings{get;}
		#endregion // Properties
		#region Methods
		/// <summary>
		/// Gets a value corresponding to a key. This reads the key/value section of the XML
		/// file, if any.
		/// </summary>
		/// <param name="keyString">
		/// The name of the key associated with the element.
		/// </param>
		/// <param name="code">
		/// Tells if the element had a special literal.
		/// </param>
		/// <returns>
		/// A value if it is in the dictionary. <see langword="null"/> if not.
		/// </returns>
		/// <remarks>
		/// Needed for legacy support. If you don't know what this is, you don't want to know.
		/// </remarks>
		[Obsolete("Legacy CLR PAF")]
		IXMLStringValue GetStringKeyValue(string keyString, out ValueLiteralCode code);

		/// <summary>
		/// Reads a document and points at the top <see cref="XElement"/>.
		/// </summary>
		/// <returns>The element, if the read was successful.</returns>
		/// <remarks>
		/// This read should always close and dispose readers and streams when done.
		/// </remarks>
		XElement ReadXMLDocumentLinq();

		#endregion // Methods
	}

}
