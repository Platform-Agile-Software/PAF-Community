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
using PlatformAgileFramework.Collections.Enumerators;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.FrameworkServices.Exceptions
{
	/// <summary>
	///	Implementation of <see cref="IPAFServicesExceptionData"/>.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 11oct2014 </date>
	/// New.
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Safe after locking.
	/// </threadsafety>
	[PAFSerializable]
	public class PAFServicesExceptionDataBase : PAFAbstractStandardExceptionDataBase,
		IPAFServicesExceptionData
	{
		#region Fields and Autoproperties
		/// <summary>
		/// The services.
		/// </summary>
		protected internal IPAFSealableEnumerable<IPAFServiceDescription>
			m_ServiceItems;
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Constructor builds with the standard arguments plus exceptions.
		/// </summary>
		/// <param name="services">
		/// Loads <see cref="Services"/>. May be <see langword="null"/>. If this
		/// enumeration is not <see langword="null"/> and terminates with a
		/// <see langword="null"/> service, no more services may be added with the
		/// <see cref="AddService"/> method. The terminating <see langword="null"/>
		///  is not added to the list.
		/// </param>
		/// <param name="extensionData">
		/// Sets <see cref="IPAFStandardExceptionData.ExtensionData"/>.
		/// </param>
		/// <param name="pafLoggingLevel">
		/// See <see cref="PAFAbstractStandardExceptionDataBase"/>
		/// </param>
		/// <param name="isFatal">
		/// See <see cref="PAFAbstractStandardExceptionDataBase"/>
		/// </param>
		public PAFServicesExceptionDataBase(IEnumerable<IPAFServiceDescription> services = null,
			object extensionData = null, PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null )
			:base(extensionData, pafLoggingLevel, isFatal)
		{
			m_ServiceItems
				= new PAFSealableEnumerable<IPAFServiceDescription>(services);
		}

		#endregion Constructors
		#region Properties
		/// <summary>
		/// <see cref="IPAFServicesExceptionData"/>
		/// </summary>
		public virtual IEnumerable<IPAFServiceDescription> Services
		{
			get { return m_ServiceItems.Items; }
		}
		#endregion // Properties
		#region  Methods
		/// <summary>
		/// <see cref="IPAFServicesExceptionData"/>
		/// </summary>
		/// <param name="service">
		/// <see cref="IPAFServicesExceptionData"/>
		/// </param>
		public virtual void AddService(IPAFServiceDescription service)
		{
			m_ServiceItems.AddItem(service);
		}
		#endregion // Methods
	}
}