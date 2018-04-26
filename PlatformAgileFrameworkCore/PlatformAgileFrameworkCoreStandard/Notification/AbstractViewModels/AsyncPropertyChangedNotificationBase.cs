using System;
using System.ComponentModel;
using PlatformAgileFramework.Notification.Helpers;
using PlatformAgileFramework.Notification.SubscriberStores.EventSubscriberStores;

namespace PlatformAgileFramework.Notification.AbstractViewModels
{
	/// <summary>
	/// This is an asynchronous version of <see cref="PropertyChangedNotificationBase"/>.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date>23jan2018 </date>
	/// <description>
	/// New. Refactoring view models.
	/// </description>
	/// </contribution>
	/// </history>
	public class AsyncPropertyChangedNotificationBase :
		PropertyChangedNotificationBase, IAsyncPropertyChangedNotificationBase
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Backing.
		/// </summary>
		private bool m_Processing;
		/// <summary>
		/// Backing.
		/// </summary>
		private double m_FractionDone;
		#endregion // Fields and Autoproperties

		#region IAsyncViewModel Implementation
		/// <summary>
		/// See <see cref="IAsyncViewModel"/>
		/// </summary>
		public virtual bool Processing
		{
			get { return m_Processing; }
			set
			{
				PceStore.NotifyOrRaiseIfPropertyChanged(ref m_Processing, value);
			}

		}
		/// <summary>
		/// See <see cref="IAsyncViewModel"/>
		/// </summary>
		public virtual double FractionDone
		{
			get { return m_FractionDone; }
			set
			{
				PceStore.NotifyOrRaiseIfPropertyChanged(ref m_FractionDone, value);
			}
		}
		#endregion IAsyncViewModel Implementation
	}
}
