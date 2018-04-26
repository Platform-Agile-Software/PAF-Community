using System;
using System.ComponentModel;
using PlatformAgileFramework.Notification.SubscriberStores.EventSubscriberStores;

namespace PlatformAgileFramework.Notification.AbstractViewModels
{
	/// <summary>
	/// This is an interface that is typically worn by PAF
	/// veiw models and other classes that use our notification system.
	/// It exposes the usual <see cref="INotifyPropertyChanged"/> mechanism
	/// as well as the subscriber store so developers can access it
	/// along with the variety of extension methods that provide most
	/// of the notification system's functionality. This is the MINIMAL
	/// interface needed for folks to build something with, based on
	/// our notification system, which is the center of our world. 
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date>23jan2018 </date>
	/// <description>
	/// New. Built this generically to support MacOS/iOS binding style as well.
	/// Stripped it down to essentials and leave the extensions to EXTENSION
	/// classes.
	/// </description>
	/// </contribution>
	/// </history>
	/// <remarks>
	/// ALL components subscribing to events should implement <see cref="IDisposable"/>
	/// in PAF, since unregistering in PAF is completely safe and NOT "unpredictable"
	/// like in the .Net delegate system. Components should unregister handlers
	/// in the dispose method. There are other stages in a component's lifetime in
	/// which handlers can be unhooked, but wearing <see cref="IDisposable"/> is
	/// the only way to clearly signal the need. The notification system optionally
	/// uses weak references, but these are not always appropriate.
	/// </remarks>
	public interface IPropertyChangedNotificationBase:
		INotifyPropertyChanged, IDisposable
	{
		/// <summary>
		/// Accesses the store. This accessor is placed in the public
		/// interface so developers understand the importance of the
		/// <see cref="IPropertyChangedEventArgsSubscriberStore"/>
		/// and can call its extension methods.
		/// </summary>
		IPropertyChangedEventArgsSubscriberStore PceStore { get; }
	}
}