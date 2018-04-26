using System;
using System.Collections.Generic;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.Manufacturing.Exceptions
{
	/// <summary>
	///	Exceptions that occur during loading of an assembly.
	/// </summary>
	[PAFSerializable]
	public abstract class PAFAssemblyLoadExceptionDataBase
		: PAFAbstractStandardExceptionDataBase, IPAFAssemblyLoadExceptionData
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Backing for the prop.
		/// </summary>
		protected internal IPAFAssemblyHolder m_ProblematicAssembly;
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Constructor builds with the standard arguments plus the
		/// <see cref="IPAFAssemblyLoadExceptionData.ProblematicAssembly"/>.
		/// </summary>
		/// <param name="problematicAssembly">
		/// <see cref="IPAFAssemblyLoadExceptionData"/>.
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
		protected PAFAssemblyLoadExceptionDataBase(IPAFAssemblyHolder problematicAssembly = null,
			object extensionData = null, PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
			: base(extensionData, pafLoggingLevel, isFatal)
		{
			m_ProblematicAssembly = problematicAssembly;
		}
		#endregion Constructors
		#region Properties
		/// <summary>
		/// See <see cref="IPAFAssemblyLoadExceptionData"/>.
		/// </summary>
		public IPAFAssemblyHolder ProblematicAssembly
		{
			get { return m_ProblematicAssembly; }
			internal set { m_ProblematicAssembly = value; }
		}
		#endregion // Properties
	}
}