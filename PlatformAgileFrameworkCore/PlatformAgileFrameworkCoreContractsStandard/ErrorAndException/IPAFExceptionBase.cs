using System;
using System.Collections.Generic;
using PlatformAgileFramework.Serializing.HelperCollections;

namespace PlatformAgileFramework.ErrorAndException
{

	/// <summary>
	/// Methods added to <see cref="Exception"/> to make it safe and serializable
	/// and better behaving. We do a lot of work with <see cref="Exception"/>s in PAF
	/// and we need this. Any implementation must extend <see cref="Exception"/>,
	/// otherwise you've got to jump through lots of hoops.
	/// </summary>
	public interface IPAFExceptionBase
	{
		/// <summary>
		/// This clears out the stack traces. This is typically done to "sanitize"
		/// the exception before passing out of the program to external observers.
		/// The idea is to leave just the message and the object store, etc.
		/// </summary>
		/// <threadsafety>
		/// Thread-safe.
		/// </threadsafety>
		void ClearStackTraces();
        /// <summary>
        /// Just grabs the payload.
        /// </summary>
        /// <returns>
        /// The generic.
        /// </returns>
        object GetExceptionData();
		/// <summary>
		/// Usually a pass-through to <see cref="Exception"/>.
		/// </summary>
		/// <returns>Inner exception or <see langword="null"/>.</returns>
		Exception GetInnerException();
		/// <summary>
		/// This method returns an object store in which elements must be serializable.
		/// It reports non-serializable object types with an exception at the time of
		/// their entry into the store. This is arguably somewhat better than
		/// letting the serialization infrastructure throw an exception that may be
		/// arcane. Users of the extended error and exception library can use a
		/// "SerializableExceptionRepresentative" to wrap any exceptions that are
		/// non-serializable for any reason, including those having non-serializable
		/// main dictionary entries.
		/// </summary>
		/// <returns>
		/// The object store.
		/// </returns>
		/// <remarks>
		/// This method creates an <see cref="IStringKeyedSerializableObjectStore"/> if
		/// one does not already exist.
		/// </remarks>
		IStringKeyedSerializableObjectStore GetSerializableObjectStore();
		/// <summary>
		/// This is a workaround for the <see cref="Exception.StackTrace"/> that overwrites
		/// the stack trace when an exception is rethrown. Usual workaround is to call
		/// "InternalPreserveStackTrace" through reflection (one rethrow only). Another
		/// workaround is to put the stack traces in <see cref="Exception.Data"/>. We
		/// provide an access technique that is slightly more organized and hopefully a
		/// bit more secure.
		/// </summary>
		/// <returns>
		/// <see langword="null"/> if store is empty.
		/// </returns>
		/// <threadsafety>
		/// Thread-safe.
		/// </threadsafety>
		IEnumerator<string> GetStackTraces();
		/// <summary>
		/// Determines if a message contains a specific tag.
		/// </summary>
		/// <param name="exceptionTag">
		/// <see langword="null"/> or blank returns <see langword="false"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the the exception's <see cref="Message"/>
		/// contains the tag.
		/// </returns>
		/// <remarks>
		/// If tag is mapped by the resource system, it is the
		/// MAPPED tag that is sought.
		/// </remarks>
		bool HasTag(string exceptionTag);
		/// <summary>
		/// This override fixes MS's stupid mistake in the design of this
		/// property. It just grabs the message exactly as it was set.
		/// </summary>
		string Message { get; }
		/// <summary>
		/// This just returns an exception with its current stack trace stack pushed.
		/// Call this just before rethrowing.
		/// See <see cref="GetStackTraces"/>.
		/// </summary>
		/// <threadsafety>
		/// Thread-safe.
		/// </threadsafety>
		IPAFExceptionBase PushedException();
		/// <summary>
		/// This pushes the current stack trace onto the list.
		/// See <see cref="GetStackTraces"/>.
		/// </summary>
		/// <threadsafety>
		/// Thread-safe.
		/// </threadsafety>
		void PushStackTrace();
		/// <summary>
		/// This just returns an exception with its current stack trace cleared.
		/// Call this just before rethrowing.
		/// See <see cref="GetStackTraces"/>.
		/// </summary>
		/// <threadsafety>
		/// Thread-safe.
		/// </threadsafety>
		/// <remarks>
		/// <para>
		/// Typical usage pattern is to clean off the exception after possibly
		/// capturing its stack traces and other sensitive info and sending
		/// these data over a secure channel, possibly back to a server. Then the
		/// exception can be reported to the user with a benign message -
		/// e.g. "something is wrong with your file system - check if your disk
		/// is full".
		/// </para>
		/// <para>
		/// Subclasses can override this class to delete unwanted entries from
		/// dictionaries, etc..
		/// </para>
		/// </remarks>
		IPAFExceptionBase SanitizedException();
	}
}