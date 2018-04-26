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

using System;
using System.Collections.Generic;
using PlatformAgileFramework.StringParsing;

namespace PlatformAgileFramework.XML
{
	/// <summary>
	/// <para>
	/// Helper methods and other items for processing XML.
	/// </para>
	/// </summary>
	/// <history>
	/// <author> DAP </author>
	/// <date> 05nov2012 </date>
	/// <contribution>
	/// <para>
	/// Converted PAF legacy CLR stuff to run in core. Took CLR-only stuff out.
	/// </para>
	/// </contribution>
	/// </history>
// ReSharper disable once PartialTypeWithSinglePart
	public partial class XMLUtils
	{
		#region Class Fields
		/// <summary>
		/// String containing "http://www.w3.org/XML/1998/namespace".
		/// </summary>
		public const string XML_PREFIX_NAMESPACE = "http://www.w3.org/XML/1998/namespace";

		/// <summary>
		/// String containing "http://www.w3.org/2000/xmlns/".
		/// </summary>
		public const string XMLNS_PREFIX_NAMESPACE = "http://www.w3.org/2000/xmlns/";

		/// <summary>
		/// This a standard separator string for separating components of a string key.
		/// </summary>
		public static readonly string SSS = ";~%@, ";
		/// <summary>
		/// Little helper for method calls.
		/// </summary>
		internal static readonly string[] SSSArray = { SSS };
		#endregion // Class Fields
		#region Methods
		/// <summary>
		/// Concatenates strings with our special separator,
		/// <see cref="SSS"/>.
		/// </summary>
		/// <param name="strings">
		/// Enumeration of incoming strings.
		/// </param>
		/// <param name="separatorString">
		/// String separating the concatenated strings. <see langword="null"/> causes
		/// <see cref="SSS"/> to be used.
		/// </param>
		/// <returns>
		/// Concatenated output or <c>""</c> for <see langword="null"/> input.
		/// </returns>
		public static string ConcatenateStrings(IEnumerable<string> strings, string separatorString)
		{
			if (separatorString == null) separatorString = SSS;
			var count = 0;
			var output = "";
			if (strings == null) return "";
			foreach (var str in strings) {
				if (str == null) continue;
				if (count != 0) output += separatorString;
				output += str;
				count++;
			}
			return output;
		}
		/// <summary>
		/// Get first string from concatenated strings with our special separator,
		/// <see cref="SSS"/>.
		/// </summary>
		/// <param name="concatenatedStrings">
		/// String formed from <see cref="ConcatenateStrings"/>. It may contain
		/// several concatenated strings, just one string or be <see langword="null"/>.
		/// </param>
		/// <param name="separatorString">
		/// String separating the concatenated strings. <see langword="null"/> causes
		/// <see cref="SSS"/> to be used.
		/// </param>
		/// <returns>
		/// Output string or <see langword="null"/> for <see langword="null"/> input.
		/// </returns>
		public static string GetFirstConcatenedString(string concatenatedStrings, string separatorString)
		{
			if (separatorString == null) separatorString = SSS;
			if (concatenatedStrings == null) return null;
			if (!concatenatedStrings.Contains(separatorString)) return concatenatedStrings;
			return concatenatedStrings.Substring(0, concatenatedStrings.IndexOf(separatorString, StringComparison.Ordinal));
		}
		/// <summary>
		/// Just forms an XML attribute.
		/// </summary>
		/// <param name="attributeName">
		/// Name of the attribute. <see langword="null"/> or blank gets <see langword="null"/> output.
		/// </param>
		/// <param name="attributeValue">
		/// Value of the attribute. <see langword="null"/> gets <see langword="null"/> output.
		/// </param>
		/// <returns>
		/// Concatenated output of the form:
		/// <paramref name="attributeName"/>="<paramref name="attributeValue"/>".
		/// </returns>
		public static string FormXMLAttributeString(string attributeName, string attributeValue)
		{
			if ((string.IsNullOrEmpty(attributeName)) || (attributeValue == null)) return null;
			return attributeName + @"=" + '"' + attributeValue + '"';
		}
		/// <summary>
		/// Determines whether a type is one of our Types that we construct directly
		/// from a string representation.
		/// </summary>
		/// <param name="type">Type to check.</param>
		/// <returns><see langword="true"/> if the constituent is convertable.</returns>
		public static bool IsTypeXMLSimple(Type type)
		{
			// TODO reconcile this with the xmltypecode types/conversions.
			// Simple types.
			if (StringParsingUtils.IsTypeSimpleParsable(type))
				return true;
			// Complex types.
			return false;
		}
		/// <summary>
		/// Separates strings that have been concatenated with our special
		/// separator, <see cref="SSS"/>.
		/// </summary>
		/// <param name="stringToSeparate">
		/// Concatenated string.
		/// </param>
		/// <param name="separatorString">
		/// String separating the concatenated strings. <see langword="null"/> causes
		/// <see cref="SSS"/> to be used.
		/// </param>
		/// <returns>
		/// List of strings or <see langword="null"/> for <see langword="null"/> input.
		/// </returns>
		public static IList<string> SeparateStrings(string stringToSeparate, string separatorString)
		{
			if (stringToSeparate == null) return null;
			if (separatorString == null)
				return stringToSeparate.Split(SSSArray, StringSplitOptions.None);
			return stringToSeparate.Split(new[] { separatorString }, StringSplitOptions.None);
		}
		#endregion
	}
}
