using System;
using System.Collections.Generic;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.XML.Exceptions
{
	/// <summary>
	///	Exceptions that occur during XML processing.
	/// </summary>
	[PAFSerializable]
	public abstract class PAFXMLExceptionDataBase :
		PAFAbstractStandardExceptionDataBase, IPAFXMLExceptionData
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Backing for the prop.
		/// </summary>
		internal string m_BadXMLFilePath;
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Constructor builds with the standard arguments plus the <see cref="BadXMLFilePath"/>.
		/// </summary>
		/// <param name="badXMLFilePath">
		/// Loads <see cref="BadXMLFilePath"/>.
		/// </param>
		/// <param name="extensionData">
		/// Sets <see cref="IPAFStandardExceptionData.ExtensionData"/>.
		/// </param>
		/// <param name="isFatal">
		/// See <see cref="PAFAbstractStandardExceptionDataBase"/>
		/// </param>
		/// <param name="pafLoggingLevel">
		/// See <see cref="PAFAbstractStandardExceptionDataBase"/>
		/// </param>
		protected PAFXMLExceptionDataBase(string badXMLFilePath, object extensionData = null,
			PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
			: base(extensionData, pafLoggingLevel, isFatal)
		{
			m_BadXMLFilePath = badXMLFilePath;
		}
		#endregion Constructors
		#region Properties
		/// <summary>
		/// See <see cref="IPAFXMLExceptionData"/>.
		/// </summary>
		public string BadXMLFilePath
		{
			get { return m_BadXMLFilePath; }
			protected internal set { m_BadXMLFilePath = value; }
		}
		#endregion // Properties
	}
}