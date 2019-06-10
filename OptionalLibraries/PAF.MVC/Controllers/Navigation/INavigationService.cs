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

using PlatformAgileFramework.MVC.Views;
using System;
using System.Dynamic;
using System.Threading.Tasks;
namespace PlatformAgileFramework.MVC.Controllers.Navigation
{
	/// <summary>
	/// This interface provides the basic protocol for navigation among views through their
	/// controllers.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 17may19 </date>
	/// Updated to support the updated MVC spec..
	/// </contribution>
	/// <contribution>
	/// <author> DAV(P) </author>
	/// <date> 01dec17 </date>
	/// New. 
	/// </contribution>
	/// </history>
	public interface INavigationService
	{
		/// <summary>
		/// Plugin for animation.
		/// </summary>
		Func<IViewBase, double, double, int, Task> XManipulator { get; set; }
		/// <summary>
		/// Plugin for animation.
		/// </summary>
		Func<IViewBase, double, double, int, Task> YManipulator { get; set; }
		/// <summary>
		/// Plugin for animation.
		/// </summary>
		Func<IViewBase, double, double, int, Task> OpacityManipulator { get; set; }
		/// <summary>
		/// Pop current controller off the list.
		/// </summary>
		/// <param name="animate">
		/// <see langword="true"/> to animate.
		/// </param>
		/// <returns>
		/// <see langword="false"/> if the controller list was empty.
		/// </returns>
		Task<bool> PopAsync(bool animate = true);

		/// <summary>
		/// Dismiss current modal from view.
		/// </summary>
		/// <param name="animate">
		/// <see langword="true"/> to animate.
		/// </param>
		/// <returns>
		/// <see langword="false"/> if the controller list was empty.
		/// </returns>
		Task<bool> PopModalAsync(bool animate = true);

		/// <summary>
		/// Push a controller on the top of the list.
		/// </summary>
		/// <returns><see cref="Task"/></returns>
		/// <param name="navigableController">
		/// Controller to put on top of the list. Not <see langword="null"/>.
		/// </param>
		/// <param name="animate">
		/// <see langword="true"/> to animate.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">"navigableController"</exception>
		/// </exceptions>
		Task PushAsync(INavigableController navigableController, bool animate = true);


		/// <summary>
		/// Switch a controller with the current one. The current
		/// one is removed from the list entirely.
		/// </summary>
		/// <returns><see cref="Task"/></returns>
		/// <param name="navigableController">
		/// Controller to switch in.
		/// </param>
		/// <param name="animate">
		/// <see langword="true"/> to animate.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">"navigableController"</exception>
		/// </exceptions>
		Task SwitchCurrentAsync(INavigableController navigableController, bool animate = true);

		/// <summary>
		/// Switch current controller with another.
		/// </summary>
		/// <returns><see cref="Task"/></returns>
		/// <param name="initialize">Initialize.</param>
		/// <param name="animate">
		/// <see langword="true"/> to animate.
		/// </param>
		/// <typeparam name="T">
		/// Must be a <see cref="INavigableController"/>.
		/// </typeparam>
		Task SwitchCurrentAsync<T>(Action<T> initialize = null, bool animate = true)
			where T : INavigableController;
		/// <summary>
		/// Push a controller on the top of the list as a modal.
		/// </summary>
		/// <returns><see cref="Task"/></returns>
		/// <param name="dismissableNavigatingController">
		/// Modal is a "dismissable".
		/// </param>
		/// <param name="animate">
		/// <see langword="true"/> to animate.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">"dismissingNavigableController"</exception>
		/// </exceptions>
		Task PushModalAsync<T>(IDismissableNavigableController dismissableNavigatingController,
			bool animate = true);

		/// <summary>
		/// Remove all from navigation list, but the root controller
		/// </summary>
		/// <returns><see cref="Task"/></returns>
		/// <param name="animate">
		/// <see langword="true"/> to animate.
		/// </param>
		/// <returns>
		/// <see langword="false"/> if the controller list was empty.
		/// </returns>
		Task<bool> PopToRootAsync(bool animate);

		/// <summary>
		/// Remove the modal controller from the top of the list.
		/// </summary>
		/// <returns><see cref="Task"/></returns>
		/// <param name="animate">
		/// <see langword="true"/> to animate.
		/// </param>
		/// <returns>
		/// <see langword="false"/> if the controller list was empty.
		/// </returns>
		Task<bool> PopPopupAsync(bool animate = true);
		/// <summary>
		/// Switch current root. Rarely needed, since the root is normally the
		/// master view of the app and usually doesn't change.
		/// </summary>
		/// <param name="navigableBaseController">
		/// The root has the ability to manipulate the controller hierarchy.
		/// </param>
		/// <param name="animate">
		/// <see langword="true"/> to animate.
		/// </param>
		void SwitchRootController(INavigableBaseController navigableBaseController, bool animate = true);
		/// <summary>
		/// Gets the controller corresponding to the currently displayed view.
		/// This should never be <see langword="null"/>.
		/// </summary>
		INavigableController CurrentNonModal { get; }
		/// <summary>
		/// Gets the controller corresponding to the currently displayed modal view.
		/// This can be <see langword="null"/> if no modals are up. A modal is always
		/// displayed on top of the current non-modal.
		/// </summary>
		IDismissableNavigableController CurrentModal { get; }
	}
}
