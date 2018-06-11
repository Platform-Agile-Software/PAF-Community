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

namespace PlatformAgileFramework.TypeHandling.WeakBindings
{
	/// <summary>
	/// See <see cref="IPAFWeakableReference{T}"/>.
	/// </summary>
	/// <typeparam name="T">
	/// The type of the item referenced.
	/// </typeparam>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 26dec2017 </date>
	/// <description>
	/// Modified to implement the new interface.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> Brian T. </author>
	/// <date> 02sep2014 </date>
	/// <description>
	/// I changed the implementation to just wrap <see cref="WeakReference{T}"/>. I moved
	/// this into the platform-independent library, since we can now use it on phones.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> BMC </author>
	/// <date> 21aug2013 </date>
	/// <description>
	/// New.
	/// </description>
	/// </contribution>
	/// </history>
	public class PAFWeakReference<T> : IPAFWeakableReference<T> where T : class
	{
		#region Class Fields and AutoProperties
		/// <summary>
		/// Our wrapped weak reference.
		/// </summary>
		private readonly WeakReference<T> m_WeakReference;
		/// <summary>
		/// Our wrapped strong reference.
		/// </summary>
		private readonly T m_StrongReference;

		/// <summary>
		/// Backing...
		/// </summary>
		private readonly bool m_IsWeak;

		#endregion // Class Fields and AutoProperties

		#region Constructors
		/// <summary>
		/// Initializes a new instance of the <see cref="PAFWeakReference{T}" /> class, referencing the
		/// specified item.
		/// </summary>
		/// <param name="target">The item to track. </param>
		/// <param name="isWeak">
		/// Loads <see cref="IsWeak"/>. Pass <see langword="false"/> to hold a "strong"
		/// (normal) reference.
		/// </param>
		public PAFWeakReference(T target, bool isWeak = true)
		{
			if (isWeak)
			{
				m_WeakReference = new WeakReference<T>(target, false);
				m_IsWeak = true;
				return;
			}

			m_StrongReference = target;
			m_IsWeak = false;
		}
		#endregion // Constructors
		#region Properties
		/// <summary>
		/// <see cref="IPAFWeakableReference{T}"/>.
		/// </summary>
		public bool IsWeak
		{
			get { return m_IsWeak; }
		}
		/// <summary>
		/// <see cref="IPAFWeakableReference{T}"/>.
		/// </summary>
		/// <returns>
		/// <see cref="IPAFWeakableReference{T}"/>.
		/// </returns>
		/// <remarks>
		/// <see cref="IPAFWeakableReference{T}"/>.
		/// </remarks>
		public virtual T Target
		{
			get
			{
				if (!IsWeak)
					return m_StrongReference;
				return m_WeakReference.GetTarget();
			}
		}
		#endregion // Properties
	}

}