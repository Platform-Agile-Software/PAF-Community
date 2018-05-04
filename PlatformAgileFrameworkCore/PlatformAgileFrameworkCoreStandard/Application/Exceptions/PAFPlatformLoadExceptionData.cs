using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Logging;

namespace PlatformAgileFramework.Application.Exceptions
{
    /// <summary>
    ///	See base class. This is the sealed version.
    /// </summary>
    //[PAFSerializable]
    public sealed class PAFPlatformLoadExceptionData :
		PAFPlatformLoadExceptionDataBase
	{
		#region Constructors
		/// <summary>
		/// Constructor builds with the standard arguments plus the <paramref name="platformAssemblyOrName"/>.
		/// </summary>
		/// <param name="platformAssemblyOrName">
		/// Loads <see cref="IPAFPlatformLoadExceptionData.PlatformAssemblyOrName"/>.
		/// </param>
		/// <param name="extensionData">
		/// Sets <see cref="IPAFStandardExceptionData.ExtensionData"/>.
		/// </param>
		/// <param name="isFatal">
		/// See <see cref="PAFAbstractStandardExceptionDataBase"/>
		/// </param>
		/// <param name="pafLoggingLevel">
		/// See <see cref="PAFAbstractStandardExceptionDataBase"/>
		/// </param>
		public PAFPlatformLoadExceptionData(string platformAssemblyOrName, object extensionData = null,
			PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
			: base(platformAssemblyOrName, extensionData, pafLoggingLevel, isFatal)
		{
		}
		#endregion Constructors
	}
}