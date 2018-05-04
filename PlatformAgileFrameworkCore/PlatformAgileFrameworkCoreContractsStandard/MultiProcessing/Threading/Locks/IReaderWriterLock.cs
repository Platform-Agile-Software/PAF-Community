using System;

namespace PlatformAgileFramework.MultiProcessing.Threading.Locks
{
	/// <summary>
	/// Interface has typical methods as in "ReaderWriterLockSlim" and
	/// other reader/writer locks. This interface has a reduced set of methods
	/// compared to some to result (hopefully) in a more efficient implementation.
	/// We don't need any others than those prototyped here in PAF.
	/// </summary>
	public interface IReaderWriterLock :IDisposable
	{
		/// <summary>
		/// The lock is entered in a "readonly" fashion. Multiple callers
		/// can access the synchronized data for operations that only
		/// involved reading.
		/// </summary>
		void EnterReadLock();
		/// <summary>
		/// The lock is released by this particular calling thread. Others may
		/// still hold it.
		/// </summary>
		void ExitReadLock();
		/// <summary>
		/// The lock is entered in a read/write fashion. Only one calling thread
		/// may access the synchronized data for the purpose of writing. This
		/// lock will not be taken until all other locks have been released.
		/// </summary>
		void EnterWriteLock();
		/// <summary>
		/// The lock is released by this particular calling thread. Others may
		/// now take the lock.
		/// </summary>
		void ExitWriteLock();
		/// <summary>
		/// Timed version of <see cref="EnterReadLock"/> that returns after
		/// a specified time if it cannot acquire the lock.
		/// </summary>
		/// <param name="timeSpan">
		/// Time to wait for acquiring lock.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if lock was acquired within the given time.
		/// </returns>
		bool TryEnterReadLock(TimeSpan timeSpan);
		/// <summary>
		/// Timed version of <see cref="EnterWriteLock"/> that returns after
		/// a specified time if it cannot acquire the lock.
		/// </summary>
		/// <param name="timeSpan">
		/// Time to wait for acquiring lock.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if lock was acquired within the given time.
		/// </returns>
		bool TryEnterWriteLock(TimeSpan timeSpan);
	}
}
