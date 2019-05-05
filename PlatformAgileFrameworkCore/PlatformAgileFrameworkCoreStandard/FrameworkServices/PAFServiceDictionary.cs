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


#region Using Directives

using System.Collections.Generic;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.FrameworkServices.Exceptions;
#region Exception Shorthand
using PAFSED = PlatformAgileFramework.FrameworkServices.Exceptions.PAFServiceExceptionData;
using IPAFSED = PlatformAgileFramework.FrameworkServices.Exceptions.IPAFServiceExceptionData;
#endregion // Exception Shorthand
#endregion // Using Directives

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// This is an implementation of <see cref="IPAFServiceDictionary"/> that
	/// is primarily used by service managers.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 20jun2013 </date>
	/// <description>
	/// New.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Not thread-safe. Must be locked externally.
	/// </threadsafety>
// ReSharper disable once PartialTypeWithSinglePart
// core
	// ReSharper disable once InconsistentNaming
	public partial class PAFGenericServiceDictionary
		:Dictionary<IPAFNamedAndTypedObject, IDictionary<IPAFNamedAndTypedObject,
		IPAFServiceDescription>>, IPAFServiceDictionary
	{
		#region Constructors
		/// <summary>
		/// Constructor installs our <see cref="PAFServiceTypeComparer"/> to sort our
		/// inner dictionaries by type.
		/// </summary>
		public PAFGenericServiceDictionary()
			: base(new PAFServiceTypeComparer()) { } 
		#endregion // Constructors
		#region IPAFServiceDictionary Implementation
		#region Methods
		/// <summary>
		/// Default just returns a standard dictionary. It is, however, specialized
		/// by the installation of our custom comparer, which compares items
		/// by name and default flag.
		/// </summary>
		/// <returns>
		/// Empty dictionary.
		/// </returns>
		public static IDictionary<IPAFNamedAndTypedObject, IPAFServiceDescription>
			NewInnerDictionary()
		{
			return new Dictionary<IPAFNamedAndTypedObject, IPAFServiceDescription>
				(new PAFServiceInstanceComparer());
		}
		/// <remarks>
		/// See <see cref="IPAFServiceDictionary"/>
		/// </remarks>
		public void AddService(IPAFServiceDescription serviceDescription)
		{
			IDictionary<IPAFNamedAndTypedObject, IPAFServiceDescription> nameDictionary;
			if (ContainsKey(serviceDescription))
			{
				nameDictionary = this[serviceDescription];
			}
			else
			{
				nameDictionary = NewInnerDictionary();
				Add(serviceDescription, nameDictionary);
			}
			if (nameDictionary.ContainsKey(serviceDescription))
			{
				throw new PAFStandardException<IPAFSED>(new PAFSED(serviceDescription),
					PAFServiceExceptionMessageTags.DUPLICATE_SERVICE);
			}

			// Recall that service is it's own key.
			nameDictionary.Add(serviceDescription, serviceDescription);
		}
		/// <remarks>
		/// See <see cref="IPAFServiceDictionary"/>
		/// </remarks>
		public IPAFServiceDescription GetService(IPAFNamedAndTypedObject nto,
			bool exactInterfaceTypeMatch)
		{
			return this.TryLocateService(nto, exactInterfaceTypeMatch);
		}
		/// <remarks>
		/// See <see cref="IPAFServiceDictionary"/>
		/// </remarks>
		public IEnumerable<IPAFServiceDescription> GetAllServices()
		{
			return this.EnumerateAllServices();
		}
		#endregion // Methods
		#endregion // IPAFServiceDictionary Implementation
	}
}
