using System.Collections.Generic;
using System.IO;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.FileAndIO;
using PlatformAgileFramework.FileAndIO.FileAndDirectoryService;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Platform;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace PlatformAgileFramework.Navigation
{
	/// <summary>
	/// Extensions for Xamarin types.
	/// </summary>
	public static class NavigationHelpers
	{
		/// <summary>
		/// This one climbs a CONTAINMENT tree to find an <see cref="Element"/> that
		/// is castable to a type <typeparam name="T"/>.
		/// </summary>
		/// <typeparam name="T">Must be a reference type.</typeparam>
		/// <param name="potentialChildElement">
		/// Any object, first checked to see if it is an <see cref="Element"/>
		/// , before climbing its tree.
		/// </param>
		/// <returns>
		/// An element cast to <typeparamref name="T"/>, if one is found.
		/// <see langword="null"/>, otherwise.
		/// </returns>
		/// <remarks>
		/// This one is useful to act on the object in classical event handler signatures.
		/// If you have the latitude, don't use such a stupid signature in your custom events!
		/// </remarks>
		public static T FindCastableElementObject<T>(this object potentialChildElement)
			where T : class
		{
			if (!(potentialChildElement is Element childElement)) return null;
			var currentElement = childElement;
			do
			{
				if (currentElement is T casted)
					return casted;

				currentElement = currentElement.Parent;
			} while (currentElement != null);

			return null;
		}
		/// <summary>
		/// This one climbs a CONTAINMENT tree to find an <see cref="Element"/> that
		/// is castable to a type <typeparam name="T"/>.
		/// </summary>
		/// <typeparam name="T">Must be a reference type.</typeparam>
		/// <param name="childElement">
		/// Any element, but typically "US", when we want to find an implementing ancestor.
		/// </param>
		/// <returns>
		/// An element cast to <typeparamref name="T"/>, if one is found.
		/// <see langword="null"/>, otherwise.
		/// </returns>
		public static T FindCastableElement<T>(this Element childElement)
			where T : class
		{
			var currentElement = childElement;
			do
			{
				if (currentElement is T casted)
					return casted;

				currentElement = currentElement.Parent;
			}
			while (currentElement != null);
			return null;
		}
		/// <summary>
		/// This one climbs a CONTAINMENT tree to find an <see cref="IExceptionReceiverProvider"/>.
		/// </summary>
		/// <param name="childElement">
		/// Any element, but typically "US", when we want to find an implementing ancestor.
		/// </param>
		/// <returns>
		/// An element that implements <see cref="IExceptionReceiverProvider"/>, if one is found.
		/// <see langword="null"/>, otherwise.
		/// </returns>
		public static IExceptionReceiverProvider FindExceptionReceiverProvider(this Element childElement)
		{
			return FindCastableElement<IExceptionReceiverProvider>(childElement);
		}
	}
}
