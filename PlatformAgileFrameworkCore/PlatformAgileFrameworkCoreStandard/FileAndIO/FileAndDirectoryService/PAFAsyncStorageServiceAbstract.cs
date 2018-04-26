using System;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using PlatformAgileFramework.MultiProcessing.Tasking;

namespace PlatformAgileFramework.FileAndIO.FileAndDirectoryService
{
	/// <summary>
	/// This implementation typically employs standard stuff from <c>File</c>
	/// and <c>Directory</c>, which are exposed fully on ECMA and now also on
	/// Xamarin.Android and Xamarin.iOS. That is why we have some defaults in
	/// here assuming a wide-open file/directory system.
	/// </summary>
	/// <remarks>
	/// As usual, we employ explicit interface implementation with virtual
	/// backing methods for extenders. This class just adds the async stuff.
	/// The methods in this class do not throw or generate exceptions, since
	/// exceptional conditions will be platform-specific. Methods will throw
	/// or encapsulate exceptions thrown or generated in the implementation.  
	/// </remarks>
	/// <threadsafety>
	/// Depends on the implementation.
	/// </threadsafety>
	/// <history>
	/// <contribution>
	/// <author> Bello </author>
	/// <date> 12mar2016 </date>
	/// <description>
	/// New. Built this when TPL got fully functional.
	/// </description>
	/// </contribution>
	/// </history>
	public abstract class PAFAsyncStorageServiceAbstract : PAFStorageServiceAbstract
		, IPAFAsyncStorageService
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Saves some code.
		/// </summary>
		protected internal readonly IPAFAsyncStorageService AsIasyncstorage;
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// This default constructor just sets some needed fields.
		/// </summary>
		protected PAFAsyncStorageServiceAbstract()
		{
			AsIasyncstorage = this;
		}
		#endregion // Constructors


		#region Implementation of IPAFAsyncStorageService
		/// <remarks>
		/// See <see cref="IPAFAsyncStorageService"/>.
		/// </remarks>
		Task<IPAFStorageStream> IPAFAsyncStorageService.PAFOpenFileAsync(string path)
		{
			return PAFOpenFileAsyncPV(path, -1);
		}

		/// <remarks>
		/// See <see cref="IPAFAsyncStorageService"/>.
		/// </remarks>
		Task<IPAFStorageStream> IPAFAsyncStorageService.PAFOpenFileAsync(string path, int timeoutInMilliseconds)
		{
			return PAFOpenFileAsyncPV(path, timeoutInMilliseconds);
		}

		/// <remarks>
		/// Support for <see cref="IPAFAsyncStorageService"/>.
		/// </remarks>
		protected virtual async Task<IPAFStorageStream> PAFOpenFileAsyncPV(string path, int timeoutInMilliseconds)
		{
			IPAFStorageStream stream = null;

			var fileTask = Task<IPAFStorageStream>.Factory.StartNew(
				() =>
				{
					stream = PAFOpenFilePIV(path, PAFFileAccessMode.READONLY);
					return stream;
				});

			var taskCompletedFirst
				= await TaskUtils.WaitAnyWithTimeoutAsync(new[] {fileTask}, timeoutInMilliseconds);

			// If we get file task completed first, all is well, relatively speaking.......
			if (taskCompletedFirst == 0)
			{
				return stream;
			}

			// Here we will throw a standard .Net exception for a timeout.
			throw new TimeoutException();
		}

		#endregion
	}
}
