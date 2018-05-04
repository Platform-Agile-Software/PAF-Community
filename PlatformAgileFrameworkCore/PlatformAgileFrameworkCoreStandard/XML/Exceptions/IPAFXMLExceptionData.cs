using System;
using System.Collections.Generic;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.ErrorAndException.CoreCustomExceptions;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.XML.Exceptions
{
	/// <summary>
	///	Exceptions that occur during XML processing.
	/// </summary>
	[PAFSerializable]
	public interface IPAFXMLExceptionData
		: IPAFStandardExceptionData
	{
		#region Properties
		/// <summary>
		/// The problematic XML file if the problem is file-related. <see langword="null"/>
		/// if not file-related.
		/// </summary>
		string BadXMLFilePath { get; }
		#endregion // Properties
	}
    /// <summary>
    /// Set of tags with an enumerator for exception messages. These are the dictionary keys
    /// for extended.
    /// </summary>
    public class PAFXMLExceptionMessageTags
        : PAFExceptionMessageTagsBase<IPAFXMLExceptionData>
    {
        #region Fields and Autoproperties
        /// <summary>
        /// Error message. General wrapper exception for a variety of problems
        /// with processing XML documents or fragments. This error is used as a wrapper
        /// for exceptions that are associated with malformed XML and other "soft"
        /// errors that need not cause the termination of an XML read or other operations.
        /// </summary>
        public const string XML_PROCESSING_ERROR = "XML processing error";
        /// <summary>
        /// Error message. Error indicating that the number of XML processi9ng errors
        /// has exceeded a threashold.
        /// </summary>
        public const string EXCESSIVE_XML_PROCESSING_ERRORS = "Excessive XML processing errors";
        #endregion // Fields and Autoproperties
        /// <summary>
        /// Just puts the tags in a list to hand out.
        /// </summary>
        static PAFXMLExceptionMessageTags()
        {
            if ((s_Tags != null) && (s_Tags.Count > 0)) return;
            s_Tags = new List<string>
            {
                XML_PROCESSING_ERROR,
                EXCESSIVE_XML_PROCESSING_ERRORS
            };
            // Always store by the interface for an aggregable exception.
            PAFAbstractStandardExceptionDataBase.RegisterNamedExceptionTagsInternal(s_Tags,
                typeof(IPAFXMLExceptionData));
        }

    }

}
