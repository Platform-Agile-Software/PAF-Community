//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2019 Icucom Corporation
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

using System;
using System.Threading.Tasks;
using PlatformAgileFramework.MVC.Views;
namespace PlatformAgileFramework.MVC.Controllers.Navigation
{
	// Note: KRM this class in progress.
	/// <summary>
	/// This interface provides the basic protocol for navigation among views through their
	/// controllers.
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
	public class NavigationService : INavigationService
	{
		/// <summary>
		/// Backing.
		/// </summary>
		private INavigableController m_CurrentNonModal;
		/// <summary>
		/// Backing.
		/// </summary>
		private IDismissableNavigableController m_CurrentModal;
		/// <summary>
		/// Backing.
		/// </summary>
		private INavigationServiceCenter m_NavigationCenter;
		/// <summary>
		/// Constructor builds with a navigation center, which is the navigation
		/// root. Also supports the construct and set style if arg is <see langword="null"/>.
		/// </summary>
		/// <param name="navigationCenter">root nav.</param>
		public NavigationService(INavigationServiceCenter navigationCenter = null)
		{
			m_NavigationCenter = navigationCenter;
		}
		#region Properties
		/// <summary>
		/// <see cref="INavigationService"/>
		/// </summary>
		public Func<IViewBase, double, double, int, Task> XManipulator { get; set; }
		/// <summary>
		/// <see cref="INavigationService"/>
		/// </summary>
		public Func<IViewBase, double, double, int, Task> YManipulator { get; set; }
		/// <summary>
		/// <see cref="INavigationService"/>
		/// </summary>
		public Func<IViewBase, double, double, int, Task> OpacityManipulator { get; set; }
		#endregion // Properties
		/// <remarks>
		/// See <see cref="INavigationService"/>.
		/// </remarks>
		public virtual Task<bool> PopAsync(bool animate = true)
		{
			throw new NotImplementedException();
		}
		/// <remarks>
		/// See <see cref="INavigationService"/>.
		/// </remarks>
		public virtual Task<bool> PopModalAsync(bool animate = true)
		{
			throw new NotImplementedException();
		}
		/// <remarks>
		/// See <see cref="INavigationService"/>.
		/// </remarks>
		public virtual async Task PushAsync(INavigableController navigableController, bool animate = true)
		{
			if (navigableController == null) throw new ArgumentNullException(nameof(navigableController));
			if (CurrentNonModal != navigableController)
				await CurrentNonModal.OnDisappearingAsync();

			using (var disposableListWrapper = NavigationCenter.DisposableNonModalNavigationList)
			{
					disposableListWrapper.LockedList.Add(navigableController);
			}

// KRM in progress			await RefreshViewOnPush();
			await navigableController.OnAppearingAsync();
		}
		/// <remarks>
		/// See <see cref="INavigationService"/>.
		/// </remarks>
		public virtual Task SwitchCurrentAsync(INavigableController navigableController,
			bool animate = true)
		{
			throw new NotImplementedException();
		}
		/// <remarks>
		/// See <see cref="INavigationService"/>.
		/// </remarks>
		public virtual Task SwitchCurrentAsync<T>(Action<T> initialize = null,
			bool animate = true) where T : INavigableController
		{
			throw new NotImplementedException();
		}
		/// <remarks>
		/// See <see cref="INavigationService"/>.
		/// </remarks>
		public virtual Task PushModalAsync<T>(IDismissableNavigableController dismissableNavigatingController,
			bool animate = true)
		{
			throw new NotImplementedException();
		}
		/// <remarks>
		/// See <see cref="INavigationService"/>.
		/// </remarks>
		public virtual Task<bool> PopToRootAsync(bool animate)
		{
			throw new NotImplementedException();
		}
		/// <remarks>
		/// See <see cref="INavigationService"/>.
		/// </remarks>
		public virtual Task<bool> PopPopupAsync(bool animate = true)
		{
			throw new NotImplementedException();
		}
		/// <remarks>
		/// See <see cref="INavigationService"/>.
		/// </remarks>
		public virtual void SwitchRootController(INavigableBaseController navigableBaseController,
			bool animate = true)
		{
			throw new NotImplementedException();
		}
		/// <remarks>
		/// See <see cref="INavigationService"/>.
		/// </remarks>
		public virtual INavigableController CurrentNonModal
		{
			get => m_CurrentNonModal;
			set => m_CurrentNonModal = value;
		}
		/// <remarks>
		/// See <see cref="INavigationService"/>.
		/// </remarks>
		public virtual IDismissableNavigableController CurrentModal
		{
			get => m_CurrentModal;
			set => m_CurrentModal = value;
		}
		/// <remarks>
		/// See <see cref="INavigationService"/>.
		/// </remarks>
		protected virtual INavigationServiceCenter NavigationCenter
		{
			get { return m_NavigationCenter; }
			set { m_NavigationCenter = value; }
		}
		protected virtual async void RefreshViewOnPush(IViewBase view)
		{

		}
	}
}

