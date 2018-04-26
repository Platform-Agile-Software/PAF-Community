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
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System;
using PlatformAgileFramework.Collections;

namespace PlatformAgileFramework.Security
{
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
	public abstract class PAFSecretKeyProviderBase: IPAFSecretKey
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal object m_SecretKey;
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Just installs our key.
		/// </summary>
		/// <param name="secretKey">
		/// If <see langword="null"/>, a randomly-generated key is typically installed
		/// by a subclass.
		/// </param>
		protected PAFSecretKeyProviderBase(object secretKey)
		{
			m_SecretKey = secretKey;
		}
		#endregion // Constructors
		#region Properties
		/// <summary>
		/// Holds our key. Note that the secret key can be changed, but
		/// for value types, the key must be gotten, modified and boxed
		/// back into the Object.
		/// </summary>
		protected virtual object SecretKey
		{ get { return m_SecretKey; } set { m_SecretKey = value; } }
		#endregion // Properties
		#region Methods
		/// <summary>
		/// See <see cref="IPAFSecretKey"/>.
		/// </summary>
		/// <param name="secretKey">
		/// See <see cref="IPAFSecretKey"/>.
		/// </param>
		/// <returns>
		/// See <see cref="IPAFSecretKey"/>.
		/// </returns>
		/// <remarks>
		/// Abstract since we can't get an arbitrary payload from the object
		/// without knowing the type at load time. Could use reflection, but
		/// don't want to. Do this stuff in the derived Generic classes.
		/// </remarks>
		public abstract bool KeyMatches(object secretKey);
		#region IDeepCloneable Implementation
		/// <summary>
		/// See <see cref="IPAFDeepCloneable"/>.
		/// This method is abstract, since we are an abstract class and we
		/// can't produce one of ourselves.
		/// </summary>
		/// <returns>
		/// See <see cref="IPAFDeepCloneable"/>.
		/// </returns>
		/// <remarks>
		/// Return type is <see cref="IPAFSecretKey"/>.
		/// </remarks>
		public abstract object DeepClone();
		#endregion // IDeepCloneable Implementation
		#region Static Helper Methods
		/// <summary>
		/// Method determines whether a potentially matching key matches a supplied
		/// "template" key. 
		/// </summary>
		/// <param name="templateKey">
		/// <see langword="null"/> returns <see langword="false"/>. Not a <see cref="IPAFSecretKey"/>
		/// returns false.
		/// </param>
		/// <param name="keyThatNeedsToMatch">
		/// <see langword="null"/> returns <see langword="false"/>. Not a <see cref="IPAFSecretKey"/>
		/// returns false.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if keys are there and types match.
		/// </returns>
		/// <remarks>
		/// <para>
		/// If reference types are passed in to this routine (other than
		/// <see cref="IPAFSecretKey"/>'s), they must override
		/// <see cref="Object.Equals(Object)"/> so that different references containing
		/// identical data will return <see langword="true"/>.
		/// </para>
		/// <para>
		/// Framework extenders may wish to create a public method that calls this.
		/// </para>
		/// </remarks>
		public static bool KeysMatch(object templateKey, object keyThatNeedsToMatch)
		{
			if (templateKey == null) return false;
			var templateKeyProvider = templateKey as IPAFSecretKey;

			if (keyThatNeedsToMatch == null) return false;
			var keyThatNeedsToMatchKeyProvider = keyThatNeedsToMatch as IPAFSecretKey;

			if (templateKeyProvider != null)
				if (templateKeyProvider.KeyMatches(keyThatNeedsToMatch)) return true;

			if (keyThatNeedsToMatchKeyProvider != null)
				if (keyThatNeedsToMatchKeyProvider.KeyMatches(templateKey)) return true;

			// At this point, neither key is null and neither is a provider. We just
			// compare objects.
			if (templateKey.Equals(keyThatNeedsToMatch)) return true;
			return false;
		}
		#endregion // Static Helper Methods
		#endregion // Methods
	}
}



