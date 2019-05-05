//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2018 Icucom Corporation
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
using System.Windows.Input;

namespace PlatformAgileFramework.Execution.Commands
{
	/// <summary>
	/// Allows a command to have a name.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 27dec2017 </date>
	/// <description>
	/// New. Needed for console UI.
	/// </description>
	/// </contribution>
	/// </history>
	public interface IPAFNamedCommand: ICommand
	{
		#region Properties
		/// <summary>
		/// The textual name of the command. Command names are usually
		/// created to be unique so they can be put into a dictionary.
		/// </summary>
		string CommandName { get; }
		/// <summary>
		/// Needed to determine state change - we need the state to be remembered.
		/// <see langword="null"/> means we did not call <see cref="ICommand.CanExecute"/>
		/// yet.
		/// </summary>
		bool? IsExecutable { get; set; }
		#endregion // Properties
		#region Methods
		/// <summary>
		/// In our work, we need a way to trap erroneous execution WITHOUT
		/// throwing an exception. We do, however, indicate what's wrong
		/// with an exception. Typically this method can be used to pre-filter
		/// command executions.
		/// </summary>
		/// <param name="parameter">Parameter for the execution method.</param>
		/// <returns>Typically <see langword="null"/> if all is well.</returns>
		/// <remarks>
		/// This method is useful where there is ambiguity about whether a command
		/// can execute, even after calling <see cref="ICommand.CanExecute"/>.
		/// </remarks>
		Exception ExecuteCommand(object parameter);
		#endregion // Methods

	}
}