//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2017 Icucom Corporation
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

using System.Threading;
using PlatformAgileFramework.Application;

// ReSharper disable once CheckNamespace
namespace PatformAgileFramework.MVC.Controllers
{
	/// <summary>
	/// This interface provides a protocol for accessing platform-independent services
	/// that should be available on ALL platforms. This extension of the base interface
	/// includes functionality for graphics applications under .Net Core.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 01apr18 </date>
	/// New. Will use synchronization contexts universally now. Originally
	/// built for Xamarin.Forms support.
	/// </contribution>
	/// </history>
	// TODO KRM - move this to Forms-independent assembly.
	public interface ISharedUIApplication: ISharedApplication
	{
		/// <summary>
		/// Holds the <see cref="SynchronizationContext"/> pushed in from the UI
		/// initialization code on each platform. This allows posts or sends to
		/// the UI thread.
		/// </summary>
		SynchronizationContext UISynchronizationContext
		{
			get;
		}
	}
}