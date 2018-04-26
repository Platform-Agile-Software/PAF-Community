//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2016 Icucom Corporation
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
using System.Security;
using System.Threading;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.FrameworkServices.Exceptions;
using PlatformAgileFramework.MultiProcessing.Threading.Delegates;
using PAFSEDB = PlatformAgileFramework.FrameworkServices.Exceptions.PAFServiceExceptionDataBase;

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// <para>
	///	Class providing a method with the same signature as <see cref="WaitCallback"/>
	/// wrapping a <see cref="PAFServicePipelineDelegate{T}"/>.
	/// </para>
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> BMC </author>
	/// <date> 22jan2012 </date>
	/// <description>
	/// Killed all the pseudo-delegate stuff in lieu of simple wrappers like
	/// these. We only need two of these in core. Pseudo-delegates moved to
	/// extended.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Not thread-safe. The method exposed by this class is designed to be
	/// called by a single thread.
	/// </threadsafety>
	public class CoreStandardServicePipelineDelegator<T> :
		ParameterizedThreadStartMethodDelegator<IPAFServicePipelineObject<T>>
		where T: class, IPAFService
	{
		#region Class Fields And Autoproperties
		/// <summary>
		/// The service that needs to be staged.
		/// </summary>
		protected IPAFServiceExtended PipelinedService { get; set; }
		#endregion // Class Fields And Autoproperties
		#region Constructors
		/// <summary>
		/// Builds with a service delegate.
		/// </summary>
		/// <param name="servicePipelineDelegate">
		/// The delegate.
		/// </param>
		/// <param name="pipelinedService">
		/// The service that needs to be staged.
		/// </param>
		public CoreStandardServicePipelineDelegator(
			PAFContravariantThreadMethod<IPAFServicePipelineObject<T>>
			servicePipelineDelegate,
			IPAFServiceExtended
			pipelinedService)
			:base(servicePipelineDelegate)
		{
			PipelinedService = pipelinedService;
		}
		#endregion // Constructors
		#region Methods
		/// <summary>
		/// This method checks the services required for either load or
		/// initialization and verifies that they are available. This method
		/// uses only the service interface to check for default services.
		/// Since this class is used only in the service setup stages,
		/// default services should be the ones sought.
		/// </summary>
		/// <param name="pipelineObject">
		/// Standard pipeline object carrying the manager, etc.. <see langword="null"/>
		/// or <see cref="IPAFServicePipelineObject.PipelineStage"/> = <see langword="null"/>
		/// returns <see langword="null"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if all is well and all services are available.
		/// If <see langword="false"/>, the <see cref="IPAFServicePipelineObject.ExecutionException"/>
		/// should be checked.
		/// </returns>
		[SecuritySafeCritical]
		public virtual bool FetchNeededServices(
			IPAFServicePipelineObject pipelineObject)
		{
			if (pipelineObject == null) throw new ArgumentNullException("pipelineObject");
			//if ((pipelineObject == null) || (pipelineObject.PipelineStage == null))
			//	return false;

			IEnumerable<IPAFServiceDescription> neededServices = null;
			if (pipelineObject.PipelineStage == ServicePipelineStage.LOAD)
				neededServices = PipelinedService.ServicesRequiredForLoad;
			else if (pipelineObject.PipelineStage == ServicePipelineStage.INITIALIZE)
				neededServices = PipelinedService.ServicesRequiredForInitialization;

			if (neededServices == null) return true;

			foreach (var svc in neededServices) {
				try {
					var interfaceType = svc.ServiceInterfaceType.ResolveType(true);
					if (pipelineObject.ServiceManager.GetService(interfaceType) == null)
						return false;
				}
				catch (Exception ex)
				{
					var sex = ex as PAFStandardException<IPAFServiceExceptionData>;
					// We let a service not found pass without report.
                    if ((sex == null) || (!sex.HasTag(PAFServiceExceptionMessageTags.SERVICE_NOT_FOUND)))
						pipelineObject.ExecutionException = ex;
					return false;
				}
			}
			return true;
		}
		#endregion // Methods
	}
}