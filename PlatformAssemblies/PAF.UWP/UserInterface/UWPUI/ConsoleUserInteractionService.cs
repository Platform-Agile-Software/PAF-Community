using System;
using System.Security;
using PlatformAgileFramework.UserInterface;
using PlatformAgileFramework.UserInterface.ConsoleUI;
using PlatformAgileFramework.UserInterface.Interfaces;
using PlatformAgileFramework.UserInterface.UserInteractionService;
namespace PAF.UWP.UserInterface.UWPUI
{
	/// <summary>
	/// This class is a basic implementation of the <see cref="IPAFUIService"/>
	/// interface for use through inheritance or containment. This implementation
	/// uses console I/O. It is typically used for clients (like servers) which do
	/// not implement a GUI. It is also often used during application startup in
	/// GUI-based clients before the GUI is loaded.
	/// </summary>
	// Class employs explicit interface implementation with protected virtual delegation
	// methods.
	public class ConsoleUserInteractionService : AbstractUserInteractionService
	{
		#region Class Fields
		/// <summary>
		/// We hold onto the provider so we can switch it's internals.
		/// </summary>
		protected IUIUtils m_IUiUtils;
		#endregion // Class Fields

		/// <summary>
		/// Parameterless constructor needed because Activator won't find the default.
		/// Had to refactor constructors.
		/// </summary>
		[SecurityCritical]
		public ConsoleUserInteractionService()
			: this(null)
		{
		}

		/// <remarks>
		/// This is the way our friends can construct us. Builds with standard
		/// internals.
		/// </remarks>
		protected internal ConsoleUserInteractionService(
			Type serviceImplementationType = null, string serviceName = null, Guid guid = default(Guid))
			: base(serviceImplementationType, serviceName, guid)
		{
			// Build a standard IUI.
			m_IUiUtils = new ConsoleUIUtils();
			// Stick in the Console implementations.
			m_IMessageAndDispatch = new ConsoleMessageAndDispatch(m_IUiUtils);
			m_IMessageListAndStringResponse = new ConsoleMessageListAndStringResponse(m_IUiUtils);
			m_IMessageAndStringResponse = new ConsoleMessageAndStringResponse(m_IUiUtils);
			m_IMessageWithNoResponse = new ConsoleMessageWithNoResponse(m_IUiUtils);
			m_IMessageOkQuit = new ConsoleMessageOkQuit(m_IUiUtils);
			m_IYesNoQuery = new ConsoleYesNoQuery(m_IUiUtils);
		}

		#region IPAFUIService Implementation
		/// <summary>
		/// This method gets the internal <see cref="IStringIO"/> provider.
		/// </summary>
		protected override IStringIO GetStringIOProviderPIV()
		{ return m_IUiUtils.GetStringIOProvider(); }
		/// <summary>
		/// This method sets the internal <see cref="IStringIO"/> provider.
		/// </summary>
		protected override void SetStringIOProviderPIV(IStringIO stringIO)
		{ m_IUiUtils.SetStringIOProvider(stringIO); }
		#endregion // IPAFUIService Implementation
	}
}
