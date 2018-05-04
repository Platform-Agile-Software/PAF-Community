using System;
using System.Collections.Generic;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.ErrorAndException.CoreCustomExceptions
{
	/// <summary>
	///	Exceptions that occur during file and directory operations.
	/// </summary>
	[PAFSerializable]
	public interface IPAFFileAndDirectoryIOExceptionData
		: IPAFStandardExceptionData
	{
		#region Properties
		/// <summary>
		/// The problematic file path.
		/// </summary>
		string BadIOFilePath { get; }
		#endregion // Properties
	}

	/// <summary>
	/// Set of tags with an enumerator for exception messages. These are the dictionary keys
	/// for extended.
	/// </summary>
	public class PAFFileAndIOExceptionMessageTags
		: PAFExceptionMessageTagsBase<IPAFFileAndDirectoryIOExceptionData>
	{
		/// <summary>
		/// Error message. General wrapper exception for a variety of problems
		/// deleting a directory.
		/// </summary>
		public const string ERROR_DELETING_DIRECTORY = "Error deleting directory";

		/// <summary>
		/// Error message. General wrapper exception for a variety of problems
		/// deleting a file.
		/// </summary>
		public const string ERROR_DELETING_FILE = "Error deleting file";

		/// <summary>
		/// Error message. Issued when a directory is missing. Use this
		/// when we are specifically looking for a directory.
		/// </summary>
		public const string DIRECTORY_NOT_FOUND = "Directory not found";

		/// <summary>
		/// Error message. Usually wraps a thrown exception when permissions
		/// are not granted for directory creation.
		/// </summary>
		public const string ERROR_CREATING_DIRECTORY = "Error creating directory";

		/// <summary>
		/// Error message.
		/// </summary>
		public const string FILE_IS_LOCKED = "File is locked";

		/// <summary>
		/// Error message. Path is there, but not file.
		/// </summary>
		public const string FILE_NOT_FOUND = "File Not Found";

		/// <summary>
		/// Error message. Path is not found. Use this when we
		/// don't know whether it is a directory or a file we
		/// are looking for. Also for when we are looking for
		/// a file, but the directory associated with the file
		/// is not even there.
		/// </summary>
		public const string PATH_NOT_FOUND = "Path not found";

		/// <summary>
		/// Just puts the tags in a list to hand out.
		/// </summary>
		static PAFFileAndIOExceptionMessageTags()
		{
			if ((s_Tags != null) && (s_Tags.Count > 0)) return;
			s_Tags = new List<string>
				{
					ERROR_DELETING_FILE,
					ERROR_CREATING_DIRECTORY,
					ERROR_DELETING_DIRECTORY,
					DIRECTORY_NOT_FOUND,
					FILE_IS_LOCKED,
					FILE_NOT_FOUND,
					PATH_NOT_FOUND,
				};
            // Always store by the interface for an aggregable exception.
            PAFAbstractStandardExceptionDataBase.RegisterNamedExceptionTagsInternal(s_Tags,
                typeof(IPAFFileAndDirectoryIOExceptionData));

		}

	}
}
