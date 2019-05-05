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
	/// <summary>
	/// Interface allowing the determination of whether a provider holds a key.
	/// The pattern often used is to first check on the existance of a key.
	/// If there is none, then the security check fails if a key is required.
	/// </summary>
	/// <history>
	/// <author> DAP </author>
	/// <date> 29dec2011 </date>
	/// <contribution>
	/// Updated to have both internal and public interfaces for SL conversion.
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Readonly - safe.
	/// </threadsafety>
	/// <remarks>
	/// <para>
	/// This interface derives from <see cref="IPAFDeepCloneable"/> so
	/// that implementations are forced to offer deep copies of
	/// themselves. For security reasons, references should not be passed.
	/// Instead, copies should be made.
	/// </para>
	/// <para>
	/// Implementations should normally ensure that deep copies of the
	/// key be made. If the key is a <see cref="ValueType"/> with no
	/// reference type fields, this is not a problem. Otherwise make sure
	/// that an independant copy is made. As an example, if a byte array
	/// is used as the key, wrap it in a type that implements
	/// <see cref="IPAFDeepCloneable"/>. Implementations should normally
	/// ensure that keys wearing <see cref="IPAFDeepCloneable"/> are cloned
	/// with this interface. The type returned from the deepclone method
	/// must be a <see cref="IPAFDeepCloneable"/>.
	/// </para>
	/// <para>
	/// This interface deals only with <see cref="object"/>'s to provide
	/// security. Any key that is a <see cref="ValueType"/> should override
	/// <see cref="ValueType.Equals(object)"/> if it contains any fields
	/// that are reference types. Otherwise comparisons are fast.
	/// </para>
	/// </remarks>
	public interface IPAFSecretKeyProvider: IPAFDeepCloneable
	{
		#region Methods
		/// <summary>
		/// Tells if a supplied key matches the one we hold. If the one we hold is
		/// the default value for the type, the method returns <see langword="true"/>.
		/// </summary>
		/// <param name="secretKeyThatNeedsToMatch">
		/// The key to check.
		/// </param>
		/// <returns><see langword="true"/> if key matches.</returns>
		/// <remarks>
		/// If the secret key is a value type containing any reference fields,
		/// its <see cref="object.Equals(object)"/> method must be overriden
		/// for this method to work properly. Something like a <see cref="Guid"/>
		/// doesn't have this problem, since <see cref="ValueType"/>'s
		/// <see cref="object.Equals(object)"/> override will just do
		/// a memcmp on the datastructure. Curiously, <see cref="Guid"/>
		/// overrides <see cref="object.Equals(object)"/> anyway.
		/// </remarks>
		bool KeyMatches(object secretKeyThatNeedsToMatch);
		#endregion // Methods
	}
}

