//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2018 Icucom Corporation
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

using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods.Helpers;

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// This interface handles basic services within the PAF system. It is intended
	/// to be used to hold framework services such as file service, resource service
	/// and other basic services. This is the internal version for core framework
	/// extenders. The implementations of the properties and methods here should not
	/// be marked as <see cref="SecurityCriticalAttribute"/>. These are essentially a
	/// copy of methods in <see cref="IPAFServiceManagerExtended{T}"/> with internal
	/// visibility. Framework extenders can use the <see cref="InternalsVisibleToAttribute"/>
	/// to expose these members to their extensions.
	/// </summary>
	/// <history>
	/// <author> DAP </author>
	/// <date> 05jan2012 </date>
	/// <contribution>
	/// Rebuilt/ReDOC'ed for the SL model.
	/// </contribution>
	/// </history>
	// ReSharper disable once PartialTypeWithSinglePart
	// Core removes any late instantiations of interfaces.
	internal partial interface IPAFServiceManagerInternal<in T> : IPAFServiceManagerExtended<T>
		where T : class, IPAFService
	{
		#region Properties
		/// <summary>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </summary>
		IEnumerable<IPAFServiceDescription> ServiceArrayInternal
		{
			get;
		}
		#endregion
		#region Methods
		/// <summary>
		/// Re-added 10oct2018 to support Golea. Doesn't need
		/// any public version and we don't want one. This is to support
		/// a legacy application.
		/// </summary>
		/// <param name="iFservice">
		/// The service to be added. If there is already one in the manager,
		/// it is first removed. The preferred way to upgrade services is
		/// now through the use of <see cref="IPAFEmergencyServiceProvider{T}"/>,
		/// not by replacing the old service.
		/// </param>
		/// <returns>
		/// <see langword="false"/> if service not already in dictionary.
		/// </returns>
		bool AddOrReplaceServiceInternal<U>(IPAFServiceDescription<U> iFservice)
			where U: class, T;
		/// <summary>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </summary>
		/// <param name="iFservice">
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </param>
		/// <remarks>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </remarks>
		/// <exception>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </exception>
		void AddServiceInternal<U>(IPAFServiceDescription<U> iFservice)
			where U: class,T;
		/// <summary>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </summary>
		/// <param name="iFservices">
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </param>
		void AddServicesInternal<U>(IEnumerable<IPAFServiceDescription<U>> iFservices)
			where U: class, T;
		/// <summary>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </summary>
		/// <param name="servicePipelineObject">
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </param>
		/// <param name="serviceDescription">
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </param>
		/// <param name="typeFilter">
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </param>
		/// <param name="localServiceInstantiator">
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </param>
		/// <returns>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </returns>
		IPAFServiceDescription<U> CreateServiceInternal<U>(
			IPAFServicePipelineObject<IPAFService> servicePipelineObject,
			IPAFServiceDescription<U> serviceDescription,
			IPAFTypeFilter typeFilter,
			PAFLocalServiceInstantiator<U> localServiceInstantiator)
			where U: class,T;
		/// <summary>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </summary>
		/// <returns>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </returns>
		IEnumerable<IPAFServiceDescription> GetServiceDescriptionsInternal<U>()
			where U: class, T;

		/// <summary>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </summary>
		/// <param name="servicePipelineObject">
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </param>
		/// <param name="serviceDescription">
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </param>
		/// <param name="typeFilter">
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </param>
		/// <returns>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </returns>
		IPAFServiceDescription<U> InstantiateLocalServiceInternal<U>(
			IPAFServicePipelineObject<IPAFService> servicePipelineObject,
			IPAFServiceDescription serviceDescription, 
			IPAFTypeFilter typeFilter)
			where U : class, T;
		/// <summary>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </summary>
		/// <param name="iFservice">
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </param>
		/// <exceptions>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </exceptions>
		void MakeServiceDefaultForInterfaceInternal<U>(
			IPAFServiceDescription<U> iFservice) where U: class, T;
		/// <summary>
		/// Re-added 10oct2018 to support Golea. Doesn't need
		/// any public version and we don't want one. This is to support
		/// a legacy application.
		/// </summary>
		/// <param name="iFservice">
		/// The service to be removed.
		/// </param>
		/// <returns>
		/// <see langword="false"/> if service not in dictionary.
		/// </returns>
		bool RemoveServiceInternal<U>(IPAFServiceDescription<U> iFservice)
			where U : class, T;
		#endregion
	}
}
