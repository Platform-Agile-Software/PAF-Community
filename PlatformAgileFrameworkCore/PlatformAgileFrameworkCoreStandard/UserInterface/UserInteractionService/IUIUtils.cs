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
	/// This interface does basic read/write and other things. These methods
	/// are designed to read/write abstractly from/to a "console", whatever that
	/// may be. It might be a window in a windows-oriented implementation.
	/// </summary>
	public interface IUIUtils: IDisposable
	{
		/// <summary>
		/// This method gets the internal <see cref="IStringIO"/> provider.
		/// </summary>
		IStringIO GetStringIOProvider();

		/// <summary>
		/// This method simply reads a string from the console after the
		/// user terminates it by hitting the <c>"Enter"</c> key.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> read from the console.
		/// </returns>
		string ReadFromConsole();

		/// <summary>
		/// This method is designed to replace the internal <see cref="IStringIO"/>
		/// interface provider with another. It is designed to be used for substituting
		/// a mock object for a real console IO provider for the purpose of testing.
		/// </summary>
		/// <param name="provider">
		/// A <see cref="IStringIO"/> provider - usually reads/writes from a buffer
		/// of strings.
		/// </param>
		void SetStringIOProvider (IStringIO provider);

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
		string WriteReadConsole(string consoleOutputString);

		/// <summary>
		/// This method simply writes a string to the console.
		/// </summary>
		/// <param name="consoleOutputString">
		/// A <see cref="System.String"/> to write to the console. No line
		/// termination characters are added.
		/// </param>
		void WriteToConsole(string consoleOutputString);
	}
}
