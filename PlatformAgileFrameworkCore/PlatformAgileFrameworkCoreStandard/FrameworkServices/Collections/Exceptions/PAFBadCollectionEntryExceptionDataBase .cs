using System;
using System.Collections.ObjectModel;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.Collections.Exceptions
{
	/// <summary>
	///	Exceptions that occur when an item is somehow incompatible with
	/// a collection.
	/// </summary>
	[PAFSerializable]
	public abstract class PAFBadCollectionEntryExceptionDataBase :
		PAFAbstractStandardExceptionDataBase, IPAFBadCollectionEntryExceptionData
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Backing for the prop.
		/// </summary>
		internal object m_BadCollectionEntry;
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Constructor builds with the standard arguments plus the
		/// <see cref="IPAFBadCollectionEntryExceptionData.BadCollectionEntry"/>.
		/// </summary>
		/// <param name="badCollectionEntry">
		/// See <see cref="IPAFBadCollectionEntryExceptionData"/>.
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
		protected PAFBadCollectionEntryExceptionDataBase(object badCollectionEntry,
			object extensionData = null,
			PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
			: base(extensionData, pafLoggingLevel, isFatal)
		{
			m_BadCollectionEntry = badCollectionEntry;
		}
		#endregion Constructors
		#region Properties
		/// <summary>
		/// See <see cref="IPAFBadCollectionEntryExceptionData"/>.
		/// </summary>
		public object BadCollectionEntry
		{
			get { return m_BadCollectionEntry; }
			internal set { m_BadCollectionEntry = value; }
		}
		#endregion // Properties
	}
}