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

#region Using Directives
using System;
using System.Collections.Generic;
using PlatformAgileFramework.Collections.Comparers;
#endregion // Using Directives


namespace PlatformAgileFramework.TypeHandling.PartialClassSupport
{
	/// <summary>
	/// This class implements a comparison of <see cref="ExtendablePseudoEnum{T}"/>s.
	/// We compare the string name of the enumeration value, which is unique
	/// within an enum type. This is designed to be used in the
	/// inner dictionary of a two-level pseudoenumvalue dictionary.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 20aug2015 </date>
	/// <description>
	/// Built for the rewrite of the extendable enum stuff.
	/// </description>
	/// </contribution>
	/// </history>
	public class PseudoEnumValueComparer : AbstractComparerShell<IExtendablePseudoEnumTypeType>
	{
		/// <summary>
		/// Compares the <see cref="IExtendablePseudoEnumTypeType"/>s.
		/// </summary>
		/// <param name="firstKey">
		/// The first <see cref="IExtendablePseudoEnumTypeType"/> to compare.
		/// </param>
		/// <param name="secondKey">
		/// The other <see cref="IExtendablePseudoEnumTypeType"/> to compare.
		/// </param>
		/// <returns>
		/// See <see cref="IComparer{T}"/>
		/// </returns>
		public override int DefaultMainCompare(IExtendablePseudoEnumTypeType firstKey,
			IExtendablePseudoEnumTypeType secondKey)
		{
			var firstEnumName = firstKey.Name;
			var secondEnumName = secondKey.Name;

			var comparison = string.CompareOrdinal(firstEnumName, secondEnumName);

			if (comparison == 0)
				return 0;
			if (comparison < 0)
				return -1;
			return 1;
		}

		/// <summary>
		/// This override gets the hash code of the enumeration name, because
		/// that is what we are sorting by.
		/// </summary>
		/// <param name="obj">
		/// A <see cref="IExtendablePseudoEnumTypeType"/>.
		/// </param>
		/// <returns>The hash code of the name string.</returns>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">
		/// "obj"
		/// </exception>
		/// <exception cref="ArgumentException">
		/// "Not a 'IExtendablePseudoEnumTypeType'"
		/// </exception>
		/// </exceptions>
		public override int GetHashCode(object obj)
		{
			if (obj == null) throw new ArgumentNullException("obj");
			var isPEV = obj is IExtendablePseudoEnumTypeType;
			if (!isPEV) throw new ArgumentException("Not a IExtendablePseudoEnumTypeType'");

			// Type hash code is what we sort on.
			var pev = (IExtendablePseudoEnumTypeType)obj;
			var enumName = pev.Name;
			return enumName.GetHashCode();
		}
	}
}
