//@#$&+
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

#region Using Directives

using System;
using System.Collections.Generic;
using System.Security;
using PlatformAgileFramework.Collections.Enumerators;
using PlatformAgileFramework.MultiProcessing.Threading.Locks;
using PlatformAgileFramework.Serializing;
using PlatformAgileFramework.Serializing.Attributes;
using PlatformAgileFramework.Serializing.ECMAReplacements;
using PlatformAgileFramework.Serializing.HelperCollections;

#endregion // Using Directives

namespace PlatformAgileFramework.ErrorAndException
{
	/// <summary>
	/// <para>
	///	The base non-Generic exception from which all of our custom exceptions derive. The exception
	/// is designed to be used in both low and elevated trust environments with serialization
	/// performed by the PAFSerializer.
	/// </para>
	/// </summary>
	/// <remarks>
	/// Partial class for extension in CLR and extended library.
	/// </remarks>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 07jul2013 </date>
	/// <description>
	/// Replaced R/W locks, since we don't want disposable exceptions and
	/// don't need the speed.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> BMC </author>
	/// <date> 02jul2011 </date>
	/// <description>
	/// Recast as a partial class for extensibility in ECMA lib.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Not thread-safe, since <see cref="Exception"/> is not. However, the
	/// members we have added are thread-safe. See their comments. We use monitor
	/// locks here, since we don't want the baggage of anything more sophisticated.
	/// The traffic on the class is not expected to be severe.
	/// </threadsafety>
	/// <remarks>
	/// The surrogate for this class will serialize the needed fields in the
	/// exception class, as well. We don't provide a separate surrogate for
	/// the <see cref="Exception"/> class. If you put anything in the dictionary
	/// on the <see cref="Exception"/> class that is NOT serializable, it just gets
	/// it's <see cref="object.ToString"/> sent unless you have registered your
	/// own surrogate for the type.
	/// </remarks>
	[PAFSerializable(PAFSerializationType.PAFSurrogate)]
// Silverlight.
// ReSharper disable PartialTypeWithSinglePart
	public abstract partial class PAFAbstractExceptionBase : Exception,
		IPAFExceptionBase
// ReSharper restore PartialTypeWithSinglePart
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Monitor object which locks the object store creation process. Not enough
		/// traffic here for a R/W lock.
		/// </summary>
		private object m_ObjectStoreCreationLockObject;
		/// <summary>
		/// Backing for the object store. internal for the surrogate.
		/// Uses a <see cref="MonitorReaderWriterLock"/>, since we don't
		/// want disposable exceptions and this is a low-traffic class.
		/// </summary>
		protected internal IStringKeyedSerializableObjectStore m_ObjectStore;
		/// <summary>
		/// Serialization indexer for the object store.
		/// </summary>
		protected internal const string OBJECT_STORE_TAG = "OBJECT_STORE_TAG";
		/// <summary>
		/// Backing for the stack traces. internal for the surrogate. This will be
		/// an empty list if no stacktraces have been pushed into it - never
		/// <see langwoprd="null"/>
		/// </summary>
		protected internal List<string> m_StackTraces;
		/// <summary>
		/// Serialization indexer for the stack trace.
		/// </summary>
		protected internal const string STACK_TRACES_TAG = "STACK_TRACES_TAG";
		/// <summary>
		/// Backing for <see cref="SetMessage"/>.
		/// </summary>
		internal string m_SetMessage;
		/// <summary>
		/// Serialization indexer for the message.
		/// </summary>
		protected internal const string SET_MESSAGE_TAG = "SET_MESSAGE_TAG";
		/// <summary>
		/// Serialization indexer for the inner exception.
		/// </summary>
		protected internal const string INNER_EXCEPTION_TAG = "INNER_EXCEPTION_TAG";
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="PAFAbstractExceptionBase"/> class.
		/// </summary>
		/// <param name="message"><see cref="Exception"/></param>
		/// <param name="innerException"><see cref="Exception"/></param>
		/// <remarks>
		/// Constructor also sets our versions of the message and inner exception.
		/// </remarks>
		protected PAFAbstractExceptionBase(string message = null, 
			Exception innerException = null)
			: base(message, innerException)
		{
			Initialize_Protected();
			m_SetMessage = message;
		}
		/// <summary>
		/// Protected for subclasses. Security critical for elevated priviledge environments.
		/// Calls internal constructor. This is the standard constructor that is used in the
		/// legacy .Net serializer.
		/// </summary>
		/// <param name="info">See <see cref="IPAFSerializationInfoCLS"/>.
		///</param>
		/// <param name="context">
		/// Not used in this method.
		/// </param>
		[SecurityCritical]
		protected internal PAFAbstractExceptionBase(IPAFSerializationInfoCLS info,
			PAFSerializationContext context)
			: this(context, info) { }
		/// <summary>
		/// Internal for the surrogate. Builds basic exception.
		/// </summary>
		/// <param name="context">
		/// Not used in this method.
		/// </param>
		/// <param name="info">See <see cref="IPAFSerializationInfoCLS"/>.
		///</param>
// ReSharper disable UnusedParameter.Local
		internal PAFAbstractExceptionBase(PAFSerializationContext context,
			IPAFSerializationInfoCLS info)
// ReSharper restore UnusedParameter.Local
			:base(info.GetValueNoThrow<string>(SET_MESSAGE_TAG),
				info.GetValueNoThrow<Exception>(INNER_EXCEPTION_TAG))
		{
			Initialize_Protected();
			var traces = info.GetValueNoThrow<string[]>(STACK_TRACES_TAG);
			if ((traces != null) && (traces.Length > 0))
			{
				lock (m_StackTraces) {
					m_StackTraces.AddRange(traces);
				}
			}
			var storePairs = info.GetValueNoThrow<KeyValuePair<string, object>[]>(OBJECT_STORE_TAG);
			if ((storePairs != null) && (storePairs.Length > 0)) {
				var store = GetSerializableObjectStore();
				foreach (var pair in storePairs) {
					store.Add(pair);
				}
			}
		}
		#endregion // Constructors
		#region Construction Helpers
		/// <summary>
		/// This is a common initialization method that is present to
		/// factor out initialization code when a constructor is not
		/// necessarily called. This allows future flexibility in
		/// changing serialization methods.
		/// </summary>
		protected internal void Initialize_Protected()
		{
			m_ObjectStoreCreationLockObject = new object();
			// ReSharper disable once InconsistentlySynchronizedField
			m_StackTraces = new List<string>();
		}
		#endregion // Construction Helpers
		#region Novel Members
		#region Properties
		/// <summary>
		/// Provides access to our version of the message.
		/// </summary>
		// TODO - KRM - How come we don't want to override Message?
		protected internal virtual string SetMessage
		{
			get { return m_SetMessage; }
			[SecurityCritical] set { m_SetMessage = value; }
		}
		#endregion // Properties

