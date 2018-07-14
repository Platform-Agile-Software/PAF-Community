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
using System.Threading.Tasks;

namespace PlatformAgileFramework.Notification.AbstractViewControllers
{
	/// <summary>
	/// This is a general model of the lifecycle of a view from the controller's perspective.
	/// These phases are platform-independent, as are the view controller's.
	/// </summary>
	public interface IGenericViewLifecycle
	{
		/// <summary>
		/// This method is called when a controller's view is presented
		/// to the user, becoming an active view.
		/// </summary>
		/// <returns>
		/// <see cref="Task"/> to be examined for completion status.
		/// </returns>
		/// <exceptions>
		/// Any exceptions should be attached to the <see cref="Task"/>.
		/// </exceptions>
		Task OnAppearing();
		/// <summary>
		/// This method is called when the controller or view is created.
		/// Sometimes a controller is expensive in terms of resources used
		/// and this is where those resources are created.
		/// </summary>
		/// <returns>
		/// <see cref="Task"/> to be examined for completion status.
		/// </returns>
		/// <exceptions>
		/// Any exceptions should be attached to the <see cref="Task"/>.
		/// </exceptions>
		Task OnCreating();
		/// <summary>
		/// This method is called when the controller or view is inactive.
		/// This assumes that controller/view can be kept alive, but simply
		/// hidden from view.
		/// </summary>
		/// <returns>
		/// <see cref="Task"/> to be examined for completion status.
		/// </returns>
		/// <exceptions>
		/// Any exceptions should be attached to the <see cref="Task"/>.
		/// </exceptions>
		Task OnDisappearing();
		/// <summary>
		/// This method is called when the controller or view is no
		/// longer needed. Sometimes a controller is expensive in terms
		/// of resources used and this is where those resources are released.
		/// </summary>
		/// <returns>
		/// <see cref="Task"/> to be examined for completion status.
		/// </returns>
		/// <exceptions>
		/// Any exceptions should be attached to the <see cref="Task"/>.
		/// </exceptions>
		Task OnDestroying();
	}
}