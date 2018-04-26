using System;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.FrameworkServices.Exceptions
{
	/// <summary>
	///	Sealed version of <see cref="PAFServiceTypeMismatchExceptionDataBase"/>.
	/// </summary>
	[PAFSerializable]
	public sealed class PAFServiceTypeMismatchExceptionData
		: PAFServiceTypeMismatchExceptionDataBase
	{
		/// <summary>
		/// See <see cref="PAFServiceExceptionDataBase"/>.
		/// <see cref="IPAFServiceExceptionData.ProblematicService"/>.
		/// </summary>
		/// <param name="problematicService">
		/// See <see cref="IPAFServiceExceptionData"/>.
		/// </param>
		/// <param name="requiredType">
		/// See <see cref="IPAFServiceTypeMismatchExceptionData"/>.
		/// </param>
		/// <param name="extensionData">
		/// <see cref="IPAFStandardExceptionData"/>.
		/// </param>
		/// <param name="isFatal">
		/// <see cref="IPAFStandardExceptionData"/>.
		/// </param>
		/// <param name="pafLoggingLevel">
		/// <see cref="IPAFStandardExceptionData"/>.
		/// </param>
		public PAFServiceTypeMismatchExceptionData(IPAFServiceDescription problematicService,
			IPAFTypeHolder requiredType, object extensionData = null,
			PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
			: base(problematicService, requiredType, extensionData,
			pafLoggingLevel, isFatal)
		{
		}
	}
}