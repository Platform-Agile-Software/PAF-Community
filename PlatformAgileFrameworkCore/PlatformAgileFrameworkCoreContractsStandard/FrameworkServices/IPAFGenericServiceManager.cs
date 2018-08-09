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
using System.Security;

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// This interface handles basic services within the PAF system.
	/// </summary>
	/// <typeparam name="T">
	/// The type is constrained to be a <see cref="IPAFService"/>
	/// </typeparam>
	/// <remarks>
	/// <para>
	/// This is the interface exposed to the outside world to enable request
	/// of services. It is normally placed in a separate interface assembly.
	/// Note: We usually don't use variable arg lists, since we want implementors
	/// to use explicit interface implementation if they need to.
	/// </para>
	/// <para>
	/// As is the case in the non-Genric interface, we NEVER expose implementation
	/// classes, but allow service lookup by interface type and service name only.
	/// This is the SOA approach.
	/// </para>
	/// </remarks>
	public interface IPAFServiceManager<in T> :IPAFServiceManager
		where T : class, IPAFService
	{
		#region Methods
		/// <summary>
		/// Method allows privileged clients to add services on the fly.
		/// </summary>
		/// <typeparam name="U">Type of the service</typeparam>
		/// <param name="service">The service</param>
		/// <param name="serviceName">Name of the service, which is usually not "".</param>
		/// <param name="isDefaultService"><see langword="true"/> if this is to become the
		/// "default" service.</param>
		//[SecurityCritical]
		void AddTypedService<U>(U service, string serviceName, bool isDefaultService)
			 where U : class, T;
		/// <summary>
		/// This method requests a specific service by its service interface type.
		/// </summary>
		/// <typeparam name="U">
		/// The <typeparamref name="U"/> is the type of an interface, which is what
		/// is exposed to a requesting client.
		/// </typeparam>
		/// <param name="serviceName">
		/// Optional service name. When this parameter is <see langword="null"/>,
		/// a default service must always be supplied. It is up to the implementor
		/// to describe the named services available.
		/// </param>
		/// <param name="exactTypeMatch">
		/// If set to <see langword="true"/>, derived Types will not be returned.
		/// Default = <see langword="true"/>.
		/// </param>
		/// <param name="clientObject">
		/// Requestor info for the request. This object can be used in low-trust
		/// environments to allow callers to do things that otherwise might
		/// not be possible. In the addin framework, this is the addin's security
		/// token provider. Not all implementations require its use, but it's a good idea
		/// to pass "this" or the <see cref="Type"/> of a static call if the 
		/// application is later retrofitted with security.
		/// </param>
		/// <returns>
		/// The found <see cref="IPAFService"/> or <see langword="null"/>.
		/// </returns>
		/// <remarks>
		/// <para>
		/// If a service is an extended service, the service
		/// is only returned if it is initialized.
		/// </para>
		/// <para>
		/// This is the one you have to call for named services.
		/// </para>
		/// </remarks>
		U GetTypedService<U>(string serviceName, bool exactTypeMatch,
			object clientObject) where U :class, T;
		/// <remarks>
		/// See <see cref="GetTypedService{U}(String, Boolean, Object)"/>.
		/// "serviceName" is <see langword="null"/>
		/// </remarks>
		U GetTypedService<U>(bool exactTypeMatch, object clientObject)
			where U: class, T;
		/// <remarks>
		/// See <see cref="GetTypedService{U}(Boolean, Object)"/>.
		/// "exactTypeMatch" is <see langword="true"/>
		/// </remarks>
		U GetTypedService<U>(object clientObject)
			where U: class, T;
		/// <remarks>
		/// See <see cref="GetTypedService{U}(Object)"/>.
		/// "clientObject" is <see langword="null"/>.
		/// </remarks>
		U GetTypedService<U>()
			where U: class, T;
		#endregion
	}
}
