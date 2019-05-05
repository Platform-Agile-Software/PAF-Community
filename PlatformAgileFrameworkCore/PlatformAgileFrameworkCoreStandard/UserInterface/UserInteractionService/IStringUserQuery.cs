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

using PlatformAgileFramework.UserInterface.Interfaces;

namespace PlatformAgileFramework.UserInterface.UserInteractionService
{
	/// <summary>
	/// This interface describes the protocol for obtaining a yes/no query/answer
	/// from the user. It is a helper interface designed to allow construction of
	/// a yes/no query/response. The interface provides an abstraction layer to
	/// a "console", whatever that may be.
	/// </summary>
	public interface IStringUserQuery
	{
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
		UserInteractionStatus AskQuestion(string messageString);
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
		UserInteractionStatus PrintMessage(string messageString);
		#endregion
	}
}
