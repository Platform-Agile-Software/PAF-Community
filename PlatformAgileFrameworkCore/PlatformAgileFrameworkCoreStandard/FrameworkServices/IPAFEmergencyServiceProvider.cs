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

using PlatformAgileFramework.Logging;

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// <para>
	///	Interface for a service that can provide various types of "emergency" services
	/// before it is initialized.
	/// </para>
	/// <para>
	/// This interface provides access to an emergency service that can be used in situations
	/// where the main service is not available. This can happen during application initialization
	/// when needed files are missing, etc., before the service manager is set up.
	/// </para>
	/// <para>
	/// Applications can be designed to look for a type wearing this interface, either statically
	/// linked or late bound. It is typically the service manager that will do this if it has trouble
	/// instantiating the normal service.
	/// </para>
	/// </summary>
	/// <typeparam name="T">
	/// Any implementation of <see cref="IPAFService"/>.
	/// </typeparam>
	public interface IPAFEmergencyServiceProvider<out T>: IPAFService where T : IPAFService
	{
		/// <summary>
		/// Returns the emergency service. This is present so clients can access the original
		/// emergency service, even after the main service is loaded. What is actually returned
		/// is completely implementation dependent. In a standard, simple delegation pattern,
		/// this property will always return a reference to the type instance implementing the
		/// <see cref="IPAFEmergencyServiceProvider{T}"/> and <see cref="MainService"/> will
		/// return a reference to a wrapped <typeparamref name="T"/>, if one has been created.
		/// </summary>
		/// <remarks>
		/// This property is left here from the legacy implementation that clients still use
		/// in their own apps. The standard <see cref="EmergencyLoggingServiceBase"/> within
		/// PAF core will not return the emergency text logger, for example. 
		/// </remarks>
		/// <history>
		/// <author> BMC </author>
		/// <date> 26sep2013 </date>
		/// <contribution>
		/// Added DOCs.
		/// </contribution>
		/// </history>
		T EmergencyService { get; }

		/// <summary>
		/// This is the main service that may be loaded after a setup
		/// stage is complete.
		/// </summary>
		IPAFServiceDescription MainServiceDescription { get; }

		/// <summary>
		/// This is the main service that may be loaded after a setup
		/// stage is complete. This is the actual service instance.
		/// </summary>
		T MainService { get; }
	}
}