using System;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.XML.Exceptions
{
	/// <summary>
	///	Exceptions that occur during file operations.
	/// </summary>
	[PAFSerializable]
	public sealed class PAFXMLExceptionData :
		PAFXMLExceptionDataBase
	{
		#region Constructors
		/// <summary>
		/// Constructor builds with the standard arguments plus the <paramref name="badXMLFilePath"/>.
		/// </summary>
		/// <param name="badXMLFilePath">
		/// Loads <see cref="IPAFXMLExceptionData.BadXMLFilePath"/>.
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
		public PAFXMLExceptionData(string badXMLFilePath, object extensionData = null,
			PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
			: base(badXMLFilePath, extensionData, pafLoggingLevel, isFatal)
		{
			m_BadXMLFilePath = badXMLFilePath;
		}
		#endregion Constructors
	}
}