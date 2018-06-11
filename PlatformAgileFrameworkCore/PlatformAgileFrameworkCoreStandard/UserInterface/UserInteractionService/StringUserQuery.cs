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
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System;
using PlatformAgileFramework.UserInterface.Interfaces;


namespace PlatformAgileFramework.UserInterface.UserInteractionService
{
	/// <summary>
	/// This class implements a yes/no query/answer from the user. It is a helper
	/// class designed to allow construction of a yes/no query/response. Its single
	/// method takes a message to be displayed to the user and appends a prompt
	/// string to the end to instruct the user how to respond. The main thing this
	/// class does is construct the prompt string based on the construction arguments.
	/// </summary>
	public class StringUserQuery: IStringUserQuery
	{
		#region Class Fields
		/// <summary>
		/// Construct a version of utils for the console. This must be loaded by
		/// inheritors or through public constructor.
		/// </summary>
		protected IUIUtils m_Utils;
		/// <summary>
		/// This is the default response string that is expected to be entered from
		/// the user if none is specified in the method calls.
		/// </summary>
		protected static readonly string s_DefaultPositiveResponseString = "yes";
		/// <summary>
		/// This is the default response string that is expected to be entered from
		/// the user if none is specified in the method calls.
		/// </summary>
		protected static readonly string s_DefaultNegativeResponseString = "no";

		// These are the strings that establish the query/response for this instance.
		private readonly string m_UserPromptString;
		private readonly string m_PositiveResponseString;
		private readonly string m_NegativeResponseString;

		// This tells if we accept <CR> for a positive response.
		private readonly bool m_AcceptCrForPositive;
		#endregion
		#region Constructors
		/// <summary>
		/// Default constructor. Stuffs in default response strings <c>"yes"</c> and
		/// <c>"no"</c> for the queries.
		/// </summary>
		protected StringUserQuery()
		{
			m_PositiveResponseString = s_DefaultPositiveResponseString;
			m_NegativeResponseString = s_DefaultNegativeResponseString;
			m_UserPromptString = "(" + m_PositiveResponseString + "/" +
				m_NegativeResponseString + ")?";
			m_AcceptCrForPositive = false;
		}

