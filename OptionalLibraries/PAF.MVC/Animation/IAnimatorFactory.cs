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
namespace PlatformAgileFramework.MVC.Animation
{
	/// <summary>
	/// This interface prescribes the protocol for providing animations.
	/// Normally accesses a static that is platform-dependent.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 06jun19 </date>
	/// New.
	/// </contribution>
	/// </history>
	public interface IAnimatorFactory
	{
		/// <summary>
		/// Generates an animator.
		/// </summary>
		/// <returns>
		/// An animator.
		/// </returns>
		/// <remarks>
		/// Params passed to animator constructor.
		/// </remarks>
		IAnimator GetAnimator(Action<double> callback, IEnumerable<IAnimator> children = null,
		int timeInMilliseconds = 250, double start = 0, double end = 1, Action finished = null);
	}
}
