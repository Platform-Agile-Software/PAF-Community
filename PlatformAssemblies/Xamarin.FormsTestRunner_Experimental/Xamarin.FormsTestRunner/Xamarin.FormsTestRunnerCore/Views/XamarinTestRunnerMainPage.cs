using Xamarin.Forms;

namespace Xamarin.FormsTestRunner.Views
{
	/// <summary>
	/// This is the main page that first emerges when the app is started. No
	/// XAML for this, just sets up things, depending which device...
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 15feb18 </date>
	/// <description>
	/// Porting the "xUnit emulator" to the mobiles.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Unsafe, but gets called only in app initialization path on the main thread.
	/// </threadsafety>
    public class XamarinTestRunnerMainPage : TabbedPage
    {
		/// <summary>
		/// The main page with test results.
		/// </summary>
	    private readonly Page m_TestResultsPage;
		/// <summary>
		/// About the app.
		/// </summary>
	    private readonly Page m_AboutPage;
        public XamarinTestRunnerMainPage()
        {
			switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    m_TestResultsPage = new NavigationPage(new TestResultsSummaryPage())
                    {
                        Title = "Test"
                    };

                    m_AboutPage = new NavigationPage(new AboutPage())
                    {
                        Title = "About"
                    };
                    m_TestResultsPage.Icon = "tab_feed.png";
                    m_AboutPage.Icon = "tab_about.png";
                    break;
                default:
                    m_TestResultsPage = new TestResultsSummaryPage()
                    {
                        Title = "Test"
                    };

                    m_AboutPage = new AboutPage()
                    {
                        Title = "About"
                    };
                    break;
            }

            Children.Add(m_TestResultsPage);
            Children.Add(m_AboutPage);

            Title = Children[0].Title;
        }

        protected override void OnCurrentPageChanged()
        {
            base.OnCurrentPageChanged();
            Title = CurrentPage?.Title ?? string.Empty;
        }
    }
}
