
using System;
using System.IO;

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.FileAndIO.FileAndDirectoryService
{
	/// <summary>
	/// This is an interface that models methods on "System.IO.IsolatedStorage.IsolatedStorageFileStream".
	/// The interface is designed to allow retrofitting existing Silverlight storage access
	/// code to control other storage implementations. See individual implementation classes
	/// for details of the behavior of methods. Most cannot be described in the interface,
	/// since implementations may differ widely.
	/// </summary>
	public interface IIsolatedStorageFileStream : IDisposable
	{
		#region Properties
		/// <remarks/>
		bool CanRead { get; }

		/// <remarks/>
		bool CanWrite { get; }

		/// <remarks/>
		bool CanSeek { get; }

		/// <remarks/>
		long Length { get; }

		/// <remarks/>
		long Position { get; set; }
		#endregion // Properties

		#region Methods
		#region Novel Methods
		/// <summary>
		/// This method exposes a standard <see cref="Stream"/>when needed
		/// for passing into normal framework methods. For a wrapped
		/// implementation this is just the contained stream. For other
		/// implementations, it can be a surrogate
		/// <see cref="MemoryStream"/> or something similar.
		/// </summary>
		/// <returns>The stream.</returns>
		Stream GetStream();
		#endregion // Novel Methods

		/// <remarks/>
		void Flush();

		/// <remarks/>
		void Flush(bool flushToDisk);

		/// <remarks/>
		int Read(byte[] buffer, int offset, int count);

		/// <remarks/>
		int ReadByte();

		/// <remarks/>
		long Seek(long offset, SeekOrigin origin);

		/// <remarks/>
		void SetLength(long value);

		/// <remarks/>
		void Write(byte[] buffer, int offset, int count);

		/// <remarks/>
		void WriteByte(byte value);
		#region AsyncMethods

		/// <remarks/>
		IAsyncResult BeginRead(byte[] buffer, int offset, int numBytes, AsyncCallback userCallback, object stateObject);

		/// <remarks/>
		int EndRead(IAsyncResult asyncResult);

		/// <remarks/>
		IAsyncResult BeginWrite(byte[] buffer, int offset, int numBytes, AsyncCallback userCallback, object stateObject);

		/// <remarks/>
		void EndWrite(IAsyncResult asyncResult);
		#endregion // AsyncMethods
		#endregion // Methods
	}
}
