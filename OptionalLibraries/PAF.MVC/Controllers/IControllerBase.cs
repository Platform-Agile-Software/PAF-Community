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

using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using PlatformAgileFramework.Connections.BaseConnectors;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.Notification.AbstractViewControllers;
using PlatformAgileFramework.Notification.SubscriberStores.EventSubscriberStores;
namespace PlatformAgileFramework.MVC.Controllers
{
	/// <summary>
	/// This interface prescribes base functionality for a Graphics
	/// view controller.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 11aug18 </date>
	/// New. Factored old interface to make more modular.
	/// </contribution>
	/// </history>
	public interface IControllerBase: IPropertyChangedNotificationBase,
		ISharedUIApplication, IGenericViewLifecycle
	{
		IPAFConnector Connector { get; set; }
	}
	public class ControllerBase : IControllerBase
	{
		private IPropertyChangedEventArgsSubscriberStore m_PceStore;
		private IPAFServiceManager<IPAFService> m_ServiceManager;
		private SynchronizationContext m_UISynchronizationContext;
		private IPAFConnector m_Connector;
		public event PropertyChangedEventHandler PropertyChanged;
		#region Implementation of IDisposable
		/// <summary>
		/// <see cref="IDisposable"/>
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
		}
		#endregion // Implementation of IDisposable
		/// <summary>
		/// Disposes the stream.
		/// </summary>
		/// <param name="disposing">
		/// If <see langword="true"/>, disposes managed objects.
		/// </param>
		protected virtual void Dispose(bool disposing)
		{
			
		}
		public IPropertyChangedEventArgsSubscriberStore PropertyChangedStore
		{
			get => m_PceStore;
			set => m_PceStore = value;
		}
		public virtual IPAFServiceManager<IPAFService> ServiceManager
		{
			get => m_ServiceManager;
			set => m_ServiceManager = value;
		}
		public virtual SynchronizationContext UISynchronizationContext
		{
			get => m_UISynchronizationContext;
			set => m_UISynchronizationContext = value;
		}
		public virtual Task OnAppearingAsync()
		{
			return Task.FromResult(0);
		}
		public virtual Task OnCreatingAsync()
		{
			return Task.FromResult(0);
		}
		public virtual Task OnDisappearingAsync()
		{
			return Task.FromResult(0);
		}
		public virtual Task OnDestroyingAsync()
		{
			return Task.FromResult(0);
		}
		public virtual IPAFConnector Connector
		{
			get => m_Connector;
			set => m_Connector = value;
		}
	}
}