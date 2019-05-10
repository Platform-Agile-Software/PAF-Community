using System;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.ErrorAndException.CoreCustomExceptions;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;
namespace PlatformAgileFramework.TypeHandling.Disposal.Exceptions
{
	/// <summary>
	///	Exceptions that occur during disposal operations.
	/// See <see cref="IPAFDisposalExceptionData"/>.
	/// </summary>
	[PAFSerializable]
	public abstract class PAFDisposalExceptionDataBase :
		PAFTypeExceptionDataBase, IPAFDisposalExceptionData
	{

		#region Constructors

		/// <summary>
		/// Constructor builds with the standard arguments plus the
		/// <see cref="IPAFTypeExceptionData.ProblematicType"/>.
		/// </summary>
		/// <param name="problematicType">
		/// See <see cref="IPAFTypeExceptionData"/>.
		/// </param>
		/// <param name="extensionData">
		/// See <see cref="PAFAbstractStandardExceptionDataBase"/>
		/// </param>
		/// <param name="pafLoggingLevel">
		/// See <see cref="PAFAbstractStandardExceptionDataBase"/>
		/// </param>
		/// <param name="isFatal">
		/// See <see cref="PAFAbstractStandardExceptionDataBase"/>
		/// </param>
		protected PAFDisposalExceptionDataBase(IPAFTypeHolder problematicType, object extensionData = null,
	        PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
	        : base(problematicType,  extensionData, pafLoggingLevel, isFatal)
	    {
	    }
	    /// <summary>
	    /// Constructor builds with the standard arguments plus the
	    /// <see cref="Type"/>.
	    /// </summary>
	    /// <param name="problematicType">
	    /// See <see cref="Type"/>.
	    /// </param>
	    /// <param name="extensionData">
	    /// See <see cref="PAFAbstractStandardExceptionDataBase"/>
	    /// </param>
	    /// <param name="pafLoggingLevel">
	    /// See <see cref="PAFAbstractStandardExceptionDataBase"/>
	    /// </param>
	    /// <param name="isFatal">
	    /// See <see cref="PAFAbstractStandardExceptionDataBase"/>
	    /// </param>
	    protected PAFDisposalExceptionDataBase(Type problematicType, object extensionData = null,
	        PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
	        : base(problematicType, extensionData, pafLoggingLevel, isFatal)
	    {
	    }
		#endregion // Constructors
	}
}