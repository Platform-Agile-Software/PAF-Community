using System;
using System.Collections.Generic;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.ErrorAndException.CoreCustomExceptions;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.TypeHandling.Disposal
{
	/// <summary>
	///	This exception is thrown when a finalizer is called and it should not be.
	/// </summary>
	public interface IPAFFinalizerExceptionData
		: IPAFStandardExceptionData
	{
		/// <summary>
		/// Representation of the type the finalizer was called on.
		/// </summary>
		PAFTypeHolder FinalizationType { get; }
	}


	/// <summary>
	/// Set of tags with an enumerator for exception messages. These are the dictionary keys
	/// for extended.
	/// </summary>
	public class PAFFinalizerExceptionDataMessageTags
		: PAFExceptionMessageTagsBase<IPAFFinalizerExceptionData>
	{
		/// <summary>
		/// Error message
		/// </summary>
		public const string FINALIZER_WAS_CALLED = "Finalizer was called";

		/// <summary>
		/// Just puts the tags in a list to hand out.
		/// </summary>
		static PAFFinalizerExceptionDataMessageTags()
		{
			if ((s_Tags != null) && (s_Tags.Count > 0)) return;
			s_Tags = new List<string>
				{
					FINALIZER_WAS_CALLED
				};
            // Always store by the interface for an aggregable exception.
            PAFAbstractStandardExceptionDataBase.RegisterNamedExceptionTagsInternal(s_Tags,
                typeof(IPAFFinalizerExceptionData));
		}

	}

}