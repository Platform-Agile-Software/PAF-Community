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
	internal partial interface IPAFServiceManagerInternal<T> : IPAFServiceManagerExtended<T>
		where T : class, IPAFService
	{
		#region Properties
		/// <summary>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </summary>
		IEnumerable<IPAFServiceDescription<T>> ServiceArrayInternal
		{
			get;
		}
		#endregion
		#region Methods
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
		void AddServiceInternal(IPAFServiceDescription<T> iFservice);
		/// <summary>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </summary>
		/// <param name="iFservices">
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </param>
		void AddServicesInternal(IEnumerable<IPAFServiceDescription<T>> iFservices);
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
		IPAFServiceDescription<T> CreateServiceInternal(
			IPAFServicePipelineObject<IPAFService> servicePipelineObject,
			IPAFServiceDescription<T> serviceDescription,
			IPAFTypeFilter typeFilter,
			PAFLocalServiceInstantiator<T> localServiceInstantiator);
		/// <summary>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </summary>
		/// <param name="exactTypeMatch">
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </param>
		/// <returns>
		/// See <see cref="IPAFServiceManagerExtended{T}"/>.
		/// </returns>
		IEnumerable<IPAFServiceDescription> GetServicesInternal<U>(bool exactTypeMatch)
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
		IPAFServiceDescription<T> InstantiateLocalServiceInternal(
			IPAFServicePipelineObject<IPAFService> servicePipelineObject,
			IPAFServiceDescription<T> serviceDescription, 
			IPAFTypeFilter typeFilter);
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
		#endregion
	}
}
