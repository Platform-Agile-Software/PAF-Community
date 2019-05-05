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
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System.Collections.Generic;
using System.Reflection;
using System.Security;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.FrameworkServices.Exceptions;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods.Helpers;

namespace PlatformAgileFramework.FrameworkServices
{
	#region Delegates
	/// <summary>
	/// This is a delegate used in creating a service, either remote or local.
	/// </summary>
	/// <param name="servicePipelineObject">
	/// The pipeline object that is needed for staged services.
	/// </param>
	/// <param name="serviceDescription">
	/// The description of the service to be created.
	/// </param>
	/// <param name="typeFilter">
	/// An optional type filter on service implementation types.
	/// Default = <see langword="null"/>.
	/// </param>
	/// <param name="localServiceInstantiator">
	/// Instantiator for local services. Default = <see langword="null"/>
	/// will cause an internal default to be used.
	/// </param>
	/// <returns>
	/// Never <see langword="null"/>.
	/// </returns>
	public delegate IPAFServiceDescription PAFServiceCreator(
		IPAFServicePipelineObject servicePipelineObject,
		IPAFServiceDescription serviceDescription,
		IPAFTypeFilter typeFilter = null,
		PAFLocalServiceInstantiator localServiceInstantiator = null);
	/// <summary>
	/// This is a delegate used in creating a local service from type information
	/// that is already loaded into the current "AppDomain".
	/// </summary>
	/// <param name="servicePipelineObject">
	/// The pipeline object that is needed for staged services.
	/// </param>
	/// <param name="serviceDescription">
	/// The description of the service interface and implementation.
	/// </param>
	/// <param name="typeFilter">
	/// An optional type filter on service implementation types.
	/// </param>
	/// <param name="assemblyList">
	/// Optional list of assemblies to constrain the search to. Default = <see langword="null"/>
	/// causes all assemblies in current "AppDomain" to be searched.
	/// </param>
	/// <returns>
	/// Service instance or <see langword="null"/>.
	/// </returns>
	public delegate IPAFServiceDescription PAFLocalServiceInstantiator(
		IPAFServicePipelineObject servicePipelineObject,
		IPAFServiceDescription serviceDescription, IPAFTypeFilter typeFilter = null,
		IEnumerable<Assembly> assemblyList = null);
	#endregion // Delegates
	/// <summary>
	/// This interface provides internal access to manager parent.
	/// </summary>
	// ReSharper disable once PartialTypeWithSinglePart
	// Addition of services moved to extended and to inheritors.
	public partial interface IPAFServiceManagerExtended : IPAFServiceManager
	{
		/// <summary>
		/// Creates a service from either loaded assemblies or a specific assembly
		/// in "available assemblies" or anywhere else we have a spec for.
		/// </summary>
		/// <param name="servicePipelineObject">
		/// Callees need <see cref="IPAFServicePipelineObject.ServiceManager"/>.
		/// </param>
		/// <param name="serviceDescription">
		/// The <see cref="IPAFServiceDescription"/> provides a description
		/// of the service to be created. Please see the documentation for this
		/// interface.
		/// </param>
		/// <param name="typeFilter">
		/// Optional filter on the created service.
		/// </param>
		/// <param name="localServiceInstantiator">
		/// Constructs services after their type information is verified as
		/// available in current "AppDomain".
		/// </param>
		/// <returns>
		/// A framework service. Never <see langword="null"/>.
		/// </returns>
		/// <exceptions>
		/// <exception cref="PAFStandardException{T}">
		/// <see cref="PAFServiceExceptionMessageTags.SERVICE_CREATION_FAILED"/>
		/// </exception>
		/// </exceptions>
		[SecurityCritical]
		IPAFServiceDescription CreateService(
			IPAFServicePipelineObject servicePipelineObject,
			IPAFServiceDescription serviceDescription,
			IPAFTypeFilter typeFilter,
			PAFLocalServiceInstantiator localServiceInstantiator);
		/// <summary>
		/// Provides access to a parent manager.
		/// </summary>
		/// <returns>
		/// See <see cref="IPAFServiceManager"/>.
		/// </returns>
		IPAFServiceManager ParentManager { get; [SecurityCritical]set; }


	}
}