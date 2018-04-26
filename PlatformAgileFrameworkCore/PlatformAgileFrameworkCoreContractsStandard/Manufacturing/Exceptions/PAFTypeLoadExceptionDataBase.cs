using System;
using System.Collections.Generic;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.Manufacturing.Exceptions
{
	/// <summary>
	///	Exceptions that occur during loading of a type from an assembly.
	/// </summary>
	[PAFSerializable]
	public abstract class PAFTypeLoadExceptionDataBase
		: PAFAssemblyLoadExceptionDataBase, IPAFTypeLoadExceptionData
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Backing for the prop.
		/// </summary>
		protected internal IPAFTypeHolder m_ProblematicType;
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Constructor builds with the standard arguments plus the
		/// <see cref="IPAFTypeLoadExceptionData.ProblematicType"/>.
		/// </summary>
		/// <param name="problematicType">
		/// <see cref="IPAFTypeLoadExceptionData"/>.
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
		protected PAFTypeLoadExceptionDataBase(IPAFTypeHolder problematicType = null,
			object extensionData = null, PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
			: base((problematicType == null) ? null: problematicType.GetAssemblyHolder(), extensionData, pafLoggingLevel, isFatal)
		{
			m_ProblematicType = problematicType;
		}
		#endregion Constructors
		#region Properties
		/// <summary>
		/// See <see cref="IPAFTypeLoadExceptionData"/>.
		/// </summary>
		public IPAFTypeHolder ProblematicType
		{
			get { return m_ProblematicType; }
			internal set { m_ProblematicType = value; }
		}
		#endregion // Properties
	}
}