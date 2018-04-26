using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.Notification.Exceptions
{
	/// <summary>
	/// Sealed version of <see cref="PAFDelegateExceptionDataBase"/>
	/// </summary>
	[PAFSerializable]
	public sealed class PAFDelegateExceptionData :
		PAFDelegateExceptionDataBase
	{
		#region Constructors
		/// <summary>
		/// Public constructor for
		/// <see cref="PAFDelegateExceptionDataBase"/>
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
		public PAFDelegateExceptionData(IPAFTypeHolder problematicDelegateType = null,
			object extensionData = null,
			PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
			: base(problematicDelegateType, extensionData, pafLoggingLevel, isFatal)
		{
		}
		#endregion Constructors
	}
}