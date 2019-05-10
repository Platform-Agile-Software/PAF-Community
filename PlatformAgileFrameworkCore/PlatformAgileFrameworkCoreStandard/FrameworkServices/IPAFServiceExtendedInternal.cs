//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2019 Icucom Corporation
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
using System.Collections.Generic;
using PlatformAgileFramework.Remoting;
using PlatformAgileFramework.Security;

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// This one supports internal access to <see cref="IPAFServiceExtended"/>
	/// functionality.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> Brian T. </author>
	/// <date> 02jan2019 </date>
	/// <description>
	/// New. Created for the refactor of the "IsDefault" property.
	/// </description>
	/// </contribution>
	/// </history>
// ReSharper disable once PartialTypeWithSinglePart
	internal partial interface IPAFServiceExtendedInternal
		: IPAFServiceExtended
	{
		#region Properties
		/// <summary>
		/// Manipulates the LMO for remote services.
		/// </summary>
		IPAFLifetimeManagedObjectInternal<IPAFSecretKeyProvider> LifetimeManagedObjectInternal { get; set; }
		/// <summary>
		/// This property gets the manager associated with the service. It is often
		/// used to climb a tree of managers that are constructed hierarchically. This
		/// property can return <see langword="null"/> if the service has not yet
		/// been added to a service manager or if the information is not revealed
		/// by the service designer. This latter case is rare.
		/// </summary>
		IPAFServiceManager ServiceManagerInternal
		{ get; set; }
		/// <summary>
		/// Secures the service. Used in extended.
		/// </summary>
		IPAFSecretKey SecurityObjectInternal { get; }
		/// <summary>
		/// See the corresponding property in <see cref="IPAFServiceExtended"/>.
		/// </summary>
		IEnumerable<IPAFServiceDescription> ServicesRequiredForInitializationInternal { get; }
		/// <summary>
		/// See the corresponding property in <see cref="IPAFServiceExtended"/>.
		/// </summary>
		IEnumerable<IPAFServiceDescription> ServicesRequiredForLoadInternal { get; }
		#endregion // Properties
		#region Methods
		/// <summary>
		/// See the corresponding property in <see cref="IPAFServiceExtended"/>.
		/// </summary>
		/// <param name="serviceIsInitialized">
		/// See the corresponding property in <see cref="IPAFServiceExtended"/>.
		/// </param>
		void SetServiceIsInitialized(bool serviceIsInitialized);
		/// <summary>
		/// See the corresponding property in <see cref="IPAFServiceExtended"/>.
		/// </summary>
		/// <param name="serviceIsLoaded">
		/// See the corresponding property in <see cref="IPAFServiceExtended"/>.
		/// </param>
		void SetServiceIsLoaded(bool serviceIsLoaded);
		/// <summary>
		/// See the corresponding property in <see cref="IPAFServiceExtended"/>.
		/// </summary>
		/// <param name="serviceIsUninitialized">
		/// See the corresponding property in <see cref="IPAFServiceExtended"/>.
		/// </param>
		void SetServiceIsUninitialized(bool serviceIsUninitialized);
		/// <summary>
		/// See the corresponding property in <see cref="IPAFServiceExtended"/>.
		/// </summary>
		/// <param name="serviceIsUnloaded">
		/// See the corresponding property in <see cref="IPAFServiceExtended"/>.
		/// </param>
		void SetServiceIsUnloaded(bool serviceIsUnloaded);
		/// <summary>
		/// See the corresponding method in <see cref="IPAFServiceExtended"/>.
		/// </summary>
		/// <param name="pipelineObject">
		/// See the corresponding method in <see cref="IPAFServiceExtended"/>.
		/// </param>
		void InitializeServiceInternal(IPAFServicePipelineObject<IPAFService> pipelineObject);
		/// <summary>
		/// See the corresponding method in <see cref="IPAFServiceExtended"/>.
		/// </summary>
		/// <param name="pipelineObject">
		/// See the corresponding method in <see cref="IPAFServiceExtended"/>.
		/// </param>
		void LoadServiceInternal(IPAFServicePipelineObject<IPAFService> pipelineObject);
		/// <summary>
		/// See the corresponding method in <see cref="IPAFServiceExtended"/>.
		/// </summary>
		/// <param name="pipelineObject">
		/// See the corresponding method in <see cref="IPAFServiceExtended"/>.
		/// </param>
		void UninitializeServiceInternal(IPAFServicePipelineObject<IPAFService> pipelineObject);
		/// <summary>
		/// See the corresponding method in <see cref="IPAFServiceExtended"/>.
		/// </summary>
		/// <param name="pipelineObject">
		/// See the corresponding method in <see cref="IPAFServiceExtended"/>.
		/// </param>
		void UnloadServiceInternal(IPAFServicePipelineObject<IPAFService> pipelineObject);
		/// <summary>
		/// See the corresponding method in <see cref="IPAFServiceExtended"/>.
		/// </summary>
		/// <param name="pipelineObject">
		/// See the corresponding method in <see cref="IPAFServiceExtended"/>.
		/// </param>
		void UpdateServiceInternal(IPAFServicePipelineObject<IPAFService> pipelineObject);
		#endregion // Methods
	}
}
