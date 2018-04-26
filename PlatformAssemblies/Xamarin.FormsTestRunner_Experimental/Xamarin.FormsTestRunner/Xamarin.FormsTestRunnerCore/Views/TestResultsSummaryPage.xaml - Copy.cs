using System;
using System.Threading.Tasks;
using PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator;
using PlatformAgileFramework.XamarinFormsHelpers;
using Xamarin.Forms;
using Xamarin.FormsTestRunner.Models;
using Xamarin.FormsTestRunner.ViewModels;

namespace Xamarin.FormsTestRunner.Views
{
	/// <summary>
	/// This is the main or summary page that is used to display generic
	/// information about each node in a tree. This page creates its
	/// own children (instances of itself) when the tree is navigated.
	/// </summary>
	public partial class TestResultsSummaryPage : ContentPage, ISwipeCallBack
	{
		#region Fields and Autoproperties

		public int DoubleClickTimeInMilliseconds { get; set; } = 500;
		/// <summary>
		/// View model - stapled in in the constructor.
		/// </summary>
		private readonly TestResultsSummaryViewModel m_ViewModel;

		/// <summary>
		/// To detect double-clicks on the items.
		/// </summary>
		//private readonly SelectionListener m_SelectionListener;
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Constructor can be passed a view model that is
		/// already loaded from a parent.
		/// </summary>
		/// <param name="viewModel">
		/// View model. <see langword="null"/> to load the tree.
		/// </param>
		public TestResultsSummaryPage(TestResultsSummaryViewModel viewModel = null)
		{
			InitializeComponent();
			// The root view model is responsible for loading the tree.
			m_ViewModel = viewModel ?? new TestResultsSummaryViewModel();

			BindingContext = m_ViewModel;
			Content.GestureRecognizers.Add(new SwipeListener(Content, this));

			//// Set up the selection listener so we can detect double clicks
			//// on the items.
			//m_SelectionListener
			//	= new SelectionListener(SelectionReceiver, DoubleClickTimeInMilliseconds);
		}
		#endregion // Constructors

		async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
		{
			var item = args.SelectedItem as ITestResultSummary;
			// Manually deselect item so we can select (click/tap) again.
			TestResultsSummaryListView.SelectedItem = null;

			if (item == null)
				return;
			// Can't expand a leaf.
			if (m_ViewModel.IsLeaf)
				return;
			await DownToChild(item);

			//Task<bool> isDoubleClick = m_SelectionListener.ReceiveClick(item);
		}

		///// <summary>
		///// This method is designed to receive either a selection
		///// or a "click" from multiple sources. It will go down in
		///// a tree by displaying the curent node's children if it
		///// is not a leaf node. It can also display details about
		///// a "current" node. Useful for receiving selections from
		///// multiple sorts of gestures on different views.
		///// </summary>
		///// <param name="isDetails">
		///// <see langword="true"/> to display details of a node, not
		///// expand it.
		///// </param>
		///// <param name="sender">
		///// Can expand if a <see cref="ITestResultSummary"/>.
		///// </param>
		//async Task SelectionReceiver(bool isDetails, object sender)
		//{
		//	var summary = sender as ITestResultSummary;
		//	if (isDetails)
		//	{
		//		if (summary != null)
		//		{
		//			// It's a list item.
		//			await PresentDetails(summary.TestElementInfo);
		//			return;
		//		}

		//		// It's us.
		//		await PresentDetails(m_ViewModel.CurrentTestNode);
		//		return;
		//	}

		//	if (sender is ITestResultSummary)
		//	{
		//		if (m_ViewModel.IsLeaf)
		//			// TODO - put some sort of error message or visual signal here?
		//			return;
		//		await DownToChild((ITestResultSummary) sender);
		//	}

		//}
		/// <summary>
		/// This brings up a detail page about the CURRENT node that is the parent
		/// of its children in the list. // IF it called by a toolbar item click.
		/// </summary>
		/// <param name="sender">Not used.</param>
		/// <param name="e">Not used.</param>
		async void Details_Clicked(object sender, EventArgs e)
		{
			//await Task.Run(() =>
			//{
			//	m_SelectionListener.ReceiveClick(null);
			//});
			await PresentDetails(m_ViewModel.CurrentTestNode);
		}
		/// <summary>
		/// This brings up a detail page about the CURRENT node that is the parent
		/// of its children in the list IF it called by a toolbar item click. // It
		/// is also called when a double-click is made on a list item. In that case,
		/// details of the list item are displayed.
		/// </summary>
		/// <param name="testElementInfo">
		/// This is the item we explore for its details.
		/// </param>
		async Task PresentDetails(IPAFTestElementInfo testElementInfo)
		{
			var detailViewModel
				= new TestResultDetailViewModel(testElementInfo);
			await Navigation.PushAsync(new TestResultDetailPage(detailViewModel));
		}

		async Task DownToChild(ITestResultSummary listItem)
		{
			var childViewModel = new TestResultsSummaryViewModel(listItem.TestElementInfo);

			childViewModel.CurrentTestNode = listItem.TestElementInfo;
			//await childViewModel.ExecuteLoadTestBranchCommand();
			await childViewModel.ExecuteLoadTestBranchCommand();

			// await Navigation.PushAsync(new TestResultsSummaryPage(childViewModel));
			await Navigation.PushAsync(new TestResultsSummaryPage(childViewModel));
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			if (m_ViewModel.TestResultSummaries.Count == 0)
				m_ViewModel.LoadTestBranch.Execute(null);
		}
		#region ISwipeCallback Implementation
		/// <summary>
		/// See <see cref="ISwipeCallBack"/>.
		/// </summary>
		/// <param name="view">
		/// See <see cref="ISwipeCallBack"/>.
		/// </param>
		/// <param name="direction">
		/// See <see cref="ISwipeCallBack"/>.
		/// </param>
		public virtual void OnSwipe(View view, SwipeDirection direction)
		{
			
		}

		#endregion // ISwipeCallback Implementation
	}
}
