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
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Events.Execution.Commands;
using PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Display.Exceptions;
using PlatformAgileFramework.TypeHandling.AggregableObjectArguments;

// Exception shorthand.
using PTRNED = PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Display.Exceptions.PAFTestResultNavigationExceptionData;
using IPTRNED = PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Display.Exceptions.IPAFTestResultNavigationExceptionData;


namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Display.Commands
{
	/// <summary>
	/// An implementation of <see cref="IPAFTestResultInfoCommand"/> for
	/// navigating to the previous node at a given current node level in the tree.
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
	// ReSharper disable once InconsistentNaming
	public class PAFTestResultInfoPRCommand : PAFTestResultInfoCommand
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Our name.
		/// </summary>
		public const string COMMAND_NAME = "PR";
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// This constructor builds with a <see cref="IPAFTestElementResultInfo"/>
		/// node that is needed to determine if the command is executable.
		/// Cannot be <see langword="null"/>.
		/// </summary>
		/// <param name="resultInfo">The node in the tree of results.</param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="resultInfo"/>
		/// </exception>
		/// </exceptions>
		public PAFTestResultInfoPRCommand(IPAFTestElementResultInfo resultInfo)
			:base(resultInfo)
		{
			if(resultInfo == null)
				throw new ArgumentNullException("resultInfo");
			m_ResultInfo = resultInfo;
			m_CommandName = COMMAND_NAME;
			m_IsExecutable = resultInfo.HasPriorDisplaySibling() != true;
		}
		#endregion // Constructors

		/// <summary>
		/// For us, we can execute if we have a sibling to the left.
		/// If there is a change in executability, the event is fired. 
		/// </summary>
		/// <param name="parameter">Not used.</param>
		/// <returns>
		/// <see langword="true"/> if there is somebody to our left.
		/// </returns>
		public override bool CanExecute(object parameter)
		{
			var canExecute = m_ResultInfo.HasPriorDisplaySibling();
			IsExecutable = canExecute;
			return canExecute;
		}

		/// <summary>
		/// Goes up to the parent node.
		/// </summary>
		/// <param name="parameter">
		/// This must be a <see cref="IPAFExceptionArgumentHolder"/>.
		/// </param>
		/// <remarks>
		/// We have a static tree, so we use state-based indication
		/// of whether commands are executable or not. This requires
		/// us to set <see cref="IPAFNamedCommand.IsExecutable"/> when
		/// we are finished. Note exceptions are RETURNED in our model,
		/// not thrown.
		/// </remarks>
		/// <exceptions>
		/// <exception cref="PAFStandardException{IPTRNED}">
		/// <see cref="PAFTestResultNavigationExceptionMessageTags.NODE_IS_FIRST_CHILD"/>
		/// </exception>
		/// </exceptions>
		public override void Execute(object parameter)
		{
			if (!m_ResultInfo.HasPriorDisplaySibling())
			{
				var data = new PTRNED(PAFTestResultNavigationExceptionMessageTags.NODE_IS_FIRST_CHILD);
				var newEx = new PAFStandardException<IPTRNED>(data);
				IsExecutable = false;
				((IPAFExceptionArgumentHolder)parameter).Argument = newEx;
				return;
			}

			m_ResultInfo = m_ResultInfo.GoToPriorDisplaySibling();
			if (!m_ResultInfo.HasPriorDisplaySibling())
			{
				// Can't go back anymore.
				IsExecutable = false;
				return;
			}

			IsExecutable = true;
		}
	}
}