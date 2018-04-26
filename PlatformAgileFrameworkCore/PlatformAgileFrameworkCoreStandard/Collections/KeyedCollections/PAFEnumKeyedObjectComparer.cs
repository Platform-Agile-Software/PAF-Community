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
using PlatformAgileFramework.Collections.Comparers;

#endregion

namespace PlatformAgileFramework.Collections.KeyedCollections
{
	/// <summary>
	/// This class implements a comparison of <see cref="IPAFEnumKeyedObject"/>s.
	/// </summary>
	public class PAFEnumKeyedObjectComparer : AbstractComparerShell<IPAFEnumKeyedObject>
	{
		#region Fields and Autoproperties
		/// <summary>
		/// The type of the <see cref="Enum"/>.
		/// </summary>
		public Type m_EnumType;
		/// <summary>
		/// Determines whether a case-insensitive comparision is made on
		/// the Enum string if string indexing is used.
		/// </summary>
		public bool IgnoreCase { get; protected internal set; }
		/// <summary>
		/// Determines whether an enum's integer value is used for
		/// the indexing. If <see langword="false"/>, string indexing is used.
		/// </summary>
		public bool m_ValueMode;
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Default constructor builds with string comparison enabled
		/// with case-insensitive comparison.
		/// </summary>
		/// <param name="enumType">
		/// The type of the Enum.
		/// </param>
		public PAFEnumKeyedObjectComparer(Type enumType)
		{
			EnumType = enumType;
			IgnoreCase = true;
		}
		/// <summary>
		/// Constructor allows indexing mode and case-sensitivity to be set.
		/// </summary>
		/// <param name="enumType">
		/// The type of the Enum.
		/// </param>
		/// <param name="valueMode">
		/// <see langword="true"/> if the Enum's integer value is to be used for indexing.
		/// </param>
		/// <param name="ignoreCase">
		/// <see langword="true"/> for case-insensitive comparison.
		/// </param>
		public PAFEnumKeyedObjectComparer(Type enumType, bool valueMode, bool ignoreCase)
			:this(enumType)
		{
			ValueMode = valueMode;
			IgnoreCase = ignoreCase;
		}
		#endregion // Constructors

		#region Properties
		/// <summary>
		/// Determines whether an enum's integer value is used for
		/// the indexing. If <see langword="false"/>, string indexing is used.
		/// </summary>
		public bool ValueMode
		{
			get { return m_ValueMode; }
			protected internal set
			{
				// TODO exception for repeated integer values if mode is int.
				m_ValueMode = value;
			}
		}
		/// <summary>
		/// The type of the <see cref="Enum"/>.
		/// </summary>
		public Type EnumType
		{
			get { return m_EnumType; }
			protected internal set
			{
				// TODO exception for non-enum type.
				m_EnumType = value;
			}
		}
		#endregion // Properties
		#region Methods

		/// <summary>
		/// Compares the <see cref="Enum"/>'s integer values or their string
		/// values, depending on the setting.
		/// </summary>
		/// <param name="firstKey">
		/// The first <see cref="IPAFEnumKeyedObject"/> to compare.
		/// </param>
		/// <param name="secondKey">
		/// The other <see cref="IPAFEnumKeyedObject"/> to compare.
		/// </param>
		/// <returns>
		/// See <see cref="IComparer{T}"/>
		/// </returns>
		public override int DefaultMainCompare(IPAFEnumKeyedObject firstKey,
			IPAFEnumKeyedObject secondKey)
		{
			// TODO arg null exceptions.
			if (ValueMode)
			{
				return firstKey.GetItemEnumKey().CompareTo(secondKey.GetItemEnumKey());
			}

			var symbol1 = firstKey.ToString();
			var symbol2 = secondKey.ToString();

			//
			if(IgnoreCase)
				return string.Compare(symbol1, symbol2, StringComparison.OrdinalIgnoreCase);
			return string.Compare(symbol1, symbol2, StringComparison.Ordinal);
		}
		#endregion // Methods
	}
}
