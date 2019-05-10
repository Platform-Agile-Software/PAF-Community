//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2019 Icucom Corporation
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PlatformAgileFramework.Annotations;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.ErrorAndException;

// Exceptions
using PlatformAgileFramework.TypeHandling;

// Exception shorthand.
// ReSharper disable IdentifierTypo
using PAFAED = PlatformAgileFramework.ErrorAndException.PAFAggregateExceptionData;
using PTMED = PlatformAgileFramework.TypeHandling.Exceptions.PAFTypeMismatchExceptionData;
// ReSharper restore IdentifierTypo

namespace PlatformAgileFramework.MultiProcessing.AsyncControl
{
	/// <summary>
	/// Extensions and statics for <see cref=" IAsyncControllerObject"/>.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 31mar2019 </date>
	/// <description>
	/// Built this as a convenience for new stochastic tests.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Thread-safe.
	/// </threadsafety>
	/// <serialization>
	/// Class is not anticipated to be serialized.
	/// </serialization>
	public static class AsyncControllerObjectExtensions
	{

		/// <summary>
		/// This is a list of all task groups controlled by controller objects. It
		/// is used to wait for task completion after shutdown of a process.
		/// </summary>
		internal static IDictionary<IAsyncControllerObject, IList<Task>> s_Processes
			= new ConcurrentDictionary<IAsyncControllerObject, IList<Task>>();
		/// <summary>
		/// This is a default method that monitors the execution times and iteration counts
		/// of tasks. It allows tasks to run until <see cref="IAsyncControllerObject.ProcessRunTimeRemaining"/>
		/// has been exceeded or <see cref="IAsyncControllerObject.IterationsRemaining"/>
		/// has reached zero, then signals them all to stop. If the
		/// <see cref="IAsyncControllerObject.AbortTimeRemaining"/> has counted down, the processes
		/// are aborted.
		/// </summary>
		/// <param name="controllerObject">
		/// Standard controller object which contains the branch of tasks to be monitored.
		/// </param>
		public static void TaskStartControllerDelegateMethod
			([NotNull] this IAsyncControllerObject controllerObject)
		{
			if (controllerObject.ProcessHasTerminated) return;
			// If the abort time has been exceeded, we abort children on this branch
			// recursively.
			if (controllerObject.AbortTimeRemaining <= TimeSpan.Zero)
			{
				// Let the world know that we are terminating.
				controllerObject.ProcessShouldTerminate = true;
			}

			// See if we have time left on the clock or more iterations to do.
			if (
				((controllerObject.ProcessRunTimeRemaining > TimeSpan.Zero)
				||
				(controllerObject.IterationsRemaining > 0))
				&& (!controllerObject.ProcessShouldTerminate)
				)
			{

				// Start up our tasks if the first time through.
				if (controllerObject.ProcessShouldStart)
				{
					var processTasks = new List<Task>();

					// Start all of our children.
					foreach (var wCo in controllerObject.ControlObjects.EnumIntoSubtypeList<IAsyncControlObject, IAsyncWorkControlObject>())
					{
						var task = Task.Factory.StartNew(wCo.WorkPayloadObject.ThreadDelegate, wCo.WorkPayloadObject.Payload);
						wCo.TaskOrThreadId = task.Id;
						wCo.ProcessHasStarted = true;
						processTasks.Add(task);
					}

					s_Processes.Add(controllerObject, processTasks);
					// Close the start gate.
					controllerObject.ProcessShouldStart = false;
					return;
				}

				return;
			}

			//// If we are here, we stop the processing.
			// Let the world (including our children) know that we are terminating.
			controllerObject.ProcessShouldTerminate = true;

			// Loop preset.
			var childrenHaveTerminated = true;

			// Terminate all of our children.
			if (controllerObject.ControlObjects != null)
			{
				foreach (var cO in controllerObject.ControlObjects)
				{
					if (!cO.ProcessHasTerminated) childrenHaveTerminated = false;
					cO.ProcessShouldTerminate = true;
				}
			}
			if (childrenHaveTerminated)
			{
				Task.WaitAll(s_Processes[controllerObject].ToArray());
				s_Processes.Remove(controllerObject);
				controllerObject.ProcessHasTerminated = true;
			}
		}
		/// <summary>
		/// This method is employed when no <see cref="IAsyncControllerObject.ControllerDelegateCallerDelegate"/> is
		/// specified.
		/// </summary>
		/// <param name="obj">
		/// This parameter must be a <see cref="IAsyncControllerObject"/>, since it is
		/// immediately cast inside this method to that interface. A different method need
		/// not do the same as this one if it is of a different design. May not be <see langword="null"/>.
		/// This is not provided as an extension method in order not to pollute the
		/// object extension space.
		/// </param>
		/// <para>
		/// This can serve as the <see cref="ParameterizedThreadStart"/> method that is passed to
		/// a managed thread to be executed. This method counts the times down and the timer for
		/// this controller is manipulated. This is also the method in which the iteration count
		/// is decremented if iteration-based stopping criteria is enabled. This method sets the
		/// various flags to abort or terminate the process when various termination criteria
		/// have been satisfied.
		/// </para>
		/// <remarks>
		/// The times are counted down BEFORE the <see cref="IAsyncControllerObject.ControllerDelegate"/>
		/// is invoked, because <see cref="IAsyncControllerObject.ProcessCheckInterval"/> will have elapsed before the
		/// method is called the first time. Also note that the callback times are not
		/// perfectly periodic in this simple controller, since time is suspended during the
		/// processing of the <see cref="IAsyncControllerObject.ControllerDelegate"/> and restarted when 
		/// it is complete. Also note that the <see cref="IAsyncControllerObject.AbortTimeRemaining"/> is not counted
		/// down in the same callback as the <see cref="IAsyncControllerObject.ProcessRunTimeRemaining"/>. The ramification
		/// of this is that a thread will be given at least one iteration to
		/// respond to any termination command that is issued before an abort is forced
		/// unless <see cref="IAsyncControllerObject.AbortTimeRemaining"/> is 0, which is a signal value.
		/// </remarks>
		public static void TaskControllerDelegateCallerDelegateMethod([NotNull] object obj)
		{
			if (obj == null) throw new ArgumentNullException(nameof(obj));
			// todo cast exception.
			if (!(obj is IAsyncControllerObject controllerObject))
				throw new PAFStandardException<PTMED>(
					new PTMED(PAFTypeHolder.IHolder(typeof(object)),
						PAFTypeHolder.IHolder(typeof(IAsyncControllerObject)))
						);
			////////////////////////////////////////////////////////////////////////////////////
			// If we are being told to abort, dispose the timer or thread and
			// quit.
			////////////////////////////////////////////////////////////////////////////////////
			//
			if (controllerObject.IsAborting)
			{
				// First make sure the signal has been given to stop.
				controllerObject.ProcessShouldTerminate = true;
				if (controllerObject.ProcessRunTimer != null)
				{
					// We can safely dispose of the timer here, since by our design, we
					// can't have multiple callbacks scheduled that are still on the queue.
					controllerObject.ProcessRunTimer.Dispose();
					controllerObject.ProcessRunTimer = null;
					return;
				}
			}

			var lastCallTime = DateTime.Now;
			// Avoid multiple calls if running on a timer. We must effectively shut
			// off the timer.
			controllerObject.ProcessRunTimer?.Change(TimeSpan.MaxValue, TimeSpan.FromMilliseconds(-1));

			// Count down the times, if applicable. We don't touch abort time until
			// wait time has counted down.
			if (controllerObject.ProcessRunTimeRemaining > TimeSpan.Zero)
			{
				controllerObject.ProcessRunTimeRemaining
					= (controllerObject.ProcessRunTimeRemaining - controllerObject.ProcessCheckInterval);
			}
			else if (controllerObject.AbortTimeRemaining > TimeSpan.Zero)
			{
				controllerObject.AbortTimeRemaining
					= (controllerObject.AbortTimeRemaining - controllerObject.ProcessCheckInterval);
			}

			// Call the delegate to control the threads.
			controllerObject.ControllerDelegate(controllerObject);

			// If the process has been terminated, we are all done.
			if (controllerObject.ProcessHasTerminated) return;

			// Count down the iterations, if applicable.
			if (controllerObject.IterationsRemaining > 0)
				controllerObject.IterationsRemaining = (controllerObject.IterationsRemaining - 1);

			////////////////////////////////////////////////////////////////////////////////////
			// We need to create a delay that is adjusted by the time we spent in this method.
			////////////////////////////////////////////////////////////////////////////////////
			var newDelay = CalculateAdjustedWaitTime(lastCallTime, controllerObject.ProcessCheckInterval);
			if (controllerObject.ProcessRunTimer != null)
				// Reset for more calls.
				controllerObject.ProcessRunTimer.Change(newDelay, TimeSpan.FromMilliseconds(-1));
			else
				// If we are running on a thread, suspend ourselves for a bit.
				//// Thread.Sleep doesn't do what we expect in the task-based world.
				//Thread.Sleep(newDelay);
				Task.Delay(newDelay).Wait();
		}
		/// <summary>
		/// Little helper to calculate the adjusted wait time.
		/// </summary>
		/// <param name="lastCallTime">
		/// Time at which the wait time was calculated last, which is often approximated
		/// by the time at which a control method was entered.
		/// </param>
		/// <param name="maxWaitInterval">
		/// This is the desired time between call intervals. If the processing that
		/// takes place during a call takes more time than this, no additional delay
		/// is created before the next call. If not, the new wait time is calculated
		/// as the difference.
		/// </param>
		/// <returns>
		/// <see cref="TimeSpan.Zero"/> if <see cref="DateTime.Now"/> minus the
		/// <paramref name="lastCallTime"/> is greater than <paramref name="maxWaitInterval"/>.
		/// <paramref name="maxWaitInterval"/> - (<see cref="DateTime.Now"/> - <paramref name="lastCallTime"/>)
		/// if not. 
		/// </returns>
		public static TimeSpan CalculateAdjustedWaitTime(DateTime lastCallTime, TimeSpan maxWaitInterval)
		{
			var timeSinceOldCall = DateTime.Now - lastCallTime;
			if (timeSinceOldCall > maxWaitInterval) return TimeSpan.Zero;
			return maxWaitInterval - timeSinceOldCall;
		}

	}
}
