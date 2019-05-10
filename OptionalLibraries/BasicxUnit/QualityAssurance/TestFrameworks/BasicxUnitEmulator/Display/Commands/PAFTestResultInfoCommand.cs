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
using PlatformAgileFramework.Events.Execution.Commands;
using PlatformAgileFramework.Execution.Commands;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Display.Commands
{
	/// <summary>
	/// An implementation of <see cref="IPAFNamedCommand"/> for test results.
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
	public abstract class PAFTestResultInfoCommand : PAFNamedCommand, IPAFTestResultInfoCommand
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Backing ...
		/// </summary>
		protected internal IPAFTestElementResultInfo m_ResultInfo;
		#endregion // Fields and Autoproperties

		#region Constructors
		/// <summary>
		/// This constructor builds with a <see cref="IPAFTestElementResultInfo"/>
		/// node that is needed to determine which commands are executable.
		/// </summary>
		/// <param name="resultInfo">The node in the tree of results.</param>
		protected PAFTestResultInfoCommand(IPAFTestElementResultInfo resultInfo)
		{
			m_ResultInfo = resultInfo;
		}
		#endregion // Constructors

		#region Properties
		/// <summary>
		/// <see cref="IPAFTestResultInfoCommand"/>. This default implementation
		/// just checks the node for <see langword="null"/> in the setter.
		/// </summary>
		/// <remarks>
		/// Derived classes typically fire the event.
		/// </remarks>
		public virtual IPAFTestElementResultInfo ResultInfo
		{
			get { return m_ResultInfo; }
			set
			{
				if(value == null)
					throw new ArgumentNullException("ResultInfo");
				m_ResultInfo = value;
			}
		}
		#endregion // Properties
	}
}