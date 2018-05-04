using System;
using System.Collections.Generic;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.ErrorAndException.CoreCustomExceptions;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.TypeHandling.Exceptions
{
	/// <summary>
	///	Exceptions that occur during operations involving two types.
	/// Sealed version of <see cref="PAFTypeMismatchExceptionDataBase"/>
	/// </summary>
	[PAFSerializable]
	public sealed class PAFTypeMismatchExceptionData :
		PAFTypeMismatchExceptionDataBase
	{
		#region Constructors
		/// <summary>
		/// Public constructor for
		/// <see cref="PAFTypeMismatchExceptionDataBase"/>
		/// </summary>
		/// <param name="problematicType">
		/// <see cref="IPAFTypeExceptionData"/>.
		/// </param>
		/// <param name="problematicIncompatibleType">
		/// <see cref="IPAFTypeMismatchExceptionData"/>.
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
		public PAFTypeMismatchExceptionData(IPAFTypeHolder problematicType = null,
			IPAFTypeHolder problematicIncompatibleType = null, object extensionData = null,
			PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
			: base(problematicType, problematicIncompatibleType, extensionData, pafLoggingLevel, isFatal)
		{
		}
		#endregion Constructors
	}
}