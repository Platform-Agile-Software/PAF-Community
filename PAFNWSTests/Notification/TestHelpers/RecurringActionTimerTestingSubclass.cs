using System;
using System.Threading;
using System.Threading.Tasks;
using PlatformAgileFramework.Notification.Helpers;

namespace PlatformAgileFramework.Notification.TestHelpers
{
	/// <summary>
	/// Testing subclass of <see cref="RecurringActionTimer"/>. Need to count
	/// calls, etc.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 04jan2018 </date>
	/// <description>
	/// New. Subclass of  <see cref="RecurringActionTimer"/> 
	/// </description>
	/// </contribution>
	/// </history>
	public class RecurringActionTimerTestingSubclass : RecurringActionTimer
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Allows us to tally the count of calls made by the task.
		/// </summary>
		public int m_NumCalls;
		/// <summary>
		/// Allows us to count the number of times a main task was started.
		/// </summary>
		public int m_NumMainTasks;
		/// <summary>
		/// Allows us to tally the count of how many times the main
		/// task was continued/disposed.
		/// </summary>
		public int m_NumDisposals;
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <remarks>
		/// See base class.
		/// </remarks>
		public RecurringActionTimerTestingSubclass(Action actionToPerform)
		:base(actionToPerform)
		{
		}
		#endregion // Constructors
		#region Methods
		/// <summary>
		/// Overidden method just counts calls.
		/// </summary>
		protected override void ActionInvoker()
		{
			base.ActionInvoker();

			// Addition is NOT atomic.
			Interlocked.Add(ref m_NumCalls, 1);
		}
		/// <summary>
		/// Overidden method just counts task startups.
		/// </summary>
		protected override void StartTask()
		{
			base.StartTask();

			// Addition is NOT atomic.
			Interlocked.Add(ref m_NumMainTasks, 1);
		}
		/// <summary>
		/// Overidden method just counts task continuations onto the disposal thread.
		/// </summary>
		protected override void TaskContinuationMethod(Task mainTask)
		{
			base.TaskContinuationMethod(mainTask);

			// Addition is NOT atomic.
			Interlocked.Add(ref m_NumDisposals, 1);
		}
		#endregion // Methods
	}
}