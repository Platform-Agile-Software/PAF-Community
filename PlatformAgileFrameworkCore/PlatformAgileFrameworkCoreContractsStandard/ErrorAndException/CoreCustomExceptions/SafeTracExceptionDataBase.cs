using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;

namespace Delta.SafeTrac.ErrorAndException
{
	/// <summary>
	///	Exceptions that occur during operations in SafeTrac.
	/// </summary>
	[PAFSerializable]
	public abstract class SafeTracExceptionDataBase :
		PAFAbstractStandardExceptionDataBase, ISafeTracExceptionData
	{
		#region Constructors

	    /// <summary>
	    /// Constructor builds with the standard arguments.
	    /// </summary>
	    /// <param name="extensionData">
	    /// See <see cref="PAFAbstractStandardExceptionDataBase"/>
	    /// </param>
	    /// <param name="pafLoggingLevel">
	    /// See <see cref="PAFAbstractStandardExceptionDataBase"/>
	    /// </param>
	    /// <param name="isFatal">
	    /// See <see cref="PAFAbstractStandardExceptionDataBase"/>
	    /// </param>
	    protected SafeTracExceptionDataBase(object extensionData = null,
	        PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
	        : base(extensionData, pafLoggingLevel, isFatal)
	    {
	    }
	#endregion Constructors
	}
}