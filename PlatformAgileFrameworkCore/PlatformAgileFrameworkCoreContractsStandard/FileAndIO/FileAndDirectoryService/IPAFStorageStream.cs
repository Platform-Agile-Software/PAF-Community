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
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
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
	///	Interface for our file service. This one is a generalization of the
	/// PAF add-in framework. It has been generalized to also support the
	/// full SL isolated storage model. The <see cref="IPAFStorageStream"/>
	/// is a storage stream that is designed to protect the OS from
	/// rogue apps that want to write too much data and other such
	/// things. Same deal as anybody else's defensive storage stream,
	/// really.
	/// </para>
	/// <para>
	/// Implementations can derive from something that is already a <c>FileStream</c>
	/// if they want to do file I/O and can even just inherit from "IsolatedStorageFileStream"
	/// if the SL base classes are used.
	/// </para>
	/// <para>
	/// This interface contains methods with which to read, write and manipulate
	/// storage streams.
	/// </para>
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> BMC </author>
	/// <date> 11jan2016 </date>
	/// <description>
	/// Added <see cref="Stream"/> and the ability to check whether <see cref="Stream"/>s
	/// are expandable. This whole thing now supports the PCL thing (has for a while),
	/// but we do insist on <see cref="Stream"/>s, so we don't need the extra layer.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 17jan2012 </date>
	/// <description>
	/// New.
	/// Decided to put the functionality from the add-in framework file access
	/// in here. The add-in requirements are only a modest extension of the isolated
	/// storage model and this allows one more interface to be consoldidated. Names
	/// are new, but legacy users can just search and replace. This interface supports
	/// the manipulation of a disk file system under standard environments like
	/// ECMA/CLR and Silverlight, but supports the more general concept of
	/// named storage areas in other scenarios.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Noted that there is a big issue here with concurrency. With finite storage,
	/// implementations must handle concurrent requests for storage. Such requests
	/// must be synchronized to avoid overrunning maximum allocations. Shared storage
	/// access obviously also must be synchronized. Delegating to a platform's native
	/// stuff for this generally solves some of these problems, but that's not the only
	/// way we have been used.
	/// </threadsafety>
	/// <exceptions>
	/// As is the usual policy in PAF, general framework exceptions (e.g. null reference)
	/// are not wrapped by any methods here. Exceptions thrown from a specific stream
	/// operation gone wrong (e.g. setting length on a non-settable stream) are.
	/// </exceptions>
	public interface IPAFStorageStream : IDisposable
	{
		#region Properties
		/// <summary>
		/// Gets a Boolean value indicating whether the stream can be read.
		/// </summary>
		/// <returns>
		/// <see langword="true" /> if an <see cref="IPAFStorageStream"/> can
		/// be read.
		/// </returns>
		bool PAFCanRead { get; }
		/// <summary>
		/// Gets a Boolean value indicating whether the stream supports seek
		/// operations.
		/// </summary> 
		/// <returns>
		/// <see langword="true" /> if an <see cref="IPAFStorageStream"/> can
		/// support seek operations.
		/// </returns>
		bool PAFCanSeek { get; }
		/// <summary>
		/// Gets a Boolean value indicating whether the stream supports setting its
		/// length.
		/// </summary> 
		/// <returns>
		/// <see langword="true" /> if an <see cref="IPAFStorageStream"/> can
		/// have its lenght set.
		/// </returns>
		bool PAFCanSetLength { get; }
		/// <summary>
		/// Gets a Boolean value indicating whether the stream can be written to.
		/// </summary> 
		/// <returns>
		/// <see langword="true" /> if an <see cref="IPAFStorageStream"/> can
		/// be written.
		/// </returns>
		bool PAFCanWrite { get; }
		/// <summary>
		/// Gets a value indicating the length of the stream. If the length is settable,
		/// it can be changed dynamically by calling set length. This adjustability of
		/// length is determined on some platforms by parameters passed into a method
		/// creating the stream. If an attempt to write more data than is allocated
		/// for the stream, exceptions are generally thrown. These exceptions are
		/// implementation-specific.
		/// </summary> 
		/// <returns>
		/// Size of the stream, in bytes.
		/// </returns>
		long PAFLength { get; }
		/// <summary>
		/// Gets or sets the current position of the <see cref="IPAFStorageStream"/>.
		/// </summary>
		/// <remarks>
		/// The set operation will generate exceptions if an attempt is made to set
		/// beyond the length of the stream or the position is not settable.
		/// </remarks>
		/// <exceptions>
		/// Various exceptions will be thrown if the implementation relies on native
		/// implementations. These exceptions should be documented and usually wrapped
		/// TODO - Prescribe and document wrapper exceptions.
		/// </exceptions>
		long PAFPosition { get; set; }
		#endregion // Properties
		#region Methods
		/// <summary>
		/// This method exposes a standard <see cref="Stream"/>when needed
		/// for passing into normal framework methods. For a wrapped
		/// implementation this is just the contained stream. For other
		/// implementations, it can be a surrogate
		/// <see cref="MemoryStream"/> or something similar.
		/// </summary>
		/// <returns>The stream.</returns>
		/// <remarks>
		/// BMC - added because we don't want to deal with anybody who doesn't
		/// at least know about <see cref="Stream"/>.
		/// </remarks>
		Stream PAFGetStream();
		/// <summary>
		/// Updates the file with the current state of the buffer then clears the buffer.
		/// </summary>
		/// <exceptions>
		/// Various exceptions will be thrown if the implementation relies on native
		/// implementations. These exceptions should be documented and usually wrapped
		/// TODO - Prescribe and document wrapper exceptions.
		/// </exceptions>
		void PAFFlush();
		/// <summary>
		/// Clears buffers for this stream and causes any buffered data to be written to
		/// the permanent storage area and also optionally clears all intermediate file buffers.
		/// </summary>
		/// <param name="flushToDisk">
		/// <see langword="true"/> to flush all intermediate file buffers.
		/// </param>
		/// <exceptions>
		/// Various exceptions will be thrown if the implementation relies on native
		/// implementations. These exceptions should be documented and usually wrapped
		/// TODO - Prescribe and document wrapper exceptions.
		/// </exceptions>
		void PAFFlush(bool flushToDisk);
		/// <summary>
		/// Sets the length of this <see cref="IPAFStorageStream"/> to the specified
		/// <paramref name="value"/>.
		/// </summary>
		/// <param name="value">
		/// The new length of the <see cref="IPAFStorageStream"/>.
		/// </param>
		/// <exceptions>
		/// Various exceptions will be thrown if the implementation relies on native
		/// implementations. These exceptions should be documented and usually wrapped
		/// TODO - Prescribe and document wrapper exceptions.
		/// </exceptions>
		/// <remarks>
		/// Note that this method may be marked security critical
		/// in some environments (e.g. addin). 
		/// </remarks>
		void PAFSetLength(long value);
		/// <summary>
		/// Sets the length of this <see cref="IPAFStorageStream"/> to the specified
		/// <paramref name="value"/>. This operation may fail, since another thread
		/// may have snuck in and performed another allocation since this thread
		/// called <see cref="PAFSetLength"/>
		/// or something similar. In this case, there may not actually be sufficient
		/// space available at the instant this call is made. The form of this
		/// method allows a concurrent programming style to be used safely. The
		/// implementation can be as simple as trying the call to <see cref="PAFSetLength"/>
		/// and catching an exception resulting a false return. Having this documented
		/// method here brings this issue to the attention of developers who may
		/// not anticipate this issue.
		/// </summary>
		/// <param name="value">
		/// The new desired length of the <see cref="IPAFStorageStream"/>.
		/// </param>
		/// <param name="acceptAvailableSize">
		/// If <see langword = "true"/>, the size will be set to the largest size
		/// possible if the allocation request is not possible.
		/// </param>
		/// <param name="clientObject">
		/// An object that MAY contain security information. This is needed, since
		/// this call may need to increase the overall storage quota and we don't
		/// want low-priviledge callers bumping up the default file size, anyway.
		/// </param>
		/// <exceptions>
		/// Various exceptions will be thrown if the implementation relies on native
		/// implementations. These exceptions should be documented and usually wrapped
		/// TODO - Prescribe and document wrapper exceptions.
		/// </exceptions>
		/// <returns>
		/// The requested size if the allocation suceeded. If the allocation did not
		/// succeed and <paramref name="acceptAvailableSize"/> is <see langword = "true"/>,
		/// the method returns the new size. If the allocation did not succeed and
		/// <paramref name="acceptAvailableSize"/> is <see langword = "false"/>,
		/// the method returns 0.
		/// </returns>
		/// <remarks>
		/// This combines the old "SetBufferSize" methods into one with with a new
		/// boolean argument.
		/// </remarks>
		long PAFTrySetStorageSize(long value, bool acceptAvailableSize, object clientObject);
		/// <summary>
		/// Copies bytes from the current <see cref="IPAFStorageStream"/> to an array.
		/// </summary>
		/// <returns>
		/// The total number of bytes read into the <paramref name="buffer"/>. This can be less
		/// than the number of bytes requested if that many bytes are not currently available,
		/// or zero if the end of the stream is reached.
		/// </returns>
		/// <param name="buffer">
		/// The buffer to read.
		/// </param>
		/// <param name="offset">
		/// The offset in the buffer at which to begin reading.
		/// </param>
		/// <param name="count">
		/// The maximum number of bytes to read.
		/// </param>
		/// <exceptions>
		/// Various exceptions will be thrown if the implementation relies on native
		/// implementations. These exceptions should be documented and usually wrapped
		/// TODO - Prescribe and document wrapper exceptions.
		/// </exceptions>
		/// <threadsafety>
		/// When using shared buffers, the concurrency issue comes up once again.
		/// Implementations must synchronize access to to shared buffers.
		/// </threadsafety>
		int PAFReadBytes(byte[] buffer, int offset, int count);
		/// <summary>
		/// Reads a single byte from the <see cref="IPAFStorageStream"/>.
		/// </summary>
		/// <returns>
		/// The 8-bit unsigned integer value read into an Int32. We adopt
		/// Microsoft's trick of reading into an 32-bit integer so we
		/// can set the sign to -1 as a signature value for failure.
		/// Don't use this to read lots of bytes!
		/// </returns>
		/// <exceptions>
		/// Various exceptions will be thrown if the implementation relies on native
		/// implementations. These exceptions should be documented and usually wrapped
		/// TODO - Prescribe and document wrapper exceptions.
		/// </exceptions>
		int PAFReadByte();
		/// <summary>
		/// Sets the current position of this <see cref="IPAFStorageStream"/> to the specified value.
		/// </summary> 
		/// <param name="offset">
		/// The requested position of the <see cref="IPAFStorageStream"/>.
		/// </param>
		/// <param name="origin">
		/// One of the <see cref="SeekOrigin"/> values.
		/// </param>
		/// <returns>
		/// The new position in the <see cref="IPAFStorageStream"/>.
		/// </returns>
		/// <exceptions>
		/// Various exceptions will be thrown if the implementation relies on native
		/// implementations. These exceptions should be documented and usually wrapped
		/// TODO - Prescribe and document wrapper exceptions.
		/// </exceptions>
		long PAFSeek(long offset, SeekOrigin origin);
		/// <summary>
		/// Writes a block of bytes to the <see cref="IPAFStorageStream"/> from a byte array.
		/// </summary>
		/// <param name="buffer">
		/// The array to write.
		/// </param>
		/// <param name="offset">
		/// The byte offset in the array at which to begin the writing of data.
		/// </param>
		/// <param name="count">The maximum number of bytes to write. </param>
		/// <threadsafety>
		/// This method cannot be relied upon to write safely after calling
		/// <see cref="IPAFStorageService.PAFAvailableFreeSpace"/>
		/// or similar methods, since another thread may have snuck in and done
		/// another allocation or write in the intervening time. Either
		/// <see cref="IPAFStorageStream.PAFSetLength"/> (with caught exception)
		/// or <see cref="IPAFStorageStream.PAFTrySetStorageSize"/> should be called
		/// to ensure that there is enough space on this stream to do the write.
		/// </threadsafety>
		void PAFWriteBytes(byte[] buffer, int offset, int count);
		/// <summary>
		/// Writes a single byte to the <see cref="IPAFStorageStream"/>.
		/// </summary>
		/// <param name="value">
		/// The byte value to write to the stream.
		/// </param>
		/// <exceptions>
		/// Various exceptions will be thrown if the implementation relies on native
		/// implementations. These exceptions should be documented and usually wrapped
		/// TODO - Prescribe and document wrapper exceptions.
		/// </exceptions>
		/// <threadsafety>
		/// See <see cref="PAFWriteBytes"/>.
		/// </threadsafety>
		void PAFWriteByte(byte value);

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
	}

}