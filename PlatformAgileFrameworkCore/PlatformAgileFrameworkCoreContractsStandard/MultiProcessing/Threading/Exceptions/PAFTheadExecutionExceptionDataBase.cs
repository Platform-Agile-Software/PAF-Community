using System;
using System.Collections.Generic;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.MultiProcessing.Threading.Exceptions
{
	/// <summary>
	///	See <see cref="IPAFThreadExecutionExceptionData"/>.
	/// </summary>
	[PAFSerializable]
	public abstract class PAFThreadExecutionExceptionDataBase :
		PAFAbstractStandardExceptionDataBase , IPAFThreadExecutionExceptionData
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Backing field.
		/// </summary>
		internal string m_ExecutingThreadName;
		/// <summary>
		/// Backing field.
		/// </summary>
		internal int m_ExecutingThreadID;
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Constructor builds with the standard arguments plus the
		/// <see cref="IPAFThreadExecutionExceptionData.ExecutingThreadID"/>
		/// and <see cref="IPAFThreadExecutionExceptionData.ExecutingThreadName"/>.
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
		/// <param name="pafLoggingLevel">
		/// <see cref="IPAFStandardExceptionData"/>.
		/// </param>
		/// <param name="isFatal">
		/// <see cref="IPAFStandardExceptionData"/>.
		/// </param>
		protected PAFThreadExecutionExceptionDataBase(int executingThreadID = -1,
			string executingThreadName = null, PAFLoggingLevel? pafLoggingLevel = null,
			object extensionData = null, bool? isFatal = null)
			: base(extensionData, pafLoggingLevel, isFatal)
		{
			m_ExecutingThreadID = executingThreadID;
			m_ExecutingThreadName = executingThreadName;
		}
		#endregion Constructors
		#region Properties
		/// <summary>
		/// See <see cref="IPAFThreadExecutionExceptionData"/>.
		/// </summary>
		public string ExecutingThreadName
		{
			get { return m_ExecutingThreadName; }
			internal set { m_ExecutingThreadName = value; }
		}
		/// <summary>
		/// See <see cref="IPAFThreadExecutionExceptionData"/>.
		/// </summary>
		public int ExecutingThreadID
		{
			get { return m_ExecutingThreadID; }
			internal set { m_ExecutingThreadID = value; }
		}
		#endregion // Properties
	}
}