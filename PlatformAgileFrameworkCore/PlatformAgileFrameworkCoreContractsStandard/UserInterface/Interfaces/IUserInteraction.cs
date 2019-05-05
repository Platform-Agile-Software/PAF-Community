//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2005 - 2010 - 2016 Icucom Corporation
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
	/// This enumeration is the lowest common denominator for all MPFramework
	/// <see cref="IUserInteraction{T,U}"/> returns. It is designed to be "inherited"
	/// from within the framework within higher-level UI functions.
	/// </summary>
	[Flags]
	public enum UserInteractionStatus
	{
		/// <summary>
		/// Bit 1 indicates a positive/negative yes/no return from the interaction.
		/// It can be used to represent a negative or affirmative response from the
		/// user or for any other purpose.
		/// </summary>
		PositiveResultBit = 1,
		/// <summary>
		/// Mask for the result field.
		/// </summary>
		PositiveResultFieldMask = 1
	}

	/// <summary>
	/// <para>
	/// This interface defines a protocol for MPFramework utilities to interact with
	/// the user. The interface defines platform-independent protocols for presenting
	/// something to the user and getting a response. The response may be as simple
	/// as a yes/no or the specification of a string. This interface is intended
	/// to be implemented by console dialogs and window-based dialogs. Console-based
	/// dialogs are typically implemented by servers that do not necessarily have
	/// a GUI. <see cref="IUserInteraction&lt;TUserQuery,KUserResult&gt;"/> is the
	/// base interface that all MPFramework user dialogs must implement.
	/// </para>
	/// </summary>
	public interface IUserInteraction<in TUserQuery, KUserResult>: IDisposable
	{
		/// <summary>
		/// This method presents a query to the user and collects a response all in
		/// one step.
		/// </summary>
		/// <param name="userQuery">
		/// An instance of a Type containing information to be presented to the user.
		/// This could be a simple string to place in an error message box, for example.
		/// </param>
		/// <param name="userResult">
		/// An instance of a Type containing information about a user's response.
		/// This could be a filename typed in within a file load dialog. If the user
		/// interaction does not require a response, a copy of the
		/// <see cref="UserInteractionStatus"/> could be returned in this output
		/// parameter.
		/// </param>
		/// <returns>
		/// A System.Enum representing the status of the user interaction. This
		/// value is a CLR-compliant version of the C# <see cref="UserInteractionStatus"/>
		/// <c>enum</c>.
		/// </returns>
		Enum PresentToUser(TUserQuery userQuery, out KUserResult userResult);

		/// <summary>
		/// This method presents a query to the user. It is designed to be used
		/// when only a positive/negative response needs to be collected.
		/// </summary>
		/// <param name="userQuery">
		/// An instance of a Type containing information to be presented to the user.
		/// This could be a simple string to place in an error message box, for example.
		/// </param>
		/// <returns>
		/// A System.Enum representing the status of the user interaction. This
		/// value is a CLR-compliant version of the C# <see cref="UserInteractionStatus"/>
		/// <c>enum</c>.
		/// </returns>
		Enum PresentToUser(TUserQuery userQuery);
	}
	/// <summary>
	/// <para>
	/// This interface exposes a method to set the internal string provider
	/// for testing using mock UI interaction.
	/// </para>
	/// </summary>
	internal interface IUserInteractionInternal<in TUserQuery, KUserResult>: IUserInteraction<TUserQuery, KUserResult>
	{
		/// <summary>
		/// This method gets the internal <see cref="IStringIO"/> provider.
		/// </summary>
	 	IStringIO GetStringIOProvider();
		/// <summary>
		/// This method sets the internal <see cref="IStringIO"/> provider.
		/// </summary>
		void SetStringIOProvider(IStringIO stringIO);
	}
}
