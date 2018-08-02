using PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator;
using PlatformAgileFramework.TypeHandling;

namespace Xamarin.FormsTestRunner.Models
{
	/// <summary>
	/// Basic component of view models. Binding to an
	/// interface gives much more flexibility in fetching
	/// and displsying data. Allows data to be easily
	/// broken up for token/full pattern. Wears the
	/// provider pattern, so everything is available.
	/// </summary>
	public interface ITestResultSummary:
		IPAFClassProviderPattern<IPAFTestElementInfo>
	{
		string Id { get; set; }
		string Status { get; set; }
		IPAFTestElementInfo TestElementInfo { get; set; }
		string TestElementName { get; set; }
	}
}