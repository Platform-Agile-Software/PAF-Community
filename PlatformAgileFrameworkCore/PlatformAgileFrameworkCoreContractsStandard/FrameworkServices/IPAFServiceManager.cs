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

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// This interface handles basic services within the PAF system. It is intended
	/// to be used to hold framework services such as file service, resource service
	/// and other basic services. This is the base non-Generic interface. This is here
	/// to support legacy customer stuff.
	/// </summary>
	// ReSharper disable once PartialTypeWithSinglePart
	// Core removes any late instantiations of interfaces and addition of services.
	public partial interface IPAFServiceManager : IPAFService
	{
		/// <summary>
		/// This method requests a specific service by its type.
		/// </summary>
		/// <param name="serviceType">
		/// The <paramref name="serviceType"/> can be a top-level type or a base
		/// type or an Interface. The set of installed services is first checked
		/// for an exact type match. If it finds a match, this is the service
		/// that is returned. If it does not find a match, it searches for a type
		/// that inherits from the <paramref name="serviceType"/> and returns the
		/// first one found. This will always happen for a <paramref name="serviceType"/>
		/// that is passed as an interface. It will happen if the
		/// <paramref name="serviceType"/> is a base class and no instance of the base
		/// class is loaded, but a derived class is.
		/// </param>
		/// <param name="serviceName">
		/// Optional service name. When this parameter is <see langword="null"/>,
		/// a default service must always be supplied. It is up to the implementor
		/// to describe the named services available.
		/// </param>
		/// <param name="exactTypeMatch">
		/// If set to <see langword="true"/>, derived Types will not be returned.
		/// Default = <see langword="false"/>.
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
		/// Noted that the found <see cref="IPAFService"/> will depend on the
		/// order in which services are sorted in the event that more than one service meets
		/// the criterion of deriving from a given type.
		/// </para>
		/// <para>
		/// Only elevated-priviledge callers can request services by specific IMPLEMENTATION
		/// type. For other callers, <paramref name="serviceType"/> must be an interface type.
		/// Callers can also access secure resources through the <paramref name="clientObject"/>
		/// parameter.
		/// </para>
		/// <para>
		/// If a service is a <c>IPAFFrameworkServiceExtended</c>, the service
		/// is only returned if it is initialized.
		/// </para>
		/// </remarks>
		IPAFService GetService(Type serviceType, string serviceName, bool exactTypeMatch,
			object clientObject);
		/// <remarks>
		/// See <see cref="GetService(Type, String, Boolean, Object)"/>.
		/// "serviceName" is <see langword="null"/>
		/// </remarks>
		IPAFService GetService(Type serviceType, bool exactTypeMatch, object clientObject);
		/// <remarks>
		/// See <see cref="GetService(Type, Boolean, Object)"/>.
		/// "exactTypeMatch" is <see langword="false"/>
		/// </remarks>
		IPAFService GetService(Type serviceType, object clientObject);
		/// <remarks>
		/// See <see cref="GetService(Type, Object)"/>.
		/// "clientObject" is <see langword="null"/>.
		/// </remarks>
		IPAFService GetService(Type serviceType);
	}
}
