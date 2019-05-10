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
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.Collections.ExtensionMethods;
#endregion // Using Directives

namespace PlatformAgileFramework.FileAndIO
{
	/// <summary>
	/// Augmentation of <see cref="PAFFormattingFileDispatcher"/> for testing. This
	/// subclass captures the list of files that were dispatched.
	/// </summary>
	public class AuditingTestFileDispatcher : PAFFormattingFileDispatcher
	{
		#region Class Fields and AutoProperties
		/// <summary>
		/// Holder for the list of files dispatched.
		/// </summary>
		public IList<string> DispatchedFiles { get; set; }
		#endregion // Class Fields and AutoProperties
		#region Constructors
		/// <summary>
		/// Constructor just sets props and also staples in storage service.
		/// </summary>
		/// <param name="dispatchDirectory">Always required.</param>
		/// <param name="fileFormatter">
		/// See <see cref="IPAFFormattingFileDispatcher.FileFormatter"/>
		/// </param>
		public AuditingTestFileDispatcher(string dispatchDirectory,
			Func<string, string> fileFormatter = null):
		base(dispatchDirectory, fileFormatter)
		{
		}
		#endregion // Constructors
		#region IPAFFormattingFileDispatcher Implementation
		#region Methods
		/// <summary>
		/// <see cref="IPAFFormattingFileDispatcher"/>.
		/// </summary>
		/// <param name="filesToMove">
		/// <see cref="IPAFFormattingFileDispatcher"/>.
		/// </param>
		/// <remarks>
		/// <see cref="IPAFFormattingFileDispatcher"/>. This override just
		/// captures the file names for testing purposes. It loads
		/// <see cref="DispatchedFiles"/>.
		/// </remarks>
		public override void DispatchFiles(IList<string> filesToMove)
		{
			if (filesToMove.SafeCount() == 0)
				return;

			if (DispatchedFiles == null)
				DispatchedFiles = new List<string>();
			DispatchedFiles.AddItems(filesToMove);
			base.DispatchFiles(filesToMove);
		}
		#endregion // Methods
		#endregion // IPAFFormattingFileDispatcher Implementation
	}
}
