using System;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Logging;

namespace PlatformAgileFramework.MultiProcessing.Threading.Exceptions
{
	/// <summary>
	///	Exceptions that occur during thread execution.
	/// Sealed version of <see cref="PAFThreadExecutionExceptionDataBase"/>
	/// </summary>
	// TODO KRM
	// KRM DO NOT add serializable attribute until surrogate is built.
	// [PAFSerializable(PAFSerializationType.PAFSurrogate)]
	public sealed class PAFThreadExecutionExceptionData
		: PAFThreadExecutionExceptionDataBase
	{
		#region Constructors
		/// <summary>
		/// Public constructor for
		/// <see cref="PAFThreadExecutionExceptionDataBase"/>.
		/// </summary>
		/// <param name="executingThreadID">
		/// <see cref="IPAFThreadExecutionExceptionData"/>.
		/// </param>
		/// <param name="executingThreadName">
		/// <see cref="IPAFThreadExecutionExceptionData"/>.
		/// </param>
		/// <param name="extensionData">
		/// <see cref="IPAFStandardExceptionData"/>.
		/// </param>
		/// <param name="isFatal">
		/// <see cref="IPAFStandardExceptionData"/>.
		/// </param>
		/// <param name="pafLoggingLevel">
		/// <see cref="IPAFStandardExceptionData"/>.
		/// </param>
		public PAFThreadExecutionExceptionData(int executingThreadID = -1,
			string executingThreadName = null,  PAFLoggingLevel? pafLoggingLevel = null,
			object extensionData = null, bool? isFatal = null)
			: base(executingThreadID, executingThreadName, pafLoggingLevel, extensionData, isFatal)
		{
		}
		#endregion Constructors
	}
}