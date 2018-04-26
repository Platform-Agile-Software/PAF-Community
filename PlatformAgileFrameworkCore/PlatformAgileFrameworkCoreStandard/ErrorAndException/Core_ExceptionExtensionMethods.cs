using System;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;

namespace PlatformAgileFramework.ErrorAndException
{
	/// <summary>
	///	Extends exceptions in a way that allows us to examine the details of an exception
	/// and it's inner exception(s).
	/// </summary>
// ReSharper disable PartialTypeWithSinglePart
	public static partial class ExceptionExtensionMethods
// ReSharper restore PartialTypeWithSinglePart
	{
        public static string[] s_IgnoredProps
        = new string[]
        { "IsFatal", "ExtensionData", "LogLevel", "SpecificExceptionTags", "PropertyFormattingExclusionNames" };
		#region Methods
		/// <summary>
		/// This is a little helper method that determines whether a given exception type is contained
		/// within the internal "inner exception" chain. The reason for this construct is to allow
		/// "standard" exceptions (e.g. <see cref="System.IO.IOException"/> to be filtered even though
		/// such an exception is not in the inheritance chain.
		/// </summary>
		/// <param name="thisException"> "this" </param>
		/// <param name="exceptionType">The <see cref="System.Exception"/>) that is to be checked.</param>
		/// <returns>
		/// <see langword="true"/> if the specific Sub-Type of the <see cref="System.Exception"/> is a base Type of
		/// any exception anywhere in the containment chain of "inner exception"s.
		/// </returns>
		/// <remarks>
		/// Example:<br/>
		/// If a <see cref="PAFAbstractExceptionBase"/> contains an inner exception
		/// <see cref="System.IO.FileNotFoundException"/>
		/// and the client passes in an <see cref="System.IO.IOException"/>
		/// (the base type of <see cref="System.IO.FileNotFoundException"/>) the method would
		/// return <see langword="true"/>.
		/// </remarks>
		public static bool Contains(this Exception thisException, Type exceptionType)
		{
			var currentInnerException = thisException.InnerException;

			// Walk down the containment tree.
			while (currentInnerException != null)
			{
				if (currentInnerException.GetType() == exceptionType)
				{
					return true;
				}
				if (currentInnerException.GetType().IsTypeASubtypeOf(exceptionType))
				{
					return true;
				}
				currentInnerException = currentInnerException.InnerException;
			}

			return false;
		}

		/// <summary>
		/// Just helps flag bad arg. Throws an exception if arg is <see langword="null"/>. Useful
		/// in constructors.
		/// </summary>
		/// <param name="argName">
		/// Name that is to be displayed in the <see cref="ArgumentNullException"/>.
		/// </param>
		/// <param name="obj">
		/// Incoming <see cref="Object"/>.
		/// </param>
		/// <returns>
		/// The <paramref name="obj"/>, if not <see langword="null"/>.
		/// </returns>
		//public static object ExceptNullObject (this object obj, string argName)
		//{
		//	if (obj == null) throw new ArgumentNullException(argName);
		//	return obj;
		//}

		/// <summary>
		/// This method is the same as <c>Contains</c>, but also returns <see langword="true"/> if the
		/// <paramref name="exceptionType"/> matches the Type of the <see cref="Exception"/> it
		/// is called on or if "this" is a subtype of the <paramref name="exceptionType"/>.
		/// </summary>
		/// <param name="thisException"> "this" </param>
		/// <param name="exceptionType">See <see cref="Contains"/>.</param>
		/// <returns>See <see cref="Contains"/></returns>
		public static bool Is(this Exception thisException, Type exceptionType)
		{
			// Try us first
			if (thisException.GetType() == exceptionType)
			{
				return true;
			}
			if (thisException.GetType().IsTypeASubtypeOf(exceptionType))
			{
				return true;
			}
			// OK, try contained exceptions.
			return Contains(thisException, exceptionType);
		}
        /// <summary>
        /// This method renders an exception to a string at two levels. Level 1 just
        /// prints out the message with any properties on the exception. Level 2
        /// also includes the full stack trace.
        /// </summary>
        /// <param name="thisException">
        ///  "this". <see langword="null"/> returns <see cref="string.Empty"/>. 
        /// </param>
        /// <param name="detailLevel">
        /// The level. 2 prints the stack trace, level 1 prints properties from an <see cref="IPAFExceptionBase"/>. 
        /// </param>
        /// <returns>Rendered string.</returns>
        public static string RenderExceptionDetail(this Exception thisException, int detailLevel)
        {
            if (thisException == null)
                return string.Empty;
            var message = thisException.Message;
            if (detailLevel == 0)
                return message;
            var pAFException = thisException as IPAFExceptionBase;
            var pAFExceptionData = pAFException?.GetExceptionData();
            if (pAFExceptionData != null)
            message = TypeExtensions.PublicPropsToString(pAFExceptionData, message, s_IgnoredProps);
            if (detailLevel == 1) return message;

            if (pAFExceptionData != null)
                return message;
            message += thisException.ToString();
            return message;
        }
        /// <summary>
        /// This method is the same as <see cref="Contains"/>, except checks only the immediate "inner exception" and does not
        /// walk the chain.
        /// </summary>
        /// <param name="thisException"> "this" </param>
        /// <param name="exceptionType">See <see cref="Contains"/>.</param>
        /// <returns>See <see cref="Contains"/>.</returns>
        public static bool Wraps(this Exception thisException, Type exceptionType)
        {
            var innerException = thisException.InnerException;

            // Just check the first.
            if (innerException != null)
            {
                if (innerException.GetType() == exceptionType)
                {
                    return true;
                }
                if (innerException.GetType().IsTypeASubtypeOf(exceptionType))
                {
                    return true;
                }
            }
            return false;
        }
		#endregion Methods
	}
}
