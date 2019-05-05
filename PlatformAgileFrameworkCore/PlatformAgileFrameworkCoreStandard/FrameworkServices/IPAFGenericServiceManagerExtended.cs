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
using PlatformAgileFramework.ErrorAndException.CoreCustomExceptions;
using PlatformAgileFramework.FrameworkServices.Exceptions;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods.Helpers;

namespace PlatformAgileFramework.FrameworkServices
{
	#region Delegates
	/// <summary>
	/// Generic version.
	/// </summary>
	/// <param name="servicePipelineObject">
	/// See non-Generic version.
	/// </param>
	/// <param name="serviceDescription">
	/// See non-Generic version.
	/// </param>
	/// <param name="typeFilter">
	/// See non-Generic version.
	/// </param>
	/// <param name="localServiceInstantiator">
	/// See non-Generic version.
	/// </param>
	/// <returns>
	/// See non-Generic version.
	/// </returns>
	public delegate IPAFServiceDescription PAFServiceCreator<T>(
		IPAFServicePipelineObject<IPAFService> servicePipelineObject,
		IPAFServiceDescription serviceDescription,
		IPAFTypeFilter typeFilter = null,
		PAFLocalServiceInstantiator<T> localServiceInstantiator = null)
		where T: class, IPAFService;

	/// <summary>
	/// Generic version.
	/// </summary>
	/// <param name="servicePipelineObject">
	/// See non-Generic version.
	/// </param>
	/// <param name="serviceDescription">
	/// See non-Generic version.
	/// </param>
	/// <param name="typeFilter">
	/// See non-Generic version.
	/// </param>
	/// <param name="assemblyList">
	/// See non-Generic version.
	/// </param>
	/// <returns>
	/// See non-Generic version.
	/// </returns>
	public delegate IPAFServiceDescription<T> PAFLocalServiceInstantiator<T>(
		IPAFServicePipelineObject<IPAFService> servicePipelineObject,
		IPAFServiceDescription<T> serviceDescription, IPAFTypeFilter typeFilter = null,
		IEnumerable<Assembly> assemblyList = null) where T: class, IPAFService;
	#endregion // Delegates

