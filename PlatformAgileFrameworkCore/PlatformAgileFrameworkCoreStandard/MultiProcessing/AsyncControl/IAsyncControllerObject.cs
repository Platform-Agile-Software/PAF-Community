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
using System.Collections.Generic;
using System.Security;
using System.Threading;

namespace PlatformAgileFramework.MultiProcessing.AsyncControl
{
	/// <summary>
	/// <para>
	/// This interface provides simple control functionality for multi-threaded operations.
	/// A <see cref="IAsyncControllerObject"/> is able to monitor the execution of a number
	/// of tasks.
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
	public interface IAsyncControllerObject: IAsyncControlObject
	{
		#region Properties
		/// <summary>
		/// Maximum time to wait for all tasks completed after
		/// <see cref="IAsyncControlObject.ProcessShouldTerminate"/> has been set.
		/// If tasks have not stopped, they are aborted when this time has expired.
		/// Set to 0 to abort when the wait time has expired.
		/// </summary>
		/// <remarks>
		/// This property can be expected to be counted down while the controller
		/// is running.
		/// </remarks>
		TimeSpan AbortTime { get; [SecurityCritical] set; }
		/// <summary>
		/// Time interval for checking on thread completion. Callback
		/// method is called this often.
		/// </summary>
		/// <remarks>
		/// <para>
		/// Callback method may be called less often if the processing time during any
		/// given callback is more than <see cref="CallbackDelay"/>. In this case, the
		/// next callback is scheduled immediately after processing is complete.
		/// </para>
		/// <para>
		/// The delay in a parent controller must be considered in conjunction with that
		/// of its children (if any). Generally, speaking the parent delay should not be
		/// less than that of children and should actuallly be more than twice the value.
		/// This ensures that child tasks have an opportunity to stop processing when
		/// they are signalled.
		/// </para>
		/// </remarks>
		TimeSpan CallbackDelay
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
		/// This is the maximum number of iterations that an iteration-based delegate
		/// will be allowed to run. This is defaulted to 0 to disable iteration-based
		/// stopping criteria. This variable is counted down during operation. This
		/// property is included in the model to optionally support the "unit of work"
		/// protocol for processing delegates.
		/// </summary>
		long MaxIterations
		{ get; [SecurityCritical] set; }
		/// <summary>
		/// Maximum time to wait for all tasks completed. After this time, the
		/// <see cref="IAsyncControlObject"/>'s <see cref="IAsyncControlObject.ProcessShouldTerminate"/>
		/// property should be set to <see langword="true"/>.
		/// </summary>
		/// <remarks>
		/// This property can be expected to be counted down while the controller
		/// is running.
		/// </remarks>
		TimeSpan WaitTime { get; [SecurityCritical] set; }
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
		#endregion // Methods
	}
}
