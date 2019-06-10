﻿//@#$&+
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
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using PlatformAgileFramework.MVC.Controllers.Navigation;
using PlatformAgileFramework.MVC.Views;
using Xamarin.Forms;
namespace PlatformAgileFramework.Views
{
	/// <summary>
	/// This interface provides a prescription for what a delegating version of
	/// the Xamarin view base must implement. It allows us to wrap
	/// a view controller into a regular view base and then talk to a
	/// contained Xamarin Page or view, etc. that derives from a
	/// <see cref="BindableObject"/>.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 30may19 </date>
	/// <description>
	/// New. Xam-specific.
	/// </description>
	/// </contribution>
	/// </history>
	public interface IXamFormsDelegatingNavigableViewBase<T, out U>
		: INavigableViewBase<T>
		where T : class, INavigableController where U : BindableObject
	{
		#region Properties
		/// <summary>
		/// Gets the <see cref="BindableObject"/>.
		/// </summary>
		U XamBindableObject { get; }
		#endregion // Properties
	}
}