using PlatformAgileFramework.UserInterface.Interfaces;

namespace PlatformAgileFramework.UserInterface
{
	/// <summary>
	/// <para>
	/// This interface exposes a method to set the internal string provider
	/// for testing using mock UI interaction, among other things.
	/// </para>
	/// </summary>
	// ReSharper disable InconsistentNaming
	internal interface IPAFUIServiceInternal : IPAFUIService
		// ReSharper restore InconsistentNaming
	{
		/// <summary>
		/// This method gets the internal <see cref="IStringIO"/> provider.
		/// </summary>
		// ReSharper disable once InconsistentNaming
		IStringIO GetStringIOProviderInternal();
		/// <summary>
		/// This method sets the internal <see cref="IStringIO"/> provider.
		/// </summary>
		// ReSharper disable InconsistentNaming
		void SetStringIOProviderInternal(IStringIO stringIO);
		// ReSharper restore InconsistentNaming
	}
}
