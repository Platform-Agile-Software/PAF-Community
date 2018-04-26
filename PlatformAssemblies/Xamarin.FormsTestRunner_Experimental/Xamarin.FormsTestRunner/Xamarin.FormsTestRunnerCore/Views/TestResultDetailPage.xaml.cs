using Xamarin.Forms;
using Xamarin.FormsTestRunner.ViewModels;

namespace Xamarin.FormsTestRunner.Views
{
    public partial class TestResultDetailPage : ContentPage
    {
        TestResultDetailViewModel m_ViewModel;
        public TestResultDetailPage(TestResultDetailViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = m_ViewModel = viewModel;
        }
    }
}
