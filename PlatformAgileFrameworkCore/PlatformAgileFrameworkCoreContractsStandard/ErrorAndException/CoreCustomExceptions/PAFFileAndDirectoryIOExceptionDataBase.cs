using System;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.ErrorAndException.CoreCustomExceptions
{
	/// <summary>
	///	Exceptions that occur during file operations.
	/// </summary>
	[PAFSerializable]
	public abstract class PAFFileAndDirectoryIOExceptionDataBase :
		PAFAbstractStandardExceptionDataBase, IPAFFileAndDirectoryIOExceptionData
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Backing for the prop.
		/// </summary>
		internal string m_BadIOFilePath;
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Constructor builds with the standard arguments plus the <see cref="BadIOFilePath"/>.
		/// </summary>
		/// <param name="badIOFilePath">
		/// Loads <see cref="BadIOFilePath"/>.
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
		protected PAFFileAndDirectoryIOExceptionDataBase(string badIOFilePath, object extensionData = null,
			PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
			: base(extensionData, pafLoggingLevel, isFatal)
		{
			m_BadIOFilePath = badIOFilePath;
		}
		#endregion Constructors
		#region Properties
		/// <summary>
		/// See <see cref="IPAFFileAndDirectoryIOExceptionData"/>.
		/// </summary>
		public string BadIOFilePath
		{
			get { return m_BadIOFilePath; }
			protected internal set { m_BadIOFilePath = value; }
		}
		#endregion // Properties
	}
}