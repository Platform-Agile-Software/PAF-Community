﻿//@#$&+
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

#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.MultiProcessing.Threading.Exceptions;

#region Exception shorthand.
using PAFTMED = PlatformAgileFramework.MultiProcessing.Threading.Exceptions.PAFThreadMismatchExceptionData;
using IPAFTMED = PlatformAgileFramework.MultiProcessing.Threading.Exceptions.IPAFThreadMismatchExceptionData;
#endregion // Exception shorthand.
#endregion

namespace PlatformAgileFramework.MultiProcessing.Tasking
{
	/// <summary>
	/// This class extends the <see cref="Task"/> class and provides some utilities.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 14jul2019 </date>
	/// <description>
	/// Added extension method style methods and the <see cref="TimedOutTaskPayload{T}"/>
	/// support.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> JAW(P) </author>
	/// <date> 27jun2015 </date>
	/// <description>
	/// New. Thread-speak converted to to task-speak.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Safe.
	/// </threadsafety>
	// ReSharper disable PartialTypeWithSinglePart
	public static partial class TaskUtils
// ReSharper restore PartialTypeWithSinglePart
	{
		/// <summary>
		/// This method checks to see if two tasks are equal and if not
		/// throws an exception. It works with the task IDs. It is commonly
		/// used on a class with a set of methods that are to be used by a single
		/// task only, usually because the method accesses unsynchronized data.
		/// It is called as a protective "gate" to generate a consistent exception
		/// before concurrency errors can corrupt data.
		/// </summary>
		/// <param name="executingTaskID">
		/// Current task we want to check against another.
		/// </param>
		/// <param name="singleTaskID">
		/// Task to check against.
		/// </param>
		/// <exceptions>
		/// <exception cref="PAFStandardException{T}">
		/// Message: <see cref="PAFThreadMismatchExceptionMessageTags.MULTIPLE_TASK_ACCESS"/>
		/// if the tasks are not the same.
		/// </exception>
		/// </exceptions>
		public static void SingleTaskGuard(int? executingTaskID, int? singleTaskID)
		{
			if (executingTaskID == singleTaskID) return;
			var data = new PAFTMED(-1, singleTaskID.ToString(),
				-1, executingTaskID.ToString());
			throw new PAFStandardException<IPAFTMED>(data, PAFThreadMismatchExceptionMessageTags.MULTIPLE_TASK_ACCESS);
		}

		/// <summary>
		/// The sole purpose of this method is to overcome a problem found in the mono implementation
		/// of <see cref="Task.WaitAny(Task[], int)"/>. We built our own out of necessity. The method
		/// waits on the completion of ANY task in the enumeration up until the specified timeout.
		/// </summary>
		/// <param name="tasks">Set of tasks to wait on.</param>
		/// <param name="timeoutInMilliseconds">Time to wait. -1
		/// for infinite timeout.
		/// </param>
		/// <returns>
		/// -1 for timeout exceeded. Otherwise, the index in the enumeration of the task first completed.
		/// </returns>
		public static int WaitAnyWithTimeout(IEnumerable<Task> tasks, int timeoutInMilliseconds)
		{
			var incomingTasksToWaitOn = tasks.ToList();

			var tasksToWaitOn = incomingTasksToWaitOn;

			if (timeoutInMilliseconds > 0)
			{
				var delayTask = Task.Delay(timeoutInMilliseconds);

				tasksToWaitOn.Insert(0, delayTask);
			}

			var firstToFinish = Task.WaitAny(tasksToWaitOn.ToArray());

			// Account for a possible delay task.
			if (timeoutInMilliseconds > -1) firstToFinish--;

			return firstToFinish;
		}
		/// <summary>
		/// The sole purpose of this method is to overcome a problem found in the mono implementation
		/// of <see cref="Task.WaitAny(Task[], int)"/>. We built our own out of necessity. The method
		/// waits on the completion of ANY task in the enumeration up until the specified timeout.
		/// Extension version
		/// </summary>
		/// <param name="tasks">Set of tasks to wait on.</param>
		/// <param name="timeoutInMilliseconds">Time to wait. -1
		/// for infinite timeout.
		/// </param>
		/// <returns>
		/// -1 for timeout exceeded. Otherwise, the index in the enumeration of the task first completed.
		/// </returns>
		public static int WaitAnyWithTimeoutExtension(this IEnumerable<Task> tasks, int timeoutInMilliseconds)
		{
			return WaitAnyWithTimeout(tasks, timeoutInMilliseconds);
		}
		/// <summary>
		/// Async version that does not block, but returns values based on timeout. This
		/// gives the caller the ability to "asynchronously" wait for tasks to finish
		/// with a constraint on timeout.
		/// </summary>
		/// <param name="tasks">Set of tasks to wait on.</param>
		/// <param name="timeoutInMilliseconds">Time to wait. -1 for infinite timeout.</param>
		/// <returns>
		/// -1 for timeout exceeded. Otherwise, the index in the enumeration of the task first completed.
		/// </returns>
		public static async Task<int> WaitAnyWithTimeoutAsync(IEnumerable<Task> tasks, int timeoutInMilliseconds = -1)
		{

			int firstToFinish;

			var waitTask = await Task<int>.Factory.StartNew(
				() =>
				{
					firstToFinish = WaitAnyWithTimeout(tasks, timeoutInMilliseconds);

					return firstToFinish;
				});
			return waitTask;
		}
		/// <summary>
		/// Async version that does not block, but returns values based on timeout. This
		/// gives the caller the ability to "asynchronously" wait for tasks to finish
		/// with a constraint on timeout. Extension Version.
		/// </summary>
		/// <param name="tasks">Set of tasks to wait on.</param>
		/// <param name="timeoutInMilliseconds">Time to wait. -1 for infinite timeout.</param>
		/// <returns>
		/// -1 for timeout exceeded. Otherwise, the index in the enumeration of the task first completed.
		/// </returns>
		public static async Task<int> WaitAnyWithTimeoutAsyncExtension(this IEnumerable<Task> tasks, int timeoutInMilliseconds = -1)
		{

			return await WaitAnyWithTimeoutAsync(tasks, timeoutInMilliseconds);
		}
		/// <summary>
		/// Async version that does not block, but returns values based on timeout. This
		/// gives the caller the ability to "asynchronously" wait for tasks to finish
		/// with a constraint on timeout.
		/// </summary>
		/// <param name="tasks">Set of tasks to wait on.</param>
		/// <param name="timeoutInMilliseconds">Time to wait. -1 produces infinite timeout.</param>
		/// <returns>
		/// <see langword="false"/> for timeout exceeded.
		/// </returns>
		public static async Task<bool> WaitAllWithTimeoutAsync(IEnumerable<Task> tasks, int timeoutInMilliseconds = -1)
		{

			bool isTimeout;

			var taskArray = tasks.ToArray();

			var waitTask = await Task<bool>.Factory.StartNew(
				() =>
				{
					isTimeout = Task.WaitAll(taskArray, timeoutInMilliseconds);

					return isTimeout;
				});
			return waitTask;
		}
		/// <summary>
		/// Async version that does not block, but returns values based on timeout. This
		/// gives the caller the ability to "asynchronously" wait for tasks to finish
		/// with a constraint on timeout. Extension version. Calls <see cref="WaitAllWithTimeoutAsync"/>.
		/// 
		/// </summary>
		/// <param name="tasks">Set of tasks to wait on.</param>
		/// <param name="timeoutInMilliseconds">Time to wait. -1 produces infinite timeout.</param>
		/// <returns>
		/// <see langword="false"/> for timeout exceeded.
		/// </returns>
		public static async Task<bool> WaitAllWithTimeoutAsyncExtension(this IEnumerable<Task> tasks, int timeoutInMilliseconds = -1)
		{
			return await WaitAllWithTimeoutAsync(tasks, timeoutInMilliseconds);
		}
		/// <summary>
		/// Async version that does not block, but returns values based on timeout. This
		/// gives the caller the ability to "asynchronously" wait for a task to finish
		/// with a constraint on timeout. Extension version.Calls <see cref="WaitAllWithTimeoutAsync"/>
		/// </summary>
		/// <param name="task">Task to wait on.</param>
		/// <param name="timeoutInMilliseconds">Time to wait. -1 produces infinite timeout.</param>
		/// <returns>
		/// <see langword="false"/> for timeout exceeded.
		/// </returns>
		public static async Task<bool> WaitAllWithTimeoutAsyncExtension(this Task task, int timeoutInMilliseconds = -1)
		{
			var tasks = new List<Task>();
			tasks.Add(task);
			return await WaitAllWithTimeoutAsync(tasks, timeoutInMilliseconds);
		}
		/// <summary>
		/// Async version that does not block, but returns values based on timeout. This
		/// gives the caller the ability to "asynchronously" wait for a task to finish
		/// with a constraint on timeout. Extension version. Calls <see cref="WaitAnyWithTimeoutAsync"/>
		/// </summary>
		/// <param name="task">Task to wait on.</param>
		/// <param name="timeoutInMilliseconds">Time to wait. -1 produces infinite timeout.</param>
		/// <returns>
		/// <see cref="ITimedOutTaskPayload{T}.TimedOut"/> will be set to <see langword="false"/> for timeout exceeded.
		/// </returns>
		/// <exceptions>
		/// Exceptions generated in the <see paramref="task"/> are caught and returned in the output.
		/// </exceptions>
		public static async Task<ITimedOutTaskPayload<T>>
			WaitTaskWithTimeoutAsyncTimeOutPayload<T>(this Task<T> task, int timeoutInMilliseconds = -1)
		{
			var timedOutPayload = new TimedOutTaskPayload<T>();
			var tasks = new List<Task>();
			tasks.Add(task);
			int firstTaskToFinishBeforeTimeout = 0;

			try
			{
				firstTaskToFinishBeforeTimeout = await WaitAnyWithTimeoutAsync(tasks, timeoutInMilliseconds);
			}
			catch (Exception ex)
			{
				timedOutPayload.CaughtException = ex;
			}

			// -1 indicates timeout.
			timedOutPayload.TimedOut = firstTaskToFinishBeforeTimeout < 0;

			if (!timedOutPayload.TimedOut)
				timedOutPayload.ReturnValue = task.Result;
			else
				timedOutPayload.ReturnValue = default;

			return timedOutPayload;
		}
		/// <summary>
		/// Async version that does not block, but returns values based on timeout. This
		/// gives the caller the ability to "asynchronously" wait for a task to finish
		/// with a constraint on timeout. Extension version. Calls <see cref="WaitAnyWithTimeoutAsync"/>
		/// This is th non-Generic version.
		/// </summary>
		/// <param name="task">Task to wait on.</param>
		/// <param name="timeoutInMilliseconds">Time to wait. -1 produces infinite timeout.</param>
		/// <returns>
		/// <see cref="ITimedOutTask.TimedOut"/> will be set to <see langword="false"/> for timeout exceeded.
		/// </returns>
		/// <exceptions>
		/// Exceptions generated in the <see paramref="task"/> are caught and returned in the output.
		/// </exceptions>
		public static async Task<ITimedOutTask>
			WaitTaskWithTimeoutAsyncTimeOut(this Task task, int timeoutInMilliseconds = -1)
		{
			var timedOutTask = new TimedOutTask();
			var tasks = new List<Task>();
			tasks.Add(task);
			int firstTaskToFinishBeforeTimeout = 0;

			try
			{
				firstTaskToFinishBeforeTimeout = await WaitAnyWithTimeoutAsync(tasks, timeoutInMilliseconds);
			}
			catch (Exception ex)
			{
				timedOutTask.CaughtException = ex;
			}

			// -1 indicates timeout.
			timedOutTask.TimedOut = firstTaskToFinishBeforeTimeout < 0;

			return timedOutTask;
		}
		/// <summary>
		/// Async version that does not block, but returns values based on timeout. This
		/// gives the caller the ability to "asynchronously" wait for a task to finish
		/// with a constraint on timeout. Extension version. Calls <see cref="WaitAnyWithTimeoutAsync"/>
		/// </summary>
		/// <param name="task">Task to wait on.</param>
		/// <param name="timeoutInMilliseconds">Time to wait. -1 produces infinite timeout.</param>
		/// <returns>
		/// <see langword="false"/> for timeout exceeded.
		/// </returns>
		public static async Task<bool>
			WaitTaskWithTimeoutAsync(this Task task, int timeoutInMilliseconds = -1)
		{
			var tasks = new List<Task>();
			tasks.Add(task);
			var firstTaskToFinishBeforeTimeout = await WaitAnyWithTimeoutAsync(tasks, timeoutInMilliseconds);

			// -1 indicates timeout.
			return firstTaskToFinishBeforeTimeout < 0;
		}
	}
}
