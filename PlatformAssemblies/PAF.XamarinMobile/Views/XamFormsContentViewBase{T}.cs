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

using PlatformAgileFramework.MVC.Controllers.Navigation;
using Xamarin.Forms;

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.Views.XamForms
{
	/// <summary>
	/// This class provides the lowest-level implementation of a base implementing
	/// class for navigable content views.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 31may19 </date>
	/// New.
	/// </contribution>
	/// </history>
	[ContentProperty("Content")]
	public class XamFormsNavigableContentViewBase<T>
		: XamFormsNavigableViewBase<T>, IXamContentView
		where T: INavigableController
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// The content property. Needed for consumption by XAML, of course.
		/// </summary>
		public static readonly BindableProperty ContentProperty =
			BindableProperty.Create(
				"Content", typeof(View),
				typeof(XamFormsNavigableContentViewBase<T>),
				defaultValue: default(View));

		#endregion // Class Fields and Autoproperties
		#region Properties
		/// <summary>
		/// Gets or sets the content.
		/// </summary>
		public virtual View Content
		{
			get { return (View)GetValue(ContentProperty); }
			set { SetValue(ContentProperty, value); }
		}
		#endregion // Properties
	}
}
