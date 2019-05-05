using System;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.ErrorAndException.CoreCustomExceptions
{
	/// <summary>
	///	Sealed version of <see cref="PAFTypeExceptionDataBase"/>.
	/// </summary>
	[PAFSerializable]
	public sealed class PAFTypeExceptionData : PAFTypeExceptionDataBase
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
		public PAFTypeExceptionData(IPAFTypeHolder problematicType, object extensionData = null,
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
		public
			PAFTypeExceptionData(Type problematicType, object extensionData = null,
				PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
			: base(problematicType, extensionData, pafLoggingLevel, isFatal)
		{
		}
	}
}