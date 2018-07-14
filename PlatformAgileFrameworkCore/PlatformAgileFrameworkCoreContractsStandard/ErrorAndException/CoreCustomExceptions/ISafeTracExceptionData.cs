
using System.Collections.Generic;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.ErrorAndException.CoreCustomExceptions;

namespace Delta.SafeTrac.ErrorAndException
{
	/// <summary>
	///	Exceptions that occur in SafeTrac. Just a signature exception - no specific data.
	/// </summary>
	public interface ISafeTracExceptionData : IPAFStandardExceptionData
	{
	}
    /// <summary>
    /// Set of tags with an enumerator for exception messages. These are the dictionary keys
    /// for extended.
    /// </summary>
    /// <remarks>
    /// Many more can be added here by developers to give a "high-level" indication of
    /// what's wrong. An inner exception will have details in many cases.
    /// </remarks>
    public class SafeTracExceptionMessageTags
        : PAFExceptionMessageTagsBase<ISafeTracExceptionData>
    {
        #region Fields and Autoproperties
        /// <summary>
        /// Error in the scanning process.
        /// </summary>
        public const string SCANNING_ERROR = "Scanning Error";
        /// <summary>
        /// Error in setting up or initializing the scanner.
        /// </summary>
        public const string SCANNER_INITIALIZATION_ERROR = "Scanner Initialization Error";
        /// <summary>
        /// Error involved in tracking a bag.
        /// </summary>
        public const string BAG_TRACKING_ERROR = "Bag Tracking Error";
        #endregion // Fields and Autoproperties
        /// <summary>
        /// Just puts the tags in a list to hand out.
        /// </summary>
        static SafeTracExceptionMessageTags()
        {
            if ((s_Tags != null) && (s_Tags.Count > 0)) return;
            s_Tags = new List<string>
                {
                    SCANNING_ERROR,
                    SCANNER_INITIALIZATION_ERROR,
                    BAG_TRACKING_ERROR
                };
            PAFAbstractStandardExceptionDataBase.RegisterNamedExceptionTagsInternal(s_Tags, typeof(IPAFTypeExceptionData));
        }

    }
}