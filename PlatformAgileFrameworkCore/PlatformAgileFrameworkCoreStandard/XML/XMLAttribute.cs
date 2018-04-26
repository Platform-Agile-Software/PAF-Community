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
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.XML
{
	/// <summary>
	/// Class that holds the <see cref="string"/> components of an XML
	/// attribute of the form:
	/// <c>ATTRIBUTE_NAME="ATTRIBUTE_VALUE</c>.
	/// </summary>
	/// <history>
	/// <author> DAP </author>
	/// <date> 05nov2012 </date>
	/// <contribution>
	/// <para>
	/// Created. Support for allowing PAF legacy CLR stuff to run in core.
	/// </para>
	/// </contribution>
	/// </history>
	[PAFSerializable]
	public class XMLAttribute
	{
		#region Class AutoProperties
		/// <summary>
		/// The name, which will never be <see langword="null"/> or blank.
		/// </summary>
		public string AttributeName { get; protected internal set; }
		/// <summary>
		/// The value, which will never be <see langword="null"/>.
		/// </summary>
		public string AttributeValue { get; protected internal set; }
		#endregion // Class AutoProperties
		#region Constructors
		/// <summary>
		/// Constructor just set the fields.
		/// </summary>
		/// <param name="attributeName">
		/// The name. Not <see langword="null"/> or blank.
		/// </param>
		/// <param name="attributeValue">
		/// The name. Not <see langword="null"/>.
		/// </param>
		/// <exceptions>
		/// <exception> <see cref="ArgumentNullException"/> is thrown if
		/// either argument is <see langword="null"/>.
		/// </exception>
		/// <exception> <see cref="ArgumentException"/> is thrown if
		/// <paramref name="attributeName"/> is blank.
		/// </exception>
		/// </exceptions>
		public XMLAttribute(string attributeName, string attributeValue)
		{
			if (attributeName == null) throw new ArgumentNullException("attributeName");
			if (attributeName == "") throw new ArgumentException("attributeName cannot be blank");
			if (attributeValue == null) throw new ArgumentNullException("attributeValue");
			AttributeName = attributeName;
			AttributeValue = attributeValue;
		}
		#endregion // Constructors
		#region Methods
		/// <summary>
		/// Produces output of the form:
		/// <see cref ="AttributeName"/>="<see cref="AttributeValue"/>".
		/// </summary>
		/// <returns>The output.</returns>
		public string GetStringRepresentation()
		{
			return XMLUtils.FormXMLAttributeString(AttributeName, AttributeValue);
		}
		/// <summary>
		/// Accepts an input of the form:
		/// <see cref ="AttributeName"/>="<see cref="AttributeValue"/>"
		/// and builds an <see cref="XMLAttribute"/>.
		/// </summary>
		/// <param name="XMLAttributeString">Incoming attribute string.</param>
		/// <returns>
		/// Valid <see cref="XMLAttribute"/> if the parse was successful.
		/// <see langword="null"/> if not.
		/// </returns>
		public static XMLAttribute ParseStringRepresentation(string XMLAttributeString)
		{
			ParserIndices indices;
			if(!PreParse(XMLAttributeString, out indices)) return null;

			// Basically just picks out the name and value.
			var xmlName = XMLAttributeString.Substring(0, indices.NameEndIndex + 1);
			var valueLength
				= indices.LastQuoteIndex - indices.FirstQuoteIndex + 1;
			var xmlValue = XMLAttributeString.Substring(indices.FirstQuoteIndex + 1, valueLength);
			return new XMLAttribute(xmlName, xmlValue);
		}
		/// <summary>
		/// Checks that the input of the form:
		/// <see cref ="AttributeName"/>="<see cref="AttributeValue"/>".
		/// String can have whitespece, but not in the name.
		/// </summary>
		/// <param name="stringToVerify">Incoming string.</param>
		/// <returns>
		/// <see langword="true"/> if the string fits the pattern.
		/// </returns>
		public static bool IsAnAttributeRepresentation(string stringToVerify)
		{
			ParserIndices indices;
			return PreParse(stringToVerify, out indices);
		}
		/// <summary>
		/// Checks that the input of the form:
		/// <see cref ="AttributeName"/>="<see cref="AttributeValue"/>".
		/// String can have whitespece, but not in the name.
		/// </summary>
		/// <param name="stringToVerify">Incoming string.</param>
		/// <param name="indices">
		/// Return of the parsed indices for use in the full parse.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the string fits the pattern.
		/// </returns>
		internal static bool PreParse(string stringToVerify, out ParserIndices indices)
		{
			indices = new ParserIndices();
			if ((indices.EqualsIndex = stringToVerify.IndexOf('=')) == -1) return false;

			indices.FirstQuoteIndex = stringToVerify.IndexOf('"');
			if ((indices.FirstQuoteIndex == -1) || (indices.FirstQuoteIndex < indices.EqualsIndex)) return false;

			indices.LastQuoteIndex = stringToVerify.LastIndexOf('"');
			if ((indices.LastQuoteIndex == -1) || (indices.LastQuoteIndex == indices.FirstQuoteIndex)) return false;

			indices.NameEndIndex = -1;

			// Check the name for spaces.
			for (var index = indices.EqualsIndex; index >= 0; index--) {
				if (!PAFString.IsCharWhiteSpace(stringToVerify[index])) continue;
				// Can't have whitespace embedded in the name.
				if (indices.NameEndIndex > -1) return false;
				// We have found the end of the name.
				indices.NameEndIndex = index;
			}
			// Check for stuff at the end.
			for (var index = indices.LastQuoteIndex; index < stringToVerify.Length; index++) {
				if (!PAFString.IsCharWhiteSpace(stringToVerify[index])) {
					// Can have only whitespace in the end.
					return false;
				}
			}
			// Name can't be blank.
			if(indices.NameEndIndex == 0) return false;
			return true;
		}
		#endregion // Methods
	}
	#region Helper Types
	/// <summary>
	/// A structure that allows return of some indices calculated in the parsing
	/// process.
	/// </summary>
	internal struct ParserIndices
	{
		public int EqualsIndex { get; set; }
		public int FirstQuoteIndex { get; set; }
		public int LastQuoteIndex { get; set; }
		public int NameEndIndex { get; set; }
	}
	#endregion // Helper Types
}
