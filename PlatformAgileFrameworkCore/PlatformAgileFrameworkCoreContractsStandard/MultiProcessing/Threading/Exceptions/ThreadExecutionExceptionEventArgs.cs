using System;

namespace PlatformAgileFramework.MultiProcessing.Threading.Exceptions
{
	/// <summary>
	/// Event arguments that are returned from thread processing loops.
	/// </summary>
	public class ProcessExecutionExceptionEventArgs : EventArgs
	{
		/// <summary>
		/// Gets the wrapped thread Exception.
		/// </summary>
		public Exception ProcessException { get; protected set;}
		/// <summary>
		/// Builds from an internal thread exception that was thrown inside the
		/// thread and is designed to be returned to the calling thread. Execeptions
		/// are also used to communicate information.
		/// </summary>
		/// <param name="exception">The exception.</param>
		public ProcessExecutionExceptionEventArgs(Exception exception)
		{
			ProcessException = exception;
		}
	}
}
