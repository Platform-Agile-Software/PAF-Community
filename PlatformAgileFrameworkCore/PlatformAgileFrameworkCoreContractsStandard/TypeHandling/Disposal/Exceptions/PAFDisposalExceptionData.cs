using System;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.ErrorAndException.CoreCustomExceptions;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;
namespace PlatformAgileFramework.TypeHandling.Disposal.Exceptions
{
	/// <summary>
	///	Sealed version of <see cref="PAFDisposalExceptionDataBase"/>.
	/// </summary>
	[PAFSerializable]
	public sealed class PAFDisposalExceptionData : PAFDisposalExceptionDataBase
	{
		/// <summary>
		/// See <see cref="PAFTypeExceptionDataBase"/>.
		/// <see cref="IPAFTypeExceptionData.ProblematicType"/>.
		/// </summary>
		/// <param name="problematicType">
		/// See <see cref="PAFTypeExceptionDataBase"/>.
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
		public PAFDisposalExceptionData(IPAFTypeHolder problematicType, object extensionData = null,
				PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
			: base(problematicType, extensionData, pafLoggingLevel, isFatal)
		{
		}
		/// <summary>
		/// See <see cref="PAFTypeExceptionDataBase"/>.
		/// <see cref="Type"/>.
		/// </summary>
		/// <param name="problematicType">
		/// See <see cref="PAFTypeExceptionDataBase"/>.
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
		public PAFDisposalExceptionData(Type problematicType, object extensionData = null,
				PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
			: base(problematicType, extensionData, pafLoggingLevel, isFatal)
		{
		}
	}
}