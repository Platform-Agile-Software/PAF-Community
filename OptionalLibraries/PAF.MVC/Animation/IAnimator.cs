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
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace PlatformAgileFramework.MVC.Animation
{
	/// <summary>
	/// This interface prescribes the protocol for animations of things,
	/// typically views.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 06jun19 </date>
	/// New.
	/// </contribution>
	/// </history>
	public interface IAnimator: IEnumerable<IAnimator>
	{
		/// <summary>
		/// Animates (changes) a <see cref="double"/> value from
		/// a starting value to an ending value. Base interface
		/// offers linear animation only. Synchronous version.
		/// </summary>
		void Animate();
		#region Properties
		/// <summary>
		/// Stored until fired.
		/// </summary>
		Action<double> Callback { get; }
		/// <summary>
		/// Stored until fired.
		/// </summary>
		int TimeInMilliseconds { get; }
		/// <summary>
		/// Stored until fired.
		/// </summary>
		double Start { get; }
		/// <summary>
		/// Stored until fired.
		/// </summary>
		double End { get; }
		/// <summary>
		/// Stored until fired.
		/// </summary>
		Action Finished { get; }
		#endregion // Properties
		/// <summary>
		/// Child IAnimators which will be fired.
		/// </summary>
		IList<IAnimator> Children { get; set; }
		/// <summary>
		/// Kicks the animations off asynchronously.
		/// </summary>
		Task StartAnimationAsync();
	}
}
