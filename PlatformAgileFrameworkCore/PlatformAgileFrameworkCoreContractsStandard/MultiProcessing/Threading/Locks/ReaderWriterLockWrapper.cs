using System;
using System.Threading;

namespace PlatformAgileFramework.MultiProcessing.Threading.Locks
{
	/// <summary>
	/// Wrapper class uses an internal <see cref="ReaderWriterLockSlim"/> to perform
	/// the locking, but we neuter it, since the upgradable lock stuff always had
	/// concurrency issues.
	/// </summary>
	public class ReaderWriterLockSlimWrapper : IReaderWriterLock
	{
		#region Fields and Autoproperties
		/// <summary>
		/// The reader/writer lock we delegate to.
		/// </summary>
		private readonly ReaderWriterLockSlim m_Slim = new ReaderWriterLockSlim();
		#endregion // Fields and Autoproperties
		/// <summary>
		/// Delegates to <see cref="ReaderWriterLockSlim"/>.
		/// </summary>
		public virtual void EnterReadLock()
		{
			m_Slim.EnterReadLock();
		}
		/// <summary>
		/// Delegates to <see cref="ReaderWriterLockSlim"/>.
		/// </summary>
		public virtual void ExitReadLock()
		{
			m_Slim.ExitReadLock();
		}
		/// <summary>
		/// Delegates to <see cref="ReaderWriterLockSlim"/>.
		/// </summary>
		public virtual void EnterWriteLock()
		{
			m_Slim.EnterWriteLock();
		}
		/// <summary>
		/// Delegates to <see cref="ReaderWriterLockSlim"/>.
		/// </summary>
		public virtual void ExitWriteLock()
		{
			m_Slim.ExitWriteLock();
		}
		/// <summary>
		/// Delegates to <see cref="ReaderWriterLockSlim"/>.
		/// </summary>
		/// <param name="timeSpan">Time to wait for the lock to be acquired.</param>
		/// <returns>
		/// <see cref="ReaderWriterLockSlim"/>.
		/// </returns>
		public virtual bool TryEnterReadLock(TimeSpan timeSpan)
		{
			return m_Slim.TryEnterReadLock(timeSpan);
		}
		/// <summary>
		/// Delegates to <see cref="ReaderWriterLockSlim"/>.
		/// </summary>
		/// <param name="timeSpan">Time to wait for the lock to be acquired.</param>
		/// <returns>
		/// <see cref="ReaderWriterLockSlim"/>.
		/// </returns>
		public virtual bool TryEnterWriteLock(TimeSpan timeSpan)
		{
			return m_Slim.TryEnterWriteLock(timeSpan);
		}
		#region IDisposable Implementation
		/// <summary>
		/// Calls <see cref="Dispose(bool)"/> with <see langword="true"/>.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}
		/// <summary>
		/// Does nothing.
		/// </summary>
		public virtual void Dispose(bool isDisposing)
		{
		}
		#endregion // IDisposable Implementation
	}
}
