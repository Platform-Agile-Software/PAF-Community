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

using System;
using System.Security;
using PlatformAgileFramework.Security;
// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.TypeHandling.Disposal
{
	/// <summary>
	/// Static class to test disposal stuff.
	/// </summary>
	/// <summary>
	/// Test class to show how the disposal mechanism is to be used.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 16mar2019 </date>
	/// <description>
	/// New. Putting sanitized tests for the disposal mechanism
	/// into PAF Community for release.
	/// </description>
	/// </contribution>
	/// </history>
	public static class StaticClass2
	{
		/// <summary>
		/// An object that will always be null to cause an exception.
		/// </summary>
		public static object s_DisposableObject;

		/// <summary>
		/// Nested class to do the disposing. This wouldn't have to be nested,
		/// but it's easier to package it this way. Wears a variety of interfaces
		/// to demonstrate various disposal features.
		/// </summary>
		internal sealed class DisposerClass2 : IDisposable,
			IPAFDisposableDisposalClientProvider, IPAFSecretKeyProviderInternal<Guid>,
			IDisposeableInternal
		{
			#region Fields and Autoprops
			/// <summary>
			/// Every single class that is disposable should have a guard.
			/// </summary>
			internal volatile bool m_IsDisposed;
			/// <summary>
			/// This will be the default Guid if never set, which is
			/// always a valid Guid for disposal. This must be set
			/// to a proper (non-default) Guid for secure disposal.
			/// </summary>
			/// <remarks>
			/// Internal visibility for manipulation by a test fixture.
			/// </remarks>
			internal static Guid s_DisposalGuid;
			#endregion // Fields and Autoprops
			internal DisposerClass2(Guid disposalGuid)
			{
				s_DisposalGuid = disposalGuid;
			}

			/// <summary>
			/// This is where we release resources that we really care
			/// about on <see cref="StaticClass1"/>.
			/// </summary>
			private void ReleaseUnmanagedResources()
			{
				// release unmanaged resources here
			}
			public void Dispose()
			{
				// This is the normal (non-test) code.
				Dispose(true);
				GC.SuppressFinalize(this);
			}
			/// <summary>
			/// Private visibility because it's just for us.
			/// </summary>
			/// <param name="hasDisposeBeenCalled">
			/// Will be set if not finalizing.
			/// </param>
			private void Dispose(bool hasDisposeBeenCalled)
			{
				if (m_IsDisposed)
					return;

				// Set it before we do anything, since we may fail.
				m_IsDisposed = true;

				if (hasDisposeBeenCalled)
				{
					// Do managed stuff here.
					// Do something to generate an exception, just for the test.
					((IDisposable)s_DisposableObject).Dispose();
				}
				ReleaseUnmanagedResources();
			}
			/// <summary>
			/// A call through interop or otherwise through a class that
			/// does not get finalized upon shutdown can be inserted here
			/// to advise of any undisposed classes at application or
			/// app domain shutdown time. We are hooked in to the disposal
			/// system, so we don't need this.
			/// </summary>
			~DisposerClass2()
			{
				ReleaseUnmanagedResources();
			}
			#region IDisposableInternal Implementation
			/// <summary>
			/// Internal (secure) path to the <see cref="IDisposable.Dispose"/>
			/// method.
			/// </summary>
			/// <remarks>
			/// security safe critical to allow calling dispose.
			/// </remarks>
			[SecuritySafeCritical]
			void IDisposeableInternal.DisposeInternal()
			{
				Dispose();
			}
			#endregion // IDisposableInternal Implementation
			#region Implementation of IPAFSecretKeyProviderInternal<Guid>
			/// <summary>
			/// <see cref="IPAFSecretKeyProviderInternal{T}"/>.
			/// Implementations of internal interfaces are almost always
			/// explicit.
			/// </summary>
			/// <returns>
			/// <see cref="IPAFSecretKeyProviderInternal{T}"/>.
			/// </returns>
			Guid IPAFSecretKeyProviderInternal<Guid>.GetSecretKeyInternal()
			{
				return s_DisposalGuid;
			}
			#region Implementation of IPAFSecretKey
			#region Implementation of IDeepCloneable
			/// <summary>
			/// <see cref="IPAFSecretKey"/>.
			/// We don't clone in this implementation.
			/// </summary>
			/// <returns>
			/// <see langword="null"/> in this implementation.
			/// </returns>
			public object DeepClone()
			{
				return null;
			}
			#endregion // Implementation of IDeepCloneable
			/// <summary>
			/// <see cref="IPAFSecretKey"/>.
			/// </summary>
			/// <returns>
			/// <see cref="IPAFSecretKey"/>.
			/// </returns>
			public bool KeyMatches(object secretKeyThatNeedsToMatch)
			{
				if (secretKeyThatNeedsToMatch == null)
					return false;
				if (secretKeyThatNeedsToMatch.GetType() != typeof(Guid))
					return false;
				return ((Guid)secretKeyThatNeedsToMatch) == s_DisposalGuid;
			}
			#endregion // Implementation of IPAFSecretKey
			#endregion // Implementation of IPAFSecretKeyProviderInternal<Guid>
			#region Implementation of IPAFDisposableDisposalClientProvider
			#region Implementation of IPAFDisposalClientProvider
			/// <summary>
			/// <see cref="IPAFDisposableDisposalClientProvider"/>
			/// </summary>
			public IDisposable DisposableDisposalClient
			{ get => this; }
			#endregion // Implementation of IPAFDisposalClientProvider
			/// <summary>
			/// <see cref="IPAFDisposableDisposalClientProvider"/>
			/// </summary>
			public object DisposalClient
			{ get => this; }
			#endregion // Implementation of IPAFDisposableDisposalClientProvider
		}
	}
}
