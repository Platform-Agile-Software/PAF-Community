using System.Security;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.UserInterface.Interfaces;

namespace PlatformAgileFramework.UserInterface
{
	/// <summary>
	/// <para>
	/// This interface must be implemented by all services providing a basic UI
	/// function. It contains access methods for the various core interfaces for
	/// interacting with the user.
	/// </para>
	/// <para>
	/// This is one of the "guaranteed" PAF services that will never fail when it
	/// is requested through the framework service manager. To accomplish this,
	/// the service is normally loaded with Console routines, which are static
	/// and always available, no matter what the platform. Under these conditions,
	/// the console implementation is the "default" implementation, since it is
	/// the only implementation. If the Framework client loads a GUI implementation,
	/// it may wish to make that the "default" implementation, but normally the
	/// console implementation is kept as a "named" implementation.
	/// </para>
	/// </summary>
	/// <history>
	/// <author> KRM </author>
	/// <date> 28nov2012 </date>
	/// <contribution>
	/// Added history.
	/// Added secure methods to this public interface, in consonance with our
	/// extensibility pattern. Original author unknown - design of the interface
	/// doesn't look too bad, though - seems to have the right stuff.
	/// TODO - KRM I think we should design an additional abstraction around
	/// TODO - Xamarin's dialog thing.
	/// </contribution>
	/// </history>
	public interface IPAFUIService : IPAFService
	{
		#region Methods
		/// <summary>
		/// This method returns an interface implementing the standard
		/// <see cref="IErrorMessageAndDispatch"/> that all UI's in the framework
		/// must implement.
		/// </summary>
		/// <returns>
		/// An interface on a Type implementing <see cref="IErrorMessageAndDispatch"/>.
		/// </returns>
		IErrorMessageAndDispatch GetErrorMessageAndDispatch();
		/// <summary>
		/// This method returns an interface implementing the standard
		/// <see cref="IMessageWithNoResponse"/> that all UI's in the framework
		/// must implement.
		/// </summary>
		/// <returns>
		/// An interface on a Type implementing <see cref="IMessageWithNoResponse"/>.
		/// </returns>
		IMessageWithNoResponse GetMessageWithNoResponse();
        /// <summary>
        /// This method returns an interface implementing the standard
        /// <see cref="IMessageAndStringResponse"/> that all UI's in the framework
        /// must implement.
        /// </summary>
        /// <returns>
        /// An interface on a Type implementing <see cref="IMessageAndStringResponse"/>.
        /// </returns>
        IMessageAndStringResponse GetMessageAndStringResponse();
        /// <summary>
        /// This method returns an interface implementing the standard
        /// <see cref="IMessageListAndStringResponse"/> that all UI's in the framework
        /// must implement.
        /// </summary>
        /// <returns>
        /// An interface on a Type implementing <see cref="IMessageListAndStringResponse"/>.
        /// </returns>
        IMessageListAndStringResponse GetMessageListAndStringResponse();
		/// <summary>
		/// This method returns an interface implementing the standard
		/// <see cref="IMessageOkQuit"/> that all UI's in the framework
		/// must implement.
		/// </summary>
		/// <returns>
		/// An interface on a Type implementing <see cref="IMessageOkQuit"/>.
		/// </returns>
		IMessageOkQuit GetMessageOkQuit();
		/// <summary>
		/// This method returns an interface implementing the standard
		/// <see cref="IYesNoQuery"/> that all UI's in the framework
		/// must implement.
		/// </summary>
		/// <returns>
		/// An interface on a Type implementing <see cref="IYesNoQuery"/>.
		/// </returns>
		IYesNoQuery GetYesNoQuery();
		#region SecureMethods
		/// <summary>
		/// This method gets the internal <see cref="IStringIO"/> provider.
		/// </summary>
		[SecurityCritical]
		IStringIO GetStringIOProvider();
		/// <summary>
		/// This method sets the internal <see cref="IStringIO"/> provider.
		/// </summary>
		[SecurityCritical]
		void SetStringIOProvider(IStringIO stringIO);
		#endregion // SecureMethods
		#endregion // Methods
	}
}