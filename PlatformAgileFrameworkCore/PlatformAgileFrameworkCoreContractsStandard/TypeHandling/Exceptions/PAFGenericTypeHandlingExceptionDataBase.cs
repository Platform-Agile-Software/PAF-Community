using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.TypeHandling.Exceptions
{
	/// <summary>
	///	Abstract implementation of <see cref="IPAFGenericTypeHandlingExceptionData"/>.
	/// </summary>
	[PAFSerializable]
	public abstract class PAFGenericTypeHandlingExceptionDataBase :
		PAFAbstractStandardExceptionDataBase,
		IPAFGenericTypeHandlingExceptionData
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Backing for the prop.
		/// </summary>
		internal string m_ProblematicTypeString;
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Constructor builds with the standard arguments plus the
		/// <see cref="IPAFGenericTypeHandlingExceptionData.ProblematicTypeString"/>.
		/// </summary>
		/// <param name="problematicTypeString">
		///     <see cref="IPAFGenericTypeHandlingExceptionData"/>.
		/// </param>
		/// <param name="extensionData">
		///     <see cref="IPAFStandardExceptionData"/>.
		/// </param>
		/// <param name="pafLoggingLevel">
		///     <see cref="IPAFStandardExceptionData"/>.
		/// </param>
		/// <param name="isFatal">
		///     <see cref="IPAFStandardExceptionData"/>.
		/// </param>
		protected PAFGenericTypeHandlingExceptionDataBase(
			string problematicTypeString, object extensionData = null,
			PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
			: base(extensionData, pafLoggingLevel, isFatal)
		{
			m_ProblematicTypeString = problematicTypeString;
		}
		#endregion Constructors
		#region Properties
		/// <summary>
		/// See <see cref="IPAFGenericTypeHandlingExceptionData"/>.
		/// </summary>
		public string ProblematicTypeString
		{
			get { return m_ProblematicTypeString; }
			internal set { m_ProblematicTypeString = value; }
		}
		#endregion // Properties
	}
}