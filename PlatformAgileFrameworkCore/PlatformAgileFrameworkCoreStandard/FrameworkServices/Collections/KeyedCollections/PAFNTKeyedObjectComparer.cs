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
using System.Collections.Generic;
using PlatformAgileFramework.Collections.Comparers;

#endregion

namespace PlatformAgileFramework.Collections.KeyedCollections
{
	/// <summary>
	/// This class implements a comparision of <see cref="IPAFNameAndTypeKeyedObject"/>s.
	/// It pulls the key <see cref="IPAFNamedAndTypedObject"/> off <typeparamref name="T"/> and compares
	/// the keys.
	/// </summary>
	/// <typeparam name="T">
	/// Any reference type implementing <see cref="IPAFNameAndTypeKeyedObject"/>.
	/// </typeparam>
	// ReSharper disable once InconsistentNaming
	public class PAFNTKeyedObjectComparer<T> : AbstractComparerShell<T>
		where T : IPAFNameAndTypeKeyedObject
	{
		#region Class AutoProperties
		/// <summary>
		/// Gives access to our wrapped comparer.
		/// </summary>
		protected PAFNameAndTypeComparer NAndTComparer { get; }
		#endregion // Class AutoProperties
		#region Constructors
		/// <summary>
		/// Default constructor just creates our wrapped copy of <see cref="PAFNameAndTypeComparer"/>.
		/// Sets for case-insensitive comparison.
		/// </summary>
		public PAFNTKeyedObjectComparer()
		{
			NAndTComparer = new PAFNameAndTypeComparer();
		}
		/// <summary>
		/// Constructor allows case-insensitive comparison to be set.
		/// </summary>
		/// <param name="ignoreCase">
		/// <see langword="true"/> for case-insensitive comparison.
		/// </param>
		public PAFNTKeyedObjectComparer(bool ignoreCase)
		{
			NAndTComparer = new PAFNameAndTypeComparer(ignoreCase);
		}
		#endregion // Constructors
		/// <summary>
		/// Compares the two items according to their <see cref="IPAFNamedAndTypedObject"/> key.
		/// Internally uses <see cref="PAFNameAndTypeComparer"/>.
		/// </summary>
		/// <param name="firstItem">
		/// The first <typeparamref name="T"/> to compare.
		/// </param>
		/// <param name="secondItem">
		/// The second <typeparamref name="T"/> to compare.
		/// </param>
		/// <returns>
		/// See <see cref="IComparer{T}"/>
		/// </returns>
		/// <exceptions>
		/// <exception> <see cref="ArgumentNullException"/> is thrown if
		/// either parameter is <see langword="null"/>.
		/// </exception>
		/// <exception> <see cref="ArgumentException"/> is thrown if
		/// key of either parameter is <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		/// <remarks>
		/// <see cref="IPAFNamedObject.ObjectName"/> of either key
		/// can be <see langword="null"/>. <see langword="null"/> is ordered before non-<see langword="null"/>.
		/// </remarks>
		public override int DefaultMainCompare(T firstItem, T secondItem)
		{
// ReSharper disable CompareNonConstrainedGenericWithNull
			if (firstItem == null)
				throw new ArgumentNullException("firstItem");
			if (secondItem == null)
				throw new ArgumentNullException("secondItem");
// ReSharper restore CompareNonConstrainedGenericWithNull
			if (firstItem.GetItemKey() == null)
				throw new ArgumentException("firstItem.Key is null");
			if (secondItem.GetItemKey() == null)
				throw new ArgumentException("secondItem.Key is null");
			return NAndTComparer.DefaultMainCompare(firstItem.GetItemKey(), secondItem.GetItemKey());
		}
	}
}
