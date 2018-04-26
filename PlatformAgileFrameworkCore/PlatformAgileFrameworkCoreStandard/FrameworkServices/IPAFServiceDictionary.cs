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
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.Collections.Dictionaries;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.FrameworkServices.Exceptions;

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// <para>
	/// This interface provides storage for typed services that may or may not
	/// be instantiated. The dictionary exploits the fact that services are
	/// also named and typed objects. The inner dictionary is constrained to
	/// have elements with "default" identification for identifying a specific
	/// service implementation as the "default" for an interface. The inner
	/// dictionary uses service name and the "default" indicator to sort/access
	/// and the outer uses interface type.
	/// </para>
	/// <para>
	/// We place novel methods and properties on this interface so as not to
	/// restrict access to functionality on the underlying dictionaries. This
	/// gives developers a great deal of flexibility in using this type.
	/// </para>
	/// </summary>
	/// <remarks>
	/// Don't need to subclass dictionaries, really. Elements have necessary
	/// info for doing everything with custom comparers. This dictionary was
	/// factored out of original implementation to exclude lifetime management
	/// machinery for non-singleton services.
	/// </remarks>
	/// <threadsafety>
	/// Implementations do not necessarily have to be thread-safe. Implementations
	/// may be designed to be used inside a lock. Document, please!!!!!
	/// </threadsafety>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 20jun2013 </date>
	/// <description>
	/// New - developed two-level sort for speed. This makes it easier for clients
	/// to fetch a service fresh each time it's needed, rather than storing a
	/// reference because of efficiency concerns. Profiling has revealed this
	/// to be a problem in the old dictionary.
	/// </description>
	/// </contribution>
	/// </history>
// ReSharper disable once PartialTypeWithSinglePart
	// ReSharper disable once InconsistentNaming
	public partial interface IPAFServiceDictionary
		: IPAFTwoLevelDictionary<IPAFNamedAndTypedObject,
		IPAFNamedAndTypedObject, IPAFServiceDescription>
	{
		/// <summary>
		/// Normally, this method will partition the incoming items into separate
		/// dictionaries, based on the key.
		/// </summary>
		/// <param name="serviceDescription">An instantiated service.</param>
		/// <exceptions>
		/// Normal exceptions will propagate up from dictionaries concerning duplicates.
		/// These are not caught. No exceptions need be caught or thrown from the
		/// implementation.
		/// </exceptions>
		void AddService(IPAFServiceDescription serviceDescription);

		/// <summary>
		/// This method looks for a service throughout the inner dictionaries.
		/// </summary>
		/// <param name="nto">nto key to look for.</param>
		/// <param name="exactInterfaceTypeMatch">
		/// If <see langword="false"/>, derived interfaces will be included in
		/// the search, if the dictionary does not have an exact match.
		/// </param>
		/// <returns>Located service or <see langword="null"/>.</returns>
		/// <exceptions>
		/// <exception> <see cref="PAFStandardException{T}"/>
		/// <see cref="PAFServiceExceptionDataBase.SERVICE_NOT_FOUND"/>.
		/// This exception will wrap any infrastructure exceptions as
		/// inner exceptions.
		/// </exception>
		/// No exceptions caught or thrown.
		/// </exceptions>
		IPAFServiceDescription GetService(IPAFNamedAndTypedObject nto,
			bool exactInterfaceTypeMatch);
		/// <summary>
		/// Gets all services from all of the dictionaries, lined up in their
		/// sort order.
		/// </summary>
		/// <returns>Never <see langword="null"/>.</returns>
		/// <exceptions>
		/// No exceptions thrown or caught.
		/// </exceptions>
		IEnumerable<IPAFServiceDescription> GetAllServices();

	}
}