	/// <summary>
	/// This interface handles basic services within the PAF system. It is intended
	/// to be used to hold framework services such as file service, resource service
	/// and other basic services. The generic version provides some type-safety.
	/// </summary>
	/// <history>
	/// <author> DAP </author>
	/// <date> 04jan2012 </date>
	/// <contribution>
	/// Changed to a public interface. 4.0 security allows us to expose it
	/// and simply leave the methods security critical.
	/// </contribution>
	/// </history>
// ReSharper disable once PartialTypeWithSinglePart
	// Core removes any late instantiations of interfaces.
	public partial interface IPAFServiceManagerExtended<in T>
		:IPAFServiceManager<T>
		where T : class, IPAFService
	{
		#region Properties
		/// <summary>
		/// Just an accessor for the service list. This array is a ThreadSafe collection
		/// that is generated in the current sort order of the service manager dictionary.
		/// It represents the internal collection of services at the moment the Property
		/// is requested. The dictionary is NOT locked during the use of the array
		/// obtained from this call.
		/// </summary>
		IEnumerable<IPAFServiceDescription> ServiceArray
		{
			[SecurityCritical]get;
		}
		#endregion
		#region Methods
		/// <summary>
		/// Adds a <typeparamref name="U"/> to the list of available services.
		/// </summary>
		/// <typeparam name="U">Any service derived from <typeparamref name="T"/>.</typeparam>
		/// <param name="iFservice">
		/// A valid service description.
		/// </param>
		/// <remarks>
		/// Checks to see if it is a pure interface type and throws an exception if it is.
		/// </remarks>
		/// <exception>
		/// An exception must be thrown if an attempt is made to add a pure interface.
		/// A <see cref="PAFStandardException{IPAFConstructorExceptionData}"/> is thrown with a
		/// <see cref="PAFConstructorExceptionMessageTags.ATTEMPT_TO_INSTANTIATE_PURE_INTERFACE"/> message.
		/// </exception>
		[SecurityCritical]
		void AddService<U>(IPAFServiceDescription<U> iFservice)
			where U : class, T;
		/// <summary>s
		/// Adds a group of <typeparamref name="U"/>s.
		/// </summary>
		/// <typeparam name="U">Any service derived from <typeparamref name="T"/>.</typeparam>
		/// <param name="iFservices">
		/// An enumeration of valid service description.
		/// </param>
		[SecurityCritical]
		void AddServices<U>(IEnumerable<IPAFServiceDescription<U>> iFservices)
			where U : class, T;

		/// <summary>
		/// See <see cref="PAFServiceCreator{U}"/>.
		/// </summary>
		/// <typeparam name="U">Any service derived from <typeparamref name="T"/>.</typeparam>
		/// <param name="servicePipelineObject">
		/// See <see cref="PAFServiceCreator{U}"/>.
		/// </param>
		/// <param name="serviceDescription">
		/// See <see cref="PAFServiceCreator{U}"/>.
		/// </param>
		/// <param name="localServiceInstantiator">
		/// See <see cref="PAFServiceCreator{U}"/>.
		/// </param>
		/// <param name="typeFilter">
		/// See <see cref="PAFServiceCreator{U}"/>.
		/// </param>
		/// <returns>
		/// See <see cref="PAFServiceCreator{U}"/>.
		/// </returns>
		[SecurityCritical]
		IPAFServiceDescription<U> CreateService<U>(
			IPAFServicePipelineObject<IPAFService> servicePipelineObject,
			IPAFServiceDescription<U> serviceDescription,
			IPAFTypeFilter typeFilter,
			PAFLocalServiceInstantiator<U> localServiceInstantiator)
			where U : class, T;
		/// <summary>
		/// This method requests a class of services by its type. All loaded services
		/// that are registered by the type <typeparamref name="U"/> are returned.
		/// </summary>
		/// <typeparam name="U">Any service derived from <typeparamref name="T"/>.</typeparam>
		/// <returns>
		/// The found <see cref="IPAFServiceDescription"/>s matching the type
		/// <typeparamref name="U"/>.
		/// </returns>
		[SecurityCritical]
		IEnumerable<IPAFServiceDescription> GetServiceDescriptions<U>()
			where U: class, T;
		/// <summary>
		/// Creates a service from loaded assemblies.
		/// </summary>
		/// <typeparam name="U">Any service derived from <typeparamref name="T"/>.</typeparam>
		/// <param name="servicePipelineObject">
		/// Callees need <see cref="IPAFServicePipelineObject.ServiceManager"/>.
		/// </param>
		/// <param name="serviceDescription">
		/// A proper service description for the context under which the service is being
		/// created. See details on <see cref="IPAFServiceDescription{U}"/>. If this
		/// service is already constructed this method does nothing.
		/// </param>
		/// <param name="typeFilter">
		/// Optional filter on the type.
		/// </param>
		/// <returns>
		/// An instantiated, but not loaded or initialized <see cref="IPAFService"/>.
		/// If service is already constructed, the service is simply returned.
		/// </returns>
		[SecurityCritical]
		IPAFServiceDescription<U> InstantiateLocalService<U>(
			IPAFServicePipelineObject<IPAFService> servicePipelineObject,
			IPAFServiceDescription serviceDescription, IPAFTypeFilter typeFilter)
			where U : class, T;
		/// <summary>
		/// This method modifies the default DEFAULT lookup procedure by allowing
		/// an interface type to have a service object registered as it's default
		/// implementor. Normally, the service implementing an interface with a
		/// BLANK name is the default. Usual procedure is for a caller to grab a
		/// service from the manager and use it here as an argument. This method
		/// will cause the "IsDefault" property on the service to be set.
		/// </summary>
		/// <typeparam name="U">Any service derived from <typeparamref name="T"/>.</typeparam>
		/// <param name="iFservice">
		/// The specific service to be registered as the default. This service
		/// must already be present in the dictionary of services and be constructed.
		/// </param>
		/// <exceptions>
		/// <exception cref="PAFStandardException{IPAFSED}"> is thrown if the
		/// <paramref name="iFservice"/> is not found.
		/// <see cref="PAFServiceExceptionMessageTags.SERVICE_NOT_FOUND"/>.
		/// </exception>
		/// </exceptions>
		[SecurityCritical]
		void MakeServiceDefaultForInterface<U>(IPAFServiceDescription<U> iFservice)
			where U: class, T;

		#endregion
	}
}
