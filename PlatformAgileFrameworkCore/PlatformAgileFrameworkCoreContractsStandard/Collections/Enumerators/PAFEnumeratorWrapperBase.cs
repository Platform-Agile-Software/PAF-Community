using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using PlatformAgileFramework.MultiProcessing.Tasking;
using PlatformAgileFramework.MultiProcessing.Threading;
using PlatformAgileFramework.MultiProcessing.Threading.NullableObjects;
using PlatformAgileFramework.TypeHandling;
using PlatformAgileFramework.TypeHandling.Disposal;

namespace PlatformAgileFramework.Collections.Enumerators
{
	/// <summary>
	/// <para>
	/// This is a base class for an enumerator. It is a base implementation
	/// of <see cref="IPAFEnumeratorWrapper{T}"/>. Most of its methods are virtual
	/// for subclassing purposes.
	/// </para>
	/// <para>
	/// This class can be disposed early, potentially by another thread. This
	/// allows enumerations to be stopped early, instead of allowing a foreach
	/// loop using the enumerable to continue. When the class is disposed, its
	/// <see cref="MoveNext"/> method will return <see langword="false"/> so that the
	/// foreach loop will exit.
	/// </para>
	/// <para>
	/// The enumerator exposed by this <see cref="IEnumerable{T}"/> can potentially
	/// accessed by multiple threads. This has application in the parallel processing
	/// library. By default, the enumerator is permitted to be accessed by one thread
	/// only. This is enforced by a thread guard on the <see cref="IEnumerator{T}"/>
	/// methods.
	/// </para>
	/// </summary>
	/// <typeparam name="T">
	/// This is the generic type of items on the enumerable.
	/// </typeparam>
	/// <threadsafety>
	/// <para>
	/// This class is designed to be POTENTIALLY thread-safe. All properties and
	/// methods are synchronized. However, what you get from this class is as
	/// good as what you put into it. If items of type <typeparamref name="T"/>
	/// are mutable and they are attempted to be modified on multiple threads,
	/// collisions will occcur unless items of type <typeparamref name="T"/>
	/// are themselves synchronized. The class offers a pluggable
	/// <see cref="TypeHandlingUtils.TypeCloner{T}"/> to make appropriate (deep or shallow) copies
	/// of the items on the way out. In many cases, this solves the problem unless
	/// the original items must be modified. Then there is no choice but to
	/// synchronize them.
	/// </para>
	/// <para>
	/// Note that this class employs monitor locks - R/W locks don't buy us anything,
	/// since items have to be write-locked even when they are copied out.
	/// </para>
	/// </threadsafety>
	/// <serialization>
	/// Not serialized.
	/// </serialization>
	/// <history>
	/// <contribution>
	/// <author> JAW(P) </author>
	/// <date> 15jun2015 </date>
	/// <description>
	/// Updated for task-based operation.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 15oct2012 </date>
	/// <description>
	/// New. Default implementation of the interface.
	/// </description>
	/// </contribution>
	/// </history>
	public abstract class PAFEnumeratorWrapperBase<T> : IPAFEnumeratorWrapper<T>
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Atomically set variable for disposal state.
		/// </summary>
		private int m_1ForDisposed;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal long m_EnumerationPosition;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal T m_CurrentItem;
		/// <summary>
		/// Dispose inner enumerable when done?.
		/// </summary>
		protected internal readonly bool m_DisposeEnumerable;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal IEnumerable<T> m_InnerEnumerable;
		/// <summary>
		/// Concurrency flag.
		/// </summary>
		protected internal readonly bool m_IsTaskBound;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal IEnumerator<T> m_ItemEnumerator;
		/// <summary>
		/// Lock for item enumerator.
		/// </summary>
		protected internal readonly object m_ItemEnumeratorLock
			= new object();
		/// <summary>
		/// Lock for reset.
		/// </summary>
		protected internal readonly object m_MainEnumerationLock
			= new object();
		/// <summary>
		/// Loaded if we are in task-bound mode. Only written
		/// once, thus efficient.
		/// </summary>
		protected internal NullableSynchronizedWrapper<int?> m_TaskID
			= new NullableSynchronizedWrapper<int?>();
		/// <summary>
		/// Pluggable copyer.
		/// </summary>
		protected internal readonly TypeHandlingUtils.TypeCloner<T> m_TypeCloner;

		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Constructor sets <see cref="InnerEnumerable"/>, disposal flag and the cloner.
		/// </summary>
		/// <param name="enumerable">
		/// The incoming enumerable to build with. Can't be be <see langword="null"/>.
		/// </param>
		/// <param name="disposeEnumerable">
		/// Set this to <see langword="true"/> to dispose the <see cref="InnerEnumerable"/>
		/// when dispose is called on the enumerator if the enumerable is disposable.
		/// This may mean that this class is no longer usable, depending on the
		/// implementation.
		/// </param>
		/// <param name="cloner">
		/// This the cloner that makes clones of items on the way out. If
		/// <see langword="null"/>, items are cloned according to <see cref="CopyItem"/>.
		/// </param>
		/// <param name="isMTaskBound">
		/// Tells whether the class will allow multiple threads to call the
		/// <see cref="IEnumerator{T}"/> methods on this class. Default
		/// is <see langword="false"/>.
		/// </param>
		protected PAFEnumeratorWrapperBase(IEnumerable<T> enumerable,
			bool disposeEnumerable = false, TypeHandlingUtils.TypeCloner<T> cloner = null,
			bool isMTaskBound = false)
		{
			m_InnerEnumerable = enumerable;
			m_DisposeEnumerable = disposeEnumerable;
			m_TypeCloner = cloner;
			m_IsTaskBound = isMTaskBound;
		}
		#endregion // Constructors
		#region Properties
		/// <summary>
		/// Helper to manage access to the "Current" item with a monitor lock.
		/// </summary>
		/// <remarks>
		/// This synchronized accessor should be used by all but the
		/// data reset methods in subclasses, which also lock the class internals
		/// during their work.
		/// </remarks>
		protected internal virtual T CurrentItemSynchronized
		{
			get
			{
				PAFDisposalUtils.DisposalGuard("WrapperBase", m_1ForDisposed);
				lock (m_MainEnumerationLock)
				{
					// We have to do all the action here, so another thread
					// can't touch the item when we are copying it.
					return CopyItem(m_CurrentItem);
				}
			}
			set
			{
				PAFDisposalUtils.DisposalGuard("WrapperBase", m_1ForDisposed);
				lock (m_MainEnumerationLock)
				{
					m_CurrentItem = value;
				}
			}
		}

