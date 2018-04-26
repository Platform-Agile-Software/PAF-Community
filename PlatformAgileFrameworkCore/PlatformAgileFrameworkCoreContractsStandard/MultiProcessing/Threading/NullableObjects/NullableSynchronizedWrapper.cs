using PlatformAgileFramework.MultiProcessing.Threading.Locks;

namespace PlatformAgileFramework.MultiProcessing.Threading.NullableObjects
{
	/// <summary>
	/// Sealed version of <see cref="NullableSynchronizedWrapperBase{T}"/>.
	/// </summary>
	public sealed class NullableSynchronizedWrapper<T>
		: NullableSynchronizedWrapperBase<T>
	{
		/// <summary>
		/// See <see cref="NullableSynchronizedWrapperBase{T}"/>.
		/// </summary>
		public NullableSynchronizedWrapper()
			:base(null) 
		{
		}
		/// <summary>
		/// See <see cref="NullableSynchronizedWrapperBase{T}"/>.
		/// </summary>
		/// <param name="t">
		/// See <see cref="NullableSynchronizedWrapperBase{T}"/>.
		/// </param>
		public NullableSynchronizedWrapper(T t)
			: base(t)
		{
		}
		/// <summary>
		/// See <see cref="NullableSynchronizedWrapperBase{T}"/>.
		/// </summary>
		/// <param name="t">
		/// See <see cref="NullableSynchronizedWrapperBase{T}"/>.
		/// </param>
		/// <param name="iReaderWriterLock">
		/// See <see cref="NullableSynchronizedWrapperBase{T}"/>.
		/// </param>
		public NullableSynchronizedWrapper(T t, IReaderWriterLock iReaderWriterLock)
			: base(t, iReaderWriterLock)
		{
		}
	}
}
