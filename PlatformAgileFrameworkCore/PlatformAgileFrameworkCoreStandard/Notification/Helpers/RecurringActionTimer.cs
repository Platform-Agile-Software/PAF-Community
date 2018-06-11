using System;
using System.Threading.Tasks;

namespace PlatformAgileFramework.Notification.Helpers
{

	/// <summary>
	/// Provides a container for a recurring action. This implementation uses
	/// a long-running task, since this is presumed to be a recurring action,
	/// whose operation would overload the thread pool. In order to begin
	/// execution, it is simply necessary to set the timing
	/// </summary>
	/// <threadsafety>Safe.</threadsafety>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 26dec2017 </date>
	/// <description>
	/// New. See <see cref="IRecurringActionTimer"/> 
	/// </description>
	/// </contribution>
	/// </history>
	/// <remarks>
	/// <para>
	/// This class currently only works reliably on .Net 4.*. There is a known
	/// problem in .Net standard 2.0 TPL that causes tasks to be destroyed
	/// early.  
	/// </para>
	/// <para>
	/// As one can imagine, this class requires extensive concurrent testing.
	/// This is the reason for the accessibility of some of the internal members.
	/// </para>
	/// </remarks>
	public class RecurringActionTimer : IRecurringActionTimer
	{
		// We create a task with a continuation which MUST be an
		// independent async task to allow disposal of its antecedent.
		 public const TaskContinuationOptions DISPOSAL_TASK_CONTINUATION_OPTIONS
			= TaskContinuationOptions.RunContinuationsAsynchronously
			  & TaskContinuationOptions.DenyChildAttach;
		/// <summary>
		/// We hold a long-running task.
		/// </summary>
		protected internal Task m_ActionLongRunningTask;
		/// <summary>
		/// The interval that controls the behavior of the recurrance.
		/// See <see cref="RecurringActionWrapperInternal"/>.
		/// </summary>
		protected internal volatile int m_ActionIntervalInMilliseconds;
		/// <summary>
		/// The action delegate.
		/// </summary>
		protected internal readonly Action m_ActionToPerform;
		/// <summary>
		/// To ensure that a timing problem doesn't stop the one-shot
		/// being executed.
		/// </summary>
		/// <remarks>
		/// This is a flag that is needed to ensure a very specific sort of behavior
		/// from the implementation that can't easily be implemented with TPL functions.
		/// </remarks>
		/////protected internal volatile bool m_HasOneShotExecuted;
		/// <summary>
		/// Flag tells us when the main task is being disposed and we
		/// should create a new one.
		/// </summary>
		protected internal volatile bool m_CurrentLongRunningTaskIsDisposing;
		/// <summary>
		/// To ensure that a timing problem doesn't stop the one-shot
		/// being executed.
		/// </summary>
		/// <remarks>
		/// This is a flag that is needed to ensure a very specific sort of behavior
		/// from the implementation that can't easily be implemented with TPL functions.
		/// </remarks>
		protected internal volatile bool m_ShouldExecuteOneShot;
		/// <summary>
		/// For test fixtures.
		/// </summary>
		protected internal volatile bool m_IsRunning;

		/// <summary>
		/// This is the default delay for the disposal loop to wait
		/// until a currently running task completes. It is the
		/// responsibility of the attaching class to ensure a
		/// proper timeout of their method. Otherwise, the dispose
		/// method in this class will be in an infinite loop. This
		/// is NOT the job of this class.
		/// </summary>
		public const int DISPOSAL_LOOP_DELAY = 50;
		#region Constructors
		/// <summary>
		/// We wrap the supplied delegate that we have to subscribe to. We also
		/// wrap the desired action delegate that gets run.
		/// </summary>
		/// <param name="actionToPerform">
		/// Action to perform, typically on a recurring basis. We provide NO exception
		/// service for this delegate. The implementor of this delegate MUST wrap the
		/// executable code in a try/catch block and do their own exception
		/// handling as is the general style for all good async programming in our
		/// world. 
		/// </param>
		public RecurringActionTimer(Action actionToPerform)
		{
			m_ActionToPerform = actionToPerform;
		}
		#endregion // Constructors
		#region Methods
		/// <summary>
		/// This is the protected version for subclassing.
		/// </summary>
		protected virtual int RecurringActionWrapper()
		{
			return RecurringActionWrapperInternal();
		}
		/// <summary>
		/// This is the internal version for exposing to test fixtures.
		/// This method runs the specified action <see cref="m_ActionToPerform"/>
		/// over and over again, on the <see cref="m_ActionLongRunningTask"/>
		/// until the <see cref="m_ActionIntervalInMilliseconds"/> is zero.
		/// At that time, the <see cref="m_ActionLongRunningTask"/> will
		/// enter an inactive state. This method, or it's protected version
		/// should never be called directly, except for testing purposes.
		/// </summary>
		/// <remarks>
		/// Because of the design of this method, recurring actions are
		/// always performed serially - it's never possible that another
		/// action will be taken while one is in progress. Neither
		/// are actions put into a queue. If an action is in progress,
		/// concurrent timer expirys are ignored.
		/// </remarks>
		internal virtual int RecurringActionWrapperInternal()
		{

			// Copy onto the stack so we don't react to concurrent changes.
			int currentStateOfRecurranceTime;
			while ((currentStateOfRecurranceTime = m_ActionIntervalInMilliseconds) > 0)
			{
				m_IsRunning = true;

				if (currentStateOfRecurranceTime == int.MinValue)
					break;

				var timeStart = DateTime.Now;

				ActionInvoker();

				var timeFinished = DateTime.Now;

				var timeItTookInMilliseconds = (timeFinished - timeStart).Milliseconds;
				var possibleTimeToDelayIfPositive
					= currentStateOfRecurranceTime - timeItTookInMilliseconds;

				// Delay the appropriate time if required.
				// NOTE: JMY Task is null sometimes and it should not be.
				if (possibleTimeToDelayIfPositive > 0)
					m_ActionLongRunningTask?.Wait(possibleTimeToDelayIfPositive);
					//m_ActionLongRunningTask.Wait(possibleTimeToDelayIfPositive);
			}

			// This is the one shot case. This is placed at the end of the run loop
			// in case a request for the one-shot comes in during the timer being
			// active or as an isolated one-time call.
			if (m_ShouldExecuteOneShot)
			{
				// If we are here, this means that a one-shot was requested and
				// we need to do it.
				m_ActionIntervalInMilliseconds = 0;
				m_ShouldExecuteOneShot = false;
				ActionInvoker();
			}

			m_IsRunning = false;

			// Start the disposal task.
			// NOTE: JMY Task is null sometimes and it should not be.
			//m_ActionLongRunningTask.ContinueWith(TaskContinuationMethod,
				m_ActionLongRunningTask?.ContinueWith(TaskContinuationMethod,
				DISPOSAL_TASK_CONTINUATION_OPTIONS);
			return 0;
		}

