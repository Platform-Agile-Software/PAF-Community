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
using System.ComponentModel;
using System.Security;
using PlatformAgileFramework.TypeHandling.Disposal;

namespace PlatformAgileFramework.MultiProcessing.AsyncControl
{
	/// <summary>
	/// <para>
	/// This interface provides simple control functionality for multi-threaded operations.
	/// By implementing this interface, thread data objects are able to signal to a
	/// controller when they are finished and whether any exceptions have been generated.
	/// </para>
	/// <para>
	/// It is entirely up to the user to create a processing delegate method that periodically
	/// checks the <see cref="ProcessShouldTerminate"/> value and stop processing when it is
	/// <see langword="true"/>.
	/// </para>
	/// </summary>
	/// <threadsafety>
	/// Must be thread-safe.
	/// </threadsafety>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 10apr2019 </date>
	/// <description>
	/// Put <see cref="INotifyPropertyChanged"/> into the base interface for
	/// code consolidation in the .Net standard port.
	/// </description>
	/// </contribution>
	/// <author> KRM </author>
	/// <date> 06jun2012 </date>
	/// <contribution>
	/// <description>
	/// Redid this so all controllers will use a secured disposal procedure.
	/// Controllers are expected to have an <see cref="IDisposable"/> implementation
	/// that has a Dispose method that is <see cref="SecurityCriticalAttribute"/>,
	/// but also have a disposal surrogate. Default implementation has been redesigned
	/// to support this.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 04apr2012 </date>
	/// <description>
	/// Refactored this out of "IThreadControlObject" in the monolithic program
	/// so Core could have a simple thread controller.
	/// </description>
	/// </contribution>
	/// </history>
	/// <remarks>
	/// If an implementation will always have its dispose method called in an
	/// elevated-trust environment, <see cref="IUnprotectedDisposableProvider"/>
	/// could be a vacuous implementation. If the implementations are to be used
	/// in possibly a low-trust environment, the implementation of
	/// <see cref="IUnprotectedDisposableProvider"/> should be used with a
	/// key if we care about who can dispose the instance of an implementation.
	/// </remarks>
	public interface IAsyncControlObject: IUnprotectedDisposableProvider,
		INotifyPropertyChanged, IDisposable
	{
		#region Properties
		/// <summary>
		/// This variable is exposed as a signal that an abort process has been started or
		/// requested. Some implementations can provide a "last ditch" attempt to terminate
		/// a task gracefully in the abort process and require this flag. This is a "one-way"
		/// flag. It can never be set to <see langword="false"/> from the setter, so a
		/// backing field is normally used.
		/// </summary>
		/// <remarks>
		/// Some working in the software field claim that a thread should NEVER be aborted.
		/// This is nonsense. As usual, there is no hard and fast rule about this. An abort
		/// should be performed as a last-ditch effort when calling into, for example, a
		/// third-party library method that hangs without returning. 
		/// </remarks>
		bool IsAborting { get; set; }
		/// <summary>
		/// The ID assigned to a given task or thread when it is created. In the case of a multi-thread
		/// process, this is the thread of the process controller.
		/// </summary>
		int TaskOrThreadId { get; set; }
		/// <summary>
		/// This property is generally written
		/// by the spawned process and read by the spawning process. This
		/// property is used to return any caught exceptions that the
		/// process does not wish to throw. Generally speaking, methods
		/// running on threads should not throw exceptions, since these will
		/// abort the thread's process. Should be synchronized.
		/// </summary>
		Exception ProcessException
		{ get; set; }
		/// <summary>
		/// This property is generally written
		/// by the spawned thread and read by the spawning thread. This
		/// property is <see langword="true"/> when the process has been successfully
		/// started. This property should be set to <see langword="false"/> when an
		/// ACO is created and before it is started. Should be synchronized.
		/// If an ACO is created with a process already executing, this should
		/// be set to <see langword="false"/>. This is a "one-way"
		/// flag. It can never be set to <see langword="false"/> from the setter, so a
		/// backing field is normally used.
		/// </summary>
		bool ProcessHasStarted
		{ get; set; }
		/// <summary>
		/// Getter/setter for the field. This property is generally written
		/// by the spawning thread and read by the spawned process. This is
		/// set to <see langword="true"/> when the process should start. This property
		/// should be set to <see langword="false"/> when an ACO is created and before
		/// it is started. Should be synchronized. If an ACO is created with
		/// a process already executing, this should be set to <see langword="false"/>.
		/// This cannot be a "one-way" flag, since a process may receive a termination
		/// signal before it even starts.
		/// </summary>
		bool ProcessShouldStart
		{ get; set; }
		/// <summary>
		/// This property is generally written
		/// by the spawned thread and read by the spawning thread. This
		/// property is <see langword="true"/> when the thread has been successfully
		/// terminated. This property should be set to <see langword="false"/> when an
		/// ACO is created and before it is started. Should be synchronized. This is a "one-way"
		/// flag. It can never be set to <see langword="false"/> from the setter, so a
		/// backing field is normally used.
		/// </summary>
		bool ProcessHasTerminated
		{ get; set; }
		/// <summary>
		/// Getter/setter for the field. This property is generally written
		/// by the spawning thread and read by the spawned thread. This is
		/// set to <see langword="true"/> when the thread should terminate. This property
		/// should be set to <see langword="false"/> when an ACO is created and before
		/// it is started. Should be synchronized. This is a "one-way"
		/// flag. It can never be set to <see langword="false"/> from the setter, so a
		/// backing field is normally used.
		/// </summary>
		bool ProcessShouldTerminate
		{ get; set; }
		#endregion // Properties
	}
}
