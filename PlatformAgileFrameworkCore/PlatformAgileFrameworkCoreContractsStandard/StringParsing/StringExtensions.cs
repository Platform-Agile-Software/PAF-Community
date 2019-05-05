//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2018 Icucom Corporation
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
using System.Linq;
using PlatformAgileFramework.Platform;

namespace PlatformAgileFramework.StringParsing
{
	/// <summary>
	/// Few extension methods for strings.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 21jan2018 </date>
	/// <description>
	/// Added DOCs and added EnsureTerminator.
	/// </description>
	/// </contribution>
	/// </history>
	public static class StringExtensions
	{
		/// <summary>
		/// Breaks up a string into two parts - one before a separator and one after.
		/// </summary>
		/// <param name="stringToParse">
		/// The string to be parsed.
		/// </param>
		/// <param name="separatorChar">
		/// Character to look for separating the parts of the string Default is a comma.
		/// strings. 
		/// </param>
		/// <param name="nthOccurrence">
		/// Must be greater than 0. Default is 1.
		/// </param>
		/// <returns>
		/// The two strings on either side of the found separator. <see langword="null"/> if separator
		/// not found. Elements may be <see langword="null"/> if separator is either at the beginning or
		/// end of the string.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// If <paramref name="nthOccurrence"/> is less than 1.
		/// </exception>
		public static IList<string> BreakStringInTwo(
			this string stringToParse, char separatorChar = ',',
			int nthOccurrence = 1)
		{
			var index = IndexOfNth(stringToParse, separatorChar, nthOccurrence);
			if (index == -1) return null;
			var list = new List<string>(new string[] {null, null});

			// First part.
			if (index == 0) list[0] = null;
			else list[0] = stringToParse.Substring(0, index);

			// Second part.
			if (index == stringToParse.Length) list[1] = null;
			else list[1] = stringToParse.Substring(index + 1);
			return list;
		}
        /// <summary>
        /// This method ensures that there is a <see cref="PlatformUtils.LTRMN"/>
        /// at the end of a string.
        /// </summary>
        /// <param name="stringToExamine">
        /// The string to be checked for a terminator. <see langword="null"/> just returns
        /// a terminator.
        /// </param>
        /// <returns>
        /// String with a terminator at the end.
        /// </returns>
        public static string EnsureTerminator(this string stringToExamine)
        {
            if (string.IsNullOrEmpty(stringToExamine))
                return PlatformUtils.LTRMN;
	        if (!stringToExamine.EndsWith(PlatformUtils.LTRMN, StringComparison.Ordinal))
	        {
		        return stringToExamine + PlatformUtils.LTRMN;
	        }

	        return stringToExamine;
        }
		/// <summary>
		/// This method finds the nth occurence of a character in a string.
		/// </summary>
		/// <param name="stringToParse">
		/// The string to be parsed.
		/// </param>
		/// <param name="searchChar">
		/// Character to look for Default is a comma. We use comma to break apart comma-delimited
		/// strings. 
		/// </param>
		/// <param name="nthOccurrence">
		/// Must be greater than 0. Default is 1.
		/// </param>
		/// <returns>
		/// The 0-based index at which the occurence is found. -1 for not
		/// found.
		/// </returns>
		/// <exception cref="ArgumentOutOfRangeException">
		/// If <paramref name="nthOccurrence"/> is less than 1.
		/// </exception>
		public static int IndexOfNth(this string stringToParse, char searchChar = ',', int nthOccurrence = 1)
		{
			if (string.IsNullOrEmpty(stringToParse)) return -1;
			if (nthOccurrence < 1) throw new ArgumentOutOfRangeException("nthOccurence = " + nthOccurrence);
			// Working index is always on the separator or potential separator.
			var indexOfNth = -1;
			do
			{
				// Ran out of string?
				if (stringToParse.Length < indexOfNth + 2) return -1;
				indexOfNth = stringToParse.IndexOf(searchChar, indexOfNth + 1);
				nthOccurrence--;
			} while (nthOccurrence > 0);
			return indexOfNth;
		}

		/// <summary>
		/// Removes the terminators and sends out the segments of the string that
		/// were separated by the terminators.
		/// </summary>
		/// <returns>
		/// The list of separated strings - never <see langword = "null"/>. Will
		/// send out a single-element list if no terminators.
		/// </returns>
		/// <param name="stringWithPossibleTerminators">
		/// String with possible terminators.
		/// </param>
		/// <exceptions>
		/// <exception cref = "ArgumentNullException">
		/// <paramref name="stringWithPossibleTerminators"/>
		/// </exception>
		/// </exceptions>
		public static IList<string> RemoveTerminators(this string stringWithPossibleTerminators)
		{
			if (stringWithPossibleTerminators == null)
				throw new ArgumentNullException(nameof(stringWithPossibleTerminators));
			if (!stringWithPossibleTerminators.Contains(PlatformUtils.LTRMN))
				return new List<string>(new [] { stringWithPossibleTerminators });

			var strings = stringWithPossibleTerminators.Split(PlatformUtils.LTRMN.ToCharArray());

			// return an actual writable list.
			return strings.ToList();
		}
		/// <summary>
		/// Finds a string on the end of "this" string after the last occurrence
		/// of another string within "this" string.
		/// </summary>
		/// <returns>
		/// The string found. <see langword="null"/> if the string is not found
		/// or <paramref name="stringToFind"/> is <see langword="null"/>.
		/// </returns>
		/// <param name="stringToCheck">
		/// String with possible included string.
		/// </param>
		/// <param name="stringToFind">
		/// This is the string to find within the <paramref name="stringToCheck"/>.
		/// </param>
		/// <exceptions>
		/// <exception cref = "ArgumentNullException">
		/// <paramref name="stringToCheck"/>
		/// </exception>
		/// </exceptions>
		public static string SafeStringBeyondLastString(
			this string stringToCheck, string stringToFind)
		{
			if (stringToCheck == null)
				throw new ArgumentNullException(nameof(stringToCheck));

			if (stringToFind == null)
				return null;

			var index = stringToCheck.LastIndexOf(stringToFind, StringComparison.Ordinal);

			// Find it?
			if (index < 0)
				return null;

			var lengthOfStringToFind = stringToFind.Length;
			var targetStartIndex = index + lengthOfStringToFind;
			if (targetStartIndex > (stringToCheck.Length - 1))
				return null;

			return stringToCheck.Substring(targetStartIndex);
		}

		///// <summary>
		///// Replaces certain substrings with other substrings, if found.
		///// </summary>
		///// <returns>
		///// A string with the substrings replaced.
		///// </returns>
		///// <param name="stringWithPossibleSubstrings">
		///// </param>
		///// <param name="searchString">
		///// String to look for.
		///// </param>
		///// <param name="replaceString">
		///// String to replace every instance of a found substring.
		///// </param>
		///// <exceptions>
		///// <exception cref = "ArgumentNullException">
		///// "stringWithPossibleSubstrings"
		///// </exception>
		///// </exceptions>
		///// <remarks>
		///// Needed due to a bad bug in netstandard string class.
		///// </remarks>
		/////Note: KRM - bug was fixed in 2.0 use .Net facilities.
		//public static string ReplaceStringsInString(this string stringWithPossibleSubstrings,
		//	string searchString, string replaceString)
	}
}