		#region Members
		/// <summary>
		/// This clears out the stack traces - internal version.
		/// </summary>
		/// <threadsafety>
		/// Thread-safe.
		/// </threadsafety>
		protected internal virtual void ClearStackTracesInternal()
		{
			if (m_StackTraces.Count == 0) return;
			lock (m_StackTraces)
			{
				m_StackTraces.Clear();
			}
		}
		#endregion // Members
		#endregion // Novel Members
		#region IPAFExceptionBase Implementation
		#region Properties
		/// <summary>
		/// See <see cref="IPAFExceptionBase"/>
		/// </summary>
		public override string Message
		{
			get
			{
				return SetMessage;
			}
		}
		#endregion // Properties
		#region Methods
		/// <summary>
		/// See <see cref="IPAFExceptionBase"/>
		/// </summary>
		/// <threadsafety>
		/// Thread-safe.
		/// </threadsafety>
		[SecurityCritical]
		public virtual void ClearStackTraces()
		{
			ClearStackTracesInternal();
		}

		/// <summary>
		/// See <see cref="IPAFExceptionBase"/>
		/// </summary>
		/// <returns>
		/// See <see cref="IPAFExceptionBase"/>
		/// </returns>
		public Exception GetInnerException()
		{
			return InnerException;
		}
		/// <summary>
		/// See <see cref="IPAFExceptionBase"/>
		/// </summary>
		/// <returns>
		/// See <see cref="IPAFExceptionBase"/>
		/// </returns>
		public IStringKeyedSerializableObjectStore GetSerializableObjectStore()
		{
			lock (m_ObjectStoreCreationLockObject) {
				if (m_ObjectStore != null) return m_ObjectStore;
				m_ObjectStore
					= new StringKeyedSerializableObjectStore(new MonitorReaderWriterLock());
				return m_ObjectStore;
			}
		}
		/// <summary>
		/// See <see cref="IPAFExceptionBase"/>
		/// </summary>
		/// <returns>
		/// See <see cref="IPAFExceptionBase"/>
		/// </returns>
		/// <threadsafety>
		/// Thread-safe.
		/// </threadsafety>
		public virtual IEnumerator<string> GetStackTraces()
		{
			if (m_StackTraces.Count == 0) return null;
			lock (m_StackTraces)
			{
				return new ShallowCopyEnumerator<string>(m_StackTraces);
			}
		}
		/// <summary>
		/// See <see cref="IPAFExceptionBase"/>
		/// </summary>
		/// <param name="exceptionTag">
		/// See <see cref="IPAFExceptionBase"/>
		/// </param>
		/// <returns>
		/// See <see cref="IPAFExceptionBase"/>
		/// </returns>
		public virtual bool HasTag(string exceptionTag)
		{
			if (string.IsNullOrEmpty(exceptionTag)) return false;
			if (string.IsNullOrEmpty(Message)) return false;
			return Message.Contains(exceptionTag);
		}
		/// <summary>
		/// See <see cref="IPAFExceptionBase"/>
		/// </summary>
		/// <threadsafety>
		/// Thread-safe.
		/// </threadsafety>
		public virtual IPAFExceptionBase PushedException()
		{
			PushStackTrace();
			return this;
		}
		/// <summary>
		/// See <see cref="IPAFExceptionBase"/>
		/// </summary>
		/// <threadsafety>
		/// Thread-safe.
		/// </threadsafety>
		public virtual void PushStackTrace()
		{
			lock (m_StackTraces)
			{
				m_StackTraces.Add(StackTrace);
			}
		}
		/// <summary>
		/// See <see cref="IPAFExceptionBase"/>
		/// </summary>
		/// <threadsafety>
		/// Thread-safe.
		/// </threadsafety>
		/// <remarks>
		/// Subclasses can override this class to delete unwanted entries from
		/// dictionaries, etc..
		/// </remarks>
		public virtual IPAFExceptionBase SanitizedException()
		{
			ClearStackTraces();
			return this;
		}
		#endregion // IPAFExceptionBase Implementation
		#region Obligatory Patch for Equals and Hash Code
		/// <summary>
		/// Determines whether the specified <see cref="object"/> is equal to the
		/// current <see cref="object"/>.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if the specified <see cref="object"/> is equal to the current
		/// <see cref="object"/>; otherwise, false.
		/// </returns>
		/// <param name="obj">
		/// The <see cref="object"/> to compare with the current <see cref="object"/>.
		/// </param>
		/// <remarks>
		/// Patch for Microsoft's mistake.
		/// </remarks>
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			// ReSharper disable BaseObjectEqualsIsObjectEquals
			return GetType() == obj.GetType() && base.Equals(obj);
			// ReSharper restore BaseObjectEqualsIsObjectEquals
		}
		/// <summary>
		/// We are a reference type so just call base to shut up the compiler/tools.
		/// </summary>
		/// <returns>
		/// The original hash code.
		/// </returns>
		public override int GetHashCode()
		{
			// ReSharper disable BaseObjectGetHashCodeCallInGetHashCode
			return base.GetHashCode();
			// ReSharper restore BaseObjectGetHashCodeCallInGetHashCode
		}

        /// <summary>
        /// Gets the exception data. Abstract, since we only implement
        /// Generic payloads, but want to read them as a non-Generic.
        /// </summary>
        /// <returns>The non-Generic exception data.</returns>
        public abstract object GetExceptionData();
        #endregion // Obligatory Patch for Equals and Hash Code
        #endregion // Methods
    }
}