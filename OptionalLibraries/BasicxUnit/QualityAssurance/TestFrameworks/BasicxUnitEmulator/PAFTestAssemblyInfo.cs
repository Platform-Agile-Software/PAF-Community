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
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Execution.Pipeline;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.Manufacturing;
using PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Exceptions;
using PlatformAgileFramework.Serializing;
using PlatformAgileFramework.Serializing.Attributes;
using PlatformAgileFramework.TypeHandling;

// Exception shorthand.
using PlatformAgileFramework.TypeHandling.AssemblyExtensionMethods;
using PAFTAIED = PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.PAFTestAssemblyInitializationExceptionData;
using IPAFTAIED = PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Exceptions.IPAFTestAssemblyInitializationExceptionData;
using PAFTFIED = PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.PAFTestFixtureInitializationExceptionData;
using IPAFTFIED = PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Exceptions.IPAFTestFixtureInitializationExceptionData;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator
{
	/// <summary>
	/// Default implementation of <see cref="IPAFTestAssemblyInfo"/>. This class now
	/// contains <see cref="IPAFTestFixtureInfo"/>s and <see cref="IPAFTestFixtureWrapper"/>s.
	/// </summary>
	/// <threadsafety>
	/// Implementations are NOT necessarily expected to be thread-safe. This class
	/// is setup once and is then only read.
	/// </threadsafety>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 21dec2017 </date>
	/// <description>
	/// Touched up to put this interface in the element tree and make the class
	/// work with both fixtures directly or wrappers. Anything new should use wrappers.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> DAP </author>
	/// <date> 09aug2012 </date>
	/// <description>
	/// Made partial - moved observers to extended.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 04aug2012 </date>
	/// <description>
	/// Added history.
	/// </description>
	/// </contribution>
	/// </history>
	[PAFSerializable(PAFSerializationType.PAFSurrogate)]
	// ReSharper disable once PartialTypeWithSinglePart
	public partial class PAFTestAssemblyInfo
		: PAFTestElementInfo<IPAFTestAssemblyInfo>, IPAFTestAssemblyInfo
    {
		#region Class Fields and AutoProperties
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal IPAFAssemblyHolder m_AsmName;
		/// <summary>
		/// Backing.
		/// </summary>
		private bool? m_IsHarness;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal IPAFTestFrameworkBehavior m_TestFrameworkBehavior;
	    /// <summary>
	    /// See <see cref="IPAFTestAssemblyInfo"/>
	    /// </summary>
	    public Func<IPAFAssemblyHolder, IPAFTestFrameworkBehavior, IList<IPAFTypeHolder>>
			FixtureGatherer { get; set; }
	    /// <summary>
	    /// See <see cref="IPAFTestAssemblyInfo"/>
	    /// </summary>
	    public Action<IPAFTestAssemblyInfo, object> FixtureRunner { get; set; }
		#endregion Class Fields and AutoProperties
		#region Constructors
		/// <summary>
		/// Constructs with the one mandatory parameter.
		/// </summary>
		/// <param name="asmName">
		/// Sets <see cref="AsmName"/>. Not <see langword="null"/> or blank.
		/// </param>
		/// <param name="name">
		/// Sets <see cref="IPAFTestElementInfo.TestElementName"/>. Can be <see langword="null"/> or blank.
		/// <c>asmName.AssemblySimpleName</c> is then used.
		/// </param>
		/// <param name="testFrameworkBehavior">
		/// Loads <see cref="TestFrameworkBehavior"/>. Default is PAF.
		/// </param>
		/// <param name="children">
		/// Sets <see cref="IPAFTestElementInfo.AllChildren"/>
		/// </param>
		/// <param name="parent">
		/// Sets <see cref="IPAFTestElementInfo.Parent"/>. May be <see langword="null"/> if a root node.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">"asmName"</exception>
		/// </exceptions>
		public PAFTestAssemblyInfo(IPAFAssemblyHolder asmName, string name = null,
			IPAFTestFrameworkBehavior testFrameworkBehavior = null,
			IEnumerable<IPAFTestFixtureInfo> children = null, IPAFTestElementInfo parent = null)
			: base(GetName(asmName, name), children, parent)
		{
		    m_AsmName = asmName ?? throw new ArgumentNullException(nameof(asmName));

			TestFrameworkBehavior = testFrameworkBehavior ?? PAFTestFrameworkBehavior.GetStandardPAFUnitParams();

			// Just reset the name - no big deal.
			if (string.IsNullOrEmpty(name)) m_Name = asmName.AssemblySimpleName;
		}
        #endregion // Constructors
        /// <summary>
        /// Little helper so we don't have to fiddle with the base constructors.
        /// This method takes <paramref name="name"/> if it is non - <see langword="null"/>.
        /// It will take <see cref="IPAFAssemblyHolder.AssemblySimpleName"/> if
        /// <paramref name="asmName"/> is non - <see langword="null"/>. Otherwise it
        /// returns <see langword="null"/>.
        /// </summary>
        /// <param name="asmName">Assembly name.</param>
        /// <param name="name">String name.</param>
        /// <returns>A name or <see langword="null"/>.</returns>
        protected internal static string GetName(IPAFAssemblyHolder asmName, string name)
		{
			if (!string.IsNullOrEmpty(name)) return name;
		    return asmName?.AssemblySimpleName;
		}
		#region Properties
		/// <summary>
		/// See <see cref="IPAFTestAssemblyInfo"/>
		/// </summary>
		public IPAFAssemblyHolder AsmName
		{ get { return m_AsmName; } set { m_AsmName = value; } }
		/// <summary>
		/// See <see cref="IPAFTestAssemblyInfo"/>
		/// </summary>
		public bool? IsHarness
		{ get { return m_IsHarness; } set { m_IsHarness = value; } }
		/// <summary>
		/// See <see cref="IPAFTestAssemblyInfo"/>
		/// </summary>
		public IPAFTestFrameworkBehavior TestFrameworkBehavior
		{ get { return m_TestFrameworkBehavior; } set { m_TestFrameworkBehavior = value; } }
		#endregion Properties
		#region Methods
		#region Implementation of IPAFBaseExePipelineInitialize
		/// <summary>
		/// Builds all the fixtures without instantiating.
		/// </summary>
		public override void InitializeExePipeline(IPAFProviderPattern<IPAFPipelineParams<IPAFServiceManager<IPAFService>>> provider)
		{
			if (IsExePipelineInitialized) return;
			GetMyFixturesFromAssembly(AsmName, TestFrameworkBehavior);
			IsExePipelineInitialized = true;
		}
		#endregion // Implementation of IPAFBaseExePipelineInitialize
	    #region Implementation of IPAFBaseExePipeline
	    /// <summary>
		/// See <see cref="IPAFBaseExePipeline{T}"/>. We initialize ourselves,
		/// then iterate over the wrappers, running each. If <see cref="FixtureRunner"/>
		/// is present, that is used. If not fixtures are executed serially and
		/// synchronously.
		/// </summary>
		/// <param name="obj">See interface.</param>
		public override void RunPipelinedObject(object obj)
	    {
			InitializeExePipeline(this);

			// Use the runner that's pushed in?
		    if (FixtureRunner != null)
		    {
			    FixtureRunner(this, obj);
			    return;
		    }

			// No, just the default synchronous run.
		    foreach (var wrapper in this.GetWrappers())
		    {
			    wrapper.RunPipelinedObject(obj);
		    }
	    }
	    #endregion // Implementation of IPAFBaseExePipeline
		/// <summary>
		/// This method will load the children of this node with test fixtures
		/// found in the assembly and their wrappers. This just calls the static helper
		/// <see cref="GetFixturesFromAssembly"/>  if the
		/// <see cref="FixtureGatherer"/> is <see langword="null"/>.
		/// It then loads the child collection. It then calls <see cref="InitializeExePipeline"/>
		/// on each wrapper to pre-qualify the wrappers and associated fixtures.
		/// </summary>
		/// <param name="holder">
		/// <see cref="GetFixturesFromAssembly"/>.
		/// </param>
		/// <param name="behavior">
		/// <see cref="GetFixturesFromAssembly"/>.
		/// </param>
		/// <param name="loader">
		/// <see cref="GetFixturesFromAssembly"/>.
		/// </param>
		protected virtual void GetMyFixturesFromAssembly
		(IPAFAssemblyHolder holder, IPAFTestFrameworkBehavior behavior, IPAFAssemblyLoader loader = null)
		{
			// TODO exceptions for null args
			try
			{
				// First order of business is to gather fixtures, either with a custom
				// gatherer or our standard one.
				IList<IPAFTypeHolder> fixtureTypes;
				if (FixtureGatherer != null)
					fixtureTypes = FixtureGatherer(holder, behavior);
				else
					fixtureTypes = GetFixturesFromAssembly(holder, behavior, loader).IntoArray();


				if ((fixtureTypes == null) || (fixtureTypes.Count == 0)) {
					var data = new PAFTAIED(holder);
					var ex = new PAFStandardException<IPAFTAIED>(data, PAFTestAssemblyInitializationExceptionMessageTags.NO_TEST_FIXTURES_FOUND_IN_ASSEMBLY);
					AddTestException(ex);
					// This one causes us to turn our light off.
					TestElementStatus = TestElementRunnabilityStatus.ExcludedByErrors;
                    ExcludedReason = PAFTestAssemblyInitializationExceptionMessageTags.NO_TEST_FIXTURES_FOUND_IN_ASSEMBLY;
					return;
				}
				var exceptionList = new List<Exception>();
				foreach (var fixtureType in fixtureTypes) {
					try {
						// Call the innocuous (relatively speaking) constructor.
						var fixture = new PAFTestFixtureInfo(fixtureType, behavior, this);
						// We add the fixture here, before we conduct the full initialization,
						// since that is the dangerous part. We want to hold the fixture
						// as a child, even though it may be a bad child.
						AddTestElement(fixture);

						// Wrap the fixture and add the wrapper.
						var wrapper = new PAFTestFixtureWrapper(fixture, this);
						AddTestElement(wrapper);

						// The wrapper and fixture initialization may generate errors, but it
						// annotates the fixture/wrapper with the errors and makes it inactive
						// if bad things happen.
						wrapper.InitializeExePipeline(this);
					}
					catch (Exception ex) {
						// If we get here, something really unexpected happened. All we
						// can do is put it in our exception collection. We indicate the
						// fixture type that we had the problem with.
						var data = new PAFTFIED(fixtureType);
						exceptionList.Add(new PAFStandardException<IPAFTFIED>
                            (data, PAFTestFixtureInitiaizationExceptionMessageTags.FIXTURE_INITIALIZATION_FAILURE, ex));
					}
				}
				if (exceptionList.Count > 0) {
					var aggregationData = new PAFAggregateExceptionData(exceptionList);
					// We aren't really worried about security in a testing framework, but
					// we seal the list, anyway.
					aggregationData.AddException(null);
					AddTestException(new PAFStandardException<PAFAggregateExceptionData>
									(aggregationData, "Bad fixtureTypes"));
				}
			}
			catch (Exception ex) {
				// If we got here, there is something bogus with the assembly.
				var data = new PAFTAIED(holder);
				var exception = new PAFStandardException<PAFTAIED>
					(data, PAFStandardExceptionMessageTags.UNKNOWN_FAILURE, ex);
				AddTestException(exception);
				// This one causes us to turn our light off.
				TestElementStatus = TestElementRunnabilityStatus.ExcludedByErrors;
				ExcludedReason = PAFStandardExceptionMessageTags.UNKNOWN_FAILURE;
			}
		}
		#region Static Helpers
		/// <summary>
		/// This method will load test fixtures found in the assembly. This method
		/// sets all fixtures to use our <see cref="IPAFAssemblyHolder"/>.
		/// </summary>
		/// <param name="holder">
		/// Early/late bound assembly.
		/// </param>
		/// <param name="behavior">
		/// Incoming framework behavior that specifies what framework we are using
		/// with <see cref="IPAFTestFrameworkBehavior.FrameworkName"/>. If this is
		/// <see langword="null"/> or blank, the assembly is scanned for fixtures attributed
		/// with all possible fixture attributes for our loaded framework emulations.
		/// The attribute that is found first is used to declare what type of framework
		/// is in use. Sometimes folks like to build test fixtures for multiple
		/// frameworks (e.g. MS and NUnit) and we support that. However, the
		/// <paramref name="behavior"/> must come in with a name or the first
		/// one discovered inside the assy wins. the framework name on the
		/// incoming behavior is loaded from what is found.
		/// </param>
		/// <param name="loader">
		/// Loader to load assembly with. If <see langword="null"/> the one on the holder
		/// is used.
		/// </param>
		/// <returns>
		/// Never <see langword="null"/>.
		/// </returns>
		/// <exceptions>
		/// Lots. We don't catch anything coming up from our stuff or MS code.
		/// Wrap this in a try/catch block.
		/// </exceptions>
		public static IList<IPAFTypeHolder> GetFixturesFromAssembly
		(IPAFAssemblyHolder holder, IPAFTestFrameworkBehavior behavior, IPAFAssemblyLoader loader)
		{
			var fixtureTypes = new List<IPAFTypeHolder>();
			// TODO exceptions for null args
			var asm = PAFAssemblyHolderBase.ResolveAssembly(holder, loader);
			IEnumerable<IPAFTypeHolder> types = null;
			if (string.IsNullOrEmpty(behavior.FrameworkName)) {
				// In this loop, we automatically determine the framework in use.
				foreach (var fName in TestFrameworkData.GetAvailableTestFrameworks()) {
					var attributeName = TestFrameworkData.TestClassAttributeName(fName);
					types = asm.GatherAttributedTypes(attributeName, true);
					if (types != null) {
						behavior.FrameworkName = fName;
						break;
					}
				}
			}
			else {
				var attributeName = TestFrameworkData.TestClassAttributeName(behavior.FrameworkName);
				types = asm.GatherAttributedTypes(attributeName, true);
			}
			if (types == null)
				return fixtureTypes;

			types = types.ToArray();

			fixtureTypes.AddRange(types);


			var delegatedTypes = new List<IPAFTypeHolder>();
			// Create new types to delegate to our root holder.
			foreach (var type in types) {
				var delegatedType
					= new PAFTypeHolder(type.TypeType, null, holder, null, loader);
				delegatedTypes.Add(delegatedType);
			}
			return delegatedTypes;
		}
		#endregion // Static Helpers
		#endregion // Methods
	}
}
