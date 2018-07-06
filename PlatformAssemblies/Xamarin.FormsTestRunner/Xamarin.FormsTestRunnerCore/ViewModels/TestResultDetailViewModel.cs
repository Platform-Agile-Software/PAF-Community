using System.Linq;
using PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator;

namespace Xamarin.FormsTestRunner.ViewModels
{
    public class TestResultDetailViewModel
	    : BaseViewModel, ITestResultDetailViewModel
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Backing for the prop.
		/// </summary>
		protected IPAFTestElementInfo m_TestElement;
		#endregion Fields and Autoproperties

		#region Constructors
		/// <summary>
		/// Constructor builds with the optional backing.
		/// </summary>
		/// <param name="testElement">Test element node we are exploring.</param>
		public TestResultDetailViewModel(IPAFTestElementInfo testElement = null)
        {
            Title = testElement?.TestElementName;
            m_TestElement = testElement;
        }
		#endregion // Constructors

		#region Properties
		/// <summary>
		/// get/set the backing. Updates title.
		/// </summary>
		public virtual IPAFTestElementInfo TestElement
	    {
		    get { return m_TestElement; }
		    set
		    {
			    m_TestElement = value;
			    Title = m_TestElement?.TestElementName;
		    }
	    }

	    public virtual string TestFailureStatus
	    {
		    get
		    {
			    if (m_TestElement.Passed == null)
				    return "Not run";
			    if (m_TestElement.Passed.Value)
				    return "Passed";
			    return "Failed";
		    }
		    // ReSharper disable once ValueParameterNotUsed
		    set { ; }
	    }
	    public virtual string ExceptionType
	    {
		    get
		    {
			    if (!m_TestElement.Exceptions.Any())
				    return "None";
			    return m_TestElement.Exceptions.First().GetType().ToString();
		    }
		    // ReSharper disable once ValueParameterNotUsed
		    set { ; }
	    }

		#endregion // Properties
	}
}
