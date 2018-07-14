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
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-
using System;
using System.ComponentModel;
using PlatformAgileFramework.Notification.SubscriberStores.EventSubscriberStores;

namespace PlatformAgileFramework.Notification.AbstractViewControllers
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
		    m_PceStore.Start();
		}
		#endregion // Constructors
		#region Properties
		/// <summary>
		/// See <see cref="IPropertyChangedNotificationBase"/>. We almost always want virtual
		/// props in framework classes for maximum flexibility.
		/// </summary>
		public virtual IPropertyChangedEventArgsSubscriberStore PceStore
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
		/// <summary>
		/// This method is always virtual and protected.
		/// </summary>
		/// <param name="disposing">
		/// <see langword="true"/> when called through <see cref="IDisposable.Dispose()"/>
		/// <see langword="false"/> when called from a finalizer.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				m_PceStore?.Dispose();
			}
		}
		/// <summary>
		/// This method is NEVER virtual.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			// This statement is ALWAYS needed in case subclasses implement a finalizer.
			GC.SuppressFinalize(this);
		}

		#endregion // IDispose Implementation
	}
}