		/// <summary>
		/// State of the enumeration. Synchronized access with a monitor.
		/// </summary>
		/// <remarks>
		/// This synchronized accessor should be used by all but the
		/// <see cref="Reset"/> method, which also locks the class internals
		/// during its work.
		/// </remarks>
		protected virtual long EnumerationPosition
		{
			get
			{
				long enumerationPosition;
				PAFDisposalUtils.DisposalGuard("WrapperBase", m_1ForDisposed);
				lock (m_MainEnumerationLock)
				{
					enumerationPosition = m_EnumerationPosition;
				}
				return enumerationPosition;
			}
			set
			{
				PAFDisposalUtils.DisposalGuard("WrapperBase", m_1ForDisposed);
				lock (m_MainEnumerationLock)
				{
					m_EnumerationPosition = value;
				}
			}
		}

		/// <summary>
		/// Holds the items iterated over. Synchronized access with a monitor.
		/// </summary>
		/// <remarks>
		/// This synchronized accessor should be used by all but the
		/// <see cref="Reset"/> method, which also locks the class internals
		/// during its work.
		/// </remarks>
		public virtual IEnumerable<T> InnerEnumerable
		{
			get
			{
				IEnumerable<T> enumerable;
				PAFDisposalUtils.DisposalGuard("WrapperBase", m_1ForDisposed);
				lock (m_MainEnumerationLock)
				{
					enumerable = m_InnerEnumerable;
				}
				return enumerable;
			}
			protected set
			{
				PAFDisposalUtils.DisposalGuard("WrapperBase", m_1ForDisposed);
				lock (m_MainEnumerationLock)
				{
					m_InnerEnumerable = value;
				}
			}
		}
		/// <summary>
		/// Holds the item enumerator. This method should be called only
		/// from an enumerator method or before the class is in use.
		/// </summary>
		protected internal virtual IEnumerator<T> ItemEnumeratorUnsynchronized
		{
			get
			{
				PAFDisposalUtils.DisposalGuard("WrapperBase", m_1ForDisposed);
				return m_ItemEnumerator;
			}
			set
			{
				PAFDisposalUtils.DisposalGuard("WrapperBase", m_1ForDisposed);
				m_ItemEnumerator = value;
			}
		}
		#endregion // Properties
		#region Methods
		/// <summary>
		/// See <see cref="IEnumerator"/>. This method calls our internal
		/// enumerator after loading it, if neccessary from the
		/// <see cref="InnerEnumerable"/>, if necessary. If the enumeration is
		/// over, the <see cref="Current"/> value is set to <c>default(T)</c>.
		/// The last value is NOT held.
		/// </summary>
		/// <returns>
		/// See <see cref="IEnumerator"/>. This will return <see langword="false"/> if
		/// this class has been disposed.
		/// </returns>
		protected internal virtual bool MoveNextSynchronized()
		{
			PAFDisposalUtils.DisposalGuard("WrapperBase", m_1ForDisposed);
			lock (m_MainEnumerationLock)
			{
				// Startup with ordinary enumerator?
				if (ItemEnumeratorUnsynchronized == null)
					ItemEnumeratorUnsynchronized = InnerEnumerable.GetEnumerator();
			}

			// Our wrapped enumerator got any more?
			if (ItemEnumeratorUnsynchronized.MoveNext())
			{
				CurrentItemSynchronized = ItemEnumeratorUnsynchronized.Current;
				return true;
			}

			CurrentItemSynchronized = default(T);
			return false;
		}
		#region IDisposable Implementation
		///////////////////////////////////////////////////////////////////////
		/// <summary>
		/// Calls main method - <see cref="DisposeEnumerableWrapperBase"/>.
		/// </summary>
		public void Dispose()
		{
			if (ThreadingUtils.IsFirstSetBooleanInt(ref m_1ForDisposed))
				DisposeEnumerableWrapperBase(true, null);
		}
		/// <summary>
		/// Disposes the internal enumerator instance and <see langword="null"/>'s it.
		/// Disposes the internal <see cref="InnerEnumerable"/> if it is not
		/// <see langword="null"/> and it is disposable.
		/// </summary>
		protected virtual Exception DisposeEnumerableWrapperBase(
			bool disposing, object obj)
		{
			// At this point, nobody can touch stuff but us.
			// TODO - trap exceptions.
		    m_ItemEnumerator?.Dispose();
		    var disposableEnumerable = m_InnerEnumerable as IDisposable;
			if ((disposableEnumerable != null) && (m_DisposeEnumerable))
				disposableEnumerable.Dispose();
			return null;
		}
		///////////////////////////////////////////////////////////////////////
		#endregion //IDisposable Implementation
		#region IEnumerator<T> Implementation
		/// <summary>
		/// Delegates to <see cref="CurrentItemSynchronized"/>. This will return
		/// "default(T)" if this class has been disposed. This get method copies
		/// items in various ways. If this class has a type cloner installed, it
		/// uses that. If an individual item implements
		/// <see cref="IPAFGenericDeepCloneable{T}"/> it is copied that way.
		/// Finally, if no cloner is installed, it just outputs the item
		/// directly.
		/// </summary>
		public virtual T Current
		{
			get
			{
				if (m_1ForDisposed == 1) return default(T);

				// Check if we have a task collision problem.
				if (m_IsTaskBound) SingleTaskGuard();

				var item = CurrentItemSynchronized;
				return item;
			}
		}
		#region IEnumerator Implementation
		/// <summary>
		/// <see cref="IEnumerator"/>.
		/// </summary>
		object IEnumerator.Current
		{
			get { return Current; }
		}
		/// <summary>
		/// See <see cref="IEnumerator"/>. This method calls our internal
		/// enumerator after loading it, if neccessary from the
		/// <see cref="InnerEnumerable"/>, if necessary. If the enumeration is
		/// over, the <see cref="Current"/> value is set to <c>default(T)</c>.
		/// The last value is NOT held.
		/// </summary>
		/// <returns>
		/// See <see cref="IEnumerator"/>. This will return <see langword="false"/> if
		/// this class has been disposed.
		/// </returns>
		public virtual bool MoveNext()
		{
			if (m_1ForDisposed == 1) return false;

			// Check if we have a task collision problem.
			if (m_IsTaskBound) SingleTaskGuard();

			return MoveNextSynchronized();
		}
		/// <summary>
		/// This implementation calls dispose on the current internal enumerator
		/// (if not <see langword="null"/>) and <see langword="null"/>s it.
		/// </summary>
		public virtual void Reset()
		{
			PAFDisposalUtils.DisposalGuard("WrapperBase", m_1ForDisposed);
			lock (m_MainEnumerationLock)
			{
				// Check if we have a task collision problem.
				if (m_IsTaskBound) SingleTaskGuard();

			    m_ItemEnumerator?.Dispose();
			    m_ItemEnumerator = null;
			}
		}
		#endregion // IEnumerator Implementation
		#endregion IEnumerator<T> Implementation
		#region Implementation of IEnumerable
		/// <summary>
		/// Calls virtual method.
		/// </summary>
		/// <returns>us.</returns>
		public IEnumerator<T> GetEnumerator()
		{
			return GetEnumerator_PIV();
		}
		/// <summary>
		/// Calls Generic version.
		/// </summary>
		/// <returns>An enumerator.</returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion
		/// <summary>
		/// This is just about the simplest implementation of an
		/// enumerable. We just hand ourselves out. This is useful
		/// to implement a provider, where the provider creates
		/// a new instance of us each time it hands out an enumerable.
		/// </summary>
		/// <returns>us.</returns>
		protected internal virtual IEnumerator<T> GetEnumerator_PIV()
		{
			return this;
		}
		/// <summary>
		/// This get method copies items in various ways. If this class
		/// has a type cloner installed, it uses that. Otherwise it calls
		/// <see cref="TypeHandlingUtils.LayeredSafeReplication{T}"/>.
		/// </summary>
		protected internal virtual T CopyItem(T itemToCopy)
		{
			if (TypeHandlingUtils.GenericIsNull(itemToCopy)) return default(T);

			if (m_TypeCloner != null) return m_TypeCloner(itemToCopy);

			return TypeHandlingUtils.LayeredSafeReplication(itemToCopy);
		}

		/// <summary>
		/// Helper method just checks against multiple tasks.
		/// </summary>
		protected internal void SingleTaskGuard()
		{
			var executingTaskID = Task.CurrentId;
			using (var mutator = m_TaskID.GetWriteLockedObject())
			{
				if (!mutator.HasBeenSet) mutator.WriteLockedNullableObject = executingTaskID;
				else
				{
					var singleTaskID = mutator.WriteLockedNullableObject;
					TaskUtils.SingleTaskGuard(executingTaskID, singleTaskID);
				}
			}

		}
		#endregion // Methods
	}
}