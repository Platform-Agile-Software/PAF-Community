//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2016 Icucom Corporation
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Logging;

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.MultiProcessing.Threading.Exceptions
{
	/// <summary>
	/// Sealed implementation of <see cref="IPAFThreadMismatchExceptionData"/>.
	/// </summary>
	// KRM DO NOT add serializable attribute until surrogate is built.
	// [PAFSerializable(PAFSerializationType.PAFSurrogate)]
	public sealed class PAFThreadMismatchExceptionData: PAFThreadMismatchExceptionDataBase
	{
		#region Constructors
		/// <summary>
		/// Constructor builds with the standard arguments plus the
		/// <see cref="IPAFThreadMismatchExceptionData.ConflictingThreadID"/>
		/// and <see cref="IPAFThreadMismatchExceptionData.ConflictingThreadName"/>.
		/// and <see cref="IPAFThreadExecutionExceptionData.ExecutingThreadName"/>.
		/// and <see cref="IPAFThreadExecutionExceptionData.ExecutingThreadName"/>.
		/// </summary>
		/// <param name="conflictingThreadID">
		/// <see cref="IPAFThreadMismatchExceptionData"/>.
		/// </param>
		/// <param name="conflictingThreadName">
		/// <see cref="IPAFThreadMismatchExceptionData"/>.
		/// </param>
		/// <param name="executingThreadID">
		/// <see cref="IPAFThreadExecutionExceptionData"/>.
		/// </param>
		/// <param name="executingThreadName">
		/// <see cref="IPAFThreadExecutionExceptionData"/>.
		/// </param>
		/// <param name="extensionData">
		/// <see cref="IPAFStandardExceptionData"/>.
		/// </param>
		/// <param name="pafLoggingLevel">
		/// <see cref="IPAFStandardExceptionData"/>.
		/// </param>
		/// <param name="isFatal">
		/// <see cref="IPAFStandardExceptionData"/>.
		/// </param>
		public PAFThreadMismatchExceptionData(int conflictingThreadID = -1,
			string conflictingThreadName = null, int executingThreadID = -1,
			string executingThreadName = null, PAFLoggingLevel? pafLoggingLevel = null,
			object extensionData = null, bool? isFatal = null)
			: base(conflictingThreadID, conflictingThreadName ,executingThreadID, executingThreadName , pafLoggingLevel,
			extensionData, isFatal)
		{
			m_ConflictingThreadID = conflictingThreadID;
			m_ConflictingThreadName = conflictingThreadName;
		}
		#endregion Constructors
	}
}