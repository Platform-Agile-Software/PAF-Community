using System;
using PlatformAgileFramework.ErrorAndException.Delegates;

namespace PlatformAgileFramework.ErrorAndException
{
	/// <summary>
	///	Provides for a type to act as a receiver of exceptions. The use of exceptions
	/// are many, and are not limited to fault information. Even in the simple use of
	/// the <see cref="Exception"/> type for communicating fault information, it's
	/// sometimes useful to collect them for analysis. Thus, it's useful to have
	/// a receiver of some sort sometimes.
	/// </summary>
	public interface IExceptionReceiverProvider
	{
		/// <summary>
		/// This is the standard <see cref="ExceptionReceiver"/> delegate;
		/// </summary>
		ExceptionReceiver ExceptionReceiver { get; set;}
	}
}