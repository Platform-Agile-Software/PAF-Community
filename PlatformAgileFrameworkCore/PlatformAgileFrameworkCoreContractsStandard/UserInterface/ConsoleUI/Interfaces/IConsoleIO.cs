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
using PlatformAgileFramework.UserInterface.Interfaces;


namespace PlatformAgileFramework.UserInterface.ConsoleUI.Interfaces
{
	/// <summary>
	/// This is an internal interface that is used to perform IO to/from
	/// the console. It inherits from the basic <see cref="IStringIO"/>
	/// interface, but adds supports for logging and/or redirecting the
	/// error output. This interface can very well be used in Console-based
	/// or GUI-based applications.
	/// </summary>
	internal interface IConsoleIO: IStringIO
	{
		/// <summary>
		/// Just sends a string somewhere. In the real application, this is normally
		/// something like a Console.Error.Write() or an output to a text or message window
		/// in a GUI-based application. In tests it is normally just a buffer that can
		/// be checked by the tester to see that the appropriate thing is written.
		/// When the UI is set up, a logfile can be attached for the application to
		/// write to.
		/// </summary>
		/// <param name ="stringToWrite">
		/// The string to be written.
		/// </param>
		void WriteErrorString(string stringToWrite);
	}
}
