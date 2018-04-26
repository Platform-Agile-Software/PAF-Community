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
	/// </summary>
	[PAFSerializable]
	public abstract class PAFTypeMismatchExceptionDataBase :
		PAFTypeExceptionDataBase , IPAFTypeMismatchExceptionData
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Backing for the prop.
		/// </summary>
		internal IPAFTypeHolder m_ProblematicIncompatibleType;
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Constructor builds with the standard arguments plus the
		/// <see cref="IPAFTypeExceptionData.ProblematicType"/>
		/// and <see cref="IPAFTypeMismatchExceptionData.ProblematicIncompatibleType"/>.
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
		protected PAFTypeMismatchExceptionDataBase(IPAFTypeHolder problematicType = null,
			IPAFTypeHolder problematicIncompatibleType = null, object extensionData = null,
			PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
			: base(problematicType, extensionData, pafLoggingLevel, isFatal)
		{
			m_ProblematicIncompatibleType = problematicIncompatibleType;
		}
		#endregion Constructors
		#region Properties
		/// <summary>
		/// See <see cref="IPAFTypeExceptionData"/>.
		/// </summary>
		public IPAFTypeHolder ProblematicIncompatibleType
		{
			get { return m_ProblematicIncompatibleType; }
			internal set { m_ProblematicIncompatibleType = value; }
		}
		#endregion // Properties
	}
}