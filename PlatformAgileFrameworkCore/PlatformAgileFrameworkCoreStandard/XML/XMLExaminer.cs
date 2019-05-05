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
using System.IO;
using System.Xml;
using System.Xml.Linq;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Execution.Pipeline;
using PlatformAgileFramework.FileAndIO;
using PlatformAgileFramework.FileAndIO.FileAndDirectoryService;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.Logging;

// Exception shorthand.
using PlatformAgileFramework.TypeHandling;
using PlatformAgileFramework.XML.Exceptions;
using PAFXED = PlatformAgileFramework.XML.Exceptions.PAFXMLExceptionData;
using IPAFXED = PlatformAgileFramework.XML.Exceptions.IPAFXMLExceptionData;


// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.XML
{
	/// <summary>
	/// This class reads an XML file or stream with optional validation and
	/// with error handling. It also has a "look-through" capability to
	/// read a configuration file or to selectively read configuration
	/// statements from a general XML file for setup for a second reading
	/// pass. The configuration file that this class reads can have the same
	/// specification as any app.config file, but the Key/Value dictionary
	/// that can be embedded in the file is limited to
	/// <ELEMENT_NAME key="StringKey" value="StringValue"/> constructs.
	/// The behavior of the class is largely dependent on the setting of
	/// various construction parameters. The class implements
	/// <see cref="IDisposable"/> to dispose of any internal streams.
	/// </summary>
	/// <threadsafety>
	/// Implementations are not expected to be thread-safe.
	/// </threadsafety>
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
	/// Converted from CLR version, cleaned up and removed CLR-only stuff.
	/// </contribution>
	/// </description>
	/// </history>
	// ReSharper disable once PartialTypeWithSinglePart
	public partial class XMLExaminer : IXMLExaminer
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Get the internal key/value dictionary, which is recreated every time
		/// readkeyvalues is called with a non-<see langword="null"/> "elementName" value.
		/// </summary>
		// TODO -KRM we absolutely cannot obsolete stuuf which we are still using ourselves!
		// [Obsolete("Legacy CLR PAF")]
		public IDictionary<string, string> KeyValueDictionary { get; protected internal set; }
		
		/// <summary>
		/// The number of the current reader node.
		/// </summary>
		protected internal int m_NodeNumber;

		/// <summary>
		/// Get the number of errors that have occurred, either from an
		/// installed validation event handler or from caught exceptions.
		/// </summary>
		public int TotalErrors { get; protected internal set; }

		/// <summary>
		/// Get the number of warnings that have occurred from an installed
		/// validation event handler.
		/// </summary>
		public int TotalWarnings { get; protected internal set; }

		/// <summary>
		/// Allows logging of a list of exceptions that occurred during processing
		/// of an XML file. This list will not be populated if an event handler for
		/// XMLreader error is hooked.
		/// </summary>
		public IList<Exception> ExceptionList { get; protected set; }

		/// <summary>
		/// Overall settings we always want.
		/// </summary>
		private static readonly XmlReaderSettings s_GlobalSettings = new XmlReaderSettings();
		#endregion // Class Fields and Autoproperties
		#region Constructors
		#region Static Constructor
		/// <summary>
		/// Sets up XML settings not to close the stream when the reader is closed.
		/// </summary>
		static XMLExaminer()
		{
			s_GlobalSettings.CloseInput = false;
		}
		#endregion // Static Constructor
		/// <summary>
		/// Default constructor. This constructor initializes props with default values.
		/// </summary>
		public XMLExaminer()
		{
// ReSharper disable once CSharpWarnings::CS0618
			KeyValueDictionary = new Dictionary<string, string>();
			ExceptionList = new List<Exception>();
		}
		#endregion // Constructors
		#region Properties

		/// <summary>
		/// Little easier to access parameters.
		/// </summary>
		protected IXMLExaminerParams ExaminerParams
		{
			get
			{
			    return PipelineParams?.ApplicationParameters;
			}
		}
		#endregion // Properties

		#region Protected Methods
		/// <summary>
		/// Just reads in the XML file and gets the key/value pairs. Our XML
		/// configuration elements have a simple format in which a
		/// certain element can contain a simple set of key/value pairs.
		/// The <see cref="IXMLExaminerParams.XMLInputStream"/> must be
		/// set. It is rewound before the read.
		/// </summary>
		/// <remarks>
		/// This method is also called for the purpose of validating XML files
		/// against schemas or just for well-formedness, too. It's lightweight
		/// (forward XmlReader only) and only the key-values in the file are
		/// stored in the internal dictionary, anyway. Specify <see paramref="elementName"/>
		/// to be <see langword="null"/> and no key/values will be processed.
		/// </remarks>
		protected internal virtual Stream ReadKeyValues()
		{
			// Keyvalue stuff.
#pragma warning disable 618
			if (ExaminerParams.XMLInputStream == null)
// ReSharper disable once NotResolvedInText
				throw new ArgumentNullException("XMLInputStream is null");
			ExaminerParams.XMLInputStream.PAFPosition = 0;
			///////////////////////////////////////////////////////////////////
			XmlReader xmlReader = null;
			TotalErrors = 0;
			TotalWarnings = 0;
			ExceptionList.Clear();
			KeyValueDictionary.Clear();
			// Open it up.
			// Build the validation settings.
			var settings = s_GlobalSettings.Clone();
			settings.IgnoreComments = true;
			settings.IgnoreWhitespace = true;
			try
			{
				if (ExaminerParams.Validator != null)
				{
					// We can only validate if files have been set.
					if ((ExaminerParams.XMLSchemas != null) && (ExaminerParams.XMLSchemas.Count > 0))
					{
						var directory = ExaminerParams.XMLSchemaDirectory;
						if (string.IsNullOrEmpty(directory)) directory = "";
						var rootedSchemaSet = new List<IXMLSchemaSpec>();
						foreach (var spec in ExaminerParams.XMLSchemas)
						{
							rootedSchemaSet.Add(new XMLSchemaSpec(directory + spec.FileName,
								spec.NameSpace));
						}
						ExaminerParams.Validator(this, rootedSchemaSet,
							ExaminerParams.ValidationEventHandler);
					}
				}

				// Just test the opening to make sure we are a valid stream.
				ExaminerParams.XMLInputStream.PAFPosition = 0;
				xmlReader = XmlReader.Create(ExaminerParams.XMLInputStream.PAFGetStream(), settings);
			}
			catch (Exception e)
			{
				var exceptionData = new PAFXED(ExaminerParams.XMLInputFilePath);
				var exception = new PAFStandardException<IPAFXED>(exceptionData,
					PAFXMLExceptionMessageTags.XML_PROCESSING_ERROR, e);
				throw exception;
			}
			finally
			{
			    xmlReader?.Dispose();
			    ExaminerParams.XMLInputStream.PAFPosition = 0;
			}
			////////////////////////////////////////////////////////////////////////////////////////////
			///////////////////////////////////////////////////////////////////
			// Setup went OK, now read it.
			m_NodeNumber = -1;
			while (true)
			{
				// Just read every node in the file.
				try
				{
					var readerReturn = xmlReader.Read();
					m_NodeNumber++;
					// Exit if done.
					if (!readerReturn)
					{
						xmlReader.Dispose();
						ExaminerParams.XMLInputStream.PAFPosition = 0;
						return ExaminerParams.XMLInputStream.PAFGetStream();
					}
					// Interested only in elements for key/values.
					if ((xmlReader.NodeType == XmlNodeType.Element)
						&& (ExaminerParams.KeyValueElementName != null))
					{
						// Of the right name.....
						if ((ExaminerParams.KeyValueElementName == "")
							|| (xmlReader.LocalName == ExaminerParams.KeyValueElementName))
						{
							// Only interested in key/value.
							string keyString;
							string valueString;
							if ((xmlReader.AttributeCount == 2)
								&& ((keyString = xmlReader.GetAttribute("key")) != null)
								&& ((valueString = xmlReader.GetAttribute("value")) != null))
							{
								// Load it into the dictionary.
								KeyValueDictionary.Add(keyString, valueString);
							}
						}
					}
				}
#pragma warning restore 618
				// In this section, we allow a number of XML exceptions up to the specified
				// max and quit otherwise.
				catch (Exception e)
				{
					// Allow XML-type Exceptions to continue if not maxed out on errors.
					if (e is XmlException)
					{
						if (TotalErrors < ExaminerParams.MaxErrors)
						{
							// Call the reporter delegate.
							var xe = new PAFXMLValidationErrorEventArgs(e, e.Message);
							XmlReaderErrorEventHandler(xmlReader, xe);
							continue;
						}
					}
					else
					{
						var xmlExceptionData = new PAFXED(ExaminerParams.XMLInputFilePath);
						var xmlException = new PAFStandardException<IPAFXED>(xmlExceptionData,
                            PAFXMLExceptionMessageTags.EXCESSIVE_XML_PROCESSING_ERRORS, e);
						throw xmlException;
					}

					var exceptionData = new PAFXED(ExaminerParams.XMLInputFilePath);
					var exception = new PAFStandardException<IPAFXED>(exceptionData,
                        PAFXMLExceptionMessageTags.XML_PROCESSING_ERROR, e);
					throw exception;
				}
				finally
				{
				    xmlReader?.Dispose();
				    ExaminerParams.XMLInputStream.PAFPosition = 0;
				}
			}
		}
		#endregion // Protected Methods
		/// <summary>
		/// This is default event handler for the XML validation process. It is
		/// very simple, just storing the exceptions in the list and tallying errors and warnings.
		/// </summary>
		/// <param name="sender">
		/// The usual event callback "sender" object.
		/// </param>
		/// <param name="xe">
		/// <see cref="PAFXMLValidationErrorEventArgs"/>.
		/// </param>
		protected virtual void XmlReaderErrorEventHandler(object sender, PAFXMLValidationErrorEventArgs xe)
		{
			if (xe == null)
				return;
			if (xe.Warning) TotalWarnings++;
			else TotalErrors++;

			if (xe.Exception != null)
				// Record our exception.
				ExceptionList.Add(xe.Exception);
		}
		#region Helper Methods
		/// <summary>
		/// Opens an XML file for reading only.
		/// </summary>
		/// <param name="filePath">
		/// Path to get file from. Can be a symbolic path or an absolute path.
		/// </param>
		/// <returns>
		/// A valid <see cref="Stream"/> if successful.
		/// </returns>
		protected virtual IPAFStorageStream OpenXMLFileForRead(string filePath)
		{
			var fileService = PAFServices.Manager.GetTypedService<IPAFStorageService>();
			return fileService.PAFOpenFile(filePath, PAFFileAccessMode.READONLY);
		}
		#endregion // Helper Methods
		#region IDisposable
		/// <summary>
		/// Disposes of any resources.
		/// </summary>
		public virtual void Dispose()
		{
			try
			{
				Uninitialize();
			}
// ReSharper disable EmptyGeneralCatchClause
			catch{}
// ReSharper restore EmptyGeneralCatchClause
		}
		#endregion //IDisposable
		/// <summary>
		/// Disposes of any internal streams.
		/// </summary>
		protected virtual void Uninitialize()
		{
		    if (ExaminerParams?.XMLInputStream == null) return;
			ExaminerParams.XMLInputStream.Dispose();
			ExaminerParams.XMLInputStream = null;
		}

		#region IXMLExaminer Members
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
		[Obsolete("Legacy CLR PAF")]
		public virtual IXMLStringValue GetStringKeyValue(string keyString, out ValueLiteralCode code)
		{
			code = ValueLiteralCode.Normal;
		    // Try to get it if we can.
			if (KeyValueDictionary.TryGetValue(keyString, out var stringVal))
			{
				if (stringVal == "DEFAULTED") code = ValueLiteralCode.Defaulted;
				if (stringVal == "INHERITED") code = ValueLiteralCode.Inherited;
				return new XMLStringValue(stringVal, code);
			}
			return null;
		}
		/// <summary>
		/// See <see cref="IXMLExaminer"/>
		/// </summary>
		/// <returns>
		/// See <see cref="IXMLExaminer"/>
		/// </returns>
		/// <remarks>
		/// <para>
		/// Since this method builds an XElement from the input stream, it
		/// always rewinds it first, since it must see the complete doc.
		/// </para>
		/// </remarks>
		public virtual XElement ReadXMLDocumentLinq()
		{
			XElement retvalElement = null;
			Stream stream = null;
			XmlReader reader = null;
			try
			{
				stream = ReadKeyValues();
				if (stream == null) return null;
				stream.Position = 0;
				reader = XmlReader.Create(stream);
				var xDocument = XDocument.Load(reader);
				retvalElement = xDocument.Root;
			}
			catch (Exception ex)
			{
				// TODO - KRM - make a specific exception, please?
				var logger =
				PAFServices.Manager.GetTypedService<IPAFLoggingService>();
				logger.LogEntry("Error reading XDocument", PAFLoggingLevel.Error, ex);
			}
			finally
			{
			    stream?.Dispose();
			    reader?.Dispose();
			}

			return retvalElement;
		}
		#endregion

		#region IPAFBaseExePipelineInitialize<IXMLExaminerParams> Members
		/// <remarks>
		/// <see cref="IXMLExaminer"/>
		/// </remarks>
		public bool IsExePipelineInitialized { get; protected set; }

		/// <remarks>
		/// <see cref="IXMLExaminer"/>
		/// </remarks>
		public IPAFPipelineParams<IXMLExaminerParams> PipelineParams { get; protected set; }

		/// <summary>
		/// Initializes and optionally re-parametrizes the examiner. Opens a file if stream
		/// not active. Reads in key values, if appropriate. Performs validation, if appropriate.
		/// </summary>
		/// <param name="provider">
		/// Parameters supplied overwrite <see cref="PipelineParams"/> if not <see langword="null"/>.
		/// </param>
		public virtual void InitializeExePipeline(IPAFProviderPattern<IPAFPipelineParams<IXMLExaminerParams>> provider)
		{
			// So we can re-parameterize.
			Uninitialize();

			// Set pipeline params if incoming.
			if ((provider != null) && (provider.ProvidedItem != null))
			{
				if (PipelineParams == null)
					PipelineParams = provider.ProvidedItem;
				else
					PipelineParams = PipelineParams.ReparameterizedCopy(provider.ProvidedItem.ApplicationParameters);
			}
			
			// Open up a stream if the user passed a filename.
			if (!string.IsNullOrEmpty(ExaminerParams.XMLInputFilePath))
			{
			    ExaminerParams.XMLInputStream?.Dispose();
			    ExaminerParams.XMLInputStream = OpenXMLFileForRead(ExaminerParams.XMLInputFilePath);
			}
			// Read in any key/value pairs and rewind stream.
			ReadKeyValues();
		}

		#endregion
	}
}
