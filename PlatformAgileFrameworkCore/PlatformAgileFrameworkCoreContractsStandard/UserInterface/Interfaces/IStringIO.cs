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

namespace PlatformAgileFramework.UserInterface.Interfaces
{
	/// <summary>
	/// This is an interface that is used mostly on mock objects
	/// for testing. It must be implemented by Types that wish to provide
	/// a string input/output capability to the user if it is desired to
	/// provide mock stimuli to simulate user interaction, whether it is
	/// console-based or GUI-based.
	/// </summary>
	public interface IStringIO
	{
		/// <summary>
		/// Just gets a string from somewhere. In the real application, this is
		/// normally a Console.ReadLine() or input from a text window. In tests it
		/// is normally fed from a buffer of strings.
		/// </summary>
		/// <returns>
		/// The string returned.
		/// </returns>
		string ReadString();

		/// <summary>
		/// Just sends a string somewhere. In the real application, this is normally
		/// something like a Console.Write() or an output to a text or message window
		/// in a GUI-based application. In tests it is normally just a buffer that can
		/// be checked by the tester to see that the appropriate thing is written.
		/// </summary>
		/// <param name ="stringToWrite">
		/// The string to be written.
		/// </param>
		void WriteString(string stringToWrite);
	}
}
