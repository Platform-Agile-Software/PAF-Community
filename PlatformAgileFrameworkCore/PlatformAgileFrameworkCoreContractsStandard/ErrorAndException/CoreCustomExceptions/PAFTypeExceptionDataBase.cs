using System;
using System.Collections.ObjectModel;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.ErrorAndException.CoreCustomExceptions
{
	/// <summary>
	///	Exceptions that occur during operations on types.
	/// </summary>
	[PAFSerializable]
	public abstract class PAFTypeExceptionDataBase :
		PAFAbstractStandardExceptionDataBase, IPAFTypeExceptionData
	{
		#region Fields and Autoproperties
		/// <summary>
        /// Backing for the prop.
        /// </summary>
        internal IPAFTypeHolder m_ProblematicType;
		#endregion // Fields and Autoproperties
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
	    protected PAFTypeExceptionDataBase(IPAFTypeHolder problematicType, object extensionData = null,
	        PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
	        : base(extensionData, pafLoggingLevel, isFatal)
	    {
	        m_ProblematicType = problematicType;
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
	    protected PAFTypeExceptionDataBase(Type problematicType, object extensionData = null,
	        PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
	        : base(extensionData, pafLoggingLevel, isFatal)
	    {
	        m_ProblematicType = new PAFTypeHolder(problematicType);
	    }
		#endregion Constructors
        #region Properties
        /// <summary>
        /// See <see cref="IPAFTypeExceptionData"/>.
        /// </summary>
        public IPAFTypeHolder ProblematicType
		{
			get { return m_ProblematicType; }
			internal set { m_ProblematicType = value; }
		}
		#endregion // Properties
	}
}