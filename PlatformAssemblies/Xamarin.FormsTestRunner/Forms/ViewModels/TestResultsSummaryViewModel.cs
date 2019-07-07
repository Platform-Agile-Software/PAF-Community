using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using PlatformAgileFramework.AssemblyHandling;
using PlatformAgileFramework.FrameworkServices.ErrorAndException.Tests;
using PlatformAgileFramework.Notification.AbstractViewControllers;
using PlatformAgileFramework.Notification.Helpers;
using PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator;
using Xamarin.Forms;
using Xamarin.FormsTestRunner.Models;
using Xamarin.FormsTestRunner.Services;

namespace Xamarin.FormsTestRunner.ViewModels
{
	/// <summary>
	/// This is the "main" view model for displaying "summary" information
	/// about a node in an xUnit emulator test tree.
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
	//public class TestResultsSummaryViewModel : BaseViewModel
	public class TestResultsSummaryViewModel : AsyncProgressChangedNotificationBase, ITitledElement
	{
		// Temp for testing.
		public IDataStore<ITestResultSummary> DataStore => DependencyService.Get<IDataStore<ITestResultSummary>>() ?? new MockDataStore();

		#region Fields and Autoproperties
		/// <summary>
		/// This is the title before any tests are loaded.
		/// </summary>
		public static string s_DefaultTitle = "Tests";
		/// <summary>
		/// Backing.
		/// </summary>
		private IPAFTestElementInfo m_CurrentTestNode;
		/// <summary>
		/// Backing.
		/// </summary>
		private string m_Title;
		/// <summary>
		/// This is the root of the test result tree. It is
		/// <see langword="null"/> until LoadTestTree is called
		/// unless we have pushed in the root from the constructor.
		/// </summary>
		protected static IPAFTestElementInfo TestRootNode { get; set; }
		/// <summary>
		/// This is the collection of data items that item views are bound to.
		/// </summary>
		public ObservableCollection<ITestResultSummary> TestResultSummaries { get; protected set; }

		private bool m_IsBusy;

		#endregion Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// This constructor optionally pushes in a test element. If the
		/// <see cref="TestRootNode"/> is not set, it sets it. If the root
		/// is already set, the incoming element is assumed to point to
		/// an arbitrary node in the tree.
		/// </summary>
		/// <param name="testElementInfo">
		/// Some node in the test tree.
		/// </param>
		/// <remarks>
		/// "Title" is set to "Tests" if no element provided.
		/// </remarks>
		public TestResultsSummaryViewModel(IPAFTestElementInfo testElementInfo = null)
		{
			Title = s_DefaultTitle;
			TestResultSummaries = new ObservableCollection<ITestResultSummary>();
			LoadTestBranch = new Command(async () => await ExecuteLoadTestBranchCommand());

			MessagingCenter.Subscribe<NewItemPage, ITestResultSummary>
			(this, "AddItem", async (obj, item) =>
			{
				var addedItem = item;
				TestResultSummaries.Add(addedItem);
				await DataStore.AddItemAsync(addedItem);
			});
			m_CurrentTestNode = testElementInfo;
		}
		#endregion // Constructors
		#region Properties

		// TODO: KRM this was just completly broken since material
		// design in Android and doesn't work in iOS, either.  We still
		// need a solution.
		/// <summary>
		/// PAF-speak to Xamarin.Forms-speak.
		/// </summary>
		public bool IsBusy
		{
			get { return Processing; }
			set
			{
				// This will raise isBusy and return to avoid infinite loop.
				if(!PropertyChangedStore.NotifyOrRaiseIfPropertyChanged(ref m_IsBusy, value))
					return;
				// This will raise Processing IFF change in isBusy.
				Processing = value;
			}
		}

		/// <summary>
		/// Override couples Processing and IsBusy.
		/// </summary>
		public override bool Processing
		{
			get { return base.Processing; }
			set
			{
				base.Processing = value;
				IsBusy = value;
			}
		}

