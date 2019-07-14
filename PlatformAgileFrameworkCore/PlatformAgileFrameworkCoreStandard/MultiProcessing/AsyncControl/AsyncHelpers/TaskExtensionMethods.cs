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

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PlatformAgileFramework.Collections.ExtensionMethods;
using PlatformAgileFramework.Manufacturing;
namespace PlatformAgileFramework.MultiProcessing.AsyncControl.AsyncHelpers
{
	/// <summary>
	/// Extension methods for <see cref="Task"/>s.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 13jul2019 </date>s
	/// <description>
	/// Moved a couple from Golea.
	/// </description>
	/// </contribution>
	/// </history>
	public static class TaskExtensionMethods
	{
		/// <summary>
		/// Provides a timeout for a set of <see cref="Task"/>. Awaits
		/// are often done one after the other in an async method with no
		/// method of preventing tasks from going off into neverland.
		/// This method is often used on a SINGLE async method to prevent it from
		/// running too long.
		/// </summary>
		/// <param name="tasks">
		/// Set of tasks to set a maximum completion time on. If timeout has
		/// been exceeded on ALL tasks (if NO task has completed within the time
		/// limit) <see langword="false"/> is returned.
		/// </param>
		/// <param name="timeoutInMilliseconds">
		/// The time to let the tasks try to complete before declaring a timeout.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if at least one task has completed before the timeout.
		/// </returns>
		public static Task<bool> TimeWaiter(
			this IEnumerable<Task> tasks, int timeoutInMilliseconds)
		{
			// Little bit easier to convert from int to bool with a TCS.
			var tcs = new TaskCompletionSource<bool>();
			tasks = tasks.ToArray();
			var timedTasks = new List<Task>();

			// Shove in a delay in position 0.
			timedTasks.Add(Task.Delay(timeoutInMilliseconds));

			// The tasks we are timing go behind it.
			timedTasks.AddItems(tasks);

			// Run the tasks with the timer.
			var which = Task.Run(() => Task.WaitAny(timedTasks.ToArray()));

			// It's ok if timer didn't finish first.
			tcs.SetResult(which.Result != 0);
			return tcs.Task;
		}
		/// <summary>
		/// Provides a timeout for a set of <see cref="Task"/>. Awaits
		/// are often done one after the other in an async method with no
		/// method of preventing tasks from going off into neverland.
		/// This method is often used on a SINGLE async method to prevent it from
		/// running too long.
		/// </summary>
		/// <param name="task">
		/// Single task to set a maximum completion time on. If timeout has
		/// been exceeded <see langword="false"/> is returned.
		/// </param>
		/// <param name="timeoutInMilliseconds">
		/// The time to let the task try to complete before declaring a timeout.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if  task has completed before the timeout.
		/// </returns>
		public static Task<bool> TimeWaiter(
			this Task task, int timeoutInMilliseconds)
		{
			var tasks = new List<Task>();
			tasks.Add(task);

			return tasks.TimeWaiter(timeoutInMilliseconds);
		}
	}
}
