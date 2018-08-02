namespace Xamarin.FormsTestRunner.ViewModels
{
	/// <summary>
	/// Just the data that the view needs to bind to.
	/// </summary>
	public interface ITestResultDetailViewModel
	{
		/// <summary>
		/// The type of the exception that failed the test.
		/// </summary>
		string ExceptionType { get; set; }
		/// <summary>
		/// Tells if the test failed/passed or did not run.
		/// </summary>
		string TestFailureStatus { get; set; }
	}
}