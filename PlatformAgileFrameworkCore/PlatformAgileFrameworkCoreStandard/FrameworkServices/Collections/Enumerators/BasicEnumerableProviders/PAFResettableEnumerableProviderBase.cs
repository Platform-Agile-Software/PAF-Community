//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 Icucom Corporation
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

using System;
using System.Collections.Generic;
using System.Threading;
using PlatformAgileFramework.MultiProcessing.Tasking;
using PlatformAgileFramework.MultiProcessing.Threading;
using PlatformAgileFramework.TypeHandling;
using PlatformAgileFramework.TypeHandling.Disposal;

namespace PlatformAgileFramework.Collections.Enumerators.BasicEnumerableProviders
{
	/// <summary>
	/// <para>
	/// Implements <see cref="IPAFResettableEnumerableProvider{T}"/> by
	/// wrapping an ordinary <see cref="IEnumerable{T}"/>.
	/// </para>
	/// <para>
	/// This class employs an internal nested enumerator factory class that
	/// is actually reconstructed each time we are called upon to hand out
	/// an enumerator. This class was designed to deal with infinite enumerators
	/// and enumerators we wish to stop prior to the termination of
	/// foreach loops they may be used in. The two issues are separate,
	/// but sometimes related.
	/// </para>
	/// </summary>
	/// <typeparam name="T">Type that is to be enumerated.</typeparam>
	/// <threadsafety>
	/// This class is thread-safe. The operation of the class is thread-safe
	/// if <typeparamref name="T"/>s are either synchronized or not accessed
	/// by multiple threads.
	/// </threadsafety>
	public class PAFResettableEnumerableProviderBase<T>: IPAFResettableEnumerableProvider<T>
	{
		#region Nested Factory Class
		/// <summary>
		/// Just subclasses <see cref=" PAFEnumeratorWrapperBase{U}"/>
		/// </summary>
		/// <typeparam name="U">
		/// Type of the item to be enumerated.
		/// </typeparam>
		protected class EnumeratorFactoryClass<U> : PAFEnumeratorWrapperBase<U>
		{
			#region Constructors
			/// <summary>
			/// Just calls base.
			/// </summary>
			/// <param name="enumerable">
			/// See base.
			/// </param>
			/// <param name="disposeEnumerable">
			/// See base.
			/// </param>
			/// <param name="cloner">
			/// See base.
			/// </param>
			public EnumeratorFactoryClass(IEnumerable<U> enumerable,
				bool disposeEnumerable = false, TypeHandlingUtils.TypeCloner<U> cloner = null)
				:base(enumerable, disposeEnumerable, cloner){}
			#endregion // Constructors
		}
		#endregion // Nested Factory Class
		#region Class Fields
		/// <summary>
		/// Atomically set variable for disposal state.
		/// </summary>
		private int m_1ForDisposed;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal bool m_HaltCurrentEnumeratorOnReset;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal bool? m_IsEnumerationFinite;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal IEnumerable<T> m_InnerEnumerable;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal IEnumerable<T> m_EnumeratorFactory;
		/// <summary>
		/// Lock for reset.
		/// </summary>
		private readonly object m_ResetLock = new object();
		/// <summary>
		/// Pluggable copyer.
		/// </summary>
		protected internal TypeHandlingUtils.TypeCloner<T> m_TypeCloner;
		#endregion // Class Fields
		#region Constructors
		/// <summary>
		/// For inheritors.
		/// </summary>
		protected PAFResettableEnumerableProviderBase()
		{
			m_InnerEnumerable = new List<T>();
		}
		/// <summary>
		/// Constructor sets <see cref="InnerEnumerable"/> and the cloner.
		/// </summary>
		/// <param name="enumerable">
		/// The incoming enumerable to build with. This may be <see langword="null"/>,
		/// in which case <see cref="SetData"/> must be called before the
		/// instance is usable.
		/// </param>
		/// <param name="haltCurrentEnumeratorOnReset">
		/// If <see langword="true"/>, the current enumerator, which may be held
		/// by a client's executing foreach loop is set to return
		/// <see langword="false"/> in its <see cref="IEnumerator{T}.MoveNext"/>
		/// method when the <see cref="SetData"/> method is called
		/// on this class. If this parameter is <see langword="false"/>, the
		/// enumerator that is held by the client is allowed to run its
		/// course.
		/// </param>
		/// <param name="cloner">
		/// This is the cloner that makes clones of items on the way out. If
		/// <see langword="null"/>, items are just directly handed out. This is the
		/// cloner that is used within the internal enumerator factory.
		/// </param>
		protected PAFResettableEnumerableProviderBase(IEnumerable<T> enumerable = null,
			bool haltCurrentEnumeratorOnReset = true, TypeHandlingUtils.TypeCloner<T> cloner = null)
			:this()
		{
			if(enumerable != null)
				m_InnerEnumerable = enumerable;
			m_HaltCurrentEnumeratorOnReset = haltCurrentEnumeratorOnReset;
			m_TypeCloner = cloner;
		}
		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="oldEnumerable">The original instance.</param>
		protected PAFResettableEnumerableProviderBase(PAFResettableEnumerableProviderBase<T> oldEnumerable)
		{
			m_InnerEnumerable = oldEnumerable.InnerEnumerable;
			m_HaltCurrentEnumeratorOnReset = oldEnumerable.m_HaltCurrentEnumeratorOnReset;
			m_TypeCloner = oldEnumerable.m_TypeCloner;
		}
		#endregion // Constructors
		#region Properties
		/// <summary>
		/// In this simple base implementation, this enumerable is used to
		/// build a contained enumerator/enumerable each time <see cref="GetEnumerable"/>
		/// is called. This property can be refreshed from time-to-time
		/// by calling the <see cref="SetData"/> method.
		/// </summary>
		protected IEnumerable<T> InnerEnumerable
		{
			get
			{
				PAFDisposalUtils.DisposalGuard("EnumerableProvider", m_1ForDisposed);
				lock (m_ResetLock) {
					return m_InnerEnumerable;
				}
			}
			set
			{
				PAFDisposalUtils.DisposalGuard("EnumerableProvider", m_1ForDisposed);
				lock (m_ResetLock) {
					m_InnerEnumerable = value;
				}
			}
		}
		/// <summary>
		/// This is the inner enumerable that we hold that acts as a factory
		/// for <see cref="IEnumerator{T}"/>'s.
		/// </summary>
		protected IEnumerable<T> EnumeratorFactory
		{
			get
			{
				PAFDisposalUtils.DisposalGuard("EnumerableProvider", m_1ForDisposed);
				lock (m_ResetLock)
				{
					return m_EnumeratorFactory;
				}
			}
			set
			{
				PAFDisposalUtils.DisposalGuard("EnumerableProvider", m_1ForDisposed);
				lock (m_ResetLock)
				{
					m_EnumeratorFactory = value;
				}
			}
		}
		#endregion // Properties
		#region Methods
		/// <summary>
		/// <see cref="IPAFResettableEnumerableProvider{T}"/>.
		/// Just examines the <see cref="InnerEnumerable"/> to see
		/// if it's finite.
		/// </summary>
		public virtual bool? IsEnumerationFinite
		{
			get
			{
				return InnerEnumerable.CheckEnumerationIsFinite();
			}
		}
		/// <summary>
		/// First calls <see cref="DisposeResettableProvider"/>, then
		/// disposes the internal <see cref="InnerEnumerable"/> if it is not
		/// <see langword="null"/> and it is disposable. This is the method that sets the
		/// <see cref="m_1ForDisposed"/> flag. This method does not execute if
		/// the flag is already set. Calling this method renders the class
		/// unusable. The ordinary <see cref="Dispose"/> is not used for this
		/// purpose on this class.
		/// </summary>
		public void DisposeEnumeration()
		{
			if (!ThreadingUtils.IsFirstSetBooleanInt(ref m_1ForDisposed)) return;

			DisposeResettableProvider(true, null);

			var disposableEnumerable = m_InnerEnumerable as IDisposable;
		    disposableEnumerable?.Dispose();
		    m_InnerEnumerable = null;
		}
		/// <summary>
		/// <see cref="IPAFResettableEnumerableProvider{T}"/>.
		/// This implementation destroys the <see cref="EnumeratorFactory"/> and
		/// <see langword="null"/>s it.
		/// </summary>
		/// <param name="dataCollection">
		/// <see cref="IPAFResettableEnumerableProvider{T}"/>.
		/// If this parameter is <see langword="null"/>, the <see cref="InnerEnumerable"/>
		/// is not reset.
		/// </param>
		public virtual void SetData(IEnumerable<T> dataCollection)
		{
			PAFDisposalUtils.DisposalGuard("EnumerableProvider", m_1ForDisposed);
			lock (m_ResetLock) {
				if (dataCollection != null)
					m_InnerEnumerable = dataCollection;
				if((m_EnumeratorFactory is IDisposable disposableFactory) && (m_HaltCurrentEnumeratorOnReset))
					disposableFactory.Dispose();
				m_EnumeratorFactory = null;
			}
		}
		/// <summary>
		/// <see cref="IPAFResettableEnumerableProvider{T}"/>. This implementation
		/// creates a new internal factory from the <see cref="InnerEnumerable"/>
		/// and just hands out its enumerable.
		/// </summary>
		/// <returns>
		/// <see cref="IPAFResettableEnumerableProvider{T}"/>.
		/// </returns>
		public virtual IEnumerable<T> GetEnumerable()
		{
			// We've got to lock the internals while we get the
			// enumerable. The factory is synchronized, but its
			// destruction/creation must be synchronized here.
			PAFDisposalUtils.DisposalGuard("EnumerableProvider", m_1ForDisposed);
			Monitor.Enter(m_ResetLock);
			if(m_EnumeratorFactory == null)
			m_EnumeratorFactory
				= new EnumeratorFactoryClass<T>(m_InnerEnumerable, true, m_TypeCloner);
			var factory = m_EnumeratorFactory;
			Monitor.Exit(m_ResetLock);
			return factory;
		}
		#endregion Methods
		#region IDisposable Implementation
		///////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Calls main method - <see cref="DisposeResettableProvider"/>.
		/// </summary>
		public void Dispose()
		{
			DisposeResettableProvider(true, null);
		}
		/// <summary>
		/// Disposes the internal enumerator factory instance and <see langword="null"/>'s it.
		/// </summary>
		protected virtual Exception DisposeResettableProvider(
			bool disposing, object obj)
		{
			// At this point, nobody can touch stuff but us.
			// TODO - trap exceptions.
			var disposableFactory = m_EnumeratorFactory as IDisposable;
		    disposableFactory?.Dispose();
		    m_EnumeratorFactory = null;
			return null;
		}
		///////////////////////////////////////////////////////////////////////
		#endregion //IDisposable Implementation
	}
}