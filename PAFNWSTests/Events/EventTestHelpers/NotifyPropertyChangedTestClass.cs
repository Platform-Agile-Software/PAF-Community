//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2017 Icucom Corporation
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
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System.ComponentModel;
using PlatformAgileFramework.Notification.Helpers;
using PlatformAgileFramework.Notification.SubscriberStores.EventSubscriberStores;
using PlatformAgileFramework.Properties;

namespace PlatformAgileFramework.Events.EventTestHelpers
{
	/// <summary>
	/// This is a simple class with two properties to test the correct
	/// operation of the <see cref="IPropertyChangedEventArgsSubscriberStore"/>
	/// and the associated machinery for using it. It is designed to support
	/// tests for transmitting and receiving property changed events (messages)
	/// and the determination of whether properties were properly reset (or not).
	/// </summary>
	public class NotifyPropertyChangedTestClass : INotifyPropertyChangedTestClass
	{
		#region Fields and Autoproperties
		/// <summary>
		/// This is a test node as the same type as us.
		/// </summary>
		public INotifyPropertyChangedTestClass m_MyTwoWayCommunicatingTestPartner;
		/// <summary>
		/// The store under test....
		/// </summary>
		public readonly IPropertyChangedEventArgsSubscriberStore m_PceStore;
		/// <summary>
		/// Backing...
		/// </summary>
		public string m_Aname;
		/// <summary>
		/// Backing...
		/// </summary>
		public int m_AnAge;
		/// <summary>
		/// For the test fixture.
		/// </summary>
		public int m_NumSetAttempts;
		/// <summary>
		/// For the test fixture. This one is set when the new prop
		/// value is different.
		/// </summary>
		public int m_NumSetSuccesses;
		/// <summary>
		/// For demonstration of dynamic one-way switching.
		/// </summary>
		public bool m_ReceiveOn = true;
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// This default constructor just staples in and starts the
		/// subscriber store.
		/// </summary>
		public NotifyPropertyChangedTestClass()
		{
			m_PceStore = new PropertyChangedEventArgsSubscriberStore(this);
			m_PceStore.Start();
		}
		#endregion // Constructors
		#region Properties
		/// <summary>
		/// Typical string property to test.
		/// </summary>
		public virtual string Aname
		{
			get { return m_Aname; }
			set
			{
				m_NumSetAttempts++;
				if (m_PceStore.NotifyOrRaiseIfPropertyChanged(ref m_Aname, value))
					m_NumSetSuccesses++;
			}
		}
		/// <summary>
		/// Typical integer property to test.
		/// </summary>
		public virtual int AnAge
		{
			get { return m_AnAge; }
			set
			{
				m_NumSetAttempts++;
				if (m_PceStore.NotifyOrRaiseIfPropertyChanged(ref m_AnAge, value))
					m_NumSetSuccesses++;
			}
		}
		#endregion // Properties
		#region Methods
		#region INotifyPropertyChanged Implementation
		/// <summary>
		/// This is the implementation of the event in <see cref="INotifyPropertyChanged"/>.
		/// The explicit implementation of the add and remove methods effectively
		/// turns the event into a facade. For example, it is no longer possible
		/// to call<c>PropertyChanged.Invoke(...)</c>, since the compiler knows there
		/// is nothing inside to invoke. It can, however, allow the event to masquerade
		/// to the outside world as a "normal" .Net event. We make this virtual for
		/// developing subclasses of this test class that do different things in the
		/// add and remove.
		/// </summary>
		public virtual event PropertyChangedEventHandler PropertyChanged
		{
			add { m_PceStore.WeaklySubscribe(value); }
			remove { m_PceStore.Unsubscribe(value); }
		}
		#endregion // INotifyPropertyChanged Implementation

		/// <summary>
		/// This is the receiver method for the property change occurrance broadcast.
		/// </summary>
		/// <param name="obj">Standard args for the event.</param>
		/// <param name="args">Standard args for the event.</param>
		/// <remarks>
		/// This method ignores any calls on it if <see cref="m_ReceiveOn"/> is
		/// <see langword="false"/>.
		/// </remarks>
		public virtual void EventBroadcastReceiverMethod(object obj, [NotNull] PropertyChangedEventArgs args)
		{
			if (!m_ReceiveOn)
				return;
			if (args.PropertyName == "Aname")
				Aname = m_MyTwoWayCommunicatingTestPartner.Aname;
			if (args.PropertyName == "AnAge")
				AnAge = m_MyTwoWayCommunicatingTestPartner.AnAge;
		}

		/// <summary>
		/// This method allows us to subscribe to property changed occurrances
		/// from another "node" in PAF-speak and to query it for actual updated
		/// property values. This is called by some external coordinating mechanism
		/// and the same method would be called on our peer to hook it up to us.
		/// In UI applications, this is ordinarily performed by an MVC-style framework.
		/// </summary>
		/// <param name="myPeer">
		/// Another node that I want to receive notifications from.
		/// </param>
		public virtual void OneWayBindToMyPeer(INotifyPropertyChangedTestClass myPeer)
		{
			m_MyTwoWayCommunicatingTestPartner = myPeer;
			m_MyTwoWayCommunicatingTestPartner.PropertyChanged += EventBroadcastReceiverMethod;
		}
		#region IDisposable Implementation
		/// <summary>
		/// Not the correct canonical dispose pattern, but we don't need it for
		/// this test class.
		/// </summary>
		public virtual void Dispose()
		{
			if (m_MyTwoWayCommunicatingTestPartner != null)
				m_MyTwoWayCommunicatingTestPartner.PropertyChanged -= EventBroadcastReceiverMethod;
			m_PceStore?.Dispose();
		}
		#endregion //IDisposable Implementation
		#endregion //Methods

	}
}
