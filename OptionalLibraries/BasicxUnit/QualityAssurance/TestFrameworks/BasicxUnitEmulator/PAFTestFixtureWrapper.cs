//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2017 Icucom Corporation
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System;
using PlatformAgileFramework.Collections.Enumerators;
using PlatformAgileFramework.Collections.ExtensionMethods;
using PlatformAgileFramework.Execution.Pipeline;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.MultiProcessing.AsyncControl;
using PlatformAgileFramework.MultiProcessing.Threading;
using PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.TestEnumerableProviders;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator
{
	/// <summary>
	/// This class provides the ability to call into various tests designed to run within
	/// different unit test frameworks. Currently Nunit and MSTest are
	/// supported. The class offers the ability to install a custom
	/// enumerator that allows an arbitrary enumeration over the test methods.
	/// </summary>
	/// <threadsafety>
	/// This class is NOT thread-safe.
	/// </threadsafety>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 13dec2017 </date>
	/// <description>
	/// More consolidation and type-safety work. Derive now from the CLOSED Generic
	/// <see cref="IPAFTestElementInfo{IPAFTestWrapperInfo}"/> and
	/// <see cref="IPAFResettableEnumerableProvider{IPAFTestMethodInfo}"/>
	/// to get a simple, type-safe enumerator for methods.
	/// extension methods. Fixed the problem of the fixture not inheriting
	/// the method enumerable from the fixture if it did not have it's own
	/// installed. It was always intended to have an OVERRIDE capability
	/// from the fixture. Now it works that way.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> BMC </author>
	/// <date> 07aug2012 </date>
	/// <description>
	/// Added a <see cref="IAsyncControlObject"/> to the wrapper to allow
	/// direct containment of a CO to be passed into the test method run loop.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 01aug2012 </date>
	/// <description>
	/// Separated from the fixture tree for BasicxUnit according to new interface.
	/// Cleaned up DOCs. Moved test inclusion, exclusion lists to extended.
	/// </description>
	/// </contribution>
	/// </history>
	public class PAFTestFixtureWrapper : PAFTestElementInfo<IPAFTestFixtureWrapper>,
		IPAFTestFixtureWrapper
	{

		#region Class Fields and AutoProperties
		/// <summary>
		/// Protected backing for overriders that know what they are doing.
		/// </summary>
		protected IPAFResettableEnumerableProvider<IPAFTestMethodInfo> m_MethodEnumerableProvider;

		/// <summary>
		/// Backing...
		/// </summary>
		protected Action<IPAFTestFixtureWrapper, object> m_TestRunnerdelegate;
		/// <summary>
		/// See <see cref="IPAFTestFixtureWrapper"/>
		/// </summary>
		public IPAFTestFixtureInfo FixtureInfo { get; protected set; }

		/// <summary>
		/// <see cref="IPAFTestFixtureWrapper"/>.
		/// </summary>
		public PAFTestRunner TestRunner { get; protected internal set; }
		/// <summary>
		/// <see cref="IAsyncControlObject"/>. This may be left <see langword="null"/>. In this
		/// case, no stopping criteria from the CO is used. The ordinary loop control
		/// is employed. If this property is present, its state is used to make a
		/// decision about stopping. If
		/// <see cref="IAsyncControlObject.ProcessShouldTerminate"/> is <see langword="true"/>,
		/// the test method run loop in methods like <see cref="TestRunnerDelegateMethods.SimpleTestRunner"/>
		/// will stop running test methods.
		/// </summary>
		protected internal IAsyncControlObject AsyncTestControlObject { get; set; }
		/// <summary>
		/// <see cref="IInfoAndControlProvider"/>. Allows us to aggregate fixture info
		/// and control.
		/// </summary>
		protected internal IInfoAndControlProvider InfoAndControlProvider { get; set; }
		#endregion // Class Fields and AutoProperties
		#region Constructors
		/// <summary>
		/// Constructor sets up from an <see cref="IPAFTestFixtureInfo"/>. This can
		/// be a valid test fixture when examined for the attributes required for
		/// the test framework in use. Otherwise the initialization methods
		/// in this class will initialize the fixture.
		/// </summary>
		/// <param name="testFixtureInfo">
		/// See <see cref="IPAFTestFixtureInfo"/>.
		/// This provides all the needed parameters for the wrapper.
		/// </param>
		/// <param name="parent">
		/// Test controller, if in use.
		/// </param>
		/// <param name="name">
		/// If <see langword="null"/> "_Wrapper" is prepended to the fixture name.
		/// </param>
		/// <exceptions>
		/// <exception>
		/// <see cref="ArgumentNullException"/> is thrown if the incoming
		/// <paramref name="testFixtureInfo"/> is <see langword="null"/>.
		/// "testFixtureInfo".
		/// </exception>
		/// <see cref="PAFTestElementInfoExtensions.EnsureTestElementName"/> is
		/// called to verify that a proper name is found on the the incoming
		/// <paramref name="testFixtureInfo"/> if not passed in. We can't run without it.
		/// </exceptions>
		public PAFTestFixtureWrapper(IPAFTestFixtureInfo testFixtureInfo,
			IPAFTestElementInfo parent = null, string name = null)
			: base(name ?? testFixtureInfo.EnsureTestElementName() + "_Wrapper", null, parent)

		{
		    FixtureInfo = testFixtureInfo ?? throw new ArgumentNullException(nameof(testFixtureInfo));
		}
		#endregion // Constructors
		#region Properties
		/// <summary>
		/// See <see cref="IPAFTestFixtureWrapper"/>
		/// </summary>
		public Action<IPAFTestFixtureWrapper, object> TestRunnerdelegate
		{
			get { return m_TestRunnerdelegate; }
			set
			{
				m_TestRunnerdelegate = value ?? throw new ArgumentNullException("TestRunnerDelegate");
			}
		}
		/// <summary>
		/// Just delegates to the SINGLE wrapped fixture.
		/// </summary>
		public override bool? Passed
		{
			get { return FixtureInfo.Passed; }
			set { }
		}


		#region Implementation of IPAFBaseExePipelineInitialize
 	    /// <summary>
		/// Helper to access converted parameters.
		/// </summary>
		public virtual IPAFTestFixtureWrapperParameters TFWPipelineParams { get; protected set; }
		#endregion // Implementation of IPAFBaseExePipelineInitialize
		#region Implementation of IParameterizedThreadStartProvider
		/// <summary>
		/// See <see cref="IParameterizedThreadStartProvider"/>.
		/// </summary>
		public virtual Action<object> ThreadExecutionDelegate
		{ get { return RunTestMethods; } }
		#endregion // Implementation of IParameterizedThreadStartProvider
		#endregion // Properties
		#region Methods
		/// <summary>
		/// <see cref="IPAFTestFixtureWrapper"/>. This one simply constructs the actual
		/// instance of the wrapped test fixture type. If <paramref name="reset"/> is
		/// <see langword="true"/>, it <see langword="null"/>s the wrapped instance first. If <paramref name="reset"/>
		/// is <see langword="false"/>, a fresh instance is not created if one alredy exists.
		/// </summary>
		/// <param name="reset"><see cref="IPAFTestFixtureWrapper"/>.</param>
		/// <exceptions>
		/// <exception> <see cref="ArgumentNullException"/> is thrown if
		/// either parameter is <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		protected virtual void Initialize(bool reset)
		{
			if (FixtureInfo.TestElementStatus != TestElementRunnabilityStatus.Active)
				return;
			Exception generatedException = null;
			try
			{
				// Reset kills any existing fixture.
				if (reset)
				{
					if (FixtureInfo.FixtureInstance != null)
					{
						// If fixture is disposable, dispose it.
						IDisposable disposable;
						if ((disposable = FixtureInfo.FixtureInstance as IDisposable) != null)
							disposable.Dispose();
						FixtureInfo.FixtureInstance = null;
					}
				}

				// Reset the enumerable for another run.
				this.GetTestElementInfoResettableEnumerableProviderProvider().EnumerableProvider
					.SetData(FixtureInfo.GetActiveTestMethods());

				// Build fixture if none exists.
				Exception ex = null;
				if (FixtureInfo.FixtureInstance == null)
					ex = FixtureInfo.PAFConstructTestFixtureInstance();
				if (ex == null) return;

				FixtureInfo.AddTestException(ex);
				FixtureInfo.ExcludedReason = ex.Message;
				FixtureInfo.TestElementStatus = TestElementRunnabilityStatus.ExcludedByErrors;
			}
			catch (Exception ex)
			{
				generatedException = ex;
			}
			finally
			{
				if (generatedException != null)
				{
					// TODO wrap in something more specific - some new wrapper exceptions?
					// NOTE: KRM - good idea. I put them on the fixture directly - still
					// need fixture exceptions.
					FixtureInfo.AddTestException(generatedException);
					AddTestException(generatedException);
					if (!string.IsNullOrEmpty(generatedException.Message))
						FixtureInfo.ExcludedReason = generatedException.Message;
					else
						FixtureInfo.ExcludedReason
							= "Unknown exception, Type = " + generatedException.GetType();
					FixtureInfo.TestElementStatus = TestElementRunnabilityStatus.ExcludedByErrors;
				}
			}
			// We are initialized, although perhaps with errors.
		}
		/// <summary>
		/// <see cref="IPAFTestFixtureWrapper"/>. This is internal guts
		/// that is used by default as a test runner. It uses the plugin if it is there.
		/// </summary>
		/// <param name="obj">
		/// <see cref="IPAFTestFixtureWrapper"/>. Unused in this implementation.
		/// </param>
		/// <remarks>
		/// This method will initialize the class if it is not already.
		/// </remarks>
		/// <remarks>
		/// This base version sets any exceptions received from the runner on the
		/// <see cref="FixtureInfo"/> and invalidates it. If a <see cref="TestRunnerdelegate"/>
		/// is supplied, it will do things it's own way.
		/// </remarks>
		protected virtual void RunTestMethods(object obj)
		{
			// User pushed in a delegate?
			if (TestRunnerdelegate != null)
			{
				TestRunnerdelegate(this, obj);
				return;
			}

			// Nope, just the synchronous way.

			if (FixtureInfo.TestElementStatus != TestElementRunnabilityStatus.Active) return;
			Initialize(false);
			if (FixtureInfo.TestElementStatus != TestElementRunnabilityStatus.Active) return;

			FixtureInfo.SetTestFixtureRunning(true);
			var ex = TestRunner(InfoAndControlProvider);
			FixtureInfo.SetTestFixtureRunning(false);
	
			if (ex != null)
			{
				// TODO wrap in something more specific - some new wrapper exceptions?
				FixtureInfo.AddTestException(ex);
				if (!string.IsNullOrEmpty(ex.Message))
					FixtureInfo.ExcludedReason = ex.Message;
				else
					FixtureInfo.ExcludedReason = "Unknown exception, Type = " + ex.GetType();

				FixtureInfo.TestElementStatus = TestElementRunnabilityStatus.ExcludedByErrors;
			}
		}
		#endregion // Methods
		#region Implementation of IPAFBaseExePipelineInitialize
		/// <summary>
		/// This implementation digests parameters and prepares the wrapper. This method
		/// should typically be called in an overall initialization phase where validation
		/// is performed. The incoming parameters are converted to their corresponding
		/// properties on this class.
		/// <para>
		/// The initialization is performed with the following steps.
		/// <list type="number">
		/// <item>
		/// <term><see cref="IPAFBaseExePipeline{T}.IsExePipelineInitialized"/></term>
		/// <description>
		/// If this is true (indicating that this method has been called), the method
		/// returns without running again.
		/// </description>
		/// </item>
		/// <item>
		/// <term><see cref="TestRunner"/></term>
		/// <description>
		/// If property is <see langword="null"/>, <see cref="TestRunnerDelegateMethods.SimpleTestRunner"/>
		/// is installed.
		/// </description>
		/// </item>
		/// <item>
		/// <term><see cref="IPAFEnumerableProviderProvider{IPAFTestMethodInfo}.EnumerableProvider"/></term>
		/// <description>
		/// Loaded from <see cref="IPAFEnumerableProviderProvider{IPAFTestMethodInfo}.EnumerableProvider"/>
		/// on <see cref="FixtureInfo"/> if <see cref="ITestElementInfoItemEnumerableProviderProvider{T}"/>
		/// on this class is <see langword="null"/>
		/// class property is <see langword="null"/>. If still <see langword="null"/>,
		/// <see cref="AlphabetizingTestEnumerableProvider{IPAFTestMethodInfo}"/>
		/// is installed. This provides the default behavior for a standard NUnit run on a fixture.
		/// </description>
		/// </item>
		/// <item>
		/// <term><see cref="InfoAndControlProvider"/></term>
		/// <description>
		/// Created from <see cref="FixtureInfo"/> and <see cref="AsyncTestControlObject"/>. The
		/// latter may be <see langword="null"/>
		/// </description>
		/// </item>
		/// <item>
		/// <term><see cref="IPAFBaseExePipeline{T}. IsExePipelineInitialized"/></term>
		/// <description> 
		/// Set to <see langword="true"/> to complete the initialization process.
		/// </description>
		/// </item>
		/// </list>
		/// </para>
		/// </summary>
		public override void InitializeExePipeline(IPAFProviderPattern<IPAFPipelineParams<IPAFServiceManager<IPAFService>>> provider)
		{
			if (IsExePipelineInitialized) return;

			// Call base, since we may want to provide customized services
			// to children.
			base.InitializeExePipeline(provider);

			// Make sure our fixture is initialized.
			FixtureInfo.InitializeExePipeline(this);

			if (FixtureInfo.TestElementStatus != TestElementRunnabilityStatus.Active) return;

			if (TestRunner == null)
				TestRunner = TestRunnerDelegateMethods.SimpleTestRunner;

			// If we don't have an enumerator loaded, pull it off the fixture.
			if (this.GetTestElementInfoResettableEnumerableProviderProvider().EnumerableProvider == null)
			{
				IPAFEnumerableProvider<IPAFTestMethodInfo> enumerableProvider;
				// We need a resettable provider for our tests.
			    if (FixtureInfo.GetTestElementInfoEnumerableProviderProvider<IPAFTestMethodInfo>()?.EnumerableProvider is ITestElementInfoItemResettableEnumerableProvider<IPAFTestMethodInfo> resettableEnumerableProvider)
					SetResettableEnumerableProvider(resettableEnumerableProvider);

				else if ((enumerableProvider = FixtureInfo.GetTestElementInfoEnumerableProviderProvider<IPAFTestMethodInfo>()?.EnumerableProvider) != null)
			    {
					// It's not resettable, so wrap it.
					SetResettableEnumerableProvider(
						new ResettableTestElementInfoEnumerableProvider<IPAFTestMethodInfo>(enumerableProvider.GetEnumerable()));
			    }
			}

			if (this.GetTestElementInfoResettableEnumerableProviderProvider().EnumerableProvider == null)
			{
				// At this point, we have found nothing on the fixture, so use the alphabetizing default.
				SetResettableEnumerableProvider(new AlphabetizingTestEnumerableProvider<IPAFTestMethodInfo>
					(FixtureInfo.GetActiveTestMethods()));
			}

			// Now that we have the providers straightened away, we can initialize
			// the rest of the way.
			Initialize(true);

			// Aggregate the fixture info and the CO.
			InfoAndControlProvider
				= new InfoAndControlProvider(this, AsyncTestControlObject);

			IsExePipelineInitialized = true;
		}
		#endregion // Implementation of IPAFBaseExePipelineInitialize
		#region Implementation of IPAFBaseExePipeline
		/// <summary>
		/// See <see cref="IPAFBaseExePipeline{T}"/>. We add the wrapper to the
		/// fixture's collection, then initialize ourselves, then run the methods.
		/// </summary>
		/// <param name="obj">See interface.</param>
		public override void RunPipelinedObject(object obj)
		{
			FixtureInfo.TestFixtureWrappers.AddNoDupes(this);
			InitializeExePipeline(this);
			RunTestMethods(obj);
		}
		#endregion // Implementation of IPAFBaseExePipeline
		#region Implementation of IPAFResettableEnumerableProviderProvider<IPAFTestMethodInfo>
		/// <summary>
		/// <see cref="IPAFResettableEnumerableProviderProvider{IPAFTestMethodInfo}"/>.
		/// This has to explicit for interface prop conflicts.
		/// </summary>
		IPAFResettableEnumerableProvider<IPAFTestMethodInfo> IPAFResettableEnumerableProviderProvider<IPAFTestMethodInfo>.EnumerableProvider
		{
			get { return m_MethodEnumerableProvider; }
		}

		/// <summary>
		/// <see cref="IPAFResettableEnumerableProviderProvider{IPAFTestMethodInfo}"/>.
		/// </summary>
		public void SetResettableEnumerableProvider(ITestElementInfoItemResettableEnumerableProvider<IPAFTestMethodInfo> resettableEnumerableProvider)
		{
			m_MethodEnumerableProvider = resettableEnumerableProvider;
		}
		#endregion // Implementation of IPAFResettableEnumerableProviderProvider<IPAFTestMethodInfo>
	}
}
