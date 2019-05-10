//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2019 Icucom Corporation
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

#region Using Directives
using System;
using System.Collections.Generic;
using System.Net;
using PlatformAgileFramework.FileAndIO.FileAndDirectoryService;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.Platform;
#endregion // Using Directives

namespace PlatformAgileFramework.FileAndIO
{
	/// <summary>
	/// Default implementation of <see cref="IPAFFormattingFileDispatcher"/>.
	/// </summary>
	public class PAFFormattingFileDispatcher : IPAFFormattingFileDispatcher
	{
		/// <summary>
		/// Internal backing for testing.
		/// </summary>
		internal Func<string, string> m_FileFormatter;
		/// <summary>
		/// Internal backing for testing.
		/// </summary>
		/// <summary>
		/// Stapled in at construction time for high-speed operation.
		/// </summary>
		protected readonly IPAFStorageService m_StorageService;
		internal string m_DispatchDirectory;
		#region Constructors
		/// <summary>
		/// Constructor just sets props and also staples in storage service.
		/// </summary>
		/// <param name="dispatchDirectory">Always required.</param>
		/// <param name="fileFormatter">
		/// See <see cref="FileFormatter"/>
		/// </param>
		public PAFFormattingFileDispatcher(string dispatchDirectory,
			Func<string, string> fileFormatter = null)
		{
			m_StorageService
				= PAFServiceManagerContainer.ServiceManager.GetTypedService<IPAFStorageService>();
			m_DispatchDirectory = dispatchDirectory;
			m_FileFormatter = fileFormatter;
		}
		#endregion // Constructors
		#region IPAFFormattingFileDispatcher Implementation
		/// <summary>
		/// <see cref="IPAFFormattingFileDispatcher"/>.
		/// </summary>
		public virtual Func<string, string> FileFormatter
		{
			get { return m_FileFormatter; }
			protected set { m_FileFormatter = value; }
		}
		/// <summary>
		/// <see cref="IPAFFormattingFileDispatcher"/>.
		/// </summary>
		public virtual string DispatchDirectory
		{
			get { return m_DispatchDirectory; }
			protected set { m_DispatchDirectory = value; }
		}
		/// <summary>
		/// Utility method that is accessed OUTSIDE OF <see cref="IPAFFormattingFileDispatcher"/>
		/// for security.
		/// </summary>
		/// <param name="dispatchDirectory">Sets <see cref="DispatchDirectory"/>.</param>
		public virtual void SetDispatchDirectory(string dispatchDirectory)
		{
			m_DispatchDirectory = dispatchDirectory;
		}
		#region Methods
		/// <summary>
		/// <see cref="IPAFFormattingFileDispatcher"/>.
		/// This implementation acts by killing any directory on the front end of
		/// incoming files, then attaching <see cref="DispatchDirectory"/> to the
		/// front and simply moving the file to the new place. The file in
		/// the old place is deleted. The files in the dispatch directory could be
		/// transmitted to a server in an asynchronous fashion, for example. This
		/// was the original motivation for two clients. It is best to monitor dispatch
		/// directories and create new ones if the existing ones are not empty or
		/// otherwise prevent multiple transmissions.
		/// </summary>
		/// <param name="filesToMove">
		/// <see cref="IPAFFormattingFileDispatcher"/>.
		/// <see lngword="null"/> just returns.
		/// </param>
		/// <remarks>
		/// <see cref="IPAFFormattingFileDispatcher"/>.
		/// </remarks>
		public virtual void DispatchFiles(IList<string> filesToMove)
		{
			if (filesToMove == null)
				return;

			var sep = PlatformUtils.GetDirectorySeparatorChar();

			foreach (var filePath in filesToMove)
			{
				if (filePath == null)
					continue;

				var newFilePath = DispatchDirectory
					+ sep + PAFFileUtils.KillDirectory(filePath);

				// Customer specs demand that partially complete files be dispatched.
				// The problem is that writer may still be writing to the same
				// file, so this file needs to be repeatedly transferred as it
				// is filled.
				if (m_StorageService.PAFFileExists(newFilePath))
				{
					m_StorageService.PAFDeleteFile(newFilePath);
				}


				if (FileFormatter == null)
				{
					m_StorageService.PAFMoveFile(filePath, newFilePath);

					return;
				}

				// Grab the contents of the file, then delete it.
				string fileContents;
				using (var stream = m_StorageService.PAFOpenFile(filePath))
				{
					fileContents = stream.PAFReadString();
				}
				m_StorageService.PAFDeleteFile(filePath);

				// Format the file, then send it out.
				var outputFileContents = FileFormatter(fileContents);
				using (var stream = m_StorageService.PAFOpenFile(newFilePath,
					PAFFileAccessMode.REPLACE))
				{
					stream.PAFWriteString(outputFileContents);
				}
			}
		}
		#endregion // Methods
		#endregion // IPAFFormattingFileDispatcher Implementation
	}
}