		/// <summary>
		/// This property indicates that a node is a leaf node.
		/// </summary>
		public bool IsLeaf
		{
			get { return !CurrentTestNode.GetDisplayChildElements(CurrentTestNode).Any(); }
		}
		/// <summary>
		/// This is the current node in the test result tree. It is
		/// <see langword="null"/> until LoadTestTree is called at
		/// the root node. It will set a title as the name of the
		/// element if it is not <see langword="null"/>.
		/// </summary>
		public virtual IPAFTestElementInfo CurrentTestNode
		{
			get { return m_CurrentTestNode; }
			set
			{
				m_CurrentTestNode = value;
				if (m_CurrentTestNode == null)
					Title = s_DefaultTitle;
				else
					Title = m_CurrentTestNode.TestElementResultInfo.ElementTypeTag;
			}
		}
		#region ITitledElement Implementation
		/// <summary>
		/// See <see cref="ITitledElement"/>.
		/// </summary>
		public string Title
		{
			get { return m_Title; }
			set
			{
				PropertyChangedStore.NotifyOrRaiseIfPropertyChanged(ref m_Title, value);
			}
		}
		#endregion // ITitledElement Implementation

		/// <summary>
		/// This command changes meaning depending on which node we are on
		/// in the test tree. Specifically, it loads the entire set of tests if
		/// <see cref="CurrentTestNode"/> is <see langword="null"/>.
		/// </summary>
		public ICommand LoadTestBranch { get; protected set; }
		#endregion // Properties


		public async Task<IList<IPAFTestElementInfo>> ExecuteLoadTestBranchCommand()
		{
			var tcs = new TaskCompletionSource<IList<IPAFTestElementInfo>>();
			if (Processing)
			{
				tcs.SetCanceled();
				return tcs.Task.Result;
			}

			Processing = true;
			Task.Delay(5).Wait();

			IList<IPAFTestElementInfo> returnValue = null;
			try
			{
				if (CurrentTestNode == null)
					CurrentTestNode = await Task.Run((Func<IPAFTestElementInfo>)ExecuteLoadTestTreeCommand);
				returnValue = CurrentTestNode.GetElementsToDisplay();
				// Just for testing, we put this here.
				LoadTestResultsIntoViewModel(returnValue);
			}
			catch (Exception ex)
			{
				tcs.SetException(ex);
			}
			finally
			{
				Processing = false;
			}
			tcs.TrySetResult(returnValue);
			return tcs.Task.Result;
		}

		private IPAFTestElementInfo ExecuteLoadTestTreeCommand()
		{
			#region Temporary Test Section
			////////////////////////////////////////////////////////////////
			// This section LOADS the test tree, without executing any tests.
			////////////////////////////////////////////////////////////////
			// Get our test assembly.
			var assembly = typeof(BasicExceptionTests).Assembly;

			// Install a filter for ALL assembly infos to use.
			// In core, we have a one-to-one correspondance between fixtures
			// and wrappers.
			PAFTestAssemblyInfo.DefaultGetDisplayChildElementItems
				= PAFTestElementInfoExtensions.GetUntypedChildInfosOfType<IPAFTestElementInfo<IPAFTestAssemblyInfo>, IPAFTestFixtureInfo>;
			////////////////////////////////////////////////////////////////////////////
			///// To run tests in an assembly.
			var assemblyInfo = new PAFTestAssemblyInfo(assembly.ToAssemblyholder(), null,
				PAFTestFrameworkBehavior.GetStandardNUnitParams());
			assemblyInfo.InitializeExePipeline(null);
			#endregion // Temporary Test Section

			TestRootNode = assemblyInfo;
			CurrentTestNode = TestRootNode;
			assemblyInfo.RunPipelinedObject(null);
			//var testConsoleUI = new PAFTestResultUserInteraction(assemblyInfo.TestElementResultInfo);
			//testConsoleUI.ProcessCommand("OR");
			return TestRootNode;
		}
		private void LoadTestResultsIntoViewModel(IList<IPAFTestElementInfo> testInfos)
		{
			TestResultSummaries.Clear();
			foreach (var info in testInfos)
			{
				TestResultSummaries.Add
					(new TestResultSummary(info));
			}
		}
		private async Task ExecuteLoadDataCommand()
		{
			if (Processing)
				return;

			Processing = true;

			try
			{
				TestResultSummaries.Clear();
				var items = await DataStore.GetItemsAsync(true);
				foreach (var item in items)
				{
					TestResultSummaries.Add(item);
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
			finally
			{
				Processing = false;
			}
		}
	}
}
