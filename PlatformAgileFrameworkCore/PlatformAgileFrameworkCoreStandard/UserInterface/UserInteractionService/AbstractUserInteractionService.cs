using System;
using System.Collections.Generic;
using System.Security;
using PlatformAgileFramework.Collections.ExtensionMethods;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.TypeHandling.Disposal;
using PlatformAgileFramework.UserInterface.Interfaces;

namespace PlatformAgileFramework.UserInterface.UserInteractionService
{
	/// <summary>
	/// This class is a basic implementation of the <see cref="IPAFUIService"/>
	/// interface for use through inheritance. This implementation acts as an aggregator
	/// for the multiple interfaces that must be returned by <see cref="IPAFUIService"/>.
	/// </summary>
	public abstract class AbstractUserInteractionService : PAFService,
		IPAFUIServiceInternal
	{
		#region Class Fields And Autoproperties
		///////////////////////////////////////////////////////////////////////////////////
		// The mandatory services.
		///////////////////////////////////////////////////////////////////////////////////
		/// <remarks/>
		protected IErrorMessageAndDispatch m_IMessageAndDispatch;
        /// <remarks/>
        protected IMessageAndStringResponse m_IMessageAndStringResponse;
        /// <remarks/>
        protected IMessageListAndStringResponse m_IMessageListAndStringResponse;
		/// <remarks/>
		protected IMessageWithNoResponse m_IMessageWithNoResponse;
		/// <remarks/>
		protected IMessageOkQuit m_IMessageOkQuit;
		/// <remarks/>
		protected IYesNoQuery m_IYesNoQuery;
		#endregion // Class Fields And Autoproperties
		#region Constructors
		/// <summary>
		/// Constructor accepts Guid, the name and type. Direct pass-through to base.
		/// </summary>
		[SecurityCritical]
		protected AbstractUserInteractionService(Type serviceImplementationType = null,
			string serviceName = null, Guid guid = default(Guid))
			: base(serviceImplementationType, serviceName, guid)
		{}
		#endregion // Constructors
		#region IPAFUIServiceInternal Implementation
		#region IPAFUIService Implementation
		/// <summary>
		/// See <see cref="IPAFUIService"/>.
		/// </summary>
		/// <returns>
		/// See <see cref="IPAFUIService"/>.
		/// </returns>
		public virtual IErrorMessageAndDispatch GetErrorMessageAndDispatch()
		{ return (m_IMessageAndDispatch); }

        /// <summary>
        /// See <see cref="IPAFUIService"/>.
        /// </summary>
        /// <returns>
        /// See <see cref="IPAFUIService"/>.
        /// </returns>
        public virtual IMessageAndStringResponse GetMessageAndStringResponse()
        { return (m_IMessageAndStringResponse); }

        /// <summary>
        /// See <see cref="IPAFUIService"/>.
        /// </summary>
        /// <returns>
        /// See <see cref="IPAFUIService"/>.
        /// </returns>
        public virtual IMessageListAndStringResponse GetMessageListAndStringResponse()
        { return (m_IMessageListAndStringResponse); }

		/// <summary>
		/// See <see cref="IPAFUIService"/>.
		/// </summary>
		/// <returns>
		/// See <see cref="IPAFUIService"/>.
		/// </returns>
		public virtual IMessageWithNoResponse GetMessageWithNoResponse()
		{ return (m_IMessageWithNoResponse); }

		/// <summary>
		/// See <see cref="IPAFUIService"/>.
		/// </summary>
		/// <returns>
		/// See <see cref="IPAFUIService"/>.
		/// </returns>
		public virtual IMessageOkQuit GetMessageOkQuit()
		{ return (m_IMessageOkQuit); }

		/// <summary>
		/// See <see cref="IPAFUIService"/>.
		/// </summary>
		/// <returns>
		/// See <see cref="IPAFUIService"/>.
		/// </returns>
		public virtual IYesNoQuery GetYesNoQuery()
		{ return m_IYesNoQuery; }
		#region Secure Methods
		/// <summary>
		/// See <see cref="IPAFUIService"/>.
		/// </summary>
		/// <returns>
		/// See <see cref="IPAFUIService"/>.
		/// </returns>
		[SecurityCritical]
		public virtual IStringIO GetStringIOProvider()
		{ return GetStringIOProviderPIV(); }
		/// <summary>
		/// See <see cref="IPAFUIService"/>.
		/// </summary>
		/// <returns>
		/// See <see cref="IPAFUIService"/>.
		/// </returns>
		[SecurityCritical]
		public virtual void SetStringIOProvider(IStringIO stringIO)
		{ SetStringIOProviderPIV(stringIO); }
		#endregion // Secure Methods
		#endregion // IPAFUIService Implementation
		/// <summary>
		/// See <see cref="IPAFUIServiceInternal"/>.
		/// </summary>
		/// <returns>
		/// See <see cref="IPAFUIServiceInternal"/>.
		/// </returns>
		IStringIO IPAFUIServiceInternal.GetStringIOProviderInternal()
		{ return GetStringIOProviderPIV(); }
		/// <remarks/>
		protected abstract IStringIO GetStringIOProviderPIV();
		/// <summary>
		/// See <see cref="IPAFUIServiceInternal"/>.
		/// </summary>
		/// <returns>
		/// See <see cref="IPAFUIServiceInternal"/>.
		/// </returns>
		void IPAFUIServiceInternal.SetStringIOProviderInternal(IStringIO stringIO)
		{ SetStringIOProviderPIV(stringIO); }
		/// <remarks/>
		protected abstract void SetStringIOProviderPIV(IStringIO stringIO);
		#endregion // IPAFUIServiceInternal Implementation
		#region IDisposable implementation
		/// <summary>
		/// <see cref="IPAFDisposable"/>. This override disposes our contained
		/// services, then calls base.
		/// </summary>
		/// <param name="disposing">
		/// <see cref="IPAFDisposable"/>.
		/// </param>
		/// <param name="obj">
		/// <see cref="IPAFDisposable"/>.
		/// This is not used in this method.
		/// </param>
		/// <returns>
		/// <see cref="IPAFDisposable"/>.
		/// </returns>
		/// <remarks>
		/// <para>
		/// Exceptions are caught and recorded in the registry.
		/// </para>
		/// </remarks>
		protected override Exception PAFFrameworkServiceDispose(bool disposing, object obj)
		{
			Exception retval = null;
			var eList = new List<Exception>();
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_IMessageAndDispatch, true));
            eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_IMessageAndStringResponse, true));
            eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_IMessageListAndStringResponse, true));
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_IMessageWithNoResponse, true));
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_IMessageOkQuit, true));
			eList.AddNoNulls(PAFDisposalUtils.Disposer(ref m_IYesNoQuery, true));

			// If we have any exceptions, put them in an aggregator.
			if (eList.Count > 0) {
				var exceptions = new PAFAggregateExceptionData(eList);
				var ex = new PAFStandardException<PAFAggregateExceptionData>(exceptions);
				// Seal the list.
				exceptions.AddException(null);
				// We just put these in the registry.
				DisposalRegistry.RecordDisposalException(GetType(), ex);
				retval = ex;
			}
			return retval;
		}
		#endregion // IDisposable implementation
	}
}
