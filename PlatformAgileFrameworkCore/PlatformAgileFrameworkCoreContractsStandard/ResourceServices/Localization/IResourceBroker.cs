using System;
using System.Globalization;
using System.Resources;

namespace PlatformAgileFramework.ResourceServices.Localization
{
	/// <summary>
	/// This interface describes the behavior of a "Resource Broker", which
	/// is a replacement for the standard <see cref="ResourceManager"/>.
	/// </summary>
	/// <history>
	/// <author> DAP </author>
	/// <date> 28sep2011 </date>
	/// <contribution>
	/// New Generic interface for 4.0 upgrade.
	/// </contribution>
	/// </history>
	public interface IResourceBroker<out T> : IResourceBroker
	{
		/// <summary>
		/// Fetches a cultured resource.
		/// </summary>
		/// <param name="tag">
		/// String describing the resource.
		/// </param>
		/// <param name="culture">
		/// <see langword="null"/> for the default culture.
		/// </param>
		/// <param name="throwException">
		/// <see langword="false"/> to suppress exceptions.
		/// </param>
		/// <returns>
		/// <see langword="null"/> if unable to build resource and exceptions disabled.
		/// </returns>
		T GetResource(string tag, CultureInfo culture, bool throwException);
	}
	/// <summary>
	/// This interface describes the behavior of a "Resource Broker", which
	/// is a replacement for the standard <see cref="ResourceManager"/>.
	/// </summary>
	public interface IResourceBroker
	{
		/// <summary>
		/// Fetches a cultured resource.
		/// </summary>
		/// <param name="tag">
		/// String describing the resource.
		/// </param>
		/// <param name="culture">
		/// <see langword="null"/> for the default culture.
		/// </param>
		/// <param name="type">
		/// Type of the resource.
		/// </param>
		/// <param name="throwException">
		/// <see langword="false"/> to suppress exceptions.
		/// </param>
		/// <returns>
		/// <see langword="null"/> if unable to build resource and exceptions disabled.
		/// </returns>
		object GetResource(string tag, CultureInfo culture, Type type, bool throwException);
	}
}
