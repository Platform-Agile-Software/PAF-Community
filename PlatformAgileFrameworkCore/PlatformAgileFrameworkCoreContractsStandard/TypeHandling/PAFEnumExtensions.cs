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
using System.Collections.Generic;
using PlatformAgileFramework.Collections;

namespace PlatformAgileFramework.TypeHandling
{
	/// <summary>
	/// This class has a set of extension methods to help the <see cref="Enum"/> class.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 20aug2015 </date>s
	/// <description>
	/// Put in a patch to get around Enums not wearing "IConvertable" anymore in some
	/// of the PCL profiles we are using. Created history section - looks like a good class, don't
	/// know who did it.
	/// </description>
	/// </contribution>
	/// </history>
	public static class PAFEnumExtensions
	{
		/// <summary>
		/// This method determines whether all bits in one enum <paramref name="otherEnum"/>"/>
		/// are present in the current enum (this). This method is generally useful only when
		/// the enums involved wear the "flags" attribute.
		/// </summary>
		/// <param name="thisEnum">
		/// this
		/// </param>
		/// <param name="otherEnum">
		/// The other <see cref="Enum"/> whose bitfields need to be overlaid.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if "this" has at least the bits <see langword="true"/> as
		/// <paramref name="otherEnum"/>.
		/// </returns>
		public static bool Covers(this Enum thisEnum, Enum otherEnum)
		{
			// TODO PAF Exception with resource string.
			if (thisEnum.GetType() != otherEnum.GetType())
			{
				throw new ArgumentException("Argument_EnumTypeDoesNotMatch: "
					+ otherEnum.GetType(), " != " + thisEnum.GetType());
			}

			// Int64 wide enough to handle any.
			//var otherInt = ((IConvertible) otherEnum).ToUInt64(null);
			var otherInt = long.Parse(otherEnum.ToString());
			//var thisInt = ((IConvertible) thisEnum).ToUInt64(null);
			var thisInt = long.Parse(thisEnum.ToString());
			return ((thisInt & otherInt) == otherInt);
		}
		
		/// <summary>
		/// Same idea as <see cref="Covers"/>, but just checks a string
		/// to see if it matches any of the Enum names that correspond to
		/// <see langword="true"/> bits. ordinal case-sensitive comparison.
		/// </summary>
		/// <param name="thisEnum">this</param>
		/// <param name="str">String to check</param>
		/// <returns><see langword="true"/> if string is found.</returns>
		public static bool CoversName(this Enum thisEnum, string str)
		{
			return thisEnum.GetEnumStrings().StringIsPresent(str);
		}

		/// <summary>
		/// This method converts an <see cref="Enum"/> to an <see cref="long"/>. This
		/// should work for everything we use. It converts by outputting a string value,
		/// then parsing into an integer.
		/// </summary>
		/// <param name="thisEnum">
		/// this
		/// </param>
		/// <returns>
		/// Integer value.
		/// </returns>
		public static long ToInt64(this Enum thisEnum)
		{
			var thisInt = long.Parse(thisEnum.ToString());
			return thisInt;
		}

		/// <summary>
		/// This method returns a set of strings corresponding to the bits that
		/// are <see langword="true"/> in the Enum. This method is generally useful only when
		/// the enums involved wear the "flags" attribute. This method is just a
		/// convenience to turn the strings returned from <see cref="Enum.ToString(string)"/>
		/// into a list.
		/// </summary>
		/// <param name="thisEnum">
		/// this
		/// </param>
		/// <returns>
		/// List of strings - never <see langword="null"/>. If no bits are <see langword="true"/>
		/// and the value of 0 is not mapped to a string, "0" is returned as
		/// the single entry in the list. Strings are trimmed of whitespace.
		/// </returns>
		public static IList<string> GetEnumStrings(this Enum thisEnum)
		{
			// "F" gets the strings separated by commas.
			var commaSeparatedEnumStrings = thisEnum.ToString("F");
			var enumStrings = commaSeparatedEnumStrings.Split(',');
			var list = new List<string>();
			foreach (var str in enumStrings)
			{
				// whitespace is a problem.
				var str1 = str.Trim();
				list.Add(str1);
			}
			return list;
		}
	}

	/// <summary>
	/// This class has a set of extension methods to help the <see cref="Enum"/> class.
	/// </summary>
	public class PAFEnumEx
	{
		/// <summary>
		/// This method does a case-sensitive parsing of an <see cref="Enum"/>. It was
		/// created to replace the parse method Microsoft left off Silverlight. It is
		/// named thusly for clarity. Best to just go through and rename all uses of the
		/// two-argument parse method to use this. No ifdef's, no cross-platform
		/// problems, no sweat...
		/// </summary>
		/// <param name="enumType">
		/// Type of the <see cref="Enum"/> we are to create.
		/// </param>
		/// <param name="value">
		/// <see cref="string"/>-ful representation of the Enum.
		/// </param>
		/// <exception cref="ArgumentException">
		/// // TODO
		/// </exception>
		/// <returns>
		/// The <see cref="Enum"/>.
		/// </returns>
		public static object ParseCaseSensitive(Type enumType, string value)
		{
			// TODO PAF Exception with resource string.
			return Enum.Parse(enumType, value, false);
		}
	}
}
