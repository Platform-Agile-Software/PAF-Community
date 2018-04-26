using System;
using System.Collections.Generic;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.TypeHandling.Disposal
{
	/// <summary>
	///	This exception is thrown when a finalizer is called and it should not be.
	/// </summary>
	[PAFSerializable]
	public sealed class PAFFinalizerExceptionData : PAFAbstractStandardExceptionDataBase
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Error message
		/// </summary>
		public const string FINALIZER_WAS_CALLED = "Finalizer was called";
		/// <summary>
		/// Backing for the prop.
		/// </summary>
		internal PAFTypeHolder m_FinalizationType;
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Static constructor loads our tags for extended.
		/// </summary>
		static PAFFinalizerExceptionData()
		{
			var tags = new List<string>
			{
			    FINALIZER_WAS_CALLED
			};
			// Register the exception.
			RegisterNamedExceptionTagsInternal(tags, typeof(IPAFFinalizerExceptionData));
		}

		/// <summary>
		/// Constructor builds with the standard arguments plus the <see cref="PAFTypeHolder"/>.
		/// </summary>
		/// <param name="finalizationType">
		/// Loads <see cref="FinalizationType"/>. May be <see langword="null"/>.
		/// </param>
		/// <param name="extensionData">
		/// Sets <see cref="IPAFStandardExceptionData.ExtensionData"/>.
		/// </param>
		/// <param name="pafLoggingLevel">
		/// See <see cref="PAFAbstractStandardExceptionDataBase"/>
		/// </param>
		/// <param name="isFatal">
		/// See <see cref="PAFAbstractStandardExceptionDataBase"/>
		/// </param>
		public PAFFinalizerExceptionData(PAFTypeHolder finalizationType, object extensionData = null,
			PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
			: base(extensionData, pafLoggingLevel, isFatal)
		{
			// TODO custom exception.
			if(finalizationType == null) throw new ArgumentNullException("finalizationType");
			m_FinalizationType = finalizationType;
		}
		#endregion Constructors
		#region Properties
		/// <summary>
		/// Representation of the type the finalizer was called on.
		/// </summary>
		public PAFTypeHolder FinalizationType
		{
			get { return m_FinalizationType; }
			internal set { m_FinalizationType = value; }
		}
		#endregion // Properties
	}
}