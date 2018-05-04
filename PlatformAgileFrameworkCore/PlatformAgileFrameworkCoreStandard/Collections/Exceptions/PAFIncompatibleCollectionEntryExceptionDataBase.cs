using System;
using System.Collections.ObjectModel;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.Collections.Exceptions
{
	/// <summary>
	///	<see cref="IPAFIncompatibleCollectionEntryExceptionData"/>
	/// </summary>
	[PAFSerializable]
	public abstract class PAFIncompatibleCollectionEntryExceptionDataBase :
		PAFBadCollectionEntryExceptionDataBase, IPAFIncompatibleCollectionEntryExceptionData
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Backing for the prop.
		/// </summary>
		internal object m_IncomatibleCollectionEntry;
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <remarks>
		/// Constructor builds with the standard arguments plus the
		/// <see cref="IPAFBadCollectionEntryExceptionData.BadCollectionEntry"/>
		/// and <see cref="IPAFIncompatibleCollectionEntryExceptionData.IncompatibleCollectionEntry"/>.
		/// </remarks>
		protected PAFIncompatibleCollectionEntryExceptionDataBase(object badCollectionEntry = null,
			object incompatibeCollectionEntry = null, object extensionData = null,
			PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
			: base(badCollectionEntry, extensionData, pafLoggingLevel, isFatal)
		{
			m_IncomatibleCollectionEntry = incompatibeCollectionEntry;
		}
		#endregion Constructors
		#region Properties
		/// <summary>
		/// See <see cref="IPAFIncompatibleCollectionEntryExceptionData"/>.
		/// </summary>
		public object IncompatibleCollectionEntry
		{
			get { return m_IncomatibleCollectionEntry; }
			internal set { m_IncomatibleCollectionEntry = value; }
		}
		#endregion // Properties

	}
}