		/// <summary>
		/// Constructor which accepts a <see cref="IUIUtils"/> interface to build a complete
		/// class. Delegates to <see cref="StringUserQuery(System.String, System.String, System.String, System.Boolean)"/>.
		/// </summary>
		/// <param name="positiveResponseString">
		/// See <see cref="StringUserQuery(System.String, System.String, System.String, System.Boolean)"/>.
		/// </param>
		/// <param name="negativeResponseString">
		/// See <see cref="StringUserQuery(System.String, System.String, System.String, System.Boolean)"/>.
		/// </param>
		/// <param name="userPromptString">
		/// See <see cref="StringUserQuery(System.String, System.String, System.String, System.Boolean)"/>.
		/// </param>
		/// <param name="acceptCrForPositive">
		/// See <see cref="StringUserQuery(System.String, System.String, System.String, System.Boolean)"/>.
		/// </param>
		/// <param name="uIUtils">
		/// UI utilities reference.
		/// </param>
		public StringUserQuery(string positiveResponseString,
			string negativeResponseString, string userPromptString,
			bool acceptCrForPositive, IUIUtils uIUtils)
			: this(positiveResponseString, negativeResponseString, userPromptString, acceptCrForPositive)
		{
			m_Utils = uIUtils;

		}
		/// <summary>
		/// Constructor which accepts a <see cref="IUIUtils"/> interface to build a complete
		/// class. Delegates to <see cref="StringUserQuery()"/>.
		/// </summary>
		public StringUserQuery(IUIUtils uIUtils)
			: this()
		{
			m_Utils = uIUtils;

		}
#pragma warning disable 1570
		/// <summary>
		/// Constructor which takes response strings for Positive/Negative responses.
		/// </summary>
		/// <param name="positiveResponseString">
		/// Example: <c>"Confirm"</c>. May be <see langword="null"/>, in which case
		/// <c>"yes"</c> is used.
		/// </param>
		/// <param name="negativeResponseString">
		/// Example <c>"Cancel"</c>. May be <see langword="null"/>, in which case
		/// <c>"no"</c> is used.
		/// </param>
		/// <param name="userPromptString">
		/// Example: <c>"Cancel to abort transaction or Confirm to accept"</c>.
		/// May be <see langword="null"/>. If <see langword="null"/> and either <paramref name="positiveResponseString"/>
		/// and <paramref name="negativeResponseString"/> are blank, <c>"CR to continue:"</c> is
		/// used. If both <paramref name="positiveResponseString"/> and <paramref name="negativeResponseString"/>
		/// are provided, a default prompt string is constructed if < paramref name="userPromptString"/> is
		/// passed as <see langword="null"/>. This prompt string is formed as
		/// <c>"(POSITIVERESPONSESTRING/NEGATIVERESPONSESTRING)?"</c>.
		/// </param>
		/// <param name="acceptCrForPositive">
		/// This flag tells if the user should be allowed to enter a <c>"&lt.CR&gt;"</c> for
		/// a positive response. This can sometimes be allowed when it is desired to make
		/// it easy to say "yes". In some cases, when the ramifications of a "yes" are
		/// significant (e.g. deleting files or directories) it is desired to compel the
		/// user to type in the full string. Setting this flag will cause the CR to
		/// be accepted as a positive response, irregardless of the response strings.
		/// </param>
		/// <remarks>
		/// If both strings are blank (not null), the methods always return positive.
		/// </remarks>
#pragma warning restore 1570
		protected StringUserQuery(string positiveResponseString,
			string negativeResponseString, string userPromptString,
			bool acceptCrForPositive)
			: this()
		{
			if (positiveResponseString != null)
				m_PositiveResponseString = positiveResponseString;
			if (negativeResponseString != null)
				m_NegativeResponseString = negativeResponseString;
			if (userPromptString != null)
				m_UserPromptString = userPromptString;
			else if ((m_PositiveResponseString != "") && (m_NegativeResponseString != ""))
				m_UserPromptString = "(" + m_PositiveResponseString + "/" +
					m_NegativeResponseString + ")?";
			// This last case is used for just outputting a string for the
			// user to see that just requires a keypress to dismiss.
			else
				m_UserPromptString = "<CR> to continue:";
			// We wait to convert to lower here, since we want the default
			// constructed message to embed the user's original strings, if applicable.
			m_PositiveResponseString = m_PositiveResponseString.ToLower();
			m_NegativeResponseString = m_NegativeResponseString.ToLower();
			// Is <CR> ok?
			m_AcceptCrForPositive = acceptCrForPositive;
		}
		#endregion
		#region Methods
		/// <summary>
		/// This method asks a question at the console and returns the positive
		/// or negative response as the <see cref="UserInteractionStatus"/>. The
		/// user must respond with a text string. The string is compared with the
		/// positive and negative response strings after converting to lower case.
		/// if the user's response is a substring of the positive response string,
		/// a positive response is returned. If both response strings are blank,
		/// a positive response is returned. Otherwise, a negative response is
		/// returned.
		/// </summary>
		/// <param name="messageString">
		/// This is a message that is to be presented to the user before the user
		/// is prompted for a response. The prompt is appended to the message.
		/// Example: <c>"Can't find file"</c>. May be <see langword="null"/>, in which case
		/// no message is prepended to the prompt.
		/// </param>
		/// <returns>
		/// A <see cref="UserInteractionStatus"/> enum representing the status of the
		/// user interaction. In this case, the <c>"UserInteractionStatus.PositiveResult"</c>
		/// flag (bit 1) corresponds to a positive result <c>(1)</c> or a negetive
		/// result <c>(0)</c>.
		/// </returns>
		public virtual UserInteractionStatus AskQuestion(string messageString)
		{
			// Save a little duplication.
			var positiveResult
				= UserInteractionStatus.PositiveResultBit;
			var negativeResult
				= (~UserInteractionStatus.PositiveResultBit) & UserInteractionStatus.PositiveResultBit;
			// Our I/O strings.
			var stringToOutput = m_UserPromptString;
			// If we've got a message, prepend it.
			if (messageString != null)
				stringToOutput = messageString + stringToOutput;
			// Present to user and elicit response.
			m_Utils.WriteToConsole(stringToOutput);
			var userResponseString = m_Utils.ReadFromConsole();
			// Handle degenerate cases before we do anything else.
			if ((m_PositiveResponseString == "") && (m_NegativeResponseString == ""))
				return positiveResult;
			if (m_AcceptCrForPositive && (string.IsNullOrEmpty(userResponseString)))
				return positiveResult;
			// Get in lower case and compare.
			userResponseString = userResponseString.ToLower();
			if (userResponseString.IndexOf(m_PositiveResponseString, StringComparison.Ordinal) >= 0)
				return positiveResult;
			return negativeResult;
		}
		/// <summary>
		/// This method prints a string at the console and always returns the positive
		/// response as the <see cref="UserInteractionStatus"/>.
		/// </summary>
		/// <param name="messageString">
		/// This is a message that is to be presented to the user.
		/// </param>
		/// <returns>
		/// A <see cref="UserInteractionStatus"/> enum representing the status of the
		/// user interaction. In this case, the <c>"UserInteractionStatus.PositiveResult"</c>
		/// flag (bit 1) corresponds to a positive result and is what is always returned.
		/// </returns>
		public virtual UserInteractionStatus PrintMessage(string messageString)
		{
			// If we've got a message, prepend it.
			if (messageString != null)
			// Present to user and return.
			m_Utils.WriteToConsole(messageString);
			return UserInteractionStatus.PositiveResultBit;
		}
		#endregion
	}
}
