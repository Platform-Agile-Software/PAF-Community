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
	/// navigating down in the tree to a certain node with an index.
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
	public class PAFTestResultInfoDNICommand : PAFTestResultInfoCommand
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Our name.
		/// </summary>
		public const string COMMAND_NAME = "DNI";
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
		/// on a leaf.
		/// </remarks>
		public PAFTestResultInfoDNICommand(IPAFTestElementResultInfo resultInfo)
			:base(resultInfo)
		{
			if(resultInfo == null)
				throw new ArgumentNullException("resultInfo");
			m_ResultInfo = resultInfo;
			m_CommandName = COMMAND_NAME;
			m_IsExecutable = resultInfo.IsOnALeaf() != true;
		}
		#endregion // Constructors

		/// <summary>
		/// For us, we can execute if we are not at a leaf node.
		/// If there is a change in executability, the event is fired.
		/// This can be called after the command and index is parsed
		/// for a validity check on the index.
		/// </summary>
		/// <param name="parameter">
		/// Unused
		/// </param>
		/// <returns>
		/// <see langword="true"/> if not on a leaf and child number is in range.
		/// </returns>
		/// <remarks>
		/// Passing an index of 0 always works if we are not on a leaf node..
		/// </remarks>
		public override bool CanExecute(object parameter)
		{
			var childIndex = ((IPAFIntArgumentHolder)parameter).Argument;
			var canExecute
				= ((m_ResultInfo.IsOnALeaf() != true) && (m_ResultInfo.HasNumberedDisplayChild(childIndex)));
			// If we are on a leaf, we can not execute no matter what the index is.
			if (m_ResultInfo.IsOnALeaf())
				IsExecutable = false;
			// If we are not, we could execute with the right parameters.
			else
				IsExecutable = true;

			return canExecute;
		}

		/// <summary>
		/// Goes down to a numbered child node.
		/// </summary>
		/// <param name="parameter">
		/// This must be an <see cref="IPAFIntArgumentHolder"/> and a
		/// a <see cref="IPAFExceptionArgumentHolder"/>
		/// and hold a valid child number.
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
		/// <see cref="PAFTestResultNavigationExceptionMessageTags.NODE_IS_LEAF_NODE"/>
		/// </exception>
		/// <exception cref="PAFStandardException{IPTRNED}">
		/// <see cref="PAFTestResultNavigationExceptionMessageTags.NODE_IS_OUT_OF_RANGE"/>
		/// </exception>
		/// </exceptions>
		/// <remarks>
		/// We can't reasonably check if the same command can be executed again, since
		/// the check would make no sense, generally. We do disable the command, however,
		/// if we are at a leaf node when finished. The index can't be checked until
		/// the command is actually executed. The solution is always to call
		/// <see cref="CanExecute"/>.  Note exceptions are RETURNED in our model,
		/// not thrown.
		/// </remarks>
		public override void Execute(object parameter)
		{
			var childIndex = ((IPAFIntArgumentHolder)parameter).Argument;

			if (m_ResultInfo.IsOnALeaf())
			{
				var data = new PTRNED(PAFTestResultNavigationExceptionMessageTags.NODE_IS_LEAF_NODE);
				var newEx = new PAFStandardException<IPTRNED>(data);
				IsExecutable = false;
				((IPAFExceptionArgumentHolder)parameter).Argument = newEx;
				return;
			}

			if (!m_ResultInfo.HasNumberedDisplayChild(childIndex))
			{
				var data = new PTRNED(PAFTestResultNavigationExceptionMessageTags.NODE_IS_OUT_OF_RANGE);
				var newEx = new PAFStandardException<IPTRNED>(data, "ChildIndex = " + childIndex);
				((IPAFExceptionArgumentHolder)parameter).Argument = newEx;
				return;
			}

			m_ResultInfo = m_ResultInfo.GoDownToNumberedDisplayChild(childIndex);
			if (m_ResultInfo.IsOnALeaf())
			{
				// Can't go down anymore.
				IsExecutable = false;
			}
		}
	}
}