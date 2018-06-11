//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2005 - 2018 Icucom Corporation
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
using PlatformAgileFramework.Connections;
using PlatformAgileFramework.Connections.BaseConnectors;
using PlatformAgileFramework.Notification.SubscriberStores.EventSubscriberStores;
using PlatformAgileFramework.Properties;
using PlatformAgileFramework.TypeHandling.PartialClassSupport;

namespace PlatformAgileFramework.Notification.Connectors
{
	/// <summary>
	/// This is a base class for a connector for two nodes wearing
	/// <see cref="INotifyPropertyChanged"/>.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 19mar2018 </date>
	/// <description>
	/// Reformulated for the unification of MVC styles. Took out all converters.
	/// </description>
	/// </contribution>
	/// </history>
	/// <remarks>
	/// TODO: Until we get one-way connectors rebuilt, subclass a class without
	/// <see cref="INotifyPropertyChanged"/> add it, but just don't use it. 19mar2018
	/// </remarks>
	public class TwoWayPropertyNotificationConnector<TNode1, TNode2> : PAFConnector<TNode1, TNode2>
		where TNode1 : class, INotifyPropertyChanged where TNode2 : class, INotifyPropertyChanged
	{
		#region Class Fields and Autoprops
		/// <summary>
		/// The intermediary store for transmission of events TO node2.
		/// </summary>
		protected readonly IPropertyChangedEventArgsSubscriberStore m_ForwardPceStore;
		/// <summary>
		/// The intermediary store for transmission of events TO node1.
		/// </summary>
		protected readonly IPropertyChangedEventArgsSubscriberStore m_ReversePceStore;
		#endregion // Class Fields and Autoprops
		#region Constructors
		/// <summary>
		/// Sets all props that are known at construction time and builds stores.
		/// </summary>
		/// <param name="node1">
		/// Loads <see cref="IPAFConnector.UniDirectionalSource"/>. Not <see langword="null"/>.
		/// </param>
		/// <param name="node2">
		/// Loads <see cref="IPAFConnector.UniDirectionalSink"/>.  Not <see langword="null"/>.
		/// </param>
		/// <param name="dataTransferDirection">
		/// Loads <see cref="IPAFConnector.DataTransferDirection"/>.
		/// <see langword="null"/> is two-way.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">"source"</exception>
		/// <exception cref="ArgumentNullException">"sink"</exception>
		/// </exceptions>
		public TwoWayPropertyNotificationConnector(
			TNode1 node1, TNode2 node2, TransferDirection dataTransferDirection = null)
			: base( node1, node2, dataTransferDirection)
		{
			m_ForwardPceStore = new PropertyChangedEventArgsSubscriberStore(this);
			m_ForwardPceStore.Start();
			m_ReversePceStore = new PropertyChangedEventArgsSubscriberStore(this);
			m_ReversePceStore.Start();
		}
		#endregion // Constructors

		#region PropertyChanged Implementations
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
		/// <remarks>
		/// This is the "forward" (node1 to node2) event. 
		/// </remarks>
		public virtual event PropertyChangedEventHandler Node1PropertyChanged
		{
			add { m_ForwardPceStore.WeaklySubscribe(value); }
			remove { m_ForwardPceStore.Unsubscribe(value); }
		}
		/// <summary>
		/// This is the receiver method for the property change occurrance broadcast
		/// for a change occurring in node 1.
		/// </summary>
		/// <param name="obj">Standard args for the event.</param>
		/// <param name="args">Standard args for the event.</param>
		/// <remarks>
		/// This method ignores any calls on it if the "ONE_TO_TWO" bit is
		/// off. This is the base version in which we ignore events rather
		/// than queing or hydrating them.
		/// </remarks>
		public virtual void OneToTwoEventBroadcastReceiverMethod(object obj, [NotNull] PropertyChangedEventArgs args)
		{
			if (DataTransferDirection.BitwiseANDValue(TransferDirection.ONE_TO_TWO) == 0)
				return;
			m_ForwardPceStore.Payload = args;
			m_ForwardPceStore.NotifySubscribers();
		}
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
		/// <remarks>
		/// This is the "forward" (node1 to node2) event. 
		/// </remarks>
		public virtual event PropertyChangedEventHandler Node2PropertyChanged
		{
			add { m_ReversePceStore.WeaklySubscribe(value); }
			remove { m_ReversePceStore.Unsubscribe(value); }
		}
		/// <summary>
		/// This is the receiver method for the property change occurrance broadcast
		/// for a change occurring in node 2.
		/// </summary>
		/// <param name="obj">Standard args for the event.</param>
		/// <param name="args">Standard args for the event.</param>
		/// <remarks>
		/// This method ignores any calls on it if the "TWO_TO_ONE" bit is
		/// off. This is the base version in which we ignore events rather
		/// than queing or hydrating them.
		/// </remarks>
		public virtual void TwoToOneEventBroadcastReceiverMethod(object obj, [NotNull] PropertyChangedEventArgs args)
		{
			if (DataTransferDirection.BitwiseANDValue(TransferDirection.TWO_TO_ONE) == 0)
				return;
			m_ReversePceStore.Payload = args;
			m_ReversePceStore.NotifySubscribers();
		}
		#endregion // PropertyChanged Implementations

		#region IDisposable implementation

		protected override void Dispose(bool disposing)
		{

		}
		#endregion // IDisposable implementation
	}
}


