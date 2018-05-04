using System;

namespace PlatformAgileFramework.MultiProcessing.Threading.Locks
{
	/// <summary>
	/// This class is a dummy - it doesn't lock anything.
	/// </summary>
	public class DummyReaderWriterLock : IReaderWriterLock
	{
		/// <summary>
		/// Does nothing.
		/// </summary>
		public virtual void EnterReadLock()
		{
		}
		/// <summary>
		/// Does nothing.
		/// </summary>
		public virtual void ExitReadLock()
		{
		}
		/// <summary>
		/// Does nothing.
		/// </summary>
		public virtual void EnterWriteLock()
		{
		}
		/// <summary>
		/// Does nothing.
		/// </summary>
		public virtual void ExitWriteLock()
		{
		}
		/// <summary>
		/// Does nothing.
		/// </summary>
		/// <param name="timeSpan">Time to wait for the lock to be acquired.</param>
		/// <returns><see langword="true"/> always.</returns>
		public virtual bool TryEnterReadLock(TimeSpan timeSpan)
		{
			return true;
		}
		/// <summary>
		/// Does nothing.
		/// </summary>
		/// <param name="timeSpan">Time to wait for the lock to be acquired.</param>
		/// <returns><see langword="true"/> always.</returns>
		public virtual bool TryEnterWriteLock(TimeSpan timeSpan)
		{
			return true;
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
