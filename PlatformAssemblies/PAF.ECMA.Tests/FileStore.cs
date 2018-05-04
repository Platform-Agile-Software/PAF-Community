using System;
using System.Collections.Generic;
using System.IO;
using PlatformAgileFramework.FileAndIO.FileAndDirectoryService;

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.FileAndIO
{
	/// <summary>
	/// Class that loads and stores text data. Basically just a test class.
	/// </summary>
	public class FileStore
	{
		// Variables .
		/// <summary>
		/// Rooted directory path with filename at the end
		/// </summary>
		private static string s_FilePath;
		/// <summary>
		/// (Leaf) file name where we put our stuff.
		/// </summary>
		private const string FILE_NAME = "FileStoreTestFile";

		/// <summary>
		/// Rooted directory without trailing slash.
		/// </summary>
		private static string s_FileDir = Environment.CurrentDirectory;
		/// <summary>
		/// Our test dir on the phone.
		/// </summary>
		private const string VENDOR_PATH = "";
		/// <summary>
		/// Working list of strings.
		/// </summary>
		private readonly IList<string> m_FileData = new List<string>();
		/// <summary>
		/// The storage service we test in this class.
		/// </summary>
		protected internal IPAFStorageService m_StorageService;
		/// <summary>
		/// Just staples in the storage service.
		/// </summary>
		/// <param name="storageService">Storage service.</param>
		public FileStore(IPAFStorageService storageService)
		{
			m_StorageService = storageService;			
		}

		protected internal virtual void Initialize()
		{
				string dir = Path.Combine(s_FileDir, VENDOR_PATH);

				if (!Directory.Exists(dir))
                {
					Directory.CreateDirectory(dir);
				}
			//var file = File.Create("krup");

				s_FilePath = Path.Combine(dir, FILE_NAME);
		}

		public virtual IList<string> RetrieveDataFromFile()
		{
			Initialize();
			// Kill the data we have stored.
			m_FileData.Clear();
			if (!File.Exists(s_FilePath)) return null;
			IList<string> stringsInFile;
			stringsInFile = new List<string>();
			var stream = m_StorageService.PAFOpenFile(s_FilePath, PAFFileAccessMode.READONLY);
			string str;
			while((str = stream.PAFReadString()) != null)
				m_FileData.Add(str);
			stream.Dispose();
			foreach (string lineEntry in stringsInFile)
			{
				m_FileData.Add(lineEntry);
			}
			return m_FileData;
		}

		/// <summary>
		/// Saves the supplied text lines to a file.
		/// </summary>
		/// <param name="fileDataToBeSaved">
		/// Data to be saved to a file.
		/// </param>
		public virtual void SaveData(IList<string> fileDataToBeSaved)
		{
			Initialize();
			// We write the file fresh each time.
			if (File.Exists(s_FilePath))
					File.Delete(s_FilePath);
			if (fileDataToBeSaved == null) return;

			m_FileData.Clear ();
			foreach (var str in fileDataToBeSaved)
			{
				m_FileData.Add (str);
			}

			//using (var file = File.AppendText(s_FilePath))
			using (var fileStream = m_StorageService.PAFOpenFile(s_FilePath))
			{
				foreach (var strng in m_FileData)
				{
					fileStream.PAFWriteString(strng);
				}
			}
		}
	}
}
