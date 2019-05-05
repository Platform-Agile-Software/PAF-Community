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
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

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
#pragma warning disable 1584,1711,1572,1581,1580
	/// New. Built this so we could use <see cref="IPAFWeakReference{T}"/>s
#pragma warning restore 1584,1711,1572,1581,1580
	/// more easily.
	/// </description>
	/// </contribution>
	public static class PAFWeakReferenceExtensions
	{
		/// <summary>
		/// Just gets the reference from the wrapper if it still alive.
		/// </summary>
		/// <typeparam name="T">
		/// The type of the item referenced.
		/// </typeparam>
		/// <param name="pafWeakReference"></param>
		/// <returns>The reference or <see langword="null"/></returns>
		public static T GetTargetIfAlive<T>(this IPAFWeakableReference<T> pafWeakReference)
			where T : class
		{
			var outT = pafWeakReference.Target;
			return outT;
		}
	}

}