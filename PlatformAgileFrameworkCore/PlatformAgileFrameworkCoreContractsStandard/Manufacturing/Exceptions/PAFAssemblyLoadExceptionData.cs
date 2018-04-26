using System;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.Manufacturing.Exceptions
{
	/// <summary>
	///	Exceptions that occur during assembly load.
	/// </summary>
	[PAFSerializable]
	public sealed class PAFAssemblyLoadExceptionData :
		PAFAssemblyLoadExceptionDataBase
	{
		#region Constructors
		/// <summary>
		/// See base.
		/// </summary>
		/// <param name="problematicAssembly">
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
		public PAFAssemblyLoadExceptionData(IPAFAssemblyHolder problematicAssembly = null,
			object extensionData = null, PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
			: base(problematicAssembly, extensionData, pafLoggingLevel, isFatal)
		{
		}
		#endregion Constructors
	}
}