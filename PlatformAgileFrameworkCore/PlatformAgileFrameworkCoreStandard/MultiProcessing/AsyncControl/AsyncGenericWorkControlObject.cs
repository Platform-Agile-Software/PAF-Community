//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2019 Icucom Corporation
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
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System;
using System.Collections.Generic;
using PlatformAgileFramework.Annotations;
using PlatformAgileFramework.Collections.ExtensionMethods;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.TypeHandling.Disposal;

// Exception shorthand.
// ReSharper disable once IdentifierTypo
using PAFAED = PlatformAgileFramework.ErrorAndException.PAFAggregateExceptionData;

namespace PlatformAgileFramework.MultiProcessing.AsyncControl
{
	/// <summary>
	/// This class provides simple control functionality for multi-threaded operations.
	/// Default implementation of <see cref="IAsyncWorkControlObject{T}"/>.
	/// Basically holds a Generic payload that is usually shared for concurrency testing.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 31mar2019 </date>
	/// <description>
	/// Built for concurrency testing in the .Net standard world.
	/// </description>
	/// </contribution>
	/// </history>
	public class AsyncGenericWorkControlObject<T> : AsyncWorkControlObject, IAsyncWorkControlObject<T>
	where T: class
	{
		/// <summary>
		/// Backing for the prop. - internal for testing ONLY.
		/// </summary>
		internal IAsyncGenericWorkPayloadObject<T> m_GenericWorkPayloadObject;
		#region Constructors
		/// <summary>
		/// Main constructor that supplies a <see cref="Guid"/> and loads
		/// the payload and method.
		/// </summary>
		/// <param name="genericWorkPayloadObject">. Can be <see langword="null"/>
		/// for chicken and egg scenarios where this needs to be loaded after construction.
		/// Loads <see cref="GenericWorkPayloadObject"/>.
		/// </param>
		/// <param name="guid">
		/// See base class.
		/// </param>
		/// <threadsafety>
		///	NOT thread-safe. One of the uses of this class is to allow multiple threads access
		/// to a SHARED payload. If we have exclusive access in this class on a single thread,
		/// the class is thread-safe. 
		/// </threadsafety>
		public AsyncGenericWorkControlObject(
			IAsyncGenericWorkPayloadObject<T> genericWorkPayloadObject = null,
			 Guid guid = default(Guid))
		:base(genericWorkPayloadObject, guid)
		{
			m_GenericWorkPayloadObject = genericWorkPayloadObject;
		}
		#endregion // Constructors
		#region Properties
		#region IAsyncWorkControlObject{T} Implementation
		/// <summary>
		/// See <see cref="IAsyncWorkControlObject{T}"/>.
		/// </summary>
		public virtual IAsyncGenericWorkPayloadObject<T> GenericWorkPayloadObject
		{
			get => m_GenericWorkPayloadObject;
			set
			{
				m_GenericWorkPayloadObject = value;
			}
		}
		#endregion // IAsyncWorkControlObject{T} Implementation
		#endregion // Properties
		#region Methods
		protected override Exception AsyncControlObjectDispose(bool disposing, object obj)
		{
			var baseException = base.AsyncControlObjectDispose(disposing, obj);

			if (GenericWorkPayloadObject == null)
				return baseException;

			if (!(GenericWorkPayloadObject.GenericPayload is IDisposable disposable))
				return baseException;

			var eList = new List<Exception>();
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref disposable, true));

			// If we have any exceptions, put them in an aggregator.
			if (eList.Count > 0)
			{
				var exceptions = new PAFAED(eList);
				var ex = new PAFStandardException<PAFAED>(exceptions);
				// Seal the list.
				exceptions.AddException(null);
				// We just put these in the registry.
				DisposalRegistry.RecordDisposalException(GetType(), ex);
				return ex;
			}
			return null;
		}
		#endregion // Methods
	}
}
