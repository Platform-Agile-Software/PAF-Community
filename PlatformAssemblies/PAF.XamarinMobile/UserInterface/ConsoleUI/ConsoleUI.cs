//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 Icucom Corporation
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
using PlatformAgileFramework.UserInterface.UserInteractionService;

namespace PlatformAgileFramework.UserInterface.ConsoleUI
{
	/// <summary>
	/// This class implements a message to the user and waits for a string to be
	/// entered and returns it.
	/// </summary>
	public class ConsoleMessageAndStringResponse : IMessageAndStringResponse
	{
		#region Class Fields
		/// <summary>
		/// Holds a version of utils for the console.
		/// </summary>
		protected IUIUtils m_Utils;
		#endregion // Class Fields
		#region Constructors
		/// <summary>
		/// Default constructor - builds with the default <see cref="ConsoleUIUtils"/>.
		/// </summary>
		public ConsoleMessageAndStringResponse()
		{
			m_Utils = new ConsoleUIUtils();
		}
		/// <summary>
		/// Constructor - builds with a <see cref="IUIUtils"/>.
		/// </summary>
		public ConsoleMessageAndStringResponse(IUIUtils utils)
		{
			m_Utils = utils;
		}
		#endregion

		/// <summary>
		/// This method waits for a string response from the console after printing
		/// the message. See <see cref="IMessageAndStringResponse"/> for details.
		/// </summary>
		/// <param name="messageString">
		/// See <see cref="IMessageAndStringResponse"/>.
		/// </param>
		/// <param name="userResult">
		/// This is the string that the user entered.
		/// </param>
		/// <returns>
		/// <see cref="UserInteractionStatus.PositiveResultBit"/> is set if the user
		/// entered something other than a "CR".
		/// </returns>
		public virtual Enum PresentToUser(String messageString, out String userResult)
		{
			// Need a reference to catch the out parameter.
			String result;
			// Collect user response.
			result = m_Utils.WriteReadConsole(messageString);
			userResult = result;
			if (String.IsNullOrEmpty(userResult))
				return (~UserInteractionStatus.PositiveResultBit) & UserInteractionStatus.PositiveResultBit;
			else
				return UserInteractionStatus.PositiveResultBit;

		}

		/// <summary>
		/// Unlike the previous method, the method does not return the
		/// <see cref="System.String"/> user response value as an output parameter.
		/// Clients can use this method if they just want to check for a "CR".
		/// This method calls the previous method to do it's work. See that method
		/// for further details.
		/// </summary>
		/// <param name="messageString">
		/// See other method.
		/// </param>
		/// <returns>
		/// See other method.
		/// </returns>
		public virtual Enum PresentToUser(String messageString)
		{
			// Need a reference to catch the out parameter.
			String result;
			// Just call the other method to do the work.
			return PresentToUser(messageString, out result);
		}
		#region IDisposable interface
		/// <summary>
		/// Virtual dispose method.
		/// </summary>
		public virtual void Dispose() { }
		#endregion // IDisposable interface
	}
	#region IMessageWithNoResponse Implementation
	/// <summary>
	/// This class implements a message to the user and returns.
	/// </summary>
	public class ConsoleMessageWithNoResponse : IMessageWithNoResponse
	{
		#region Class Fields
		#endregion // Class Fields
		/// <summary>
		/// Holds a version of utils for the console.
		/// </summary>
		protected IUIUtils m_Utils;
		/// <summary>
		/// This is our little contained query class for waiting for the response.
		/// </summary>
		protected IStringUserQuery m_IStringUserQuery;
		#region Constructors
		/// <summary>
		/// Default constructor - builds with the default <see cref="ConsoleUIUtils"/>.
		/// </summary>
		public ConsoleMessageWithNoResponse()
		{
			m_Utils = new ConsoleUIUtils();
			// Construct with two blank response strings with blank prompt
			// and allowance for <CR> as a positive response (true), although
			// this last won't matter, since we have two blanks anyway.
			m_IStringUserQuery = new StringUserQuery("", "", "", true, m_Utils);
		}
		/// <summary>
		/// Constructor - builds with a <see cref="IUIUtils"/>.
		/// </summary>
		public ConsoleMessageWithNoResponse(IUIUtils utils)
		{
			m_Utils = utils;
			// Construct with two blank response strings with autogenerated prompt
			// (null) and allowance for <CR> as a positive response (true), although
			// this last won't matter, since we have two blanks anyway.
			m_IStringUserQuery = new StringUserQuery("", "", null, true, m_Utils);
		}
		#endregion

