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
#endregion // Using Directives
using System;
namespace PlatformAgileFramework.FileAndIO
{
	/// <summary>
	/// Add on for a file writer that has to "dispatch" files to some
	/// place other than the directory it is writing to.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 23feb2019 </date>
	/// <description>
	/// New. Needed this augmentation for a couple of clients.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Implementations are EXPECTED to be thread-safe.
	/// </threadsafety>
	/// <remarks>
	/// Nothing exposed in this interface for security reasons. Internals
	/// are fed through the constructor.
	/// </remarks>
	public interface IPAFFileDispatcher
	{
		#region Methods
		/// <summary>
		/// This method is here to unify locking for ALL
		/// dispatch operations. In a concurrent scenario,
		/// this must be called BEFORE <see cref="DispatchFilesIfNeeded"/>
		/// is called and must be disposed after. This mechanism was
		/// introduced to help programmers remember to dispose the
		/// lock by using a <see langword="using"/> block.
		/// </summary>
		/// <returns>
		/// Disposable which MUST be called after the dispatch method.
		/// </returns>
		IDisposable GetDisposableLock();
		/// <summary>
		/// Somehow moves the files to another place, typically emptying the original directory.
		/// </summary>
		void DispatchFilesIfNeeded();
		#endregion Methods
	}
}