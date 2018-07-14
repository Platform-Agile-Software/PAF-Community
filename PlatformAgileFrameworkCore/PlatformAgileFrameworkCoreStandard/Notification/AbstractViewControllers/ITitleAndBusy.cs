namespace PlatformAgileFramework.Notification.AbstractViewControllers
{
	/// <summary>
	/// Interface to be worn by view controllers that need a title
	/// and an indication of "busyness".
	/// </summary>
	public interface ITitleAndBusy
	{
		/// <summary>
		/// Are we busy?
		/// </summary>
		bool IsBusy { get; set; }
		/// <summary>
		/// Title to appear somewhere on the view.
		/// </summary>
		string Title { get; set; }
	}
}