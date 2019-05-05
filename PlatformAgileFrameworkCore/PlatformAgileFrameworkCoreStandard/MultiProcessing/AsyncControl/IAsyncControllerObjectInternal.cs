//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 Icucom Corporation
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
using System.Runtime.CompilerServices;
using System.Threading;

namespace PlatformAgileFramework.MultiProcessing.AsyncControl
{
	/// <summary>
	/// This interface allows access to internal members of an <see cref="IAsyncControllerObject"/>.
	/// This interface is designed to allow framework developers to access the "unsafe" elements
	/// of the asynchronous processing mechanisms in their own frameworks. Delegates are protected,
	/// Timer is protected and the set methods for countdown times are protected in this fashion.
	/// These items should only be accessed on the single thread running through a given
	/// <see cref="IAsyncControllerObject"/> or be synchronized.
	/// </summary>
	/// <remarks>
	/// If the developer needs to author code that accesses these data, enable external assemblies
	/// to access the internals of this assembly with the <see cref="InternalsVisibleToAttribute"/> attribute
	/// applied to this assembly.
	/// </remarks>
	internal interface IAsyncControllerObjectInternal : IAsyncControlObjectInternal, IAsyncControllerObject
	{
		#region Properties
		/// <summary>
		/// Internal timer that we use to call us back to check the task completion. This
		/// timer should never be created if the controller is running on a thread, indicated by
		/// the existence of <see cref="IAsyncControlObject.ManagedThread"/>. If the managed
		/// thread is <see langword="null"/>, a wait timer can be created to periodically callback the
		/// delegates.
		/// </summary>
		Timer WaitTimer
		{ get; set; }
		#endregion// Properties
		#region Methods
		/// <summary>
		/// Setter for <see cref="IAsyncControllerObject.AbortTimeRemaining"/>.
		/// </summary>
		void SetAbortTimeRemainingInternal(TimeSpan abortTimeRemaining);
		/// <summary>
		/// Setter for <see cref="IAsyncControllerObject.ProcessRunTimeRemaining"/>.
		/// </summary>
		void SetWaitTimeRemainingInternal(TimeSpan waitTimeRemaining);
		/// <summary>
		/// Setter for <see cref="IAsyncControllerObject.IterationsRemaining"/>.
		/// </summary>
		void SetIterationsRemainingInternal(long iterationsRemaining);
		#endregion // Methods
	}
}
