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

using System;

namespace PlatformAgileFramework.UserInterface.Interfaces
{
	/// <summary>
	/// This interface defines a closed specialization of the open interface
	/// <see cref="IUserInteraction{T,U}"/>. Methods here are designed to display
	/// an error message string to the user and wait for a keypress or
	/// a mouse click on a button. Whatever key or button the user presses,
	/// the interface methods should cause the
	/// <see cref="UserInteractionStatus.PositiveResultBit"/> bit to be set.
	/// The <see cref="System.Enum"/> <c>out</c> parameter is a copy of the
	/// return value.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 16jan2018 </date>
	/// <description>
	/// Added history to this one and renamed from "IMessageAndDispatch"
	/// for definiteness.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Usually called from Main thread...
	/// </threadsafety>
	public interface IErrorMessageAndDispatch : IUserInteraction<string, Enum> { }
}
