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
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System.Collections.Generic;
using System.Threading;
using PlatformAgileFramework.Collections;
namespace PlatformAgileFramework.MVC.Controllers.Navigation
{
	/// <summary>
	/// This interface provides the basic protocol for navigation among views
	/// through their controllers. This is the service center, which has the
	/// controller lists.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 17may19 </date>
	/// Updated to support the updated interface.
	/// </contribution>
	/// <contribution>
	/// <author> DAV(P) </author>
	/// <date> 01dec17 </date>
	/// New. 
	/// </contribution>
	/// </history>
	public class NavigationServiceCenter: NavigationService, INavigationServiceCenter
	{
		/// <summary>
		/// Backing.
		/// </summary>
		private readonly IList<IDismissableNavigableController> m_ModalNavigationList;
		/// <summary>
		/// Backing.
		/// </summary>
		private readonly IList<INavigableController> m_NonModalNavigationList;
		/// <summary>
		/// Lock for the list.
		/// </summary>
		private readonly object m_NmLock = new object();
		/// <summary>
		/// Lock for the list.
		/// </summary>
		private readonly object m_MLock = new object();
		public NavigationServiceCenter()
		{
			m_ModalNavigationList = new List<IDismissableNavigableController>();
			m_NonModalNavigationList = new List<INavigableController>();
			base.NavigationCenter = this;
		}
		public IDisposableListProvider<IDismissableNavigableController> DisposableModalNavigationList
		{
			get => new DisposableModalList(this);
		}
		public IDisposableListProvider<INavigableController> DisposableNonModalNavigationList
		{
			get => new DisposableNonModalList(this);
		}
		/// <summary>
		/// Disposable thread-safe access to modal list. This is so list
		/// can be accessed in a using block.
		/// </summary>
		protected class DisposableModalList : IDisposableListProvider<IDismissableNavigableController>
		{
			private readonly NavigationServiceCenter m_ServiceCenter;
			public DisposableModalList(NavigationServiceCenter navigationServiceCenter)
			{
				m_ServiceCenter = navigationServiceCenter;
				Monitor.Enter(m_ServiceCenter.m_MLock);
			}
			public IList<IDismissableNavigableController> LockedList
			{
				get { return m_ServiceCenter.m_ModalNavigationList; }
			}

			public void Dispose()
			{
				Monitor.Exit(m_ServiceCenter.m_MLock);
			}
		}
		/// <summary>
		/// Disposable thread-safe access to non-modal list. This is so list
		/// can be accessed in a using block.
		/// </summary>
		protected class DisposableNonModalList : IDisposableListProvider<INavigableController>
		{
			private readonly NavigationServiceCenter m_ServiceCenter;
			public DisposableNonModalList(NavigationServiceCenter navigationServiceCenter)
			{
				m_ServiceCenter = navigationServiceCenter;
				Monitor.Enter(m_ServiceCenter.m_NmLock);
			}
			public IList<INavigableController> LockedList
			{
				get { return m_ServiceCenter.m_NonModalNavigationList; }
			}

			public void Dispose()
			{
				Monitor.Exit(m_ServiceCenter.m_NmLock);
			}
		}
	}
}

