
namespace PlatformAgileFramework.UserInterface.Interfaces
{
	/// <summary>
	/// This is an interface that is used mostly on mock objects
	/// for testing. It must be implemented by Types that wish to provide
	/// a string input/output capability to the user if it is desired to
	/// provide mock stimuli to simulate user interaction, whether it is
	/// console-based or GUI-based.
	/// </summary>
	public interface IStringIOProvider
	{
		/// <summary>
		/// Just returns the IO interface.
		/// </summary>
		/// <returns>
		/// The <see cref="IStringIO"/> returned.
		/// </returns>
		IStringIO GetStringIO();
	}
}
