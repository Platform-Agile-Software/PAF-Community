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

using System.ComponentModel;
using PlatformAgileFramework.MVC.Controllers;

namespace PlatformAgileFramework.MVC.Views
{
	/// <summary>
	/// This interface provides a prescription for what any view in the
	/// PAF MVC model must implement.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 01apr18 </date>
	/// New. Refactored original to be more modular.
	/// </contribution>
	/// </history>
	/// <remarks>
	/// Both view and controller must broadcast property changed events.
	/// </remarks>
	public interface IViewBase : INotifyPropertyChanged
	{
		/// <summary>
		/// Reference to this view's controller.
		/// </summary>
		IControllerBase ControllerBase { get; set; }
	}
}

//	class ViewBase : IViewBase
//	{
//		private IControllerBase m_ControllerBase;
//		public virtual IControllerBase ControllerBase
//		{
//			get => m_ControllerBase;
//			set => m_ControllerBase = value;
//		}
//	}
//	/// <summary>
//	/// Base page.
//	/// </summary>
//	[ContentProperty("Content")]
//	public class BasePage : BindableObject
//	{
//		/// <summary>
//		/// The content property.
//		/// </summary>
//		public static readonly BindableProperty ContentProperty =
//			BindableProperty.Create(
//				"Content", typeof(View), typeof(BasePage),
//				defaultValue: default(View));

//		/// <summary>
//		/// Gets or sets the content.
//		/// </summary>
//		/// <value>The content.</value>
//		public View Content
//		{
//			get { return (View)GetValue(ContentProperty); }
//			set { SetValue(ContentProperty, value); }
//		}
//	}

//	//TODO: Rename this to BaseContentView?
//	[ContentProperty("Content")]
//	public class BasePage<T> : BasePage, IViewFor<T> where T : BaseNavigationViewModel
//	{
//		T _viewModel;

//		/// <summary>
//		/// Gets or sets the view model.
//		/// </summary>
//		/// <value>The view model.</value>
//		public T ViewModel
//		{
//			get => _viewModel;
//			set
//			{
//				_viewModel = value;
//				BindingContext = _viewModel;
//			}
//		}

//		/// <summary>
//		/// Gets or sets the view model.
//		/// </summary>
//		/// <value>The nomad. core. IV iew for. view model.</value>
//		object IViewFor.ViewModel
//		{
//			get => _viewModel;
//			set => ViewModel = (T)value;
//		}
//	}
//}
//}