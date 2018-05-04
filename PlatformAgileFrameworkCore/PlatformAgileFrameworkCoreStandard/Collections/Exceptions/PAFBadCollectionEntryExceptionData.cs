using System;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.Collections.Exceptions
{
	/// <summary>
	/// Sealed version of <see cref="PAFBadCollectionEntryExceptionDataBase"/>.
	/// </summary>
	[PAFSerializable]
	public sealed class PAFBadCollectionEntryExceptionData :
		PAFBadCollectionEntryExceptionDataBase
	{
		#region Constructors
		/// <remarks>
		/// <see cref="PAFBadCollectionEntryExceptionDataBase"/>
		/// </remarks>
		public PAFBadCollectionEntryExceptionData(object badCollectionEntry = null,
			object extensionData = null,
			PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
			: base(badCollectionEntry, extensionData,
			pafLoggingLevel, isFatal)
		{
		}
		#endregion Constructors
	}
}