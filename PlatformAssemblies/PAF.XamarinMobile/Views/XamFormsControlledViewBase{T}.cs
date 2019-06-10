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

using PlatformAgileFramework.MVC.Controllers;
using PlatformAgileFramework.MVC.Views;

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.Views.XamForms
{
	/// <summary>
	/// This class provides the lowest-level implementation of a base implementing
	/// class for views that are bound to a navigable view controller.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 30may19 </date>
	/// New.
	/// </contribution>
	/// </history>
	public class XamFormsControlledViewBase<T> 
		: XamFormsViewBase, IControlledViewBase<T>
		where T: IControllerBase
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Internal backing for testing.
		/// </summary>
		internal T m_ViewController;
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// This constructor is here to support the construct and set style for the
		/// controller.
		/// </summary>
		public XamFormsControlledViewBase()
		{
		}
		/// <summary>
		/// This constructor is here to support the manual construction style.
		/// </summary>
		public XamFormsControlledViewBase(T viewController)
		{
			m_ViewController = viewController;
		}
		#endregion // Constructors
		#region Properties
		/// <summary>
		/// Gets or sets the view controller.
		/// </summary>
		public virtual T ViewController
		{
			get => m_ViewController;
			set
			{
				m_ViewController = value;
				BindingContext = m_ViewController;
			}
		}
		#endregion // Properties

	}
}