		/// <summary>
		/// This method waits for a string response from the console after printing
		/// the message. See <see cref="IMessageWithNoResponse"/> for details.
		/// </summary>
		/// <param name="messageString">
		/// See <see cref="IMessageWithNoResponse"/>.
		/// </param>
		/// <param name="userResult">
		/// This is the string that the user entered.
		/// </param>
		/// <returns>
		/// <see cref="UserInteractionStatus.PositiveResultBit"/> is set if the user
		/// entered something other than a "CR".
		/// </returns>
		public virtual Enum PresentToUser(String messageString, out Enum userResult)
		{
			// Need a reference to catch the out parameter.
			Enum status;
			// Display at console.
			status = m_IStringUserQuery.PrintMessage(messageString);
			// Load and go home....
			userResult = status;
			return status;

		}

		/// <summary>
		/// Unlike the previous method, the method does not return the
		/// <see cref="System.String"/> user response value as an output parameter.
		/// Clients can use this method if they just want to check for a "CR".
		/// This method calls the previous method to do it's work. See that method
		/// for further details.
		/// </summary>
		/// <param name="messageString">
		/// See other method.
		/// </param>
		/// <returns>
		/// See other method.
		/// </returns>
		public virtual Enum PresentToUser(String messageString)
		{
			// Need a reference to catch the out parameter.
			Enum result;
			// Just call the other method to do the work.
			return PresentToUser(messageString, out result);
		}
		#region IDisposable interface
		/// <summary>
		/// Virtual dispose method.
		/// </summary>
		public virtual void Dispose() { }
		#endregion // IDisposable interface
	}
	#endregion
	#region IMessageOkQuit Implementation
	/// <summary>
	/// This class implements a message to the user with positive/negative response
	/// in the form of "(Ok/Quit)?".
	/// </summary>
	public class ConsoleMessageOkQuit : IMessageOkQuit
	{
		#region Class Fields
		/// <summary>
		/// Holds a version of utils for the console.
		/// </summary>
		protected IUIUtils m_Utils;
		/// <summary>
		/// This is our little contained query class for waiting for the response.
		/// </summary>
		protected IStringUserQuery m_IStringUserQuery;
		#endregion // Class Fields
		#region Constructors
		/// <summary>
		/// Default constructor - builds with the default <see cref="ConsoleUIUtils"/>.
		/// </summary>
		public ConsoleMessageOkQuit()
		{
			m_Utils = new ConsoleUIUtils();
			// Construct for Ok/Quit with autogenerated prompt (null) and
			// allowance for <CR> as a positive response (true).
			m_IStringUserQuery = new StringUserQuery("Ok", "Quit", null, true, m_Utils);
		}
		/// <summary>
		/// Constructor - builds with a <see cref="IUIUtils"/>.
		/// </summary>
		public ConsoleMessageOkQuit(IUIUtils utils)
		{
			m_Utils = utils;
			// Construct with two blank response strings with autogenerated prompt
			// (null) and allowance for <CR> as a positive response (true), although
			// this last won't matter, since we have two blanks anyway.
			m_IStringUserQuery = new StringUserQuery("Ok", "Quit", null, true, m_Utils);
		}
		#endregion

		/// <summary>
		/// This method asks the Ok/Quit question at the console after printing
		/// the message. See <see cref="IMessageOkQuit"/> for details.
		/// </summary>
		/// <param name="messageString">
		/// See <see cref="IMessageOkQuit"/>.
		/// </param>
		/// <param name="userResult">
		/// See <see cref="IMessageOkQuit"/>.
		/// </param>
		/// <returns>
		/// See <see cref="IMessageOkQuit"/>.
		/// </returns>
		public virtual Enum PresentToUser(String messageString, out Enum userResult)
		{
			// Need a reference to catch the out parameter.
			Enum status;
			// Elicit the user's yes/no.
			status = m_IStringUserQuery.AskQuestion(messageString);
			// Load and go home....
			userResult = status;
			return status;
		}

