using System;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.Collections.Exceptions
{
	/// <summary>
	/// Sealed version of <see cref="PAFIncompatibleCollectionEntryExceptionDataBase"/>.
	/// </summary>
	[PAFSerializable]
	public sealed class PAFIncompatibleCollectionEntryExceptionData :
		PAFIncompatibleCollectionEntryExceptionDataBase
	{
		#region Constructors
		/// <remarks>
		/// See base.
		/// </remarks>
		public PAFIncompatibleCollectionEntryExceptionData(object badCollectionEntry = null,
			object incompatibeCollectionEntry = null, object extensionData = null,
			PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
			: base(badCollectionEntry, incompatibeCollectionEntry, extensionData,
			pafLoggingLevel, isFatal)
		{
		}
		#endregion Constructors
	}
}