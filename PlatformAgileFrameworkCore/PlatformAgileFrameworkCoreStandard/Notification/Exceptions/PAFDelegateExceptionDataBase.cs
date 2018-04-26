using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.Notification.Exceptions
{
	/// <summary>
	///	Implementation of <see cref="IPAFDelegateExceptionData"/>.
	/// </summary>
	[PAFSerializable]
	public abstract class PAFDelegateExceptionDataBase :
		PAFAbstractStandardExceptionDataBase, IPAFDelegateExceptionData
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Backing for the prop.
		/// </summary>
		internal IPAFTypeHolder m_ProblematicDelegateType;
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Constructor builds with the standard arguments plus the
		/// <see cref="IPAFDelegateExceptionData.ProblematicDelegateType"/>.
		/// </summary>
		/// <param name="problematicDelegateType">
		/// <see cref="IPAFDelegateExceptionData"/>.
		/// </param>
		/// <param name="extensionData">
		/// <see cref="IPAFStandardExceptionData"/>.
		/// </param>
		/// <param name="pafLoggingLevel">
		/// <see cref="IPAFStandardExceptionData"/>.
		/// </param>
		/// <param name="isFatal">
		/// <see cref="IPAFStandardExceptionData"/>.
		/// </param>
		protected PAFDelegateExceptionDataBase
		(
			IPAFTypeHolder problematicDelegateType = null,
			object extensionData = null,
			PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null
		)
		: base(extensionData, pafLoggingLevel, isFatal)
		{
			m_ProblematicDelegateType = problematicDelegateType;
		}
		#endregion Constructors
		#region Properties
		/// <summary>
		/// See <see cref="IPAFDelegateExceptionData"/>.
		/// </summary>
		public IPAFTypeHolder ProblematicDelegateType
		{
			get { return m_ProblematicDelegateType; }
			internal set { m_ProblematicDelegateType = value; }
		}
		#endregion // Properties
	}
}