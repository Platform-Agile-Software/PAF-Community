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
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-


using System;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Execution.Commands;
using PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Display.Exceptions;
using PlatformAgileFramework.TypeHandling.AggregableObjectArguments;

// Exception shorthand.
using PTRNED = PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Display.Exceptions.PAFTestResultNavigationExceptionData;
using IPTRNED = PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Display.Exceptions.IPAFTestResultNavigationExceptionData;


namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Display.Commands
{
	/// <summary>
	/// An implementation of <see cref="IPAFTestResultInfoCommand"/> for
	/// navigating OVer to another node with a certain Index at the current
	/// node level. "OVI"...
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
	public class PAFTestResultInfoOVICommand : PAFTestResultInfoCommand
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Our name.
		/// </summary>
		public const string COMMAND_NAME = "OVI";
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
		/// <remarks>
		/// The only thing we can precheck executability for is if we are
		/// at the root.
		/// </remarks>
		public PAFTestResultInfoOVICommand(IPAFTestElementResultInfo resultInfo)
			:base(resultInfo)
		{
			if(resultInfo == null)
				throw new ArgumentNullException("resultInfo");
			m_ResultInfo = resultInfo;
			m_CommandName = COMMAND_NAME;
			m_IsExecutable = resultInfo.IsAtRoot() != true;
		}
		#endregion // Constructors

		/// <summary>
		/// For us, we can execute if we are not at the root node and the
		/// numbered sibling exists. If there is a change in executability,
		/// the event is fired. This can be called after the command and
		/// index is parsed for a validity check on the index.
		/// </summary>
		/// <param name="parameter">
		/// This must be an <see cref="IPAFIntArgumentHolder"/> and be a
		/// valid sibling number.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if not at the root and the sibling index is valid.
		/// </returns>
		/// <remarks>
		/// Passing an index of 0 always works.
		/// </remarks>
		public override bool CanExecute(object parameter)
		{
			var siblingIndex = ((IPAFIntArgumentHolder)parameter).Argument;
			var canExecute
				= ((m_ResultInfo.IsAtRoot() != true) && (m_ResultInfo.HasNumberedDisplaySibling(siblingIndex)));
			// If we are at the root, we can not execute no matter what the index is.
			if (m_ResultInfo.IsAtRoot())
				IsExecutable = false;
			// If we are not, we could execute with the right parameters.
			else
				IsExecutable = true;
			return canExecute;
		}

		/// <summary>
		/// Goes over to a sibling node.
		/// </summary>
		/// <param name="parameter">
		/// This must be an <see cref="IPAFIntArgumentHolder"/> and a
		/// a <see cref="IPAFExceptionArgumentHolder"/>
		/// and hold a valid sibling number.
		/// </param>
		/// <remarks>
		/// We have a static tree, so we use state-based indication
		/// of whether commands are executable or not. This requires
		/// us to set <see cref="IPAFNamedCommand.IsExecutable"/> when
		/// we are finished, if we can.
		/// </remarks>
		/// <exceptions>
		/// <exception cref="PAFStandardException{IPTRNED}">
		/// <see cref="PAFTestResultNavigationExceptionMessageTags.NODE_IS_LEAF_NODE"/>
		/// </exception>
		/// <exception cref="PAFStandardException{IPTRNED}">
		/// <see cref="PAFTestResultNavigationExceptionMessageTags.NODE_IS_OUT_OF_RANGE"/>
		/// </exception>
		/// </exceptions>
		/// <remarks>
		/// We can't reasonably check if the same command can be executed again, since
		/// the check would make no sense, generally. We disable it if we are at the root node,
		/// though. The index can't be checked until the command is actually executed.
		/// The solution is always to call <see cref="CanExecute"/>. Note exceptions are RETURNED in our model,
		/// not thrown.
		/// </remarks>
		public override void Execute(object parameter)
		{
			var siblingIndex = ((IPAFIntArgumentHolder)parameter).Argument;

			if (m_ResultInfo.IsAtRoot())
			{
				var data = new PTRNED(PAFTestResultNavigationExceptionMessageTags.NODE_IS_ROOT_NODE);
				var newEx = new PAFStandardException<IPTRNED>(data);
				IsExecutable = false;
				((IPAFExceptionArgumentHolder)parameter).Argument = newEx;
				return;
			}

			if (!m_ResultInfo.HasNumberedDisplaySibling(siblingIndex))
			{
				var data = new PTRNED(PAFTestResultNavigationExceptionMessageTags.NODE_IS_OUT_OF_RANGE);
				var newEx = new PAFStandardException<IPTRNED>(data, "SiblingIndex = " + siblingIndex);
				((IPAFExceptionArgumentHolder)parameter).Argument = newEx;
				return;
			}

			m_ResultInfo = m_ResultInfo.GoToNumberedDisplaySibling(siblingIndex);
		}
	}
}