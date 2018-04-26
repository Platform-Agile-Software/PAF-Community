using System;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.ErrorAndException.CoreCustomExceptions
{
	/// <summary>
	///	Exceptions that occur during file operations.
	/// </summary>
	[PAFSerializable]
	public sealed class PAFFileAndDirectoryIOExceptionData :
		PAFFileAndDirectoryIOExceptionDataBase
	{
		#region Constructors

		/// <summary>
		/// Constructor builds with the standard arguments plus the <paramref name="badIOFilePath"/>.
		/// </summary>
		/// <param name="badIOFilePath">
		/// Loads <see cref="IPAFFileAndDirectoryIOExceptionData.BadIOFilePath"/>.
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
		public PAFFileAndDirectoryIOExceptionData(string badIOFilePath, object extensionData = null,
			PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
			: base(badIOFilePath, extensionData, pafLoggingLevel, isFatal)
		{
			m_BadIOFilePath = badIOFilePath;
		}
		#endregion Constructors
	}
}