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
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
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
		/// Getter for the delegate plugin that will be called back periodically. This
		/// may be <see langword="null"/>, indicating that an internal default delegate will be used.
		/// This delegate has access to only the <see cref="IAsyncControllerObject"/> by
		/// default and can examine controlled threads/tasks and maninpulate them.
		/// </summary>
		ACOCallerProcessDelegate ControllerDelegate
		{ get; }
		/// <summary>
		/// Delegate used to call <see cref="ControllerDelegate"/>. Can be <see langword="null"/>, in
		/// which case an internal default will be used. TYPICAL delegates are expecting an
		/// incoming object that implements <see cref="IAsyncControllerObjectInternal"/>
		/// or can be used to produce one. This is the delegate that resets timers
		/// and counters and thus has need for access to these items. This means that
		/// developers wishing to author custom implementations of this delegate must
		/// expose the internals of this assembly to their framework code or extension
		/// code.
		/// </summary>
		Action<object> ControllerDelegateCallerDelegate
		{ get; }
		/// <summary>
		/// Internal timer that we use to call us back to check the task completion. This
		/// timer should never be created if the controller is running on a thread, indicated by
		/// the existance of <see cref="IAsyncControlObject.ManagedThread"/>. If the managed
		/// thread is <see langword="null"/>, a wait timer can be created to periodically callback the
		/// delegates.
		/// </summary>
		Timer WaitTimer
		{ get; set; }
		#endregion// Properties
		#region Methods
		/// <summary>
		/// Setter for <see cref="IAsyncControllerObject.AbortTime"/>.
		/// </summary>
		void SetAbortTimeInternal(TimeSpan timeSpan);
		/// <summary>
		/// Setter for <see cref="IAsyncControllerObject.WaitTime"/>.
		/// </summary>
		void SetWaitTimeInternal(TimeSpan timeSpan);
		/// <summary>
		/// Setter for <see cref="IAsyncControllerObject.MaxIterations"/>.
		/// </summary>
		void SetMaxIterationsInternal(long maxIterations);
		#endregion // Methods
	}
}
