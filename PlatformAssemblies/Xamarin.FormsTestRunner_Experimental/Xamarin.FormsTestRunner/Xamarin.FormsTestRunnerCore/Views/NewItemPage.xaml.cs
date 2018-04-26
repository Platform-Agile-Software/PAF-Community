using System;

using Xamarin.Forms;
using Xamarin.FormsTestRunner.Models;

namespace Xamarin.FormsTestRunner
{
    public partial class NewItemPage : ContentPage
    {
        public ITestResultSummary Item { get; set; }

        public NewItemPage()
        {
            InitializeComponent();

            Item = new TestResultSummary
            {
                TestElementName = "Item name",
                Status = "This is an item description."
            };

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopToRootAsync();
        }
    }
}
