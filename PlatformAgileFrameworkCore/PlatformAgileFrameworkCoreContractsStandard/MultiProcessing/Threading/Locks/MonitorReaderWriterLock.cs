using System;
using System.Threading;

namespace PlatformAgileFramework.MultiProcessing.Threading.Locks
{
	/// <summary>
	/// This class uses a monitor for the lock. This means that there
	/// is no "read" lock - Monitor locks don't allow reads or writes. The original
	/// purpose for this lock was in scenarios where we didn't want to make
	/// types disposable. It's fine to substitute a monitor in places
	/// where traffic is low as long as the hosting class is not counting
	/// on simultaneous reads.
	/// </summary>
	public class MonitorReaderWriterLock : IReaderWriterLock
	{
		#region Fields and Autoproperties
		/// <summary>
		/// The object we take the lock on.
		/// </summary>
		private readonly object m_LockObject = new object();
		#endregion // Fields and Autoproperties
		/// <summary>
		/// <see cref="IReaderWriterLock"/>
		/// </summary>
		public virtual void EnterReadLock()
		{
			Monitor.Enter(m_LockObject);
		}
		/// <summary>
		/// <see cref="IReaderWriterLock"/>
		/// </summary>
		public virtual void ExitReadLock()
		{
			Monitor.Exit(m_LockObject);
		}
		/// <summary>
		/// <see cref="IReaderWriterLock"/>
		/// </summary>
		public virtual void EnterWriteLock()
		{
			Monitor.Enter(m_LockObject);
		}
		/// <summary>
		/// <see cref="IReaderWriterLock"/>
		/// </summary>
		public virtual void ExitWriteLock()
		{
			Monitor.Exit(m_LockObject);
		}
		/// <summary>
		/// <see cref="IReaderWriterLock"/>
		/// </summary>
		/// <param name="timeSpan">Time to wait for the lock to be acquired.</param>
		/// <returns><see langword="true"/> always.</returns>
		public virtual bool TryEnterReadLock(TimeSpan timeSpan)
		{
			return Monitor.TryEnter(m_LockObject, timeSpan);
		}
		/// <summary>
		/// <see cref="IReaderWriterLock"/>
		/// </summary>
		/// <param name="timeSpan">Time to wait for the lock to be acquired.</param>
		/// <returns><see langword="true"/> always.</returns>
		public virtual bool TryEnterWriteLock(TimeSpan timeSpan)
		{
			return Monitor.TryEnter(m_LockObject, timeSpan);
		}
		#region IDisposable Implementation
		/// <summary>
		/// Does nothing.
		/// </summary>
		public virtual void Dispose()
		{
		}
		#endregion // IDisposable Implementation
	}
}
