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
using PlatformAgileFramework.Collections;
namespace PlatformAgileFramework.MVC.Controllers.Navigation
{
	/// <summary>
	/// This class provides the basics for navigation among views through their
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
	public interface INavigationServiceCenter: INavigationService
	{
		/// <summary>
		/// List of modal navigables. NOT a stack. Use it in a using block. This list can be
		/// empty if there are no modals.
		/// </summary>
		IDisposableListProvider<IDismissableNavigableController> DisposableModalNavigationList { get; }

		/// <summary>
		/// List of non-modal navigables. NOT a stack. Use it in a using block. In our model,
		/// this will always contain one entry at the bottom of the list, which is the
		/// root controller, wearing the <see cref="INavigationServiceCenter"/> interface.
		/// </summary>
		IDisposableListProvider<INavigableController> DisposableNonModalNavigationList { get; }


	}
}

