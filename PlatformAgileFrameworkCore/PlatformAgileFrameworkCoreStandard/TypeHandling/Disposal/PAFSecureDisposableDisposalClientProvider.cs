//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2014 Icucom Corporation
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
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.Security;

namespace PlatformAgileFramework.TypeHandling.Disposal
{
	/// <summary>
	/// Default implementation of the interfaces. Note that this class is anticipated
	/// to be used as a non-public member variable on a type that wishes to employ it's
	/// functionality.
	/// </summary>
	/// <history>
	/// <author> KRM </author>
	/// <date> 04mar2012 </date>
	/// <contribution>
	/// Built so Clients can be easily provided WITH a secure key.
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Thread-safe if developers do not attempt reset of content
	/// during application operation.
	/// </threadsafety>
	public class PAFSecureDisposableDisposalClientProvider<T>
		: IPAFSecureDisposableDisposalClientProviderInternal<T>
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Holds our key.
		/// </summary>
		internal IPAFSecretKeyProviderInternal<T> SecretKeyProvider { get; set; }
		/// <summary>
		/// Holds our client.
		/// </summary>
		protected IPAFDisposableDisposalClientProvider DisposableDisposalClientProvider { get; set; }
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Just installs our client.
		/// </summary>
		/// <param name="disposalClient">
		/// See <see cref="IPAFDisposableDisposalClientProvider"/>.
		/// </param>
		/// <param name="secretKey">
		/// Can be "default(T)". This is considered no key at all.
		/// </param>
		public PAFSecureDisposableDisposalClientProvider(IDisposable disposalClient, T secretKey)
		{
			DisposableDisposalClientProvider = new PAFDisposableDisposalClientProvider(disposalClient);
			SecretKeyProvider = new PAFSecretKeyProvider<T>(null, true, secretKey);
		}
		#endregion // Constructors
		#region Properties
		/// <summary>
		/// Implementation of <see cref="IPAFDisposableDisposalClientProvider"/>
		/// </summary>
		public virtual IDisposable DisposableDisposalClient
		{
			get { return DisposableDisposalClientProvider.DisposableDisposalClient; }
		}
		/// <summary>
		/// Implementation of <see cref="IPAFDisposalClientProvider"/>
		/// </summary>
		public virtual object DisposalClient
		{
			get { return DisposableDisposalClientProvider.DisposalClient; }
		}
		#endregion // Properties
		#region Methods
		/// <summary>
		/// Implementation of <see cref="IPAFSecretKeyProviderInternal{T}"/>. This is the
		/// virtual method that should be overridden to change behavior.
		/// </summary>
		/// <returns>
		/// The secret key.
		/// </returns>
		protected virtual T GetSecretKey()
		{
			return SecretKeyProvider.GetSecretKeyInternal();
		}
		/// <summary>
		/// See <see cref="IPAFDeepCloneable"/>
		/// </summary>
		/// <returns>
		/// See <see cref="IPAFDeepCloneable"/>
		/// </returns>
		public object DeepClone()
		{
			return SecretKeyProvider.DeepClone();
		}
		/// <summary>
		/// See <see cref="IPAFSecretKey"/>
		/// </summary>
		/// <param name="secretKeyThatNeedsToMatch">
		/// See <see cref="IPAFSecretKey"/>
		/// </param>
		/// <returns>
		/// See <see cref="IPAFSecretKey"/>
		/// </returns>
		public bool KeyMatches(object secretKeyThatNeedsToMatch)
		{
			return SecretKeyProvider.KeyMatches(secretKeyThatNeedsToMatch);
		}
		#region Static Helpers
		/// <summary>
		/// Gets a secret key provider from an object implementing
		/// <see cref="IPAFDisposalClientProvider"/>.
		/// </summary>
		/// <param name="clientProvider">
		/// Can be <see langword="null"/>. In this case, a default key is installed
		/// in the provider that is returned. This is equivalent to no
		/// key at all.
		/// </param>
		/// <returns>
		/// A separate, independent copy of <see cref="IPAFSecretKey"/>
		/// if the key is <see cref="IPAFDeepCloneable"/> or is a value type
		/// that is copyable.
		/// </returns>
		internal static IPAFSecretKeyProviderInternal<T> GetKeyProviderFromClientProvider
			(IPAFDisposalClientProvider clientProvider)
		{
			var secretKeyProvider = clientProvider as IPAFSecretKeyProviderInternal<T>;
			if (secretKeyProvider == null)
				// We install the default key so we match anything - no security.
				return new PAFSecretKeyProvider<T>(null, true);
			T secretKey = secretKeyProvider.GetSecretKeyInternal();
			if (secretKey is IPAFDeepCloneable)
				secretKey = (T)((IPAFDeepCloneable) secretKey).DeepClone();
			return new PAFSecretKeyProvider<T>(null, false, secretKey);
		}
		#endregion // Static Helpers
		#endregion Methods
		#region Explicit Implementation of IPAFSecretKeyProvider
		/// <summary>
		/// See <see cref="IPAFSecretKeyProviderInternal{T}"/>.
		/// </summary>
		/// <returns>
		/// See <see cref="IPAFSecretKeyProviderInternal{T}"/>.
		/// </returns>
		T IPAFSecretKeyProviderInternal<T>.GetSecretKeyInternal()
		{
			return SecretKeyProvider.GetSecretKeyInternal();
		}
		#endregion // Explicit Implemantation of IPAFSecretKeyProvider
	}
}



