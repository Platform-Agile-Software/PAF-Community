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
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using PlatformAgileFramework.TypeHandling;
using PlatformAgileFramework.MultiProcessing.Threading.Locks;
using PlatformAgileFramework.Serializing.Attributes;
using PlatformAgileFramework.Serializing.Interfaces;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;

namespace PlatformAgileFramework.Serializing.HelperCollections
{
	/// <summary>
	/// This class implements <see cref="IStringKeyedSerializableObjectStore"/>.
	/// It provides an internal default serializability check using
	/// <see cref="TypeExtensions.IsTypeSerializable"/> if no custom checker is
	/// loaded.
	/// </summary>
	/// <history>
	/// <author> DAP </author>
	/// <date> 21jun2012 </date>
	/// <contribution>
	/// Added a customizable serializability checker. Cleaned up DOCs.
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Thread-safe.
	/// </threadsafety>
	/// <serialization>
	/// By surrogate.
	/// </serialization>
	[PAFSerializable]
	public class StringKeyedSerializableObjectStoreBase : IStringKeyedSerializableObjectStore
	{
		#region Fields and Autoproperties

		/// <summary>
		/// See <see cref="IStringKeyedSerializableObjectStore"/>
		/// </summary>
		protected internal SerializabilityChecker m_ObjectIsSerializable;
		/// <summary>
		/// For synchronized access.
		/// </summary>
		protected internal IReaderWriterLock m_IReaderWriterLock;
		/// <summary>
		/// Wrapped inner dictionary that we use for storage.
		/// </summary>
		protected internal Dictionary<string, object> m_Objects;
		#endregion // Fields And Autoproperties
		#region Constructors
		/// <summary>
		/// Default constructor builds with <see cref="MonitorReaderWriterLock"/> lock.
		/// </summary>
		public StringKeyedSerializableObjectStoreBase()
		{
			Initialize_StringKeyedSerializableObjectStoreBaseInternal();
			Finalize_StringKeyedSerializableObjectStoreBaseInternal();
		}
		/// <summary>
		/// Constructor builds with the <see cref="IReaderWriterLock"/> lock of the client's choice.
		/// </summary>
		/// <param name="iReaderWriterLock">
		/// The lock. If <see langword="null"/>, <see cref="MonitorReaderWriterLock"/> is used.
		/// </param>
		/// <param name="objectIsSerializable">
		/// Allows a custom serizability check to be installed. If <see langword="null"/>,
		/// <see cref="TypeExtensions.IsTypeSerializable"/> is used.
		/// </param>
		public StringKeyedSerializableObjectStoreBase(IReaderWriterLock iReaderWriterLock, 
			SerializabilityChecker objectIsSerializable = null)
		{
			Initialize_StringKeyedSerializableObjectStoreBaseInternal();
			m_IReaderWriterLock = iReaderWriterLock;
			m_ObjectIsSerializable = objectIsSerializable;
			Finalize_StringKeyedSerializableObjectStoreBaseInternal();
		}
		#endregion // Constructors
		#region Properties
		/// <summary>
		/// See <see cref="IStringKeyedSerializableObjectStore"/>.
		/// </summary>
		public virtual string[] Keys
		{
			get
			{
				m_IReaderWriterLock.EnterReadLock();
				var names = new string[m_Objects.Keys.Count];
				this.m_Objects.Keys.CopyTo(names, 0);
				m_IReaderWriterLock.ExitReadLock();
				return names;
			}
		}
		/// <summary>
		/// See <see cref="IStringKeyedSerializableObjectStore"/>
		/// </summary>
		public virtual SerializabilityChecker ObjectIsSerializable
		{ get { return m_ObjectIsSerializable; } }
		/// <summary>
		/// Handle dictionary by its interface.
		/// </summary>
		protected internal virtual IDictionary<string, object> ObjectStore
		{
			get { return this.m_Objects; }
		}
		/// <summary>
		/// For synchronized access.
		/// </summary>
		protected internal virtual IReaderWriterLock ReaderWriterLock
		{
			get { return m_IReaderWriterLock; }
		}
		#endregion Properties
		#region Methods
		/// <summary>
		/// This method is the backing method for the explicit interface method.
		/// </summary>
		/// <returns>Enumerator for the object store.</returns>
		/// <threadsafety>
		/// This method locks the dictionary before making a REFERENCE copy of
		/// the key-value pairs. It is thus thread-safe if members of the Object are not
		/// modified. If a deep copy is needed, override this method.
		/// </threadsafety>
		protected internal virtual IEnumerator<KeyValuePair<string, object>> GetEnumerator()
		{
			m_IReaderWriterLock.EnterReadLock();
			try {
				var coll = new List<KeyValuePair<string, object>>(ObjectStore);
				return coll.GetEnumerator();
			}
			finally
			{
				m_IReaderWriterLock.ExitReadLock();
			}
		}
		#endregion // Methods
		#region Serialization Helpers
		/// <summary>
		/// For universal serializer support.
		/// </summary>
		[SecurityCritical]
		public void Initialize_StringKeyedSerializableObjectStoreBase()
		{
			Initialize_StringKeyedSerializableObjectStoreBaseInternal();
		}
		/// <summary>
		/// For IPAFSerializable support.
		/// </summary>
		protected internal void Initialize_StringKeyedSerializableObjectStoreBaseInternal()
		{
			m_Objects = new Dictionary<string, object>();
		}
		/// <summary>
		/// For universal serializer support.
		/// </summary>
		[SecurityCritical]
		public void Finalize_StringKeyedSerializableObjectStoreBase()
		{
			Finalize_StringKeyedSerializableObjectStoreBaseInternal();		}
		/// <summary>
		/// For IPAFSerializable support.
		/// </summary>
		protected internal void Finalize_StringKeyedSerializableObjectStoreBaseInternal()
		{
			m_IReaderWriterLock = m_IReaderWriterLock ?? new MonitorReaderWriterLock();
			m_ObjectIsSerializable = ObjectIsSerializable ?? TypeExtensions.IsTypeSerializable;
		}
		#endregion // Serialization Helpers
		#region ISerializableObjectStore Implementation
		/// <summary>
		/// See <see cref="IStringKeyedSerializableObjectStore"/>.
		/// </summary>
		/// <param name="objectName">
		/// See <see cref="IStringKeyedSerializableObjectStore"/>.
		/// </param>
		/// <param name="obj">
		/// See <see cref="IStringKeyedSerializableObjectStore"/>.
		/// </param>
		/// <exceptions>
		/// See <see cref="IStringKeyedSerializableObjectStore"/>.
		/// </exceptions>
		public virtual bool Add(string objectName, object obj)
		{
			// TODO KRM finish exceptions - null params
			if (!ObjectIsSerializable(obj.GetType()))
				throw new ArgumentException("Object not serializable");

			m_IReaderWriterLock.EnterWriteLock();
			try {
				if (ObjectStore.Keys.Contains(objectName))
					return false;

				ObjectStore.Add(objectName, obj);
				return true;
			}
			finally
			{
				m_IReaderWriterLock.ExitWriteLock();
			}
		}
		/// <summary>
		/// See <see cref="IStringKeyedSerializableObjectStore"/>.
		/// </summary>
		/// <param name="key">
		/// See <see cref="IStringKeyedSerializableObjectStore"/>.
		/// </param>
		/// <exceptions>
		/// See <see cref="IStringKeyedSerializableObjectStore"/>.
		/// </exceptions>
		public virtual object GetItem(string key)
		{
			object obj;
			TryGetItem(key, out obj);
			return obj;
		}
		/// <summary>
		/// See <see cref="IStringKeyedSerializableObjectStore"/>.
		/// </summary>
		/// <param name="key">
		/// See <see cref="IStringKeyedSerializableObjectStore"/>.
		/// </param>
		/// <param name="item">
		/// See <see cref="IStringKeyedSerializableObjectStore"/>.
		/// </param>
		/// <exceptions>
		/// See <see cref="IStringKeyedSerializableObjectStore"/>.
		/// </exceptions>
		public virtual bool TryGetItem(string key, out object item)
		{
			return this.ObjectStore.TryGetValue(key, out item);
		}
		#endregion // ISerializableObjectStore Implementation
		#region IPAFSerializableCollection<KeyValuePair> Implementation
		/// <summary>
		/// See <see cref="IPAFSerializableCollection{KeyValuePair}"/>. This method does
		/// not check for serializability.
		/// </summary>
		/// <param name="item">
		/// See <see cref="IPAFSerializableCollection{KeyValuePair}"/>
		/// </param>
		/// <threadsafety>
		/// This method is not thread-safe. It is intended to be used by serializers.
		/// Application code should use the other add method.
		/// </threadsafety>
		void IPAFSerializableCollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
		{
			ObjectStore.Add(item);
		}
		/// <summary>
		/// See <see cref="IPAFSerializableCollection{KeyValuePair}"/>.
		/// Backed by <see cref="GetEnumerator"/>.
		/// </summary>
		IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
		{
			return GetEnumerator();
		}
		/// <summary>
		/// See <see cref="IPAFSerializableCollection{KeyValuePair}"/>.
		/// Delegates to <see cref="IEnumerable{KeyValuePair}.GetEnumerator()"/>.
		/// </summary>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<KeyValuePair<string, object>>)this).GetEnumerator();
		}
		#endregion // IPAFSerializableCollection<KeyValuePair> Implementation
	}
}
