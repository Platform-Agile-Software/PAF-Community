using System;
using System.ComponentModel;
using PlatformAgileFramework.Notification.SubscriberStores.EventSubscriberStores;

namespace PlatformAgileFramework.Notification.AbstractViewModels
{
	/// <summary>
	/// This is an example of an extremely minimal controller base
	/// class that can be built using the notification facilities of
	/// PAF. The general problem is that many third-party providers
	/// of MVC frameworks create base classes that have needed
	/// functionality exposed IN THAT CLASS, so the developer is
	/// pretty much constrained to use the provider's base class.
	/// Our clients have applications that require derivation from
	/// specific classes and our philosophy is interface-based to
	/// allow more easy composition of custom controller bases.
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
	/// Generally in a single-inheritance system, base classes should always
	/// have their functionality exposed through interfaces and the interfaces
	/// should be layered so implementations can be changed without too much
	/// disruption.
	/// </remarks>
    public class PropertyChangedNotificationBase : IPropertyChangedNotificationBase
	{
		#region Fields and Autoproperties
	    /// <summary>
	    /// The store for our <see cref="PropertyChanged"/> subscribers.
	    /// </summary>
	    private readonly IPropertyChangedEventArgsSubscriberStore m_PceStore;
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Default constructor just builds our internal subscriber store
		/// and starts it.
		/// </summary>
		public PropertyChangedNotificationBase()
	    {
		    m_PceStore = new PropertyChangedEventArgsSubscriberStore(this);
		    PceStore.Start();
		}
		#endregion // Constructors
		#region Properties
		/// <summary>
		/// The store for our <see cref="PropertyChanged"/> subscribers.
		/// </summary>
		public IPropertyChangedEventArgsSubscriberStore PceStore
	    {
		    get { return m_PceStore; }
	    }
		#endregion // Properties
		#region INotifyPropertyChanged Implementation
		/// <summary>
		/// This is the implementation of the event in <see cref="INotifyPropertyChanged"/>.
		/// The explicit implementation of the add and remove methods effectively
		/// turns the event into a facade. For example, it is no longer possible
		/// to call<c>PropertyChanged.Invoke(...)</c>, since the compiler knows there
		/// is nothing inside to invoke. It can, however, allow the event to masquerade
		/// to the outside world as a "normal" .Net event. We make this virtual for
		/// developing subclasses of this class that do different things in the
		/// add and remove. The "add" method here creates a weak subscription, although
		/// a strong subscription can be created by simply accessing
		/// <see cref="PceStore"/>.
		/// </summary>
		public virtual event PropertyChangedEventHandler PropertyChanged
		{
			add { m_PceStore.WeaklySubscribe(value); }
			remove { m_PceStore.Unsubscribe(value); }
		}
		#endregion // INotifyPropertyChanged Implementation

		#region IDispose Implementation
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				m_PceStore?.Dispose();
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion // IDispose Implementation
	}
}
