using System;

namespace PlatformAgileFramework.Notification.Helpers
{
	/// <summary>
	/// Provides a protocol for a recurring action. In order to begin
	/// execution, one normally must call <see cref="SetRecurranceTime"/>.
	/// This is typically attached to a class that needs an operation
	/// run on a schedule.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 26dec2017 </date>
	/// <description>
	/// New. Originally created as generic infrastructure to solve the
	/// problem with purge overhead on high-speed graphics in the notification
	/// infrastructure. Did not replicate the "timeout" from the old interface.
	/// This should not be this component's responsibilty. Put it in the attaching
	/// class's method. 
	/// </description>
	/// </contribution>
	/// </history>
	public interface IRecurringActionTimer: IDisposable
	{
		/// <summary>
		/// This resets the recurrance time and generally fires off
		/// the first recurring action. Typically a non-zero
		/// recurrance time will result in the action recurring
		/// forever. Set it to zero to stop AFTER any ongoing
		/// action has finished. This does NOT cancel an action.
		/// That would be the job of the attaching class.
		/// </summary>
		/// <param name="actionIntervalInMilliseconds">
		/// The interval.
		/// </param>
		void SetRecurranceTime(int actionIntervalInMilliseconds);
	}
}