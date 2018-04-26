using System;
using System.ComponentModel;
using PlatformAgileFramework.Notification.SubscriberStores.EventSubscriberStores;

namespace PlatformAgileFramework.Notification.AbstractViewModels
{
	/// <summary>
	/// Asynchronous version of PropertyChanged base.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date>23jan2018 </date>
	/// <description>
	/// New. Refactoring for new view model structure.
	/// </description>
	/// </contribution>
	/// </history>
	public interface IAsyncPropertyChangedNotificationBase
		: IPropertyChangedNotificationBase, IAsyncViewModel
	{
	}
}