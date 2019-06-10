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
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using PlatformAgileFramework.MVC.Views;
namespace PlatformAgileFramework.MVC.BindingHelpers
{
	/// <summary>
	/// These are binding helpers for views that need an object to be bound.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 03jun19 </date>
	/// <description>
	/// New.
	/// </description>
	/// </contribution>
	/// </history>
	public static class BindingHelpersAndExtensions
	{
		#region Methods
		/// <summary>
		/// We often use a content page and attach stuff to the content
		/// view and not the outer page. This just figures out if we are a content
		/// page and detaches the binding from the content, or, if not,
		/// detaches it from the view directly. 
		/// </summary>
		public static void RemoveBindingOrContentBinding(this IViewBase view)
		{
			if (view is IContentViewBase contentViewBase)
			{
				contentViewBase.Content.BindingObject = null;
				return;
			}

			view.BindingObject = null;
		}
		/// <summary>
		/// We often use a content page and attach stuff to the content
		/// view and not the outer page. This just figures out if we are a content
		/// page and attaches the binding to the content, or, if not,
		/// attaches it to the view directly. 
		/// </summary>
		public static void AttachBindingOrContentBinding(this IViewBase view, object bindingObject)
		{
			if (view is IContentViewBase contentViewBase)
			{
				contentViewBase.Content.BindingObject = bindingObject;
				return;
			}

			view.BindingObject = bindingObject;
		}
		#endregion // Methods
	}
}
