﻿//@#$&+
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

using System;
namespace PlatformAgileFramework.MVC.Notifications.Events
{
	/// <summary>
	/// This is an interface that is typically worn by PAF
	/// view controllers and other classes that use our notification system
	/// when changing connections (bindings).
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date>29may2019 </date>
	/// <description>
	/// Had to change the name for the overlay on Xamarin.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date>23jan2018 </date>
	/// <description>
	/// New.
	/// </description>
	/// </contribution>
	/// </history>
	public interface INotifyBindingContextChanging
	{
		/// <summary>
		/// Notifies when a binding between a controller and a view is changing.
		/// </summary>
		event EventHandler BindingContextChanging;
	}
}