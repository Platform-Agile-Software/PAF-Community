namespace PlatformAgileFramework.Notification.AbstractViewModels
{
	/// <summary>
	/// This is an interface that is typically worn by PAF
	/// veiw models and other classes that have asynchronous
	/// operations.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date>23jan2018 </date>
	/// <description>
	/// New. Refactoring for new view model structure.
	/// </description>
	/// </contribution>
	/// </history>
	public interface IAsyncViewModel
	{
		/// <summary>
		/// Tells if the component is busy doing some work.
		/// </summary>
		bool Processing { get; set; }
		/// <summary>
		/// Fraction 0.0 - 1.0 (inclusive) of processing inclusive.
		/// </summary>
		double FractionDone { get; set; }
	}
}