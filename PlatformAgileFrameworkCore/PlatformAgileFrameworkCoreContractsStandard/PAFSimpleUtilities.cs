//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2017 Icucom Corporation
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
using System.Text;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework
{
	/// <summary>
	/// This class is a utility class for various mathematical tools and formulas
	/// used in PAF services.
	/// </summary>
	public class PAFSimpleUtilities
	{
		#region Methods
		/// <summary>
		/// Method to determine if a number is "almost" greater than or equal
		/// to another number. This is useful when a number might have been
		/// subjected to machine roundoff and the caller wishes to determine
		/// whether a number is greater than or equal to a threshold after a
		/// "delta" has been added to the threshold.
		/// </summary>
		/// <param name="inputValue">The input number.</param>
		/// <param name="threshold">
		/// The threshold to test against.
		/// </param>
		/// <param name="thresholdDelta">
		/// </param>
		/// The number to add to the threshold to determine whether the input value
		/// is greater or equal to it. This parameter can be positive or negative.
		/// <returns>
		/// true if the input is GEQ than the threshold plus delta.
		/// </returns>
		public static bool DeltaGEQ(double inputValue, double threshold,
					double thresholdDelta)
		{
			if (inputValue > threshold + thresholdDelta) {
				return true;
			}

			return false;
		}

		/// <summary>
		/// Method to determine if a number is "almost" less than or equal
		/// to another number. This is useful when a number might have been
		/// subjected to machine roundoff and the caller wishes to determine
		/// whether a number is less than or equal to a threshold after a
		/// "delta" has been added to the threshold.
		/// </summary>
		/// <param name="inputValue">The input number.</param>
		/// <param name="threshold">
		/// The threshold to test against.
		/// </param>
		/// <param name="thresholdDelta">
		/// The number to add to the threshold to determine whether the
		/// input value is less or equal to it. This parameter can be positive
		/// or negative.
		/// </param>
		/// <returns>
		/// true if the input is LEQ than the threshold plus delta.
		/// </returns>
		public static bool DeltaLEQ(double inputValue, double threshold,
					double thresholdDelta)
		{
			if (inputValue < threshold + thresholdDelta) {
				return true;
			}

			return false;
		}

		/// <summary>
		/// Method to determine which one of a number of bands a number falls in.
		/// </summary>
		/// <param name="inputValue">The input number.</param>
		/// <param name="bandArray">
		/// An array containing the alternating start/end points of the numBands
		/// bands to check. The array is 2*numBands long. The bands are assumed
		/// to be disjoint and the start is assumed to be lower than the end of
		/// each band.
		/// </param>
		/// <param name="numBands">The number of bands to check</param>
		/// <param name="lowerThresholdDelta">
		/// The number to add to the start point of each band to determine whether
		/// the input value falls into it. This is useful for numbers that have
		/// incurred machine roundoff to determine if the number is "almost" in the
		/// band. This number can be negative or positive.
		/// </param>
		/// <param name="upperThresholdDelta">
		/// The number to add to the end point of each band to determine whether
		/// the input value falls into it.
		/// </param>
		/// <returns>
		/// The band number (0 - numBands-1) that the <paramref name="inputValue"/>  falls
		/// in or -1 for no band.
		/// </returns>
		public static int BandNumber(double inputValue, double[] bandArray,
					int numBands, double lowerThresholdDelta,
					double upperThresholdDelta)
		{
			for (var bandNum = 0; bandNum < numBands; bandNum++) {
				if (DeltaGEQ(inputValue, bandArray[2 * bandNum], lowerThresholdDelta)
					&&
					DeltaLEQ(inputValue, bandArray[2 * bandNum + 1], upperThresholdDelta)
				   ) {
					// If we found it, go home.
					return bandNum;
				}
			}

			// Not in any band.
			return -1;
		}

		/// <summary>
		/// Method to determine if an integer is contained in an integer array and return
		/// the position (index) in the array at which it was found.
		/// </summary>
		/// <param name="numToFind">The input number to locate in the array.</param>
		/// <param name="array">
		/// An array to search through for the number.
		/// </param>
		/// <param name="searchLength">
		/// The portion of the array to search through, if not the whole array. Set this
		/// to -1 to search the whole array.
		/// </param>
		/// <returns>
		/// The zero-based index in the array at which the number is found. -1 is
		/// returned if the number is not in the array.
		/// </returns>
		public static int IndexInArray(int numToFind, int[] array, int searchLength)
		{
			// Full search if negative
			if (searchLength == -1)
				searchLength = int.MaxValue;

			for (var i = 0; i < array.Length; i++) {
				// If we ran past the search length, get out because it's not here.
				if (i >= searchLength)
					return -1;

				// Get out because we found it.
				if (numToFind == array[i])
					return i;
			}

			// We searched the whole array and it's not here.
			return -1;
		}
		/// <summary>
		/// Method to determine if there is a change between two boolean values and
		/// in which direction.
		/// </summary>
		/// <param name="oldValue">The original value.</param>
		/// <param name="newValue">The new value.</param>
		/// <returns>
		/// <c>1</c> if the change is from <see langword="false"/> to <see langword="true"/>,
		/// <c>-1</c> if the change is from <see langword="true"/> to <see langword="false"/>,
		/// 0 for no change.
		/// </returns>
		public static int BooleanChange(bool oldValue, bool newValue)
		{
			if (oldValue && !newValue)
				return -1;
			if (!oldValue && newValue)
				return 1;
			return 0;
		}

		#endregion
	}

	/// <summary>
	/// This class is a utility class for various mathematical constants and functions.
	/// </summary>
	public class PAFMath
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// A positive float value 256 times the minimum positive number.
		/// </summary>
		/// <remarks>
		/// <see cref="float"/>s are not normally used for signal processing ANALYSIS,
		/// but when we want to emulate a <see cref="float"/> IMPLEMENTATION. 
		/// </remarks>
		public static readonly float s_TinyPositiveFloat;
		/// <summary>
		/// A negative float value 256 times larger in magnitude than the minimum
		/// negative number.
		/// </summary>
		public static readonly float s_TinyNegativeFloat;
		/// <summary>
		/// A positive double value 1024 times the minimum positive number.
		/// </summary>
		public static readonly double s_TinyPositiveDouble;
		/// <summary>
		/// A negative double value 1024 times larger in magnitude than the minimum
		/// negative number.
		/// </summary>
		public static readonly double s_TinyNegativeDouble;
		#endregion //  Class Fields and Autoproperties
		/// <summary>
		/// Static constructor initializes fields.
		/// </summary>
		static PAFMath()
		{
			s_TinyPositiveDouble = double.Epsilon * 1024;
			s_TinyNegativeDouble = -s_TinyPositiveDouble;
			s_TinyPositiveFloat = float.Epsilon * 256;
			s_TinyNegativeFloat = -s_TinyPositiveFloat;
		}

		#region Methods
		/// <summary>
		/// Method finds the smallest power of 2 that is greater than or
		/// equal to a given number.
		/// </summary>
		/// <param name="number">The number to process.</param>
		/// <returns>The power of two.</returns>
		/// <exceptions>
		/// <exception>
		/// <see cref="ArgumentException"/> is thrown if the <see paramref="number"/>
		/// is less than or equal to 0.
		/// "Number &lt;= 0".
		/// </exception>
		/// </exceptions>
		public static int CeilPow2(int number)
		{
			if (number == 0) throw new ArgumentException("Number <= 0");
			if (number == 1) return 0;
			var pow = 1;
			do {
				pow = pow * 2;
			} while (pow < number);
			return pow;
		}
		/// <summary>
		/// Similar to <see cref="CeilPow2"/> excepts calculates an all-1's mask that is larger
		/// or equal to the number.
		/// </summary>
		/// <param name="number">The number to process.</param>
		/// <returns>The power of two minus one.</returns>
		/// <exceptions>
		/// <exception>
		/// <see cref="ArgumentException"/> is thrown if the <see paramref="number"/>
		/// is less than or equal to 0.
		/// "Number &lt;= 0".
		/// </exception>
		/// </exceptions>
		public static int CeilPow2Mask(int number)
		{
			if (number <= 0) throw new ArgumentException("Number <= 0");
			if (number == 1) return 0;
			var pow = 1;
			do {
				pow = pow * 2;
			} while (pow - 1 < number);
			return pow - 1;
		}
		/// <summary>
		/// This method determines if two arbitraray <see cref="Enum"/>'s are equal.
		/// </summary>
		/// <param name="enum1">The first Enum.</param>
		/// <param name="enum2">The first Enum.</param>
		/// <returns><see langword="true"/> if the enums have bits exactly equal.</returns>
		public static bool EnumsEqual(Enum enum1, Enum enum2)
		{
			var uInt1 = enum1.ToInt64();
			var uInt2 = enum2.ToInt64();
			return (uInt1 == uInt2);

		}
		/// <summary>
		/// This method determines if two arbitraray <see cref="Enum"/>'s intersect.
		/// </summary>
		/// <param name="enum1">The first Enum.</param>
		/// <param name="enum2">The first Enum.</param>
		/// <returns><see langword="true"/> if the enums have bits in common.</returns>
		public static bool EnumsIntersect(Enum enum1, Enum enum2)
		{
			var uInt1 = enum1.ToInt64();
			var uInt2 = enum2.ToInt64();
			return (uInt1 & uInt2) != 0;

		}
		/// <summary>
		/// A "safe" logarithm that also performs a "floor" operation on the input.
		/// </summary>
		/// <param name="inputValue">The input number to have the Log10 taken of.</param>
		/// <param name="lowerBound">
		/// A lower bound on the input value. If the input value is below this
		/// threshold Log10(lowerBound) is returned to the user instead of Log10(inputValue).
		/// If the lowerBound value is less than or equal to 0.0 and the inputValue
		/// is less than or equal to 0.0, a "safety" value that is just above
		/// the smallest positive double value.
		/// </param>
		/// <returns>
		/// The base 10 logarithm of the input.
		/// </returns>
		public static double Log10(double inputValue, double lowerBound)
		{
			// Normal return.
			if (inputValue > lowerBound) { return Math.Log10(inputValue); }

			// Floor.
			if (lowerBound > 0.0) { return Math.Log10(lowerBound); }

			// SafetyValve.
			return s_TinyPositiveDouble;
		}

		/// <summary>
		/// Calculates the result of a positive or negative number modulo a given
		/// "modulus".
		/// </summary>
		/// <param name="modulus">
		/// The size of the alphabet. The out number is always from 0 to
		/// <paramref name="modulus"/> - 1.
		/// </param>
		/// <param name="number">
		/// The number for which to calculate the remainder, modulo the
		/// <paramref name="modulus"/>.
		/// </param>
		/// <returns>
		/// The remainder.
		/// </returns>
		/// <remarks>
		/// 7 modulo 7 is 0. 6 modulo 7 is 6. -1 modulo 7 is 6. -18 modulo 7
		/// is 3.
		/// </remarks>
		public static int Modulo(int modulus, int number)
		{
			// Just modulo arithmetic.
			var cycles = number / modulus;
			var remainder = number - cycles * modulus;
			if (remainder < 0)
				remainder = modulus + remainder;
			return remainder;
		}
		/// <summary>
		/// Calculates the number of bit that are "1" in a long int.
		/// </summary>
		/// <param name="theLongInt">The integer to process.</param>
		/// <returns>The number of  bits set to "1".</returns>
		/// <remarks>
		/// Checks 31 bits.
		/// </remarks>
		public static int NumBitsOn(long theLongInt)
		{
			var numOn = 0;
			var bitPos = 0;
			do {
				var and = 1 << bitPos;
				if ((theLongInt & and) != 0) numOn++;
				bitPos++;
			} while (bitPos < 32);
			return numOn;
		}
		/// <summary>
		/// Calculates the number of bit that are "1" in an <see cref="Enum"/>.
		/// </summary>
		/// <param name="theEnum">The enum to process.</param>
		/// <returns>The number of  bits set to "1".</returns>
		/// <exceptions>
		/// Exceptions will be thrown if a negative value for the enum
		/// is passed.
		/// </exceptions>
		public static int NumBitsOn(Enum theEnum)
		{
			var longRegister = theEnum.ToInt64();
			return NumBitsOn(longRegister);
		}
		#endregion
	}

	/// <summary>
	/// This class is a utility class for strings.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> Brian T. </author>
	/// <date> 01feb2019 </date>
	/// <description>
	/// Added history. Fixed a problem in "Compress" that injected blanks.
	/// </description>
	/// </contribution>
	/// </history>
	public class PAFString
	{
		/// <summary>
		/// Character array that is used to identify whitespace characters.
		/// </summary>
		private static readonly char[] s_DefaultWhitespaceChars
			= { ' ', '\n', '\t', '\r', '\f', '\v' };
		/// <summary>
		/// Method concatenates strings to a main string if they are not <see langword="null"/>.
		/// </summary>
		/// <param name="mainString">
		/// The string that the other strings are to be concatenated to. If <see langword="null"/>,
		/// <see langword="null"/> is returned.
		/// </param>
		/// <param name="stringsToConcat">
		/// Array of strings to be concatenated to <paramref name="mainString"/> if they
		/// are not <see langword="null"/>. The array object can itself can be <see langword="null"/>. In this
		/// case, no strings are concatenated, but the original main string is returned.
		/// </param>
		/// <param name="noStringsNull">
		/// Set to <see langword="true"/> to abort the concatenation operation if any strings
		/// in <paramref name="stringsToConcat"/> are <see langword="null"/>. Set to <see langword="false"/>
		/// to ignore only those strings that are <see langword="null"/>.
		/// </param>
		/// <returns>
		/// Concatenation of main string and suitable others if main not <see langword="null"/>.
		/// Returns <see langword="null"/> otherwise.
		/// </returns>
		public static string ConcatNoNull(string mainString, string[] stringsToConcat,
			bool noStringsNull)
		{
			// Flag a vacuous string.
			if (mainString == null)
				return null;
			// Life is easy if we got nothin'.
			if (stringsToConcat == null)
				return mainString;
			// Life is still easy if we got any nulls and we don't accept them.
			if (noStringsNull) {
// ReSharper disable ForCanBeConvertedToForeach
				for (var i = 0; i < stringsToConcat.Length; i++)
// ReSharper restore ForCanBeConvertedToForeach
					// Just leave if we find a null.
					if (stringsToConcat[i] == null)
						return mainString;
			}
			// Well, guess we have to do some work....
// ReSharper disable ForCanBeConvertedToForeach
			for (var i = 0; i < stringsToConcat.Length; i++)
// ReSharper restore ForCanBeConvertedToForeach
				if (stringsToConcat[i] != null)
					mainString = mainString + stringsToConcat[i];
			return mainString;
		}
		/// <summary>
		/// Returns the whitespace characters we use internally.
		/// </summary>
		/// <returns>List of chars.</returns>
		public static IList<char> DefaultWhiteSpaceCharacters()
		{
			return new List<char>(s_DefaultWhitespaceChars);
		}
		/// <summary>
		/// Method tells if a character is a whitespace character by comparing against
		/// <see cref="DefaultWhiteSpaceCharacters"/>.
		/// </summary>
		/// <param name="charToCheck">The char to be checked.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if whitespace.
		/// </returns>
		public static bool IsCharWhiteSpace(char charToCheck)
		{
// ReSharper disable LoopCanBeConvertedToQuery
// Mono
			foreach( var chr in s_DefaultWhitespaceChars)
// ReSharper restore LoopCanBeConvertedToQuery
			{
				if(charToCheck == chr) return true;
			}
			return false;
		}
		/// <summary>
		/// Method truncates string to <paramref name="length"/> characters.
		/// </summary>
		/// <param name="str">The string to be checked. <see langword="null"/> returns
		/// blank.
		/// </param>
		/// <param name="length">Maximum length of output string.
		/// blank.
		/// </param>
		/// <returns>
		/// A string that has potentially been shortened.
		/// </returns>
		public static string LimitedString(string str, int length)
		{
			// Flag a vacuous string.
			if (str == null)
				return "";
			if (str.Length < length) return str;
			return str.Substring(0, length);
		}
		/// <summary>
		/// Method just checks a string to see if it is <see langword="null"/> and
		/// if so, substitutes a blank.
		/// </summary>
		/// <param name="str">The string to be checked.</param>
		/// <returns>
		/// A blank string if the input string was <see langword="null"/> or
		/// else just the input string.
		/// </returns>
		public static string NullStringToBlank(string str)
		{
			// Flag a vacuous string.
			if (str == null)
				return "";
			return str;
		}
		/// <summary>
		/// This method removes whitespace characters within a string by default. These are
		/// <c>{ ' ', '\n', '\t', '\r', '\f', '\v' }</c>. User can specify another set
		/// of characters as an option.
		/// </summary>
		/// <param name="inputString">String to remove embedded characters from.</param>
		/// <param name="charsToRemove">Characters to remove. If <see langword="null"/>, default is used.</param>
		/// <returns></returns>
		public static string Compress(string inputString, char[] charsToRemove = null)
		{
			// Default if user specified none.
			if (charsToRemove == null)
				charsToRemove = s_DefaultWhitespaceChars;

			var sb = new StringBuilder();
			// Pull them apart with a split, meanwhile doing the deletions.
			var parts = inputString.Split(charsToRemove, StringSplitOptions.RemoveEmptyEntries);
			var size = parts.Length;
			// Put 'em back together
			for (var i = 0; i < size; i++)
				sb.AppendFormat("{0}", parts[i]);
			return sb.ToString();
		}
		/// <summary>
		/// Method to check strings for equality where either or both can be
		/// <see langword="null"/> or <see cref="string.Empty"/> and they still we
		/// be considered equal.
		/// </summary>
		/// <param name="string1">The first string to be checked.</param>
		/// <param name="string2">The second string to be checked.</param>
		/// <returns>
		/// <see langword="true"/> if both strings are not <see langword="null"/>,
		/// both strings are not blank ("") and they are the same. Also returns
		/// <see langword="true"/> if either string or both strings are <see langword="null"/>
		/// or blank.
		/// </returns>
		public static bool StringsNullOrMatch(string string1, string string2)
		{
			// Vacuous string cases.
			if (string.IsNullOrEmpty(string1))
				return true;
			if (string.IsNullOrEmpty(string2))
				return true;
			// See if they match.
			return string.Equals(string1, string2);
		}
	}
}
