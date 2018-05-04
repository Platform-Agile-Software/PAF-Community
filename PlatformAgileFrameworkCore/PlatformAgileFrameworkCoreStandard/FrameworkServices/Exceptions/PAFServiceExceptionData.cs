using System;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.FrameworkServices.Exceptions
{
	/// <summary>
	///	Sealed version of <see cref="PAFServiceExceptionDataBase"/>.
	/// </summary>
	[PAFSerializable]
	public sealed class PAFServiceExceptionData : PAFServiceExceptionDataBase
	{
		/// <summary>
		/// See <see cref="PAFServiceExceptionDataBase"/>.
		/// <see cref="IPAFServiceExceptionData.ProblematicService"/>.
		/// </summary>
		/// <param name="problematicService">
		/// See <see cref="PAFServiceExceptionDataBase"/>.
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
		public PAFServiceExceptionData(IPAFServiceDescription problematicService,
			object extensionData = null, PAFLoggingLevel? pafLoggingLevel = null,
			bool? isFatal = null)
			: base(problematicService, extensionData, pafLoggingLevel, isFatal)
		{
		}
	}
}