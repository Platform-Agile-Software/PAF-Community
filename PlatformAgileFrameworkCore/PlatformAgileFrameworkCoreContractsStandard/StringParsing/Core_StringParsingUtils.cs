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
#pragma warning disable 1587
///<file>
/// <summary>
/// This file contains parsing utilities that support 
/// general custom string parsing within the .Net framework.
/// <summary/>
/// <file/>
#pragma warning restore 1587
using System;
using System.Text;
using System.Text.RegularExpressions;
using PlatformAgileFramework.Platform;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;

namespace PlatformAgileFramework.StringParsing
{
	/// <summary>
	/// <para>
	/// Helper methods and other items for parsing strings. There are some things
	/// in here that could have been done with regexes, but regexes are PAINFULLY
	/// OS-dependent. (i.e. MS's implementation of capture groups vs. BSD)
	/// and PAINFULLY expensive in terms of GC's.
	/// </para>
	/// <para>
	/// This is the half of the class that is Silverlight-compatible.
	/// </para>
	/// </summary>
// ReSharper disable PartialTypeWithSinglePart
	public partial class StringParsingUtils
// ReSharper restore PartialTypeWithSinglePart
	{
		#region Class Fields
		// Here follows a number of regular expressions used by the parser.
		/// <summary>
		///  This regex pulls apart items inside the {}'s.
		/// </summary>
		public static readonly Regex ConstituentValueArrayRegex
			= new Regex(@"(?<Constituent>[\w]+)\s*=\s*(?<Value>[\w\d]+)");
		/// <summary>
		/// This regex finds a leading label inside a set of braces and
		/// separates it from the rest of the text string, which it places
		/// in "StringValue". The braces are assumed to have already been
		/// removed, so this Regex also works on any string that has an
		/// alphanumeric label separated from the rest of the string
		/// by whitespace and no intervening comma or equals signs.
		/// </summary>
		public static readonly Regex LeadingLabelInsideBracesRegex
			= new Regex(@"(?<Label>[\w]+)\s*(?<StringValue>[^=,].*)");
		/// <summary>
		/// These are the simple types that can be converted from a string. They
		/// include the <see cref="IConvertible"/> types and the <see cref="Enum"/>-derived
		/// types and <see cref="Guid"/>.
		/// </summary>
		public static readonly Type[] SimpleParsableTypes
			=
		{
			typeof (bool), typeof (byte), typeof (char), typeof (DateTime), typeof (decimal),
			typeof (double), typeof (short), typeof (int), typeof (long), typeof (sbyte),
			typeof (float), typeof (ushort), typeof (uint), typeof (ulong), typeof(string),
			typeof(Enum), typeof(Guid)
		};
		//// These are standard return values for the MatchDescriptor.
		/// <summary>
		/// Error descriptor has the error code (-1) and 0 settings for the indices.
		/// </summary>
		public static readonly MatchDescriptor MatchError = new MatchDescriptor(-1, 0, 0);
		/// <summary>
		/// Our failure match descriptor has no matches and 0 settings for the indices.
		/// Use this when it is not necessary to return indices indicating the point
		/// of failure or other use of indices.
		/// </summary>
		public static readonly MatchDescriptor MatchFailure = new MatchDescriptor(0, 0, 0);
		/// <summary>
		/// Our success match descriptor has 1 match and 0 settings for the indices.
		/// Use this when it is not necessary to return indices indicating the point
		/// of match. Useful for "simple" matches where only one match is possible
		/// or it is irrelevant how many matches have been found and we don't care where.
		/// </summary>
		public static readonly MatchDescriptor MatchSuccess = new MatchDescriptor(1, 0, 0);
#pragma warning disable 649
		/// <summary>
		/// A little pre-stored argument for passing a <see langword="null"/> list into match
		/// functions.
		/// </summary>
		public static readonly MatchPartition NullMatchDescriptorList = null;
#pragma warning restore 649
		#endregion

		#region Methods
		/// <summary>
		/// Interface to <see cref="MultiplePatternWildCardMatch"/> with specialized
		/// parameters. Just checks if any patterns are contained in the input string
		/// starting at a given offset.
		/// </summary>
		/// <param name="stringToCheck">
		/// See <see cref="MultipleWildCardMatch"/>.
		/// </param>
		/// <param name="stringIndex">
		/// See <see cref="MultipleWildCardMatch"/>.
		/// </param>
		/// <param name="patterns">
		/// See <see cref="MultipleWildCardMatch"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if string is not <see langword="null"/> or blank and has one of the
		/// patterns.
		/// </returns>
		/// <remarks>
		///  We built this because .Net doesn't have one for multiple string patterns.
		/// </remarks>
		public static bool ContainsAPattern(string stringToCheck,
			int stringIndex, string[] patterns)
		{
			if (MultiplePatternWildCardMatch(stringToCheck, stringIndex, patterns, null, null,
				true, 1, false, null).NumMatches > 0)
				return true;
			return false;
		}
		/// <summary>
		/// A little helper method that just checks a char to see if it matches
		/// any char in an array.
		/// </summary>
		/// <param name="charToCheck">
		/// This is the character that is to be found in the array.
		/// </param>
		/// <param name="testChars">
		/// This is the array of chars that is to be searched within. It can
		/// be <see langword="null"/>, in which case <see langword="false"/> is returned.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the input character matches any in the array.
		/// </returns>
		public static bool CheckChars(char charToCheck, char[] testChars)
		{
			if (testChars == null)
				return false;
			foreach (var c in testChars)
				if (charToCheck == c)
					return true;
			return false;
		}
		/// <summary>
		/// Just finds an isolated (not repeated) character.
		/// </summary>
		/// <param name="stringPossiblyContainingCharacter">
		/// The name. <see langword="null"/> or <see cref="String.Empty"/>
		/// returns -1.
		/// </param>
		/// <param name="character">
		/// Character to find.
		/// </param>
		/// <returns>
		/// Index of the found isolated character or -1.
		/// </returns>
		public static int FindIsolatedCharacter(string stringPossiblyContainingCharacter, char character)
		{
			// Safety valve.
			if (string.IsNullOrEmpty(stringPossiblyContainingCharacter)) return -1;
			var index = stringPossiblyContainingCharacter.IndexOf(character, 0);
			if (index == -1) return -1;

			// Need to check for another.
			if ((stringPossiblyContainingCharacter.Length > index + 1) &&
			    (stringPossiblyContainingCharacter[index + 1] == character))
				return -1;
			return index;
		}
		/// <summary>
		/// Simple formatter just prints name, followed by a colon, then value.
		/// line feed and carriage retrurn is added at the end.
		/// </summary>
		/// <param name="name">
		/// The name. <see langword="null"/> or <see cref="String.Empty"/>
		/// returns <see cref="String.Empty"/>.
		/// </param>
		/// <param name="value">
		/// The value. <see langword="null"/> prints "null".
		/// </param>
		/// <returns>
		/// The formatted name/value pair.
		/// </returns>
		public static string FormatNameValue(string name, object value)
		{
			// Safety valve.
			if (string.IsNullOrEmpty(name)) return "";
			// Prefix its name.
			var outputString = name + ": ";
			if (value != null)
				outputString += value + PlatformUtils.LTRMN;
			else
				outputString += "null" + PlatformUtils.LTRMN;

			return outputString;
		}
		/// <summary>
		/// Just runs through all the simpleparsable types and checks to see if
		/// <paramref name="type"/> is one.
		/// </summary>
		/// <param name="type">Type to check.</param>
		/// <returns><see langword="true"/> if we can convert.</returns>
		public static bool IsTypeSimpleParsable(Type type)
		{
			if (type.IsTypeAnEnum()) return true;
			foreach (var t in SimpleParsableTypes)
			{
				if (type == t) return true;
			}
			return false;
		}
		/// <summary>
		/// Checks if a character is a digit.
		/// </summary>
		/// <param name="characterToCheck">.</param>
		/// <returns><see langword="true"/> if a numeric digit 0-9.</returns>
		public static bool IsANumber(char characterToCheck)
		{
			if (characterToCheck == '0')
				return true;
			if (characterToCheck == '1')
				return true;
			if (characterToCheck == '2')
				return true;
			if (characterToCheck == '3')
				return true;
			if (characterToCheck == '4')
				return true;
			if (characterToCheck == '5')
				return true;
			if (characterToCheck == '6')
				return true;
			if (characterToCheck == '7')
				return true;
			if (characterToCheck == '8')
				return true;
			if (characterToCheck == '9')
				return true;
			return false;
		}
		/// <summary>
		/// Interface to <see cref="MultipleWildCardMatch"/> with specialized parameters.
		/// </summary>
		/// <param name="stringToCheck">
		/// See <see cref="MultipleWildCardMatch"/>.
		/// </param>
		/// <param name="stringIndex">
		/// See <see cref="MultipleWildCardMatch"/>.
		/// </param>
		/// <param name="pattern">
		/// See <see cref="MultipleWildCardMatch"/>.
		/// </param>
		/// <param name="maxMatches">
		/// See <see cref="MultipleWildCardMatch"/>.
		/// </param>
		/// <param name="overlapping">
		/// See <see cref="MultipleWildCardMatch"/>.
		/// </param>
		/// <param name="matches">
		/// See <see cref="MultipleWildCardMatch"/>.
		/// </param>
		/// <returns>
		/// See <see cref="MultipleWildCardMatch"/>.
		/// </returns>
		public static MatchDescriptor Match(string stringToCheck,
			int stringIndex, string pattern, int maxMatches,
			bool overlapping, MatchPartition matches)
		{
			return MultipleWildCardMatch(stringToCheck, stringIndex, pattern, 0, null, null,
			true, maxMatches, overlapping, matches);
		}
		/// <summary>
		/// Interface to <see cref="MultiplePatternWildCardMatch"/> with specialized parameters.
		/// </summary>
		/// <param name="stringToCheck">
		/// See <see cref="MultiplePatternWildCardMatch"/>.
		/// </param>
		/// <param name="stringIndex">
		/// See <see cref="MultiplePatternWildCardMatch"/>.
		/// </param>
		/// <param name="patterns">
		/// See <see cref="MultiplePatternWildCardMatch"/>.
		/// </param>
		/// <param name="maxMatches">
		/// See <see cref="MultiplePatternWildCardMatch"/>.
		/// </param>
		/// <param name="overlapping">
		/// See <see cref="MultiplePatternWildCardMatch"/>.
		/// </param>
		/// <param name="matches">
		/// See <see cref="MultiplePatternWildCardMatch"/>.
		/// </param>
		/// <returns>
		/// See <see cref="MultipleWildCardMatch"/>.
		/// </returns>
		public static MatchDescriptor Match(string stringToCheck,
			int stringIndex, string[] patterns, int maxMatches,
			bool overlapping, MatchPartition matches)
		{
			return MultiplePatternWildCardMatch(stringToCheck, stringIndex, patterns,
			null, null, true, maxMatches, overlapping, matches);
		}
		/// <summary>
		/// Interface to <see cref="MultiplePatternWildCardMatch"/> with specialized parameters.
		/// </summary>
		/// <param name="stringToCheck">
		/// See <see cref="MultiplePatternWildCardMatch"/>.
		/// </param>
		/// <param name="patterns">
		/// See <see cref="MultiplePatternWildCardMatch"/>.
		/// </param>
		/// <param name="maxMatches">
		/// See <see cref="MultiplePatternWildCardMatch"/>.
		/// </param>
		/// <param name="matches">
		/// See <see cref="MultiplePatternWildCardMatch"/>.
		/// </param>
		/// <returns>
		/// See <see cref="MultipleWildCardMatch"/>.
		/// </returns>
		public static MatchDescriptor Match(string stringToCheck,
			string[] patterns, int maxMatches,
			MatchPartition matches)
		{
			return MultiplePatternWildCardMatch(stringToCheck, 0, patterns,
			null, null, true, maxMatches, false, matches);
		}

		/// <summary>
		/// This method examines a string for a single match of a given input pattern
		/// which must be lined up with the indexed start of the string. Example: The
		/// string <c>abcdef</c> is a match to the pattern <c>abc</c>, but the string
		/// <c>aabc</c> is not. The method returns the starting and ending index in
		/// the string where the match was found. The starting index will always be
		/// the <see paramref="stringIndex"/> passed in the this method. Because of
		/// multiple-match wildcards, the ending index can be different than would
		/// be predicted solely on the basis of the pattern length. This is a lightweight
		/// method which does no heap allocations.
		/// </summary>
		/// <param name="stringToCheck">
		/// The input string to be checked for matches.
		/// </param>
		/// <param name="stringIndex">
		/// The 0-based index in the input string where the match is to start. A value
		/// of less than 0 or off the end of the string produces a match failure.
		/// </param>
		/// <param name="pattern">
		/// The pattern to search for. This pattern may contain wildcard characters
		/// of the type prescribed in <see paramref ="singleMatchChar"/> and/or
		/// <see paramref ="multipleMatchChar"/>. It may not contain repeated multiple match
		/// characters in successive positions. A match failure indication is returned
		/// if two successive multiple match characters are detected (e.g. a <c>"**"</c>
		/// embedded in the pattern).
		/// </param>
		/// <param name="patternIndex">
		/// The 0-based index in the input pattern string where the match is to start.
		/// A value of less than 0 produces a match failure. A value off the end of the
		/// string produces a match success, since there are no characters in the pattern
		/// to cause a mismatch.
		/// </param>
		/// <param name="singleMatchChars">
		/// This is a set of characters that will match one single position in
		/// the input string. Example: <c>?</c>. In this example, "abcxefg" would match
		/// the pattern "abc*efg".
		/// </param>
		/// <param name="multipleMatchChars">
		/// This is a set of characters that will match multiple positions in the input
		/// string. Example: <c>*</c>. In this example, "abcxefg" would match
		/// the pattern "abc*efg" and so would "abxyzefg". 
		/// </param>
		/// <param name="multipleMatchOneOrMore">
		/// If <see langword="true"/>, a multiple match wildcard must correspond to at least one
		/// character position in the input string. If <see langword="true"/>, "abcefg" would not
		/// match the pattern "abc*efg", if <c>*</c> were a multiple wildcard character.
		/// This parameter does not affect the behavior of matches involving the single
		/// wildcard match characters.
		/// </param>
		/// <returns>
		/// A <see cref="MatchDescriptor"/> containing information about the match. If
		/// no match is found, <c>MatchDescriptor.NumMatches</c> is returned as 0. If
		/// an error occurred, <c>MatchDescriptor.NumMatches</c> is returned as -1. If
		/// a match is found <c>MatchDescriptor.NumMatches</c> is returned as 1. The
		/// starting index in the string where the pattern match occurred is always
		/// <see paramref="stringIndex"/> and is returned in
		/// <c>MatchDescriptor.OffsetOfMatchStart</c>. The ending index in the string
		/// where the pattern match occurred is returned in
		/// <c>MatchDescriptor.OffsetOfMatchEnd</c>.
		/// </returns>
		public static MatchDescriptor WildCardMatch(string stringToCheck,
			int stringIndex, string pattern, int patternIndex,
			char[] singleMatchChars, char[] multipleMatchChars,
			bool multipleMatchOneOrMore)
		{
			// Safety valve.
			if (string.IsNullOrEmpty(stringToCheck) || string.IsNullOrEmpty(pattern)
				|| (stringIndex < 0) || stringIndex >= stringToCheck.Length
				|| (patternIndex < 0)) {
				return MatchError;
			}
			var patternLen = pattern.Length;
			var stringLen = stringToCheck.Length;
			// This is the pattern character we are working on.
			// Successful match descriptor has exactly one match. The start position is
			// always at the string index....
			var matchSuccess = new MatchDescriptor(1, stringIndex, 0);
			// If the pattern is finished, we match.
			if (patternIndex >= pattern.Length) {
				matchSuccess.OffsetOfMatchEnd = stringLen - 1;
				return matchSuccess;
			}
			// Each iteration of this loop advances exactly one character in the
			// pattern string.
			do {
				// Time to go if string scanned.
				if (stringIndex == stringLen) {
					// If we are here, it means that we have processed the entire
					// string but still have characters left in the pattern to match.
					// This constitutes a non-match.
					return MatchFailure;
				}
				// Grab the current character in the pattern and point to next char.
				var curPatChar = pattern[patternIndex++];
				// Here we process multiple wildcard chars in the pattern.
				if (CheckChars(curPatChar, multipleMatchChars)) {
					// We do not allow two consecutive multiple match characters.
					if ((patternIndex < patternLen)
						&& CheckChars(pattern[patternIndex], multipleMatchChars))
						return MatchError;
					// If we are not set to accept zero matches for the wildcard, we
					// need to advance by one and running off the string means failure.
					if ((multipleMatchOneOrMore) && (++stringIndex == stringLen))
						return MatchFailure;
					// Walk out to the end of the string to see if we can find a match,
					// calling ourselves recursively.
					while (stringIndex < stringLen) {
						var innerMatch = WildCardMatch(stringToCheck, stringIndex++,
							pattern, patternIndex, singleMatchChars, multipleMatchChars,
							multipleMatchOneOrMore);
						// Inspect the inner match to see if we got anything.
						if (innerMatch.NumMatches <= 0) continue;
						// If we were successful, capture the ending index and return.
						matchSuccess.OffsetOfMatchEnd = innerMatch.OffsetOfMatchEnd;
						return matchSuccess;
					}
					// If we got here, it means we ran off the string without a match.
					// matchDescriptor will already be properly loaded.
					return MatchFailure;
				}
				// Just single-step characters.
				// If the current pattern character is a single wildcard matcher,
				// its Ok - we skip over this check.
				if (!CheckChars(curPatChar, singleMatchChars)) {
					// This is an ordinary character - if not a match, we're done.
					if (stringToCheck[stringIndex] != curPatChar)
						return MatchFailure;
				}
				// We matched this character - advance the string.
				stringIndex++;
				// At this point in the loop, stringIndex will be pointing at the
				// next character to match.
			} while ((patternIndex < patternLen) && (stringIndex < stringLen));
			// If pattern is finished and we got here, that means we matched up this
			// point (success). Hand back the correct offset.
			if (patternIndex == patternLen) {
				// Gotta' back up string index by one to be correct.
				matchSuccess.OffsetOfMatchEnd = stringIndex - 1;
				return matchSuccess;
			}
			// If string has ended prematurely, it's a failure.
			return MatchFailure;
		}
		/// <summary>
		/// This method examines a string for a matches of a given input pattern.
		/// The method searches for up to a user-specified number of matches.
		/// The method returns the starting and ending index in the string where the
		/// last match was found. This is a lightweight method which does no heap
		/// allocations.
		/// </summary>
		/// <param name="stringToCheck">
		/// The input string to be checked for matches.
		/// </param>
		/// <param name="stringIndex">
		/// The 0-based index in the input string where the match is to start. A value
		/// of less than 0 or off the end of the string produces a match failure.
		/// </param>
		/// <param name="pattern">
		/// The pattern to search for. This pattern may contain wildcard characters
		/// of the type prescribed in <see paramref ="singleMatchChar"/> and/or
		/// <see paramref ="multipleMatchChar"/>. It may not contain repeated multiple match
		/// characters in successive positions. A match failure indication is returned
		/// if two successive multiple match characters are detected (e.g. a <c>"**"</c>
		/// embedded in the pattern).
		/// </param>
		/// <param name="patternIndex">
		/// The 0-based index in the input pattern string where the match is to start.
		/// A value of less than 0 produces a match failure. A value off the end of the
		/// string produces a match success, since there are no characters in the pattern
		/// to cause a mismatch.
		/// </param>
		/// <param name="singleMatchChars">
		/// This is a set of characters that will match one single position in
		/// the input string. Example: <c>?</c>. In this example, "abcxefg" would match
		/// the pattern "abc*efg".
		/// </param>
		/// <param name="multipleMatchChars">
		/// This is a set of characters that will match multiple positions in the input
		/// string. Example: <c>*</c>. In this example, "lmnabcxefg" would match
		/// the pattern "abc*efg" and so would "lmnabxyzefg". The string "mabxyzefgnabxyzefghi"
		/// would contain two matches.
		/// </param>
		/// <param name="multipleMatchOneOrMore">
		/// If <see langword="true"/>, a multiple match wildcard must correspond to at least one
		/// character position in the input string. If <see langword="true"/>, "abcefg" would not
		/// match the pattern "abc*efg", if <c>*</c> were a multiple wildcard character.
		/// This parameter does not affect the behavior of matches involving the single
		/// wildcard match characters.
		/// </param>
		/// <param name="maxMatches">
		/// This parameter indicates the maximum number of matches to attempt to locate.
		/// Set it to -1 to locate as many matches as possible. If set to zero, this
		/// method will attempt to find a single match, only looking at the initial
		/// offset position in the input string. Thus the pattern <c>abc</c> will match
		/// the string <c>abcxyz</c>, but not <c>xyzabc</c>.
		/// </param>
		/// <param name="overlapping">
		/// This parameter indicates the matches can be overlapping if <see langword="true"/>. In
		/// this case, the string "ababa" contains two matches of the pattern "aba".
		/// </param>
		/// <param name="matches">
		/// This parameter references a partition that is loaded
		/// with a match descriptor for every match encountered. Each of these descriptors
		/// indicates a single match with the indices of the match. This parameter can
		/// be <see langword="null"/>, in which case, no match descriptors are returned. This is
		/// useful when the client needs to know only the number of matches and the
		/// location of the last match. The matches are added (appended) to the list
		/// maintained by the partition if it is non-<see langword="null"/>.
		/// </param>
		/// <returns>
		/// A <see cref="MatchDescriptor"/> containing information about the match. If
		/// no match is found, <c>MatchDescriptor.NumMatches</c> is returned as 0. If
		/// an error occurred, <c>MatchDescriptor.NumMatches</c> is returned as -1. If
		/// matches are found <c>MatchDescriptor.NumMatches</c> is returned with the
		/// number of matches. The starting index in the string where the last pattern
		/// match occurred is returned in <c>MatchDescriptor.OffsetOfMatchStart</c>. The
		/// ending index is returned in <c>MatchDescriptor.OffsetOfMatchEnd</c>. If
		/// an error occurs, the returned <see cref="MatchDescriptor"/> will contain
		/// the indices of the LAST successful match, if any. If there was no successful
		/// match, they will be 0 and 0. If an error occurs and <see paramref="matches"/>
		/// is non-<see langword="null"/>, it will be loaded with the list of successful matches
		/// before the error occurred, if any.
		/// </returns>
		public static MatchDescriptor MultipleWildCardMatch(string stringToCheck,
			int stringIndex, string pattern, int patternIndex,
			char[] singleMatchChars, char[] multipleMatchChars,
			bool multipleMatchOneOrMore, int maxMatches,
			bool overlapping, MatchPartition matches)
		{
			// Safety valve.
			if (string.IsNullOrEmpty(stringToCheck) || string.IsNullOrEmpty(pattern)
				|| (stringIndex < 0) || stringIndex >= stringToCheck.Length
				|| (patternIndex < 0)) {
				return MatchError;
			}
			var stringLen = stringToCheck.Length;
			// Create match descriptor. We will load it with specifics when we have them.
			var matchDescriptor = new MatchDescriptor();
			// Each iteration of this loop advances to the next possible match position
			// and tries to find a single match.
			do {
				var innerMatch = WildCardMatch(stringToCheck, stringIndex,
					pattern, patternIndex, singleMatchChars, multipleMatchChars,
					multipleMatchOneOrMore);
				// Inspect the inner match to see if we got anything.
				if (innerMatch.NumMatches <= 0) {
					// If we have an error we must load the NumMatches value
					// and return it to hand the error indication back. We leave it
					// loaded with the indices of the last match (if any).
					if (innerMatch.NumMatches < 0) {
						matchDescriptor.NumMatches = innerMatch.NumMatches;
						return matchDescriptor;
					}// Otherwise we'll keep going and keep trying for matches.
				}
				else {
					// If we were successful, capture the indices and bump the
					// number of matches.
					matchDescriptor.OffsetOfMatchStart = innerMatch.OffsetOfMatchStart;
					matchDescriptor.OffsetOfMatchEnd = innerMatch.OffsetOfMatchEnd;
					matchDescriptor.NumMatches++;
					// Capture this match if we have a List.
				    matches?.AddMatch(innerMatch);
				    // We must move the stringindex to the end of the match if we
					// are non-overlapping.
					if (!overlapping)
						stringIndex = matchDescriptor.OffsetOfMatchEnd;
				}
				// We matched this position - advance the string.
				stringIndex++;
			} while ((stringIndex < stringLen)
				&& ((maxMatches < 0) || (matchDescriptor.NumMatches < maxMatches)));
			// We have reached the end of the string. Return the last match (if any).
			return matchDescriptor;
		}
		/// <summary>
		/// This method examines a string for a matches of given input patterns.
		/// The method searches for up to a user-specified number of matches.
		/// The method returns the starting and ending index in the string where the
		/// last match was found. This is a lightweight method which does no heap
		/// allocations.
		/// </summary>
		/// <param name="stringToCheck">
		/// The input string to be checked for matches.
		/// </param>
		/// <param name="stringIndex">
		/// The 0-based index in the input string where the match is to start. A value
		/// of less than 0 or off the end of the string produces a match failure.
		/// </param>
		/// <param name="patterns">
		/// An array of patterns to search for. Each pattern may contain wildcard
		/// characters of the type prescribed in <see paramref ="singleMatchChar"/> and/or
		/// <see paramref ="multipleMatchChar"/>. It may not contain repeated multiple match
		/// characters in successive positions. A match failure indication is returned
		/// if two successive multiple match characters are detected (e.g. a <c>"**"</c>
		/// embedded in the pattern). Patterns need not be the same length. The patterns
		/// array may be empty or <see langword="null"/>, in which case a match failure is returned.
		/// When searching at a given offset position in the input string, the match
		/// patterns are tried one by one, in their installed order. As soon as one is
		/// found that matches, a match is declared and the string index is advanced to
		/// the next offset position, where the patterns are scanned again until the
		/// <see paramref="maxMatches"/> is exceeded or we reach the end of the string.
		/// </param>
		/// <param name="singleMatchChars">
		/// This is a set of characters that will match one single position in
		/// the input string. Example: <c>?</c>. In this example, "abcxefg" would match
		/// the pattern "abc*efg".
		/// </param>
		/// <param name="multipleMatchChars">
		/// This is a set of characters that will match multiple positions in the input
		/// string. Example: <c>*</c>. In this example, "lmnabcxefg" would match
		/// the pattern "abc*efg" and so would "lmnabxyzefg". The string "mabxyzefgnabxyzefghi"
		/// would contain two matches.
		/// </param>
		/// <param name="multipleMatchOneOrMore">
		/// If <see langword="true"/>, a multiple match wildcard must correspond to at least one
		/// character position in the input string. If <see langword="true"/>, "abcefg" would not
		/// match the pattern "abc*efg", if <c>*</c> were a multiple wildcard character.
		/// This parameter does not affect the behavior of matches involving the single
		/// wildcard match characters.
		/// </param>
		/// <param name="maxMatches">
		/// This parameter indicates the maximum number of matches to attempt to locate.
		/// Set it to -1 to locate as many matches as possible. If set to zero, this
		/// method will attempt to find a single match, only looking at the initial
		/// offset position in the input string. Thus the pattern <c>abc</c> will match
		/// the string <c>abcxyz</c>, but not <c>xyzabc</c>.
		/// </param>
		/// <param name="overlapping">
		/// This parameter indicates the matches can be overlapping if <see langword="true"/>. In
		/// this case, the string "ababa" contains two matches of the pattern "aba".
		/// </param>
		/// <param name="matches">
		/// This parameter references a partition that is loaded
		/// with a match descriptor for every match encountered. Each of these descriptors
		/// indicates a single match with the indices of the match. This parameter can
		/// be <see langword="null"/>, in which case, no match descriptors are returned. This is
		/// useful when the client needs to know only the number of matches and the
		/// location of the last match. The matches are added (appended) to the list
		/// maintained by the partition if it is non-<see langword="null"/>.
		/// </param>
		/// <returns>
		/// A <see cref="MatchDescriptor"/> containing information about the match. If
		/// no match is found, <c>MatchDescriptor.NumMatches</c> is returned as 0. If
		/// an error occurred, <c>MatchDescriptor.NumMatches</c> is returned as -1. If
		/// matches are found <c>MatchDescriptor.NumMatches</c> is returned with the
		/// number of matches. The starting index in the string where the last pattern
		/// match occurred is returned in <c>MatchDescriptor.OffsetOfMatchStart</c>. The
		/// ending index is returned in <c>MatchDescriptor.OffsetOfMatchEnd</c>. If
		/// an error occurs, the returned <see cref="MatchDescriptor"/> will contain
		/// the indices of the LAST successful match, if any. If there was no successful
		/// match, they will be 0 and 0. If an error occurs and <see paramref="matches"/>
		/// is non-<see langword="null"/>, it will be loaded with the list of successful matches
		/// before the error occurred, if any.
		/// </returns>
		public static MatchDescriptor MultiplePatternWildCardMatch(string stringToCheck,
			int stringIndex, string[] patterns,
			char[] singleMatchChars, char[] multipleMatchChars,
			bool multipleMatchOneOrMore, int maxMatches,
			bool overlapping, MatchPartition matches)
		{
			// Safety valves.
			if (string.IsNullOrEmpty(stringToCheck) || (patterns == null)
				|| (stringIndex < 0) || stringIndex >= stringToCheck.Length) {
				return MatchError;
			}
			// Degenerate patterns.
			if (patterns.Length == 0) {
				return MatchFailure;
			}
			var stringLen = stringToCheck.Length;
			// Create match descriptor. We will load it with specifics when we have them.
			var matchDescriptor = new MatchDescriptor();
			// Each iteration of this loop advances to the next possible match position
			// and tries to find a single match.
			do {
				// For every iteration of this loop, we try a different pattern.
				foreach (var pattern in patterns) {
					var innerMatch = WildCardMatch(stringToCheck, stringIndex,
						pattern, 0, singleMatchChars, multipleMatchChars,
						multipleMatchOneOrMore);
					// Inspect the inner match to see if we got anything.
					if (innerMatch.NumMatches <= 0) {
						// If we have an error we must load the NumMatches value
						// and return it to hand the error indication back. We leave it
						// loaded with the indices of the last match (if any).
						if (innerMatch.NumMatches < 0) {
							matchDescriptor.NumMatches = innerMatch.NumMatches;
							return matchDescriptor;
						}// Otherwise we'll keep going and keep trying for matches.
					}
					else {
						// If we were successful, capture the indices and bump the
						// number of matches.
						matchDescriptor.OffsetOfMatchStart = innerMatch.OffsetOfMatchStart;
						matchDescriptor.OffsetOfMatchEnd = innerMatch.OffsetOfMatchEnd;
						matchDescriptor.NumMatches++;
						// Capture this match if we have a partition.
					    matches?.AddMatch(innerMatch);
					    // We exit the loop because we only look for one pattern match
						// in every match position. First, we must move the stringindex
						// to the end of the match if we are non-overlapping.
						if (!overlapping)
							stringIndex = matchDescriptor.OffsetOfMatchEnd;
						break;
					}
				}
				//// We matched this position - advance the string.
				stringIndex++;
			} while ((stringIndex < stringLen)
				&& ((maxMatches < 0) || (matchDescriptor.NumMatches < maxMatches)));
			// We have reached the end of the string or exhausted maxMatches. Return
			// the last match (if any).
			return matchDescriptor;
		}
		/// <summary>
		/// This method performs a search and replace operation <c>SeaRep</c>for
		/// a single type of character in a string.
		/// </summary>
		/// <param name="stringToProcess">
		/// The input string to be checked for matches of a "search" character and
		/// replaced with a "replace" character.
		/// </param>
		/// <param name="stringIndex">
		/// The 0-based index in the input string where the SeaRep is to start. A value
		/// of less than 0 or off the end of the string produces a match failure.
		/// </param>
		/// <param name="searchChar">
		/// The character to search for.
		/// </param>
		/// <param name="replaceChar">
		/// The character to replace the search character with. If this character is
		/// <see langword="null"/>, replacements are not made, but occurrances of the
		/// <see paramref="searchChar"/> are merely counted.
		/// </param>
		/// <param name="maxNumReplacements">
		/// This is the maximum number of occurrances of the search character that are
		/// to be replaced with the replace character.
		/// </param>
		/// <param name="numReplacementsMade">
		/// This "out" parameter indicates the number of successful replacements that
		/// have been made.
		/// </param>
		/// <returns>
		/// A processed string if 0 or more replacements have been made. We return
		/// <see langword="null"/> if input string was <see langword="null"/>, stringIndex was out of bounds
		/// or <see paramref="searchChar"/> is <see langword="null"/>. <see paramref="numReplacementsMade"/>
		/// is returned as -1 under these error conditions.
		/// </returns>
		public static string Replace(string stringToProcess,
			int stringIndex, char searchChar, char replaceChar,
			int maxNumReplacements, out int numReplacementsMade)
		{
			StringBuilder outputMString = null;
			// Assume we didn't do anything.
			numReplacementsMade = 0;
			// Safety valves.
			if (string.IsNullOrEmpty(stringToProcess) || (searchChar == 0)
				|| (stringIndex < 0)
				|| stringIndex >= stringToProcess.Length)
			{
				return null;
			}
			var stringLen = stringToProcess.Length;
			//// Let's first find out if we have anything to do.
			if (stringToProcess.IndexOf(searchChar, stringIndex) < 0)
				return stringToProcess;
			if (searchChar == replaceChar)
				return stringToProcess;
			// OK, Create the stringbuilder, but only if we are replacing.
			if (replaceChar != PlatformUtils.NoChar)
			{
				outputMString = new StringBuilder(stringToProcess);
				do
				{
					// See if we match.
					if (outputMString[stringIndex] == searchChar)
					{
						numReplacementsMade++;
						if (replaceChar != PlatformUtils.NoChar)
							outputMString[stringIndex] = replaceChar;
					}
				} while ((++stringIndex < stringLen) && (numReplacementsMade != maxNumReplacements));
			}
			// Replaced anything?
			if ((outputMString != null) && (outputMString.Length > 0))
			{
				var finalOutputString = "";
				for (var charNum = 0; charNum < outputMString.Length; charNum++)
				{
					finalOutputString += outputMString[charNum];

				}

				outputMString.Clear();
				outputMString.Append(finalOutputString);

				finalOutputString = outputMString.ToString();

				return finalOutputString;
			}

			// Nope.
			return stringToProcess;
		}
		///// <summary>
		///// This simple method just checks to see if a string of tokens has a leading
		///// token separated from the rest with a only whitespace and no comma or equals
		///// sign. The format would be <code>LABEL item1,item2,...,itemN</code> or simply
		///// <code>LABEL STRING</code> would be processed fine, too. Anything with a first
		///// string, then whitespace, then a second string with no intervening commas or
		///// equals signs would be processed correctly.
		///// </summary>
		///// <param name="stringToCheck">
		///// A string to be checked.
		///// </param>
		///// <returns>
		///// An array of strings of length 2 if the input string has a label. The
		///// first string is the label and the second string is the remainder of
		///// the input string with the label removed along with intervening whitespace
		///// between the label and the rest of the string. A <see langword="null"/> is
		///// returned if the string was not in the required format. The output
		///// would have <code>LABEL</code> as the first string and
		///// <code>STRING</code> as the second string if the input were
		///// in the required format.
		///// </returns>
		// TODO KRM - What the hell is this?
		//public static string[] SeparateLeadingLabel(string stringToCheck)
		//{
		//    return null;
		//}
		/// <summary>
		/// Method that just removes enclosing quotes <code>""</code> from a string.
		/// </summary>
		/// <param name="inputString">
		/// String to take quotes off of.
		/// </param>
		/// <returns>
		/// A string with whitespace trimmed off the ends and stripped of enclosong
		/// quotes, if they were there. Otherwise the original string is returned,
		/// including any enclosing whitespace if it was there originally.
		/// </returns>
		public static string TrimEnclosingQuotes(string inputString)
		{
			// Just call the general method.
			return TrimEnclosingDelimiters(inputString, "\"");
		}
		/// <summary>
		/// Method that just removes enclosing braces <code>{}</code>
		/// from a string.
		/// </summary>
		/// <param name="inputString">
		/// String to take braces off of.
		/// </param>
		/// <returns>
		/// A string with whitespace trimmed off the ends and stripped of enclosong
		/// braces, if they were there. Otherwise the original string is returned,
		/// including any enclosing whitespace if it was there originally.
		/// </returns>
		public static string TrimEnclosingBraces(string inputString)
		{
			// Just call the general method.
			return TrimEnclosingDelimiters(inputString, "{", "}");
		}
		/// <summary>
		/// A simple little method that just removes enclosing delimiting strings
		/// from a string. The delimiter string can be a single character.
		/// </summary>
		/// <param name="inputString">
		/// String to take delimiters off of.
		/// </param>
		/// <param name="delimiter">
		/// The delimiter string which must be on both ends of the
		/// <paramref name="inputString"/>.
		/// </param>
		/// <returns>
		/// A string with whitespace trimmed off the ends and stripped of enclosong
		/// delimiters, if they were there. Otherwise the original string is returned,
		/// including any enclosing whitespace if it was there originally.
		/// </returns>
		public static string TrimEnclosingDelimiters(string inputString, string delimiter)
		{
			// Just call the general method.
			return TrimEnclosingDelimiters(inputString, delimiter, delimiter);
		}
		/// <summary>
		/// A simple little method that just removes enclosing delimiting strings
		/// from a string. The delimiter strings can be a single character.
		/// </summary>
		/// <param name="inputString">
		/// String to take delimiters off of.
		/// </param>
		/// <param name="startingDelimiter">
		/// The delimiter string which must be at the beginning of the
		/// <paramref name="inputString"/>.
		/// </param>
		/// <param name="endingDelimiter">
		/// The delimiter string which must be at the end of the
		/// <paramref name="inputString"/>.
		/// </param>
		/// <returns>
		/// A string with whitespace trimmed off the ends and stripped of enclosong
		/// delimiters, if they were there. Otherwise the original string is returned,
		/// including any enclosing whitespace if it was there originally.
		/// </returns>
		//todo ensure this is flyweight - not sure how String.Trim() works inside.
		public static string TrimEnclosingDelimiters(string inputString, string startingDelimiter,
			string endingDelimiter)
		{
			// First trim the string of whitespace.
			var outputString = inputString.Trim();
			// Find delimiter from both the front and the back?
			if (outputString.StartsWith(startingDelimiter, StringComparison.Ordinal) && outputString.EndsWith(endingDelimiter, StringComparison.Ordinal)) {
				var lengthOfStartingDelimiter = startingDelimiter.Length;
				var lengthOfEndingDelimiter = endingDelimiter.Length;
				var lengthOfDelimiters = lengthOfStartingDelimiter + lengthOfEndingDelimiter;
				// Make sure that string is more than just delimiter characters.
				// We need at least a length equal to two full delimiter strings.
				if (outputString.Length < lengthOfDelimiters)
					return inputString;
				// Lose the delimiters.
				outputString
					= outputString.Substring(lengthOfStartingDelimiter,
						outputString.Length - lengthOfDelimiters);
				return outputString;
			}
			return inputString;
		}
		#endregion
	}
}
