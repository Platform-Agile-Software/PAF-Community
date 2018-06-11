//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010-2016 Icucom Corporation
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
using System.IO;

namespace PlatformAgileFramework.FileAndIO.FileAndDirectoryService
{
	/// <summary>
	/// <para>
	///	Default for our file service. Does the streams that support files and other
	/// things.
	/// </para>
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> BMC </author>
	/// <date> 17jan2016 </date>
	/// <description>
	/// New.
	/// Decided not to support any PCL's that do not have the concept of a <see cref="Stream"/>.
	/// Tired of trying to support every single scenario in the universe.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Unsafe.
	/// </threadsafety>
	public class PAFStorageStream : IPAFStorageStream
	{
		#region Fields and AutoProperies
		/// <summary>
		/// This is the stream we are built with.
		/// </summary>
		private readonly Stream m_StreamInUse;
		#endregion // Fields and AutoProperies
		#region Constructors

		/// <summary>
		/// Just embeds a stream
		/// </summary>
		/// <param name="stream">Incoming <see cref="Stream"/>.</param>
		public PAFStorageStream(Stream stream)
		{
			m_StreamInUse = stream;
		}
		#endregion // Constructors
		#region Implementation of IPAFStorageStream
		#region Properties
		/// <summary>
		/// <see cref = "IPAFStorageStream"/>.
		/// </summary>
		/// <returns>
		/// <see cref = "IPAFStorageStream"/>.
		/// </returns>
		public virtual bool PAFCanRead { get { return m_StreamInUse.CanRead; } }
		/// <summary>
		/// <see cref = "IPAFStorageStream"/>.
		/// </summary> 
		/// <returns>
		/// <see cref = "IPAFStorageStream"/>.
		/// </returns>
		public virtual bool PAFCanSeek { get { return m_StreamInUse.CanSeek; } }
		/// <summary>
		/// <see cref = "IPAFStorageStream"/>.
		/// This one always returns <see langword="true"/> because we don't
		/// know what we are. This is from the addin interface and has to stay, but
		/// it's OK, since we are documenting it, no?
		/// </summary> 
		/// <returns>
		/// <see langword="true" />.
		/// </returns>
		public virtual bool PAFCanSetLength
		{
			get { return true; }
		}
		/// <summary>
		/// <see cref = "IPAFStorageStream"/>.
		/// </summary> 
		/// <returns>
		/// <see cref = "IPAFStorageStream"/>.
		/// </returns>
		public virtual bool PAFCanWrite { get { return m_StreamInUse.CanWrite; } }
		/// <summary>
		/// <see cref = "IPAFStorageStream"/>.
		/// </summary> 
		/// <returns>
		/// <see cref = "IPAFStorageStream"/>.
		/// </returns>
		public virtual long PAFLength { get { return m_StreamInUse.Length; } }

		/// <summary>
		/// <see cref = "IPAFStorageStream"/>.
		/// </summary>
		/// <remarks>
		/// <see cref = "IPAFStorageStream"/>.
		/// </remarks>
		public virtual long PAFPosition
		{
			get { return m_StreamInUse.Position; }
			set { m_StreamInUse.Position = value; }
		}
		#endregion // Properties
		#region Methods
		/// <summary>
		/// <see cref = "IPAFStorageStream"/>.
		/// </summary>
		/// <returns>
		/// <see cref = "IPAFStorageStream"/>.
		/// </returns>
		public virtual Stream PAFGetStream()
		{
			return m_StreamInUse;
		}

		/// <summary>
		/// <see cref = "IPAFStorageStream"/>.
		/// This is a no op.
		/// </summary>
		public virtual void PAFFlush()
		{
			m_StreamInUse.Flush();
		}

		/// <summary>
		/// <see cref = "IPAFStorageStream"/>.
		/// Just calls <see cref="PAFFlush()"/>.
		/// </summary>
		/// <param name="flushToDisk">
		/// <see cref = "IPAFStorageStream"/>.
		/// Not used.
		/// </param>
		public virtual void PAFFlush(bool flushToDisk)
		{
			m_StreamInUse.Flush();
		}

		/// <summary>
		/// <see cref = "IPAFStorageStream"/>.
		/// </summary>
		/// <param name="value">
		/// <see cref = "IPAFStorageStream"/>.
		/// </param>
		public virtual void PAFSetLength(long value)
		{
			m_StreamInUse.SetLength(value);
		}

