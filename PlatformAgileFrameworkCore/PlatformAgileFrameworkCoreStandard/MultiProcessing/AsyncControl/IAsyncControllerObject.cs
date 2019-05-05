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
using System.Collections.Generic;
using System.Threading;

namespace PlatformAgileFramework.MultiProcessing.AsyncControl
{
	/// <summary>
	/// <para>
	/// This interface provides simple control functionality for multi-threaded operations.
	/// A <see cref="IAsyncControllerObject"/> is able to monitor the execution of a number
	/// of tasks. This entire infrastructure has nothing to do with calling methods
	/// on threads which do the actual work. It is designed, instead, to send/receive
	/// control signals to/from executing tasks.
	/// </para>
	/// <para>
	/// Tasks are added, then the calling method waits
	/// for their completion. This particular implementation provides a two-level
	/// delegation scheme, where an outer delegate  calls an inner delegate.
	/// </para>
	/// <para>
	/// <para>
	/// To apply different times to tasks, assign each an individual controller.
	/// </para>
	/// </para>
	/// </summary>
	/// <remarks>
	/// The model envisioned here optionally employs a <see cref="Timer"/>. Implementations
	/// should disable periodic callbacks and enable callback after a delay only. The "abort"
	/// functionality assumes that simultaneous access by multiple callback threads is precluded.
	/// </remarks>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 31mar2019 </date>
	/// <description>
	/// Checked DOCs and added history.
	/// </description>
	/// </contribution>
	/// </history>
	public interface IAsyncControllerObject: IAsyncControlObject
	{
		#region Properties
		/// <summary>
		/// Getter for the delegate plugin that will be called back periodically.
		/// This delegate has access to only the <see cref="IAsyncControllerObject"/> by
		/// default and can examine controlled threads/tasks and manipulate them.
		/// </summary>
		Action<IAsyncControllerObject> ControllerDelegate
		{ get; }
		/// <summary>
		/// Delegate used to call <see cref="ControllerDelegate"/>. TYPICAL delegates
		/// are expecting an incoming object that implements <see cref="IAsyncControllerObject"/>
		/// or can be used to produce one. This is the delegate that adjusts timers
		/// and counters. This can be called on a thread which would be a "control thread".
		/// </summary>
		Action<object> ControllerDelegateCallerDelegate
		{ get; }
		/// <summary>
		/// Time remaining time to wait for all tasks completed after
		/// <see cref="IAsyncControlObject.ProcessShouldTerminate"/> has been set.
		/// If tasks have not stopped, they are aborted when this time has expired.
		/// Set to 0 to abort when the wait time has expired. Set to
		/// <see cref="TimeSpan.MaxValue"/> avoid the internal abort process altogether.
		/// </summary>
		/// <remarks>
		/// This property can be expected to be counted down while the controller
		/// is running.
		/// </remarks>
		TimeSpan AbortTimeRemaining { get; set; }
		/// <summary>
		/// Time interval for checking on thread completion. Callback
		/// method is called this often. This is used when a polling operation is
		/// used to periodically check on completion status of controlled objects.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Callback method may be called less often if the processing time during any
		/// given callback is more than <see cref="ProcessCheckInterval"/>. In this case, the
		/// next callback is scheduled immediately after processing is complete.
		/// </para>
		/// <para>
		/// The delay in a parent controller must be considered in conjunction with that
		/// of its children (if any). Generally, speaking the parent delay should not be
		/// less than that of children and should actually be more than twice the value.
		/// This ensures that child tasks have an opportunity to stop processing when
		/// they are signaled.
		/// </para>
		/// </remarks>
		TimeSpan ProcessCheckInterval
		{ get; }
		/// <summary>
		/// Getter for the <see cref="IAsyncControlObject"/>s that the controller is
		/// watching. This represents the snapshot of the objects when the "watch" is
		/// started. This will be different than the collection of total control objects
		/// added only if the caller adds objects when the controller is running.
		/// </summary>
		IEnumerable<IAsyncControlObject> ControlObjects
		{ get; }
		/// <summary>
		/// This is the remaining number of iterations that an iteration-based delegate
		/// will be allowed to run. This is defaulted to -1 to disable iteration-based
		/// stopping criteria. This variable is counted down during operation. This
		/// property is included in the model to optionally support the "unit of work"
		/// protocol for processing delegates.
		/// </summary>
		long IterationsRemaining
		{ get;  set; }
		/// <summary>
		/// Internal timer that we use to call us back to check the task completion. This
		/// timer should never be created if the controller is running on a thread. indicated by
		/// <see cref="IAsyncControlObject.TaskOrThreadId"/> > <see cref="int.MinValue"/>. If not,
		/// a wait timer can be created to periodically callback the
		/// delegates.
		/// </summary>
		Timer ProcessRunTimer
		{ get; set; 
		}
		/// <summary>
		/// Time remaining to wait for all tasks completed. After this time, the
		/// <see cref="IAsyncControlObject"/>'s <see cref="IAsyncControlObject.ProcessShouldTerminate"/>
		/// property should be set to <see langword="true"/>.
		/// </summary>
		/// <remarks>
		/// This property can be expected to be counted down while the controller
		/// is running. <see cref="TimeSpan.MaxValue"/> is a signal value that will
		/// disable time-based process termination.
		/// </remarks>
		TimeSpan ProcessRunTimeRemaining { get; set; }
		#endregion // Properties
		#region Methods
		/// <summary>
		/// Adds a control object to the watch list. Adding a control object when
		/// the <see cref="IAsyncControllerObject"/> is running will have no effect
		/// until it is stopped.
		/// </summary>
		/// <param name="controlObject">
		/// A properly initialized <see cref="IAsyncControlObject"/>.
		/// </param>
		void AddControlObject(IAsyncControlObject controlObject);
		/// <summary>
		/// This is a method that can be called on a thread that should not return
		/// until all processing by control objects is complete. In its simplest
		/// form it just waits until <see cref="IAsyncControlObject.ProcessHasTerminated"/>
		/// is set.
		/// </summary>
		/// <param name="obj">
		/// Usually a <see cref="IAsyncControllerObject"/>. Can be <see langword="null"/>,
		/// depending on the implementation. 
		/// </param>
		void ControlProcess(object obj);
		#endregion // Methods
	}
}
