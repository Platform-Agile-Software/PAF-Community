using System;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.Manufacturing.Exceptions
{
	/// <summary>
	///	Exceptions that occur during loading types from assemblies.
	/// </summary>
	[PAFSerializable]
	public sealed class PAFTypeLoadExceptionData: PAFTypeLoadExceptionDataBase
	{
		#region Constructors
		/// <summary>
		/// See base.
		/// </summary>
		/// <param name="problematicType">
		/// See base.
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
		public PAFTypeLoadExceptionData(IPAFTypeHolder problematicType = null,
			object extensionData = null, PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
			: base(problematicType, extensionData, pafLoggingLevel, isFatal)
		{
		}
		#endregion Constructors
	}
}