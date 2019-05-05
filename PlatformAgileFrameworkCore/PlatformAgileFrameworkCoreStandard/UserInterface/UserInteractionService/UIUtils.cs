//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2005 - 2017 Icucom Corporation
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
	/// This class does basic read/write and other things. Pattern is a class exposing
	/// virtual methods delegating to a contained object.
	/// </summary>
	public class UIUtils: IUIUtils
	{
		#region Class Fields
		/// <summary>
		/// Our protected string I/O provider. This is the contained provider.
		/// </summary>
		protected IStringIO m_IStringIOProvider;
		#endregion
		#region Constructors
		/// <summary>
		/// Default constructor normally builds with a specific <see cref="IStringIO"/>.
		/// </summary>
		protected UIUtils() { }
		/// <summary>
		/// Builds with our I/O provider and results in a usable object.
		/// </summary>
		/// <param name="stringIO">
		/// Type implementing <see cref="IStringIO"/>.
		/// </param>
		public UIUtils(IStringIO stringIO)
		{
			m_IStringIOProvider = stringIO;
		}
		#endregion // Constructors

		/// <summary>
		/// This method simply reads a string from the console after the
		/// user terminates it by hitting the <c>"Enter"</c> key.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> read from the console.
		/// </returns>
		public virtual string ReadFromConsole()
		{
			// Just read using .Net console routines.
			return (string)m_IStringIOProvider.ReadString();
		}
		/// <summary>
		/// This method gets the internal <see cref="IStringIO"/> provider.
		/// </summary>
		public IStringIO GetStringIOProvider() { return m_IStringIOProvider; }

		/// <summary>
		/// This method is designed to replace the internal <see cref="IStringIO"/>
		/// interface provider with another. It is designed to be used for substituting
		/// a mock object for a real console IO provider for the purpose of testing.
		/// </summary>
		/// <param name="provider">
		/// A <see cref="IStringIO"/> provider - usually reads/writes from a buffer
		/// of strings.
		/// </param>
		/// <remarks>
		/// The setting of the string provider is done here at the level of the
		/// overall console I/O system, since it is designed only for testing and the
		/// assumption is that we won't be doing anything else.
		/// </remarks>
		public virtual void SetStringIOProvider (IStringIO provider)
		{ m_IStringIOProvider = provider; }

		/// <summary>
		/// This method writes a string to the console and reads a user's response.
		/// </summary>
		/// <param name="consoleOutputString">
		/// A <see cref="System.String"/> to write to the console. No line
		/// termination characters are added.
		/// </param>
		/// <returns>
		/// A <see cref="System.String"/> read from the console.
		/// </returns>
		public virtual string WriteReadConsole(string consoleOutputString)
		{
			// Write/Read using our IO interface.
			m_IStringIOProvider.WriteString(consoleOutputString);
			return ReadFromConsole();
		}

		/// <summary>
		/// This method simply writes a string to the console.
		/// </summary>
		/// <param name="consoleOutputString">
		/// A <see cref="System.String"/> to write to the console. No line
		/// termination characters are added.
		/// </param>
		public virtual void WriteToConsole(string consoleOutputString)
		{
			// Write using our IO interface.
			m_IStringIOProvider.WriteString(consoleOutputString);
			return;
		}
		/// <summary>
		/// Base method does nothing.
		/// </summary>
		// TODO - KRM wrong pattern......	
		public virtual void Dispose() { }
	}
}
