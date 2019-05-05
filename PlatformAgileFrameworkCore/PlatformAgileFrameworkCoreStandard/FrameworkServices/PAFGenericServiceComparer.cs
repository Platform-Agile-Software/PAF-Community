//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010-2011 Icucom Corporation
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
#region Using directives

using System;
using PlatformAgileFramework.Collections.Comparers;

#endregion

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// This class implements a comparison of <see cref="IPAFServiceDescription{T}"/>s.
	/// We compare the interface type and the name. This is just a wrapper on the base
	/// class.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> DAV </author>
	/// <date> 21jun2012 </date>
	/// <description>
	/// Built this type-safe comparer to work with the new service dictionary.
	/// </description>
	/// </contribution>
	/// </history>
	public class PAFServiceComparer<T> : PAFServiceComparer,
		IAllComparers<IPAFServiceDescription<T>>
		where T: class, IPAFService
	{
		#region Constructors
		/// <summary>
		/// See <see cref="PAFServiceComparer"/>.
		/// </summary>
		public PAFServiceComparer()
		{
			IgnoreCase = true;
		}
		/// <summary>
		/// See <see cref="PAFServiceComparer"/>.
		/// </summary>
		/// <param name="ignoreCase">
		/// See <see cref="PAFServiceComparer"/>.
		/// </param>
		public PAFServiceComparer(bool ignoreCase)
			:base(ignoreCase)
		{
			IgnoreCase = ignoreCase;
		}
		#endregion // Constructors
		/// <remarks>
		/// Delegates to <see cref="PAFServiceComparer"/>.
		/// </remarks>
		/// <exceptions>
		/// See exceptions on base class.
		/// </exceptions>
		public virtual int DefaultMainCompare(IPAFServiceDescription<T> firstKey,
			IPAFServiceDescription<T> secondKey)
		{
			return base.DefaultMainCompare(firstKey, secondKey);
		}

		/// <remarks>
		/// Delegates to <see cref="PAFServiceComparer"/>.
		/// </remarks>
		public virtual int Compare(IPAFServiceDescription<T> firstKey, IPAFServiceDescription<T> secondKey)
		{
			return base.Compare(firstKey, secondKey);
		}

		/// <remarks>
		/// Delegates to <see cref="PAFServiceComparer"/>.
		/// </remarks>
		public virtual bool Equals(IPAFServiceDescription<T> firstKey, IPAFServiceDescription<T> secondKey)
		{
			return base.Equals(firstKey, secondKey);
		}

		/// <remarks>
		/// Delegates to <see cref="PAFServiceComparer"/>.
		/// </remarks>
		public virtual int GetHashCode(IPAFServiceDescription<T> description)
		{
			return base.GetHashCode(description);
		}
	}
}
