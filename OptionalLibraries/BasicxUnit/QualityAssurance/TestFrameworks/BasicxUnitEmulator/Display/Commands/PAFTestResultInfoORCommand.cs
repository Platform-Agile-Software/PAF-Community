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
using PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Display.Exceptions;
using PlatformAgileFramework.TypeHandling.AggregableObjectArguments;

// Exception shorthand.
using PTRNED = PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Display.Exceptions.PAFTestResultNavigationExceptionData;
using IPTRNED = PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Display.Exceptions.IPAFTestResultNavigationExceptionData;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Display.Commands
{
	/// <summary>
	/// An implementation of <see cref="IPAFTestResultInfoCommand"/> for
	/// printing Outputting Results at the current node. "OR"...
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
	public class PAFTestResultInfoORCommand : PAFTestResultInfoCommand
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Our name.
		/// </summary>
		public const string COMMAND_NAME = "OR";
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// This constructor builds with a <see cref="IPAFTestElementResultInfo"/>.
		/// Command is always executable.
		/// </summary>
		/// <param name="resultInfo">The node in the tree of results.</param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="resultInfo"/>
		/// </exception>
		/// </exceptions>
		public PAFTestResultInfoORCommand(IPAFTestElementResultInfo resultInfo)
			:base(resultInfo)
		{
			if(resultInfo == null)
				throw new ArgumentNullException("resultInfo");
			m_ResultInfo = resultInfo;
			m_CommandName = COMMAND_NAME;
			m_IsExecutable = true;
		}
		#endregion // Constructors

		/// <summary>
		/// We can always display with a node's default detail renderer. 
		/// </summary>
		/// <param name="parameter">
		/// Not used.
		/// </param>
		/// <returns>
		/// <see langword="true"/> always.
		/// </returns>
		public override bool CanExecute(object parameter)
		{
			IsExecutable = true;
			return true;
		}

		/// <summary>
		/// Displays information about a node.
		/// </summary>
		/// <param name="parameter">
        /// This must be an <see cref="IPAFIntArgumentHolder"/> and a
        /// a <see cref="IPAFExceptionArgumentHolder"/>
        /// and hold a valid sibling number and hold a valid detail level 0-2.
		/// </param>
        /// <exceptions>
        /// <exception cref="PAFStandardException{IPTRNED}">
        /// <see cref="PAFTestResultNavigationExceptionMessageTags.DETAIL_LEVEL_IS_0_1_2"/>
        /// </exception>
        /// </exceptions>
		public override void Execute(object parameter)
		{
            var detailLevel = ((IPAFIntArgumentHolder)parameter).Argument;

            if ((detailLevel < 0) || (detailLevel > 2))
            {
                var data = new PTRNED(PAFTestResultNavigationExceptionMessageTags.DETAIL_LEVEL_IS_0_1_2);
                var newEx = new PAFStandardException<IPTRNED>(data);
                ((IPAFExceptionArgumentHolder)parameter).Argument = newEx;
                return;
            }

			m_ResultInfo.CustomPrinter(m_ResultInfo, true, detailLevel, true);
		}
	}
}