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

using PlatformAgileFramework.Notification.AbstractViewControllers;
using System.Threading.Tasks;
using PlatformAgileFramework.MVC.Views;
namespace PlatformAgileFramework.MVC.Controllers.Navigation
{
	/// <summary>
	/// This interface prescribes the protocol for navigating controllers.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 12may19 </date>
	/// Updated completely. Basically new. Moved all of the dialog stuff
	/// into dialog service, which now implements <see cref="Views.IGenericViewLifecycle"/>.
	/// </contribution>
	/// </history>
	public interface IDismissableNavigableController : INavigableController
	{
		/// <summary>
		/// The dismiss method.
		/// </summary>
		Task Dismiss();
		/// <summary>
		/// True if in a state where it can be dismissed.
		/// </summary>
		bool CanDismiss { get; }
	}
}

