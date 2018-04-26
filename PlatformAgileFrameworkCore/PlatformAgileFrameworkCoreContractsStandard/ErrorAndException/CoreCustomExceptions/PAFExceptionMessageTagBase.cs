using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PlatformAgileFramework.ErrorAndException.CoreCustomExceptions
{
	/// <summary>
	/// This is a base class to hold exception strings. It's got a bit of thread-safety for
	/// the folks.
	/// </summary>
	/// <history>
	/// <author> KRM </author>
	/// <date> 02jul2015 </date>
	/// <contribution>
	/// New - just a container for protecting access to the tag lists.
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Safe
	/// </threadsafety>
	/// <remarks>
	/// See commented sections on how to inherit from this. The key is in the
	/// implementation of a static constructor.
	/// </remarks>
	// ReSharper disable once PartialTypeWithSinglePart
	// Moved the dictionary to extended......
	public abstract partial class PAFExceptionMessageTagsBase<T>
		: IEnumerable<string> where T:IPAFStandardExceptionData
	{
		#region A Little guidance for the folks
		///// <summary>
		///// First error message.
		///// </summary>
		//public const string FIRST_ERROR_MESSAGE = "First error message";
		///// <summary>
		///// Second error message.
		///// </summary>
		//public const string SECOND_ERROR_MESSAGE = "Second error message";
		#endregion // A Little guidance for the folks
		/// <summary>
		/// Holds the tags.
		/// </summary>
		/// <remarks>
		/// Note that this is a static field in a Generic type. Each closure
		/// of the Generic type will have its own instance. This is what we
		/// want. This would not work in Java.
		/// </remarks>
		// ReSharper disable once StaticMemberInGenericType
		protected internal static IList<string> s_Tags;
		#region A Little more guidance for the folks
		///// <summary>
		///// Just puts the tags in a list to hand out.
		///// </summary>
		//static MyMessageTags()
		//{
		//	if ((s_Tags != null) && (s_Tags.Count > 0)) return;
		//	s_Tags = new List<String>
		//		{
		//			FIRST_ERROR_MESSAGE,
		//			SECOND_ERROR_MESSAGE
		//		};
		//}
		#endregion // A Little more guidance for the folks

		#region Implementation of IEnumerable
		/// <summary>
		/// Safely gets an enumerator by copying a list.
		/// </summary>
		/// <returns>The enumerator.</returns>
		public IEnumerator<string> GetEnumerator()
		{
			lock (s_Tags)
			{
				return s_Tags.ToList().GetEnumerator();
			}
		}
		/// <summary>
		/// Compulsory implementation.
		/// </summary>
		/// <returns>The enumerator</returns>
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		#endregion
		#region Novel Methods
		/// <summary>
		/// Gets the exception type, which should always be constrained to be
		/// an inerface type for PAF developers.
		/// </summary>
		/// <returns>The type.</returns>
		public virtual Type GetExceptionType()
		{
			return typeof (T);
		}
		#endregion // Novel Methods
	}
}
