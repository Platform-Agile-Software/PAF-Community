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

#region Using Directives
using System;
using PlatformAgileFramework.MultiProcessing.Threading;
using PlatformAgileFramework.Properties;

#endregion

namespace PlatformAgileFramework.Events
{
	/// <summary>
	/// This class implements some simple extensions for <see cref="EventArgs"/>
	/// </summary>
	public static class EventArgExtensions
	{
		#region Methods
		/// <summary>
		/// This method provides a thread-safe way to raise an event. The problem
		/// it avoids is the <see langword="null"/>ing of the handler before the delegate list
		/// can be accessed in a multi-threaded environment.
		/// </summary>
		/// <param name="e">Regular argument to event handlers.</param>
		/// <param name="eventHandler">The event handler.</param>
		/// <param name="sender">Ordinary argument to event handler</param>
		public static void RaiseEvent<T>(this T e, ref EventHandler eventHandler,
			[CanBeNull] object sender = null) where T : EventArgs
		{
			var eventHandlerSnapshot
				= AtomicUtils.GetNullableItem(ref eventHandler);

		    eventHandlerSnapshot?.Invoke(sender, e);
		}

		/// <summary>
		/// This method provides a thread-safe way to raise an event. The problem
		/// it avoids is the <see langword="null"/>ing of the handler before the delegate list
		/// can be accessed in a multi-threaded environment.
		/// </summary>
		/// <typeparam name="T">Must be an <see cref="EventArgs"/>.</typeparam>
		/// <param name="e">Regular argument to event handlers.</param>
		/// <param name="eventHandler">The event handler.</param>
		/// <param name="sender">Ordinary argument to event handler</param>
		public static void RaiseEvent<T>(this T e, ref EventHandler<T> eventHandler,
			[CanBeNull] object sender = null) where T : EventArgs
		{
			var eventHandlerSnapshot
				= AtomicUtils.GetNullableItem(ref eventHandler);

		    eventHandlerSnapshot?.Invoke(sender, e);
		}

		#endregion // Methods
	}
}
