using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlatformAgileFramework.FileAndIO.FileAndDirectoryService
{
	/// <summary>
	/// Extension methods for the <see cref="IPAFStorageStream"/> interface. Put
	/// them here to avoid too much clutter in <see cref="IPAFStorageStream"/>.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 28jan2012 </date>
	/// <description>
	/// New.
	/// </description>
	/// </contribution>
	/// </history>
// ReSharper disable PartialTypeWithSinglePart
	public static partial class StorageStreamExtensionMethods
// ReSharper restore PartialTypeWithSinglePart
	{
		/// <summary>
		/// Reads a character set from the <see cref="IPAFStorageStream"/> with UNICODE
		/// encoding or a specified encoding. The stream is read from the current position
		/// to the end.
		/// </summary>
		/// <param name="storageStream">
		/// <see cref="IPAFStorageStream"/>
		/// </param>
		/// <param name="encoding">
		/// An instance of the <see cref="Encoding"/> class that specifies how byte
		/// to character mapping is to be performed. <see langword="null"/> will cause
		/// UniCode encoding to be used, Little-endian, no mark.
		/// </param>
		/// <exceptions>
		/// Various exceptions will be thrown if the implementation relies on native
		/// implementations.
		/// </exceptions>
		public static IEnumerable<char> PAFReadChars(this IPAFStorageStream storageStream,
			Encoding encoding = null)
		{
			if(encoding == null) encoding = new UTF8Encoding();
			var allocation = storageStream.PAFLength - storageStream.PAFPosition;
			var bytes = new byte[allocation];
			var count = storageStream.PAFReadBytes(bytes, 0, (int) allocation);
			var chars = encoding.GetChars(bytes, 0, count);
			return chars;
		}
		/// <summary>
		/// Reads a string from a <see cref="IPAFStorageStream"/>. Just calls
		/// into <see cref="PAFReadChars(IPAFStorageStream,Encoding)"/>.
		/// </summary>
		/// <param name="storageStream">
		/// <see cref="IPAFStorageStream"/>
		/// </param>
		/// <param name="encoding">
		/// See <see cref="PAFReadChars(IPAFStorageStream,Encoding)"/>.
		/// </param>
		/// <returns>
		/// String decoded from a stream. Empty stream returns <see langword="null"/>.
		/// </returns>
		public static string PAFReadString(this IPAFStorageStream storageStream,
			Encoding encoding = null)
		{
			var chars = storageStream.PAFReadChars(encoding);
			if (chars == null) return null;
			return new string(Enumerable.ToArray(chars));
		}
		/// <summary>
		/// Writes a character set to the <see cref="IPAFStorageStream"/> with UNICODE
		/// encoding or a specified encoding. The stream is written at the current position.
		/// </summary>
		/// <param name="storageStream">
		/// <see cref="IPAFStorageStream"/>
		/// </param>
		/// <param name="charactersToWrite">
		/// The characters to write.
		/// </param>
		/// <param name="encoding">
		/// An instance of the <see cref="Encoding"/> class that specifies how byte
		/// to character mapping is to be performed. <see langword="null"/> will cause
		/// UTF8.
		/// </param>
		/// <exceptions>
		/// Various exceptions will be thrown if the implementation relies on native
		/// implementations. These exceptions should be documented and usually wrapped
		/// </exceptions>
		/// <threadsafety>
		/// This method cannot be relied upon to write safely after calling
		/// <see cref="IPAFStorageService.PAFAvailableFreeSpace"/>
		/// or similar methods, since another thread may have snuck in and done
		/// another allocation or write in the intervening time. Either
		/// <see cref="IPAFStorageStream.PAFSetLength"/> (with caught exception)
		/// or <see cref="IPAFStorageStream.PAFTrySetStorageSize"/> should be called
		/// to ensure that there is enough space on this stream to do the write.
		/// </threadsafety>
		public static void PAFWriteChars(this IPAFStorageStream storageStream, 
			IEnumerable<char> charactersToWrite, Encoding encoding = null)
		{
			if(charactersToWrite == null) return;
			var characterArray = charactersToWrite.ToArray();
			if (characterArray.Length == 0) return;
			if(encoding == null) encoding = new UTF8Encoding();
			var bytesEncoded = encoding.GetBytes(characterArray);
			storageStream.PAFWriteBytes(bytesEncoded, 0, bytesEncoded.Length);
		}
		/// <summary>
		/// Writes a string to the <see cref="IPAFStorageStream"/> with UNICODE
		/// encoding or a specified encoding. The stream is written at the current position.
		/// </summary>
		/// <param name="storageStream">
		/// <see cref="IPAFStorageStream"/>
		/// </param>
		/// <param name="stringToWrite">
		/// String to be written to a stream. <see langword="null"/> does
		/// nothing. <see cref="string.Empty"/> does nothing.
		/// </param>
		/// <param name="encoding">
		/// An instance of the <see cref="Encoding"/> class that specifies how byte
		/// to character mapping is to be performed. <see langword="null"/> will cause
		/// UniCode encoding to be used, Little-endian, no mark.
		/// </param>
		/// <exceptions>
		/// See <see cref="PAFReadChars(IPAFStorageStream,Encoding)"/>.
		/// </exceptions>
		/// <threadsafety>
		/// See <see cref="PAFReadChars(IPAFStorageStream,Encoding)"/>.
		/// </threadsafety>
		public static void PAFWriteString(this IPAFStorageStream storageStream,
			string stringToWrite, Encoding encoding = null)
		{
			if (string.IsNullOrEmpty(stringToWrite)) return;
			storageStream.PAFWriteChars(stringToWrite.ToCharArray(), encoding);
		}
	}
}
