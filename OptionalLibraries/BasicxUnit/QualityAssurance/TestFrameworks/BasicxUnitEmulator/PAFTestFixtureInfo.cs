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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using PlatformAgileFramework.Collections.Enumerators;
using PlatformAgileFramework.Collections.ExtensionMethods;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Execution.Pipeline;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.MultiProcessing.Threading.NullableObjects;
using PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Exceptions;
using PlatformAgileFramework.TypeHandling;

using PlatformAgileFramework.TypeHandling.Disposal;
using PlatformAgileFramework.TypeHandling.MemberExtensionMethods;
using PlatformAgileFramework.TypeHandling.MethodHelpers;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;

// Exception shorthand.
using PAFAED = PlatformAgileFramework.ErrorAndException.PAFAggregateExceptionData;
using PAFTFIED = PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.PAFTestFixtureInitializationExceptionData;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator
{
    /// <summary>
    /// Holds static and dynamic information (e.g. the instance) for each fixture. The
    /// class employs the pattern of providing backing fields for properties so that
    /// properties can be virtual for adding synchronization in derived classes.
    /// </summary>
    /// <threadsafety>
    /// This class is designed to be thread-safe under proper usage. The members that are
    /// not synchronized are not expected to be changed once set at initialization time.
    /// If you plan on resetting them dynamically on multiple threads, you must synchronize
    /// them in a derived class.
    /// </threadsafety>
    /// <history>
    /// <contribution>
    /// <author> KRM </author>
    /// <date> 21aug2017 </date>
    /// <description>
    /// Killed the internal access on fields, since we no longer access the base class across
    /// <c>AppDomian</c>s. Made a test fixture with no test methods be
    /// a non-fixture. Fixtures can inherit from a base class, but it doesn't have to
    /// be attributed as a fixture, which was previously not allowed. Added history.
    /// </description>
    /// </contribution>
    /// <contribution>
    /// <author> KRM </author>
    /// <date> 01aug2012 </date>
    /// <description>
    /// Separated from the fixture tree for BasicxUnit according to new interface.
    /// Cleaned up DOCs. Derived directly from <see cref="PAFTestElementInfo{T}"/>.
    /// </description>
    /// </contribution>
    /// </history>
    public class PAFTestFixtureInfo
		: PAFTestElementInfo<IPAFTestFixtureInfo>, IPAFTestFixtureInfo
    {
        #region Class Fields and Autoproperties
        /// <summary>
        /// Backing.
        /// </summary>
        protected ConstructorInfo m_FixtureConstructor;
        /// <summary>
        /// Backing.
        /// </summary>
        protected object m_FixtureInstance;
        /// <summary>
        /// Backing.
        /// </summary>
        protected IPAFTestMethodInfo m_FixtureSetUpMethod;
        /// <summary>
        /// Backing.
        /// </summary>
        protected IPAFTestMethodInfo m_FixtureTearDownMethod;
        /// <summary>
        /// Backing.
        /// </summary>
        protected IPAFTypeHolder m_FixtureType;
        /// <summary>
        /// Backing.
        /// </summary>
        protected IPAFTestFrameworkBehavior m_FrameworkBehavior;
        /// <summary>
        /// Synchronized backing.
        /// </summary>
        protected NullableSynchronizedWrapper<bool> m_HasFixtureSetUpBeenCalled;
        /// <summary>
        /// Synchronized backing.
        /// </summary>
        protected NullableSynchronizedWrapper<bool> m_HasFixtureTearDownBeenCalled;
        /// <summary>
        /// Synchronized backing.
        /// </summary>
        protected NullableSynchronizedWrapper<long> m_MaxTimesAnyTestCalled;
        /// <summary>
        /// Synchronized backing.
        /// </summary>
        protected NullableSynchronizedWrapper<long> m_NumTimesAnyTestCalled;
        /// <summary>
        /// Backing.
        /// </summary>
        protected IList<IPAFTestFixtureWrapper> m_TestFixtureWrappers;
        /// <summary>
        /// Backing.
        /// </summary>
        protected IPAFEnumerableProvider<IPAFTestMethodInfo> m_TestMethodEnumerable;
        /// <summary>
        /// Backing.
        /// </summary>
        protected ICollection<IPAFTestMethodInfo> m_TestMethods;
        /// <summary>
        /// Backing.
        /// </summary>
        protected IPAFTestMethodInfo m_TestSetUpMethod;
        /// <summary>
        /// Backing.
        /// </summary>
        protected IPAFTestMethodInfo m_TestTearDownMethod;
        /// <summary>
        /// Pluggable checker. Leave <see langword="null"/> to use default checker
        /// <see cref="PAFTestFixtureInfoExtensions.CheckFixtureType"/>  
        /// </summary>
        public Func<IPAFTestFixtureInfo, IPAFTestFrameworkBehavior, Exception> m_CheckTestFixtureDelegate;
		/// <summary>
		/// Protected backing for extenders.
		/// </summary>
	    protected IPAFEnumerableProvider<IPAFTestMethodInfo> m_TestMethodEnumerableProvider;

	    #endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Builds with necessary parameters for later construction of all members.
		/// </summary>
		/// <param name="fixtureType">
		/// The <see cref="Type"/> that is proposed for use as a test fixture.
		/// The type must be usually be adorned with an attribute indicating
		/// that it is a test fixture and it must usually have a default constructor.
		/// The <see cref="PAFTypeHolderBase"/> containing the type will have
		/// its internal loader called to construct the actual type from its name,
		/// if it is not constructed already.
		/// </param>
		/// <param name="frameworkBehavior">
		/// Default causes PAFUnit behavior to be used.
		/// </param>
		/// <param name="parent">
		/// The parent, which is usually an <see cref="IPAFTestAssemblyInfo"/> in
		/// a standard xUnit configuration. This can be <see langword="null"/> for folks who
		/// want to plug in a single known test and run it.
		/// </param>
		/// <param name="name">
		/// Name to be given to this <see cref="IPAFTestElementInfo"/>. Defaults to
		/// the namespace-qualified Type name.
		/// </param>
		/// <remarks>
		/// <para>
		/// This class employs a surrogate disposer and is also registered
		/// with the <see cref="DisposalRegistry"/> to ensure our testing
		/// procedure does not mysteriously crash after a while if folks
		/// build something new out of our pieces and don't dispose things
		/// quite right. Tests may hold on to a potentially large number
		/// of unmanaged resources.
		/// </para>
		/// <para>
		/// The other major problem that occurs is that for test fixtures
		/// that are disposable, it is always advisable to ensure
		/// <see cref="IDisposable.Dispose"/> is called and that it executes
		/// without any errors. This is due to the fact that a failed
		/// disposal may result in a corrupt state of the entire environment.
		/// This obviously indicates a bad fixture design, but many legacy
		/// fixtures have flaws. If a test framework behavior is set to keep
		/// going in the presence of failures of individual tests, the
		/// information about the failure should be logged somehow for
		/// post-test analysis.
		/// </para>
		/// <para>
		/// There is disagreement in the testing field whether test fixtures
		/// should even dispose of resources through a <see cref="IDisposable"/>
		/// interface as opposed to a test fixture teardown method. We take no
		/// sides - we simply try to support what has already been written as
		/// gracefully as possible. We log disposal exceptions in the
		/// <see cref="DisposalRegistry"/>.
		/// </para>
		/// </remarks>
		/// <exceptions>
		/// <exception>
		/// <see cref="ArgumentNullException"/> is thrown if the incoming
		/// <paramref name ="fixtureType"/> is <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		public PAFTestFixtureInfo(IPAFTypeHolder fixtureType, IPAFTestFrameworkBehavior frameworkBehavior = null,
			IPAFTestElementInfo parent = null, string name = null)
			: base(name ?? fixtureType.NamespaceQualifiedTypeName, null, parent)
		{
			// Can't deal with no type.
			m_FixtureType = fixtureType ?? throw new ArgumentNullException(nameof(fixtureType));
			m_FrameworkBehavior = frameworkBehavior;

			// Initialize the fields.
			Initialize_TestFixtureInfo();
		}
		#endregion // Constructors
		#region Constructor Helpers
		/// <summary>
		/// We initialize fields here because we don't want to create disposable
		/// resources before we know that the class will be constructed OK. This
		/// is also the style needed for some types of serialization, since a
		/// constructor is not always called in some styles of deserialization.
		/// This gives us flexibility to support multiple types of serialization.
		/// </summary>
		protected void Initialize_TestFixtureInfo()
		{
			m_HasFixtureSetUpBeenCalled = new NullableSynchronizedWrapper<bool>();
			m_HasFixtureTearDownBeenCalled = new NullableSynchronizedWrapper<bool>();
			m_MaxTimesAnyTestCalled = new NullableSynchronizedWrapper<long>(-1);
			m_NumTimesAnyTestCalled = new NullableSynchronizedWrapper<long>();
			m_TestFixtureWrappers = new List<IPAFTestFixtureWrapper>();
		}
		#endregion // Constructor Helpers
		#region IPAFTestFixtureInfo Implementation
		#region Properties
		/// <summary>
		/// See <see cref="IPAFTestFixtureInfo"/>.
		/// </summary>
		/// <remarks>
		/// Virtual for remote/local peer.
		/// </remarks>
		/// <threadsafety>
		/// Unsynchronized.
		/// </threadsafety>
		public virtual ConstructorInfo FixtureConstructor
		{ get { return m_FixtureConstructor; } set { m_FixtureConstructor = value; } }
		/// <summary>
		/// See <see cref="IPAFTestFixtureInfo"/>.
		/// </summary>
		/// <remarks>
		/// Virtual for remote/local peer. Core allows only one instance.
		/// </remarks>
		/// <threadsafety>
		/// Unsynchronized. This should obviously be set only once.
		/// </threadsafety>
		public virtual object FixtureInstance
		{ get { return m_FixtureInstance; } set { m_FixtureInstance = value; } }
		/// <summary>
		/// See <see cref="IPAFTestFixtureInfo"/>.
		/// </summary>
		/// <remarks>
		/// Virtual for remote/local peer.
		/// </remarks>
		/// <threadsafety>
		/// Unsynchronized.
		/// </threadsafety>
		public virtual IPAFTestMethodInfo FixtureSetUpMethod
		{ get { return m_FixtureSetUpMethod; } set { m_FixtureSetUpMethod = value; } }
		/// <summary>
		/// See <see cref="IPAFTestFixtureInfo"/>.
		/// </summary>
		/// <remarks>
		/// Virtual for remote/local peer.
		/// </remarks>
		/// <threadsafety>
		/// Unsynchronized.
		/// </threadsafety>
		public IPAFTestMethodInfo FixtureTearDownMethod
		{ get { return m_FixtureTearDownMethod; } set { m_FixtureTearDownMethod = value; } }
		/// <summary>
		/// See <see cref="IPAFTestFixtureInfo"/>.
		/// </summary>
		/// <remarks>
		/// Virtual for remote/local peer.
		/// </remarks>
		/// <threadsafety>
		/// Unsynchronized.
		/// </threadsafety>
		public virtual IPAFTypeHolder FixtureType
		{ get { return m_FixtureType; } set { m_FixtureType = value; } }
		/// <summary>
		/// See <see cref="IPAFTestFixtureInfo"/>.
		/// </summary>
		/// <remarks>
		/// Virtual for remote/local peer.
		/// </remarks>
		/// <threadsafety>
		/// Unsynchronized.
		/// </threadsafety>
		public virtual IPAFTestFrameworkBehavior FrameworkBehavior
		{ get { return m_FrameworkBehavior; } set { m_FrameworkBehavior = value; } }
	    /// <summary>
		/// See <see cref="IPAFTestFixtureInfo"/>.
		/// </summary>
		/// <remarks>
		/// Virtual for remote/local peer.
		/// </remarks>
		/// <threadsafety>
		/// Synchronized.
		/// </threadsafety>
		public virtual bool HasFixtureSetupBeenCalled
		{ get { return m_HasFixtureSetUpBeenCalled.NullableObject; } set { m_HasFixtureSetUpBeenCalled.NullableObject = value; } }
		/// <summary>
		/// See <see cref="IPAFTestFixtureInfo"/>.
		/// </summary>
		/// <remarks>
		/// Virtual for remote/local peer.
		/// </remarks>
		/// <threadsafety>
		/// Synchronized.
		/// </threadsafety>
		public virtual bool HasFixtureTearDownBeenCalled
		{ get { return m_HasFixtureTearDownBeenCalled.NullableObject; } set { m_HasFixtureTearDownBeenCalled.NullableObject = value; } }
		/// <summary>
		/// See <see cref="IPAFTestFixtureInfo"/>.
		/// </summary>
		/// <remarks>
		/// Virtual for remote/local peer.
		/// </remarks>
		public virtual long MaxTimesAnyTestCalled
		{ get { return m_MaxTimesAnyTestCalled.NullableObject; } set { m_MaxTimesAnyTestCalled.NullableObject = value; } }
		/// <summary>
		/// See <see cref="IPAFTestFixtureInfo"/>.
		/// </summary>
		/// <remarks>
		/// Virtual for remote/local peer.
		/// </remarks>
		/// <threadsafety>
		/// Synchronized.
		/// </threadsafety>
		public virtual long NumTimesAnyTestCalled
		{ get { return m_NumTimesAnyTestCalled.NullableObject; } set { m_NumTimesAnyTestCalled.NullableObject = value; } }
		/// <summary>
		/// See <see cref="IPAFTestFixtureInfo"/>.
		/// </summary>
		/// <remarks>
		/// Virtual for remote/local peer.
		/// </remarks>
		/// <threadsafety>
		/// Unsynchronized.
		/// </threadsafety>
		public virtual IList<IPAFTestFixtureWrapper> TestFixtureWrappers
		{
			get { return m_TestFixtureWrappers; }
		}
		#region IPAFEnumerableProviderProvider<IPAFTestMethodInfo> implementation
		/// <summary>
		/// Backing for explicit interface implementation.
		/// </summary>
		/// <remarks>
		/// Virtual for remote/local peer.
		/// </remarks>
		/// <threadsafety>
		/// Unsynchronized.
		/// </threadsafety>
		protected virtual IPAFEnumerableProvider<IPAFTestMethodInfo> TestElementInfoItemEnumerableProviderPV
	    { get { return m_TestMethodEnumerable; } set { m_TestMethodEnumerable = value; } }

	    /// <summary>
	    /// See <see cref="IPAFTestFixtureInfo"/>.
	    /// </summary>
		public IList<string> TestMethodInclusionList { get; set; }

	    /// <summary>
	    /// See <see cref="IPAFTestFixtureInfo"/>.
	    /// </summary>
	    public IList<string> TestMethodExclusionList { get; set; }

		/// <summary>
		/// See <see cref="IPAFTestFixtureInfo"/>.
		/// </summary>
		/// <threadsafety>
		/// Unsynchronized.
		/// </threadsafety>
		/// <remarks>
		/// <para>
		/// This property may be reset internally during the initialization process.
		/// We generally use <see cref="IPAFResettableEnumerableProvider{T}"/>'s, so
		/// the incoming one may be wrapped.
		/// </para>
		/// <para>
		/// </para>
		/// Explicit implementation needed here due to interface ambiguities.
		/// </remarks>
		IPAFEnumerableProvider<IPAFTestMethodInfo> IPAFEnumerableProviderProvider<IPAFTestMethodInfo>.EnumerableProvider
		{ get { return TestElementInfoItemEnumerableProviderPV; } }

		/// <summary>
		/// See <see cref="IPAFEnumerableProviderProvider{IPAFTestMethodInfo}"/>
		/// </summary>
		/// <param name="provider">
		/// See <see cref="IPAFEnumerableProviderProvider{IPAFTestMethodInfo}"/>
		/// </param>
		public virtual void SetProvider(IPAFEnumerableProvider<IPAFTestMethodInfo> provider)
	    {
		    m_TestMethodEnumerableProvider = provider;
	    }
	    #endregion // IPAFEnumerableProviderProvider<IPAFTestMethodInfo> implementation

		/// <summary>
		/// See <see cref="IPAFTestFixtureInfo"/>.
		/// </summary>
		/// <remarks>
		/// Virtual for remote/local peer.
		/// </remarks>
		/// <threadsafety>
		/// Unsynchronized.
		/// </threadsafety>
		public virtual ICollection<IPAFTestMethodInfo> TestMethods
		{ get { return m_TestMethods; } set { m_TestMethods = value; } }
		/// <summary>
		/// See <see cref="IPAFTestFixtureInfo"/>.
		/// </summary>
		/// <remarks>
		/// Virtual for remote/local peer.
		/// </remarks>
		/// <threadsafety>
		/// Unsynchronized.
		/// </threadsafety>
		public virtual IPAFTestMethodInfo TestSetUpMethod
		{ get { return m_TestSetUpMethod; } set { m_TestSetUpMethod = value; } }
		/// <summary>
		/// See <see cref="IPAFTestFixtureInfo"/>.
		/// </summary>
		/// <remarks>
		/// Virtual for remote/local peer.
		/// </remarks>
		/// <threadsafety>
		/// Unsynchronized.
		/// </threadsafety>
		public virtual IPAFTestMethodInfo TestTearDownMethod
		{ get { return m_TestTearDownMethod; } set { m_TestTearDownMethod = value; } }
		#endregion // Properties
		#region Methods
		/// <summary>
		/// See <see cref="IPAFTestFixtureInfo"/>
		/// </summary>
		/// <returns>
		/// See <see cref="IPAFTestFixtureInfo"/>
		/// </returns>
		public virtual ICollection<IPAFTestMethodInfo> GetActiveTestMethods()
		{
			if (!IsExePipelineInitialized) return null;

			var mthds = GetTestMethods();
			mthds = mthds.GetActiveElements();

			return mthds;
		}
		/// <summary>
		/// See <see cref="IPAFTestFixtureInfo"/>
		/// </summary>
		/// <returns>
		/// See <see cref="IPAFTestFixtureInfo"/>
		/// </returns>
		public virtual ICollection<IPAFTestMethodInfo> GetTestMethods()
		{

			var mthds = this.GetChildInfoSubtypesOfType<IPAFTestElementInfo, IPAFTestMethodInfo>();
			return mthds;
		}
		/// <summary>
		/// See <see cref="IPAFTestFixtureInfo"/>
		/// </summary>
		public virtual void SetTestFixtureRunning(bool isRunning)
		{
			IsPipelinedObjectRunning = isRunning;
		}
		#endregion // Methods
		#endregion // IPAFTestFixtureInfo Implementation
		#region ITestFixtureInfoProvider Implementation
		/// <summary>
		/// So I can provide myself for non-aggregated scenarios.
		/// </summary>
		public virtual IPAFTestFixtureInfo ProvidedFixtureInfo
		{ get { return this; } }
        #endregion // ITestFixtureInfoProvider Implementation
        public override void UninitializeExePipeline()
		{
			ClearWrappers();
		}
		#region Implementation of IPAFBaseExePipelineInitialize
		/// <summary>
		/// This method just calls <see cref="SetTestFixtureProps"/> with
		/// a <see langword="false"/> argument so the fixture is not instantiated.
		/// Calls base first.
		/// </summary>
		public override void InitializeExePipeline(IPAFProviderPattern<IPAFPipelineParams<IPAFServiceManager<IPAFService>>> provider)
		{
			if (IsExePipelineInitialized) return;
			base.InitializeExePipeline(provider);
			SetTestFixtureProps(false);
		}
		#endregion // Implementation of IPAFBaseExePipelineInitialize
		#region Implementation of IPAFBaseExePipeline
		/// <summary>
		/// See <see cref="IPAFBaseExePipeline{T}"/>.
		/// </summary>
		public override bool IsPipelinedObjectRunning
		{
			get
			{
				return m_IsPipelinedObjectRunning.NullableObject;
			}
		}
        #endregion // Implementation of IPAFBaseExePipeline
        #region Methods
        /// <summary>
        /// See <see cref="IPAFBaseExePipeline{T}"/>. We dispose all wrappers
        /// (we can only contain one) and clear the wrapper collection.
        /// </summary>
        protected virtual void ClearWrappers()
		{
			foreach (var wrapper in TestFixtureWrappers) {
				wrapper.Dispose();
			}
			TestFixtureWrappers.Clear();
		}
		/// <summary>
		/// This is the main method that sets up all the properties on the fixture
		/// by examining the type and its members. This method needn't be called until
		/// the full fixture info is actually needed for a test. Furthermore, the
		/// instance of the fixture needn't be constructed at this time.
		/// </summary>
		/// <param name="constructInstance">
		/// <see langword="true"/> to construct the fixture instance when we are done setting
		/// up its props. Note that this method will exit if errors in construction
		/// of the parameters occur. It will set the <see cref="IPAFTestElementInfo.TestElementStatus"/>
		/// to an inactive state and publish error information to the appropriate
		/// props.
		/// </param>
        /// <returns>
        /// <see langword="false"/> if any exceptions have been accumulated on this fixture.
        /// The fixture should then be viewed as in a "disabled" state.
        /// </returns>
		protected virtual bool SetTestFixtureProps(bool constructInstance)
		{
			try {
				if (FixtureType.TypeType == null)
					FixtureType.ResolveType(null, false);

				if (FrameworkBehavior == null) FrameworkBehavior
					= PAFTestFrameworkBehavior.GetStandardPAFUnitParams();

                // See if fixture is OK and get out if not.
                if (m_CheckTestFixtureDelegate == null)
                    m_CheckTestFixtureDelegate = PAFTestFixtureInfoExtensions.CheckFixtureType;
				Exception fixtureAnalysisException = PAFTestFixtureInfoExtensions.CheckFixtureType(this, FrameworkBehavior);
				if (fixtureAnalysisException != null) {
					AddTestException(fixtureAnalysisException);
					TestElementStatus = TestElementRunnabilityStatus.ExcludedByErrors;
                    ExcludedReason = PAFTestFixtureInitiaizationExceptionMessageTags.FIXTURE_INITIALIZATION_FAILURE;
                    return false;
				}

                // Get the default constructor.
				FixtureConstructor = FixtureType.TypeType.GetInstanceConstructor();

				// No methods at all is obviously bogus.
				var allMethodInfosOnType = FixtureType.TypeType.PAFGetMethods().ToList();
				if (allMethodInfosOnType.Count == 0) {
                    var data = new PAFTFIED(FixtureType);
					TestElementStatus = TestElementRunnabilityStatus.ExcludedByErrors;
                    ExcludedReason = PAFTestFixtureInitiaizationExceptionMessageTags.NO_TEST_METHODS_FOUND_ON_FIXTURE;
					AddTestException(new PAFStandardException<PAFTFIED>(data));
                    return false;
				}

				MemberInfo memberInfo;

				// Is there a "test setup" method?
				var foundMemberInfoList = allMethodInfosOnType.GetMembersWithPublicNamedAttributeInfo(
					TestFrameworkData.TestSetUpMethodAttributeName(FrameworkBehavior.FrameworkName)).ToList();
				if (foundMemberInfoList.Count > 0){
					memberInfo = foundMemberInfoList[0];
					TestSetUpMethod = new PAFTestFrameworkMethodInfo(memberInfo.DeclaringType, memberInfo);
				}

				// Is there a "test teardown" method?
				foundMemberInfoList = allMethodInfosOnType.GetMembersWithPublicNamedAttributeInfo(
					TestFrameworkData.TestTearDownMethodAttributeName(FrameworkBehavior.FrameworkName)).ToList();
				if (foundMemberInfoList.Count > 0) {
					memberInfo = foundMemberInfoList[0];
					TestTearDownMethod = new PAFTestFrameworkMethodInfo(memberInfo.DeclaringType, memberInfo);
				}

				// Is there a "test fixture setup" method?
				foundMemberInfoList = allMethodInfosOnType.GetMembersWithPublicNamedAttributeInfo(
					TestFrameworkData.TestClassSetUpMethodAttributeName(FrameworkBehavior.FrameworkName)).ToList();
				if (foundMemberInfoList.Count > 0) {
					memberInfo = foundMemberInfoList[0];
					FixtureSetUpMethod = new PAFTestFrameworkMethodInfo(memberInfo.DeclaringType, memberInfo);
				}

				// Is there a "test fixture teardown" method?
				foundMemberInfoList = allMethodInfosOnType.GetMembersWithPublicNamedAttributeInfo(
					TestFrameworkData.TestClassTearDownMethodAttributeName(FrameworkBehavior.FrameworkName)).ToList();
				if (foundMemberInfoList.Count > 0) {
					memberInfo = foundMemberInfoList[0];
					FixtureTearDownMethod
						= new PAFTestFrameworkMethodInfo(memberInfo.DeclaringType, memberInfo);
				}

                // Now gather up all the test methods.
                var testMethodInfos
	                = PAFTestFixtureInfoExtensions.GatherTestMethodsOnFixture(
		                this, allMethodInfosOnType);

				if (TestMethodInclusionList != null)
					testMethodInfos
						= testMethodInfos.FilterMethodInfosOnNamesMatch(TestMethodInclusionList);

				if (TestMethodExclusionList != null)
					testMethodInfos
						= testMethodInfos.FilterMethodInfosOnNamesNonMatch(TestMethodExclusionList);
                       
				// Gotta' have actual test methods or we're no good.
				if (testMethodInfos.Count == 0) {
                    var data = new PAFTFIED(FixtureType);
					TestElementStatus = TestElementRunnabilityStatus.ExcludedByErrors;
                    ExcludedReason = PAFTestFixtureInitiaizationExceptionMessageTags.NO_TEST_METHODS_FOUND_ON_FIXTURE;
					AddTestException(new PAFStandardException<PAFTFIED>(data));
                    return false;
				}

				// We build a fixture method info for each test method and add them
				// to our child collection..
				foreach (var mthd in testMethodInfos) {
					var fixturMethod = new PAFTestFrameworkMethodInfo(null, mthd, this);
					if (mthd.DoesMemberHavePublicNamedAttributeInfo(TestFrameworkData.ExplicitAttributeName(FrameworkBehavior.FrameworkName)))
                    {
						fixturMethod.TestElementStatus = TestElementRunnabilityStatus.ExcludedByAttribute;
						// TODO - pull the reason out of the attributes. For now just indicate
						// TODO it was explicit.
						fixturMethod.ExcludedReason = "Explicit";
					}

					if (mthd.DoesMemberHavePublicNamedAttributeInfo(FrameworkBehavior.FrameworkName))
                    {
						fixturMethod.TestElementStatus = TestElementRunnabilityStatus.ExcludedByAttribute;
						// TODO - pull the reason out of the attributes. For now just indicate
						// TODO it was ignored.
						fixturMethod.IgnoredReason = "Ignored";
					}
					AddTestElement(fixturMethod);
				}
				if (constructInstance) {
					var constructionException = this.PAFConstructTestFixtureInstance();
					if (constructionException != null)
					{
						AddTestException(constructionException);
						ExcludedReason = constructionException.Message;

						TestElementStatus = TestElementRunnabilityStatus.ExcludedByErrors;
                        return false;
					}
				}
			}
			catch (Exception ex) {
				// TODO - better exception here.
				AddTestException(ex);
				if (!string.IsNullOrEmpty(ex.Message))
					ExcludedReason = ex.Message;
				else
					ExcludedReason = "Unknown exception, Type = " + ex.GetType();

				TestElementStatus = TestElementRunnabilityStatus.ExcludedByErrors;
                return false;
			}
            if (Exceptions.Any())
                return false;
            return true;
		}
        #region Disposal Methods
        /// <summary>
        /// <see cref="IPAFDisposable"/>. This is a method that is supplied as a delegate
        /// to the disposer to call during disposal.
        /// </summary>
        /// <param name="disposing">
        /// <see cref="IPAFDisposable"/>.
        /// </param>
        /// <param name="obj">
        /// <see cref="IPAFDisposable"/>.
        /// This is not used in this method.
        /// </param>
        /// <remarks>
        /// <para>
        /// When subclassing this class (or a class like it), this is the method that should
        /// be overridden. Obviously the designer of the subclass should keep in mind the order
        /// of resource disposal that should be followed and call the base at the appropriate
        /// point (usually after the subclass call, but not always).
        /// </para>
        /// <para>
        /// Exceptions are caught and recorded in the registry in addition to being returned
        /// in an <see cref="PAFStandardException{PAFAggregateExceptionData}"/>.
        /// </para>
        /// </remarks>
        protected override Exception TestElementInfoDispose(bool disposing, object obj)
        {
            // Dump the wrappers.
            ClearWrappers();

            var eList = new List<Exception>();

            // First dispose fields that are IDisposable for sure.
            eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_HasFixtureSetUpBeenCalled, true));
            eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_HasFixtureTearDownBeenCalled, true));
            eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_MaxTimesAnyTestCalled, true));
            eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_NumTimesAnyTestCalled, true));

            // Now dispose fixture if it's here and IDisposable.
            IDisposable disposable;
            if ((FixtureInstance != null) && ((disposable = FixtureInstance as IDisposable) != null))
            {
                eList.AddNoNulls(PAFDisposalUtils.Disposer(ref disposable, false));
                m_FixtureInstance = null;
            }

            //////////////////////////////////////////////////////////////////////////////
            // Now my superclass. Note that we do not aggregate exceptions from the superclass
            // here. This is an arbitrary decision. We allow the superclass to do its own
            // exception handling. Exceptions will be installed in the registry
            // in both places, and each will be attached to a different object, allowing
            // their discrimination.
            base.TestElementInfoDispose(disposing, obj);

            // If we have any exceptions, put them in an aggregator.
            if (eList.Count > 0)
            {
                var exceptions = new PAFAED(eList);
                var ex = new PAFStandardException<PAFAggregateExceptionData>(exceptions);
                // Seal the list.
                exceptions.AddException(null);
                // We just put these in the registry. If a framework is in use, it
                // should dig these out and report them.
                DisposalRegistry.RecordDisposalException(this, ex);
                return ex;
            }
            return null;
        }
		#endregion // Disposal Methods
	    #endregion // Methods
    }
}
