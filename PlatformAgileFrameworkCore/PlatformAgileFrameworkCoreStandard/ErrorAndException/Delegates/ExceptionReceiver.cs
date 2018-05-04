#region Using Directives
using System;
#endregion // Using Directives
namespace PlatformAgileFramework.ErrorAndException.Delegates
{
	/// <summary>
	/// This delegate is used to pass an exception back from executing code. It is
	/// often used within test frameworks to provide a pluggable method to receive
	/// exceptions generated within SUT's. However, it is completely general.
	/// </summary>
	/// <param name="exception">
	/// The exception that was caught or generated.
	/// </param>
	public delegate void ExceptionReceiver(Exception exception);
}