		/// <summary>
		/// <see cref = "IPAFStorageStream"/>.
		/// </summary>
		/// <param name="value">
		/// <see cref = "IPAFStorageStream"/>.
		/// </param>
		/// <param name="acceptAvailableSize">
		/// <see cref = "IPAFStorageStream"/>.
		/// </param>
		/// <param name="clientObject">
		/// <see cref = "IPAFStorageStream"/>.
		/// Unused in this implementation.
		/// </param>
		/// <returns>
		/// <see cref = "IPAFStorageStream"/>.
		/// </returns>
		public virtual long PAFTrySetStorageSize(long value,
			bool acceptAvailableSize, object clientObject)
		{
			PAFSetLength(value);
			return value;
		}

		/// <summary>
		/// <see cref = "IPAFStorageStream"/>.
		/// </summary>
		/// <returns>
		/// <see cref = "IPAFStorageStream"/>.
		/// </returns>
		/// <param name="buffer">
		/// <see cref = "IPAFStorageStream"/>.
		/// </param>
		/// <param name="offset">
		/// <see cref = "IPAFStorageStream"/>.
		/// </param>
		/// <param name="count">
		/// <see cref = "IPAFStorageStream"/>.
		/// </param>
		public virtual int PAFReadBytes(byte[] buffer, int offset, int count)
		{
			return m_StreamInUse.Read(buffer, offset, count);
		}

		/// <summary>
		/// <see cref = "IPAFStorageStream"/>.
		/// </summary>
		/// <returns>
		/// <see cref = "IPAFStorageStream"/>.
		/// </returns>
		public virtual int PAFReadByte()
		{
			return m_StreamInUse.ReadByte();
		}

		/// <summary>
		/// <see cref = "IPAFStorageStream"/>.
		/// </summary> 
		/// <param name="offset">
		/// <see cref = "IPAFStorageStream"/>.
		/// </param>
		/// <param name="origin">
		/// <see cref = "IPAFStorageStream"/>.
		/// </param>
		/// <returns>
		/// <see cref = "IPAFStorageStream"/>.
		/// </returns>
		public virtual long PAFSeek(long offset, SeekOrigin origin)
		{
			return m_StreamInUse.Seek(offset, origin);
		}

		/// <summary>
		/// <see cref = "IPAFStorageStream"/>.
		/// </summary>
		/// <param name="buffer">
		/// <see cref = "IPAFStorageStream"/>.
		/// </param>
		/// <param name="offset">
		/// <see cref = "IPAFStorageStream"/>.
		/// </param>
		/// <param name="count">
		/// <see cref = "IPAFStorageStream"/>.
		/// </param>
		public virtual void PAFWriteBytes(byte[] buffer, int offset, int count)
		{
			m_StreamInUse.Write(buffer, offset,count);
		}

		/// <summary>
		/// <see cref = "IPAFStorageStream"/>.
		/// </summary>
		/// <param name="value">
		/// <see cref = "IPAFStorageStream"/>.
		/// </param>
		public virtual void PAFWriteByte(byte value)
		{
			m_StreamInUse.WriteByte(value);
		}

		// TODO - KRM. Postponed until acsync control classes are checked out. We don't want to
		// TODO use SL native crap.
		//IAsyncResult BeginRead(byte[] buffer, int offset, int numBytes, AsyncCallback userCallback, object stateObject);

		// TODO - KRM. Postponed until acsync control classes are checked out. We don't want to
		// TODO use SL native crap.
		//Int32 EndRead(IAsyncResult asyncResult);

		// TODO - KRM. Postponed until acsync control classes are checked out. We don't want to
		// TODO use SL native crap.
		// IAsyncResult BeginWrite(byte[] buffer, int offset, int numBytes, AsyncCallback userCallback, object stateObject);

		// TODO - KRM. Postponed until acsync control classes are checked out. We don't want to
		// TODO use SL native crap.
		//void EndWrite(IAsyncResult asyncResult);
		#endregion // Methods
		#endregion // Implementation of IPAFStorageStream

		#region Implementation of IDisposable
		/// <summary>
		/// <see cref="IDisposable"/>
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}
		#endregion // Implementation of IDisposable
		/// <summary>
		/// Disposes the stream.
		/// </summary>
		/// <param name="disposing">
		/// If <see langword="true"/>, disposes stream.
		/// </param>
		protected internal virtual void Dispose(bool disposing)
		{
			if(disposing) m_StreamInUse.Dispose();
		}
	}
}