//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 -2015 Icucom Corporation
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
using System.Security;
using PlatformAgileFramework.UserInterface.Interfaces;
using PlatformAgileFramework.UserInterface.UserInteractionService;

namespace PlatformAgileFramework.UserInterface.ConsoleUI
{
	/// <summary>
	/// This class is a basic implementation of the <see cref="IPAFUIService"/>
	/// interface for use through inheritance or containment. This implementation
	/// uses console I/O. It is typically used for clients (like servers) which do
	/// not implement a GUI. It is also often used during application startup in
	/// GUI-based clients before the GUI is loaded.
	/// </summary>
	public class ConsoleUserInteractionService : AbstractUserInteractionService
	{
		#region Class Fields
		/// <summary>
		/// We hold onto the provider so we can switch it's internals.
		/// </summary>
		protected IUIUtils m_iUIUtils;
		#endregion // Class Fields

		/// <summary>
		/// Parameterless constructor needed because Activator won't find the default.
		/// Had to refactor constructors.
		/// </summary>
		[SecurityCritical]
		public ConsoleUserInteractionService()
			: this(default(Guid))
		{
		}

		/// <remarks>
		/// Builds with standard internals.
		/// </remarks>
		protected ConsoleUserInteractionService(Guid guid,
			Type serviceImplementationType = null, String serviceName = null)
			: base(serviceImplementationType, serviceName, guid)
		{
			// Build a standard IUI.
			m_iUIUtils = new ConsoleUIUtils();
			// Stick in the Console implementations.
			m_IMessageAndStringResponse = new ConsoleMessageAndStringResponse(m_iUIUtils);
			m_IMessageWithNoResponse = new ConsoleMessageWithNoResponse(m_iUIUtils);
			m_IMessageOkQuit = new ConsoleMessageOkQuit(m_iUIUtils);
			m_IYesNoQuery = new ConsoleYesNoQuery(m_iUIUtils);
		}

		#region IPAFUIService Implementation
		/// <summary>
		/// This method gets the internal <see cref="IStringIO"/> provider.
		/// </summary>
		protected override IStringIO GetStringIOProviderPIV()
		{ return m_iUIUtils.GetStringIOProvider(); }
		/// <summary>
		/// This method sets the internal <see cref="IStringIO"/> provider.
		/// </summary>
		protected override void SetStringIOProviderPIV(IStringIO stringIO)
		{ m_iUIUtils.SetStringIOProvider(stringIO); }
		#endregion // IPAFUIService Implementation
	}
}
