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
using PlatformAgileFramework.Collections;

namespace PlatformAgileFramework.Security
{
	#region Delegates
	/// <summary>
	/// This delegate is responsible for generating a random key of type
	/// <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type of the random key.</typeparam>
	/// <returns>The key - never "default{T}"</returns>
	public delegate T RandomKeyGenerator<out T>();
	#endregion // Delegates
	/// <summary>
	/// Default implementation of the interface.
	/// </summary>
	/// <history>
	/// <author> DAP </author>
	/// <date> 29dec2011 </date>
	/// <contribution>
	/// New implementation for SL.
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Thread-safe.
	/// </threadsafety>
	/// <exceptions>
	/// <exception> <see cref="InvalidOperationException"/> is thrown if
	/// a <see cref="RandomKeyGeneratorDelegate"/> has not been installed.
	/// "RandomKeyGeneratorDelegate not installed"
	/// </exception>
	/// </exceptions>
	/// <remarks>
	/// Closed subclasses are often implemented as internal or private in
	/// order to keep them from being discovered through reflection in
	/// low-trust environments.
	/// </remarks>
	public class PAFSecretKeyProvider<T> : PAFSecretKeyProviderBase,
		IPAFSecretKeyProviderInternal<T>
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Backing.
		/// </summary>
		private RandomKeyGenerator<T> m_RandomKeyGeneratorDelegate;
		#endregion //Class Fields and Autoproperties
		#region Constructors

		/// <summary>
		/// Just installs our key.
		/// </summary>
		/// <param name="randomKeyGeneratorMethod">
		/// Key generator plugin. Mandatory if <typeparamref name="T"/> is
		/// "default{T}".
		/// </param>
		/// <param name="installDefaultKey">
		/// If <see langword="true"/>, a default key is allowed. This is equivalent to
		/// no key.
		/// </param>
		/// <param name="secretKey">
		/// If "default(T)" and <paramref name="installDefaultKey"/> is <see langword="false"/>, a
		/// random key generator should be installed.
		/// </param>
		/// <exceptions>
		/// <exception> <see cref="InvalidOperationException"/> is thrown if
		/// <paramref name="installDefaultKey"/> is <see langword="false"/> and
		/// a <see cref="RandomKeyGeneratorDelegate"/> has not been installed.
		/// "RandomKeyGeneratorDelegate not installed"
		/// </exception>
		/// </exceptions>
		public PAFSecretKeyProvider(RandomKeyGenerator<T> randomKeyGeneratorMethod,
			bool installDefaultKey = false, T secretKey = default(T)) :base(secretKey)
		{
			m_RandomKeyGeneratorDelegate = randomKeyGeneratorMethod;
			if((secretKey.Equals(default(T))) && (!installDefaultKey))
			{
				if(randomKeyGeneratorMethod == null)
					throw new InvalidOperationException("RandomKeyGeneratorMethod not Installed");
				secretKey = GetRandomKey();
			}
			m_SecretKey = secretKey;
		}
		#endregion // Constructors
		#region Properties
		/// <summary>
		/// Accessor for the generator delegate.
		/// </summary>
		protected virtual RandomKeyGenerator<T> RandomKeyGeneratorDelegate
		{
			get { return m_RandomKeyGeneratorDelegate; }
			set { m_RandomKeyGeneratorDelegate = value; }
		}
		#endregion // Properties
		#region Methods
		#region Novel methods
		/// <summary>
		/// Gets a random key.
		/// </summary>
		/// <returns>The random secret key.</returns>
		/// <exceptions>
		/// <exception> <see cref="InvalidOperationException"/> is thrown if
		/// a <see cref="RandomKeyGeneratorDelegate"/> has not been installed.
		/// "RandomKeyGeneratorDelegate not installed"
		/// </exception>
		/// </exceptions>
		protected T GetRandomKey()
		{
			if(RandomKeyGeneratorDelegate == null)
				throw new InvalidOperationException("RandomKeyGeneratorDelegate not installed");
			return RandomKeyGeneratorDelegate();
		}
		#endregion // Novel Methods
		/// <summary>
		/// See <see cref="IPAFSecretKeyProviderInternal{T}"/>.
		/// </summary>
		/// <returns>
		/// See <see cref="IPAFSecretKeyProviderInternal{T}"/>.
		/// </returns>
		protected virtual T GetSecretKey()
		{
			return (T)SecretKey;
		}
		/// <summary>
		/// See <see cref="IPAFSecretKey"/>.
		/// </summary>
		/// <param name="secretKeyThatNeedsToMatch">
		/// See <see cref="IPAFSecretKey"/>.
		/// </param>
		/// <returns>
		/// See <see cref="IPAFSecretKey"/>.
		/// </returns>
		public override bool KeyMatches(object secretKeyThatNeedsToMatch)
		{
			if (GetSecretKey().Equals(default(T))) return true;
			// Note that we must pass our actual key, not ourselves as a
			// provider (this), to avoid infinite recursion with
			// KeysMatch.
			if(KeysMatch(GetSecretKey(), secretKeyThatNeedsToMatch)) return true;
			return false;
		}
		#region IDeepCloneable Implementation
		/// <summary>
		/// This method handles the case when the  <see cref="GetSecretKey()"/> is
		/// "default(T)" or it implements <see cref="IPAFDeepCloneable"/>.
		/// </summary>
		/// <returns>
		/// A provider containing "default(T)" if the <see cref="GetSecretKey()"/> is
		/// <see langword="null"/> or containing a deep clone of <see cref="GetSecretKey()"/>
		/// if the key is a <see cref="IPAFDeepCloneable"/>. Other cases are
		/// specifc to a subclass. A return value of <see langword="null"/> indicates that
		/// the clone must be handled by a subclass. The reason that value
		/// types are not simply copied is because they may themselves hold
		/// reference types in their fields. There is no way to determine
		/// that except by reflection on ALL fields, including non-public
		/// ones. This is not possible in low-trust environments and we
		/// wouldn't want to do it anyway. Thus closed Generic subclasses
		/// must handle the clone.
		/// </returns>
		/// <remarks>
		/// Return type is <see cref="IPAFSecretKey"/>.
		/// </remarks>
		public override object DeepClone()
		{
			if (default(T).Equals(SecretKey))
				return new PAFSecretKeyProvider<T>(RandomKeyGeneratorDelegate, true);
			var deepCloneable = SecretKey as IPAFDeepCloneable;
			if(deepCloneable != null)
				return new PAFSecretKeyProvider<T>(RandomKeyGeneratorDelegate,
					false, (T) deepCloneable);
			return null;
		}
		#endregion // IDeepCloneable Implementation
		#region Explicit Implementation of IPAFSecretKeyProviderInternal
		/// <summary>
		/// See <see cref="IPAFSecretKeyProviderInternal{T}"/>.
		/// </summary>
		/// <returns>
		/// See <see cref="IPAFSecretKeyProviderInternal{T}"/>.
		/// </returns>
		T IPAFSecretKeyProviderInternal<T>.GetSecretKeyInternal()
		{
			return GetSecretKey();
		}
		#endregion // Explicit Implementation of IPAFSecretKeyProviderInternal
		#endregion // Methods
	}
}