		/// <summary>
		/// Extensibility point for Goshaloma.
		/// </summary>
		/// <param name="currentDelayTime">
		/// Should be passed the current delay time. Not used in base method.
		/// </param>
		/// <returns>
		/// The new delay time in milliseconds. Base method just returns
		/// current.
		/// </returns>
		protected virtual int GenerateNewDelayTime(int currentDelayTime)
		{
			return m_ActionIntervalInMilliseconds;
		}

		/// <summary>
		/// Separate out the invoker for testing.
		/// </summary>
		protected virtual void ActionInvoker()
		{
			m_ActionToPerform.Invoke();
		}
		/// <summary>
		/// This method will reset the internal recurrance interval. A "0" will
		/// cause the cessation of the action recurrance, if it was ever active.
		/// If the task is running it will not interrupt it, but wait until it
		/// is finished. A value of -1 will cause the action to be performed
		/// exactly one time, after possibly waiting for an active run to finish.
		/// Any positive value will reset the occurrance time, and start execution
		/// on the action, if it was not running already. If already running,
		/// it will run on the new schedule in the future, but the setting will
		/// not trigger an immediate run.
		/// </summary>
		/// <param name="actionIntervalInMilliseconds">
		/// The interval. <see cref="int.MinValue"/> is a signal value
		/// that stops the schedule and exits after the current invocation (if any)
		/// is finished.
		/// </param>
		public virtual void SetRecurranceTime(int actionIntervalInMilliseconds)
		{
			if (actionIntervalInMilliseconds == -1)
				m_ShouldExecuteOneShot = true;
			else
				m_ActionIntervalInMilliseconds = actionIntervalInMilliseconds;

			// Instant stop.
			if (actionIntervalInMilliseconds == int.MinValue)
				return;

			// Die or stay dead if 0......
			if (actionIntervalInMilliseconds == 0) return;

			// If the task is still active, it must have picked up the new setting by now.
			if ((m_ActionLongRunningTask == null) || (m_CurrentLongRunningTaskIsDisposing))
			{
				// Otherwise we need a new one.
				StartTask();
			}
		}

		protected virtual void StartTask()
		{
			m_ActionLongRunningTask = Task.Factory.StartNew(RecurringActionWrapper,
				TaskCreationOptions.LongRunning);
		}

		#region IDisposable Implementation with Helpers
		/// <summary>
		/// Separate method for unmanaged.
		/// </summary>
		protected virtual void DisposeUnmanagedResources()
		{
			// release unmanaged resources here in derived classes.
		}

		/// <summary>
		/// This is the method that's called when the main task finishes
		/// its processing loop and exits. If a request for a final "one-shot"
		/// has been made, the main task is started for one more
		/// invocation.
		/// </summary>
		/// <param name="mainTask">
		/// Not used. We have to create a very specific behavior and it's not
		/// well-supported by the machinery on Tasks.
		/// </param>
		protected virtual void TaskContinuationMethod(Task mainTask)
		{
			WaitUntilMainTaskIsCompletedThenDisposeAndNullIt();
			if(m_ShouldExecuteOneShot)
				StartTask();
		}

		protected virtual void WaitUntilMainTaskIsCompletedThenDisposeAndNullIt()
		{
			// Get it on the stack so it's not possibly nulled
			// out from under us by another thread....
			var actionLongRunningTask = m_ActionLongRunningTask;

			if (actionLongRunningTask == null)
				return;

			m_CurrentLongRunningTaskIsDisposing = true;

			// This loop is needed because tasks don't end before
			// child tasks are fired.
			while (!actionLongRunningTask.IsCompleted)
			{
				Task.Delay(DISPOSAL_LOOP_DELAY).Wait();
			}
			actionLongRunningTask.Dispose();
			m_ActionLongRunningTask = null;
			m_CurrentLongRunningTaskIsDisposing = false;

		}

		/// <remarks>
		/// Standard pattern.
		/// </remarks>
		protected virtual void Dispose(bool disposing)
		{
			DisposeUnmanagedResources();
			if (disposing)
			{
				// Shut the timer off.
				SetRecurranceTime(int.MinValue);
				WaitUntilMainTaskIsCompletedThenDisposeAndNullIt();
			}
		}

		/// <remarks>
		/// Standard pattern.
		/// </remarks>
		public void Dispose()
		{
			Dispose(true);
			// This MUST always be here for subclasses that may implement a finalizer.
			GC.SuppressFinalize(this);
		}
		#endregion //IDisposable Implementation with Helpers
		#endregion // Methods
	}
}