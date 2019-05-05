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

#region Using Directives
using System;
using System.Collections.Generic;
using PlatformAgileFramework.Collections.Comparers;
#endregion // Using Directives


namespace PlatformAgileFramework.TypeHandling.PartialClassSupport
{
	/// <summary>
	/// This class implements a comparison of <see cref="ExtendablePseudoEnum{T}"/>s.
	/// We compare the Enumeration type. This is designed to be used in the
	/// outer dictionary of a two-level pseudoenumvalue dictionary.
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
	public class ExtendablePseudoEnumTypeComparer
		: AbstractComparerShell<IExtendablePseudoEnumTypeType>
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
		/// We handle nulls - two nulls produces 0, first null and second
		/// non-null produces -1, first non-null and second null
		/// produces 1.
		/// <remarks>
		/// </remarks>
		public override int DefaultMainCompare(IExtendablePseudoEnumTypeType firstKey,
			IExtendablePseudoEnumTypeType secondKey)
		{
			if ((firstKey == null) && (secondKey == null))
				return 0;
			if ((firstKey == null) && (secondKey != null))
				return -1 ;
			if ((firstKey != null) && (secondKey == null))
				return 1 ;
			var firstEnumType = firstKey.EnumType;
			var secondEnumType = secondKey.EnumType;

			if (secondEnumType.GetHashCode() == firstEnumType.GetHashCode())
				return 0;
			if (secondEnumType.GetHashCode() > firstEnumType.GetHashCode())
				return -1;
			return 1;
		}

		/// <summary>
		/// This override gets the hash code of the enumeration type, because
		/// that is what we are sorting by.
		/// </summary>
		/// <param name="obj">
		/// A <see cref="IExtendablePseudoEnumTypeType"/>.
		/// </param>
		/// <returns>The hash code of the enumeration type.</returns>
		/// <exceptions>
		/// <exception cref="ArgumentException">
		/// "Not a 'IExtendablePseudoEnumTypeType'"
		/// </exception>
		/// </exceptions>
		/// Note that we must handle nulls, so we return the (unique) hash code for
		/// the typeof TYPE in that case.
		/// <remarks>
		/// </remarks>
		public override int GetHashCode(object obj)
		{
			if (obj == null)
				return typeof(Type).GetHashCode ();
			var isPEV = obj is IExtendablePseudoEnumTypeType;
			if (!isPEV) throw new ArgumentException("Not a IExtendablePseudoEnumTypeType'");

			// Type hash code is what we sort on.
			var pev = (IExtendablePseudoEnumTypeType)obj;
			var enumType = pev.EnumType;
			var hashCode = enumType.GetHashCode ();
			return hashCode;
		}
	}
}
