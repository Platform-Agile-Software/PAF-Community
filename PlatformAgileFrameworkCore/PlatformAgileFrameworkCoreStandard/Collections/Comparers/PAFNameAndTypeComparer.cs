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
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-
#region Using directives

using System;
using System.Collections.Generic;

#endregion

namespace PlatformAgileFramework.Collections.Comparers
{
	/// <summary>
	/// This class implements a comparison of <see cref="IPAFNamedAndTypedObject"/>s.
	/// </summary>
	public class PAFNameAndTypeComparer : AbstractComparerShell<IPAFNamedAndTypedObject>
	{
		#region Class Autoproperties
		/// <summary>
		/// Determines whether a case-insensitive comparision is made on
		/// the name.
		/// </summary>
		public bool IgnoreCase { get; protected internal set; }
		#endregion // Class Autoproperties
		#region Constructors
		/// <summary>
		/// Default constructor builds with case-insensitive comparison.
		/// </summary>
		public PAFNameAndTypeComparer()
		{
			IgnoreCase = true;
		}
		/// <summary>
		/// Constructor allows case-sensitive comparison to be set.
		/// </summary>
		/// <param name="ignoreCase">
		/// <see langword="true"/> for case-insensitive comparison.
		/// </param>
		public PAFNameAndTypeComparer(bool ignoreCase)
		{
			IgnoreCase = ignoreCase;
		}
		#endregion // Constructors
		/// <summary>
		/// First compares the <see cref="IPAFNamedAndTypedObject.ObjectType"/>
		/// <see cref="Type.FullName"/> then, if those are the same, compares the
		/// <see cref="IPAFNamedObject.ObjectName"/>
		/// </summary>
		/// <param name="firstKey">
		/// The first <see cref="IPAFNamedAndTypedObject"/> to compare.
		/// </param>
		/// <param name="secondKey">
		/// The other <see cref="IPAFNamedAndTypedObject"/> to compare.
		/// </param>
		/// <returns>
		/// See <see cref="IComparer{T}"/>
		/// </returns>
		/// <exceptions>
		/// <exception> <see cref="ArgumentException"/> is thrown if
		/// <see cref="IPAFNamedAndTypedObject.ObjectType"/> of either parameter
		/// is <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		/// <remarks>
		/// <para>
		/// <see cref="IPAFNamedObject.ObjectName"/> of either argument
		/// can be <see langword="null"/>. <see langword="null"/> is ordered before non-<see langword="null"/>.
		/// </para>
		/// <para>
		/// Noted that a comparison is being made of KEYS. An object can
		/// implement <see cref="IPAFNamedAndTypedObject"/> directly, or
		/// it can have the key as a property, in which case this comparer
		/// should be wrapped in another that extracts the key and sends it
		/// to us.
		/// </para>
		/// </remarks>
		public override int DefaultMainCompare(IPAFNamedAndTypedObject firstKey,
			IPAFNamedAndTypedObject secondKey)
		{
			if (firstKey.ObjectType == null)
				throw new ArgumentException("firstKey.ObjectType is null");
			if (secondKey.ObjectType == null)
				throw new ArgumentException("secondKey.ObjectType is null");
			var strcmp = string.CompareOrdinal(firstKey.ObjectType.FullName, secondKey.ObjectType.FullName);
			if(strcmp != 0) return strcmp;
			// Handle degenerate cases.
			if ((firstKey.ObjectName == null) && (secondKey.ObjectName == null)) return 0;
			if (firstKey.ObjectName == null) return 1;
			if (secondKey.ObjectName == null) return -1;
			//
			if(IgnoreCase)
				return string.Compare(firstKey.ObjectName, secondKey.ObjectName, StringComparison.OrdinalIgnoreCase);
			return string.Compare(firstKey.ObjectName, secondKey.ObjectName, StringComparison.Ordinal);
		}
	}
}