		/// <summary>
		/// Unlike the previous method, the method does not return the
		/// <see cref="UserInteractionStatus"/> value as an output parameter.
		/// Clients will normally use this method, since the "out" parameter
		/// in the other method is redundant in this class. This class method
		/// calls the previous method to do it's work. See that method for further
		/// details.
		/// </summary>
		/// <param name="messageString">
		/// See other method.
		/// </param>
		/// <returns>
		/// See other method.
		/// </returns>
		public virtual Enum PresentToUser(String messageString)
		{
			// Need a reference to catch the out parameter.
			Enum status;
			// Just call the other method to do the work.
			return PresentToUser(messageString, out status);
		}
		#region IDisposable interface
		/// <summary>
		/// Virtual dispose method.
		/// </summary>
		public virtual void Dispose() { }
		#endregion // IDisposable interface
	}
	#endregion
	#region IYesNoQuery Implementation
	/// <summary>
	/// This class implements a yes/no query/answer from the user.
	/// </summary>
	public class ConsoleYesNoQuery : IYesNoQuery
	{
		#region Class Fields
		/// <summary>
		/// Holds a version of utils for the console.
		/// </summary>
		protected IUIUtils m_Utils;
		/// <summary>
		/// This is our little contained query class for waiting for the response.
		/// </summary>
		protected IStringUserQuery m_IStringUserQuery;
		#endregion // Class Fields
		#region Constructors
		/// <summary>
		/// Default constructor - builds with the default <see cref="ConsoleUIUtils"/>.
		/// </summary>
		public ConsoleYesNoQuery()
		{
			m_Utils = new ConsoleUIUtils();
			// nulls give us a default yes/no query. Don't allow a <CR>
			// for positive (false).
			m_IStringUserQuery = new StringUserQuery(null, null, null, false, m_Utils);
		}
		/// <summary>
		/// Constructor - builds with a <see cref="IUIUtils"/>.
		/// </summary>
		public ConsoleYesNoQuery(IUIUtils utils)
		{
			m_Utils = utils;
			// Construct with two blank response strings with autogenerated prompt
			// (null) and allowance for <CR> as a positive response (true), although
			// this last won't matter, since we have two blanks anyway.
			m_IStringUserQuery = new StringUserQuery(null, null, null, false, m_Utils);
		}
		#endregion

		/// <summary>
		/// This method asks a question at the console and returns the yes/no
		/// response as the <see cref="UserInteractionStatus"/>. The user must
		/// respond with either <c>"yes"</c> or <c>"no"</c>. <c>"y"</c> or <c>"Y"</c>
		/// or <c>"n"</c> or <c>"N"</c> are adequate.
		/// </summary>
		/// <param name="userPromptString">
		/// A <see cref="System.String"/> message to write to the console before
		/// the query for a yes/no answer. An example would be <c>quit</c>.
		/// In this case, the prompt string will be created as <c>"quit(Yes/No)?"</c>.
		/// This string may be <see langword="null"/>. 
		/// </param>
		/// <param name="userResult">
		/// A copy of the return value.
		/// </param>
		/// <returns>
		/// A System.Enum representing the status of the user interaction. This
		/// value is a CLR-compliant version of the C# <see cref="UserInteractionStatus"/>
		/// <c>enum</c>. In this case, the <c>"UserInteractionStatus.PositiveResult"</c>
		/// flag (bit 1) corresponds to yes <c>(1)</c> or no <c>(0)</c>.
		/// </returns>
		public virtual Enum PresentToUser(String userPromptString, out Enum userResult)
		{
			// Need a reference to catch the out parameter.
			Enum status;
			// Elicit the user's yes/no.
			status = m_IStringUserQuery.AskQuestion(userPromptString);
			// Load and go home....
			userResult = status;
			return status;
		}

		/// <summary>
		/// Unlike the previous method, the method does not return the
		/// <see cref="UserInteractionStatus"/> value as an output parameter.
		/// Clients will normally use this method, since the "out" parameter
		/// in the other method is redundant in this class. This class method
		/// calls the previous method to do it's work. See that method for further
		/// details.
		/// </summary>
		/// <param name="userPromptString">
		/// See other method.
		/// </param>
		/// <returns>
		/// See other method.
		/// </returns>
		public virtual Enum PresentToUser(String userPromptString)
		{
			// Need a reference to catch the out parameter.
			Enum status;
			// Just call the other method to do the work.
			return PresentToUser(userPromptString, out status);
		}
		#region IDisposable interface
		/// <summary>
		/// Virtual dispose method.
		/// </summary>
		public virtual void Dispose() { }
		#endregion // IDisposable interface
	}
	#endregion
}
