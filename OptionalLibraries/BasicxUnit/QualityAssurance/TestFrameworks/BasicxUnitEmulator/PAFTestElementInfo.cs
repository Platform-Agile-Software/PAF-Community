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
using System.Security;
using PlatformAgileFramework.Collections.Enumerators;
using PlatformAgileFramework.Collections.ExtensionMethods;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Execution.Pipeline;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.MultiProcessing.Threading.NullableObjects;
using PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Display;
using PlatformAgileFramework.TypeHandling;
using PlatformAgileFramework.TypeHandling.Disposal;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator
{
	/// <summary>
	/// Default implementation of <see cref="IPAFTestElementInfo"/>
	/// </summary>
	/// <threadsafety>
	/// Basic writable props are designed to be set once, then only read.
	/// </threadsafety>
	/// <history>
	/// <author> KRM </author>
	/// <date> 07aug2012 </date>
	/// <contribution>
	/// <para>
	/// Reconstructed to support rewrite of <see cref="IPAFTestElementInfo"/>
	/// </para>
	/// </contribution>
	/// <author> KRM </author>
	/// <date> 04aug2012 </date>
	/// <contribution>
	/// <para>
	/// Added history. Added elevated trust stuff.
	/// </para>
	/// </contribution>
	/// </history>
	public class PAFTestElementInfo: IPAFTestElementInfo
	{
		#region Class Fields and AutoProperties

		/// <summary>
		/// Static default for the display element gatherer.
		/// See <see cref="IPAFTestElementInfo"/>.
		/// </summary>
		public static Func<IPAFTestElementInfo, IList<IPAFTestElementInfo>> DefaultGetDisplayChildElements { get; set; }
			= PAFTestElementInfoExtensions.GetAllElementChildren;
		/// <summary>
		/// Almost everything has a pipeline, so we provide it here.
		/// </summary>
		protected internal IPAFProviderPattern<IPAFPipelineParams<IPAFServiceManager<IPAFService>>> m_ProvidedPipelineParams;
        /// <summary>
        /// This is the local service manager which will be found up the hierarchical search.
        /// If this is loded at any node, the search stops here.
        /// </summary>
	    public IPAFServiceManager<IPAFService> LocalServiceManager { get; set; }
        /// <summary>
        /// Storage for all children.
        /// </summary>
        protected internal Collection<IPAFTestElementInfo> m_AllChildren;
		/// <summary>
		/// Enumerator factory for all children.
		/// </summary>
		protected internal IEnumerableFactory<IPAFTestElementInfo> m_TestElementInfoEnumerableFactory;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal NullableSynchronizedWrapper<TestElementRunnabilityStatus> m_ElementStatus;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal List<Exception> m_Exceptions;
		/// <summary>
		/// Enumerator factory for exceptions.
		/// </summary>
		protected internal IEnumerableFactory<Exception> m_ExceptionEnumerableFactory;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal string m_ExcludedReason;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal NullableSynchronizedWrapper<bool> m_HasPipelinedObjectRun;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal string m_IgnoredReason;
	    /// <summary>
	    /// Backing.
	    /// </summary>
	    protected internal bool m_IsElevatedTrust;
		/// <summary>
        /// Backing.
        /// </summary>
        protected internal NullableSynchronizedWrapper<bool> m_IsExePipelineInitialized;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal NullableSynchronizedWrapper<bool> m_IsExePipelineUninitialized;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal NullableSynchronizedWrapper<bool> m_IsPipelinedObjectRunning;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal string m_Name;
		/// <summary>
		/// Holds our surrogate disposer.
		/// </summary>
		protected PAFDisposer<Guid> m_PAFDisposer;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal IPAFTestElementInfo m_Parent;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal NullableSynchronizedWrapper<bool> m_Passed;
		/// <summary>
		/// Backing.
		/// </summary>
		protected IPAFTestElementResultInfo m_TestElementResultInfo;
		/// <summary>
		/// Lazy construction lock.
		/// </summary>
		private object m_TestElementResultInfoConstructionLock;

		/// <summary>
		/// See <see cref="IPAFTestElementInfo"/>.
		/// </summary>
		/// <remarks>
		/// We load the instance from the static default.
		/// </remarks>
		public Func<IPAFTestElementInfo, IList<IPAFTestElementInfo>> GetDisplayChildElements { get; set; }
			= DefaultGetDisplayChildElements;
		/// <summary>
		/// See <see cref="IPAFTestElementInfo"/>.
		/// Never set during tests running.
		/// </summary>
		public IPAFEnumerableProvider<IPAFTestElementInfo> TestElementEnumerableProvider { get; set; }
		#endregion Class Fields and AutoProperties
		#region Constructors

		/// <summary>
		/// Constructor initializes class fields.
		/// </summary>
		protected internal PAFTestElementInfo()
		{
			InitializeTestElementInfo();
		}
		/// <summary>
		/// Constructs with the one mandatory parameter.
		/// </summary>
		/// <param name="name">
		/// Sets <see cref="TestElementName"/>. Not <see langword="null"/> or blank.
		/// </param>
		/// <param name="children">
		/// Children that may be installed at construction time.
		/// Default = <see langword="null"/>.
		/// </param>
		/// <param name="parent">
		/// Sets <see cref="Parent"/>. May be <see langword="null"/> if a root node.
		/// Default = <see langword="null"/>.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">"name"</exception>
		/// </exceptions>
		protected PAFTestElementInfo(string name, IEnumerable<IPAFTestElementInfo> children = null,
			IPAFTestElementInfo parent = null): this()
		{
			if(string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
			m_Name = name;
			m_Parent = parent;

			// We build the disposer with the empty Guid (no security) and our instance
			// and the disposal delegate.
			m_PAFDisposer = new PAFDisposer<Guid>(Guid.Empty, this, TestElementInfoDispose);
			DisposalRegistry.RegisterForDisposal(m_PAFDisposer);

			// We play it safe and build the best IEnumerable's we can.
			m_TestElementInfoEnumerableFactory = new SafeEnumerableFactory<IPAFTestElementInfo>();
			m_ExceptionEnumerableFactory = new SafeEnumerableFactory<Exception>();

			// If we's got sum chillin', get 'em in....
			if (children == null) return;
			foreach(var child in children)
				m_AllChildren.Add(child);
		}
		/// <summary>
		/// Helper to set up class for serialization support.
		/// </summary>
		protected internal void InitializeTestElementInfo()
		{
		    // ReSharper disable InconsistentlySynchronizedField
            //// lock not needed in construction path.
			m_Exceptions = new List<Exception>();
			m_AllChildren = new Collection<IPAFTestElementInfo>();
		    // ReSharper restore InconsistentlySynchronizedField
			m_ElementStatus
                = new NullableSynchronizedWrapper<TestElementRunnabilityStatus>(TestElementRunnabilityStatus.Active);
			m_HasPipelinedObjectRun = new NullableSynchronizedWrapper<bool>();
			m_IsPipelinedObjectRunning = new NullableSynchronizedWrapper<bool>();
			m_IsExePipelineInitialized = new NullableSynchronizedWrapper<bool>();
			m_IsExePipelineUninitialized = new NullableSynchronizedWrapper<bool>();
			m_Passed = new NullableSynchronizedWrapper<bool>();
			m_Passed.NullTheObject();
		    m_ProvidedPipelineParams
		        = new PAFProviderPattern<IPAFPipelineParams<IPAFServiceManager<IPAFService>>>
		            (new PAFPipelineParams<IPAFServiceManager<IPAFService>>(ProvidedItem));
			m_TestElementResultInfoConstructionLock = new object();
		}
		#endregion // Constructors
		#region Properties
		/// <summary>
		/// See <see cref="IPAFTestElementInfo"/>
		/// </summary>
		/// <remarks>
		/// Virtual for remote/local peer.
		/// </remarks>
		public virtual IEnumerable<IPAFTestElementInfo> AllChildren
		{
			get
			{
				lock (m_AllChildren)
				{
					return m_TestElementInfoEnumerableFactory.BuildEnumerable(m_AllChildren);
				}
			}
		}
		/// <summary>
		/// See <see cref="IPAFTestElementInfo"/>
		/// </summary>
		/// <remarks>
		/// Virtual for remote/local peer.
		/// </remarks>
		public virtual IEnumerable<Exception> Exceptions
		{
			get
			{
				lock (m_Exceptions)
				{
					return m_ExceptionEnumerableFactory.BuildEnumerable(m_Exceptions);
				}
			}
		}
		/// <summary>
		/// See <see cref="IPAFTestElementInfo"/>
		/// </summary>
		/// <remarks>
		/// Virtual for remote/local peer.
		/// </remarks>
		public virtual TestElementRunnabilityStatus TestElementStatus
		{
			get { return m_ElementStatus.NullableObject; }
			set { m_ElementStatus.NullableObject = value; } 
		}
		/// <summary>
		/// See <see cref="IPAFTestElementInfo"/>
		/// </summary>
		/// <remarks>
		/// Virtual for GUI subclassing.
		/// </remarks>
		/// <remarks>
		/// Virtual for remote/local peer.
		/// </remarks>
		public virtual string ExcludedReason
		{ get { return m_ExcludedReason; } set { m_ExcludedReason = value; } }
		/// <summary>
		/// See <see cref="IPAFTestElementInfo"/>
		/// </summary>
		/// <remarks>
		/// Virtual for GUI subclassing.
		/// </remarks>
		public virtual string IgnoredReason
		{ get { return m_IgnoredReason; } set { m_IgnoredReason = value; }}
	    /// <summary>
	    /// See <see cref="IPAFTestElementInfo"/>
	    /// </summary>
	    /// <remarks>
	    /// Virtual for GUI subclassing.
	    /// </remarks>
	    public virtual bool IsRunning
	    { get { return m_IsPipelinedObjectRunning.NullableObject; } set { m_IsPipelinedObjectRunning.NullableObject = value; } }
	    /// <summary>
	    /// See <see cref="IPAFTestElementInfo"/>
	    /// </summary>
	    /// <remarks>
	    /// Virtual for GUI subclassing.
	    /// </remarks>
	    public virtual bool IsElevatedTrust
	    { get { return m_IsElevatedTrust; } set { m_IsElevatedTrust = value; } }
		/// <summary>
        /// See <see cref="IPAFTestElementInfo"/>
        /// </summary>
        /// <remarks>
        /// Virtual for remote/local peer.
        /// </remarks>
        public virtual string TestElementName
		{
			get
			{
				if (string.IsNullOrEmpty(m_Name))
					return "";
				return m_Name;
			}
			set { m_Name = value; } }
		/// <summary>
		/// See <see cref="IPAFTestElementInfo"/>
		/// </summary>
		/// <remarks>
		/// Virtual for remote/local peer.
		/// </remarks>
		public virtual IPAFTestElementInfo Parent
		{ get { return m_Parent; } protected internal set { m_Parent = value; } }

		/// <summary>
		/// See <see cref="IPAFTestElementInfo"/>
		/// </summary>
		/// <remarks>
		/// In this implementation, we generate the Generic by reflection
		/// but store it in the non-Generic interface. This means it is always
		/// castable, since we only make it here!
		/// </remarks>
		public virtual IPAFTestElementResultInfo TestElementResultInfo
		{
			get
			{
				lock (m_TestElementResultInfoConstructionLock)
				{
					if (m_TestElementResultInfo != null)
						return m_TestElementResultInfo;

					var unclosedGenericType = typeof(PAFTestElementResultInfo<>);
					Type[] closingType = {GetType()};
					var closedGenericType = unclosedGenericType.MakeGenericType(closingType);
					object instanceOfClosedGenericType = Activator.CreateInstance(closedGenericType);

					m_TestElementResultInfo
						= (IPAFTestElementResultInfo) instanceOfClosedGenericType;
					m_TestElementResultInfo.ElementInfo = this;
					return m_TestElementResultInfo;
				}
			}
		}

		/// <summary>
		/// See <see cref="IPAFTestElementInfo"/>
		/// </summary>
		/// <remarks>
		/// Virtual for remote/local peer.
		/// </remarks>
		public virtual bool? Passed
		{
			get
			{
				return m_Passed.NullableObject;
			}
			set
			{
				if (value.HasValue)
				{
					m_Passed.NullableObject = value.Value;
					return;
				}
				m_Passed.NullTheObject();
			}
		}
		#endregion Properties
		#region Methods
		/// <summary>
		/// See <see cref="IPAFTestElementInfo"/>
		/// </summary>
		/// <param name="exception">
		/// See <see cref="IPAFTestElementInfo"/>
		/// </param>
		public virtual void AddTestException(Exception exception)
		{
			lock(m_Exceptions)
			{
				m_Exceptions.Add(exception);
			}
		}

		/// <summary>
		/// See <see cref="IPAFTestElementInfo"/>
		/// </summary>
		public virtual IList<IPAFTestElementInfo> GetElementsToDisplay()
		{
			return GetDisplayChildElements(this);
		}

		/// <summary>
		/// See <see cref="IPAFTestElementInfo"/>
		/// </summary>
		/// <param name="testElementInfo">
		/// See <see cref="IPAFTestElementInfo"/>
		/// </param>
		public virtual void AddTestElement(IPAFTestElementInfo testElementInfo)
		{
			NonVirtualAddTestElement(testElementInfo);
		}
		#region Disposal Methods
		#region IDisposable Implementation
		/// <summary>
		/// <para>
		/// This method is not virtual. The developer of any subclass must not be
		/// allowed to change the logic. This method is NOT marked as
		/// <see cref="SecurityCriticalAttribute"/>, so it can be called in
		/// low-trust environments. We don't need to protect a test framework.
		/// </para>
		/// </summary>
		public void Dispose()
		{
			m_PAFDisposer.Dispose();
		}
		#endregion // IDisposable Implementation
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
		/// in an <see cref="PAFStandardException{T}"/>.
		/// </para>
		/// </remarks>
		protected virtual Exception TestElementInfoDispose(bool disposing, object obj)
		{
			var eList = new List<Exception>();

			// Dispose fields that are IDisposable.
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_ElementStatus, true));
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_HasPipelinedObjectRun, true));
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_IsExePipelineInitialized, true));
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_IsExePipelineUninitialized, true));
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_IsPipelinedObjectRunning, true));
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_Passed, true));

			// If we have any exceptions, put them in an aggregator.
			if (eList.Count > 0) {
				var exceptions = new PAFAggregateExceptionData(eList);
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
        #region Implementation of IPAFBaseExePipeline
        #region Implementation of IPAFBaseExePipelineInitialize
        /// <summary>
        /// See <see cref="IPAFBaseExePipelineInitialize{T}"/>.
        /// </summary>
        public virtual bool IsExePipelineInitialized
		{
			get { return m_IsExePipelineInitialized.NullableObject; }
			protected internal set { m_IsExePipelineInitialized.NullableObject = value; }
		}
        /// <summary>
        /// See <see cref="IPAFBaseExePipelineInitialize{T}"/>.
        /// </summary>
        /// <remarks>
        /// Params are not used in this class. Always return <see langword="null"/>.
        /// </remarks>
        public virtual IPAFPipelineParams<IPAFServiceManager<IPAFService>> PipelineParams { get; protected internal set; }
        /// <summary>
        /// See <see cref="IPAFBaseExePipelineInitialize{T}"/>. This default implementation
        /// just initializes all children, then sets <see cref="IsExePipelineInitialized"/>
        /// to <see langword="true"/>.
        /// </summary>
        /// <param name="provider">
        /// Access to service provider. If this is not <see langword="null"/>,
        /// all children are initialized with it.
        /// </param>
        public virtual void InitializeExePipeline(IPAFProviderPattern<IPAFPipelineParams<IPAFServiceManager<IPAFService>>> provider)
        {
            if (IsExePipelineInitialized) return;
			foreach (var child in AllChildren)
			{
			    if (provider != null)
			    {
			        child.InitializeExePipeline(provider);
			    }
			}
			IsExePipelineInitialized = true;
		}
        #endregion // Implementation of IPAFBaseExePipelineInitialize
        /// <summary>
        /// See <see cref="IPAFBaseExePipeline{T}"/>.
        /// </summary>
        public virtual bool HasPipelinedObjectRun
		{
			get { return m_HasPipelinedObjectRun.NullableObject; }
			protected internal set { m_HasPipelinedObjectRun.NullableObject = value; }
		}
        /// <summary>
        /// See <see cref="IPAFBaseExePipeline{T}"/>.
        /// </summary>
        public virtual bool IsExePipelineUninitialized
		{
			get { return m_IsExePipelineUninitialized.NullableObject; }
			protected internal set{m_IsExePipelineUninitialized.NullableObject = value;}
		}
        /// <summary>
        /// See <see cref="IPAFBaseExePipeline{T}"/>. This default implementation
        /// just checks all children and if any are running, it declares that
        /// we are running.
        /// </summary>
        public virtual bool IsPipelinedObjectRunning
		{
			get
			{
				foreach(var child in AllChildren)
				{
					if (child.IsPipelinedObjectRunning)
					{
						IsPipelinedObjectRunning = true;
						return true;
					}
				}
				IsPipelinedObjectRunning = false;
				return false;
			}
			// We don't need the set method ourselves, but it's here for
			// subclasses that want to implement notify proerty changed
			// or some thing similar.
			protected internal set { m_IsPipelinedObjectRunning.NullableObject = value; }
		}
		/// <summary>
		/// Base implementation runs children.
		/// </summary>
		/// <param name="obj">Passed to children.</param>
		public virtual void RunPipelinedObject(object obj)
		{
			foreach (var child in AllChildren) {
				child.RunPipelinedObject(obj);
			}
		}
		/// <summary>
		/// Override this in a concrete class. This one just sets
		/// <see cref="IsExePipelineUninitialized"/> to <see langword="true"/>.
		/// </summary>
		public virtual void UninitializeExePipeline()
		{
			IsExePipelineUninitialized = true;
		}
		#endregion // Implementation of IPAFBaseExePipeline
		/// <summary>
		/// Adds an element to the node. Non-virtual version.
		/// </summary>
		/// <param name="testElementInfo">
		/// Element info to add.
		/// </param>
		public void NonVirtualAddTestElement(IPAFTestElementInfo testElementInfo)
		{
			lock (m_AllChildren)
			{
				m_AllChildren.Add(testElementInfo);
			}
		}
        #endregion // Methods
        #region Implementation of IPAFClassProviderPattern
        /// <summary>
        /// This implementation climbs the hierarchy and checks up the tree to determine if
        /// someone has loaded a specific service manager. It returns the static manager if not.
        /// </summary>
        public virtual IPAFServiceManager<IPAFService> ProvidedItem
        {
            get
            {
                IPAFTestElementInfo info = this;
                do
                {
                    if (info.LocalServiceManager != null)
                        return info.LocalServiceManager;
                } while ((info = info.Parent) != null);

                return PAFServices.Manager;
            }
        }
        #endregion // Implementation of IPAFClassProviderPattern
	    #region Implementation of IPAFProviderPattern<IPAFPipelineParams<IPAFServiceManager<IPAFService>>>
	    ////////////////////////////////////////////////////////////////////////////////////////////////////
	    // Explicit because of name conflicts.
	    ////////////////////////////////////////////////////////////////////////////////////////////////////
	    /// <remarks>
	    /// See interface
	    /// </remarks>
	    IPAFPipelineParams<IPAFServiceManager<IPAFService>> IPAFProviderPattern<IPAFPipelineParams<IPAFServiceManager<IPAFService>>>.ProvidedItem
	    {
	        get { return GetProvidePipelineParamsPV(); }
	    }

	    /// <remarks>
	    /// See interface
	    /// </remarks>
	    bool IPAFProviderPattern<IPAFPipelineParams<IPAFServiceManager<IPAFService>>>.TryGetProvidedItem
	        (out IPAFPipelineParams<IPAFServiceManager<IPAFService>> item)
	    {
	        item = GetProvidePipelineParamsPV();
	        return item != null;
	    }
	    /// <summary>
	    /// Little helper that does the work.
	    /// </summary>
	    /// <returns>The params.</returns>
	    protected virtual IPAFPipelineParams<IPAFServiceManager<IPAFService>> GetProvidePipelineParamsPV()
	    {
	        return m_ProvidedPipelineParams.ProvidedItem;
	    }
	    #endregion // Implementation of IPAFProviderPattern<IPAFPipelineParams<IPAFServiceManager<IPAFService>>>
	}
}
