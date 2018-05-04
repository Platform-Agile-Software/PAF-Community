//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2016 Icucom Corporation
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

using System;

namespace PlatformAgileFramework.TypeHandling.WeakBindings
{
	/// <summary>
	/// Methods to deal with the fact that weak references are reference types by nature
	/// and don't benefit from "TryGet()" methods
	/// </summary>
	/// <contribution>
	/// <author> Brian T. </author>
	/// <date> 22sep2014 </date>
	/// <description>
	/// New. Built this so we could use the type safe version of weak reference
	/// more easily.
	/// </description>
	/// </contribution>
	public static class WeakReferenceExtensions
	{
		/// <summary>
		/// Fixes Microsoft's goofy implementation.
		/// </summary>
		/// <typeparam name="T">
		/// The type of the item referenced
		/// </typeparam>
		/// <param name="weakReference"></param>
		/// <returns></returns>
		public static T GetTarget<T>(this WeakReference<T> weakReference)
			where T : class
		{
			T outT;
			var success = weakReference.TryGetTarget(out outT);
			if(success)
				return outT;
			return null;
		}
	}

}