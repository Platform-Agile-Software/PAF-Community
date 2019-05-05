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
	/// This class does basic read/write and other things. It's built with the shell
	/// class <see cref="UIUtils"/>.
	/// </summary>
	public class ConsoleUIUtils : UIUtils
	{
		#region Constructors
		/// <summary>
		/// Default constructor builds us with a console version of <see cref="IStringIO"/>
		/// </summary>
		public ConsoleUIUtils() : base(new ConsoleIOString()) { }
		/// <summary>
		/// Constructor builds us with an arbitrary version of <see cref="IStringIO"/>.
		/// </summary>
		public ConsoleUIUtils(IStringIO iStringIO) : base(iStringIO) { }
		#endregion // Constructors
	}

	#region IStringIO Implementations
	/// <summary>
	/// This class implements the basic IO interface for production code. It does
	/// IO to the console.
	/// </summary>
	class ConsoleIOString : IStringIO
	{
		/// <summary>
		/// This is the standard console read.
		/// </summary>
		/// <returns>
		/// The string read in.
		/// </returns>
		public String ReadString()
		{
			// Just read using .Net console routines.
			return Console.ReadLine();
		}

		/// <summary>
		/// This is the standard console write.
		/// </summary>
		/// <param name="stringToWrite"/>
		/// The string to write out with no CR, no line feed appended.
		public void WriteString(String stringToWrite)
		{
			// Just write it out using .Net console routines.
			Console.Write(stringToWrite);
		}
	}
	#endregion
}
